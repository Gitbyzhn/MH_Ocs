using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MH_Ocs.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Data.Entity;

namespace MH_Ocs.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private Entities db = new Entities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [AllowAnonymous]
        public ActionResult sadmin()
        {

          
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> sadmin(LoginViewModel model)
        {
            
            //var user = new ApplicationUser { UserName = model.UserName, Email = model.UserName};
            //var resultr = await UserManager.CreateAsync(user, model.Password);

            //if (resultr.Succeeded)
            //{
            //    return View();
            //}

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, shouldLockout: false);
            if (result == SignInStatus.Success)
            {
                return RedirectToAction("portalsignin", "Account", new { language = model.Language });
            }
            ModelState.AddModelError("", "Имя пользователя или пароль указаны неверно.");
            return View();
        }


        public ActionResult portalsignin()
        {
            if (User.IsInRole("admin"))
            {

                return RedirectToAction("VideoLessons","portal");
            }


            return RedirectToAction("sadmin");
        }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string language)
        {
            if (language == null) { language = "ru"; }
            ViewBag.language = language;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {



            AuthenticationManager.SignOut();

            model.UserName = model.UserName.Replace(" ", "");

            bool sign = false;
            bool UserAPI = false;
            string ErrorMessage = null;
            UserToken UserTokenAPI = new UserToken();
         

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, shouldLockout: false);
            
            try
            {

                var httpWebRequest = WebRequest.Create("https://my.marinehealth.asia/api/auth/mobile");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"Email\":\"" + model.UserName + "\"," + "\"Password\":\"" + model.Password + "\"}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                string responsezec;
                WebResponse response = await httpWebRequest.GetResponseAsync();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        responsezec = await reader.ReadToEndAsync();
                    }
                }


                UserTokenAPI = JsonConvert.DeserializeObject<UserToken>(responsezec);
                
                UserAPI = true;

                response.Close();

            }

            catch
            {
                UserAPI = false;
            }


            if (result == SignInStatus.Success && UserAPI)
            {

                try
                {

                    if (UserTokenAPI.Status == "Success")
                    {

                        sign = true;

                    }
                    else if (UserTokenAPI.Status == "TooManyAttempts")
                    {
                        ErrorMessage = "Cлишком много попыток, пожалуйста, попробуйте еще раз позже";
                    }
                    else
                    {

                        ErrorMessage = "Имя пользователя или пароль указаны неверно.";
                    }


                }
                catch { }


            }
            else if (UserAPI)
            {


                if (UserTokenAPI.Status == "Success")
                {

                    var User = await UserManager.FindByNameAsync(model.UserName);

                    if (User == null)
                    {

                        var user = new ApplicationUser { UserName = model.UserName, Email = model.UserName };
                        var CreateUserResult = await UserManager.CreateAsync(user, model.Password);


                        if (CreateUserResult.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            sign = true;
                        }

                    }
                    else
                    {




                        string code = await UserManager.GeneratePasswordResetTokenAsync(User.Id);
                        var ResertPasswordResult = await UserManager.ResetPasswordAsync(User.Id, code, model.Password);
                        if (ResertPasswordResult.Succeeded)
                        {
                            sign = true;
                        }

                        await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, shouldLockout: false);

                    }

                    sign = true;


                }
                else
                {
                    if (UserTokenAPI.Status == "TooManyAttempts")
                    {
                        ErrorMessage = "Cлишком много попыток, пожалуйста, попробуйте еще раз позже";
                    }
                    else
                    {

                        ErrorMessage = "Имя пользователя или пароль указаны неверно.";
                    }


                }



            }



            ModelState.AddModelError("", ErrorMessage);

            if (sign)
            {

                UT UserToken = await db.UTs.FirstOrDefaultAsync(e => e.UserName == model.UserName);

                if (UserToken != null && UserTokenAPI!=null)
                {
                    if (UserToken.Token != UserTokenAPI.Token)
                    {
                        UserToken.Token = UserTokenAPI.Token;
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    UT newUserToken = new UT();
                    newUserToken.UserName = model.UserName;
                    newUserToken.Token = UserTokenAPI.Token;
                    newUserToken.Date = DateTime.Now.AddHours(6);
                    db.UTs.Add(newUserToken);
                    await db.SaveChangesAsync();

                }
          

                return RedirectToAction("Initialize", "User", new { language = model.Language });
            }


            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            ViewBag.language = model.Language;
            return View(model);


        }



        public ActionResult ErrorLogOff(string language,string ActionName)
        {
            if (language == null) { language = "ru"; }

            try
            {

                ErrorLogOFF ErrorLogOFFObj = new ErrorLogOFF();

                ErrorLogOFFObj.Action = ActionName;

                ErrorLogOFFObj.UserName = User.Identity.Name;

                ErrorLogOFFObj.date = DateTime.Now.AddHours(6);

                db.ErrorLogOFFs.Add(ErrorLogOFFObj);
                db.SaveChanges();
            }
            catch { }


            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account",new { language = language});
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }







        #region Вспомогательные приложения
        // Используется для защиты от XSRF-атак при добавлении внешних имен входа
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}