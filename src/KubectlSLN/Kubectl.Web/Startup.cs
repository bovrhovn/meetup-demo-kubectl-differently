using System.Net;
using Kubectl.Web.Hub;
using Kubectl.Web.Interfaces;
using Kubectl.Web.Options;
using Kubectl.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace Kubectl.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AzureAdOptions>(Configuration.GetSection("AzureAd"));
            services.Configure<StorageOptions>(Configuration.GetSection("StorageOptions"));
            services.Configure<SendGridOptions>(Configuration.GetSection("SendGridOptions"));
            services.Configure<KubekOptions>(Configuration.GetSection("KubekOptions"));
            
            services.AddSignalR();

            var sendGridSettings = Configuration.GetSection("SendGridOptions").Get<SendGridOptions>();
            services.AddScoped<IEmailService, SendGridEmailSender>(
                _ => new SendGridEmailSender(sendGridSettings.ApiKey));

            var storageSettings = Configuration.GetSection("StorageOptions").Get<StorageOptions>();
            services.AddScoped<IStorageWorker, AzureStorageWorker>(
                _ => new AzureStorageWorker(storageSettings.ConnectionString, storageSettings.Container));
            
            services.AddScoped<IKubernetesService, AksService>();
            services.AddScoped<IKubernetesObjects, AKSObjectsService>();
            services.AddScoped<IContainerRegistryService, ACRService>();
            services.AddScoped<IKubernetesCrud, AKSCrudService>();
            
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddHttpContextAccessor();
            
            services.AddApplicationInsightsTelemetry();
            
            services.AddMicrosoftIdentityWebAppAuthentication(Configuration);
            services.AddControllersWithViews().AddMicrosoftIdentityUI();

            services.AddRazorPages()
                .AddRazorPagesOptions(options => options.Conventions.AddPageRoute("/Info/Index", ""));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notification");
            });
        }
    }
}