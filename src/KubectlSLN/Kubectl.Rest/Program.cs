using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spectre.Console;
using Rule = Spectre.Console.Rule;

namespace Kubectl.Rest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AnsiConsole.Render(new Rule("[white bold]Loading from REST api[/]")
                .RuleStyle("grey")
                .LeftAligned());
            AnsiConsole.WriteLine();

            //bypass ssl validation check - DO NOT DO this IN PRODUCTION - trust the ssl cert
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            
            var serverUrl = Environment.GetEnvironmentVariable("ClusterBaseAddress") ?? 
                            throw new ArgumentNullException("Server address was not provided.");
            
            using var client = new HttpClient(clientHandler);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string bearerToken = Environment.GetEnvironmentVariable("BearerToken");
            var requestData = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{serverUrl}api/v1/namespaces/app/pods",UriKind.RelativeOrAbsolute)
            };
            requestData.Headers.TryAddWithoutValidation("Authorization", $"Bearer {bearerToken}");

            var result = await client.SendAsync(requestData);

            var receivedPods = await result.Content.ReadAsStringAsync();

            var pods = JsonConvert.DeserializeObject<Pods>(receivedPods);
            
            var table = new Table();
            table.Border(TableBorder.Ascii2);
            
            table.AddColumn(new TableColumn("Pod name").Centered());

            foreach (var pod in pods.Items)
            {
                table.AddRow(pod.Metadata.Name);
            }
            
            AnsiConsole.Render(table);
        }
    }
}