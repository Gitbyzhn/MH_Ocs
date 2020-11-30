using MH_Ocs.Models;
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
    public class userprofileController : ApiController
    {

        private Entities db = new Entities();
        public async Task<IHttpActionResult> Post()
        {

            try
            {
                string UserName = User.Identity.Name;

                API_UserInfo UserInfo = new API_UserInfo();


                UserInfo DBUserInfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                UserProgress UserProgress = await UserGet.Progress(UserName, DBUserInfo.LevelId, "ru");

                UserInfo.Name = DBUserInfo.Fname;
                UserInfo.SureName = DBUserInfo.Lname;
                UserInfo.Image = DBUserInfo.Image;
                UserInfo.OUK = UserProgress.OUK;
                UserInfo.TBB = UserProgress.TBB;

                return Json(UserInfo);
            }
            catch(Exception ex) {

                return BadRequest(ex.Message);
            }
           
        }


    }
}
