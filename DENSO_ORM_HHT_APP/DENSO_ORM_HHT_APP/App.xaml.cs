using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Android.Content;
using Android.Widget;
using Matcha.BackgroundService;
using DENSO_ORM_HHT_APP.CommonClass;
using System.Threading.Tasks;
using Android.App;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DENSO_ORM_HHT_APP
{
    public partial class App 
    {
        public App()
        {
            InitializeComponent();
            CommonClass.CommonVariables.Path  = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Denso_ORM";
            
            if (Directory.Exists(CommonClass.CommonVariables.Path))
            {
                if (!File.Exists(CommonClass.CommonVariables.Path + "/Serivce.txt"))
                    File.Create(CommonClass.CommonVariables.Path + "/Serivce.txt");
                else
                {
                    StreamReader sw = new StreamReader(CommonClass.CommonVariables.Path + "/Serivce.txt");
                    CommonClass.CommonVariables.URL = sw.ReadToEnd();
                    sw.Close();
                    MainPage = new Pages.Login();

                }
            }
            else
            {
                Directory.CreateDirectory(CommonClass.CommonVariables.Path );
                File.Create(CommonClass.CommonVariables.Path + "/Serivce.txt");
            }
        }
      
        protected override void OnStart()
        {
           // Toast.MakeText(Android.App.Application.Context, "Start", ToastLength.Long).Show();

            CommonClass.CommonVariables.SleepMode = false;

            //Task.Run(() =>
            //{
            //    Pages.Kanban_Progress onj = new Pages.Kanban_Progress();
            //    onj.ConfirmMessage();
            //    //Add your code here, it might looks like:
            //   // CheckDatabase();
            //    //MakeAnUpdateDependingOnDatabase();
            //});
            // BackgroundAggregatorService.StartBackgroundService();

            // Handle when your app starts
            //TimeSpan ts = (DateTime.UtcNow - new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            //long millis = (long)ts.TotalMilliseconds;

            // Intent intentAlarm = new Intent(Android.App.Application.Context, typeof(ToastBroadcast));
            //AlarmManager alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);
            //int interval = 100;

            //   alarmManager.SetRepeating(AlarmType.ElapsedRealtime, 100, interval, PendingIntent.GetBroadcast(Android.App.Application.Context, 0, intentAlarm, PendingIntentFlags.UpdateCurrent));

        }




        protected override void OnSleep()
        {
         //   Toast.MakeText(Android.App.Application.Context, "Sleep", ToastLength.Long).Show();
            CommonClass.CommonVariables.SleepMode = true;
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
          //  Toast.MakeText(Android.App.Application.Context, "Resume", ToastLength.Long).Show();

            CommonClass.CommonVariables.SleepMode = false;

            // Handle when your app resumes
        }
    }
}
