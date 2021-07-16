using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DENSO_ORM_HHT_APP.CommonClass
{
   public class CommonMethod
    {
        public static void FillComboBox(Xamarin.Forms.Picker Picker, DataTable dt, string DisPlayMember)
        {
            try
            {
                Picker.ItemsSource = null;
                List<string> list = dt.Rows.OfType<DataRow>().Select(dr => (string)dr[DisPlayMember]).ToList();
                Picker.ItemsSource = list;
          
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UNFill_ComboBox(Xamarin.Forms.Picker Picker)
        {
            try
            {
                Picker.ItemsSource = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
