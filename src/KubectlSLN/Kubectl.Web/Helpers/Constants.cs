namespace Kubectl.Web.Helpers
{
    public static class Constants
    {
        public const string SUCCESS = "Success";
        public const string FAILURE = "Failure";
        public const string StoreImageName = "ceecsa.azurecr.io/ew/web:latest";
        public const string DefaultServiceType = ServiceTypeLoadBalancer;
        public const string ServiceTypeLoadBalancer = "LoadBalancer";
        public const string ServiceTypeClusterIp = "ClusterIp";
        public const string ServiceTypeNone = "None";
    }
}