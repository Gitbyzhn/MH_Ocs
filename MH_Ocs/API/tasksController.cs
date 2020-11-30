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
    public class tasksController : ApiController
    {

        private Entities db = new Entities();


        public async Task<IHttpActionResult> Get(string language)
        {

            if (language == null)
            {
                language = "ru";
            }

            ResponseUTasks response = new ResponseUTasks();

           


            var UTasks = new List<UTasks>();

            response.status = "filed";
            response.Tasks = UTasks;

            try
            {


                string UserName = User.Identity.Name;

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                var tasks = await db.Tasks.ToListAsync();

                string TaskTenDream = "";

                switch (language)
                {
                    case "kz":

                        TaskTenDream = "Осы бизнесте қол жеткізгіңіз келетін 10 арман туралы жазыңыз.";
                        break;
                    case "ru":
                        TaskTenDream = "Запишите 10 ваших конкретных мечты, которые Вы хотите достичь в этом бизнесе.";
                        break;

                    case "uz":
                        TaskTenDream = "Ushbu biznesda erishmoqchi bo'lgan aniq 10 ta orzularingizni yozing.";
                        break;

                    case "kr":
                        TaskTenDream = "Ушул бизнесте өзүңүз каалаган 10 кыялыңызга жазыңыз.";
                        break;

                    case "tr":
                        TaskTenDream = "Bu işte başarmak istediğiniz 10 hayalinizi yazın.";
                        break;

                    case "en":
                        TaskTenDream = "Write down 10 of your specific dreams that you want to achieve in this business.";
                        break;
                }


                foreach (var task in tasks)
                {
                    UserTaskCheck usertask = userinfo.UserTaskChecks.FirstOrDefault(e => e.TaskId == task.Id);

                    VideoL VideoL = await db.VideoLs.FirstOrDefaultAsync(e => e.XId == task.LessonXId && e.language == language);

                    if (usertask != null)
                    {

                        UTasks.Add(new UTasks
                        {
                            Id = task.Id,
                            VideoLessonId = VideoL.Id,
                            Task = TaskTenDream

                        });
                    }

                }

                response.status = "Ok";
                response.Tasks = UTasks;
                return Json(response);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }




        }

    }

