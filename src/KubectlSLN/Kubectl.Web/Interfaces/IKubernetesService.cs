using System.Threading.Tasks;
using k8s;

namespace Kubectl.Web.Interfaces
{
    public interface IKubernetesService
    {
        Task<string> GetClusterNameAsync();
        Task<Kubernetes> LoadBasedOnConfigurationAsync();
    }
}