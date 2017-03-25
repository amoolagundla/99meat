using System;
using System.Collections.Generic;

namespace _99meat.Models
{
    // Models returned by AccountController actions.

    public class OneSignalNotification
    {
        public string app_id { get; set; }
        public List<string> include_player_ids { get; set; }
        public Data data { get; set; }
        public Contents contents { get; set; }
        public string android_accent_color { get; set; }
        public string small_icon { get; set; }
        public string large_icon { get; set; }
        public string big_picture { get; set; }
    }

    public class Data
    {
        public string foo { get; set; }
    }

    public class Contents
    {
        public string en { get; set; }
    }

    public class OneSignalTokens
    {
        public string userId { get; set; }
        public string pushToken { get; set; }
    }

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }
    public class PasswordReset
    {

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }
    public class UserInfoViewModelWithAddresses
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }

        public string PhoneNumber { get; set; }

        public List<Address> Addresses { get; set; }

        public List<ViewModel.OrderViewModel> Orders { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Product> Products { get; set; }
        public List<category> Categories { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    public class PrasedToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
        public string token_type { get; set; }
    }
    public class FBTok
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
    }
    public class GmailAccess
    {
        public string email { get; set; }
        public string idToken { get; set; }
        public string serverAuthCode { get; set; }
        public string userId { get; set; }
        public string displayName { get; set; }
        public string familyName { get; set; }
        public string givenName { get; set; }
        public string imageUrl { get; set; }
    }
}
