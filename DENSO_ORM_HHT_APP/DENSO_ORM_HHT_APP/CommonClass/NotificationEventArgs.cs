using System;
using System.Collections.Generic;
using System.Text;

namespace DENSO_ORM_HHT_APP.CommonClass
{
    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
