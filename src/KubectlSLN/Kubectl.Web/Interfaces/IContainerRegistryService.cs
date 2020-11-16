using System.Collections.Generic;
using System.Threading.Tasks;
using Kubectl.Web.Models;
using Microsoft.Azure.Management.ContainerRegistry.Fluent;

namespace Kubectl.Web.Interfaces
{
    public interface IContainerRegistryService
    {
        Task<IRegistry> GetRegistryRepositoriesAsync(string containerRegistryName);
        Task<List<DockerImageViewModel>> GetImagesForRepositoryAsync(string containerRegistryName);
    }
}