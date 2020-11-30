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
    public class training_videostatisticsController : ApiController
    {

        private Entities db = new Entities();

        public async Task<IHttpActionResult> Post([FromBody]VideoStaticsCs data)
        {


            try
            {

                string UserName = User.Identity.Name;

                Training_VideoL videols = await db.Training_VideoL.FindAsync(data.videoId);

                if (videols != null)
                {
                    Training_VideoLEM videolem = videols.Training_VideoXL.Training_VideoLEM.FirstOrDefault();



                    UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                    if (data.like == true)
                    {
                        Training_LikeV lkv = userinfo.Training_LikeV.FirstOrDefault(e => e.VideoXId == videols.XId);

                        if (lkv == null)
                        {

                            videolem.Likes += 1;
                            Training_LikeV newlkv = new Training_LikeV();
                            newlkv.UserId = userinfo.Id;
                            newlkv.VideoXId = videols.XId;
                            db.Training_LikeV.Add(newlkv);
                        }


                    }
                    else if (data.like == false)
                    {
                        videolem.Likes -= 1;
                        Training_LikeV lkv = userinfo.Training_LikeV.FirstOrDefault(e => e.VideoXId == videols.XId);
                        if (lkv != null)
                        {
                            db.Training_LikeV.Remove(lkv);
                        }


                    }


                    if (data.viewing == true)
                    {

                        Training_EyeV eye = userinfo.Training_EyeV.FirstOrDefault(e => e.VideoXId == videols.XId);

                        if (eye == null)
                        {
                            videolem.Eye += 1;
                            Training_EyeV eyenew = new Training_EyeV();
                            eyenew.UserId = userinfo.Id;
                            eyenew.VideoXId = videols.XId;
                            db.Training_EyeV.Add(eyenew);

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
