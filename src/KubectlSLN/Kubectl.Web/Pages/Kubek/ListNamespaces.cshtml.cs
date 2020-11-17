using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Kubectl.Web.Interfaces;
using Kubectl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kubectl.Web.Pages.Kubek
{
    [Authorize]
    public class ListNamespacesPageModel : PageModel
    {
        private readonly IKubernetesObjects kubernetesObjects;

        public ListNamespacesPageModel(IKubernetesObjects kubernetesObjects)
        {
            this.kubernetesObjects = kubernetesObjects;
        }

        public async Task OnGetAsync()
        {
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
            }
        }

        [BindProperty(SupportsGet = true)] public string Name { get; set; }

        [BindProperty]
        public List<NamespaceViewModel> NamespaceViewModels { get; set; } = new List<NamespaceViewModel>();
    }
}