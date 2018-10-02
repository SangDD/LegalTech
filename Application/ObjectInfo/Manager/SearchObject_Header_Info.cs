﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
   public class SearchObject_Header_Info
    {

        public decimal STT { get; set; }

        public decimal SEARCH_ID { get; set; }

        public string CASE_CODE { get; set; }

        public string CLIENT_REFERENCE { get; set; }

        public string CASE_NAME { get; set; }

        public DateTime REQUEST_DATE { get; set; }

        public DateTime RESPONSE_DATE { get; set; }

        public decimal STATUS { get; set; }

        public string STATUS_NAME { get; set; }

        public decimal LAWER_ID { get; set; }

        public string CREATED_BY { get; set; }

        public DateTime CREATED_DATE { get; set; }


        public string MODIFIED_BY { get; set; }
        
        public DateTime MODIFIED_DATE { get; set; }

        public string LANGUAGE_CODE { get; set; }

        public string CONTENT { get; set; }

        public string LAWER_NAME { get; set; }

        public string ADMIN_NAME { get; set; }






    }
}
