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

using Microsoft.Practices.Unity;

using OutdoorTracker.Database;
using OutdoorTracker.Services;

namespace OutdoorTracker.Common
{
	public static class DependencyContainer
	{
		public static void InitializeContainer(App app)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}
			UnityContainer unityContainer = new UnityContainer();

			unityContainer.RegisterInstance(new NavigationService(app), new ContainerControlledLifetimeManager());

			// Singletons
			unityContainer.RegisterType<GeoLocationService>(new ContainerControlledLifetimeManager());
			unityContainer.RegisterType<TrackRecorder>(new ContainerControlledLifetimeManager());
			unityContainer.RegisterType<MapDefinitionManager>(new ContainerControlledLifetimeManager());
			unityContainer.RegisterType<IReadonlyUnitOfWork, ReadOnlyOutdoorTrackerContext>(new ContainerControlledLifetimeManager());

			// UnitOfWork
			unityContainer.RegisterType<IUnitOfWork, OutdoorTrackerContext>("TransientUnitOfWork", new TransientLifetimeManager());
			unityContainer.RegisterType<IUnitOfWork, OutdoorTrackerContext>(new HierarchicalLifetimeManager());
			unityContainer.RegisterInstance(new UnitOfWorkFactoy(unityContainer));

			_unityContainer = unityContainer;
		}

		private static UnityContainer _unityContainer;

		public static UnityContainer Current
		{
			get { return _unityContainer; }
		}
	}
}