using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(DENSO_ORM_HHT_APP.Droid.Service_Method))]
namespace DENSO_ORM_HHT_APP.Droid
{
    public class Service_Method : CommonClass.iService_Method
    {

        public string ValidateLogin(string UserId,string Password,string Type)
        {
            try
            {
                return MainActivity.objSR.LoginValidate(UserId,Password,Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMachineGroup(string Type)
        {
            try
            {
                return MainActivity.objSR.GetMachineGroup(Type);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataSet Kanban_Progress(string MachineGroup, string MachineName,  string Barcode, string TransactionType, string Type, string PartNo, string Qty,string ModelNo, string UserdID, string ChangeOver)
        {
            try
            {
                return MainActivity.objSR.Kanban_Progress(MachineGroup, MachineName, Barcode, TransactionType, Type,PartNo,Qty,ModelNo,UserdID, ChangeOver);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet Shift_Name(string MachineGroup, string MachineName)
        //{
        //    try
        //    {
        //        return MainActivity.objSR.Get_ShiftName(MachineGroup, MachineName);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}