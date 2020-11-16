using System.Collections.Generic;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Kubectl.Web.Interfaces;
using Kubectl.Web.Options;
using Microsoft.Extensions.Options;

namespace Kubectl.Web.Services
{
    public class AKSObjectsService : IKubernetesObjects
    {
        private readonly IKubernetesService client;
        private KubekOptions kubekOptions;

        public AKSObjectsService(IKubernetesService client, IOptions<KubekOptions> kubekOptionsValue)
        {
            kubekOptions = kubekOptionsValue.Value;
            this.client = client;
        }

        public async Task<IEnumerable<V1Namespace>> ListNamespacesAsync()
        {
            var kubernetes = await client.LoadBasedOnConfigurationAsync();
            return kubernetes.ListNamespace().Items;
        }

        public async Task<IEnumerable<V1Pod>> ListPodsAsync(string namespaceName = "default")
        {
            var kubernetes = await client.LoadBasedOnConfigurationAsync();
            var list = await kubernetes.ListNamespacedPodAsync(namespaceName);
            return list.Items;
        }
    }
}