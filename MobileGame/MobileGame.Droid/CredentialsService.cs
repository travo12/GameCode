using System.Linq;
using Java.IO;
using MobileGame;
using StoreCredentials.Droid;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof (CredentialsService))]

namespace StoreCredentials.Droid
{
    public class CredentialsService : ICredentialsService
    {
        private readonly string gamename = "LeeGame";

        public string UserName
        {
            get
            {
                var account =
                    AccountStore.Create(Forms.Context)
                        .FindAccountsForService(gamename)
                        .FirstOrDefault(a => a.Username == "LeeUser");
                return account != null ? account.Properties["LeeUserName"] : null;
            }
        }

        public string CookieString
        {
            get
            {
                var account =
                    AccountStore.Create(Forms.Context)
                        .FindAccountsForService(gamename)
                        .FirstOrDefault(a => a.Username == "LeeUser");
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
                AccountStore.Create(Forms.Context).Save(account, gamename);
            }
        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create(Forms.Context).FindAccountsForService(gamename).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create(Forms.Context).Delete(account, gamename);
            }
        }


        public bool DoCredentialsExist()
        {
            try
            {
                return
                    AccountStore.Create(Forms.Context)
                        .FindAccountsForService(gamename)
                        .Any(a => a.Username == "LeeUser");
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            //if (AccountStore.Create(Forms.Context).FindAccountsForService.
            //return AccountStore.Create(Forms.Context).FindAccountsForService(gamename).Any(a => a.Username == "LeeUser") ? true : false;
        }
    }
}