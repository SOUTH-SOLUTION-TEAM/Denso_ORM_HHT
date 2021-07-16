using Android.Widget;
using DENSO_ORM_HHT_APP.CommonClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DENSO_ORM_HHT_APP.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
        CommonClass.SoundPlay obj = DependencyService.Get<CommonClass.SoundPlay>();
        public MainPage ()
		{
			InitializeComponent ();
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }
        CommonClass.iService_Method objsc = DependencyService.Get<CommonClass.iService_Method>();
        DataSet obj_DS = new DataSet();
        INotificationManager notificationManager;
        int notificationNumber = 0;
        string LineName = "";
        string Station = "";
        bool Flag1 = true;
        bool Flag2 = true;
        bool Flag3 = false;
        //void ShowNotification(string title, string message)
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        if (message != null)
        //        {
        //            Flag1 = false;
        //            CommonVariables.Flag3 = true;
        //            if (CommonClass.CommonVariables.SleepMode == false)
        //            {

        //                bool Result = await DisplayAlert("Alter!", message, "Accept", "Cancel");
        //                if (Result)
        //                {
        //                    if (Flag1 == false)
        //                    {
        //                        DataSet obj_ds = objsc.Kanban_Progress(CommonClass.CommonVariables.LineGroup, CommonClass.CommonVariables.LineName,  "", "", "AndonCallUpdate", "", "", "", "","");
        //                        obj.StopSound();
        //                        Flag1 = true;
        //                        CommonClass.CommonVariables.SleepMode = false;
        //                        CommonVariables.Flag3 = false;
        //                    }
        //                }
        //                else
        //                {
        //                    Flag1 = true;
        //                    obj.StopSound();
        //                    CommonVariables.Flag3 = false;
        //                    CommonClass.CommonVariables.SleepMode = false;

        //                }
        //            }
        //            else
        //                obj.StopSound();

        //        }
        //        else
        //            obj.StopSound();
        //    });
        //}
        //public async void ShowDateTime()
        //{
        //    bool Flag = true;
        //    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        //    {
        //        //if (CommonClass.CommonVariables.ShiftName != "")
        //        //{
        //            if (Flag1 == true)
        //            {
        //                if (CommonVariables.Flag3 == false && CommonVariables.PageName == "MainPage")
        //                {
        //                    DataSet obj_ds = objsc.Kanban_Progress(CommonVariables.LineGroup, CommonVariables.LineName, "", "", "AndonCall", "", "", "", "","");
        //                    if (obj_ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        Flag = false;
        //                        CommonVariables.Flag3 = true;
        //                        ConfirmMessage();
        //                        obj.PlaySound("Error");
        //                    }
        //                    else
        //                        obj.StopSound();
        //                }
        //                //else
        //                //    obj.StopSound();
        //           // }
        //            //else
        //            //    obj.StopSound();


        //        }

        //        return Flag;
        //    });

        //}

        //public void ConfirmMessage()
        //{
        //    Device.BeginInvokeOnMainThread(async () => {
        //        if (CommonClass.CommonVariables.SleepMode == true)
        //            notificationManager.ScheduleNotification("PART FEADING", "PLEASE LOAD PART TO MACHINE");

        //       // obj.PlaySound("Error");
        //        bool Result = await App.Current.MainPage.DisplayAlert("Alter!", "PLEASE LOAD PART TO MACHINE", "Accept", "Cancel");
        //        if (Result)
        //        {
        //            if (Flag1 == true)
        //            {
        //                DataSet obj_ds = objsc.Kanban_Progress(CommonVariables.LineGroup, CommonVariables.LineName,  "", "", "AndonCallUpdate", "", "", "", "","");
        //                CommonVariables.Flag3 = false;
        //                ShowDateTime();
        //                obj.StopSound();
        //            }
        //        }
        //        else
        //        {
        //            ShowDateTime();
        //            CommonVariables.Flag3 = false;
        //            obj.StopSound();
        //        }
        //    });
        //}
        void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (message != null)
                {
                    Flag1 = false;
                    CommonVariables.Flag3 = true;
                    if (CommonClass.CommonVariables.SleepMode == false)
                    {
                        bool Result = await DisplayAlert("Alter!", message, "Accept", "Cancel");
                        if (Result)
                        {
                            if (Flag1 == false)
                            {
                                DataSet obj_ds = objsc.Kanban_Progress(CommonClass.CommonVariables.LineGroup, LineName, "", "", "AndonCallUpdate", "", "", "", "", "");
                                obj.StopSound();
                                LineName = "";
                                Station = "";
                                Flag1 = true;
                                CommonVariables.Flag3 = false;
                                //CommonClass.CommonVariables.SleepMode = false;

                            }
                        }
                        else
                        {
                            Flag1 = true;
                            obj.StopSound();
                            CommonVariables.Flag3 = false;
                            // CommonClass.CommonVariables.SleepMode = false;

                        }
                    }
                    else
                        obj.StopSound();

                }
                else
                    obj.StopSound();
            });
        }
        public async void ShowDateTime()
        {
            bool Flag = true;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (CommonVariables.Flag3 == false && CommonVariables.PageName == "MainPage")
                {
                    //if (CommonClass.CommonVariables.ShiftName != "")
                    //{
                    if (Flag1 == true)
                    {
                        for (int i = 0; i < CommonVariables.dt.Rows.Count; i++)
                        {
                            DataSet obj_ds = objsc.Kanban_Progress(CommonVariables.LineGroup, CommonVariables.dt.Rows[i]["LineName"].ToString(), "", "", "AndonCall", "", "", "", "", "");
                            if (obj_ds.Tables[0].Rows.Count > 0)
                            {
                                Flag = false;
                                CommonVariables.Flag3 = true;
                                LineName = CommonVariables.dt.Rows[i]["LineName"].ToString();
                                Station = obj_ds.Tables[0].Rows[0]["Station"].ToString();
                                ConfirmMessage();
                                // obj.PlaySound("Error");
                            }
                            else
                                obj.StopSound();
                        }
                        // }
                        //else
                        //    obj.StopSound();
                    }
                    //else
                    //  obj.StopSound();
                }
                return Flag;
            });
        }

        public void ConfirmMessage()
        {
            Device.BeginInvokeOnMainThread(async () => {


                if (CommonClass.CommonVariables.SleepMode == true)
                    notificationManager.ScheduleNotification("PART FEADING", "PLEASE LOAD PART TO MACHINE-LINE NAME = " + LineName + " AND STATION = " + Station);

                obj.PlaySound("Error");
                bool Result = await App.Current.MainPage.DisplayAlert("Alter!", "PLEASE LOAD PART TO MACHINE-LINE NAME = " + LineName + " AND STATION = " + Station, "Accept", "Cancel");
                if (Result)
                {
                    if (Flag1 == true)
                    {
                        DataSet obj_ds = objsc.Kanban_Progress(CommonVariables.LineGroup, LineName, "", "", "AndonCallUpdate", "", "", "", "", "");
                        CommonVariables.Flag3 = false;
                        LineName = "";
                        Station = "";
                        ShowDateTime();
                        obj.StopSound();
                    }
                }
                else
                {
                    ShowDateTime();
                    CommonVariables.Flag3 = false;
                    obj.StopSound();
                }
            });
        }
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.Current.MainPage = new Pages.Kanban_Progress();
            }
            catch (Exception ex)
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
                obj.StopSound();
            }
        }

        private void ImgBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                //obj.StopSound();
                CommonVariables.PageName = "Login";
                CommonVariables.Flag3 = false;
                App.Current.MainPage = new Pages.Login();
            }
            catch (Exception ex)
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
                obj.StopSound();
            }
        }

        private void ContentPage_LayoutChanged(object sender, EventArgs e)
        {
            try
            {
                ShowDateTime();
                CommonVariables.PageName = "MainPage";
                CommonVariables.Flag3 = false;
            }
            catch (Exception ex)
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
                obj.StopSound();
            }
           
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.Current.MainPage = new Pages.LineAsign();
            }
            catch (Exception ex)
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
                obj.StopSound();
            }
        }
    }
}