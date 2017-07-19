using Android.App;
using Android.Widget;
using Android.OS;
using Jarvis.Core.Services;
using Xamarin.Auth;
using Android.Provider;
using Android.Content;
using System.Threading.Tasks;
using System;
using Jarvis.Helpers;

namespace Jarvis
{
    [Activity(Label = "Jarvis", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        private IdentityManager _identityManager;
        private DefaultAccountManager _accountManager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _identityManager = new IdentityManager();
            _accountManager = new DefaultAccountManager(this);

            var loginButton = FindViewById<Button>(Resource.Id.LoginButton);
            loginButton.Click += delegate {
                LoginAlphaFlight(true);
            };

            var overlayButton = FindViewById<Button>(Resource.Id.OverlayButton);
            overlayButton.Click += delegate
            {
                StartOverlay();
            };
        }

        protected override void OnResume()
        {
            base.OnResume();
            UpdateUIWithUser();
        }

        private void UpdateUIWithUser()
        {
            var textView = FindViewById<TextView>(Resource.Id.TextView);
            if (_accountManager.AccountExists())
            {
                var user = _accountManager.GetUser();
                textView.Text = string.Format("Logged in as {0}", user.Name);
            }
            else
            {
                textView.Text = "Please login";
            }
        }

        private void LoginAlphaFlight(bool allowCancel)
        {
            var auth = new OAuth2Authenticator(
                clientId: "19617395140.200239281335",
                scope: "identity.basic,identity.email,identity.team,identity.avatar",
                authorizeUrl: new Uri("https://slack.com/oauth/authorize"),
                redirectUrl: new Uri("http://projectr.ca/slack/oauth/web/SlackRedirectUrl.php")
                );
            auth.AccessTokenUrl = new Uri("http://projectr.ca/slack/oauth/web/GetAccessToken.php");
            auth.AllowCancel = allowCancel;

            auth.Completed += (s, ee) => {
                if (!ee.IsAuthenticated)
                {
                    var builder = new AlertDialog.Builder(this);
                    builder.SetMessage("Not Authenticated");
                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                    return;
                }

                _accountManager.SaveAccount(ee.Account);
                GetUserName(ee.Account);
            };

            var intent = auth.GetUI(this);
            StartActivity(intent);
        }

        private void GetUserName(Account account)
        {
            _identityManager.GetIdentityAsync(account.Properties["access_token"]).ContinueWith(t => {
                var builder = new AlertDialog.Builder(this);
                if (t.IsFaulted)
                {
                    builder.SetTitle("Error");
                    builder.SetMessage(t.Exception.Flatten().InnerException.ToString());
                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                }
                else if (t.IsCanceled)
                {
                    builder.SetTitle("Task Canceled");
                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                }
                else
                {
                    var result = t.Result;
                    if (result.IsAuthenticated)
                    {
                        _accountManager.SaveUser(result.User);
                        UpdateUIWithUser();
                    }
                }
            }, UIScheduler);
        }

        private void StartOverlay()
        {
            if (!Settings.CanDrawOverlays(this))
            {
                Toast.MakeText(this, "Overlay permissions have not been granted", ToastLength.Short).Show();
                return;
            }
            var intent = new Intent(this, typeof(OverlayService));
            StartService(intent);
            Finish();
        }
    }
}

