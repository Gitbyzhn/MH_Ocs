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
    public class testController : ApiController
    {

        private Entities db = new Entities();
        public async Task<IHttpActionResult> Post([FromBody]TestCs data)
        {



            ResponseUTest response = new ResponseUTest();
            var UTest = new List<UTest>();

            string UserName = User.Identity.Name;
            try
            {

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);


                JVLO JV = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);

                if (data == null)
                {

                    return BadRequest("data null");
                }

                if (data.Id == null)
                {


                    return BadRequest("data id null");
                }


                VideoL VideoL = await db.VideoLs.FindAsync(data.Id);

                if (VideoL == null)
                {
                    return BadRequest("VideoL null");

                }

                if (VideoL.Modul.Enable != true)
                {
                    return BadRequest("VideoLessons Modul Disable");
                }

                if (VideoL.VideoXL.Enable != true)
                {
                    return BadRequest("VideoLessons Disable");
                }

                if (VideoL.Modul.Modul_userLevel.FirstOrDefault().LevelId > userinfo.LevelId)
                {

                    return BadRequest("User level not available");
                }

                if (VideoL.XId > JV.X)
                {
                    return BadRequest("User level not available");
                }
                if (VideoL.Tests.Count == 0)
                {
                    return BadRequest("There is no test in the video lesson");
                }




                int total = 0;
                ValitO valito = userinfo.ValitOS.FirstOrDefault(e => e.VdeoLXId == VideoL.XId);
                if (valito != null)
                {
                    total = valito.KB;
                }

                foreach (var test in VideoL.Tests)
                {
                    UTest.Add(new UTest
                    {
                        Question = test.Question,
                        A = test.A,
                        B = test.B,
                        C = test.C,
                        D = test.D,
                        E = test.E,
                        Answer = test.Answer

                    });

                }



                response.status = "ok";
                response.Total = total;
                response.UTest = UTest;

                return Json(response);
            }

            catch(Exception ex) {

                return BadRequest(ex.Message);
            }
            
        }

    }
}
