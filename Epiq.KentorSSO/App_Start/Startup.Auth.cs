using Kentor.AuthServices;
using Kentor.AuthServices.Configuration;
using Kentor.AuthServices.Owin;
using Kentor.AuthServices.WebSso;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.IdentityModel.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Web.Hosting;

namespace Epiq.KentorSSO
{
    public partial class Startup
    {
		public void ConfigureAuth(IAppBuilder app)
        {
            //Use Authentication Cookies
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            app.SetDefaultSignInAsAuthenticationType(DefaultAuthenticationTypes.ApplicationCookie);
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //Enable Kentor SSO
            app.UseKentorAuthServicesAuthentication(GetAuthOptions());
        }

		private static KentorAuthServicesAuthenticationOptions GetAuthOptions()
        {
            var spOptions = GetSPOptions();
            var authOptions = new KentorAuthServicesAuthenticationOptions(false)
            {
				 SPOptions = spOptions				 
            };

            //Identity Server Configuration (Below we are using a dummy version of Kendor)
            var idp = new IdentityProvider(new EntityId("http://stubidp.kentor.se/Metadata"), spOptions)
            {
				 AllowUnsolicitedAuthnResponse = true,
				 Binding = Saml2BindingType.HttpRedirect,
				 SingleSignOnServiceUrl = new Uri("http://stubidp.kentor.se")
            };

            idp.SigningKeys.AddConfiguredKey(
				 new X509Certificate2(
					 HostingEnvironment.MapPath(
						 "~/App_Data/Kentor.AuthServices.StubIdp.cer")));

            authOptions.IdentityProviders.Add(idp);

            return authOptions;
        }


		private static SPOptions GetSPOptions()
        {        
            var spOptions = new SPOptions()
            {
				 EntityId = new EntityId("http://localhost:61768"),
                 ReturnUrl = new Uri("http://localhost:61768/account/ExternalLoginCallback"),
                 ModulePath = "/saml/KentorSSO/"       
            };

            spOptions.ServiceCertificates.Add(new X509Certificate2(
				AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/App_Data/Kentor.AuthServices.Tests.pfx"));


            return spOptions;
        }
    }
}
