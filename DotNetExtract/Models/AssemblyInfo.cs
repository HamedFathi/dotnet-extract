using System.Collections.Generic;

namespace DotNetExtract.Models
{
    public class AssemblyInfo
    {
        public string Hash { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public IEnumerable<EmbeddedResourceInfo> EmbeddedResources { get; set; }
        public AssemblyInfo()
        {
            EmbeddedResources = new List<EmbeddedResourceInfo>();
        }
    }
}
