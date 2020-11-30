using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MH_Ocs.Models
{

    public class GetUser
    {


        public static UserInfo Info(string UserName)
        {
            Entities db = new Entities();
            UserInfo userinfo = db.UserInfoes.Where(e => e.UserName == UserName).FirstOrDefault();
            return userinfo;
        }




        public static TextLayout language(string language)
        {

            TextLayout TextWebSite = new TextLayout();

            switch (language)
            {
                case "kz":

                    TextWebSite.Modules = "Модульдер";
                    TextWebSite.Lessons = "Барлық видеосабақтар";
                    TextWebSite.Tasks = "Тапсырмалар";
                    TextWebSite.Favorite = "Таңдаулылар";
                    TextWebSite.UsefulServices = "Пайдалы қызметтер";
                    TextWebSite.Training = "Тренинг";
                    TextWebSite.Webinar = "Вебинар";
                    TextWebSite.MyProgress = "Менің прогрессім";
                    TextWebSite.Passed = "Барлық видеосабақтан өтілгені";
                    TextWebSite.Performance = "Өтілген видеосабақтан үлгерімі";
                    TextWebSite.Reserved = "Барлық құқықтар сақталған";
                    TextWebSite.Logout = "Шығу";
                    TextWebSite.Certificate = "Сертификат";
                 


                    break;

                case "uz":

                    TextWebSite.Modules = "Moduli";
                    TextWebSite.Lessons = "Barcha video darslar";
                    TextWebSite.Tasks = "Vazifalar";
                    TextWebSite.Favorite = "Sevimli";
                    TextWebSite.UsefulServices = "Foydali xizmatlar";
                    TextWebSite.Training = "Training";
                    TextWebSite.Webinar = "Webinar";
                    TextWebSite.MyProgress = "Mening taraqqiyotim";
                    TextWebSite.Passed = "Barcha darslardan";
                    TextWebSite.Performance = "O'quv faoliyati";
                    TextWebSite.Reserved = "Barcha huquqlar himoyalangan";
                    TextWebSite.Logout = "Сhiqish";
                    TextWebSite.Certificate = "Sertifikat";
                    break;

                case "kr":

                    TextWebSite.Modules = "Модулдар";
                    TextWebSite.Lessons = "Бардык видео сабактар";
                    TextWebSite.Tasks = "Тапшырмалар";
                    TextWebSite.Favorite = "Сүйүктүүлөр";
                    TextWebSite.UsefulServices = "Пайдалуу кызмат";
                    TextWebSite.Training = "Тренинг";
                    TextWebSite.Webinar = "Вебинар";
                    TextWebSite.MyProgress = "Менин прогресс";
                    TextWebSite.Passed = "Бардык видео дарстар үзүндү";
                    TextWebSite.Performance = "Видео сабактарындагы прогресс";
                    TextWebSite.Reserved = "Бардык укуктар корголгон";
                    TextWebSite.Logout = "Чыгуу";
                    TextWebSite.Certificate = "Kүбөлүк";

                    break;


                case "tr":

                    TextWebSite.Modules = "Modüller";
                    TextWebSite.Lessons = "Tüm video eğiticileri";
                    TextWebSite.Tasks = "Atamaları";
                    TextWebSite.Favorite = "Favoriler";
                    TextWebSite.UsefulServices = "Faydalı Hizmetler";
                    TextWebSite.Training = "Training";
                    TextWebSite.Webinar = "Webinar";
                    TextWebSite.MyProgress = "İlerlemem";
                    TextWebSite.Passed = "Tüm derslerin";
                    TextWebSite.Performance = "O'quv faoliyati";
                    TextWebSite.Reserved = "Tüm hakları saklıdır.";
                    TextWebSite.Logout = "Çıkış Yap";
                    TextWebSite.Certificate = "Sertifika";
                    break;

                case "en":

                    TextWebSite.Modules = "Modules";
                    TextWebSite.Lessons = "All video tutorials";
                    TextWebSite.Tasks = "Tasks";
                    TextWebSite.Favorite = "Favorite";
                    TextWebSite.UsefulServices = "Useful Services";
                    TextWebSite.Training = "Training";
                    TextWebSite.Webinar = "Webinar";
                    TextWebSite.MyProgress = "My progress";
                    TextWebSite.Passed = "Of all the lessons";
                    TextWebSite.Performance = "Progress";
                    TextWebSite.Reserved = "All rights reserved";
                    TextWebSite.Logout = "Logout";
                    TextWebSite.Certificate = "Certificate";
                    break;


                case "ru":

                    TextWebSite.Modules = "Модули";
                    TextWebSite.Lessons = "Все видеоуроки";
                    TextWebSite.Tasks = "Задание";
                    TextWebSite.Favorite = "Избранное";
                    TextWebSite.UsefulServices = "Полезные сервисы";
                    TextWebSite.Training = "Тренинги";
                    TextWebSite.Webinar = "Вебинары";
                    TextWebSite.MyProgress = "Мой прогресс";
                    TextWebSite.Passed = "Из всех уроков";
                    TextWebSite.Performance = "Успеваемость";
                    TextWebSite.Reserved = "Все права защищены";
                    TextWebSite.Logout = "Выйти";
                    TextWebSite.Certificate = "Сертификат";
                    break;

            }



            return TextWebSite;

        }

       
        public static ModulNotReachСs TextModulNotReach(string language)
        {

            ModulNotReachСs TextWebSite = new ModulNotReachСs();

            switch (language)
            {
                case "kz":
                    TextWebSite.TextManager = "Сіз 'Менеджер' деңгейіне жеткен жоқсыз";
                    TextWebSite.TextBronzemanager = "Сіз 'Қола Менеджері' деңгейіне жеткен жоқсыз";
                    TextWebSite.TextSilverManager = "Сіз 'Күміс Менеджері' деңгейіне жеткен жоқсыз";
                    TextWebSite.TextGoldmanager = "Сіз 'Алтын Менеджері' деңгейіне жеткен жоқсыз";
                    TextWebSite.TextPlatinumManager = "Сіз 'Платина Менеджері' деңгейіне жеткен жоқсыз";
                    TextWebSite.TextDerictor = "Сіз 'Директор' деңгейіне жеткен жоқсыз";
                    break;
                case "ru":
                    TextWebSite.TextManager = "Вы не достигли уровня 'Менеджер'";
                    TextWebSite.TextBronzemanager = "Вы не достигли уровня 'Бронзовый Менеджер'";
                    TextWebSite.TextSilverManager = "Вы не достигли уровня 'Серебряный Менеджер'";
                    TextWebSite.TextGoldmanager = "Вы не достигли уровня 'Золотой Менеджер'";
                    TextWebSite.TextPlatinumManager = "Вы не достигли уровня 'Платиновый Менеджер'";
                    TextWebSite.TextDerictor = "Вы не достигли уровня 'Директор'";
                    break;
                case "uz":
                    TextWebSite.TextManager = "Siz 'Menejer' darajasiga yetmadingiz";
                    TextWebSite.TextBronzemanager = "Siz  'Bronza Menejeri' darajasiga etishmadingiz";
                    TextWebSite.TextSilverManager = "Siz 'Silver Manager' darajasiga erishmadingiz";
                    TextWebSite.TextGoldmanager = "Siz 'Gold Manager' darajasiga etmadingiz";
                    TextWebSite.TextPlatinumManager = "Siz  'Platinum Manager' darajasiga yetmadingiz ";
                     TextWebSite.TextDerictor = "Siz 'Direktor' darajasiga etishmadingiz";
                    break;
                case "kr":
                    TextWebSite.TextManager = "Сиз 'Менеджер' деңгээлине жеткен жоксуз";
                    TextWebSite.TextBronzemanager = "Сиз 'Қола Менеджері' деңгээлине жеткен жоксуз";
                    TextWebSite.TextSilverManager = "Сиз 'Күміс Менеджері' деңгээлине жеткен жоксуз";
                    TextWebSite.TextGoldmanager = "Сиз 'Алтын Менеджері' деңгээлине жеткен жоксуз";
                    TextWebSite.TextPlatinumManager = "Сіз 'Платина Менеджері' деңгээлине жеткен жоксуз";
                    TextWebSite.TextDerictor = "Сиз 'Директор' деңгээлине жеткен жоксуз";
                    break;
                case "tr":
                    TextWebSite.TextManager = "'Yönetici' seviyesine ulaşmadınız";
                    TextWebSite.TextBronzemanager = "'Bronz Yönetici' seviyesine ulaşmadınız";
                    TextWebSite.TextSilverManager = "'Gümüş Yönetici' seviyesine ulaşmadınız";
                    TextWebSite.TextGoldmanager = "'Altın Yönetici' seviyesine ulaşmadınız";
                    TextWebSite.TextPlatinumManager = "'Platin Yönetici' seviyesine ulaşmadınız";
                    TextWebSite.TextDerictor = "'Yönetmen' seviyesine ulaşmadınız";
                    break;
                case "en":
                    TextWebSite.TextManager = "You have not reached the 'Manager' level";
                    TextWebSite.TextBronzemanager = "You have not reached the 'Bronze Manager' level";
                    TextWebSite.TextSilverManager = "You have not reached the 'Silver Manager' level";
                    TextWebSite.TextGoldmanager = "You have not reached the 'Gold Manager' level";
                    TextWebSite.TextPlatinumManager = "You have not reached the 'Platinum Manager' level";
                    TextWebSite.TextDerictor = "You have not reached the 'Director' level";
                    break;
            }



            return TextWebSite;

        }

        public static TextVideosСs TextVideos(string language)
        {

            TextVideosСs TextWebSite = new TextVideosСs();

            switch (language)
            {
                case "kz":


                    TextWebSite.Title = "Барлық видеодәрістер";

                    TextWebSite.Lesson = "Бұл видеосабаққа өту үшін, алдыңғы видеосабақтан өтуіңіз керек.";
                    TextWebSite.Task = "Бұл видеосабаққа өту үшін, алдыңғы видеосабақтағы тапсырманы орындау керек.";
                    TextWebSite.Test = "Бұл видеосабаққа өту үшін, алдыңғы видеосабақтағы тестті тапсырып, жақсы баға алу керек.";

                    TextWebSite.PassedLesson = "Видеосабаққа өту";
                    TextWebSite.PassedTask = "Тапсырманы орындау";
                    TextWebSite.PassedTest = "Тестті тапсыру";


                    break;


                case "ru":


                    TextWebSite.Title = "Все видеоуроки";

                    TextWebSite.Lesson = "Чтобы получить доступ к данному видеоуроку, вам необходимо пройти предыдущий урок.";
                    TextWebSite.Task = "Чтобы получить доступ к данному видеоуроку, вам необходимо выполнить задание из предыдущего видеоурока.";
                    TextWebSite.Test = "Чтобы получить доступ к данному видеоуроку, вам необходимо пройти тест из предыдущего видеоурока и получить хорошую оценку.";

                    TextWebSite.PassedLesson = "Пройти видеоурок";
                    TextWebSite.PassedTask = "Пройти задание";
                    TextWebSite.PassedTest = "Пройти тест";


                    break;


                case "uz":


                    TextWebSite.Title = "Barcha video darslar";

                    TextWebSite.Lesson = "Ushbu video darsga kirish uchun siz avvalgi darsga o'tishingiz kerak.";
                    TextWebSite.Task = "Ushbu video darsiga kirish uchun avvalgi video darsligidagi vazifani bajarish kerak.";
                    TextWebSite.Test = "Ushbu video darsga kirish uchun avvalgi video darslikdan testni topshirish va yaxshi baho olish kerak.";

                    TextWebSite.PassedLesson = "Video darstan o'tish";
                    TextWebSite.PassedTask = "Vazifadan o'tish";
                    TextWebSite.PassedTest = "Testdan o'tish";

                    break;


                case "kr":


                    TextWebSite.Title = "Бардык видео сабак";

                    TextWebSite.Lesson = "Бул видео сабактан өтүү үчүн, буга чейинки видео сабактан өтүшүңүз керек.";
                    TextWebSite.Task = "Бул видео сабакка өтүү үчүн, мурунку видео сабактан тапшырманы аткарышыңыз керек.";
                    TextWebSite.Test = "Бул видео сабактан өтүү үчүн, мурунку видео сабакта тесттен өтүп, жакшы баа алуу керек.";

                    TextWebSite.PassedLesson = "Видео сабак өтүү";
                    TextWebSite.PassedTask = "Тапшырманы аткаруу";
                    TextWebSite.PassedTest = "Текшерүү өтүү";


                    break;


                case "tr":


                    TextWebSite.Title = "Tüm video eğiticileri";

                    TextWebSite.Lesson = "Bu eğitim videosuna erişmek için, önceki öğreticiyi tamamlamanız gerekir.";
                    TextWebSite.Task = "Bu eğitim videosuna erişmek için, önceki eğitim videosundaki görevi tamamlamanız gerekir.";
                    TextWebSite.Test = "Bu eğitim videosuna erişmek için, önceki eğitim videosundaki testi geçmeniz ve iyi bir not almanız gerekir.";

                    TextWebSite.PassedLesson = "Eğitim videosu alın";
                    TextWebSite.PassedTask = "Görevi tamamla";
                    TextWebSite.PassedTest = "Teste katılın";


                    break;


                case "en":


                    TextWebSite.Title = "All video tutorials";

                    TextWebSite.Lesson = "To access this video tutorial, you need to complete the previous tutorial.";
                    TextWebSite.Task = "To access this video tutorial, you need to complete the task from the previous video tutorial.";
                    TextWebSite.Test = "To access this video tutorial, you need to pass the test from the previous video tutorial and get a good grade.";

                    TextWebSite.PassedLesson = "Take a video tutorial";
                    TextWebSite.PassedTask = "Perform the task";
                    TextWebSite.PassedTest = "Take the test";


                    break;
            }



            return TextWebSite;

        }



        public static TextTestsСs TextTests(string language)
        {

            TextTestsСs TextWebSite = new TextTestsСs();

            switch (language)
            {
                case "kz":


                    TextWebSite.Notbad = "Жаман емес, бірақ жұмыс жасау керек.";
                    TextWebSite.Good = "Жақсы, осылай жалғыстыра беріңіз.";
                    TextWebSite.Excellent = "Тамаша, осылай жалғыстыра беріңіз.";
                    TextWebSite.TotalText = "Жинаған баллыңыз: ";
                    break;


                case "ru":

                    TextWebSite.Notbad = "Неплохо, но есть над чем поработать.";
                    TextWebSite.Good = "Хорошо, продолжай в том же духе.";
                    TextWebSite.Excellent = "Отлично, так держать.";
                    TextWebSite.TotalText = "Вы набрали: ";
                    break;

                case "uz":

                    TextWebSite.Notbad = "Yomon emas, lekin qilish kerak bo'lgan ishlar ham bor.";
                    TextWebSite.Good = "Xo'sh, ishni davom ettiring.";
                    TextWebSite.Excellent = "Ajoyib, uni ushlab turing.";
                    TextWebSite.TotalText = "Siz to'pladingiz: ";
                    break;


                case "kr":

                    TextWebSite.Notbad = "Жаман эмес, бирок муну менен иши жок.";
                    TextWebSite.Good = "Ооба, жакшы иштей берет.";
                    TextWebSite.Excellent = "Абдан жакшы, аны сактап калат.";
                    TextWebSite.TotalText = "Сиз топтодуңуз: ";
                    break;

                case "tr":

                    TextWebSite.Notbad = "Fena değil, ama yapacak işler var.";
                    TextWebSite.Good = "İyi işlere devam edin.";
                    TextWebSite.Excellent = "Harika, devam et.";
                    TextWebSite.TotalText = "Puan kazandınız: ";
                    break;

                case "en":

                    TextWebSite.Notbad = "Not bad, but there is work to do.";
                    TextWebSite.Good = "Well, keep up the good work.";
                    TextWebSite.Excellent = "Great, keep it up.";
                    TextWebSite.TotalText = "You scored: ";
                    break;
            }



            return TextWebSite;

        }



        public static TextTasktendreamsCs TextTasktendreams(string language)
        {

            TextTasktendreamsCs TextWebSite = new TextTasktendreamsCs();

            switch (language)
            {
                case "kz":


                    TextWebSite.Title = "Тапсырмалар";
                    TextWebSite.Task_First_Title = "Осы бизнесте қол жеткізгіңіз келетін 10 арман туралы жазыңыз.";
                    TextWebSite.SaveBtn = "Сақтау";


                    break;


                case "ru":


                    TextWebSite.Title = "Задание";
                    TextWebSite.Task_First_Title = "Запишите 10 ваших конкретных мечты, которые Вы хотите достичь в этом бизнесе.";
                    TextWebSite.SaveBtn = "Сохранить";

                    break;

                case "uz":


                    TextWebSite.Title = "Missiyalar";
                    TextWebSite.Task_First_Title = "Ushbu biznesda erishmoqchi bo'lgan aniq 10 ta orzularingizni yozing.";
                    TextWebSite.SaveBtn = "Saqlash";

                    break;

                case "kr":


                    TextWebSite.Title = "Тапшырмалар";
                    TextWebSite.Task_First_Title = "Ушул бизнесте өзүңүз каалаган 10 кыялыңызга жазыңыз.";
                    TextWebSite.SaveBtn = "Сактоо";

                    break;

                case "tr":


                    TextWebSite.Title = "Atamaları";
                    TextWebSite.Task_First_Title = "Bu işte başarmak istediğiniz 10 hayalinizi yazın.";
                    TextWebSite.SaveBtn = "Tutmak";

                    break;

                case "en":


                    TextWebSite.Title = "Tasks";
                    TextWebSite.Task_First_Title = "Write down 10 of your specific dreams that you want to achieve in this business.";
                    TextWebSite.SaveBtn = "Save";

                    break;
            }



            return TextWebSite;

        }



        public static VideoLessonCs VideoLesson(string language)
        {

            VideoLessonCs TextWebSite = new VideoLessonCs();

            switch (language)
            {
                case "kz":


                    TextWebSite.Next = "Келесі";
                    TextWebSite.Previous = "Алдыңғы";
                    TextWebSite.toMain = "Бастапқы бет";


                    break;


                case "ru":


                    TextWebSite.Next = "Следующий";
                    TextWebSite.Previous = "Предыдущий";
                    TextWebSite.toMain = "На главную";


                    break;

                case "uz":


                    TextWebSite.Next = "Keyingi";
                    TextWebSite.Previous = "Oldingi";
                    TextWebSite.toMain = "Asosiyga";


                    break;

                case "kr":


                    TextWebSite.Next = "Кийинки";
                    TextWebSite.Previous = "Мурунку";
                    TextWebSite.toMain = "Негизги";


                    break;

                case "tr":


                    TextWebSite.Next = "Aşağıdaki";
                    TextWebSite.Previous = "önceki";
                    TextWebSite.toMain = "Ana";

                    break;

                case "en":


                    TextWebSite.Next = "Tasks";
                    TextWebSite.Previous = "Previous";
                    TextWebSite.toMain = "To Main";
                    break;
            }



            return TextWebSite;

        }

        public static UserFavoritesCs UserFavorites(string language)
        {

            UserFavoritesCs TextWebSite = new UserFavoritesCs();

            switch (language)
            {
                case "kz":
                    TextWebSite.Title = "Таңдаулылар";
                    break;
                case "ru":
                    TextWebSite.Title = "Избранное";
                    break;
                case "uz":
                    TextWebSite.Title = "Sevimlilar";
                    break;
                case "kr":
                    TextWebSite.Title = "Сүйүктүүлөр";
                    break;
                case "tr":
                    TextWebSite.Title = "Favoriler";
                    break;
                case "en":
                    TextWebSite.Title = "Favorites";
                    break;
            }



            return TextWebSite;

        }





        public partial class VideoLessonCs
        {

            public string Next { get; set; }

            public string Previous { get; set; }

            public string toMain { get; set; }

        }

        public partial class TextTasktendreamsCs
        {

            public string Title { get; set; }

            public string Task_First_Title { get; set; }
            public string SaveBtn { get; set; }


        }



        public partial class TextLayout
        {

            public string Modules { get; set; }
            public string Lessons { get; set; }
            public string Tasks { get; set; }
            public string Favorite { get; set; }
            public string UsefulServices { get; set; }

            public string Training { get; set; }

            public string Webinar { get; set; }

            public string MyProgress { get; set; }

            public string Passed { get; set; }

            public string Performance { get; set; }
            public string Reserved { get; set; }

            public string Logout { get; set; }

            public string Certificate { get; set; }

        }



        public partial class TextVideosСs
        {

            public string Title { get; set; }

            public string Test { get; set; }
            public string Task { get; set; }
            public string Lesson { get; set; }

            public string PassedTest { get; set; }
            public string PassedTask { get; set; }
            public string PassedLesson { get; set; }

        }



        public partial class TextTestsСs
        {

            public string Notbad { get; set; }
            public string Good { get; set; }
            public string Excellent { get; set; }
            public string TotalText { get; set; }


        }

     

    }

    public class ModuleCs
    {
        public int ModuleId { get; set; }
        public int XId { get; set; }
        public string Image { get; set; }
        public string NameKZ { get; set; }
        public string NameRU { get; set; }
        public string NameUZ { get; set; }
        public string NameKR { get; set; }
        public string NameEN { get; set; }
        public string NameTR { get; set; }

        public bool publish { get; set; }
        public int LevelId { get; set; }



    }

    public class TrainingCs
    {
        public int ModuleId { get; set; }
        public int XId { get; set; }
        public string Image { get; set; }
        public string NameKZ { get; set; }
        public string NameRU { get; set; }
        public string NameUZ { get; set; }
        public string NameKR { get; set; }
        public string NameEN { get; set; }
        public string NameTR { get; set; }

        public bool publish { get; set; }
 
    }

    public class VideoLsX {

        public double XId { get; set; }
        public string Name { get; set; }
        
    }

    public class UserFavoritesCs
    {
        public string Title { get; set; }
    }

    public class UserToken
    {

        public string Status { get; set; }
        public string Token { get; set; }

    }


    public class UserName
    {

        public string Name { get; set; }
   
    }



    public class MHUserInfo
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        //public object Patronymic { get; set; }
        public string ImageFileName { get; set; }

        public int? LevelId { get; set; }

        public List<Contact> Contacts { get; set; }


    }

    public class Contact
    {
        public int UserId { get; set; }

    }


    public class ModulNotReachСs {

        public string TextManager { get; set; }

        public string TextBronzemanager { get; set; }

        public string TextSilverManager { get; set; }

        public string TextGoldmanager { get; set; }

        public string TextPlatinumManager { get; set; }

        public string TextDerictor { get; set; }

 
    }


    public class UserProgress
    {

        public int OUK { get; set; }
        public int TBB { get; set; }

        public int NextV { get; set; }
        public JVLO JVLO { get; set; }
        public List<VideoXL> EnableVideoXLs { get; set; }
    }



    public class API_UserInfo
    {

        public string Name { get; set; }
        public string SureName { get; set; }
        public string Image { get; set; }
        public int OUK { get; set; }
        public int TBB { get; set; }
        
    }


    public partial class Modules_Property_Enable
    {
        public Modules_Property Modul_Property_IN { get; set; }

        public bool Enable { get; set; }
    }

}