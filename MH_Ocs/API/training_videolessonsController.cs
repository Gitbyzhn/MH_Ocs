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
    public class training_videolessonsController : ApiController
    {

        private Entities db = new Entities();

        // POST: api/Default
        public async Task<IHttpActionResult> Post([FromBody]Training_VideoLessonsCs data)
        {
            
            ResponseUTraining_VideoLs response = new ResponseUTraining_VideoLs();

            var training_videolessons = new List<UTraining_VideoLessons>();


            try
            {

                string language = null;
                int? MId = null;

                if (data != null)
                {
                    language = data.language;
                    MId = data.MId;

                }


                if (language == null) { language = "ru"; }

                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);




                var VideoLessons = await db.Training_VideoL.Where(e => e.language == language).OrderByDescending(e=>e.XId).ToListAsync();

                if (data.MId != null)
                {
                    VideoLessons = VideoLessons.Where(e => e.TrId == data.MId).OrderByDescending(e => e.XId).ToList();
                }


            

                foreach (var item in VideoLessons)
                {

                    string Image = item.Iconimg != null ? Url.Content(item.Iconimg) : null;

                    training_videolessons.Add(new UTraining_VideoLessons
                    {

                        Id = item.Id,
                        MId = item.TrId,
                        lang = item.language,
                        Name = item.Name,
                        Image = Image,
                        Like = item.Training_VideoXL.Training_VideoLEM.FirstOrDefault().Likes,
                        View = item.Training_VideoXL.Training_VideoLEM.FirstOrDefault().Eye,
                        minute = item.Training_VideoXL.Training_VideoLEM.FirstOrDefault().minute,
                    });
                }

                
                response.status = "ok";
                response.Training_VideoLessons = training_videolessons;

                return Json(response);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }




        }

    }
}
