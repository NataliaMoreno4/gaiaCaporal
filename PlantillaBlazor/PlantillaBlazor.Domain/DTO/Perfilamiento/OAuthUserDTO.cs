namespace PlantillaBlazor.Domain.DTO.Perfilamiento
{
    public class OAuthUserDTO
    {
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? IpAddress { get; set; }
        public string? Host { get; set; }
        public string? Email { get; set; }
        public IEnumerable<string> Grupos { get; set; }
    }
}
