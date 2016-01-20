using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MobileGame
{
    internal class LogInManager
    {
        public LogInManager()
        {
            AuthCache = new CredentialStore();
            SeenHomeScreen = false;
        }

        public CredentialStore AuthCache { get; set; }
        public ContentPageController ViewController { get; set; }
        public CookieContainer CookieJar { get; set; }
        public bool SeenHomeScreen { get; set; }

        public async Task HandleLogIn(LogInLayout LIlayout, ContentPageController viewController)
        {
            //this is used only for debugging cause the emulator likes to save credentials
            //AuthCache.DeleteObject();

            ViewController = viewController;
            if (AuthCache.DoCredentialsExist())
            {
                // need to gather credentials and pass them on
                var Username = AuthCache.GetUserName();
                var CookieString = AuthCache.GetCookieString();
                //string Password = "asklfas;ldf";

                // build the cookiecontainer from authstring
                var tempcookie = new Cookie {Name = ".AspNet.ApplicationCookie", Value = CookieString};
                var tempcollection = new CookieContainer();
                tempcollection.Add(new Uri(Constants.EndPoint), tempcookie);
                await VerifyAndPass(LIlayout, Username, tempcollection, false, viewController);
            }
            else
            {
                SeenHomeScreen = true;
                var HLayout = new HomeLayout(ViewController, false);
            }
        }
        //Handles a new Log In and Stores Credientials
        public async Task NewLogIn(LogInLayout LIlayout, string userName, string password, bool saveCredentials,
            ContentPageController viewcontroller)
        {
            var cookieJar = await LogIn(userName, password);
            if (await VerifyLogin(cookieJar))
            {
                await VerifyAndPass(LIlayout, userName, cookieJar, saveCredentials, viewcontroller);
            }
            else
            {
                if (!SeenHomeScreen)
                {
                    SeenHomeScreen = true;
                    var HLayout = new HomeLayout(ViewController, false);
                }
                else
                {
                    ViewController.Invoke(() => ViewController.View.Content = ViewController.LogIn.LILayout);
                }
            }
        }
        public async Task VerifyAndPass(LogInLayout LIlayout, string userName, CookieContainer cookieJar,
            bool saveCredentials, ContentPageController viewController)
        {
            CookieJar = cookieJar;

            // store credientials if it is a new login
            if (saveCredentials)
            {
                //Gather Cookie String
                var gameUri = new Uri(Constants.EndPoint);
                var gamecookies = CookieJar.GetCookies(gameUri);
                string CookieString = null;
                foreach (Cookie c in gamecookies)
                {
                    if (c.Name == ".AspNet.ApplicationCookie") CookieString = c.Value;
                }
                //Store Cookie
                AuthCache.InsertObject(userName, CookieString);
            }

            //just preps the Game Manager
            GameManager.SetupGame(CookieJar, ViewController);

            if (!SeenHomeScreen)
            {
                SeenHomeScreen = true;
                var HLayout = new HomeLayout(ViewController, true);
            }
            else
            {
                var GSlayout = new GameSelectLayout(ViewController);
            }
        }

        public async Task LoginFacebook(string token)
        {
            var URL = Constants.FaceBookLoginEndpoint;
            var _authString = "{token:'" + token + "'}";

            //more request parameters
            var loginrequest = (HttpWebRequest) WebRequest.Create(URL);
            loginrequest.Method = "POST";

            //stores the cookies
            var cookieJar = new CookieContainer();
            loginrequest.CookieContainer = cookieJar;

            //set the encoding and data transfer type
            var encoding = new UTF8Encoding();
            var arr = encoding.GetBytes(_authString);
            loginrequest.ContentType = "application/json";

            //write the data to the datastream
            var dataStream = await loginrequest.GetRequestStreamAsync().ConfigureAwait(false);
            dataStream.Write(arr, 0, arr.Length);

            // need proper error handling
            await loginrequest.GetResponseAsync().ConfigureAwait(false);

            var gameUri = new Uri(Constants.EndPoint);
            var gamecookies = cookieJar.GetCookies(gameUri);
            string CookieString = null;
            foreach (Cookie c in gamecookies)
            {
                if (c.Name == ".AspNet.ApplicationCookie") CookieString = c.Value;
            }
            //Store Cookie
            AuthCache.InsertObject("FaceBookUser", CookieString);
        }

        public async Task<CookieContainer> LogIn(string username, string password)
        {
            // handles the login

            // gathering some request paramaters
            var URL = Constants.LoginEndPoint;
            var _authString = "{Email:'" + username + "',Password:'" + password + "',RememberMe:false}";

            //more request parameters
            var loginrequest = (HttpWebRequest) WebRequest.Create(URL);
            loginrequest.Method = "POST";
            loginrequest.ContentType = "application/x-www-form-urlencoded";

            //stores the cookies
            var cookieJar = new CookieContainer();
            loginrequest.CookieContainer = cookieJar;

            //set the encoding and data transfer type
            var encoding = new UTF8Encoding();
            var arr = encoding.GetBytes(_authString);
            loginrequest.ContentType = "application/json";

            //write the data to the datastream
            var dataStream = await loginrequest.GetRequestStreamAsync().ConfigureAwait(false);
            dataStream.Write(arr, 0, arr.Length);

            var response = (HttpWebResponse) await loginrequest.GetResponseAsync().ConfigureAwait(false);


            return cookieJar;
        }

        public void LogOut()
        {
            AuthCache.DeleteObject();
            //ViewController.GameSelect = new GameSelectLayout();
            CookieJar = null;
            ViewController.Invoke(() => ViewController.View.Content = ViewController.LogIn.LILayout);
            GameManager.Close();
        }
        //make sure credentials are valid
        public async Task<bool> VerifyLogin(CookieContainer cookieJar)
        {
            // this pulls the authstring from the cookie
            var gameUri = new Uri(Constants.EndPoint);
            var gamecookies = cookieJar.GetCookies(gameUri);
            string CookieString = null;
            foreach (Cookie c in gamecookies)
            {
                if (c.Name == ".AspNet.ApplicationCookie") CookieString = c.Value;
            }


            // unsuccessful login
            if (CookieString == null)
            {
                ViewController.Invoke(
                    () =>
                        ViewController.DisplayAlert("Login Unsuccessful",
                            "Please check username and password and try again.", "OK").ConfigureAwait(false));

                return false;
            }
            return true;
        }
    }
}