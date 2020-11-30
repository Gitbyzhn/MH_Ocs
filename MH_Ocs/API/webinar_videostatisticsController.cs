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
    public class webinar_videostatisticsController : ApiController
    {

        private Entities db = new Entities();

        public async Task<IHttpActionResult> Post([FromBody]VideoStaticsCs data)
        {
            
            try
            {

                string UserName = User.Identity.Name;

                Webinar_VideoL videols = await db.Webinar_VideoL.FindAsync(data.videoId);

                if (videols != null)
                {
                    Webinar_VideoLEM videolem = videols.Webinar_VideoXL.Webinar_VideoLEM.FirstOrDefault();



                    UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                    if (data.like == true)
                    {
                        Webinar_LikeV lkv = userinfo.Webinar_LikeV.FirstOrDefault(e => e.VideoXId == videols.XId);

                        if (lkv == null)
                        {

                            videolem.Likes += 1;
                            Webinar_LikeV newlkv = new Webinar_LikeV();
                            newlkv.UserId = userinfo.Id;
                            newlkv.VideoXId = videols.XId;
                            db.Webinar_LikeV.Add(newlkv);
                        }


                    }
                    else if (data.like == false)
                    {
                        videolem.Likes -= 1;
                        Webinar_LikeV lkv = userinfo.Webinar_LikeV.FirstOrDefault(e => e.VideoXId == videols.XId);
                        if (lkv != null)
                        {
                            db.Webinar_LikeV.Remove(lkv);
                        }


                    }


                    if (data.viewing == true)
                    {

                        Webinar_EyeV eye = userinfo.Webinar_EyeV.FirstOrDefault(e => e.VideoXId == videols.XId);

                        if (eye == null)
                        {
                            videolem.Eye += 1;
                            Webinar_EyeV eyenew = new Webinar_EyeV();
                            eyenew.UserId = userinfo.Id;
                            eyenew.VideoXId = videols.XId;
                            db.Webinar_EyeV.Add(eyenew);

                        }

                    }

                    //if (data.chosen == true)
                    //{


                    //    Isbranni izb = userinfo.Isbrannis.FirstOrDefault(e => e.VideoLXId == videols.XId);

                    //    if (izb == null)
                    //    {
                    //        Isbranni newizb = new Isbranni();
                    //        newizb.UserId = userinfo.Id;
                    //        newizb.VideoLXId = videols.XId;
                    //        db.Isbrannis.Add(newizb);

                    //    }




                    //}
                    //else if (data.chosen == false)
                    //{

                    //    Isbranni izb = userinfo.Isbrannis.FirstOrDefault(e => e.VideoLXId == videols.XId);
                    //    if (izb != null)
                    //    {
                    //        db.Isbrannis.Remove(izb);
                    //    }


                    //}


                    await db.SaveChangesAsync();

                    return Ok();

                }

                return BadRequest("Такого видео-урока нет");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }



        }

    }
}
