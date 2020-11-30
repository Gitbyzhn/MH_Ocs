using MH_Ocs.Models;
using MH_Ocs.Models.APIClass;
using MH_Ocs.Models.ModelView;
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
    public class tapeController : ApiController
    {


        private Entities db = new Entities();

        public async  Task<IHttpActionResult> Get()
        {


            string language = "ru";

       
            try
            {
                var TrapVideoLs = new List<TapVideoLessonsView>();

                var trainingVLs = await db.Training_VideoL.Where(e => e.language == language).ToListAsync();

                var webinarVLs = await db.Webinar_VideoL.Where(e => e.language == language).ToListAsync();

                foreach (var trainingVL in trainingVLs)
                {
                    TrapVideoLs.Add(new TapVideoLessonsView
                    {
                        Id = trainingVL.Id,
                        Modul = "training",
                        Name = trainingVL.Name,
                        Link = trainingVL.Link,
                        Iconimg = trainingVL.Iconimg,
                        Iconimg2 = trainingVL.Iconimg2,
                        Date = trainingVL.Date.Value.ToString("MM/dd/yyyy"),
                        Time = trainingVL.Date.Value.ToString("HH:mm:ss"),
                        language = trainingVL.language,
                        
                  

                    });
                }


                foreach (var webinarVL in webinarVLs)
                {


                    string Date = null;
                    string Time = null;
                    if (webinarVL.Date != null)
                    {
                        Date = webinarVL.Date.Value.ToString("MM/dd/yyyy");
                        Time = webinarVL.Date.Value.ToString("HH:mm:ss");
                    }

                    

                    TrapVideoLs.Add(new TapVideoLessonsView
                    {
                        Id = webinarVL.Id,
                        Modul = "webinar",
                        Name = webinarVL.Name,
                        Link = webinarVL.Link,
                        Iconimg = webinarVL.Iconimg,
                        Iconimg2 = webinarVL.Iconimg2,
                        Date = Date,
                        Time = Time,
                        language = webinarVL.language,
              

                    });
                }



                TrapVideoLs = TrapVideoLs.OrderByDescending(e => e.Date).ToList();


                return Json(TrapVideoLs);
           

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }
    }

}
