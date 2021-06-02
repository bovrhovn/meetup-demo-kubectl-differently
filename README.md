# Meetup demo: Kubectl a little bit different

Sample code and presentation for Kubernetes virtual meetup in Slovenia about kubectl alternative client libraries (available [here](https://github.com/kubernetes-client)) and how to work with them.

## DEMO explanation

Solution has 3 projects:

![solution info](https://webeudatastorage.blob.core.windows.net/web/meetup-solution-info.png)

Each of them connects to Kubernetes APIs (in a different way):
1. **[Kubectl.Console](https://github.com/bovrhovn/meetup-demo-kubectl-differently/tree/main/src/KubectlSLN/Kubectl.Console)** - example how to use different [C# managed library](https://github.com/kubernetes-client/csharp) API
![one of the results](https://webeudatastorage.blob.core.windows.net/web/meetup-list-pods-managed.png)

2. **[Kubectl.Rest](https://github.com/bovrhovn/meetup-demo-kubectl-differently/tree/main/src/KubectlSLN/Kubectl.Rest)** - example how to do a call from managed library using plain REST calls
![name of pods](https://webeudatastorage.blob.core.windows.net/web/meetup-pod-name-rest.png)

3. **[Kubectl.Web](https://github.com/bovrhovn/meetup-demo-kubectl-differently/tree/main/src/KubectlSLN/Kubectl.Web)** - example how to integrate calls to API's into [ASP.NET](https://asp.net) and taking advantage of managed libraries
![web dashboard](https://webeudatastorage.blob.core.windows.net/web/meetup-web-view.png)


## DEMO setup instructions

In order to run the application, you will need to have [.NET](https://dot.net) installed. I do recommend having fully pledged IDE (f.e. [Visual Studio](https://www.visualstudio.com), [JetBrains Rider](https://www.jetbrains.com/rider/)) or [Visual Studio Code](https://code.visualstudio.com) with [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) to check and navigate the source code.

You will need to have working Kubernetes cluster. Minikube is enough for playing, but to get the full experience (external load balancer), working cluster with external endpoint is preffered. 

### Settings

1. **Kubectl.Console** needs [kubeconfig file]((https://kubernetes.io/docs/concepts/configuration/organize-cluster-access-kubeconfig)) in order to run the application. On Linux check **.kube** hidden folder in home folder (on by default).

If you have [kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/) installed, use `kubectl config view` to see the file.

![config view](https://webeudatastorage.blob.core.windows.net/web/meetup-config-view.png)

Solution will automatically load the default config file and authenticate against Kubernetes cluster.

2. **Kubectl.Rest** needs [bootstrap token](https://kubernetes.io/docs/reference/access-authn-authz/bootstrap-tokens/) to authenticate against API. You can use [Postman](https://www.postman.com/) or [curl](https://en.wikipedia.org/wiki/CURL) in order to issue the command. To successfully run this project, you will need to provide **BearerToken** and **ClusterBaseAddress** as [environment variables](https://en.wikipedia.org/wiki/Environment_variable).

The easiest way is to create [service account](https://kubernetes.io/docs/reference/access-authn-authz/service-accounts-admin/) and then do role binding on a cluster to define access levels. With that defined, you can then query the secret to get the token. Use this command `kubectl -n kube-system describe secret $(kubectl -n kube-system get secret | grep youraccountname | awk '{print $1}')`.

3. **Kubectl.Web** uses [Azure AD authentication](https://azure.com/sdk) via managed library (C#) to authenticate with AAD to access [Azure Kubernetes Service](https://docs.microsoft.com/en-us/azure/aks/). I am using [Microsoft Identity Web authentication library](https://docs.microsoft.com/en-us/azure/active-directory/develop/microsoft-identity-web). [Flow](https://docs.microsoft.com/en-us/azure/active-directory/develop/app-sign-in-flow) is explained here.

To do it stepy by step,[follow](https://docs.microsoft.com/en-us/azure/active-directory/develop/app-objects-and-service-principals) this tutorial. When you have the service principal, you will need to fill in the [following details](https://github.com/bovrhovn/meetup-demo-kubectl-differently/blob/main/src/KubectlSLN/Kubectl.Web/appsettings.json) in configuration setting (or add environment variables):

![settings](https://webeudatastorage.blob.core.windows.net/web/meetup-web-settings.png)

You can find the data in service principal details (created earlier) and Azure AD portal details page. As part of the application, I am using [Azure Storage](https://docs.microsoft.com/en-us/azure/storage/) to store different config files (in demo only one), you will need to fill in the details about [Storage connection string](https://docs.microsoft.com/en-us/azure/storage/common/storage-configure-connection-string?toc=/azure/storage/blobs/toc.json) and container name.

If you want to get remote access to populated container images from a remote docker host (setting **DockerHostUrl**), you can follow [this tutorial here](https://docs.docker.com/engine/install/linux-postinstall/#configuring-remote-access-with-daemonjson) in order to allow TCP connectivity and provide URL (IP) to the application to show image list.

For logging purposes I use [Application Insight](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview). If you want to measure performance and see detailed logs (and many more), follow this [tutorial](https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core). You will need to provide **Instrumentation key** for app to send data and logs to AI.

## Additional information and links

1. [Kubernetes Api Client Libraries](https://github.com/kubernetes-client) and [3rd party community-maintained client libraries](https://kubernetes.io/docs/reference/using-api/client-libraries/#community-maintained-client-libraries)
2. [Kubernetes Api Overview](https://kubernetes.io/docs/reference/using-api/) and [controlling access to cluster](https://kubernetes.io/docs/concepts/security/controlling-access/)
3. [Kubeconfig view](https://kubernetes.io/docs/concepts/configuration/organize-cluster-access-kubeconfig/)
4. [Setup kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
5. [Power tools for kubectl](https://github.com/ahmetb/kubectx)
6. [Portainer](https://www.portainer.io/installation/)
7. [Azure Kubernetes Service](https://docs.microsoft.com/en-us/azure/aks/)
8. [Application Insight](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview)

# CREDITS

In this demo, we used the following 3rd party libraries and solutions:
1. [Spectre Console](https://github.com/spectresystems/spectre.console/)
2. [C# managed library for Kubernetes](https://github.com/kubernetes-client/csharp)
3. [SendGrid email sending](https://github.com/sendgrid/sendgrid-csharp)

# QUESTIONS / COMMENTS

If you have any questions, comments, open an issue and happy to answer.
