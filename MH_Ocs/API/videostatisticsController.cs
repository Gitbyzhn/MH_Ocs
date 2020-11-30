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
    public class videostatisticsController : ApiController
    {


        private Entities db = new Entities();

        public async Task<IHttpActionResult> Post([FromBody]VideoStaticsCs data)
        {


            try
            {

                string UserName = User.Identity.Name;

                VideoL videols = await db.VideoLs.FindAsync(data.videoId);

                if (videols != null)
                {
                    VideoLEM videolem = videols.VideoXL.VideoLEMs.FirstOrDefault();



                    UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);
                    if (data.like == true)
                    {
                        LikeV lkv = userinfo.LikeVs.FirstOrDefault(e => e.VideoXId == videols.XId);

                        if (lkv == null)
                        {

                            videolem.Likes += 1;
                            LikeV newlkv = new LikeV();
                            newlkv.UserId = userinfo.Id;
                            newlkv.VideoXId = videols.XId;
                            db.LikeVs.Add(newlkv);
                        }

                      
                    }
                    else if (data.like == false)
                    {
                        videolem.Likes -= 1;
                        LikeV lkv = userinfo.LikeVs.FirstOrDefault(e => e.VideoXId == videols.XId);
                        if (lkv != null)
                        {
                            db.LikeVs.Remove(lkv);
                        }


                    }


                    if (data.viewing == true)
                    {

                        EyeV eye = userinfo.EyeVs.FirstOrDefault(e => e.VideoXId == videols.XId);

                        if (eye == null)
                        {
                            videolem.Eye += 1;
                            EyeV eyenew = new EyeV();
                            eyenew.UserId = userinfo.Id;
                            eyenew.VideoXId = videols.XId;
                            db.EyeVs.Add(eyenew);

                        }

                    }

                    if (data.chosen == true)
                    {


                        Isbranni izb = userinfo.Isbrannis.FirstOrDefault(e => e.VideoLXId == videols.XId);

                        if (izb == null)
                        {
                            Isbranni newizb = new Isbranni();
                            newizb.UserId = userinfo.Id;
                            newizb.VideoLXId = videols.XId;
                            db.Isbrannis.Add(newizb);

                        }

                    


                    }
                    else if (data.chosen == false)
                    {

                        Isbranni izb = userinfo.Isbrannis.FirstOrDefault(e => e.VideoLXId == videols.XId);
                        if (izb != null)
                        {
                            db.Isbrannis.Remove(izb);
                        }


                    }


                    await db.SaveChangesAsync();

                    return Ok();

                }

                return BadRequest("Такого видео-урока нет");
            
            }
            catch(Exception ex) {

                return BadRequest(ex.Message);

            }


            
        }
    }
  }
