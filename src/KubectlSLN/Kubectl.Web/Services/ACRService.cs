using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Kubectl.Web.Interfaces;
using Kubectl.Web.Models;
using Kubectl.Web.Options;
using Microsoft.Azure.Management.ContainerRegistry.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kubectl.Web.Services
{
    public class ACRService : IContainerRegistryService
    {
        private readonly ILogger<ACRService> logger;
        private readonly IAzure azure;
        private readonly AzureAdOptions azureAdOptions;

        public ACRService(IOptions<AzureAdOptions> azureAdOptionsValue, ILogger<ACRService> logger)
        {
            this.logger = logger;
            azureAdOptions = azureAdOptionsValue.Value;

            var credentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(azureAdOptions.ClientId, azureAdOptions.ClientSecret,
                    azureAdOptions.TenantId, AzureEnvironment.AzureGlobalCloud);

            azure = Microsoft.Azure.Management.Fluent.Azure
                .Configure()
                .Authenticate(credentials)
                .WithSubscription(azureAdOptions.SubscriptionId);
        }

        public async Task<IRegistry> GetRegistryRepositoriesAsync(string containerRegistryName)
        {
            logger.LogInformation("Retrieving info about registry");
            var registry =
                await azure.ContainerRegistries.GetByResourceGroupAsync(azureAdOptions.AcrRG,
                    containerRegistryName);
            logger.LogInformation("Registry info retrieved!");
            return registry;
        }

        public async Task<List<DockerImageViewModel>> GetImagesForRepositoryAsync(string containerRegistryName)
        {
            var list = new List<DockerImageViewModel>();
            logger.LogInformation("Getting docker client to call VM to get images");
            
            try
            {
                using var client = new DockerClientConfiguration(new Uri(azureAdOptions.DockerHostUrl))
                    .CreateClient();
                var listImages = await client.Images.ListImagesAsync(
                    new ImagesListParameters {All = true});
                logger.LogInformation("Retrieved client, doing list images");
                foreach (var img in listImages)
                {
                    if (img.RepoTags != null && img.RepoTags.Count > 0)
                    {
                        string name = img.RepoTags[0];
                        if (!name.Contains("none"))
                            list.Add(new DockerImageViewModel
                            {
                                Id = img.ID,
                                Name = name
                            });
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            logger.LogInformation("Listening images done");

            return list;
        }
    }
}