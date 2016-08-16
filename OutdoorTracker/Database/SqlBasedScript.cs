using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace OutdoorTracker.Database
{
	public abstract class SqlBasedScript : IDbScript
	{
		public async Task Execute(OutdoorTrackerContext context)
		{
			foreach (string sql in GetSql())
			{
				await context.Database.ExecuteSqlCommandAsync(sql);
			}
		}

		protected abstract IEnumerable<string> GetSql();
	}
}