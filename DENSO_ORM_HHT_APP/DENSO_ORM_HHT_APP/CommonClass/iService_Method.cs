using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DENSO_ORM_HHT_APP.CommonClass
{
    public interface iService_Method
    {
        string ValidateLogin(string UserId, string Password, string Type);
        DataSet GetMachineGroup(string Type);
        DataSet Kanban_Progress(string MachineGroup, string MachineName,  string Barcode, string TransactionType, string Type, string PartNo, string Qty, string ModelNo, string UserdID ,string ChangeOver);
       // DataSet Shift_Name(string MachineGroup, string MachineName);
    }
}
