namespace IOIT.Identity.Application.Common
{
    public static class Constants
    {
        public const string AppConnectionStringName = "DbConnection";

        public static class StatusCodeResApi
        {
            public const int Success200 = 200;
            public const int Success201 = 201;
            public const int Error400 = 400;
            public const int Error401 = 401;
            public const int Error404 = 404;
            public const int Error422 = 422;
            public const int Error500 = 500;
        }

        public static class EventBusConstants
        {
            public const string UpdateUserQueue = "Update.user.queue";
        }
    }
}
