using System;

namespace IOIT.Identity.Application.Common.Attributes
{
    class AppCacheAttribute : Attribute
    {
        public string FixKey { get; set; }
        public AppCacheAttribute(int duration = 10)
        {
            Duration = duration;
        }
        public AppCacheAttribute(string fixKey, int duration = 10)
        {
            FixKey = fixKey;
            Duration = duration;
        }
        public int Duration { get; set; }
    }
}
