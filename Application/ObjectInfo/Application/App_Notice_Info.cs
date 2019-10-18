﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ObjectInfos
{
    /// <summary>
    /// Dùng chung cho một số info khác 
    /// </summary>
    public class App_Notice_Info
    {
        public decimal Id { get; set; }
        public string Case_Code { get; set; }
        public string Notice_Number { get; set; }
        public DateTime Notice_Date { get; set; }
        public decimal Notice_Type { get; set; }
        public string Notice_Type_Name { get; set; }

        public HttpPostedFileBase File_Notice_Url { get; set; }
        public string Notice_Url { get; set; }

        public HttpPostedFileBase File_Notice_Trans_Url { get; set; }
        public string Notice_Trans_Url { get; set; }

        public decimal Result { get; set; }
        public string Result_Name { get; set; }

        public DateTime Accept_Date { get; set; }
        public DateTime Exp_Date { get; set; }

        public HttpPostedFileBase File_Accept_Url { get; set; }

        public string Accept_Url { get; set; }
        public decimal Times { get; set; }
        public string Reject_Reason { get; set; }
        public decimal Status { get; set; }
        public string Status_Name { get; set; }

        public string Advise_Replies { get; set; }
        public string Advise_Replies_Trans { get; set; }

        public string Replies_Number { get; set; }
        public DateTime Replies_Date { get; set; }
        public DateTime Replies_Deadline { get; set; }
        public HttpPostedFileBase File_Replies_Url { get; set; }

        // Công văn trả lời cục có dấu đỏ
        public string Replies_Url { get; set; }

        // công văn trả lời cục tự động
        public string CV_Answer_Url { get; set; }

        public HttpPostedFileBase File_Biling_Url { get; set; }

        public string Billing_Url { get; set; }
        public decimal Billing_Id { get; set; }

        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public decimal Deleted { get; set; }

        public string Note { get; set; }
        public string  Modify_By { get; set; }
        public DateTime Modify_Date { get; set; }
        public string Customer { get; set; }
        public string Email_Customer { get; set; }
        public decimal Country_Customer { get; set; }

        public string Customer_Name { get; set; }
        public string Customer_Country_Name { get; set; }

        public string Client_Reference { get; set; }
        public string Case_Name { get; set; }
    }
}
