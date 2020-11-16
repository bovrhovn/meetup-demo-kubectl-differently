using System.Threading.Tasks;
using k8s;
using Kubectl.Web.Interfaces;
using Kubectl.Web.Options;
using Microsoft.Extensions.Options;

namespace Kubectl.Web.Services
{
    public class AksService : IKubernetesService
    {
        private readonly IStorageWorker storageWorker;
        private readonly KubekOptions kubekOptions;
        
        public AksService(IOptions<KubekOptions> kubekOptionsValue, 
            IStorageWorker storageWorker)
        {
            this.storageWorker = storageWorker;
            kubekOptions = kubekOptionsValue.Value;
        }

        public async Task<string> GetClusterNameAsync()
        {
            var stream = await storageWorker.DownloadFileAsync(kubekOptions.ConfigFileName);
            var config =  KubernetesClientConfiguration.BuildConfigFromConfigFile(stream);
            return config.Host;
        }

        public async Task<Kubernetes> LoadBasedOnConfigurationAsync()
        {
            var stream = await storageWorker.DownloadFileAsync(kubekOptions.ConfigFileName);
            var config =  KubernetesClientConfiguration.BuildConfigFromConfigFile(stream);
            var client = new Kubernetes(config);
            return client;
        }
    }
}