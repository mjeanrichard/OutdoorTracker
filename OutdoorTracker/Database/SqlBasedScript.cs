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

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace OutdoorTracker.Database
{
    public abstract class SqlBasedScript : IDbScript
    {
        public async Task Execute(IUnitOfWork context)
        {
            foreach (string sql in GetSql())
            {
                await context.Database.ExecuteSqlCommandAsync(sql);
            }
        }

        protected abstract IEnumerable<string> GetSql();
    }
}