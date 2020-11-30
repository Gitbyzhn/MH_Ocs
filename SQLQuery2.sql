/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/

delete
  FROM [DB_Mhealth_OCs].[dbo].[Training_EyeV] Where UserId=3898

  delete
  FROM [DB_Mhealth_OCs].[dbo].[Training_LikeV] Where UserId=3898

  delete
  FROM [DB_Mhealth_OCs].[dbo].[Webinar_EyeV] Where UserId=3898

  delete
  FROM [DB_Mhealth_OCs].[dbo].[Webinar_LikeV] Where UserId=3898

delete
  FROM [DB_Mhealth_OCs].[dbo].[EyeV] Where UserId=3898

delete
  FROM [DB_Mhealth_OCs].[dbo].[Isbranni] Where UserId=3898

delete
  FROM [DB_Mhealth_OCs].[dbo].[LessonVideoTime] Where UserId=3898

delete
  FROM [DB_Mhealth_OCs].[dbo].[LikeV] Where UserId=3898

delete
  FROM [DB_Mhealth_OCs].[dbo].[Task_First] Where UserId=3898

delete
  FROM [DB_Mhealth_OCs].[dbo].[UserTaskCheck] Where UserId=3898

delete
  FROM [DB_Mhealth_OCs].[dbo].[ValitOS] Where UserId=3898


delete
  FROM [DB_Mhealth_OCs].[dbo].[Users_Certificates] Where UserId=3898

delete
  FROM [DB_Mhealth_OCs].[dbo].[JVLO] Where UserName='neko.stein@gmail.com'


delete
  FROM [DB_Mhealth_OCs].[dbo].[UT] Where UserName='neko.stein@gmail.com'



delete
  FROM [DB_Mhealth_OCs].[dbo].[AspNetUsers] Where UserName='neko.stein@gmail.com'


delete
  FROM [DB_Mhealth_OCs].[dbo].[UserInfo] Where UserName='neko.stein@gmail.com'