using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Matcha.BackgroundService.Droid;
using Xamarin.Forms;
using Android.Content;
using DENSO_ORM_HHT_APP.CommonClass;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Android.Support.V4.Content;
using Android.Arch.Lifecycle;

namespace DENSO_ORM_HHT_APP.Droid
{
    [Activity(Label = "DENSO_ORM_HHT_APP", Icon = "@drawable/Qrcode", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static WebReference.Service1 objSR = new WebReference.Service1();
        string apkFile = "";
        string Version = "";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //string str=  System.Diagnostics.Process.GetCurrentProcess().ProcessName ;
            base.Window.RequestFeature(Android.Views.WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            // BackgroundAggregator.Init(this);
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
           // Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
          
           
            Task.Run(() =>
            {
                try
                {
                    CheckForUpdate();
                }
                catch (Exception ex)
                {

                }
            });
            objSR.Timeout = 100000;
            CreateNotificationFromIntent(Intent);
            LoadApplication(new App());
            objSR.Url = CommonClass.CommonVariables.URL;
            CommonClass.CommonVariables.Version = PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.Activities).VersionCode.ToString();


        }
        private void CheckForUpdate()
        {
            //#if DEBUG
            apkFile = GetFTPFile("ftp://172.28.41.246/SATO/DENSO_ORM.DENSO_ORM.apk", "VISITOR", "123456");
            //#else
            //  apkFile = GetFTPFile("ftp://172.28.41.246/SATO/DENSO_ORM.DENSO_ORM.apk", "VISITOR", "123456");

            //#endif
            var pckInfo = PackageManager.GetPackageArchiveInfo(apkFile, PackageInfoFlags.Activities);
            int CurVCode = PackageManager.GetPackageInfo(PackageName, PackageInfoFlags.Activities).VersionCode;
            //PackageManager.GetPackageInfo()
            int newVCode = pckInfo.VersionCode;
            if (newVCode != CurVCode)
            {
                AlertDialog.Builder dlg = new AlertDialog.Builder(this);
                dlg.SetMessage("New update found.Do you want to update?");
                dlg.SetPositiveButton("Yes", UpdateApplication);
                dlg.SetNegativeButton("No", CloseApp);
                RunOnUiThread(() => { dlg.Show(); });

            }
        }
        private void CloseApp(object sender, DialogClickEventArgs e)
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
        private void UpdateApplication(object sender, DialogClickEventArgs e)
        {

            //Intent intent = new Intent(Intent.ActionDelete);
            //intent.SetData(Android.Net.Uri.FromFile(. package:Com.android.DENSO_ORM.DENSO_ORM));
            //StartActivity(intent);
            //Uri packageURI = Uri("package:Com.android.DENSO_ORM.DENSO_ORM");
            //Intent uninstallIntent = new Intent(Intent.ACTION_DELETE, packageURI);
            //startActivity(uninstallIntent);

            Intent intent = new Intent(Intent.ActionInstallPackage);
            intent.SetDataAndType(Android.Net.Uri.FromFile(new Java.IO.File(apkFile)), "application/vnd.android.package-archive");
            intent.SetFlags(ActivityFlags.NewTask);
            StartActivity(intent);
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());

            //Intent promptInstall = new Intent(Intent.ActionView).SetDataAndType(Android.Net.Uri.FromFile(new Java.IO.File(apkFile)), "application/vnd.android.package-archive");
            ////Intent promptInstall = new Intent(Intent.ActionView).SetData(Android.Net.Uri.Parse("/sdcard/Download/Inspections.Inspections-Signed.apk")).SetType("application/vnd.android.package-archive");
            //promptInstall.AddFlags(ActivityFlags.NewTask);
            //StartActivity(promptInstall);

        }
        private string GetFTPFile(string path, string Username, string password)
        {

            string filePath = Android.OS.Environment.ExternalStorageDirectory + "/Denso_ORM/DENSO_ORM.DENSO_ORM.apk";
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(path);
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            req.UseBinary = true;
            req.UsePassive = false;
            req.KeepAlive = false;
            req.Timeout = System.Threading.Timeout.Infinite;
            req.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;
            req.Credentials = new NetworkCredential(Username, password);
            using (var ftpStream = req.GetResponse().GetResponseStream())
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ftpStream.CopyTo(fileStream);
                }
            }

            return filePath;
        }
        //void InitBroadcast()
        //{
        //    //TimeSpan ts = (DateTime.UtcNow - new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        //    //long millis = (long)ts.TotalMilliseconds;
        //    // android.intent.action.BOOT_COMPLETED
        //    //string intentAlarm = Intent.Action;
        //    Intent intentAlarm = new Intent(this, typeof(ToastBroadcast));
        //    AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);
        //    //int interval = 0;
        //    long interval = 5; //this is every hour, but you can do less
        //    long triggerTime = 1 + interval;
        //    //Device.StartTimer(TimeSpan.FromSeconds(5), () =>
        //    //{
        //    alarmManager.SetInexactRepeating(AlarmType.ElapsedRealtime, triggerTime, interval, PendingIntent.GetBroadcast(this, 0, intentAlarm, 0));
        //    //    return true;
        //    //});
        //    // alarmManager.;
        //    //PendingIntent pendingIntent = PendingIntent.getBroadcast(getBaseContext(), RQS_1, intent, 0);
        //    //      AlarmManager alarmManager = (AlarmManager)getSystemService(Context.ALARM_SERVICE);
        //    //      alarmManager.set(AlarmManager.RTC_WAKEUP, targetCal.getTimeInMillis(), pendingIntent);
        //}


        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.Extras.GetString(AndroidNotificationManager.TitleKey);
                string message = intent.Extras.GetString(AndroidNotificationManager.MessageKey);

                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
        }
    }  
}