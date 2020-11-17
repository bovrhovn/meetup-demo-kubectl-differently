using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Spectre.Console;

namespace Kubectl.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (AnsiConsole.Capabilities.SupportLinks)
                AnsiConsole.MarkupLine(
                    $"[link=https://github.com/bovrhovn/]Demo for working with Kubernetes[/]!");

            HorizontalRule("Connecting to cluster using default load via .kube/config and list namespaces");

            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);

            var namespaces = await client.ListNamespaceAsync();

            foreach (var ns in namespaces.Items)
            {
                System.Console.WriteLine($"{ns.Metadata.Uid} : {ns.Metadata.Name}");
            }

            System.Console.Read(); //break for continue
            
            HorizontalRule("List pods in namespace - app");

            var listPodInNamespace = await client.ListNamespacedPodAsync("app");
            foreach (var currentPod in listPodInNamespace.Items)
            {
                string labels = string.Empty;
                foreach (var labelPair in currentPod.Metadata.Labels)
                {
                    labels += $" {labelPair.Key}:{labelPair.Value} ";
                }

                System.Console.WriteLine($"{currentPod.Metadata.Name} has this labels {labels}");
            }

            System.Console.Read(); //break for continue
            
            HorizontalRule("Creating namespace and pod");
            var namespaceNameForTest = "test";
            var newNamespace = new V1Namespace {Metadata = new V1ObjectMeta {Name = namespaceNameForTest}};

            var resultNamespaceCreated = await client.CreateNamespaceAsync(newNamespace);
            System.Console.WriteLine(
                $"Namespace {resultNamespaceCreated.Metadata.Name} has been created and it is in {resultNamespaceCreated.Status.Phase} state");

            var pod = new V1Pod
            {
                Metadata = new V1ObjectMeta {Name = "nginx-test"},
                Spec = new V1PodSpec
                {
                    Containers = new List<V1Container>
                    {
                        new V1Container
                        {
                            Image = "nginx:1.7.9",
                            Name = "image-nginx-test",
                            Ports = new List<V1ContainerPort>
                            {
                                new V1ContainerPort{ContainerPort = 80}
                            }
                        }
                    }
                }
            };
            
            var createdPodInNamespaceTest = await client.CreateNamespacedPodAsync(pod, namespaceNameForTest);
            System.Console.WriteLine($"Pod in namespace {namespaceNameForTest} has been created with state {createdPodInNamespaceTest.Status.Phase}");

            System.Console.Read(); //break for continue
            
            HorizontalRule("Exec into pod");
            
            var webSocket =
                await client.WebSocketNamespacedPodExecAsync(pod.Metadata.Name, 
                    namespaceNameForTest, "ls", pod.Spec.Containers[0].Name);

            var demux = new StreamDemuxer(webSocket);
            demux.Start();

            var buff = new byte[4096];
            var stream = demux.GetStream(1, 1);
            await stream.ReadAsync(buff, 0, 4096);
            var str = System.Text.Encoding.Default.GetString(buff);
            System.Console.WriteLine(str); //ouput ls command
            
            System.Console.Read(); //break for continue
            
            HorizontalRule($"Delete namespace {namespaceNameForTest}");
            
            var status = await client.DeleteNamespaceAsync(namespaceNameForTest, new V1DeleteOptions());
            System.Console.WriteLine($"Namespace {namespaceNameForTest} has been deleted - status {status.Message} - {status.Status}");
            
            HorizontalRule("Create from yaml file (private registry) - deployment");

            System.Console.Read(); //break for continue
            
            var typeMap = new Dictionary<string, Type>
            {
                {"v1/Pod", typeof(V1Pod)},
                {"v1/Service", typeof(V1Service)},
                {"apps/v1/Deployment", typeof(V1Deployment)}
            };

            string yamlPath = Path.Join(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location),"my.yaml");
            var objects = await Yaml.LoadAllFromFileAsync(yamlPath, typeMap);

            foreach (var obj in objects) { System.Console.WriteLine(obj); }
            
            System.Console.Read(); //stop and press key to continue
            
            HorizontalRule("Watching pods - watch pods");
           
            var podlistResp = client.ListNamespacedPodWithHttpMessagesAsync("default", watch: true);
            using (podlistResp.Watch<V1Pod, V1PodList>((type, item) =>
            {
                System.Console.WriteLine("==on watch event==");
                System.Console.WriteLine(type);
                System.Console.WriteLine(item.Metadata.Name);
                System.Console.WriteLine("==on watch event==");
            }))
            {
                System.Console.WriteLine("press ctrl + c to stop watching");

                var ctrlc = new ManualResetEventSlim(false);
                System.Console.CancelKeyPress += (sender, eventArgs) => ctrlc.Set();
                ctrlc.Wait();
            }
            
            System.Console.Read(); //press any key to continue
        }

        private static void HorizontalRule(string title)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Rule($"[white bold]{title}[/]").RuleStyle("grey").LeftAligned());
            AnsiConsole.WriteLine();
        }
    }
}