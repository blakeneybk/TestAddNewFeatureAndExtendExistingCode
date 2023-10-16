using System.Threading;
using System.Threading.Tasks;

namespace Litmus.Core.ServiceHost
{
    public interface IServiceHost
    {
        Task RunService(CancellationToken cancellationToken);
    }
}
