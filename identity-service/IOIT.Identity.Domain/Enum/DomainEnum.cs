using System;

namespace IOIT.Identity.Domain.Enum
{
    public class DomainEnum
    {
        [Flags]
        public enum ResourceType
        {
            FrontEnd = 1,
            BackEnd = 2
        }

        [Flags]
        public enum TokenTypes
        {
            AccessToken = 1,
            RefreshToken = 2
        }

        public enum ResidentRequestIdentifyType
        {
            NONE = 0,
            CCCD = 1,
            CMTND = 2,
            PASSPORT = 3,
        }

        public enum TypeUtilities
        {
            Services = 1,
            Utilities = 2
        }
    }
}
