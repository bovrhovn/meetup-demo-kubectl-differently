using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Kubectl.Web.Helpers;
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

        public async Task<string> CreateScenarioAsync(string name)
        {
            logger.LogInformation("Getting cluster information - client - CreateNamespaceAsync");
            var kubernetes = await client.LoadBasedOnConfigurationAsync();
            logger.LogInformation("Creating namespace");
            string namespaceName = $"{name}-test";
            
            try
            {
                logger.LogInformation("Starting to create namespace");
                //1. create namespace trial-name
                await CreateNamespaceAsync(namespaceName);

                logger.LogInformation("Deploying deployment");
                //2. create deployment and service with load balancer)
                await kubernetes.CreateNamespacedDeploymentAsync(new V1Deployment
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = $"{name}-deployment"
                    },
                    Kind = "Deployment",
                    ApiVersion = "apps/v1",
                    Spec = new V1DeploymentSpec
                    {
                        Selector = new V1LabelSelector
                        {
                            MatchLabels = new Dictionary<string, string>
                            {
                                {"name", $"{name}-label"}
                            }
                        },
                        Replicas = 1,
                        Template = new V1PodTemplateSpec
                        {
                            Metadata = new V1ObjectMeta
                            {
                                Labels = new Dictionary<string, string>
                                {
                                    {"name", $"{name}-label"}
                                }
                            },
                            Spec = new V1PodSpec
                            {
                                Containers = new List<V1Container>
                                {
                                    new V1Container
                                    {
                                        Name = $"{name}-image",
                                        Image = Constants.StoreImageName
                                    }
                                }
                            }
                        }
                    }
                }, namespaceName);

                logger.LogInformation($"Creating service in a namespace {namespaceName}");
                
                //3. create service
                var createdService = await kubernetes.CreateNamespacedServiceAsync(new V1Service()
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = $"{name}-service"
                    },
                    Spec = new V1ServiceSpec
                    {
                        Selector = new Dictionary<string, string>
                        {
                            {"name", $"{name}-label"}
                        },
                        Type = Constants.ServiceTypeLoadBalancer
                    }
                }, namespaceName);
                
                logger.LogInformation("Getting IP from newly created service");
                
                //4. get IP (of course here you can map it to external )
                return createdService.Spec.LoadBalancerIP;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return string.Empty;
        }

        public async Task<bool> DeleteScenarioAsync(string name)
        {
            logger.LogInformation("Getting cluster information - client - CreateNamespaceAsync");
            var kubernetes = await client.LoadBasedOnConfigurationAsync();
            logger.LogInformation("Creating namespace");

            var status = await kubernetes.DeleteNamespaceAsync($"{name}-test", new V1DeleteOptions());
            return status.Status == Constants.SUCCESS;
        }
    }
}