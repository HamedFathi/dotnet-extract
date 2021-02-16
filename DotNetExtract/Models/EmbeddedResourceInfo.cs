namespace DotNetExtract.Models
{
    public class EmbeddedResourceInfo
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string FullName { get; set; }
        public string[] Folders { get; set; }
        public byte[] Stream { get; set; }
    }
}
