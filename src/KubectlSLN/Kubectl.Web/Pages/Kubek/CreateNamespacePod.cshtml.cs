using System.Collections.Generic;
using System.Threading.Tasks;
using Kubectl.Web.Helpers;
using Kubectl.Web.Interfaces;
using Kubectl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Kubectl.Web.Pages.Kubek
{
    [Authorize]
    public class CreateNamespacePodPageModel : PageModel
    {
        private readonly IContainerRegistryService containerRegistryService;
        private readonly IKubernetesCrud kubernetesCrud;
        private readonly ILogger<CreateNamespacePodPageModel> logger;

        public CreateNamespacePodPageModel(IContainerRegistryService containerRegistryService,
            IKubernetesCrud kubernetesCrud,
            ILogger<CreateNamespacePodPageModel> logger)
        {
            this.containerRegistryService = containerRegistryService;
            this.kubernetesCrud = kubernetesCrud;
            this.logger = logger;
        }

        public async Task OnGet()
        {
            var list = await containerRegistryService.GetImagesForRepositoryAsync("ceecsa");
            Images = list;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(NamespaceName))
            {
                InfoText = "Enter name";
                return RedirectToPage("/Kubek/CreateNamespacePod");
            }

            if (!await kubernetesCrud.CreateNamespaceAsync(NamespaceName))
            {
                InfoText = "There has been an error with creating namespace in k8s, try again";
                return RedirectToPage("/Kubek/CreateNamespacePod");
            }

            var form = await Request.ReadFormAsync();
            var imageName = form["image"];
            logger.LogInformation($"Received {imageName}");
            if (!string.IsNullOrEmpty(imageName))
            {
                if (string.IsNullOrEmpty(PodName)) PodName = Utils.GenerateName(5).ToLower();

                if (!await kubernetesCrud.CreatePodAsync(NamespaceName, PodName, imageName))
                {
                    InfoText = "There has been an error in creating pod, try again";
                    return RedirectToPage("/Kubek/CreateNamespacePod");
                }
            }

            logger.LogInformation("Pod, namespace created!");
            return RedirectToPage("/Kubek/ListNamespaces");
        }


        [BindProperty] public string NamespaceName { get; set; }
        [BindProperty] public string PodName { get; set; }
        [TempData] public string InfoText { get; set; }
        [BindProperty] public List<DockerImageViewModel> Images { get; set; }
    }
}