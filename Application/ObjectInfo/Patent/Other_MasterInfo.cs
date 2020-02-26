﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    /// <summary>
    /// Dùng chung cho một số info khác 
    /// </summary>
    public class Other_MasterInfo
    {
        public decimal Model_Id { get; set; }
        public decimal Id { get; set; }
        public decimal App_Header_Id { set; get; }
        public string Case_Code { set; get; }

        public string Master_Name { set; get; }

        public string Master_Address { set; get; }

        public string Master_Phone { set; get; }

        public string Master_Fax { set; get; }
        public string Master_Email { set; get; }

        public string TacGiaDongThoi { set; get; }
        public string PhoBan { set; get; }

        public decimal Country_Nationality { get; set; }
        public decimal Country_Residence { get; set; }
        public decimal Country_Incorporation { get; set; }

        public string Country_Nationality_Name { get; set; }
        public string Country_Residence_Name { get; set; }
        public string Country_Incorporation_Name { get; set; }
    }

    public class Inventor_Info
    {
        public decimal Inventor_Type { get; set; }

        public decimal Model_Id { get; set; }
        public decimal Id { get; set; }
        public decimal App_Header_Id { set; get; }
        public string Case_Code { set; get; }

        public string Inventor_Name { set; get; }

        public string Inventor_Address { set; get; }

        public string Inventor_Phone { set; get; }

        public string Inventor_Fax { set; get; }
        public string Inventor_Email { set; get; }

        public decimal Country_Nationality { get; set; }
        public decimal Country_Residence { get; set; }
        public decimal Country_Incorporation { get; set; }

        public string Country_Nationality_Name { get; set; }
        public string Country_Residence_Name { get; set; }
        public string Country_Incorporation_Name { get; set; }

        public string Inventor_Others { get; set; }
    }

    public class AuthorsInfo
    {
        public decimal Model_Id { get; set; }

        public decimal Author_Id { get; set; }
        public string Case_Code { set; get; }

        public string Author_Name { set; get; }

        public string Author_Address { set; get; }

        public string Author_Phone { set; get; }

        public string Author_Fax { set; get; }
        public string Author_Email { set; get; }

        public decimal Author_Country { set; get; }
        public string Author_Country_Display { set; get; }
        public decimal App_Header_Id { set; get; }
    }

    public class UTienInfo
    {
        public decimal Model_Id { get; set; }
        public decimal Id { get; set; }
        public string Case_Code { get; set; }
        public string UT_SoDon { set; get; }
        public DateTime UT_NgayNopDon { set; get; }
        public decimal UT_QuocGia { set; get; }
        public string UT_QuocGia_Display { set; get; }
        public string UT_Type { set; get; }
        public string UT_ThoaThuanKhac { set; get; }
        public decimal DaAddd_DonUTKhac { get; set; }
        public decimal App_Header_Id { set; get; }
    }
}
