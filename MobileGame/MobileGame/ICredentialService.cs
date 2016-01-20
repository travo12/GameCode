namespace MobileGame
{
    public interface ICredentialsService
    {
        string UserName { get; }

        string CookieString { get; }

        void SaveCredentials(string userName, string cookiestring);

        void DeleteCredentials();

        bool DoCredentialsExist();
    }
}