using System;
using System.Collections.Generic;
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
            var namespaces = await kubernetesObjects.ListNamespacesAsync();
            foreach (var currentNamespace in namespaces)
            {
                string labels = string.Empty;
                foreach (var keyValuePair in currentNamespace.Metadata.Labels)
                {
                    labels += $"{keyValuePair.Key}:{keyValuePair.Value} ";
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

        public async Task<IActionResult> OnGetPods()
        {
            var pods = await kubernetesObjects.ListPodsAsync(Name);

            string currentPods = "<ul>";
            foreach (var pod in pods)
            {
                currentPods += $"<li>{pod.Metadata.Name}</li>";
            }

            currentPods += "</ul>";

            return Content(currentPods);
        }

        [BindProperty(SupportsGet = true)] public string Name { get; set; }

        [BindProperty]
        public List<NamespaceViewModel> NamespaceViewModels { get; set; } = new List<NamespaceViewModel>();
    }
}