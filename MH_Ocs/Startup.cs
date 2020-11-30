using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using MH_OCs;

[assembly: OwinStartupAttribute(typeof(MH_Ocs.Startup))]
namespace MH_Ocs
{
    public partial class Startup
    {

       
        public void Configuration(IAppBuilder app)  
        {
            ConfigureAuth(app);


            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            var myProvider = new MyAuthorizationServerProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/mh/auth"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                Provider = myProvider
            };





            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}
