using System;

namespace Kubectl.Web.Models
{
    public class NamespaceViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string Labels { get; set; }
    }
}