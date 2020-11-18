using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Kubectl.Web.Interfaces;
using Kubectl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Kubectl.Web.Pages.Kubek
{
    [Authorize]
    public class ListNamespacesPageModel : PageModel
    {
        private readonly IKubernetesObjects kubernetesObjects;
        private readonly ILogger<ListNamespacesPageModel> logger;

        public ListNamespacesPageModel(IKubernetesObjects kubernetesObjects, 
            ILogger<ListNamespacesPageModel> logger)
        {
            this.kubernetesObjects = kubernetesObjects;
            this.logger = logger;
        }

        public async Task OnGetAsync()
        {
            logger.LogInformation("Getting data about namespaces");
            try
            {
                var namespaces = await kubernetesObjects.ListNamespacesAsync();
                foreach (var currentNamespace in namespaces)
                {
                    string labels = string.Empty;
                    if (currentNamespace.Metadata.Labels != null)
                    {
                        foreach (var keyValuePair in currentNamespace.Metadata.Labels)
                        {
                            labels += $"{keyValuePair.Key}:{keyValuePair.Value} ";
                        }
                    }

                    NamespaceViewModels.Add(new NamespaceViewModel
                    {
                        Id = currentNamespace.Metadata.Uid,
                        Name = currentNamespace.Metadata.Name,
                        CreationDate = currentNamespace.Metadata.CreationTimestamp ?? DateTime.Now,
                        Labels = labels
                    });
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                logger.LogError(e.Message);
            }
            logger.LogInformation("Done loading namespaces");
        }

        [BindProperty(SupportsGet = true)] public string Name { get; set; }

        [BindProperty]
        public List<NamespaceViewModel> NamespaceViewModels { get; set; } = new List<NamespaceViewModel>();
    }
}