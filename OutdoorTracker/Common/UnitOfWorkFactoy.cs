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