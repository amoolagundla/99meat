using System;
using System.Collections.Generic;

namespace _99meat.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
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

        public List<Address> Addresses { get;set;}

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
}
