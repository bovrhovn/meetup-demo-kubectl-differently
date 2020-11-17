using System;
using System.Threading.Tasks;
using k8s;
using Kubectl.Web.Interfaces;
using Kubectl.Web.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kubectl.Web.Services
{
    public class AksService : IKubernetesService
    {
        private readonly IStorageWorker storageWorker;
        private readonly ILogger<AksService> logger;
        private readonly KubekOptions kubekOptions;
        
        public AksService(IOptions<KubekOptions> kubekOptionsValue, 
            IStorageWorker storageWorker, ILogger<AksService> logger)
        {
            this.storageWorker = storageWorker;
            this.logger = logger;
            kubekOptions = kubekOptionsValue.Value;
        }

        public async Task<string> GetClusterNameAsync()
        {
            try
            {
                logger.LogInformation("Getting cluster information - client - GetClusterNameAsync");
                var stream = await storageWorker.DownloadFileAsync(kubekOptions.ConfigFileName);
                var config =  KubernetesClientConfiguration.BuildConfigFromConfigFile(stream);
                logger.LogInformation("Getting host name from config file");
                return config.Host;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return string.Empty;
        }

        public async Task<Kubernetes> LoadBasedOnConfigurationAsync()
        {
            try
            {
                logger.LogInformation("Getting config file from Azure Storage");
                var stream = await storageWorker.DownloadFileAsync(kubekOptions.ConfigFileName);
                logger.LogInformation("Getting cluster information - client - LoadBasedOnConfigurationAsync");
                var config =  KubernetesClientConfiguration.BuildConfigFromConfigFile(stream);
                var client = new Kubernetes(config);
                return client;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return null;
        }
    }
}