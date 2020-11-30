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
    public class training_videolessonController : ApiController
    {

        private Entities db = new Entities();

        // POST: api/Default
        public async Task<IHttpActionResult> Post([FromBody]Training_VideoLessonCs data)
        {



            ResponseUTraining_VideoL response = new ResponseUTraining_VideoL();
            UTraining_VideoLesson Training_VideoLesson = new UTraining_VideoLesson();

            try
            {

                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                if (data == null)
                {

                    return BadRequest("data null");
                }

                if (data.Id == null)
                {

                    return BadRequest("data id null");
                }


                Training_VideoL VideoL = await db.Training_VideoL.FindAsync(data.Id);

                if (VideoL == null)
                {
                    return BadRequest("VideoL null");

                }

                if (VideoL.Training.Enable != true)
                {
                    return BadRequest("VideoL Training Disable");
                }

                if (VideoL.Training_VideoXL.Enable != true)
                {
                    return BadRequest("VideoL Disable");
                }
                
                int Like = 0;
                int View = 0;


                Training_VideoLEM videLem = VideoL.Training_VideoXL.Training_VideoLEM.FirstOrDefault();

                if (videLem != null)
                {
                    Like = videLem.Likes;
                    View = videLem.Eye;


                }

                bool liked = false;
                bool viewed = false;

                Training_EyeV eye = userinfo.Training_EyeV.FirstOrDefault(e => e.VideoXId == VideoL.XId);
                if (eye != null)
                {
                    viewed = true;
                }
                Training_LikeV like = userinfo.Training_LikeV.FirstOrDefault(e => e.VideoXId == VideoL.XId);
                if (like != null)
                {
                    liked = true;
                }



                Training_VideoXL vlXID = await db.Training_VideoXL.Where(e => e.XId < VideoL.XId).OrderByDescending(e => e.XId).FirstOrDefaultAsync();
                Training_VideoXL vnXID = await db.Training_VideoXL.Where(e => e.XId > VideoL.XId).OrderBy(e => e.XId).FirstOrDefaultAsync();

                Training_VideoL vl = null;
                Training_VideoL vn = null;

                if (vlXID != null)
                {
                    vl = await db.Training_VideoL.FirstOrDefaultAsync(e => e.XId == vlXID.XId && e.language == VideoL.language);
                    if (vl != null)
                    {
                        Training_VideoLesson.previousId = vl.Id;
                    }

                }
                if (vnXID != null)
                {
                    vn = await db.Training_VideoL.FirstOrDefaultAsync(e => e.XId == vnXID.XId && e.language == VideoL.language);
                    if (vn != null) {
                        Training_VideoLesson.nextId = vn.Id;
                    }

                }


                Training_VideoLesson.Id = VideoL.Id;
                Training_VideoLesson.lang = VideoL.language;
                Training_VideoLesson.Name = VideoL.Name;
                Training_VideoLesson.Link = VideoL.Link;
                Training_VideoLesson.Like = Like;
                Training_VideoLesson.View = View;
                Training_VideoLesson.Liked = liked;
                Training_VideoLesson.Viewed = viewed;
           
             


                response.status = "ok";

                response.Training_VideoLesson = Training_VideoLesson;

                return Json(response);
            }

            catch(Exception ex) {

                return BadRequest(ex.Message);
            }
            
            




        }

    }
}
