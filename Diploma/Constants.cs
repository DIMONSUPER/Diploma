namespace Diploma
{
    public static class Constants
    {
        public const string DATABASE_NAME = "Diploma.db3";
        public const string BASE_URL = "http://217.24.171.225:1337/api";

        public static class PageConstants
        {
            public const string MainTabbedPage = nameof(MainTabbedPage);
            public const string HomePage = nameof(HomePage);
            public const string SearchPage = nameof(SearchPage);
            public const string NotificationsPage = nameof(NotificationsPage);
            public const string ProfilePage = nameof(ProfilePage);
        }

        public static class LanguageConstansts
        {
            public const string English = nameof(English);
            public const string Russian = "Русский";
            public const string Ukrainian = "Українська";
        }
    }
}
