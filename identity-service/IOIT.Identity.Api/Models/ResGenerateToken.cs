using System;

namespace IOIT.Identity.Api.Models
{
    public class ResGenerateToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }

        public ResGenerateToken(string accessToken, string refreshToken, DateTime expirationDate)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpirationDate = expirationDate;
        }
    }
}
