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
    public class training_modulesController : ApiController
    {

        private Entities db = new Entities();


        public async Task<IHttpActionResult> Post([FromBody]Training_ModulesCs data)
        {

            ResponseUTraining_Modules response = new ResponseUTraining_Modules();

            try
            {
                if (data.language == null) { data.language = "ru"; }

                string UserName = User.Identity.Name;

                
                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                

                var ModulObj = await db.Trainings.Where(e => e.Enable == true).OrderBy(e => e.XId).ToListAsync();

                var UModulesObj = new List<UTraining_Modules>();

                foreach (var module in ModulObj)
                {

                    Training_Property ModulesP = module.Training_Property.FirstOrDefault(e => e.lang == data.language);
                   

                    if (ModulesP != null)
                    {



                        UTraining_Modules newUModules = new UTraining_Modules();

                        newUModules.Id = module.XId;
                        newUModules.Name = ModulesP.Titile;

                        newUModules.Image = Url.Content(module.Image);
                        newUModules.lang = ModulesP.lang;
                        
                        UModulesObj.Add(newUModules);
                    }

                }


                response.status = "ok";
                
                response.Training_Modules = UModulesObj;


                return Json(response);
            }
            catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }


    }
}
