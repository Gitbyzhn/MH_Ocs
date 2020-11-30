using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using MH_Ocs.Models;
using MH_Ocs;
using MH_Ocs.Models.APIClass;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Data.Entity;

namespace WebApplication1.Controllers
{
    [Authorize]
    [RoutePrefix("api/mh/account")]
    public class accountController : ApiController
    {

        private Entities db = new Entities();
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public accountController()
        {
        }

        public accountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }






        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }





        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }



        // POST api/mh/account/login
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(LoginBindingModel model)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool sign = false;
            bool UserAPI = false;
            string ErrorMessage = null;
            UserToken UserTokenAPI = new UserToken();


            try
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://my.marinehealth.asia/api/auth/mobile");
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




            ApplicationUser result = await UserManager.FindAsync(model.UserName, model.Password);


            if (result != null && UserAPI)
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

                        var user = new ApplicationUser() { UserName = model.UserName, Email = model.UserName };

                        IdentityResult CreateUserResult = await UserManager.CreateAsync(user, model.Password);


                        if (!CreateUserResult.Succeeded)
                        {
                            return GetErrorResult(CreateUserResult);
                        }


                        try
                        {

                            if (UserTokenAPI.Token != null)
                            {

                                var rqstinfus = WebRequest.Create("https://my.marinehealth.asia/api/proxy/userprofile/get?includeSensitiveData=false");
                                var Httprqstinfus = (HttpWebRequest)rqstinfus;
                                Httprqstinfus.PreAuthenticate = true;

                                Httprqstinfus.Headers.Add("AuthToken", UserTokenAPI.Token);
                                Httprqstinfus.Accept = "application/json";
                                string responseiu;
                                WebResponse responseiuw = await Httprqstinfus.GetResponseAsync();
                                using (Stream stream = responseiuw.GetResponseStream())
                                {
                                    using (StreamReader reader = new StreamReader(stream))
                                    {
                                        responseiu = await reader.ReadToEndAsync();
                                    }
                                }

                                MHUserInfo MHUserInfo = JsonConvert.DeserializeObject<MHUserInfo>(responseiu);


                                if (MHUserInfo.ImageFileName != null && MHUserInfo.Contacts.Count > 0)
                                {
                                    MHUserInfo.ImageFileName = "https://my.marinehealth.asia/files/users/" + MHUserInfo.Contacts[0].UserId + "/images/" + MHUserInfo.ImageFileName;
                                }
                                
                                UserInfo UserInfoNew = new UserInfo();
                                UserInfoNew.Lname = MHUserInfo.LastName;
                                UserInfoNew.Fname = MHUserInfo.FirstName;
                                UserInfoNew.Image = MHUserInfo.ImageFileName;
                                UserInfoNew.UserName = model.UserName;
                                UserInfoNew.LevelId = MHUserInfo.LevelId == null ? 0 : MHUserInfo.LevelId;
                                UserInfoNew.RegTime = DateTime.Now.AddHours(6);
                                db.UserInfoes.Add(UserInfoNew);



                                JVLO jvnew = new JVLO();
                                jvnew.OV = 1;
                                jvnew.TBB = 0;
                                jvnew.UserName = model.UserName;
                                jvnew.X = 1;
                                db.JVLOes.Add(jvnew);

                                await db.SaveChangesAsync();

                            }


                            sign = true;
                        }
                        catch {
                            sign = false;
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

                    }



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




            if (sign)
            {
                UT UserToken = await db.UTs.FirstOrDefaultAsync(e => e.UserName == model.UserName);

                if (UserToken != null && UserTokenAPI != null)
                {
                    if (UserToken.Token != UserTokenAPI.Token)
                    {
                        UserToken.Token = UserTokenAPI.Token;

                    }
                     await db.SaveChangesAsync();

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


                return Ok();

            }

            return BadRequest(ErrorMessage);


        }



        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Вспомогательные приложения

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // Ошибки ModelState для отправки отсутствуют, поэтому просто возвращается пустой BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("Значение strengthInBits должно нацело делиться на 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
