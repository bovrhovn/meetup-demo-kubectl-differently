using System.Collections.Generic;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Kubectl.Web.Interfaces;
using Microsoft.Extensions.Logging;

namespace Kubectl.Web.Services
{
    public class AKSObjectsService : IKubernetesObjects
    {
        private readonly IKubernetesService client;
        private readonly ILogger<AKSObjectsService> logger;

        public AKSObjectsService(IKubernetesService client, ILogger<AKSObjectsService> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task<IEnumerable<V1Namespace>> ListNamespacesAsync()
        {
            logger.LogInformation("Getting cluster information - client - ListNamespacesAsync");
            var kubernetes = await client.LoadBasedOnConfigurationAsync();
            logger.LogInformation("Listining namespaces");
            return kubernetes.ListNamespace().Items;
        }

        public async Task<IEnumerable<V1Pod>> ListPodsAsync(string namespaceName = "default")
        {
            logger.LogInformation("Getting cluster information - client - ListPodsAsync");
            var kubernetes = await client.LoadBasedOnConfigurationAsync();
            logger.LogInformation("Listining pods");
            var list = await kubernetes.ListNamespacedPodAsync(namespaceName);
            return list.Items;
        }
    }
}