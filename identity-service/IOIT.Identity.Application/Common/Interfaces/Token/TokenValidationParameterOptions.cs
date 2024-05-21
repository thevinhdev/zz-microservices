namespace IOIT.Identity.Application.Common.Interfaces.Token
{
    public class TokenValidationParameterOptions
    {
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
    }
}
