using System.Collections.Generic;
using System.Threading.Tasks;
using k8s.Models;

namespace Kubectl.Web.Interfaces
{
    public interface IKubernetesObjects
    {
        Task<IEnumerable<V1Namespace>> ListNamespacesAsync();
        Task<IEnumerable<V1Pod>> ListPodsAsync(string namespaceName = "default");
    }
}