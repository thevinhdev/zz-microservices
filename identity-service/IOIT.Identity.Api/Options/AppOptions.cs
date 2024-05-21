namespace IOIT.Identity.Api.Options
{
    public class AppOptions
    {
        public ShowLogLevel ShowLogLevel { get; set; }
        public string SiteURL { get; set; }
        public string EmailConfirmationExpiryTime { get; set; }
    }

    public enum ShowLogLevel
    {
        Default = 0,
        Production = 1,
        Stacktrace = 2
    }
}
