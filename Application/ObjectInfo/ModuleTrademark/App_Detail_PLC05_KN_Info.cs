﻿using System;
using System.Web;

namespace ObjectInfos
{

    public class App_Detail_PLC05_KN_Info : ApplicationHeaderInfo
    {
        public static void CopyAppHeaderInfo(ref App_Detail_PLC05_KN_Info p_appDetail, ApplicationHeaderInfo pAppInfo)
        {
            p_appDetail.STT = pAppInfo.STT;
            p_appDetail.Detail_Id = pAppInfo.Id;
            p_appDetail.Appcode = pAppInfo.Appcode;
            p_appDetail.Master_Name = pAppInfo.Master_Name;
            p_appDetail.Master_Address = pAppInfo.Master_Address;
            p_appDetail.Master_Phone = pAppInfo.Master_Phone;
            p_appDetail.Master_Fax = pAppInfo.Master_Fax;
            p_appDetail.Master_Email = pAppInfo.Master_Email;
            p_appDetail.Rep_Master_Type = pAppInfo.Rep_Master_Type;
            p_appDetail.Rep_Master_Name = pAppInfo.Rep_Master_Name;
            p_appDetail.Rep_Master_Address = pAppInfo.Rep_Master_Address;
            p_appDetail.Rep_Master_Phone = pAppInfo.Rep_Master_Phone;
            p_appDetail.Rep_Master_Fax = pAppInfo.Rep_Master_Fax;
            p_appDetail.Rep_Master_Email = pAppInfo.Rep_Master_Email;
            p_appDetail.Relationship = pAppInfo.Relationship;
            p_appDetail.Send_Date = pAppInfo.Send_Date;
            p_appDetail.Status = pAppInfo.Status;
            p_appDetail.Status_Form = pAppInfo.Status_Form;
            p_appDetail.Status_Content = pAppInfo.Status_Content;
            p_appDetail.Remark = pAppInfo.Remark;
            p_appDetail.AppName = pAppInfo.AppName;
            p_appDetail.Address = pAppInfo.Address;
            p_appDetail.DateNo = pAppInfo.DateNo;
            p_appDetail.Months = pAppInfo.Months;
            p_appDetail.Years = pAppInfo.Years;
        }

        public App_Detail_PLC05_KN_Info()
        {

        }

        public App_Detail_PLC05_KN_Info(App_Detail_PLC05_KN_Info p_app_detail, ApplicationHeaderInfo p_app_header)
        {
            this.Language_Code = p_app_detail.Language_Code;
            this.App_Header_Id = p_app_detail.App_Header_Id;
            this.Appcode = p_app_detail.Appcode;
            this.Customer_Code = p_app_detail.Customer_Code;

            this.Times = p_app_detail.Times;
            this.Appeal_Type = p_app_detail.Appeal_Type;
            this.Appeal_Number = p_app_detail.Appeal_Number;
            this.Appeal_Date = p_app_detail.Appeal_Date;
            this.Appeal_Appno = p_app_detail.Appeal_Appno;
            this.Appeal_Degree = p_app_detail.Appeal_Degree;


            this.STT = p_app_header.STT;
            //this.Id = p_app_header.Id;
            this.Appcode = p_app_header.Appcode;
            this.Master_Name = p_app_header.Master_Name;
            this.Master_Address = p_app_header.Master_Address;
            this.Master_Phone = p_app_header.Master_Phone;
            this.Master_Fax = p_app_header.Master_Fax;
            this.Master_Email = p_app_header.Master_Email;
            this.Rep_Master_Type = p_app_header.Rep_Master_Type;
            this.Rep_Master_Name = p_app_header.Rep_Master_Name;
            this.Rep_Master_Address = p_app_header.Rep_Master_Address;
            this.Rep_Master_Phone = p_app_header.Rep_Master_Phone;
            this.Rep_Master_Fax = p_app_header.Rep_Master_Fax;
            this.Rep_Master_Email = p_app_header.Rep_Master_Email;
            this.Relationship = p_app_header.Relationship;
            this.Send_Date = p_app_header.Send_Date;
            this.Status = p_app_header.Status;
            this.Status_Form = p_app_header.Status_Form;
            this.Status_Content = p_app_header.Status_Content;
            this.Remark = p_app_header.Remark;
            this.AppName = p_app_header.AppName;
            this.Address = p_app_header.Address;
            this.DateNo = p_app_header.DateNo;
            this.Months = p_app_header.Months;
            this.Years = p_app_header.Years;
        }

        public decimal Detail_Id { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public decimal Times { get; set; }
        public decimal Appeal_Type { get; set; }
        public string Appeal_Number { get; set; }
        public DateTime Appeal_Date { get; set; }
        public string Appeal_Appno { get; set; }
        public string Appeal_Degree { get; set; }
        public string Customer_Code { get; set; }

        string _Str_Appeal_Date;
        public string Str_Appeal_Date
        {
            get
            {
                if (Appeal_Date.Date == DateTime.MinValue.Date)
                    return "";
                else return Appeal_Date.ToString("dd/MM/yyyy");
            }
            set
            {
                _Str_Appeal_Date = value;
            }
        }


        public decimal Fee_Id_1 { get; set; }
        public decimal Fee_Id_1_Check { get; set; }
        public string Fee_Id_1_Val { get; set; }

        public decimal Fee_Id_2 { get; set; }
        public decimal Fee_Id_2_Check { get; set; }
        public string Fee_Id_2_Val { get; set; }

        public decimal Total_Fee { get; set; }
        public string Total_Fee_Str { get; set; }

        // các tài liệu trong đơn
        public string Doc_Id_1 { get; set; }
        public decimal Doc_Id_1_Check { get; set; }

        public string Doc_Id_2 { get; set; }
        public decimal Doc_Id_2_Check { get; set; }

        public string Doc_Id_3 { get; set; }
        public decimal Doc_Id_3_Check { get; set; }

        public string Doc_Id_4 { get; set; }
        public decimal Doc_Id_4_Check { get; set; }

        public string Doc_Id_5 { get; set; }
        public decimal Doc_Id_5_Check { get; set; }

        public string Doc_Id_6 { get; set; }
        public decimal Doc_Id_6_Check { get; set; }

        public string Doc_Id_7 { get; set; }
        public decimal Doc_Id_7_Check { get; set; }

        public string Doc_Id_8 { get; set; }
        public decimal Doc_Id_8_Check { get; set; }

        public string Doc_Id_9 { get; set; }
        public decimal Doc_Id_9_Check { get; set; }

        public string Doc_Id_10 { get; set; }
        public decimal Doc_Id_10_Check { get; set; }

        public string Doc_Id_11 { get; set; }
        public decimal Doc_Id_11_Check { get; set; }
        public string Doc_Id_12 { get; set; }
        public decimal Doc_Id_12_Check { get; set; }
    }
}
