namespace PlantillaBlazor.Domain.Common
{
    public class ForwardedHeadersConfig
    {
        public bool Enabled { get; set; }
        public List<string> KnownProxies { get; set; } = new();
    }
}
