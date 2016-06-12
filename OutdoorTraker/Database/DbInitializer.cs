using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Practices.Unity;

using OutdoorTraker.Common;

namespace OutdoorTraker.Database
{
	public class DbInitializer
	{
		private readonly Func<OutdoorTrakerContext> _contextFactory;

		public DbInitializer(Func<OutdoorTrakerContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task InitDatabase()
		{
			using (OutdoorTrakerContext outdoorTrakerContext = _contextFactory())
			{
				IList<DbVersion> versions = await GetLatestScriptVersion(outdoorTrakerContext);
				await ExecuteScripts(versions, outdoorTrakerContext);

				await outdoorTrakerContext.SaveChangesAsync();
			}
		}

		private async Task ExecuteScripts(IEnumerable<DbVersion> executedVersions, OutdoorTrakerContext context)
		{
			IEnumerable<TypeInfo> typeInfos = typeof(DbInitializer).GetTypeInfo().Assembly.DefinedTypes.Where(t => !t.IsAbstract && !t.IsInterface && t.ImplementedInterfaces.Contains(typeof(IDbScript)));
			List<KeyValuePair<string, TypeInfo>> scripts = new List<KeyValuePair<string, TypeInfo>>();
			foreach (TypeInfo typeInfo in typeInfos)
			{
				DbScriptAttribute dbScriptAttribute = typeInfo.GetCustomAttribute<DbScriptAttribute>();
				if (dbScriptAttribute != null)
				{
					scripts.Add(new KeyValuePair<string, TypeInfo>(dbScriptAttribute.Name, typeInfo));
				}
			}

			foreach (KeyValuePair<string, TypeInfo> scriptInfo in scripts.OrderBy(s => s.Key))
			{
				if (!executedVersions.Any(v => v.Name.Equals(scriptInfo.Key)))
				{
					using (IDbContextTransaction transaction = context.Database.BeginTransaction())
					{
						IDbScript script = (IDbScript)DependencyContainer.Current.Resolve(scriptInfo.Value.AsType());
						await script.Execute(context);
						context.Set<DbVersion>().Add(new DbVersion { Name = scriptInfo.Key });

						await context.SaveChangesAsync();
						transaction.Commit();
					}
				}
			}
		}

		private async Task<IList<DbVersion>> GetLatestScriptVersion(OutdoorTrakerContext context)
		{
			await context.Database.ExecuteSqlCommandAsync("CREATE TABLE IF NOT EXISTS Version (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, CONSTRAINT UQ_Name UNIQUE (Name))");
			return await context.Set<DbVersion>().ToListAsync();
		}
	}
}