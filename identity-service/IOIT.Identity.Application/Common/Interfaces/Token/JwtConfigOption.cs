namespace IOIT.Identity.Application.Common.Interfaces.Token
{
    public class JwtConfigOptions
    {
        public string SecretKey { get; set; }
        public int ExpirationInMinutes { get; set; }
        public int RefreshExpirationInMinutes { get; set; }

        public string Audience { get; set; }
        public string Issuer { get; set; }

        public bool RequireHttpsMetadata { get; set; }
        public TokenValidationParameterOptions TokenValidationParameter { get; set; }
    }
}
