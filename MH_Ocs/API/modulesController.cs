using MH_Ocs.Models;
using MH_Ocs.Models.APIClass;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MH_Ocs.API
{
    [Authorize]
    public class modulesController : ApiController
    {
        
        private Entities db = new Entities();

       
        public async Task<IHttpActionResult> Post([FromBody]ModulCs data)
        {

            ResponseUModuls response = new ResponseUModuls();

            try
            {
                if (data == null) { 
                    data = new ModulCs() { language = "ru" };
                }

                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

               

                var ModulObj = await db.Moduls.Where(e => e.Enable == true).OrderBy(e => e.XId).ToListAsync();
                var UModulesObj = new List<UModuls>();

                foreach (var module in ModulObj)
                {

                    Modules_Property ModulesP = module.Modules_Property.FirstOrDefault(e => e.lang == data.language);
                    if (ModulesP == null)
                    {
                        ModulesP = module.Modules_Property.FirstOrDefault(e => e.lang == "ru");
                    }

                    if (ModulesP != null)
                    {
                        Modul_userLevel ModulesUL = module.Modul_userLevel.FirstOrDefault();


                        UModuls newUModules = new UModuls();
                        newUModules.Id = module.XId;
                        newUModules.Name = ModulesP.Titile;

                        newUModules.Image = Url.Content(module.Image);
                        newUModules.lang = ModulesP.lang;

                        bool Enable = true;

                        if (ModulesUL.LevelId > userinfo.LevelId)
                        {
                            Enable = false;
                        }

                        newUModules.Enable = Enable;


                        UModulesObj.Add(newUModules);
                    }

                }

                Users_Certificates UserCertificate = userinfo.Users_Certificates.FirstOrDefault(e => e.PublicCert == true && e.TypeCert == 3);


                response.status = "ok";
                response.certificateURL = UserCertificate!=null?Url.Content(UserCertificate.CertificateURL):null;
                response.UModuls = UModulesObj;

                return Json(response);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
