using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MH_Ocs.Models
{

    public class UserGet
    {



        public static async Task<UserProgress> Progress(string UserName, int? UserLevelId, string language)
        {

            Entities db = new Entities();

            if (UserLevelId == null)
            {
                UserLevelId = 0;
            }

            JVLO jv = await db.JVLOes.FirstOrDefaultAsync(e => e.UserName == UserName);
            var EnableVideoXLs = new List<VideoXL>();
            int NextV = 0;
            bool DBSave = false;
            bool EndCount = false;
            bool EmtyProcess = false;

            UserProgress progress = new UserProgress();

            try
            {
                
                //-----GET User ENABLE VIDEOXLs----------------------------------------

                var Moduls_userLevel = await db.Modul_userLevel.Where(e => e.LevelId <= UserLevelId).ToListAsync();
                if (Moduls_userLevel.Count == 0)
                {
                    EmtyProcess = true;
                }


                var Moduls = Moduls_userLevel.Select(e => e.Modul).Where(e => e.Enable == true).ToList();

                if (Moduls.Count() == 0)
                {
                    EmtyProcess = true;
                }

                if (EmtyProcess == false)
                {
                    foreach (var Module in Moduls)
                    {
                        EnableVideoXLs.AddRange(Module.VideoXLs.Where(e => e.Enable == true).ToList());
                    }

                    if (EnableVideoXLs.Count == 0)
                    {
                        EmtyProcess = true;
                    }
                }


                EnableVideoXLs = EnableVideoXLs.OrderBy(e => e.XId).ToList();

                //-----END------------------------------------------------------------



                if (!EmtyProcess)
                {
                    
                    UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);



                    //Error increase JVXID
                    double LastVXId = EnableVideoXLs.Max(e => e.XId);


                    if (jv.X > LastVXId)
                    {
                        jv.X = LastVXId;
                        DBSave = true;
                    }


                    var VideosOverXID = EnableVideoXLs.Where(e => e.XId >= jv.X).ToList();

               

                    foreach (var videoEXL in VideosOverXID)
                    {
                        
                        VideoL videols = videoEXL.VideoLs.FirstOrDefault(e => e.language == language);
                        if (videols == null)
                        {
                            videols = videoEXL.VideoLs.FirstOrDefault(e => e.language == "ru");
                        }
                        
                        if (videols != null)
                        {

                            ValitO vo = userinfo.ValitOS.FirstOrDefault(e => e.VdeoLXId == videols.XId);
                            Task task = await db.Tasks.FirstOrDefaultAsync(e => e.LessonXId == videols.XId);

                            var tst = videols.Tests.ToList();
                            if (tst.Count > 0)
                            {

                                NextV = 1;

                                if (vo != null)
                                {
                                    if (vo.KB > 74)
                                    {
                                        NextV = 0;

                                    }

                                }
                            }

                            if (NextV == 0 && task != null)
                            {
                                NextV = 2;
                                UserTaskCheck UserTaskCheck = userinfo.UserTaskChecks.FirstOrDefault(e => e.TaskId == task.Id);
                                if (UserTaskCheck != null)
                                {
                                    if (UserTaskCheck.Status == true)
                                    {
                                        NextV = 0;
                                    }
                                }

                            }

                            LessonVideoTime lookvideo = userinfo.LessonVideoTimes.FirstOrDefault(e => e.LessonXId == videols.XId);
                            if (lookvideo == null)
                            {
                                NextV = 3;
                            }
                            else if (lookvideo.Status == false)
                            {
                                NextV = 3;

                            }

                            if (NextV != 0)
                            {
                                if (jv.X < videols.XId)
                                {
                                    jv.X = videols.XId;
                                    DBSave = true;

                                }
                                break;
                            }

                        }



                    }



                    if (NextV == 0)
                    {
                        
                        jv.X = LastVXId;
                        DBSave = true;
                    }


                    if (DBSave)
                    {
                       await db.SaveChangesAsync();
                    }


                    int ALc = EnableVideoXLs.Where(e => e.XId < jv.X && (e.XId - (int)e.XId) < 0.1).Count() + 1;
                    int RALc = EnableVideoXLs.Where(e => e.XId > jv.X).Count();


                    int c = ALc + RALc;
                    int k = (ALc - 1) * 100;





                    if (ALc == c)
                    {



                        VideoL videols = await db.VideoLs.FirstOrDefaultAsync(e => e.language == "ru" && e.XId == jv.X);

                        if (videols != null)
                        {



                            var tst = db.Tests.Where(e => e.LessonId == videols.Id);

                            if (tst.Count() > 0)
                            {

                                ValitO valito = userinfo.ValitOS.FirstOrDefault(e => e.VdeoLXId == jv.X);
                                if (valito != null)
                                {
                                    if (valito.KB > 74)
                                    {
                                        EndCount = true;
                                    }
                                }

                            }
                            else if (tst.Count() == 0)
                            {

                                LessonVideoTime look = userinfo.LessonVideoTimes.FirstOrDefault(e => e.LessonXId == jv.X);
                                if (look != null)
                                {
                                    if (look.Status)
                                    {
                                        EndCount = true;
                                    }
                                }
                            }
                        }

                        if (EndCount)
                        {
                            k = ALc * 100;
                        }



                    }

                    progress.OUK = k / c;
                    progress.TBB = jv.TBB;
                    progress.EnableVideoXLs = EnableVideoXLs;
                    progress.JVLO = jv;
                    progress.NextV = NextV;
                }

            }

            catch
            { EmtyProcess = true; }


            if (EmtyProcess)
            {
                progress.OUK = 0;
                progress.TBB = 0;
                progress.EnableVideoXLs = EnableVideoXLs;
                progress.JVLO = jv;
                progress.NextV = 0;

            }

            return progress;

        }




    
    }
}