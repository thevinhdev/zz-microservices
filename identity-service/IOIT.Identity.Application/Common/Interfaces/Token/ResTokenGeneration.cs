using System;

namespace IOIT.Identity.Application.Common.Interfaces.Token
{
    public class ResTokenGeneration
    {
        public string Token { get; set; }
        public DateTime ExpiredDate { get; set; }

        public ResTokenGeneration(string token, DateTime expiredDate)
        {
            Token = token;
            ExpiredDate = expiredDate;
        }
    }
}
