using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data;
using Android.Widget;
using Android.Content;
using System.Threading;
using Matcha.BackgroundService;
using DENSO_ORM_HHT_APP.CommonClass;

namespace DENSO_ORM_HHT_APP.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Kanban_Progress : ContentPage
    {
        CommonClass.iService_Method objsc = DependencyService.Get<CommonClass.iService_Method>();
        DataSet obj_DS = new DataSet();
        CommonClass.SoundPlay obj = DependencyService.Get<CommonClass.SoundPlay>();

        INotificationManager notificationManager;
        int notificationNumber = 0;
        string LineName = "";
        string Station = "";
        bool Flag1 = true;
        bool Flag2 = true;
        bool Flag3 = false;
        public Kanban_Progress()
        {
            InitializeComponent();
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
        }


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
        //                        DataSet obj_ds = objsc.Kanban_Progress(txtLineGroup.Text, cmbLineName.SelectedItem.ToString(), "", "", "AndonCallUpdate", "", "", "","", "");
        //                        obj.StopSound();
        //                        Flag1 = true;
        //                        CommonVariables.Flag3 = false;
        //                       // CommonClass.CommonVariables.SleepMode = false;

        //                    }
        //                }
        //                else
        //                {
        //                    Flag1 = true;
        //                    obj.StopSound();
        //                    CommonVariables.Flag3 = false;
        //                    //CommonClass.CommonVariables.SleepMode = false;

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
        //      {
        //          if (CommonVariables.Flag3 == false && CommonVariables.PageName == "KanbanPage")
        //          {
        //              //if (CommonClass.CommonVariables.ShiftName != "")
        //              //{
        //              if (Flag1 == true)
        //              {
        //                  if (cmbLineName.SelectedItem != (null))
        //                  {
        //                      DataSet obj_ds = objsc.Kanban_Progress(txtLineGroup.Text, cmbLineName.SelectedItem.ToString(), "", "", "AndonCall", "", "", "", "", "");
        //                      if (obj_ds.Tables[0].Rows.Count > 0)
        //                      {
        //                          Flag = false;
        //                          ConfirmMessage();
        //                          CommonVariables.Flag3 = true;
        //                          obj.PlaySound("Error");
        //                      }
        //                      else
        //                          obj.StopSound();
        //                  }
        //              }
        //              //else
        //              //    obj.StopSound();
        //              //}
        //              //else
        //              //    obj.StopSound();
        //          }

        //          return Flag;
        //      });

        //}

        //public void ConfirmMessage()
        //{
        //    Device.BeginInvokeOnMainThread(async () => {


        //        if(CommonClass.CommonVariables.SleepMode == true)
        //        notificationManager.ScheduleNotification("PART FEADING", "PLEASE LOAD PART TO MACHINE");

        //       // obj.PlaySound("Error");
        //        bool Result = await App.Current.MainPage.DisplayAlert("Alter!", "PLEASE LOAD PART TO MACHINE", "Accept", "Cancel");
        //        if (Result)
        //        {
        //            if (Flag1 == true)
        //            {
        //                DataSet obj_ds = objsc.Kanban_Progress(txtLineGroup.Text, cmbLineName.SelectedItem.ToString(),  "", "", "AndonCallUpdate", "", "", "","","");
        //                CommonVariables.Flag3 = false;
        //                ShowDateTime();
        //                obj.StopSound();
        //                //Flag2 = false;
        //            }
        //        }
        //        else
        //        {
        //            CommonVariables.Flag3 = false;
        //            ShowDateTime();
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
                if (CommonVariables.Flag3 == false && CommonVariables.PageName == "KanbanPage")
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
        private void ContentPage_LayoutChanged(object sender, EventArgs e)
        {
            try
            {
               
                DataTable dt = new DataTable();
                dt.Columns.Add("ChangeOver");
                dt.Rows.Add("First Change Over");
                dt.Rows.Add("Second Change Over");
                if (dt.Rows.Count > 0)
                {
                    CommonClass.CommonMethod.FillComboBox(cmbChangeOver, dt, "ChangeOver");
                }
                txtLineGroup.Text = CommonClass.CommonVariables.LineGroup;
                CommonClass.CommonMethod.FillComboBox(cmbLineName, CommonVariables.dt, "LineName");
                if (CommonClass.CommonVariables.LineName != "")
                {
                    cmbLineName.SelectedItem = CommonClass.CommonVariables.LineName;
                }
                CommonVariables.PageName = "KanbanPage";
                CommonVariables.Flag3 = false;
                ShowDateTime();
             
            }
            catch (Exception ex)
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
                obj.StopSound();
            }
        }
        private void TxtSending_Completed(object sender, EventArgs e)
        {
            try
            {
                if (txtLineGroup.Text == (null) || txtLineGroup.Text == "")
                {
                    obj.PlaySound("Error");
                    Toast.MakeText(Android.App.Application.Context, "PLEASE ENTER LINE GROUP", ToastLength.Long).Show();
                    Thread.Sleep(3000);
                    obj.StopSound();
                    //  cmbModelName.Focus();
                    return;
                }
                if (cmbLineName.SelectedItem == (null) || cmbLineName.SelectedItem.ToString() == "")
                {
                    obj.PlaySound("Error");
                    Toast.MakeText(Android.App.Application.Context, "PLEASE SELECT LINE NAME", ToastLength.Long).Show();
                    Thread.Sleep(3000);
                    obj.StopSound();
                    //  cmbModelName.Focus();
                    return;
                }
                if (cmbModelName.SelectedItem ==(null) || cmbModelName.SelectedItem.ToString() =="")
                {
                    obj.PlaySound("Error");
                    Toast.MakeText(Android.App.Application.Context, "PLEASE SELECT MODULE", ToastLength.Long).Show();
                    Thread.Sleep(3000);
                    obj.StopSound();
                  //  cmbModelName.Focus();
                    return;
                }
                if (cmbChangeOver.SelectedItem == (null) || cmbChangeOver.SelectedItem.ToString() == "")
                {
                    obj.PlaySound("Error");
                    Toast.MakeText(Android.App.Application.Context, "PLEASE SELECT CHANGE OVER", ToastLength.Long).Show();
                    Thread.Sleep(3000);
                    obj.StopSound();
                  //  cmbChangeOver.Focus();
                    return;
                }

                if (txtSending.Text != "" && txtSending.Text != (null))
                {
                    string[] str = txtSending.Text.Split(' ');
                    DataSet obj_ds = objsc.Kanban_Progress(txtLineGroup.Text, cmbLineName.SelectedItem.ToString(), txtSending.Text, "Send", "Save", txtSending.Text.Substring(90, 16),Convert.ToInt32( txtSending.Text.Substring(106, 7)).ToString(), cmbModelName.SelectedItem.ToString(),CommonClass.CommonVariables.Loggedin, cmbChangeOver.SelectedItem.ToString());
                    if (obj_ds.Tables[0].Rows.Count > 0)
                    {
                        if (obj_ds.Tables[0].Columns[0].ColumnName == "Error")
                        {
                            obj.PlaySound("Error");
                            Thread.Sleep(3000);
                            Toast.MakeText(Android.App.Application.Context, obj_ds.Tables[0].Rows[0][0].ToString(), ToastLength.Long).Show();
                            obj.StopSound();
                            txtSending.Text = "";
                        }
                        else
                        {
                            if (obj_ds.Tables[0].Rows.Count > 0)
                            {
                                lblQty.Text = obj_ds.Tables[0].Rows[0]["Qty"].ToString();
                                lblSentCount.Text = obj_ds.Tables[0].Rows[0]["Count"].ToString();

                            }
                            txtSending.Text = "";
                            txtSending.Focus();
                        }
                    }
                }
                else
                {
                    obj.PlaySound("Error");
                    Thread.Sleep(3000);
                    Toast.MakeText(Android.App.Application.Context, "PLEASE SCAN KANBAN", ToastLength.Long).Show();
                    obj.StopSound();
                    txtSending.Text = "";
                }
            }
            catch (Exception ex)
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
                obj.StopSound();
                txtSending.Text = "";
            }

        }

      

        private void CmbLineName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                obj_DS = objsc.GetMachineGroup("GetMachineGroupnameForHHT");
                if (obj_DS.Tables[0].Rows.Count > 0)
                {
                    if (cmbLineName.SelectedIndex > -1)
                    {
                        DataView dv = new DataView(obj_DS.Tables[0]);
                        dv.RowFilter = "MachineName='" + cmbLineName.SelectedItem.ToString() + "' and MachineGrName='" + txtLineGroup.Text + "'";
                        CommonClass.CommonMethod.FillComboBox(cmbModelName, dv.ToTable(), "ModelName");
                    }
                    else
                        CommonClass.CommonMethod.UNFill_ComboBox(cmbModelName);
                }
            }
            catch (Exception ex)
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
                obj.StopSound();
            }
        }

      
        private void CmbModelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbModelName.SelectedIndex > -1)
                {
                    DataSet obj_ds = objsc.Kanban_Progress(txtLineGroup.Text, cmbLineName.SelectedItem.ToString(),  "", "", "GetKanbanCount", "","", cmbModelName.SelectedItem.ToString(),"", "");
                    if(obj_ds.Tables[0].Rows.Count>0)
                    {
                        lblQty.Text = obj_ds.Tables[0].Rows[0]["Qty"].ToString();
                        lblSentCount.Text = obj_ds.Tables[0].Rows[0]["Count"].ToString();

                    }
                    else
                    {
                        lblQty.Text = "KANBAN TOTAL QTY = 0";
                        lblSentCount.Text = "NO OF KANBAN = 0";
                    }
                    // OnBackButtonPressed();
                }
                else
                    CommonClass.CommonMethod.UNFill_ComboBox(cmbLineName);

            }
            catch (Exception ex)
            {
                obj.PlaySound("Error");
                Thread.Sleep(3000);
                Toast.MakeText(Android.App.Application.Context, ex.Message.ToString(), ToastLength.Long).Show();
                obj.StopSound();
            }
        }

        private void ImgSave_Clicked(object sender, EventArgs e)
        {

        }

        private void ImgBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                obj.StopSound();
                CommonVariables.PageName = "MainPage";
                CommonVariables.Flag3 = false;
                App.Current.MainPage = new Pages.MainPage();
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