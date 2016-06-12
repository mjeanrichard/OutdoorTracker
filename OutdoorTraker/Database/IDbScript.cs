using System.Threading.Tasks;

namespace OutdoorTraker.Database
{
	public interface IDbScript
	{
		Task Execute(OutdoorTrakerContext context);
	}
}