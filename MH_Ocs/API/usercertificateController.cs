using MH_Ocs.Models;
using MH_Ocs.Models.APIClass;
using NReco.ImageGenerator;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;



namespace MH_Ocs.API
{
    [Authorize]
    public class usercertificateController : ApiController
    {

        private Entities db = new Entities();

        public async Task<IHttpActionResult> Post(UcertificateCs data)
        {

            string language = "ru";

            if (data != null)
            {
                if (data.language != null)
                {
                    language = data.language;
                }

            }


            ResponseUCertificate response = new ResponseUCertificate();

            var UCertificate = new UCertificate();

            string UserName = User.Identity.Name;


            try
            {

                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                Users_Certificates UserCertificate = userinfo.Users_Certificates.FirstOrDefault(e => e.PublicCert == true && e.TypeCert == 3);

                User_SendCompleteTraining user_completeTR = await db.User_SendCompleteTraining.FirstOrDefaultAsync(e => e.UserName == UserName);

                if (user_completeTR == null)
                {
                    bool sts = await markTrainingComplete(UserName);

                    if (sts)
                    {
                        User_SendCompleteTraining newuser_completeTR = new User_SendCompleteTraining();
                        newuser_completeTR.UserName = UserName;
                        newuser_completeTR.SendCertificate = true;
                        db.User_SendCompleteTraining.Add(newuser_completeTR);
                        await db.SaveChangesAsync();
                    }

                }

                if (UserCertificate == null)
                {
                    string certificateURL = await MakeCertificate(UserName, language);
                    if (certificateURL != null)
                    {
                        UCertificate.certificateURL = Url.Content(UCertificate.certificateURL);
                    }
             

                }
                else
                {

                    UCertificate.certificateURL = Url.Content(UserCertificate.CertificateURL);

                }

                UCertificate.certificate = true;


                response.status = "ok";
                response.UCertificate = UCertificate;
                return Json(response);

            }
            catch(Exception ex) {

                return BadRequest(ex.Message);
            }
        }


        public class UcertificateCs
        {

            public string language { get; set; }
        }



        public async Task<string> MakeCertificate(string UserName, string language)
        {

            string result = null;
            language = "ru";
            try
            {
                UserInfo userinfo = await db.UserInfoes.FirstOrDefaultAsync(e => e.UserName == UserName);

                string Lname = "";
                string Fname = "";

                if (userinfo.Lname != null)
                {
                    Lname = userinfo.Lname;
                }

                if (userinfo.Fname != null)
                {
                    Fname = userinfo.Fname;
                }

                DateTime CurrentDate = DateTime.Now.AddHours(6);
                string Day = CurrentDate.Day.ToString();
                string Month = CurrentDate.Month.ToString();
                string Year = CurrentDate.Year.ToString();
                string Date = Day + "." + Month + "." + Year;
                string imageName = "";

                await System.Threading.Tasks.Task.Run(() =>
                {

                    int usernameLg = (Lname.Length + Fname.Length) / 2;

                    int fontSize = 140;
                    int alfa = 840 - usernameLg * 89;


                    if (usernameLg > 20)
                    {
                        fontSize = 120;
                        alfa = 840 - usernameLg * 80;
                    }



                    int leftPosition = 600 + alfa;


                    var htmlToImageConv = new HtmlToImageConverter();

                    //string html = "<html> <head><meta http-equiv=Content-Type content='text/html; charset=UTF-8'><style type='text/css'>@font-face{font-family: 'Font Name';src: url('http://academy.marinehealth.asia/Images/cert/10846.ttf') format('truetype');}body {font-family: 'Font Name', Fallback, sans-serif;}</style><script type = 'text/javascript' src = 'a8535914-f191-11e9-9d71-0cc47a792c0a_id_a8535914-f191-11e9-9d71-0cc47a792c0a_files/wz_jsgraphics.js' ></script></head><body ><div style = 'position:absolute;left:50%;margin-left:-1462px;top:0px;width:2924px;height:1995px;border-style:outset;overflow:hidden' > <div style = 'position:absolute;left:0px;top:0px' ><img src='http://academy.marinehealth.asia/Images/cert/cert-ru.jpg'></div><div style = 'position:absolute;right:120px;top:820px;color:#2b529f;font-size:" + fontsize + ";text-align:end'>" + Lname + " <br>" + Fname + " </div><div style='position: absolute; right: 150px; top: 1875px;color:#2b529f;font-size:50px;'>" + DateTime.Now.AddHours(6).ToString("dd.MM.yyyy") + "</div></body></html>";


                    string html = "<html> <head><meta http-equiv=Content-Type content='text/html; charset=UTF-8'><style type='text/css'></style><script type = 'text/javascript' src = 'a8535914-f191-11e9-9d71-0cc47a792c0a_id_a8535914-f191-11e9-9d71-0cc47a792c0a_files/wz_jsgraphics.js'></script></head><body style='font-family: Geneva, Arial, Helvetica, sans-serif'><div style = 'position:absolute;left:50%;margin-left:-1462px;top:0px;width:2924px;height:2075px;border-style:outset;overflow:hidden'> <div style = 'position:absolute;left:0px;top:0px' ><img src='http://academy.marinehealth.asia/Images/cert/certificate-v3.jpg'></div><div style='position:absolute;left:" + leftPosition + "px;top:1030px;color:#000;font-size:" + fontSize + "px;font-weight:900'>" + Lname + " " + Fname + "</div><div style='position: absolute; left:1350px;top:1770px;color:#717171;font-size:45px;font-weight:900'>" + DateTime.Now.AddHours(6).ToString("dd.MM.yyyy") + "</div></body></html>";

                    //string html = "<html><head><meta http-equiv=Content-Type content='text/html; charset=UTF-8'><meta name='viewport' content='width = device - width, minimum - scale = 0.1' ></head><body style='margin: 0px; background: #0e0e0e;font-family: Geneva, Arial, Helvetica, sans-serif;'><img style='-webkit-user-select: none;margin: auto;display:table' src='http://academy.marinehealth.asia/Images/cert/certificate-v3.jpg' width='1320' height='930'><div style='position:absolute;left:" + leftPosition + "px;top:480px;color:#000000;font-size:" + fontSize + "px'>Шарипжанов Айдар</div><div style='position:absolute; left:1020px; top:795px; color:#615f60d9; font-size:17px; font-weight:900'>04.11.2020</div></body></html>";


                    var img = htmlToImageConv.GenerateImage(html, ImageFormat.Jpeg);





                    imageName = "~/Images/UserCertificate/" + userinfo.Fname + "_" + "(certificate_" + userinfo.Id + ")" + ".jpg";

                    string path = HttpContext.Current.Server.MapPath(imageName);
                    System.IO.File.WriteAllBytes(path, img);
                });


                Users_Certificates UserCerNew = new Users_Certificates();
                UserCerNew.CertificateURL = imageName;
                UserCerNew.PublicCert = true;
                UserCerNew.UserId = userinfo.Id;
                UserCerNew.TypeCert = 3;
                UserCerNew.MakeDate = DateTime.Now.AddHours(6).Date;
                db.Users_Certificates.Add(UserCerNew);
                await db.SaveChangesAsync();

                result = imageName;
                

            }
            catch
            { }
            return result;


        }


        public static async Task<bool> markTrainingComplete(string UserName)
        {

            bool sts = false;
            try
            {

                string url = "https://my.marinehealth.asia/api/proxy/userManage/markTrainingCompleted?email=" + UserName;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("AuthToken", "99acbcede1554b0da47a9be1c43b09A145F43DCDE848169BEA5697859DD16A0acb303dec7c6544d39690e6e078157b05");
                var response = await client.PostAsync(url, null);
                string result = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode.ToString() == "OK")
                {

                    sts = true;

                }




            }
            catch { }
            return sts;

        }


    }
}
