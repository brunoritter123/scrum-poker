namespace ScrumPoker.API.Configurations
{
    public class TokenConfig
    {
        public int Expiration { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string JwtSecretKey { get; set; }
    }
}
