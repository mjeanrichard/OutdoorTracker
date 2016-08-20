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
	[DbScript("001_AddTracks")]
	public class AddTracks : SqlBasedScript
	{
		protected override IEnumerable<string> GetSql()
		{
			yield return @"CREATE TABLE [Tracks] (
[Id] INTEGER PRIMARY KEY AUTOINCREMENT,
[Name] TEXT NULL);";

			yield return @"CREATE TABLE [TrackPoints] (
[Id] INTEGER PRIMARY KEY AUTOINCREMENT,
[TrackId] INTEGER NOT NULL,
[Number] INTEGER NOT NULL,
[Latitude] REAL NOT NULL,
[Longitude] REAL NOT NULL,
[Altitude] REAL NULL,
[Time] TEXT NOT NULL,
FOREIGN KEY ([TrackId]) REFERENCES [Tracks] ([Id]));";
		}
	}
}