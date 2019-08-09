﻿using System;

namespace ObjectInfos
{
    public class SysAppFixChargeInfo
    {
        public int STT { get; set; }
        public decimal Id { get; set; }
        public string Appcode { get; set; }
        public string Title { get; set; }
        public decimal Fee_Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Amount_Represent { get; set; }

        public decimal Amount_Usd { get; set; }
        public decimal Amount_Represent_Usd { get; set; }


        public string Char01 { get; set; }
        public string Char02 { get; set; }
        public string Char03 { get; set; }
        public string Description { get; set; }
        public string Description_En { get; set; }

    }
}
