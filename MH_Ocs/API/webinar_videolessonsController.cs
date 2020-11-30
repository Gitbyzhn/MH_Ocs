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
    public class webinar_videolessonsController : ApiController
    {

        private Entities db = new Entities();

        // POST: api/Default
        public async Task<IHttpActionResult> Post([FromBody]Webinar_VideoLessonsCs data)
        {



            ResponseUWebinar_VideoLs response = new ResponseUWebinar_VideoLs();



            var Webinar_videolessons = new List<UWebinar_VideoLessons>();


            try
            {

                string language = null;
              


                if (language == null) { language = "ru"; }

                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);




                var VideoLessons = await db.Webinar_VideoL.Where(e => e.language == language).OrderByDescending(e=>e.XId).ToListAsync();

               

                foreach (var item in VideoLessons)
                {

                    string Image = item.Iconimg != null ? Url.Content(item.Iconimg) : null;

                    Webinar_videolessons.Add(new UWebinar_VideoLessons
                    {

                        Id = item.Id,
                     
                        lang = item.language,
                        Name = item.Name,
                        Image =Image,
                        Like = item.Webinar_VideoXL.Webinar_VideoLEM.FirstOrDefault().Likes,
                        View = item.Webinar_VideoXL.Webinar_VideoLEM.FirstOrDefault().Eye,
                        minute = item.Webinar_VideoXL.Webinar_VideoLEM.FirstOrDefault().minute,
                    });
                }


                response.status = "ok";
                response.Webinar_VideoLessons = Webinar_videolessons;
                return Json(response);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

 



        }
    }
}
