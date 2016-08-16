using System.Threading.Tasks;

namespace OutdoorTracker.Database
{
	public interface IDbScript
	{
		Task Execute(OutdoorTrackerContext context);
	}
}