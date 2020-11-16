using System.Threading.Tasks;
using Kubectl.Web.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kubectl.Web.Pages.Kubek
{
    [Authorize]
    public class DashboardPageModel : PageModel
    {
        private readonly IKubernetesService kubernetesService;

        public DashboardPageModel(IKubernetesService kubernetesService)
        {
            this.kubernetesService = kubernetesService;
        }

        public async Task OnGetAsync()
        {
            var name = await kubernetesService.GetClusterNameAsync();
            Name = name;
        }

        [BindProperty]
        public string Name { get; set; }
    }
}