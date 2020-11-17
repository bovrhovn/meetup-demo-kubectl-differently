using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Kubectl.Web.Interfaces;
using Microsoft.Extensions.Logging;

namespace Kubectl.Web.Services
{
    public class AKSCrudService : IKubernetesCrud
    {
        private readonly IKubernetesService client;
        private readonly ILogger<AKSCrudService> logger;

        public AKSCrudService(IKubernetesService client, ILogger<AKSCrudService> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task<bool> CreateNamespaceAsync(string name)
        {
            logger.LogInformation("Getting cluster information - client - CreateNamespaceAsync");
            var kubernetes = await client.LoadBasedOnConfigurationAsync();
            logger.LogInformation("Creating namespace");

            var ns = new V1Namespace
            {
                Metadata = new V1ObjectMeta {Name = name}
            };

            try
            {
                var result = kubernetes.CreateNamespace(ns);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return false;
        }

        public async Task<bool> CreatePodAsync(string namespaceName, string podname, string image)
        {
            logger.LogInformation("Getting cluster information - client - CreateNamespaceAsync");
            var kubernetes = await client.LoadBasedOnConfigurationAsync();
            logger.LogInformation("Creating namespace");

            var pod = new V1Pod
            {
                Metadata = new V1ObjectMeta {Name = podname},
                Spec = new V1PodSpec
                {
                    Containers = new List<V1Container>
                    {
                        new V1Container
                        {
                            Image = image,
                            Name = $"image-{podname}"
                        }
                    }
                }
            };

            try
            {
                await kubernetes.CreateNamespacedPodAsync(pod, namespaceName);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return false;
            }

            return true;
        }
    }
}