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
    public class PodForNamespacePageModel : PageModel
    {
        private readonly IKubernetesObjects kubernetesObjects;

        public PodForNamespacePageModel(IKubernetesObjects kubernetesObjects) => 
            this.kubernetesObjects = kubernetesObjects;

        public async Task OnGetAsync(string name)
        {
            Name = name;
            var pods = await kubernetesObjects.ListPodsAsync(name);
            foreach (var pod in pods)
            {
                string labels = string.Empty;
                if (pod.Metadata.Labels != null)
                {
                    foreach (var labelPair in pod.Metadata.Labels)
                    {
                        labels += $" {labelPair.Key}:{labelPair.Value} ";
                    }
                }

                PodViewModels.Add(new PodViewModel
                {
                    Name = pod.Metadata.Name,
                    Labels = labels
                });
            }
        }
        
        [BindProperty(SupportsGet = true)] public string Name { get; set; }
        [BindProperty]
        public List<PodViewModel> PodViewModels { get; set; } = new List<PodViewModel>();
    }
}