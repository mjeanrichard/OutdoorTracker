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

using Microsoft.Practices.Unity;

using OutdoorTracker.Database;

namespace OutdoorTracker.Common
{
	public class UnitOfWorkFactoy
	{
		private readonly IUnityContainer _container;

		public UnitOfWorkFactoy(IUnityContainer container)
		{
			_container = container;
		}

		public IUnitOfWork Create()
		{
			return _container.Resolve<IUnitOfWork>("TransientUnitOfWork");
		}
	}
}