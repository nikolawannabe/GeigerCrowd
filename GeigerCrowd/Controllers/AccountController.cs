using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using GeigerCrowd.Models;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;

namespace GeigerCrowd.Controllers
{
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();

        private GeigerCrowdContext context = new GeigerCrowdContext();


        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        [ValidateInput(false)]
		public ActionResult Authenticate(string returnUrl) 
        {
			var response = openid.GetResponse();
			if (response == null) {
				// Stage 2: user submitting Identifier
				Identifier id;
				if (Identifier.TryParse(Request.Form["openid_identifier"], out id)) {
					try {
                        IAuthenticationRequest request = openid.CreateRequest(Request.Form["openid_identifier"]);
						 request.AddExtension(new ClaimsRequest {
                                Email = DemandLevel.Require,
                                PostalCode = DemandLevel.Require,
                                TimeZone = DemandLevel.Require,
                        });
                         var ax = new FetchRequest();
                         ax.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email,true));
                         ax.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.FullName,true));
                         request.AddExtension(ax);
                         return request.RedirectingResponse.AsActionResult();
					} catch (ProtocolException ex) {
						ViewData["Message"] = ex.Message;
						return View("Login");
					}
				} else {
					ViewData["Message"] = "Invalid identifier";
					return View("Login");
				}
			} else {
				// Stage 3: OpenID Provider sending assertion response
				switch (response.Status) {
					case AuthenticationStatus.Authenticated:
						Session["FriendlyIdentifier"] = response.FriendlyIdentifierForDisplay;
                        Session["ClaimedIdentifier"] = response.ClaimedIdentifier;
                        string claimedidentifier = response.ClaimedIdentifier;
						FormsAuthentication.SetAuthCookie(response.FriendlyIdentifierForDisplay, false);
                        string username = MembershipService.GetUserName(claimedidentifier);
                        if (username != "")
                        {
                            FormsAuthentication.SetAuthCookie(username,false);
                            return RedirectToAction("Index","Home");
                        }else {
							return RedirectToAction("Register", "Account");
						}
					case AuthenticationStatus.Canceled:
						ViewData["Message"] = "Canceled at provider";
						return View("Login");
					case AuthenticationStatus.Failed:
						ViewData["Message"] = response.Exception.Message;
						return View("Login");
				}
			}
			return new EmptyResult();
		}

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        public ActionResult Login()
        {
            // Stage 1: display login form to user
            return View("Login");
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if ((ModelState.IsValid) && (model.OpenID == null))
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }
            else if ((ModelState.IsValid) && (model.OpenID != null))
            {
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email, model.OpenID);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [Authorize]
        public ActionResult Profile(string openidurl)
        {
            string username = this.User.Identity.Name;

            ViewBag.URLs = MembershipService.GetOpenIdUrls(username);
            return View();
        }

        [Authorize]
        public ActionResult AddOpenId()
        {
            var openidurl = Request.Form["openid_identifier"];
            var response = openid.GetResponse();
            if (response == null)
            {
                if (String.IsNullOrEmpty(openidurl)) return View();
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(openidurl, out id))
                {
                    try
                    {
                        IAuthenticationRequest request = openid.CreateRequest(openidurl);
                        return request.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        ViewBag.Message = ex.Message;
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid identifier";
                    return View();
                }
            }
            else 
            {
                // Stage 3: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        string claimedidentifier = response.ClaimedIdentifier;
                        string username = this.User.Identity.Name;
                        if (MembershipService.AttachOpenID(claimedidentifier, username) == true)
                        {
                            ViewBag.Message = "Added OpenID to your account!";
                        }
                        else
                        {
                            ViewBag.Message = "Unable to add this id to your account.";
                        }
                        return View();
                    case AuthenticationStatus.Canceled:
                        ViewBag.Message = "Canceled at provider";
                        return View();
                    case AuthenticationStatus.Failed:
                        ViewBag.Message = response.Exception.Message;
                        return View();
                }
            }
            return View();
        }

    }
}
