﻿using System;


namespace ObjectInfos.ModuleTrademark
{
    public class AppFeeFixInfo
    {
        public AppFeeFixInfo()
        {
            Level = 0;
        }

        public AppFeeFixInfo(decimal p_Fee_Id, decimal p_Isuse)
        {
            Fee_Id = p_Fee_Id;
            Isuse = p_Isuse;
            Level = 0;
        }

        public decimal STT { get; set; }
        public decimal Id { get; set; }
        public string AppCode { get; set; }

        public string Case_Code { get; set; }
        //public decimal App_Header_Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Amount_Usd { get; set; }

        public decimal Amount_Represent { get; set; }
        public decimal Amount_Represent_Usd { get; set; }

        public decimal Fee_Id { get; set; }
        public decimal Isuse { get; set; }
        public decimal Number_Of_Patent { get; set; }
        public string Fee_Name { get; set; }
        public string Fee_Name_En { get; set; }

        public decimal Level { get; set; }
    }
}
