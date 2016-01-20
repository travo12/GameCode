using System.Linq;
using MobileGame.iOS;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof (CredentialService))]

namespace MobileGame.iOS
{
    public class CredentialService : ICredentialsService
    {
        private readonly string GameName = "LeeGame";

        public string UserName
        {
            get
            {
                var account =
                    AccountStore.Create().FindAccountsForService(GameName).FirstOrDefault(a => a.Username == "LeeUser");
                return account != null ? account.Properties["LeeUserName"] : null;
            }
        }

        public string CookieString
        {
            get
            {
                var account =
                    AccountStore.Create().FindAccountsForService(GameName).FirstOrDefault(a => a.Username == "LeeUser");
                return account != null ? account.Properties["CookieString"] : null;
            }
        }

        public void SaveCredentials(string userName, string cookiestring)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(cookiestring))
            {
                var account = new Account
                {
                    Username = "LeeUser"
                };
                account.Properties.Add("CookieString", cookiestring);
                account.Properties.Add("LeeUserName", userName);
                AccountStore.Create().Save(account, GameName);
            }
        }

        public void DeleteCredentials()
        {
            var account =
                AccountStore.Create().FindAccountsForService(GameName).FirstOrDefault(a => a.Username == "LeeUser");
            if (account != null)
            {
                AccountStore.Create().Delete(account, GameName);
            }
        }

        public bool DoCredentialsExist()
        {
            return AccountStore.Create().FindAccountsForService(GameName).Any(a => a.Username == "LeeUser");
        }
    }
}