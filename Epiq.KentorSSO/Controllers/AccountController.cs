using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Epiq.KentorSSO.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : Controller
    {
        public AccountController()
        { }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get Owin Authentication Manager from HttpContext
        /// </summary>
        protected IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        /// <summary>
        /// Single Sign On Callback from IdentityServer with Claims to Login at Middleware
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]       
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = "~/";

            //Get Info from External login
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if(loginInfo == null)
            {
                throw new Exception("There is not External Login Info.");
            }

            var claims = new List<Claim>();
            //claims.Add(new Claim(ClaimTypes.NameIdentifier, loginInfo.ExternalIdentity.GetUserId()));
            //claims.Add(new Claim(ClaimTypes.Name, loginInfo.ExternalIdentity.Name));

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            //Sign In Using External Claims (Dummy example below)
            AuthenticationManager.SignIn(new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, identity);

            return Redirect(returnUrl);
        }

        public ActionResult LogOff()
        {
            //Signout
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
