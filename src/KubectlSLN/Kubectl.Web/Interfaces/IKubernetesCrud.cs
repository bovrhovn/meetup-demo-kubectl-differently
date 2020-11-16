using System.Threading.Tasks;

namespace Kubectl.Web.Interfaces
{
    public interface IKubernetesCrud
    {
        Task<bool> CreateNamespaceAsync(string name);
        Task<bool> CreatePodAsync(string namespaceName,string podname, string image);
    }
}