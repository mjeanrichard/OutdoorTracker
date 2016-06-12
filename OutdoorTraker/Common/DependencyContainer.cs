using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.Unity;

using OutdoorTraker.Database;
using OutdoorTraker.Services;

namespace OutdoorTraker.Common
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
			unityContainer.RegisterType<IReadonlyUnitOfWork, ReadOnlyOutdoorTrakerContext>(new ContainerControlledLifetimeManager());

			// UnitOfWork
			unityContainer.RegisterType<IUnitOfWork, OutdoorTrakerContext>("TransientUnitOfWork", new TransientLifetimeManager());
			unityContainer.RegisterType<IUnitOfWork, OutdoorTrakerContext>(new HierarchicalLifetimeManager());
			unityContainer.RegisterInstance(new UnitOfWorkFactoy(unityContainer));

			_unityContainer = unityContainer;
		}

		public static UnityContainer Current
		{
			get { return _unityContainer; }
		}
	}
}