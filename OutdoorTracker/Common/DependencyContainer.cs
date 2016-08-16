using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.Unity;

using OutdoorTracker.Database;
using OutdoorTracker.Services;

namespace OutdoorTracker.Common
{
	public static class DependencyContainer
	{
		private static UnityContainer _unityContainer;

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

		public static UnityContainer Current
		{
			get { return _unityContainer; }
		}
	}
}