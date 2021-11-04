namespace CustomerListing.Persistence.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Customers
        {
            public const string GetAll = Base + "/customers";
            public const string Update = Base + "/customers";
            public const string Delete = Base + "/customers/{customerId}";
            public const string Get = Base + "/customers/{customerId}";
            public const string Create = Base + "/customers";
        }
    }
}
