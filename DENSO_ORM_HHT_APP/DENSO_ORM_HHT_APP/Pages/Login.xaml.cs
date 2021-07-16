using Android.Widget;
using DENSO_ORM_HHT_APP.CommonClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DENSO_ORM_HHT_APP.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        CommonClass.iService_Method objsc = DependencyService.Get<CommonClass.iService_Method>();
        CommonClass.SoundPlay obj = DependencyService.Get<CommonClass.SoundPlay>();
        INotificationManager notificationManager;
        string LineName = "";
        string Station = "";
        int notificationNumber = 0;
        bool Flag1 = true;
        bool Flag2 = true;
        bool Flag3 = false;
        public Login()
        {
            InitializeComponent();
            notificationManager = DependencyService.Get<INotificationManager>();
            if (File.Exists(CommonClass.CommonVariables.Path + "/LineGroupSelection.txt"))
            {
                StreamReader Sr = new StreamReader(CommonClass.CommonVariables.Path + "/LineGroupSelection.txt");
                string Data = Sr.ReadToEnd();
                Sr.Close();
                Data = Data.Replace("\n", "|");
                string[] data1 = Data.Split('|');
                if (data1.Length > 0)
                {
                    CommonClass.CommonVariables.LineGroup = data1[0];
                }
            }

            if (File.Exists(CommonClass.CommonVariables.Path + "/LineNameSelection.txt"))
            {
                StreamReader Sr1 = new StreamReader(CommonClass.CommonVariables.Path + "/LineNameSelection.txt");
                string Data1 = Sr1.ReadToEnd();
                Sr1.Close();
                Data1 = Data1.Replace("\n", "|");
                string[] data2 = Data1.Split('|');
                if (data2.Length > 0)
                {
                    CommonVariables.dt = new DataTable();
                    CommonVariables.dt.Columns.Add("LineName");
                    for (int i = 0; i < data2.Length; i++)
                    {
                        if (data2[i] != "")
                            CommonVariables.dt.Rows.Add(data2[i].ToString());
                    }
                }
                if (CommonVariables.dt.Rows.Count == 1)
                {
                    CommonClass.CommonVariables.LineName = data2[0].ToString();
                }
            }
            
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }
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
                                DataSet obj_ds = objsc.Kanban_Progress(CommonClass.CommonVariables.LineGroup, LineName,  "", "", "AndonCallUpdate", "", "", "", "", "");
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
                if (CommonVariables.Flag3 == false && CommonVariables.PageName == "Login")
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
                bool Result = await App.Current.MainPage.DisplayAlert("Alter!", "PLEASE LOAD PART TO MACHINE-LINE NAME = "+LineName +" AND STATION = "+ Station, "Accept", "Cancel");
                if (Result)
                {
                    if (Flag1 == true)
                    {
                        DataSet obj_ds = objsc.Kanban_Progress(CommonVariables.LineGroup, LineName, "", "", "AndonCallUpdate", "", "", "", "","");
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
        private void ContentPage_LayoutChanged(object sender, EventArgs e)
        {
            try
            {
                //DataSet obj_ds = objsc.Shift_Name(CommonVariables.LineGroup, CommonVariables.LineName);
                //if (obj_ds.Tables[0].Rows.Count > 0)
                //{
                //    // CommonClass.CommonMethod.FillComboBox(cmbChangeOver, obj_ds.Tables[0], "item");
                //    CommonClass.CommonVariables.ShiftName = obj_ds.Tables[0].Rows[0]["ShiftName"].ToString();
                //}
                CommonVariables.PageName = "Login";
                ShowDateTime();
                lblLogin.Text = "LOGIN - "+CommonClass.CommonVariables.Version;
                CommonVariables.Flag3 = false;
                txtUserID.Focus();
            }
            catch (Exception ex)
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
                obj.StopSound();
            }

        }
        private bool ControlValidation()
        {
            string str = txtUserID.Text;
            if (txtUserID.Text == "" || txtUserID.Text==(null))
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, "PLEASE ENTER USER ID", ToastLength.Long).Show();
                obj.StopSound();
                txtUserID.Focus();
                return false;
            }
            if (txtPassowrd.Text == "" || txtPassowrd.Text == (null))
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, "PLEASE ENTER PASSWORD", ToastLength.Long).Show();
                obj.StopSound();
                txtPassowrd.Focus();
                return false;
            }
            return true;
        }

        private void ValidateLogin()
        {
            if (ControlValidation())
            {
                string Result = objsc.ValidateLogin(txtUserID.Text, txtPassowrd.Text, "Login");
                if (Result.StartsWith("VALID CREDENTIAL"))
                {
                    string[] Data = Result.Split('+');

                    CommonClass.CommonVariables.Loggedin = txtUserID.Text;
                    CommonClass.CommonVariables.LoggedName = Data[1];
                    CommonClass.CommonVariables.Rights = Data[2];
                   // = null;
                    App.Current.MainPage = new Pages.MainPage();
                }
                else if (Result.StartsWith("INVALID CREDENTIAL"))
                {
                    obj.PlaySound("Error");
                    Thread.Sleep(3000);
                    Toast.MakeText(Android.App.Application.Context, "INVALID CREDENTIA", ToastLength.Long).Show();
                    obj.StopSound();
                    txtUserID.Focus();
                }
                else if (Result.StartsWith("INVALID PASSWORD"))
                {
                    obj.PlaySound("Error");
                    Thread.Sleep(3000);
                    Toast.MakeText(Android.App.Application.Context, "INVALID PASSWORDDDDDDDDD", ToastLength.Long).Show();
                    obj.StopSound();
                    txtPassowrd.Focus();
                }
                else if (Result.StartsWith("INVALID USER ID"))
                {
                    obj.PlaySound("Error");
                    Thread.Sleep(3000);
                    Toast.MakeText(Android.App.Application.Context, "INVALID USER ID", ToastLength.Long).Show();
                    obj.StopSound();
                    txtUserID.Focus();
                }
            }
        }
        private void TxtPassowrd_Completed(object sender, EventArgs e)
        {
            try
            {
                ValidateLogin();
            }
            catch (Exception ex)
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
                obj.StopSound();
            }
        }

        private void ImgLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                ValidateLogin();
            }
            catch (Exception ex)
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
                obj.StopSound();
            }
        }

        private void ImgExit_Clicked(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
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