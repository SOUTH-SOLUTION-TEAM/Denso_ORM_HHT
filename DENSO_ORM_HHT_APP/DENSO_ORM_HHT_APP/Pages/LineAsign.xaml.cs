using Android.Widget;
using DENSO_ORM_HHT_APP.CommonClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class LineAsign : ContentPage
    {
        public LineAsign()
        {
            notificationManager = DependencyService.Get<INotificationManager>();
            InitializeComponent();
        }
        CommonClass.SoundPlay obj = DependencyService.Get<CommonClass.SoundPlay>();
        CommonClass.iService_Method objsc = DependencyService.Get<CommonClass.iService_Method>();
        DataSet obj_DS = new DataSet();
        public ObservableCollection<ExampleData> DataList = new ObservableCollection<ExampleData>();
        INotificationManager notificationManager;
        string LineName = "";
        string Station = "";
        int notificationNumber = 0;
        bool Flag1 = true;
        bool Flag2 = true;
        bool Flag3 = false;
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
                if (CommonVariables.Flag3 == false && CommonVariables.PageName == "LineAssign")
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
        private void ImgSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool Flag = false;

                for (int i = 0; i < DataList.Count; i++)
                {
                    if (DataList[i].Selected == true)
                    {
                        Flag = true;
                    }
                }
                if (Flag == false)
                {
                    obj.PlaySound("Error");
                    Thread.Sleep(3000);
                    Toast.MakeText(Android.App.Application.Context, "PLEASE SELECT THE LINE NAMES TO SAVE", ToastLength.Long).Show();
                    obj.StopSound();
                }
                else
                {
                    if (File.Exists(CommonClass.CommonVariables.Path + "/LineNameSelection.txt"))
                    {
                        File.Delete(CommonClass.CommonVariables.Path + "/LineNameSelection.txt");
                    }
                    for (int i = 0; i < DataList.Count; i++)
                    {
                        if (DataList[i].Selected == true)
                        {
                            string LineName = DataList[i].LineName.ToString();

                            StreamWriter Sw = new StreamWriter(CommonClass.CommonVariables.Path + "/LineNameSelection.txt", true);
                            Sw.WriteLine(LineName);
                            Sw.Close();

                            StreamWriter Sw1 = new StreamWriter(CommonClass.CommonVariables.Path + "/LineGroupSelection.txt", false);
                            Sw1.WriteLine(cmbLineGroup.SelectedItem.ToString());
                            Sw1.Close();
                        }
                    }
                    obj.PlaySound("Success");
                    Toast.MakeText(Android.App.Application.Context, "SAVED SUCCESSFULLY", ToastLength.Long).Show();
                    obj.StopSound();
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

    
        private void CmbLineGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbLineGroup.SelectedIndex > -1)
                {
                    DataView dv = new DataView(obj_DS.Tables[0]);
                    string Str = cmbLineGroup.SelectedItem.ToString();
                    dv.RowFilter = "MachineGrName='" + Str + "'";
                    DataList.Clear();
                    ListTag.ItemsSource = null;
                    for (int i = 0; i < dv.ToTable(true, "MachineName").Rows.Count; i++)
                    {
                        DataList.Add(new ExampleData
                        {
                            Selected = false,
                            LineName = dv.ToTable(true, "MachineName").Rows[i]["MachineName"].ToString(),

                        });
                    }
                    ListTag.ItemsSource = DataList;
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

        private void ContentPage_LayoutChanged(object sender, EventArgs e)
        {
            try
            {
                obj_DS = objsc.GetMachineGroup("GetMachineGroupnameForHHT");
                if (obj_DS.Tables[0].Rows.Count > 0)
                {
                    DataView dv = new DataView(obj_DS.Tables[0]);
                    CommonClass.CommonMethod.FillComboBox(cmbLineGroup, dv.ToTable(true, "MachineGrName"), "MachineGrName");
                }
                ShowDateTime();
                CommonVariables.PageName = "LineAssign";
                CommonVariables.Flag3 = false;
                ListTag.ItemsSource = DataList;


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
    public class ExampleData
    {
        public bool Selected { get; set; }
        public string LineName { get; set; }
    }
}