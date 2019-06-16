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
    public class A03_Info : ApplicationHeaderInfo
    {
        public decimal A03_Id { set; get; }

        //public string Case_Code { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }

        public string Industry_Design_Name { get; set; }
        public string Refappno { get; set; }
        public DateTime App_Sentdate { get; set; }
        public decimal Kqtd_Type { get; set; }

        public decimal Kqtd_Value { get; set; }

        public decimal Phanloai_Type { get; set; }

    }
}
