﻿using System;

namespace ObjectInfos.ModuleTrademark
{
    public interface ISysAppDocumentInfo
    {
    }
    public class SysAppDocumentInfo:ISysAppDocumentInfo
    {
        public decimal Id { get; set; }
        public string Appcode { get; set; }
        public decimal Document_Id { get; set; }
        public string Language_Code { get; set; }
        public decimal Lstord { get; set; }
    }
}
