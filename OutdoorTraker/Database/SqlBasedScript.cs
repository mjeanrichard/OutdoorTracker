using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace OutdoorTraker.Database
{
	public abstract class SqlBasedScript : IDbScript
	{
		public async Task Execute(OutdoorTrakerContext context)
		{
			foreach (string sql in GetSql())
			{
				await context.Database.ExecuteSqlCommandAsync(sql);
			}
		}

		protected abstract IEnumerable<string> GetSql();
	}
}