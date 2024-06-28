namespace PizzaApi.Application.Common.Constants
{
    public static partial class Constants
    {
        public static class Account
        {
            // Policies
            public const string MangerOrDeveloper = "MangerOrDeveloper";

            //Roles
            public const string Manager = "Manager";
            public const string Developer = "Developer";

            //Schemes
            public const string BearerAndApplication = "BearerAndApplication";
            public const string Bearer = "Bearer";

            //User
            public const int MinUserNameLength = 2;
            public const int MaxUserNameLength = 30;
        }
    }
}
