using Xamarin.Forms;

namespace MobileGame
{
    // Uses a DependencyService to get phone specific code for local storage and retreival of authentication information
    // This is to avoid logging in every time the app is opened
    internal class CredentialStore
    {
        private readonly ICredentialsService CredientialService;

        public CredentialStore()
        {
            CredientialService = DependencyService.Get<ICredentialsService>();
        }

        public void DeleteObject()
        {
            CredientialService.DeleteCredentials();
        }

        public string GetUserName()
        {
            return CredientialService.UserName;
        }

        public string GetCookieString()
        {
            return CredientialService.CookieString;
        }

        public void InsertObject(string username, string password)
        {
            CredientialService.SaveCredentials(username, password);
        }

        public bool DoCredentialsExist()
        {
            return CredientialService.DoCredentialsExist();
        }
    }
}