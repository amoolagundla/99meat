﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using _99meat.Models;
using Facebook;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace _99meat.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }

        public override async Task GrantCustomExtension(OAuthGrantCustomExtensionContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            string email = string.Empty;
            var userdetails = new GmailAccess();
            var nuser = new ApplicationUser();
            if (context.GrantType.ToLower() == "facebook")
            {
                var fb = new FacebookClient("1835538836698571|XsHdA0c38xZZArNW7MnrYlQMiIk");
                dynamic result = fb.Get("debug_token", new { input_token = context.Parameters.Get("accesstoken").ToString() });
                var appId = result.data.app_id;
                var isValid = result.data.is_valid;
                var application = result.data.application;
                var userId = result.data.user_id;
                var expiresAt = result.data.expires_at;
                var scopes = result.data.scopes;


                dynamic response = await fb.GetTaskAsync("/me?fields=email,gender", new { access_token = context.Parameters.Get("accesstoken").ToString() });

                string id = response.id;
                email = response.email;
                 nuser = new ApplicationUser()
                {
                    UserName = email,
                    Email = email
                  

                };

            }

            else if (context.GrantType.ToLower() == "google")
            {


                 userdetails = Newtonsoft.Json.JsonConvert.DeserializeObject<GmailAccess>(context.Parameters.Get("accesstoken").ToString());


                using (var clients = new HttpClient())
                {
                    clients.BaseAddress = new Uri("https://accounts.google.com/");
                    var content = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("client_id", "794768984490-keg257msut0vkmlfp92o0a68o6i36q41.apps.googleusercontent.com"),
                new KeyValuePair<string, string>("client_secret", "KqzFGztmh8rYGFTS29ge8qUr"),          
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                 new KeyValuePair<string, string>("code", userdetails.serverAuthCode)
            });
                    var result = await clients.PostAsync("o/oauth2/token", content);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    var userdet=Newtonsoft.Json.JsonConvert.DeserializeObject< PrasedToken>(resultContent);
                    if(!string.IsNullOrEmpty(userdet.access_token))
                    email = userdetails.email;

                    nuser = new ApplicationUser()
                    {
                        UserName = email,
                        Email = email,
                         FirstName=userdetails.familyName,
                         LastName=userdetails.givenName


                    };
                }

               
              
            }
            if (string.IsNullOrEmpty(email))
            {
                return;
            }
          


            ApplicationUser user = userManager.FindByEmail(email);

            if (user != null)
            {


                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = CreateProperties(user.UserName);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            else
            {

                var newUSer = await userManager.CreateAsync(nuser);

                var fUSer = userManager.FindByEmail(email);
                ClaimsIdentity oAuthIdentity = await fUSer.GenerateUserIdentityAsync(userManager,
                        OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookiesIdentity = await fUSer.GenerateUserIdentityAsync(userManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = CreateProperties(fUSer.UserName);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }

            return;

        }
    }
}