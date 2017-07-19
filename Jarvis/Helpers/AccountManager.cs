using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;
using Android.Preferences;
using Jarvis.Core.Models;

namespace Jarvis.Helpers
{
    internal class DefaultAccountManager
    {
        private const string DefaultAccount = "AlphaFlight";
        private const string UserName = "UserName";
        private const string UserId = "UserId";
        private const string UserEmail = "UserEmail";

        private AccountStore _accountStore;
        private ISharedPreferences _sharedPreferences;

        public DefaultAccountManager(Context context)
        {
            _accountStore = AccountStore.Create(context);
            _sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(context);
        }

        public bool AccountExists()
        {
            return _accountStore.FindAccountsForService(DefaultAccount).Any();
        }
         
        public Account GetAccount()
        {
            var accounts = _accountStore.FindAccountsForService(DefaultAccount);
            if (!accounts.Any())
            {
                throw new Exception("Default account does not exist");
            }
            return accounts.First();
        }

        public void SaveAccount(Account account)
        {
            _accountStore.Save(account, DefaultAccount);
        }

        public User GetUser()
        {
            return new User
            {
                Id = _sharedPreferences.GetString(UserId, string.Empty),
                Name = _sharedPreferences.GetString(UserName, string.Empty),
                Email = _sharedPreferences.GetString(UserEmail, string.Empty),
            };
        }

        public void SaveUser(User user)
        {
            var editor = _sharedPreferences.Edit();
            editor.PutString(UserId, user.Id);
            editor.PutString(UserName, user.Name);
            editor.PutString(UserEmail, user.Email);
            editor.Apply();
        }
    }
}