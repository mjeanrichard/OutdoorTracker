// 
// Outdoor Tracker - Copyright(C) 2016 Meinard Jean-Richard
//  
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//  
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Practices.Unity;

using OutdoorTracker.Common;

namespace OutdoorTracker.Database
{
	public class DbInitializer
	{
		private readonly Func<OutdoorTrackerContext> _contextFactory;

		public DbInitializer(Func<OutdoorTrackerContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task InitDatabase()
		{
			using (OutdoorTrackerContext OutdoorTrackerContext = _contextFactory())
			{
				IList<DbVersion> versions = await GetLatestScriptVersion(OutdoorTrackerContext);
				await ExecuteScripts(versions, OutdoorTrackerContext);

				await OutdoorTrackerContext.SaveChangesAsync();
			}
		}

		private async Task ExecuteScripts(IEnumerable<DbVersion> executedVersions, OutdoorTrackerContext context)
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

		private async Task<IList<DbVersion>> GetLatestScriptVersion(OutdoorTrackerContext context)
		{
			await context.Database.ExecuteSqlCommandAsync("CREATE TABLE IF NOT EXISTS Version (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, CONSTRAINT UQ_Name UNIQUE (Name))");
			return await context.Set<DbVersion>().ToListAsync();
		}
	}
}