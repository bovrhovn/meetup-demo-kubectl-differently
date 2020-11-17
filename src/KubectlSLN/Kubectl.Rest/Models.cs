using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kubectl.Rest
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Metadata
    {
        [JsonProperty("selfLink")] public string SelfLink { get; set; }

        [JsonProperty("resourceVersion")] public string ResourceVersion { get; set; }
    }

    public class Labels
    {
        [JsonProperty("ms")] public string Ms { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("pod-template-hash")] public string PodTemplateHash { get; set; }

        [JsonProperty("tier")] public string Tier { get; set; }

        [JsonProperty("controller-uid")] public string ControllerUid { get; set; }

        [JsonProperty("job-name")] public string JobName { get; set; }
    }

    public class OwnerReference
    {
        [JsonProperty("apiVersion")] public string ApiVersion { get; set; }

        [JsonProperty("kind")] public string Kind { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("uid")] public string Uid { get; set; }

        [JsonProperty("controller")] public bool Controller { get; set; }

        [JsonProperty("blockOwnerDeletion")] public bool BlockOwnerDeletion { get; set; }
    }

    public class Metadata2
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("generateName")] public string GenerateName { get; set; }

        [JsonProperty("namespace")] public string Namespace { get; set; }

        [JsonProperty("selfLink")] public string SelfLink { get; set; }

        [JsonProperty("uid")] public string Uid { get; set; }

        [JsonProperty("resourceVersion")] public string ResourceVersion { get; set; }

        [JsonProperty("creationTimestamp")] public DateTime CreationTimestamp { get; set; }

        [JsonProperty("labels")] public Labels Labels { get; set; }

        [JsonProperty("ownerReferences")] public List<OwnerReference> OwnerReferences { get; set; }
    }

    public class Secret
    {
        [JsonProperty("secretName")] public string SecretName { get; set; }

        [JsonProperty("defaultMode")] public int DefaultMode { get; set; }
    }

    public class Volume
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("secret")] public Secret Secret { get; set; }
    }

    public class Port
    {
        [JsonProperty("containerPort")] public int ContainerPort { get; set; }

        [JsonProperty("protocol")] public string Protocol { get; set; }
    }

    public class Limits
    {
        [JsonProperty("cpu")] public string Cpu { get; set; }

        [JsonProperty("memory")] public string Memory { get; set; }
    }

    public class Requests
    {
        [JsonProperty("cpu")] public string Cpu { get; set; }

        [JsonProperty("memory")] public string Memory { get; set; }
    }

    public class Resources
    {
        [JsonProperty("limits")] public Limits Limits { get; set; }

        [JsonProperty("requests")] public Requests Requests { get; set; }
    }

    public class VolumeMount
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("readOnly")] public bool ReadOnly { get; set; }

        [JsonProperty("mountPath")] public string MountPath { get; set; }
    }

    public class SecretKeyRef
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("key")] public string Key { get; set; }
    }

    public class ValueFrom
    {
        [JsonProperty("secretKeyRef")] public SecretKeyRef SecretKeyRef { get; set; }
    }

    public class Env
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("valueFrom")] public ValueFrom ValueFrom { get; set; }
    }

    public class Container
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("image")] public string Image { get; set; }

        [JsonProperty("ports")] public List<Port> Ports { get; set; }

        [JsonProperty("resources")] public Resources Resources { get; set; }

        [JsonProperty("volumeMounts")] public List<VolumeMount> VolumeMounts { get; set; }

        [JsonProperty("terminationMessagePath")]
        public string TerminationMessagePath { get; set; }

        [JsonProperty("terminationMessagePolicy")]
        public string TerminationMessagePolicy { get; set; }

        [JsonProperty("imagePullPolicy")] public string ImagePullPolicy { get; set; }

        [JsonProperty("env")] public List<Env> Env { get; set; }
    }

    public class SecurityContext
    {
    }

    public class Toleration
    {
        [JsonProperty("key")] public string Key { get; set; }

        [JsonProperty("operator")] public string Operator { get; set; }

        [JsonProperty("effect")] public string Effect { get; set; }

        [JsonProperty("tolerationSeconds")] public int TolerationSeconds { get; set; }
    }

    public class Spec
    {
        [JsonProperty("volumes")] public List<Volume> Volumes { get; set; }

        [JsonProperty("containers")] public List<Container> Containers { get; set; }

        [JsonProperty("restartPolicy")] public string RestartPolicy { get; set; }

        [JsonProperty("terminationGracePeriodSeconds")]
        public int TerminationGracePeriodSeconds { get; set; }

        [JsonProperty("dnsPolicy")] public string DnsPolicy { get; set; }

        [JsonProperty("serviceAccountName")] public string ServiceAccountName { get; set; }

        [JsonProperty("serviceAccount")] public string ServiceAccount { get; set; }

        [JsonProperty("nodeName")] public string NodeName { get; set; }

        [JsonProperty("securityContext")] public SecurityContext SecurityContext { get; set; }

        [JsonProperty("schedulerName")] public string SchedulerName { get; set; }

        [JsonProperty("tolerations")] public List<Toleration> Tolerations { get; set; }

        [JsonProperty("priority")] public int Priority { get; set; }

        [JsonProperty("enableServiceLinks")] public bool EnableServiceLinks { get; set; }
    }

    public class Condition
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("lastProbeTime")] public object LastProbeTime { get; set; }

        [JsonProperty("lastTransitionTime")] public DateTime LastTransitionTime { get; set; }

        [JsonProperty("reason")] public string Reason { get; set; }
    }

    public class PodIP
    {
        [JsonProperty("ip")] public string Ip { get; set; }
    }

    public class Running
    {
        [JsonProperty("startedAt")] public DateTime StartedAt { get; set; }
    }

    public class Terminated
    {
        [JsonProperty("exitCode")] public int ExitCode { get; set; }

        [JsonProperty("reason")] public string Reason { get; set; }

        [JsonProperty("startedAt")] public DateTime StartedAt { get; set; }

        [JsonProperty("finishedAt")] public DateTime FinishedAt { get; set; }

        [JsonProperty("containerID")] public string ContainerID { get; set; }
    }

    public class State
    {
        [JsonProperty("running")] public Running Running { get; set; }

        [JsonProperty("terminated")] public Terminated Terminated { get; set; }
    }

    public class LastState
    {
    }

    public class ContainerStatus
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("state")] public State State { get; set; }

        [JsonProperty("lastState")] public LastState LastState { get; set; }

        [JsonProperty("ready")] public bool Ready { get; set; }

        [JsonProperty("restartCount")] public int RestartCount { get; set; }

        [JsonProperty("image")] public string Image { get; set; }

        [JsonProperty("imageID")] public string ImageID { get; set; }

        [JsonProperty("containerID")] public string ContainerID { get; set; }

        [JsonProperty("started")] public bool Started { get; set; }
    }

    public class Status
    {
        [JsonProperty("phase")] public string Phase { get; set; }

        [JsonProperty("conditions")] public List<Condition> Conditions { get; set; }

        [JsonProperty("hostIP")] public string HostIP { get; set; }

        [JsonProperty("podIP")] public string PodIP { get; set; }

        [JsonProperty("podIPs")] public List<PodIP> PodIPs { get; set; }

        [JsonProperty("startTime")] public DateTime StartTime { get; set; }

        [JsonProperty("containerStatuses")] public List<ContainerStatus> ContainerStatuses { get; set; }

        [JsonProperty("qosClass")] public string QosClass { get; set; }
    }

    public class Item
    {
        [JsonProperty("metadata")] public Metadata2 Metadata { get; set; }

        [JsonProperty("spec")] public Spec Spec { get; set; }

        [JsonProperty("status")] public Status Status { get; set; }
    }

    public class Pods
    {
        [JsonProperty("kind")] public string Kind { get; set; }

        [JsonProperty("apiVersion")] public string ApiVersion { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("items")] public List<Item> Items { get; set; }
    }
}