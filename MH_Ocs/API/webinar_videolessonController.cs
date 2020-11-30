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
    public class webinar_videolessonController : ApiController
    {

        private Entities db = new Entities();

        // POST: api/Default
        public async  Task<IHttpActionResult> Post([FromBody]Webinar_VideoLessonCs data)
        {

            ResponseUWebinar_VideoL response = new ResponseUWebinar_VideoL();
            UWebinar_VideoLesson Webinar_VideoLesson = new UWebinar_VideoLesson();
            
            string UserName = User.Identity.Name;

            UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
            

            if (data == null)
            {

                return BadRequest("data null");
            }

            if (data.Id == null)
            {


                return BadRequest("data null");
            }


            Webinar_VideoL VideoL = await db.Webinar_VideoL.FindAsync(data.Id);

            if (VideoL == null)
            {
                return BadRequest("VideoL Not found");

            }
            
            if (VideoL.Webinar_VideoXL.Enable != true)
            {
                return BadRequest("VideoL Disabled");
            }





            try
            {

                int Like = 0;
                int View = 0;


                Webinar_VideoLEM videLem = VideoL.Webinar_VideoXL.Webinar_VideoLEM.FirstOrDefault();

                if (videLem != null)
                {
                    Like = videLem.Likes;
                    View = videLem.Eye;


                }



                bool liked = false;
                bool viewed = false;
    
                Webinar_EyeV eye = userinfo.Webinar_EyeV.FirstOrDefault(e => e.VideoXId == VideoL.XId);
                if (eye != null)
                {
                    viewed = true;
                }
                Webinar_LikeV like = userinfo.Webinar_LikeV.FirstOrDefault(e => e.VideoXId == VideoL.XId);
                if (like != null)
                {
                    liked = true;
                }


   


                Webinar_VideoXL vlXID = await db.Webinar_VideoXL.Where(e => e.XId < VideoL.XId).OrderByDescending(e => e.XId).FirstOrDefaultAsync();
                Webinar_VideoXL vnXID = await db.Webinar_VideoXL.Where(e => e.XId > VideoL.XId).OrderBy(e => e.XId).FirstOrDefaultAsync();

                Webinar_VideoL vl = null;
                Webinar_VideoL vn = null;

                if (vlXID != null)
                {
                    vl = await db.Webinar_VideoL.FirstOrDefaultAsync(e => e.XId == vlXID.XId && e.language == VideoL.language);
                    if (vl != null)
                    {
                        Webinar_VideoLesson.previousId = vl.Id;
                    }
                   
                }
                if (vnXID != null)
                {
                    vn = await db.Webinar_VideoL.FirstOrDefaultAsync(e => e.XId == vnXID.XId && e.language == VideoL.language);
                    if (vn != null)
                    {
                        Webinar_VideoLesson.nextId = vn.Id;
                    }

                }




                Webinar_VideoLesson.Id = VideoL.Id;
                Webinar_VideoLesson.lang = VideoL.language;
                Webinar_VideoLesson.Name = VideoL.Name;
                Webinar_VideoLesson.Link = VideoL.Link;
                Webinar_VideoLesson.Like = Like;
                Webinar_VideoLesson.View = View;
                Webinar_VideoLesson.Liked = liked;
                Webinar_VideoLesson.Viewed = viewed;
            
                Webinar_VideoLesson.nextId = vn.Id;
               

                response.status = "ok";

                response.Webinar_VideoLesson = Webinar_VideoLesson;

                return Json(response);
            }

            catch(Exception ex) {


                return BadRequest(ex.Message);
            }








        }
    }
}
