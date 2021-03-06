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

using OutdoorTracker.Database;

namespace OutdoorTracker.Migrations
{
    [DbScript("002_AddMapDefinitions")]
    public class AddMapDefinitions : SqlBasedScript
    {
        protected override IEnumerable<string> GetSql()
        {
            yield return @"CREATE TABLE [MapConfigurations] (
[Id] INTEGER PRIMARY KEY AUTOINCREMENT,
[Name] TEXT NOT NULL,
[Json] TEXT NOT NULL,
[Version] INTEGER NOT NULL);

CREATE UNIQUE INDEX [UQ_Name] ON [MapConfigurations] ([Name] ASC);
";
        }
    }
}