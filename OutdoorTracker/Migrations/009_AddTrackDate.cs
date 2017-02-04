// 
// Outdoor Tracker - Copyright(C) 2017 Meinard Jean-Richard
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

using OutdoorTracker.Database;

namespace OutdoorTracker.Migrations
{
    [DbScript("009_AddTrackDate")]
    public class AddTrackDate : SqlBasedScript
    {
        protected override IEnumerable<string> GetSql()
        {
            yield return @"ALTER TABLE Tracks ADD [Date] DateTime NOT NULL DEFAULT '01/01/2000 00:00:00'";
        }

        public override async Task Execute(IUnitOfWork context)
        {
            await base.Execute(context);
        }
    }
}