﻿using Common;
using Common.CommonData;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BussinessFacade.ModuleTrademark;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Security.Cryptography;
using ObjectInfos;
using GemBox.Document;
using BussinessFacade.ModuleUsersAndRoles;
using BussinessFacade;
using WebApps.Session;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using BussinessFacade.ModuleMemoryData;
using System.Linq;
using ObjectInfos.ModuleTrademark;
using CrystalDecisions.Shared;

namespace WebApps.CommonFunction
{
    public class AppsCommon
    {
        static MyQueue c_QueueSendEmail = new MyQueue();
        static int RecordOnpage = 10;

        public static void EnqueueSendEmail(Email_Info email_Info)
        {
            c_QueueSendEmail.Enqueue(email_Info);
        }

        public static Email_Info Dequeue_SendEmail()
        {
            Email_Info _Email_Info = (Email_Info)c_QueueSendEmail.Dequeue();
            if (_Email_Info != null)
                return _Email_Info;
            else return null;
        }

        public static string gKeyEncrypt = @"L2WcFveA50iHVuucn+bPUw==";

        public static string GetCurrentLang()
        {
            try
            {
                var culture = "";
                var queryLanguage = "";

                if (HttpContext.Current.Request == null)
                {
                    culture = "en-GB";
                    return culture.ToUpper();
                }

                if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["lang"]))
                {
                    queryLanguage = HttpContext.Current.Request.QueryString["lang"].ToString().ToUpper();
                    if (queryLanguage == "EN-GB")
                    {
                        culture = "en-GB";
                    }
                }
                else
                {
                    var httpCookie = HttpContext.Current.Request.Cookies["language"];
                    if (httpCookie != null)
                    {
                        culture = httpCookie.Value;
                    }
                    else
                    {
                        culture = ConfigurationManager.AppSettings["DefaultLang"].ToString();
                    }
                }
                if (culture.ToUpper().Equals("EN-GB"))
                    culture = Language.LangEN;
                else if (culture.ToUpper().Equals("EN-US") || culture.ToUpper().Equals("VI-VN"))
                    culture = Language.LangVI;
                else culture = Language.LangVI;
                return culture.ToUpper();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Request is not available in this context")
                {
                    Logger.LogException(ex);
                }
                return Language.LangEN;
            }
        }

        public static String RenderRazorViewToString(ControllerContext controllerContext, String viewName, Object model = null)
        {
            try
            {
                controllerContext.Controller.ViewData.Model = model;

                using (var sw = new StringWriter())
                {
                    var ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                    var ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                    ViewResult.View.Render(ViewContext, sw);
                    ViewResult.ViewEngine.ReleaseView(controllerContext, ViewResult.View);
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static string Get_HtmlPaging<T>(int p_total_record, int PCurrentPage, string pLoaiBanghi = "Bản ghi", int p_reconpage = 0, string pfuncjs = "")
        {
            try
            {
                string _defaulFuncjs = "jsPaging";
                if (!string.IsNullOrEmpty(pfuncjs))
                {
                    _defaulFuncjs = pfuncjs;
                }
                if (p_reconpage == 0)
                {
                    return Paging(PCurrentPage, RecordOnpage, p_total_record, _defaulFuncjs, pLoaiBanghi);
                }
                else
                {
                    return Paging(PCurrentPage, p_reconpage, p_total_record, _defaulFuncjs, pLoaiBanghi);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static string Paging(int pCurPage, int pRecordOnPage, int pTotalRecord, string p_js_function = "", string _str_loai_ban_ghi = "")
        {
            try
            {
                if (_str_loai_ban_ghi == "")
                {
                    _str_loai_ban_ghi = "Bản ghi";
                }
                if (p_js_function == "")
                {
                    p_js_function = "page";
                }
                string pStrPaging = "";
                double _dobTotalRec = Convert.ToDouble(pTotalRecord);
                int _TotalPage = CommonFuc.RoundUp(_dobTotalRec / pRecordOnPage);
                pStrPaging = WritePaging(_TotalPage, pCurPage, pTotalRecord, pRecordOnPage, _str_loai_ban_ghi, p_js_function);
                return pStrPaging;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static string WritePaging(int iPageCount, int iCurrentPage, int iTotalRecords, int iPageSize, string pLoaiBanGhi, string p_jsfuncion = "")
        {
            try
            {
                if (p_jsfuncion == "")
                {
                    p_jsfuncion = "page";
                }
                string strPage = "";
                strPage += "<div id='d_page'>";
                // strPage += "<div id='d_total_rec'>" + "Có tổng " + iTotalRecords + " " + pLoaiBanGhi + "</div>";
                string language = AppsCommon.GetCurrentLang();

                if (language.Contains("VN"))
                {
                    strPage += "<div id='d_total_rec'>" + "Có tổng " + iTotalRecords + " " + "nội dung" + " </div>";
                }
                else
                {
                    strPage += "<div id='d_total_rec'>" + "There are " + iTotalRecords + " " + "records in total" + " </div>";
                }

                strPage += "<div id='d_number_of_page'>";
                if (iPageCount <= 1) return strPage;
                if (iCurrentPage > 1)
                {
                    //sangdd moi them dong 1 trở về trang đầu
                    strPage += "<li onclick=\"" + p_jsfuncion + "(1)\"><span id=\"first\"  href=\"\"><<</span></li>";

                    strPage += "<li onclick=\"" + p_jsfuncion + "('" + (iCurrentPage - 1) + "')\"><span id=\"back\"  href=\"\"><</span></li>";
                }
                if (iPageCount <= 5)
                {
                    for (int j = 0; j < iPageCount; j++)
                    {
                        if (iCurrentPage == (j + 1))
                        {
                            //HungTD rem doan nay
                            strPage += "<li style=\"background-color: #CDCDCD;\" onclick=\"" + p_jsfuncion + "(" + (j + 1) + ")\"><span class=\"a-active\" id=" + (j + 1) + "  href=\"\">" + (j + 1) + "</span></li>";
                        }
                        else
                        {
                            strPage += "<li onclick=\"" + p_jsfuncion + "(" + (j + 1) + ")\"><span id=" + (j + 1) + "  href=\"\">" + (j + 1) + "</span></li>";
                        }
                    }
                }
                else
                {
                    string cl = "";
                    int t;
                    int pagePreview = 0; //nếu đang ở trang 2 thì vẽ đc có 1 trang trước nó nên sẽ vẽ thêm 3 trang phía sau 
                    //default là vẽ 2 trang trc 2 trang sau 
                    int soTrangVeLui = 2;
                    if ((iPageCount - iCurrentPage) == 1)
                    {
                        soTrangVeLui = soTrangVeLui + 1;
                    }
                    else if ((iPageCount - iCurrentPage) == 0)
                    {
                        soTrangVeLui = soTrangVeLui + 2;
                    }
                    for (t = iCurrentPage - soTrangVeLui; t <= iCurrentPage; t++) //ve truoc 2 trang 
                    {
                        if (t < 1) continue;
                        cl = t == iCurrentPage ? "a-active" : "";
                        strPage += t == iCurrentPage ? "<li onclick=\"" + p_jsfuncion + "(" + (t) + ")\" style=\"background-color: #CDCDCD;\"><span class=" + cl + " id=" + (t) + "  href=\"\">" + (t) + "</span></li>"
                                    : "<li   onclick=\"" + p_jsfuncion + "(" + (t) + ")\"><span class=" + cl + " id=" + (t) + "  href=\"\">" + (t) + "</span></li>";
                        //strPage += "<li onclick=\"page(" + (t) + ")\"><span class=" + cl + " id=" + (t) + "  href=\"\">" + (t) + "</span></li>";
                        pagePreview++;
                    }
                    t = 0;
                    cl = "";
                    if (iCurrentPage == 1) //truong hop trang dau tien thi ve du 5 trang 
                    {
                        for (t = iCurrentPage + 1; t < iCurrentPage + 5; t++)
                        {
                            if (t >= t + 5 || t > iPageCount) continue;
                            cl = t == iCurrentPage ? "a-active" : "";
                            strPage += t == iCurrentPage ? "<li onclick=\"" + p_jsfuncion + "(" + (t) + ")\" style=\"background-color: #CDCDCD;\"><span class=" + cl + " id=" + (t) + "  href=\"\">" + (t) + "</span></li>"
                                     : "<li   onclick=\"" + p_jsfuncion + "(" + (t) + ")\"><span class=" + cl + " id=" + (t) + "  href=\"\">" + (t) + "</span></li>";
                            //strPage += "<li  onclick=\"page(" + (t) + ")\"><span class=" + cl + " id=" + (t) + "  href=\"\">" + (t) + "</span></li>";
                        }
                    }
                    else if (iCurrentPage > 1)  //truogn hop ma la trang lon hon 1 thi se ve 4 trang ke tiep + 1 trang truoc 
                    {
                        int incr = 5 - (pagePreview - 1);
                        for (t = iCurrentPage + 1; t < iCurrentPage + incr; t++)
                        {
                            if (t >= t + incr || t > iPageCount) continue;
                            cl = t == iCurrentPage ? "a-active" : "";
                            strPage += t == iCurrentPage ? "<li onclick=\"" + p_jsfuncion + "(" + (t) + ")\" style=\"background-color: #CDCDCD;\"><span class=" + cl + " id=" + (t) + "  href=\"\">" + (t) + "</span></li>"
                                     : "<li   onclick=\"" + p_jsfuncion + "(" + (t) + ")\"><span class=" + cl + " id=" + (t) + "  href=\"\">" + (t) + "</span></li>";
                            //strPage += "<li onclick=\"page(" + (t) + ")\"><span class=" + cl + " id=" + (t) + "  href=\"\">" + (t) + "</span></li>";
                        }
                    }

                }
                if (iCurrentPage < iPageCount)
                {
                    strPage += "<li onclick=\"" + p_jsfuncion + "('" + (iCurrentPage + 1) + "')\"><span id=\"next\"  href=\"\">></span></li>";
                    //sangdd moi them dong 1 trở về trang cuối
                    strPage += "<li onclick=\"" + p_jsfuncion + "(" + iPageCount + ")\"><span id=\"end\"  href=\"\">>></span></li>";
                }
                strPage += "</div>";
                strPage += "</div>";
                return strPage;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return string.Empty;
            }
        }

        public static string Get_HtmlPaging_2<T>(int p_total_record, int PCurrentPage, string pLoaiBanghi = "Bản ghi")
        {
            try
            {
                return Paging(PCurrentPage, RecordOnpage, p_total_record, "jsPaging_2", pLoaiBanghi);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static string CreateRandomString(int p_length)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                char c; Random rand = new Random();
                for (int i = 0; i < p_length; i++)
                {
                    c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                    sb.Append(c);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        // chia 2 cột
        public static string SetContentMailTemplate1(string content, string yourref = "", string outref = "", string dearname = "", string p_namereply = "", string p_position_name = "")
        {
            string _content = "";


            string _yourref = yourref == null ? "" : yourref;
            string _outref = outref == null ? "" : outref;
            string _dear = "";
            string _cellphone_business = Common.Configuration.cellphone_business;
            string _urlweb_business = Common.Configuration.urlweb_business;
            string emailfrom_business = Common.Configuration.emailfrom_business;
            string _namereply = Common.Configuration.namereply;
            string _address1 = Common.Configuration.address1;
            string _address2 = Common.Configuration.address2;

            if (p_namereply == "")
            {
                p_namereply = _namereply;
            }

            if (p_position_name == "")
            {
                p_position_name = "Managing Partner";
            }

            if (dearname != "" && dearname != null)
            {
                _dear = "Dear " + dearname + ",";

            }

            try
            {
                //_content = "<head>  <style> table.column-1-2 {   margin-right: 25px;  } @media only screen and (max-width: 640px) {  .container { width: 100% !important;  max-width: 100% !important; }.container .remove{	display: none;}table.column-1-2 {  float: none !important; margin-right: 0 !important; width: 100% !important;  text-align: center !important; }	.right{margin-left: 0 !important;} }  @media only screen and (min-width: 641px) {.container { max-width: 100% !important; }table.column-1-22{display:none;} }   </style>  </head><div style='margin: 0; font-family: 'Source Sans Pro', sans-serif; font-size: 14px;'><table style='border-collapse: collapse;' width='100%' cellspacing='0' cellpadding='0' border='0' align='center'><tr> <td style='padding: 50px 0;'> <div style='max-width: 100%; margin: 0 auto;'>   <table class='container' style='border-collapse: collapse;' width='100%' cellspacing='0' cellpadding='0' border='0' align='center'> <tbody> <tr> <td><table class='column-1-2 remove' width='230' align='left' style='border-collapse: collapse;font-family: arial,sans-serif;' cellspacing='0' cellpadding='0' border='0'><tbody> <tr><td style='font-family: arial,sans-serif;color: #669cbb;font-size: 26px;' valign='top'>IPATH CONSULT CO., LTD.</td> </tr><tr><td style='color:#afafaf;font-style:italic;padding-top:50px'>Following the Rule of Law</td>  </tr> <tr><td style='padding-top: 80px;line-height: 21px;color: #666;margin-top: 80px;font-size: 13px;'>Level 16, Deaha Business Center, 360 Kim Ma, Hanoi. Contact: +84936981616 /</td></tr><tr><td style='padding-top: 0px;line-height: 21px;font-size: 13px;'><a href='mailto:tuyen.le@pathlaw.net' target='_blank'>tuyen.le@pathlaw.net</a></td></tr><tr><td style='padding-top: 0px;line-height: 21px;font-size: 13px;font-weight: bold'><a href='www.pathlaw.net' target='_blank' style='text-decoration:none'>www.pathlaw.net</a></td> </tr>  </tbody></table><div class='right'  style='background: #fff; margin-left: 230px;'><table><tr><td style='color:#5a5a5a;padding-top:10px'>Your Ref:</td></tr><tr><td style='color:#5a5a5a;padding-top:10px'>Out Ref:</td></tr><tr><td style='font-weight:bold;color:#8e8e8e;padding-top:30px'>Dear dangtq,</td></tr><tr><td style='color:#8a8a8a;line-height:20px;padding:20px 0 40px 0'>ƯQE</td></tr><tr><td style='font-weight:bold;color:#8e8e8e;'>Tuyen Le</td></tr><tr>	<td style='font-weight:bold;color:#8e8e8e;'>	Managing Partner</td></tr></table>  </div> <table class='column-1-22' width='100%' style='border-collapse: collapse;' cellspacing='0' cellpadding='0' border='0'> <tr> <td style='padding: 25px; background: #f2f2f2; text-align: center;' style='border-collapse: collapse;' cellspacing='0' cellpadding='0' border='0' valign='top'>made by <a href='https://mangools.com'>mangools.com</a> </td>  </tr></table> </td> </tr></tbody> </table></div></td> </tr></table>";  }
                _content = " <head>" +
                    "<style>  table.column-1-2 {  margin-right: 25px;  } @media only screen and (max-width: 640px) {  .container { width: 100% !important;  max-width: 100% !important;  }.container .remove{display: none;}table.column-1-2 {  float: none !important; margin-right: 0 !important;  width: 100% !important; text-align: center !important; }.right{margin-left: 0 !important;}} @media only screen and (min-width: 641px) { .container {  width: 800px !important;  }.container .column-1-22{display: none !important;} }</style>  " +
                    "</head> " +
                    "<div style='margin: 0; font-family: 'Source Sans Pro', sans-serif; font-size: 14px;'>" +
                    " <table style='border-collapse: collapse;' width='100%' cellspacing='0' cellpadding='0' border='0' align='center'><tr> <td style='padding: 50px 0;'> <div style='max-width: 100%; margin: 0 auto;'>  <table class='container' style='border-collapse: collapse;' width='800' cellspacing='0' cellpadding='0' border='0' align='center'>  <tbody><tr>  <td> " +

                    "<table width='100%' style='border-collapse:collapse;' cellspacing='0' cellpadding='0' border='0'> <tr><td style='text-align:right'><img style='width:auto; height:110px' src='https://ci3.googleusercontent.com/proxy/Mcs1RNQrQZbufG-ry8_77wBxPn1TQ30a6rvW64dc0GEYKq5EKz0YACQ1Fb1yqwNOJAMPqj4ea2ru4naJfvFbfCg8hImIOD20tQ=s0-d-e1-ft#https://pathlaw.net/Content/News/images/logo_ipace.png'><div style='padding:7px 15px 7px 0; font-family: Roboto,sans-serif; color:#5a5a5a'>ACCESS.IP SOLUTION</div> </td></tr></table>" +

                    " <table class='column-1-2 remove' width='230' align='left' style='border-collapse: collapse;font-family: arial,sans-serif;' cellspacing='0' cellpadding='0' border='0'><tbody> <tr><td style='font-family: arial,sans-serif;color: #669cbb;font-size: 26px;' valign='top'>IPATH CONSULT CO., LTD.</td> </tr> " +
                    "<tr><td style='color:#afafaf;font-style:italic;padding-top:50px'>Following the Rule of Law</td></tr>  " +
                   
                    "<tr><td style='padding-top: 80px;line-height: 21px;color: #666;margin-top: 80px;font-size: 13px;'>" +
                    "Level 16, Deaha Business Center," +
                    "</td> </tr>" +

                     "<tr><td style='line-height: 21px;color: #666; font-size: 13px;'>" +
                    "360 Kim Ma, Hanoi, Viet Nam" +
                    "</td> </tr>" +

                     "<tr><td style='line-height: 21px;color: #666;font-size: 13px;'>" +
                    "Cell phone: " + _cellphone_business + " /" +
                    "</td> </tr>" +

                    "<tr><td style='padding-top: 0px;line-height: 21px;font-size: 13px;'>" +
                    "<a href='mailto:tuyen.le@pathlaw.net' target='_blank' style='color: #666;text-decoration:none;'>Email: " + emailfrom_business + "</a></td> </tr>" +
                    "<tr><td style='padding-top: 0px;line-height: 21px;font-size: 13px;font-weight: bold'>" +
                    "<a href='" + _urlweb_business + "' target='_blank' style='text-decoration:none; color: #7d7d7d;font-style: italic;'>www.pathlaw.net</a></td></tr>" +
                    "</tbody></table> " +
                    "<div class='right'  style='background: #fff; margin-left: 230px;'>" +
                    "<table>" +
                    "<tr><td style='color:#5a5a5a;padding-top:10px'>Your Ref: " + _yourref + "</td></tr>" +
                    "<tr><td style='color:#5a5a5a;padding-top:10px'>Out Ref: " + _outref + "</td></tr>" +
                    "<tr><td style='font-weight:bold;color:#4a4a4a;padding-top:30px;font-weight: bold;'>" + _dear + "</td></tr>	" +
                    "<tr>	<td style='color:#8a8a8a;line-height:20px;padding:20px 0 40px 0'>" + content + "</td></tr>" +
                    "<tr><td style='font-weight:bold;color:#4a4a4a;font-weight: bold;'>" + p_namereply + "</td>	</tr><tr>	" +
                    "<td style='color:#8e8e8e;padding-top: 10px;line-height: 15px;'>" + p_position_name + "</td></tr>" +
                    "</table> " +
                    "</div> " +
                    "<table class='column-1-22' width='100%' style='border-collapse: collapse;' cellspacing='0' cellpadding='0' border='0'> " +

                    "<tr><td style='padding-top: 10px;line-height: 15px;color: #7d7d7d;margin-top: 80px;font-size: 13px;font-weight: bold;'>" +
                    "IPath Consult Co.," +
                    "</td> </tr>  " +

                    "<tr><td style='line-height: 21px;color: #a09f9f;font-size: 13px;'>" +
                    "Level 16, Deaha Business Center" +
                    "</td> </tr>  "+

                     "<tr><td style='line-height: 21px;color: #a09f9f;font-size: 13px;'>" +
                    "360 Kim Ma, Hanoi, Viet Nam " +
                    "</td> </tr>  " +

                     "<tr><td style='line-height: 21px;color: #a09f9f;font-size: 13px;'>" +
                    "Cell phone: " + _cellphone_business + " /" +
                    "</td> </tr>  " +

                    "<tr><td style='padding-top: 0px;line-height: 21px;font-size: 13px;'>" +
                    "<a href='mailto:tuyen.le@pathlaw.net' target='_blank' style='color: #a09f9f;text-decoration:none;'>Email: " +
                     emailfrom_business +
                    "</a></td> </tr> <tr>	<td style='padding-top: 0px;line-height: 21px;font-size: 13px;font-weight: bold'>" +
                    "<a href='" + _urlweb_business + "' target='_blank' style='text-decoration:none; color: #7d7d7d;font-style: italic;'>www.pathlaw.net</a>" +
                    "</td>  </tr> </table> </td>  </tr></tbody>  </table> </div>  </td></tr></table> ";
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return _content;
        }

        // không chi cột 
        public static string SetContentMailTemplate(string content, string yourref = "", string outref = "", string dearname = "", string p_namereply = "", string p_position_name = "")
        {
            string _content = "";


            string _yourref = yourref == null ? "" : yourref;
            string _outref = outref == null ? "" : outref;
            string _dear = "";
            string _cellphone_business = Common.Configuration.cellphone_business;
            string _urlweb_business = Common.Configuration.urlweb_business;
            string emailfrom_business = Common.Configuration.emailfrom_business;
            string _namereply = Common.Configuration.namereply;
            string _address1 = Common.Configuration.address1;
            string _address2 = Common.Configuration.address2;

            if (p_namereply == "")
            {
                p_namereply = _namereply;
            }

            if (p_position_name == "")
            {
                p_position_name = "Managing Partner";
            }

            if (dearname != "" && dearname != null)
            {
                _dear = "Dear " + dearname + ",";

            }

            try
            {
                //_content = "<head>  <style> table.column-1-2 {   margin-right: 25px;  } @media only screen and (max-width: 640px) {  .container { width: 100% !important;  max-width: 100% !important; }.container .remove{	display: none;}table.column-1-2 {  float: none !important; margin-right: 0 !important; width: 100% !important;  text-align: center !important; }	.right{margin-left: 0 !important;} }  @media only screen and (min-width: 641px) {.container { max-width: 100% !important; }table.column-1-22{display:none;} }   </style>  </head><div style='margin: 0; font-family: 'Source Sans Pro', sans-serif; font-size: 14px;'><table style='border-collapse: collapse;' width='100%' cellspacing='0' cellpadding='0' border='0' align='center'><tr> <td style='padding: 50px 0;'> <div style='max-width: 100%; margin: 0 auto;'>   <table class='container' style='border-collapse: collapse;' width='100%' cellspacing='0' cellpadding='0' border='0' align='center'> <tbody> <tr> <td><table class='column-1-2 remove' width='230' align='left' style='border-collapse: collapse;font-family: arial,sans-serif;' cellspacing='0' cellpadding='0' border='0'><tbody> <tr><td style='font-family: arial,sans-serif;color: #669cbb;font-size: 26px;' valign='top'>IPATH CONSULT CO., LTD.</td> </tr><tr><td style='color:#afafaf;font-style:italic;padding-top:50px'>Following the Rule of Law</td>  </tr> <tr><td style='padding-top: 80px;line-height: 21px;color: #666;margin-top: 80px;font-size: 13px;'>Level 16, Deaha Business Center, 360 Kim Ma, Hanoi. Contact: +84936981616 /</td></tr><tr><td style='padding-top: 0px;line-height: 21px;font-size: 13px;'><a href='mailto:tuyen.le@pathlaw.net' target='_blank'>tuyen.le@pathlaw.net</a></td></tr><tr><td style='padding-top: 0px;line-height: 21px;font-size: 13px;font-weight: bold'><a href='www.pathlaw.net' target='_blank' style='text-decoration:none'>www.pathlaw.net</a></td> </tr>  </tbody></table><div class='right'  style='background: #fff; margin-left: 230px;'><table><tr><td style='color:#5a5a5a;padding-top:10px'>Your Ref:</td></tr><tr><td style='color:#5a5a5a;padding-top:10px'>Out Ref:</td></tr><tr><td style='font-weight:bold;color:#8e8e8e;padding-top:30px'>Dear dangtq,</td></tr><tr><td style='color:#8a8a8a;line-height:20px;padding:20px 0 40px 0'>ƯQE</td></tr><tr><td style='font-weight:bold;color:#8e8e8e;'>Tuyen Le</td></tr><tr>	<td style='font-weight:bold;color:#8e8e8e;'>	Managing Partner</td></tr></table>  </div> <table class='column-1-22' width='100%' style='border-collapse: collapse;' cellspacing='0' cellpadding='0' border='0'> <tr> <td style='padding: 25px; background: #f2f2f2; text-align: center;' style='border-collapse: collapse;' cellspacing='0' cellpadding='0' border='0' valign='top'>made by <a href='https://mangools.com'>mangools.com</a> </td>  </tr></table> </td> </tr></tbody> </table></div></td> </tr></table>";  }
                _content = " <head>" +
                    "<style>.p_content p{ font-size: 14px; margin: 0; }  table.column-1-2 {  margin-right: 25px;  } @media only screen and (max-width: 640px) {  .container { width: 100% !important;  max-width: 100% !important;  }.container .show{display: none;}table.column-1-2 {  float: none !important; margin-right: 0 !important;  width: 100% !important; text-align: center !important; }.right{margin-left: 0 !important;}} @media only screen and (min-width: 641px) { .container {  width: 700px !important;  }.container .remove{display: none !important;} }</style>  " +
                    "</head> " +
                    "<div style='margin: 0; font-family: 'Source Sans Pro', sans-serif; font-size: 14px;'>" +
                    " <table style='border-collapse: collapse;' width='100%' cellspacing='0' cellpadding='0' border='0' align='center'><tr> <td style='padding: 50px 0;'> <div style='max-width: 100%; margin: 0 auto;'>  <table class='container' style='border-collapse: collapse;' width='700' cellspacing='0' cellpadding='0' border='0' align='center'>  <tbody><tr>  <td> " +

                    "<table width='100%' style='border-collapse:collapse;' cellspacing='0' cellpadding='0' border='0'> <tr><td class='show' valign='bottom' style='max-width:400px;'><p style='color:#5a5a5a;margin-bottom: 0px;'>Your Ref:   " + _yourref + "</p><p style='color:#5a5a5a;margin-top: 5px;'>Out Ref:   " + _outref + "</p></td> <td style='text-align:right'><img style='width:auto; height:110px' src='https://ci3.googleusercontent.com/proxy/Mcs1RNQrQZbufG-ry8_77wBxPn1TQ30a6rvW64dc0GEYKq5EKz0YACQ1Fb1yqwNOJAMPqj4ea2ru4naJfvFbfCg8hImIOD20tQ=s0-d-e1-ft#https://pathlaw.net/Content/News/images/logo_ipace.png'><div style='padding:7px 15px 7px 0; font-family: Roboto,sans-serif; color:#5a5a5a'>ACCESS.IP SOLUTION</div> </td></tr></table>" +



                    "<table width='100%' style='border-collapse:collapse;' cellspacing='0' cellpadding='0' border='0'>" +
                    "<tr><td class='remove' style='color:#5a5a5a;padding-top:10px; margin-bottom: 0px;'>Your Ref: " + _yourref + "</td></tr>" +
                    "<tr><td class='remove' style='color:#5a5a5a;padding-top:10px;margin-top: 5px;'>Out Ref: " + _outref + "</td></tr>" +
                    "<tr><td style='font-weight:bold;color:#4a4a4a;padding-top:30px;font-weight: bold;'>" + _dear + "</td></tr>	" +
                    "<tr>	<td style='color:#8a8a8a;line-height:23px;padding:20px 0 40px 0' class='p_content'>" + content + "</td></tr>" +
                    "<tr><td style='font-weight:bold;color:#4a4a4a;font-weight: bold;'>" + p_namereply + "</td>	</tr><tr>	" +
                    "<td style='color:#8e8e8e;padding-top: 5px;line-height: 15px;'>" + p_position_name + "</td></tr>" +
                    "</table> " +
                   
                    "<table class='column-1-22' width='100%' style='border-collapse: collapse;' cellspacing='0' cellpadding='0' border='0'> " +

                    "<tr><td style='padding-top: 10px;line-height: 15px;color: #7d7d7d;margin-top: 80px;font-size: 13px;font-weight: bold;'>" +
                    "IPath Consult Co.," +
                    "</td> </tr>  " +

                    "<tr><td style='line-height: 21px;color: #a09f9f;font-size: 13px;'>" +
                    "Level 16, Deaha Business Center" +
                    "</td> </tr>  " +

                     "<tr><td style='line-height: 21px;color: #a09f9f;font-size: 13px;'>" +
                    "360 Kim Ma, Hanoi, Viet Nam " +
                    "</td> </tr>  " +

                     "<tr><td style='line-height: 21px;color: #a09f9f;font-size: 13px;'>" +
                    "Cell phone: " + _cellphone_business + " /" +
                    "</td> </tr>  " +

                    "<tr><td style='padding-top: 0px;line-height: 21px;font-size: 13px;'>" +
                    "<a href='mailto:tuyen.le@pathlaw.net' target='_blank' style='color: #a09f9f;text-decoration:none;'>Email: " +
                     emailfrom_business +
                    "</a></td> </tr> <tr>	<td style='padding-top: 0px;line-height: 21px;font-size: 13px;font-weight: bold'>" +
                    "<a href='" + _urlweb_business + "' target='_blank' style='text-decoration:none; color: #7d7d7d;font-style: italic;'>www.pathlaw.net</a>" +
                    "</td>  </tr> </table> </td>  </tr></tbody>  </table> </div>  </td></tr></table> ";
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return _content;
        }


        public static string EncryptString(string strDataToEncrypt, string strKey = "")
        {
            if (string.IsNullOrEmpty(strKey))
                strKey = gKeyEncrypt;
            //
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(strKey));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(strDataToEncrypt);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }

        public static string DecryptString(string strEncryptedString, string strKey = "")
        {
            if (string.IsNullOrEmpty(strKey))
                strKey = gKeyEncrypt;
            //
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(strKey));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]

            byte[] DataToDecrypt = Convert.FromBase64String(strEncryptedString);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

        public static string Encrypt(string toEncrypt)
        {
            try
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] hashedBytes;
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(toEncrypt));
                StringBuilder s = new StringBuilder();
                foreach (byte _hashedByte in hashedBytes)
                {
                    s.Append(_hashedByte.ToString("x2"));
                }
                return s.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static string Export_Billing(string p_case_code)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                SearchObject_Header_Info SearchObject_Header_Info = new SearchObject_Header_Info();
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();
                Billing_Header_Info _Billing_Header_Info = _obj_bl.Billing_Search_GetBy_Code(p_case_code, AppsCommon.GetCurrentLang(), ref SearchObject_Header_Info, ref _lst_billing_detail);
                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Biling_Search_Report.doc");
                DocumentModel document = DocumentModel.Load(_fileTemp);
                string fileName_exp = "Biling_Search_Report" + p_case_code + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                string fileName_exp_doc = "/Content/Export/" + "Biling_Search_Report" + p_case_code + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

                UserBL _UserBL = new UserBL();
                UserInfo userInfo = _UserBL.GetUserByUsername(SearchObject_Header_Info.CREATED_BY);

                _Billing_Header_Info.CustomerName = userInfo.FullName;
                _Billing_Header_Info.Address = userInfo.Address;
                _Billing_Header_Info.Contract = userInfo.Contact_Person + " " + userInfo.FullName;

                #region MyRegion

                // Fill export_header

                string fileName = System.Web.HttpContext.Current.Server.MapPath(fileName_exp);
                string fileName_doc = System.Web.HttpContext.Current.Server.MapPath(fileName_exp_doc);

                document.MailMerge.FieldMerging += (sender, e) =>
                {
                    if (e.IsValueFound)
                    {
                        if (e.FieldName == "Text")
                            ((Run)e.Inline).Text = e.Value.ToString();
                    }
                };

                document.MailMerge.Execute(new { DateNo = DateTime.Now.ToString("dd-MM-yyyy") });
                document.MailMerge.Execute(new { Case_Name = SearchObject_Header_Info.CASE_NAME });
                document.MailMerge.Execute(new { Client_Reference = SearchObject_Header_Info.CLIENT_REFERENCE });
                document.MailMerge.Execute(new { Case_Code = SearchObject_Header_Info.CASE_CODE });
                document.MailMerge.Execute(new { Master_Name = SearchObject_Header_Info.Customer_Name });
                //document.MailMerge.Execute(new { App_No = SearchObject_Header_Info.App_No });
                document.MailMerge.Execute(new { Customer_Country_Name = SearchObject_Header_Info.Customer_Country_Name });
                document.MailMerge.Execute(new { Bill_Code = _Billing_Header_Info.Case_Code });

                document.MailMerge.Execute(new { Total_Amount = _Billing_Header_Info.Total_Amount.ToString("#,##0.##") });
                document.MailMerge.Execute(new { Total_Pre_Tex = _Billing_Header_Info.Total_Pre_Tex.ToString("#,##0.##") });
                document.MailMerge.Execute(new { Tex_Fee = _Billing_Header_Info.Tex_Fee.ToString("#,##0.##") });
                document.MailMerge.Execute(new { Discount_Fee_Service = _Billing_Header_Info.Discount_Fee_Service.ToString("#,##0.##") });
                document.MailMerge.Execute(new { Currency = _Billing_Header_Info.Currency });

                document.MailMerge.Execute(new { Deadline = _Billing_Header_Info.Deadline.ToString("dd/MM/yyyy") });
                document.MailMerge.Execute(new { Billing_Date = _Billing_Header_Info.Billing_Date.ToString("dd/MM/yyyy") });

                if (userInfo != null)
                {
                    document.MailMerge.Execute(new { Contact_Person = userInfo.Contact_Person + " " + userInfo.FullName });
                    document.MailMerge.Execute(new { Address = userInfo.Address });
                    document.MailMerge.Execute(new { FullName = userInfo.FullName });
                }
                else
                {
                    document.MailMerge.Execute(new { Contact_Person = "" });
                    document.MailMerge.Execute(new { Address = "" });
                    document.MailMerge.Execute(new { FullName = "" });
                }

                DataTable dtDetail = new DataTable();
                dtDetail = ConvertData.ConvertToDatatable<Billing_Detail_Info>(_lst_billing_detail);
                document.MailMerge.Execute(dtDetail, "TEMP");

                document.Save(fileName, SaveOptions.PdfDefault);
                //document.Save(fileName_doc, SaveOptions.DocxDefault);

                byte[] fileContents;
                var options = SaveOptions.PdfDefault;
                // Save document to DOCX format in byte array.
                using (var stream = new MemoryStream())
                {
                    document.Save(stream, options);
                    fileContents = stream.ToArray();
                }
                Convert.ToBase64String(fileContents);
                #endregion

                return fileName_exp;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static string Export_Billing_Crytal(string p_case_code, string p_MapPath_Report, string p_mapPath,
            string p_customer,
            Billing_Header_Info _Billing_Header_Info, List<Billing_Detail_Info> _lst_billing_detail)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                string fileName_exp = "/Content/Export/Biling_Report_" + p_case_code + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";

                UserBL _UserBL = new UserBL();
                UserInfo userInfo = _UserBL.GetUserByUsername(p_customer);

                _Billing_Header_Info.CustomerName = userInfo.Company_Name;
                _Billing_Header_Info.Address = userInfo.Address;
                _Billing_Header_Info.Contract = userInfo.Contact_Person + " " + userInfo.FullName;
                _Billing_Header_Info.Notes = userInfo.Contact_Person + " " + userInfo.FullName;

                List<Billing_Header_Info> _lst = new List<Billing_Header_Info>();
                _lst.Add(_Billing_Header_Info);

                string _tempfile = "Billing.rpt";
                if (userInfo.Country != 234 
                    || p_case_code.Contains(TradeMarkAppCode.AppCodeDangKynhanHieuNN)
                    || p_case_code.Contains(TradeMarkAppCode.AppCode_LPT))
                {
                    _tempfile = "Billing_EN.rpt";
                    foreach (Billing_Detail_Info item in _lst_billing_detail)
                    {
                        item.Biling_Detail_Name = item.Biling_Detail_Name_EN;
                    }
                }

                DataTable _dt_header = ConvertData.ConvertToDatatable<Billing_Header_Info>(_lst, false);
                DataTable _dtDetail = ConvertData.ConvertToDatatable<Billing_Detail_Info>(_lst_billing_detail, false);

                DataSet _ds = new DataSet();
                _ds.Tables.Add(_dt_header);
                _ds.Tables[0].TableName = "Table1";

                _ds.Tables.Add(_dtDetail);
                _ds.Tables[1].TableName = "Table";

                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                oRpt.Load(Path.Combine(p_MapPath_Report, _tempfile));

                if (_ds != null)
                {
                    //oRpt.SetDataSource(_ds);

                    // sửa kiểu này để tránh trường hợp Database logon failed.
                    // "File"->"Options"->"Reporting"->Uncheck "Save Data With Report"
                    oRpt.Database.Tables["Table1"].SetDataSource(_dt_header);
                    oRpt.Database.Tables["Table"].SetDataSource(_dtDetail); // Don't 
                }
                oRpt.Refresh();

                string file = System.IO.Path.Combine(p_mapPath + fileName_exp);

                System.IO.Stream oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat);
                byte[] byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
                System.IO.File.WriteAllBytes(file, byteArray.ToArray()); // Requires System.Linq

                return fileName_exp;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        // dùng cho form xem
        public static string Export_Billing_Crytal_View(string p_case_code, string p_MapPath_Report, string p_mapPath)
        {
            try
            {
                Billing_BL _obj_bl = new Billing_BL();
                Billing_Header_Info _Billing_Header_Info = new Billing_Header_Info();
                string _created_by = "";
                List<Billing_Detail_Info> _lst_billing_detail = new List<Billing_Detail_Info>();

                if (p_case_code.Contains("SEARCH") || p_case_code.Contains("L"))
                {
                    SearchObject_Header_Info SearchObject_Header_Info = new SearchObject_Header_Info();

                    _Billing_Header_Info = _obj_bl.Billing_Search_GetBy_Code(p_case_code, AppsCommon.GetCurrentLang(), ref SearchObject_Header_Info, ref _lst_billing_detail);
                    _created_by = SearchObject_Header_Info.CREATED_BY;
                }
                else
                {
                    ApplicationHeaderInfo _ApplicationHeaderInfo = new ApplicationHeaderInfo();
                    _Billing_Header_Info = _obj_bl.Billing_GetBy_Code(p_case_code, AppsCommon.GetCurrentLang(), ref _ApplicationHeaderInfo, ref _lst_billing_detail);
                    _created_by = _ApplicationHeaderInfo.Created_By;
                }

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                return Export_Billing_Crytal(p_case_code, p_MapPath_Report, p_mapPath, _created_by, _Billing_Header_Info, _lst_billing_detail);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static List<Billing_Detail_Info> Get_LstFee_Detail(string p_case_code)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = (List<Billing_Detail_Info>)SessionData.GetDataSession(p_case_code);
                if (_lst_billing_detail == null)
                {
                    _lst_billing_detail = new List<Billing_Detail_Info>();
                }

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    item.Total_Fee = item.Nation_Fee + item.Represent_Fee + item.Service_Fee;
                }

                return _lst_billing_detail;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Billing_Detail_Info>();
            }
        }

        public static decimal Get_Currentcy_VCB()
        {
            try
            {
                string _url = CommonFuc.GetConfig("Link_VCB");
                var response = new WebClient().DownloadString(_url);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, true);


                XmlElement _ExrateList = doc["ExrateList"];
                foreach (XmlElement item in _ExrateList)
                {

                    XmlElement _XmlElement = (XmlElement)item;
                    if (_XmlElement != null)
                    {
                        string _currentcy = _XmlElement.GetAttribute("CurrencyCode");
                        if (_currentcy == "USD")
                        {
                            return Convert.ToDecimal(_XmlElement.GetAttribute("Sell"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return 0;
        }

        public static decimal Insert_Billing_4Application(string p_app_Case_Code, string p_note, decimal p_insert_type, string p_mapPath_Report, string p_mapPath, ref string p_fileExport)
        {
            try
            {
                List<Billing_Detail_Info> _lst_billing_detail = AppsCommon.Get_LstFee_Detail(p_app_Case_Code);
                if (_lst_billing_detail.Count == 0)
                {
                    return -2;
                }

                if (_lst_billing_detail.Count == 1 && _lst_billing_detail[0].Total_Fee == 0)
                {
                    return -2;
                }

                decimal _ck = 0;

                Billing_BL _Billing_BL = new Billing_BL();
                Billing_Header_Info p_Billing_Header_Info = new Billing_Header_Info();
                p_Billing_Header_Info.Created_By = SessionData.CurrentUser.Username;
                p_Billing_Header_Info.Created_Date = DateTime.Now;
                p_Billing_Header_Info.Language_Code = AppsCommon.GetCurrentLang();
                p_Billing_Header_Info.Status = (decimal)CommonEnums.Billing_Status.New_Wait_Approve;
                p_Billing_Header_Info.Billing_Type = (decimal)CommonEnums.Billing_Type.App;
                p_Billing_Header_Info.Insert_Type = p_insert_type;

                p_Billing_Header_Info.Notes = "Billing for case code " + p_app_Case_Code; // + " - " + p_note;

                p_Billing_Header_Info.Case_Code = _Billing_BL.Billing_GenCaseCode();
                p_Billing_Header_Info.App_Case_Code = p_app_Case_Code;

                p_Billing_Header_Info.Billing_Date = DateTime.Now;
                p_Billing_Header_Info.Deadline = DateTime.Now.AddDays(30);

                p_Billing_Header_Info.Request_By = SessionData.CurrentUser.Username;
                p_Billing_Header_Info.Approve_By = "";

                decimal _Total_Amount_Represent = 0;
                decimal _Total_Amount_Temp = 0;
                decimal _Percent_discount = 0;
                List<AllCodeInfo> _lstDiscount = WebApps.CommonFunction.AppsCommon.AllCode_GetBy_CdTypeCdName("DISCOUNT", "SERVICE");
                if (_lstDiscount.Count > 0)
                {
                    _Percent_discount = Convert.ToDecimal(_lstDiscount[0].CdVal);
                }

                foreach (Billing_Detail_Info item in _lst_billing_detail)
                {
                    _Total_Amount_Represent = _Total_Amount_Represent + item.Represent_Fee;
                    _Total_Amount_Temp = _Total_Amount_Temp + item.Total_Fee;
                }

                decimal _discount = Math.Round(_Total_Amount_Represent * _Percent_discount / 100);
                p_Billing_Header_Info.Total_Pre_Tex = _Total_Amount_Temp - _discount;
                p_Billing_Header_Info.Tex_Fee = Math.Round(p_Billing_Header_Info.Total_Pre_Tex / 100 * Common.Common.Tax);
                p_Billing_Header_Info.Total_Amount = p_Billing_Header_Info.Total_Pre_Tex + p_Billing_Header_Info.Tex_Fee;
                p_Billing_Header_Info.Discount_Fee_Service = _discount;
                p_Billing_Header_Info.Percent_Discount = _Percent_discount;
                p_Billing_Header_Info.Currency_Rate = AppsCommon.Get_Currentcy_VCB();
                decimal _idBilling = _Billing_BL.Billing_Insert(p_Billing_Header_Info);

                if (_idBilling > 0 && _lst_billing_detail.Count > 0)
                {
                    _ck = _Billing_BL.Billing_Detail_InsertBatch(_lst_billing_detail, _idBilling);
                }

                //p_fileExport = Export_Billing(p_Billing_Header_Info.Case_Code);
                // lấy thông tin khách hàng
                Application_Header_BL _app_bl = new Application_Header_BL();
                ApplicationHeaderInfo _appHeader = _app_bl.GetApp_By_Case_Code(p_app_Case_Code);

                p_Billing_Header_Info.Customer = _appHeader.Created_By;
                p_Billing_Header_Info.Email_Customer = _appHeader.Email_Customer;
                p_Billing_Header_Info.Country_Customer = _appHeader.Customer_Country;


                string _fileExport = AppsCommon.Export_Billing_Crytal(p_Billing_Header_Info.Case_Code, p_mapPath_Report, p_mapPath, _appHeader.Created_By, p_Billing_Header_Info, _lst_billing_detail);

                // nếu kết xuất file thành công thì insert vào docking
                Insert_Docketing(p_Billing_Header_Info.App_Case_Code, "Report Billing", _fileExport, true);

                p_fileExport = _fileExport;
                return _idBilling;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public static void Insert_Docketing(string p_app_case_code, string p_doc_name, string p_url_File_Atachment, bool p_is_transaction = false)
        {
            try
            {
                // insert vào docking để lưu trữ
                Docking_BL _obj_docBL = new Docking_BL();
                Docking_Info p_Docking_Info = new Docking_Info();
                p_Docking_Info.Created_By = SessionData.CurrentUser.Username;
                p_Docking_Info.Created_Date = DateTime.Now;
                p_Docking_Info.Language_Code = AppsCommon.GetCurrentLang();
                p_Docking_Info.Status = (decimal)CommonEnums.Docking_Status.Completed;
                p_Docking_Info.Docking_Type = (decimal)CommonEnums.Docking_Type_Enum.In_Book;
                p_Docking_Info.Document_Type = (decimal)CommonEnums.Document_Type_Enum.Khac;

                p_Docking_Info.Document_Name = p_doc_name;
                p_Docking_Info.Document_Name_Other = p_doc_name;

                #region MyRegion
                //// A01 - Đk cấp bằng sáng chế 
                //if (p_app_case_code.Contains("PT"))
                //{
                //    p_Docking_Info.Document_Name_Type = p_doc_name;
                //}
                //// A04 - Đăng ký nhãn hiệu
                //else if (p_app_case_code.Contains("TM"))
                //{
                //    p_Docking_Info.Document_Name_Type = p_doc_name;
                //}
                //// A03 - ĐK kiểu dáng công nghiệp 
                //else if (p_app_case_code.Contains("ID"))
                //{
                //    p_Docking_Info.Document_Name_Type = p_doc_name;
                //}
                //// B01 - Sửa đổi đơn đăng ký sở hữu công nghiệp
                //else if (p_app_case_code.Contains("B01"))
                //{
                //    p_Docking_Info.Document_Name_Type = p_doc_name;
                //}
                //// B02 - Chuyển giao đơn
                //else if (p_app_case_code.Contains("B02"))
                //{
                //    p_Docking_Info.Document_Name_Type = p_doc_name;
                //}
                //// A03 - ĐK kiểu dáng công nghiệp 
                //else if (p_app_case_code.Contains("ID"))
                //{
                //    p_Docking_Info.Document_Name_Type = p_doc_name;
                //}
                //else
                //{
                //    p_Docking_Info.Document_Name_Type = "CV";
                //} 
                #endregion

                p_Docking_Info.In_Out_Date = DateTime.Now;
                p_Docking_Info.Isshowcustomer = 1;
                p_Docking_Info.App_Case_Code = p_app_case_code;

                //
                string[] _arr = p_url_File_Atachment.Split('/');
                if (_arr.Length > 0)
                {
                    p_Docking_Info.FileName = _arr[_arr.Length - 1];
                }

                p_Docking_Info.Url = p_url_File_Atachment;

                if (p_is_transaction == false)
                {
                    _obj_docBL.Docking_Insert(p_Docking_Info);
                }
                else
                {
                    _obj_docBL.Docking_Insert_Transaction(p_Docking_Info);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static List<Sys_Document_Info> Sys_Document_GetBy_Casecode(string p_appcode)
        {
            try
            {
                return MemoryData.c_lstSys_Document.FindAll(x => x.AppCode == p_appcode).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Sys_Document_Info>();
            }
        }

        public static List<AllCodeInfo> AllCode_GetBy_CdTypeCdName(string p_cdname, string p_cdtype, string p_lang = "VI_VN")
        {
            try
            {
                if (MemoryData.c_hs_Allcode.ContainsKey(p_cdname + "|" + p_cdtype))
                {
                    List<AllCodeInfo> _lst = new List<AllCodeInfo>();

                    List<AllCodeInfo> _lst_tem = (List<AllCodeInfo>)MemoryData.c_hs_Allcode[p_cdname + "|" + p_cdtype];
                    string language = WebApps.CommonFunction.AppsCommon.GetCurrentLang();

                    foreach (AllCodeInfo item in _lst_tem)
                    {
                        AllCodeInfo _AllCodeInfo = new AllCodeInfo();
                        _AllCodeInfo.CdName = item.CdName;
                        _AllCodeInfo.CdType = item.CdType;
                        _AllCodeInfo.CdVal = item.CdVal;
                        _AllCodeInfo.Content = item.Content;
                        _AllCodeInfo.Content_Eng = item.Content_Eng;
                        _AllCodeInfo.LstOdr = item.LstOdr;
                        _lst.Add(_AllCodeInfo);
                    }

                    _lst = _lst.OrderBy(m => m.LstOdr).ToList();
                    foreach (var item in _lst)
                    {
                        if (language != "VI_VN")
                        {
                            item.Content = item.Content_Eng;
                        }
                    }

                    return _lst;
                }
                else return new List<AllCodeInfo>();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AllCodeInfo>();
            }
        }

        public static string Get_Databy_ColumName(Sys_App_Translate_Info p_Info, DataSet p_ds)
        {
            try
            {
                string _re = "";
                DataTable p_dt = new DataTable();
                if (p_Info.Type != "CLASS")
                {
                    if (p_Info.Type == "HEADER")
                    {
                        p_dt = p_ds.Tables[0];
                    }
                    else if (p_Info.Type == "DETAIL")
                    {
                        p_dt = p_ds.Tables[1];
                    }

                    foreach (DataColumn item in p_dt.Columns)
                    {
                        if (item.ColumnName.ToUpper() == p_Info.Object_Name.ToUpper())
                        {
                            return p_dt.Rows[0][p_Info.Object_Name.ToUpper()].ToString();
                        }
                    }
                }

                return _re;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static string Get_Databy_Translate_ColumName(Sys_App_Translate_Info p_Info, List<App_Translate_Info> Lst_Translate_App)
        {
            try
            {
                string _re = "";
                foreach (App_Translate_Info item in Lst_Translate_App)
                {
                    if (p_Info.Object_Name.ToUpper() == item.Object_Name.ToUpper())
                    {
                        return item.Value_Translate;
                    }
                }
                return _re;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static string Get_Databy_Class_Translate(string p_app_class_id, List<App_Translate_Info> Lst_Translate_App)
        {
            try
            {
                string _re = "";
                foreach (var item in Lst_Translate_App)
                {
                    if (p_app_class_id == item.Object_Name && item.Type == "CLASS")
                    {
                        return item.Value_Translate;
                    }
                }
                return _re;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static string Get_Databy_Document_Translate(string p_document_id, List<App_Translate_Info> Lst_Translate_App)
        {
            try
            {
                string _re = "";
                foreach (var item in Lst_Translate_App)
                {
                    if (p_document_id == item.Object_Name && item.Type == "OTHER_DOC")
                    {
                        return item.Value_Translate;
                    }
                }
                return _re;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }


        public static void Overwrite_DataSouce_Export(ref DataSet p_ds_source, List<App_Translate_Info> Lst_Translate_App)
        {
            try
            {
                Dictionary<string, App_Translate_Info> _dic_tran = new Dictionary<string, App_Translate_Info>();
                foreach (App_Translate_Info item in Lst_Translate_App)
                {
                    _dic_tran[item.Object_Name.ToUpper()] = item;
                }

                foreach (DataColumn item in p_ds_source.Tables[0].Columns)
                {
                    if (_dic_tran.ContainsKey(item.ColumnName.ToUpper()))
                    {
                        p_ds_source.Tables[0].Rows[0][item.ColumnName.ToUpper()] = _dic_tran[item.ColumnName.ToUpper()].Value_Translate;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void Overwrite_Class(ref List<AppClassDetailInfo> p_list_class, List<App_Translate_Info> Lst_Translate_App)
        {
            try
            {
                Dictionary<string, App_Translate_Info> _dic_tran = new Dictionary<string, App_Translate_Info>();
                foreach (App_Translate_Info item in Lst_Translate_App)
                {
                    if (item.Type == "CLASS")
                    {
                        _dic_tran[item.Object_Name.ToUpper()] = item;
                    }
                }

                foreach (AppClassDetailInfo item in p_list_class)
                {
                    if (_dic_tran.ContainsKey(item.Id.ToString()))
                    {
                        item.Textinput = _dic_tran[item.Id.ToString()].Value_Translate;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void Overwrite_Other_Document(ref List<AppDocumentOthersInfo> p_list_doc_other, List<App_Translate_Info> Lst_Translate_App)
        {
            try
            {
                Dictionary<string, App_Translate_Info> _dic_tran = new Dictionary<string, App_Translate_Info>();
                foreach (App_Translate_Info item in Lst_Translate_App)
                {
                    if (item.Type == "OTHER_DOC")
                    {
                        _dic_tran[item.Object_Name.ToUpper()] = item;
                    }
                }

                foreach (AppDocumentOthersInfo item in p_list_doc_other)
                {
                    if (_dic_tran.ContainsKey(item.Id.ToString()))
                    {
                        item.Documentname = _dic_tran[item.Id.ToString()].Value_Translate;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }


        public static void Prepare_Data_Export_A01(ref A01_Info_Export app_Detail, ApplicationHeaderInfo applicationHeaderInfo,
            List<AppDocumentInfo> appDocumentInfos, List<AppFeeFixInfo> _lst_appFeeFixInfos,
            List<AuthorsInfo> _lst_authorsInfos, List<Other_MasterInfo> _lst_Other_MasterInfo,
            List<AppClassDetailInfo> pAppClassInfo, List<AppDocumentOthersInfo> _LstDocumentOthersInfo,
            List<UTienInfo> pUTienInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {
                // copy Header
                A01_Info_Export.CopyAppHeaderInfo(ref app_Detail, applicationHeaderInfo);

                if (app_Detail.Source_PCT == null)
                {
                    app_Detail.PCT_Date = DateTime.MinValue;
                    app_Detail.PCT_Filling_Date_Qt = DateTime.MinValue;
                    app_Detail.PCT_VN_Date = DateTime.MinValue;
                }

                if (app_Detail.Source_DQSC == null)
                {
                    app_Detail.DQSC_Filling_Date = DateTime.MinValue;
                }

                if (app_Detail.Source_GPHI == null)
                {
                    app_Detail.GPHI_Filling_Date = DateTime.MinValue;
                }

                if (app_Detail.Source_PCT == "Y")
                {
                    app_Detail.DQSC_Filling_Date = DateTime.MinValue;
                    app_Detail.GPHI_Filling_Date = DateTime.MinValue;

                    //if (app_Detail.PCT_Date)
                    //{

                    //}
                }
                else if (app_Detail.Source_DQSC == "Y")
                {
                    app_Detail.GPHI_Filling_Date = DateTime.MinValue;
                    app_Detail.PCT_Date = DateTime.MinValue;
                    app_Detail.PCT_Filling_Date_Qt = DateTime.MinValue;
                    app_Detail.PCT_VN_Date = DateTime.MinValue;
                }
                else if (app_Detail.Source_GPHI == "Y")
                {
                    app_Detail.DQSC_Filling_Date = DateTime.MinValue;
                    app_Detail.PCT_Date = DateTime.MinValue;
                    app_Detail.PCT_Filling_Date_Qt = DateTime.MinValue;
                    app_Detail.PCT_VN_Date = DateTime.MinValue;
                }

                // copy tác giả
                if (_lst_authorsInfos != null && _lst_authorsInfos.Count > 0)
                {
                    A01_Info_Export.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[0], 0);
                }

                if (_lst_authorsInfos != null && _lst_authorsInfos.Count > 1)
                {
                    A01_Info_Export.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[1], 1);
                    app_Detail.Author_Others = "Y";
                }
                else
                {
                    A01_Info_Export.CopyAuthorsInfo(ref app_Detail, null, 1);
                    app_Detail.Author_Others = "N";
                }

                if (_lst_authorsInfos != null && _lst_authorsInfos.Count > 2)
                {
                    A01_Info_Export.CopyAuthorsInfo(ref app_Detail, _lst_authorsInfos[2], 2);
                }
                else
                {
                    A01_Info_Export.CopyAuthorsInfo(ref app_Detail, null, 2);
                }

                // copy chủ đơn khác
                if (_lst_Other_MasterInfo != null && _lst_Other_MasterInfo.Count > 1)
                {
                    A01_Info_Export.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[0], 0);
                }
                else
                {
                    A01_Info_Export.CopyOther_MasterInfo(ref app_Detail, null, 0);
                }

                if (_lst_Other_MasterInfo != null && _lst_Other_MasterInfo.Count > 2)
                {
                    A01_Info_Export.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[1], 1);
                }
                else
                {
                    A01_Info_Export.CopyOther_MasterInfo(ref app_Detail, null, 1);
                }

                // copy đơn ưu tiên
                if (pUTienInfo != null && pUTienInfo.Count > 0)
                {
                    A01_Info_Export.CopyUuTienInfo(ref app_Detail, pUTienInfo[0]);
                }
                else
                {
                    A01_Info_Export.CopyUuTienInfo(ref app_Detail, null);
                }

                #region Tài liệu có trong đơn

                if (_LstDocumentOthersInfo != null)
                {
                    foreach (var item in _LstDocumentOthersInfo)
                    {
                        app_Detail.strDanhSachFileDinhKem += item.Documentname + " ; ";
                    }

                    app_Detail.strDanhSachFileDinhKem = app_Detail.strDanhSachFileDinhKem.Substring(0, app_Detail.strDanhSachFileDinhKem.Length - 2);
                }

                foreach (AppDocumentInfo item in appDocumentInfos)
                {
                    if (item.Document_Id == "A01_01")
                    {
                        app_Detail.Doc_Id_1 = item.CHAR01;
                        app_Detail.Doc_Id_102 = item.CHAR02;
                        app_Detail.Doc_Id_1_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "A01_02")
                    {
                        app_Detail.Doc_Id_2 = item.CHAR01;
                        app_Detail.Doc_Id_202 = item.CHAR02;

                        app_Detail.Doc_Id_2_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "A01_03")
                    {
                        app_Detail.Doc_Id_3_Check = item.Isuse;
                        app_Detail.Doc_Id_3 = item.CHAR01;
                    }
                    else if (item.Document_Id == "A01_04")
                    {
                        app_Detail.Doc_Id_4 = item.CHAR01;
                        app_Detail.Doc_Id_402 = item.CHAR02;
                        app_Detail.Doc_Id_4_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "A01_05")
                    {
                        app_Detail.Doc_Id_5_Check = item.Isuse;
                        app_Detail.Doc_Id_5 = item.CHAR01;
                    }

                    else if (item.Document_Id == "A01_06")
                    {
                        app_Detail.Doc_Id_6_Check = item.Isuse;
                        app_Detail.Doc_Id_6 = item.CHAR01;
                    }
                    else if (item.Document_Id == "A01_07")
                    {
                        app_Detail.Doc_Id_7_Check = item.Isuse;
                        app_Detail.Doc_Id_7 = item.CHAR01;
                    }
                    else if (item.Document_Id == "A01_07")
                    {
                        app_Detail.Doc_Id_8_Check = item.Isuse;
                        app_Detail.Doc_Id_8 = item.CHAR01;
                    }

                    else if (item.Document_Id == "A01_09")
                    {
                        app_Detail.Doc_Id_9 = item.CHAR01;
                        app_Detail.Doc_Id_9_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "A01_10")
                    {
                        app_Detail.Doc_Id_10_Check = item.Isuse;
                        app_Detail.Doc_Id_10 = item.CHAR01;
                    }
                    else if (item.Document_Id == "A01_11")
                    {
                        app_Detail.Doc_Id_11 = item.CHAR01;
                        app_Detail.Doc_Id_11_Check = item.Isuse;
                    }

                    // quyền ưu tiên A01_12
                    else if (item.Document_Id == "1_TLCMQUT")
                    {
                        app_Detail.Doc_Id_12 = item.CHAR01;
                        app_Detail.Doc_Id_12_Check = item.Isuse;
                    }

                    //A01_13 
                    else if (item.Document_Id == "1_BanSaoDauTien")
                    {
                        app_Detail.Doc_Id_13 = item.CHAR01;
                        app_Detail.Doc_Id_13_Check = item.Isuse;
                    }

                    //A01_14
                    else if (item.Document_Id == "1_GiayChuyenNhuong")
                    {
                        app_Detail.Doc_Id_14 = item.CHAR01;
                        app_Detail.Doc_Id_14_Check = item.Isuse;
                    }

                    // end quyền ưu tiên

                    else if (item.Document_Id == "A01_15")
                    {
                        app_Detail.Doc_Id_15 = item.CHAR01;
                        app_Detail.Doc_Id_15_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "A01_16")
                    {
                        app_Detail.Doc_Id_16 = item.CHAR01;
                        app_Detail.Doc_Id_16_Check = item.Isuse;
                    }
                }

                // nếu ko dùng đơn ưu tiên thì ko có tài liệu quyền ưu tiên
                if (pUTienInfo == null)
                {
                    app_Detail.Doc_Id_12 = "";
                    app_Detail.Doc_Id_13 = "";
                    app_Detail.Doc_Id_14 = "";
                }

                #endregion

                #region Fee
                if (_lst_appFeeFixInfos.Count > 0)
                {
                    foreach (var item in _lst_appFeeFixInfos)
                    {
                        if (item.Fee_Id == 1)
                        {
                            app_Detail.Fee_Id_1 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_1_Check = item.Isuse;

                            app_Detail.Fee_Id_1_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 2)
                        {
                            app_Detail.Fee_Id_2 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_2_Check = item.Isuse;
                            app_Detail.Fee_Id_2_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 21)
                        {
                            app_Detail.Fee_Id_21 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_21_Check = item.Isuse;
                            app_Detail.Fee_Id_21_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 3)
                        {
                            app_Detail.Fee_Id_3 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_3_Check = item.Isuse;
                            app_Detail.Fee_Id_3_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 4)
                        {
                            app_Detail.Fee_Id_4 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_4_Check = item.Isuse;
                            app_Detail.Fee_Id_4_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 5)
                        {
                            app_Detail.Fee_Id_5 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_5_Check = item.Isuse;
                            app_Detail.Fee_Id_5_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 6)
                        {
                            app_Detail.Fee_Id_6 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_6_Check = item.Isuse;
                            app_Detail.Fee_Id_6_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 61)
                        {
                            app_Detail.Fee_Id_61 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_61_Check = item.Isuse;
                            app_Detail.Fee_Id_61_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 62)
                        {
                            app_Detail.Fee_Id_62 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_62_Check = item.Isuse;
                            app_Detail.Fee_Id_62_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 7)
                        {
                            app_Detail.Fee_Id_7 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_7_Check = item.Isuse;
                            app_Detail.Fee_Id_7_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 71)
                        {
                            app_Detail.Fee_Id_71 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_71_Check = item.Isuse;
                            app_Detail.Fee_Id_71_Val = item.Amount.ToString("#,##0.##");
                        }

                        else if (item.Fee_Id == 72)
                        {
                            app_Detail.Fee_Id_72 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_72_Check = item.Isuse;
                            app_Detail.Fee_Id_72_Val = item.Amount.ToString("#,##0.##");
                        }

                        app_Detail.Total_Fee = app_Detail.Total_Fee + item.Amount;
                        app_Detail.Total_Fee_Str = app_Detail.Total_Fee.ToString("#,##0.##");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void Prepare_Data_Export_B03(ref B03_Info_Export app_Detail, ApplicationHeaderInfo applicationHeaderInfo,
           List<AppDocumentInfo> appDocumentInfos, List<AppFeeFixInfo> _lst_appFeeFixInfos)
        {
            try
            {
                // copy Header
                B03_Info_Export.CopyAppHeaderInfo(ref app_Detail, applicationHeaderInfo);
                #region Tài liệu có trong đơn
                foreach (AppDocumentInfo item in appDocumentInfos)
                {
                    if (item.Document_Id == "B03_00")
                    {
                        app_Detail.Doc_Id_001 = item.CHAR01;
                        app_Detail.Doc_Id_002 = item.CHAR02;
                        app_Detail.Doc_Id_00_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "B03_01")
                    {
                        app_Detail.Doc_Id_011 = item.CHAR01;
                        app_Detail.Doc_Id_012 = item.CHAR02;

                        app_Detail.Doc_Id_01_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "B03_02")
                    {
                        app_Detail.Doc_Id_021 = item.CHAR01;
                        app_Detail.Doc_Id_022 = item.CHAR02;
                        app_Detail.Doc_Id_02_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "B03_03")
                    {
                        app_Detail.Doc_Id_031 = item.CHAR01;
                        app_Detail.Doc_Id_032 = item.CHAR02;
                        app_Detail.Doc_Id_03_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "B03_04")
                    {
                        app_Detail.Doc_Id_041 = item.CHAR01;
                        app_Detail.Doc_Id_042 = item.CHAR02;
                        app_Detail.Doc_Id_04_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "B03_05")
                    {
                        app_Detail.Doc_Id_051 = item.CHAR01;
                        app_Detail.Doc_Id_052 = item.CHAR02;
                        app_Detail.Doc_Id_05_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "B03_06")
                    {
                        app_Detail.Doc_Id_061 = item.CHAR01;
                        app_Detail.Doc_Id_062 = item.CHAR02;
                        app_Detail.Doc_Id_06_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "B03_07")
                    {
                        app_Detail.Doc_Id_071 = item.CHAR01;
                        app_Detail.Doc_Id_072 = item.CHAR02;
                        app_Detail.Doc_Id_07_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "B03_08")
                    {
                        app_Detail.Doc_Id_081 = item.CHAR01;
                        app_Detail.Doc_Id_082 = item.CHAR02;
                        app_Detail.Doc_Id_08_Check = item.Isuse;
                    }
                }



                #endregion

                #region Fee
                if (_lst_appFeeFixInfos.Count > 0)
                {
                    foreach (var item in _lst_appFeeFixInfos)
                    {
                        if (item.Fee_Id == 1)
                        {
                            app_Detail.Fee_Id_1 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_1_Check = item.Isuse;

                            app_Detail.Fee_Id_1_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 2)
                        {
                            app_Detail.Fee_Id_2 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_2_Check = item.Isuse;
                            app_Detail.Fee_Id_2_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 21)
                        {
                            app_Detail.Fee_Id_21 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_21_Check = item.Isuse;
                            app_Detail.Fee_Id_21_Val = item.Amount.ToString("#,##0.##");
                        }

                        app_Detail.Total_Fee = app_Detail.Total_Fee + item.Amount;
                        app_Detail.Total_Fee_Str = app_Detail.Total_Fee.ToString("#,##0.##");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void Prepare_Data_Export_3B(ref App_Detail_PLB01_SDD_Info pDetail, ApplicationHeaderInfo pInfo,
            List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {
                // copy Header
                App_Detail_PLB01_SDD_Info.CopyAppHeaderInfo(ref pDetail, pInfo);
                if (pDetail.Request_Other_Content == null)
                {
                    pDetail.Request_Other_Content = "";
                }

                #region Tài liệu có trong đơn

                if (pAppDocumentInfo.Count > 0)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "01_SDD_01")
                        {
                            pDetail.Doc_Id_1 = item.CHAR01;
                            pDetail.Doc_Id_1_Check = item.Isuse;
                            if (pDetail.Doc_Id_1 == null || pDetail.Doc_Id_1 == "" || pDetail.Doc_Id_1.Length == 0)
                            {
                                pDetail.Doc_Id_1 = ".....";
                            }
                        }
                        else if (item.Document_Id == "01_SDD_02")
                        {
                            pDetail.Doc_Id_2 = item.CHAR01;
                            pDetail.Doc_Id_2_Check = item.Isuse;

                            if (pDetail.Doc_Id_2 == null || pDetail.Doc_Id_2 == "" || pDetail.Doc_Id_2.Length == 0)
                            {
                                pDetail.Doc_Id_2 = ".....";
                            }
                        }
                        else if (item.Document_Id == "01_SDD_03")
                        {
                            pDetail.Doc_Id_3_Check = item.Isuse;
                            pDetail.Doc_Id_3 = item.CHAR01;

                            if (pDetail.Doc_Id_3 == null || pDetail.Doc_Id_3 == "" || pDetail.Doc_Id_3.Length == 0)
                            {
                                pDetail.Doc_Id_3 = ".....";
                            }
                        }
                        else if (item.Document_Id == "01_SDD_04")
                        {
                            pDetail.Doc_Id_4 = item.CHAR01;
                            pDetail.Doc_Id_4_Check = item.Isuse;

                            if (pDetail.Doc_Id_4 == null || pDetail.Doc_Id_4 == "" || pDetail.Doc_Id_4.Length == 0)
                            {
                                pDetail.Doc_Id_4 = ".....";
                            }
                        }
                        else if (item.Document_Id == "01_SDD_05")
                        {
                            pDetail.Doc_Id_5_Check = item.Isuse;
                            pDetail.Doc_Id_5 = item.CHAR01;

                            if (pDetail.Doc_Id_5 == null || pDetail.Doc_Id_5 == "" || pDetail.Doc_Id_5.Length == 0)
                            {
                                pDetail.Doc_Id_5 = ".....";
                            }
                        }

                        else if (item.Document_Id == "01_SDD_06")
                        {
                            pDetail.Doc_Id_6_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_07")
                        {
                            pDetail.Doc_Id_7_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_08")
                        {
                            pDetail.Doc_Id_8_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "01_SDD_09")
                        {
                            pDetail.Doc_Id_9 = item.CHAR01;
                            pDetail.Doc_Id_9_Check = item.Isuse;

                            if (pDetail.Doc_Id_9 == null || pDetail.Doc_Id_9 == "" || pDetail.Doc_Id_9.Length == 0)
                            {
                                pDetail.Doc_Id_9 = "...............";
                            }
                        }
                        else if (item.Document_Id == "01_SDD_10")
                        {
                            pDetail.Doc_Id_10_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_11")
                        {
                            pDetail.Doc_Id_11 = item.CHAR01;
                            pDetail.Doc_Id_11_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "01_SDD_12")
                        {
                            pDetail.Doc_Id_12 = item.CHAR01;
                            pDetail.Doc_Id_12_Check = item.Isuse;
                        }
                    }
                }

                #endregion

                #region phí
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_3B(pDetail);

                if (_lstFeeFix.Count > 0)
                {
                    pDetail.Fee_Id_1 = _lstFeeFix[0].Number_Of_Patent;
                    pDetail.Fee_Id_1_Check = _lstFeeFix[0].Isuse;
                    pDetail.Fee_Id_1_Val = _lstFeeFix[0].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[0].Amount;
                }

                if (_lstFeeFix.Count > 1)
                {
                    pDetail.Fee_Id_2 = _lstFeeFix[1].Number_Of_Patent;
                    pDetail.Fee_Id_2_Check = _lstFeeFix[1].Isuse;
                    pDetail.Fee_Id_2_Val = _lstFeeFix[1].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[1].Amount;
                }

                if (_lstFeeFix.Count > 2)
                {
                    pDetail.Fee_Id_21 = _lstFeeFix[2].Number_Of_Patent;
                    pDetail.Fee_Id_21_Check = _lstFeeFix[2].Isuse;
                    pDetail.Fee_Id_21_Val = _lstFeeFix[2].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[2].Amount;
                }

                if (_lstFeeFix.Count > 3)
                {
                    pDetail.Fee_Id_22 = _lstFeeFix[3].Number_Of_Patent;
                    pDetail.Fee_Id_22_Check = _lstFeeFix[3].Isuse;
                    pDetail.Fee_Id_22_Val = _lstFeeFix[3].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[3].Amount;
                }

                pDetail.Total_Fee_Str = pDetail.Total_Fee.ToString("#,##0.##");

                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }


        public static void Prepare_Data_Export_B02(ref App_Detail_PLB02_CGD_Info pDetail, ApplicationHeaderInfo pInfo,
           List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {
                // copy Header
                App_Detail_PLB02_CGD_Info.CopyAppHeaderInfo(ref pDetail, pInfo);

                #region Tài liệu có trong đơn

                if (pAppDocumentInfo.Count > 0)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "02_CGD_01")
                        {
                            pDetail.Doc_Id_1 = item.CHAR01;
                            pDetail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_02")
                        {
                            pDetail.Doc_Id_2 = item.CHAR01;
                            pDetail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_03")
                        {
                            pDetail.Doc_Id_3 = item.CHAR01;
                            pDetail.Doc_Id_3_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_04")
                        {
                            pDetail.Doc_Id_4_Check = item.Isuse;
                            pDetail.Doc_Id_4 = item.CHAR01;
                        }
                        else if (item.Document_Id == "02_CGD_05")
                        {
                            pDetail.Doc_Id_5 = item.CHAR01;
                            pDetail.Doc_Id_5_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "02_CGD_06")
                        {
                            pDetail.Doc_Id_6_Check = item.Isuse;
                            pDetail.Doc_Id_6 = item.CHAR01;
                        }
                        else if (item.Document_Id == "02_CGD_07")
                        {
                            pDetail.Doc_Id_7_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_08")
                        {
                            pDetail.Doc_Id_8_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "02_CGD_09")
                        {
                            pDetail.Doc_Id_9 = item.CHAR01;
                            pDetail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_010")
                        {
                            pDetail.Doc_Id_10_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_11")
                        {
                            pDetail.Doc_Id_11 = item.CHAR01;
                            pDetail.Doc_Id_11_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_12")
                        {
                            pDetail.Doc_Id_12 = item.CHAR01;
                            pDetail.Doc_Id_12_Check = item.Isuse;
                        }
                    }
                }

                #endregion


                #region phí
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_B02(pDetail);

                if (_lstFeeFix.Count > 0)
                {
                    pDetail.Fee_Id_1 = _lstFeeFix[0].Number_Of_Patent;
                    pDetail.Fee_Id_1_Check = _lstFeeFix[0].Isuse;
                    pDetail.Fee_Id_1_Val = _lstFeeFix[0].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[0].Amount;
                }

                if (_lstFeeFix.Count > 1)
                {
                    pDetail.Fee_Id_2 = _lstFeeFix[1].Number_Of_Patent;
                    pDetail.Fee_Id_2_Check = _lstFeeFix[1].Isuse;
                    pDetail.Fee_Id_2_Val = _lstFeeFix[1].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[1].Amount;
                }

                pDetail.Total_Fee_Str = pDetail.Total_Fee.ToString("#,##0.##");

                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void Prepare_Data_Export_C05(ref App_Detail_PLC05_KN_Info pDetail, ApplicationHeaderInfo pInfo,
           List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                // copy Header
                App_Detail_PLC05_KN_Info.CopyAppHeaderInfo(ref pDetail, pInfo);

                #region Tài liệu có trong đơn

                if (pAppDocumentInfo.Count > 0)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "02_CGD_01")
                        {
                            pDetail.Doc_Id_1 = item.CHAR01;
                            pDetail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_02")
                        {
                            pDetail.Doc_Id_2 = item.CHAR01;
                            pDetail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_03")
                        {
                            pDetail.Doc_Id_3 = item.CHAR01;
                            pDetail.Doc_Id_3_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_04")
                        {
                            pDetail.Doc_Id_4 = item.CHAR01;
                            pDetail.Doc_Id_4_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_05")
                        {
                            pDetail.Doc_Id_5 = item.CHAR01;
                            pDetail.Doc_Id_5_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "02_CGD_06")
                        {
                            pDetail.Doc_Id_6 = item.CHAR01;
                            pDetail.Doc_Id_6_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_07")
                        {
                            pDetail.Doc_Id_7 = item.CHAR01;
                            pDetail.Doc_Id_7_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_08")
                        {
                            pDetail.Doc_Id_8 = item.CHAR01;
                            pDetail.Doc_Id_8_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "02_CGD_09")
                        {
                            pDetail.Doc_Id_9 = item.CHAR01;
                            pDetail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_010")
                        {
                            pDetail.Doc_Id_10 = item.CHAR01;
                            pDetail.Doc_Id_10_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_11")
                        {
                            pDetail.Doc_Id_11 = item.CHAR01;
                            pDetail.Doc_Id_11_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "02_CGD_12")
                        {
                            pDetail.Doc_Id_12 = item.CHAR01;
                            pDetail.Doc_Id_12_Check = item.Isuse;
                        }
                    }
                }

                #endregion

                #region phí
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_C05(pFeeFixInfo);

                if (_lstFeeFix.Count > 0)
                {
                    pDetail.Fee_Id_1 = pFeeFixInfo[0].Number_Of_Patent;
                    pDetail.Fee_Id_1_Check = pFeeFixInfo[0].Isuse;
                    pDetail.Fee_Id_1_Val = pFeeFixInfo[0].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + pFeeFixInfo[0].Amount;
                }

                if (_lstFeeFix.Count > 1)
                {
                    pDetail.Fee_Id_2 = pFeeFixInfo[1].Number_Of_Patent;
                    pDetail.Fee_Id_2_Check = pFeeFixInfo[1].Isuse;
                    pDetail.Fee_Id_2_Val = pFeeFixInfo[1].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + pFeeFixInfo[1].Amount;
                }

                pDetail.Total_Fee_Str = pDetail.Total_Fee.ToString("#,##0.##");

                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void Prepare_Data_Export_D01(ref App_Detail_PLD01_HDCN_Info pDetail, ApplicationHeaderInfo pInfo,
           List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo)
        {
            try
            {
                // copy Header
                App_Detail_PLD01_HDCN_Info.CopyAppHeaderInfo(ref pDetail, pInfo);

                #region Tài liệu có trong đơn

                foreach (AppDocumentInfo item in pAppDocumentInfo)
                {
                    if (item.Document_Id == "PLD01_HDCB_01")
                    {
                        pDetail.Doc_Id_1 = item.CHAR01;
                        pDetail.Doc_Id_1_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_02")
                    {
                        pDetail.Doc_Id_2 = item.CHAR01;
                        pDetail.Doc_Id_21 = item.CHAR02;
                        pDetail.Doc_Id_2_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_03")
                    {
                        pDetail.Doc_Id_3 = item.CHAR01;
                        pDetail.Doc_Id_3_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_04")
                    {
                        pDetail.Doc_Id_4 = item.CHAR01;
                        pDetail.Doc_Id_4_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_05")
                    {
                        pDetail.Doc_Id_5 = item.CHAR01;
                        pDetail.Doc_Id_5_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "PLD01_HDCB_06")
                    {
                        pDetail.Doc_Id_6 = item.CHAR01;
                        pDetail.Doc_Id_6_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_07")
                    {
                        pDetail.Doc_Id_7 = item.CHAR01;
                        pDetail.Doc_Id_7_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_08")
                    {
                        pDetail.Doc_Id_8 = item.CHAR01;
                        pDetail.Doc_Id_8_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "PLD01_HDCB_09")
                    {
                        pDetail.Doc_Id_9 = item.CHAR01;
                        pDetail.Doc_Id_9_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_10")
                    {
                        pDetail.Doc_Id_10 = item.CHAR01;
                        pDetail.Doc_Id_10_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_11")
                    {
                        pDetail.Doc_Id_11 = item.CHAR01;
                        pDetail.Doc_Id_11_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_12")
                    {
                        pDetail.Doc_Id_12 = item.CHAR01;
                        pDetail.Doc_Id_12_Check = item.Isuse;
                    }

                    else if (item.Document_Id == "PLD01_HDCB_13")
                    {
                        pDetail.Doc_Id_13 = item.CHAR01;
                        pDetail.Doc_Id_13_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_14")
                    {
                        pDetail.Doc_Id_14 = item.CHAR01;
                        pDetail.Doc_Id_14_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_15")
                    {
                        pDetail.Doc_Id_15 = item.CHAR01;
                        pDetail.Doc_Id_15_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_16")
                    {
                        pDetail.Doc_Id_16 = item.CHAR01;
                        pDetail.Doc_Id_16_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_17")
                    {
                        pDetail.Doc_Id_17 = item.CHAR01;
                        pDetail.Doc_Id_17_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "PLD01_HDCB_18")
                    {
                        pDetail.Doc_Id_18 = item.CHAR01;
                        pDetail.Doc_Id_18_Check = item.Isuse;
                    }
                }

                #endregion

                #region phí
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_D01(pDetail, pFeeFixInfo);

                if (_lstFeeFix.Count > 0)
                {
                    foreach (var item in _lstFeeFix)
                    {
                        if (item.Fee_Id == 1)
                        {
                            pDetail.Fee_Id_1 = item.Number_Of_Patent;
                            pDetail.Fee_Id_1_Check = item.Isuse;
                            pDetail.Fee_Id_1_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 2)
                        {
                            pDetail.Fee_Id_2 = item.Number_Of_Patent;
                            pDetail.Fee_Id_2_Check = item.Isuse;
                            pDetail.Fee_Id_2_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 3)
                        {
                            pDetail.Fee_Id_3 = item.Number_Of_Patent;
                            pDetail.Fee_Id_3_Check = item.Isuse;
                            pDetail.Fee_Id_3_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 4)
                        {
                            pDetail.Fee_Id_4 = item.Number_Of_Patent;
                            pDetail.Fee_Id_4_Check = item.Isuse;
                            pDetail.Fee_Id_4_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 5)
                        {
                            pDetail.Fee_Id_5 = item.Number_Of_Patent;
                            pDetail.Fee_Id_5_Check = item.Isuse;
                            pDetail.Fee_Id_5_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 6)
                        {
                            pDetail.Fee_Id_6 = item.Number_Of_Patent;
                            pDetail.Fee_Id_6_Check = item.Isuse;
                            pDetail.Fee_Id_6_Val = item.Amount.ToString("#,##0.##");
                        }
                        pDetail.Total_Fee = pDetail.Total_Fee + item.Amount;
                    }
                }

                pDetail.Total_Fee_Str = pDetail.Total_Fee.ToString("#,##0.##");

                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void Prepare_Data_Export_E01(ref E01_Info_Export app_Detail, ApplicationHeaderInfo applicationHeaderInfo,
            List<AppDocumentInfo> appDocumentInfos, List<AppFeeFixInfo> _lst_appFeeFixInfos)
        {
            try
            {
                // copy Header
                E01_Info_Export.CopyAppHeaderInfo(ref app_Detail, applicationHeaderInfo);
                #region Tài liệu có trong đơn
                foreach (AppDocumentInfo item in appDocumentInfos)
                {
                    if (item.Document_Id == "E01_00")
                    {
                        app_Detail.Doc_Id_001 = item.CHAR01;
                        app_Detail.Doc_Id_002 = item.CHAR02;
                        app_Detail.Doc_Id_00_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "E01_01")
                    {
                        app_Detail.Doc_Id_011 = item.CHAR01;
                        app_Detail.Doc_Id_012 = item.CHAR02;

                        app_Detail.Doc_Id_01_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "E01_02")
                    {
                        app_Detail.Doc_Id_021 = item.CHAR01;
                        app_Detail.Doc_Id_022 = item.CHAR02;
                        app_Detail.Doc_Id_02_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "E01_03")
                    {
                        app_Detail.Doc_Id_031 = item.CHAR01;
                        app_Detail.Doc_Id_032 = item.CHAR02;
                        app_Detail.Doc_Id_03_Check = item.Isuse;
                    }
                    else if (item.Document_Id == "E01_04")
                    {
                        app_Detail.Doc_Id_041 = item.CHAR01;
                        app_Detail.Doc_Id_042 = item.CHAR02;
                        app_Detail.Doc_Id_04_Check = item.Isuse;
                    }
                }



                #endregion

                #region Fee
                if (_lst_appFeeFixInfos.Count > 0)
                {
                    foreach (var item in _lst_appFeeFixInfos)
                    {
                        if (item.Fee_Id == 1)
                        {
                            app_Detail.Fee_Id_1 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_1_Check = item.Isuse;

                            app_Detail.Fee_Id_1_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 2)
                        {
                            app_Detail.Fee_Id_2 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_2_Check = item.Isuse;
                            app_Detail.Fee_Id_2_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 3)
                        {
                            app_Detail.Fee_Id_3 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_3_Check = item.Isuse;
                            app_Detail.Fee_Id_3_Val = item.Amount.ToString("#,##0.##");
                        }
                        else if (item.Fee_Id == 4)
                        {
                            app_Detail.Fee_Id_4 = item.Isuse == 0 ? "" : item.Number_Of_Patent.ToString();
                            app_Detail.Fee_Id_4_Check = item.Isuse;
                            app_Detail.Fee_Id_4_Val = item.Amount.ToString("#,##0.##");
                        }

                        app_Detail.Total_Fee = app_Detail.Total_Fee + item.Amount;
                        app_Detail.Total_Fee_Str = app_Detail.Total_Fee.ToString("#,##0.##");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void Prepare_Data_Export_C01(ref App_Detail_C01_Info pDetail, ApplicationHeaderInfo pInfo,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppDocumentOthersInfo> pLstImagePublic)
        {
            try
            {

                // copy Header
                App_Detail_C01_Info.CopyAppHeaderInfo(ref pDetail, pInfo);

                #region Tài liệu có trong đơn

                if (pAppDocumentInfo.Count > 0)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "C01_01")
                        {
                            pDetail.Doc_Id_1 = item.CHAR01;
                            pDetail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_02")
                        {
                            pDetail.Doc_Id_2 = item.CHAR01;
                            pDetail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_03")
                        {
                            pDetail.Doc_Id_3_Check = item.Isuse;
                            pDetail.Doc_Id_3 = item.CHAR01;
                        }
                        else if (item.Document_Id == "C01_04")
                        {
                            pDetail.Doc_Id_4 = item.CHAR01;
                            pDetail.Doc_Id_4_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_05")
                        {
                            pDetail.Doc_Id_5_Check = item.Isuse;
                            pDetail.Doc_Id_5 = item.CHAR01;
                        }

                        else if (item.Document_Id == "C01_06")
                        {
                            pDetail.Doc_Id_6 = item.CHAR01;
                            pDetail.Doc_Id_6_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_07")
                        {
                            pDetail.Doc_Id_7 = item.CHAR01;
                            pDetail.Doc_Id_7_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_08")
                        {
                            pDetail.Doc_Id_8 = item.CHAR01;
                            pDetail.Doc_Id_8_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "C01_09")
                        {
                            pDetail.Doc_Id_9 = item.CHAR01;
                            pDetail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_10")
                        {
                            pDetail.Doc_Id_10_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_11")
                        {
                            pDetail.Doc_Id_11 = item.CHAR01;
                            pDetail.Doc_Id_11_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "C01_12")
                        {
                            pDetail.Doc_Id_12 = item.CHAR01;
                            pDetail.Doc_Id_12_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "C01_13")
                        {
                            pDetail.Doc_Id_13 = item.CHAR01;
                            pDetail.Doc_Id_13_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "C01_14")
                        {
                            pDetail.Doc_Id_14 = item.CHAR01;
                            pDetail.Doc_Id_14_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "C01_15")
                        {
                            pDetail.Doc_Id_15 = item.CHAR01;
                            pDetail.Doc_Id_15_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "C01_16")
                        {
                            pDetail.Doc_Id_16 = item.CHAR01;
                            pDetail.Doc_Id_16_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "C01_17")
                        {
                            pDetail.Doc_Id_17 = item.CHAR01;
                            pDetail.Doc_Id_17_Check = item.Isuse;
                        }
                    }
                }

                #endregion

                #region phí
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_C01(pDetail, pAppDocumentInfo, pLstImagePublic);

                if (_lstFeeFix.Count > 0)
                {
                    pDetail.Fee_Id_1 = _lstFeeFix[0].Number_Of_Patent;
                    pDetail.Fee_Id_1_Check = _lstFeeFix[0].Isuse;
                    pDetail.Fee_Id_1_Val = _lstFeeFix[0].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[0].Amount;
                }
                if (_lstFeeFix.Count > 1)
                {
                    pDetail.Fee_Id_11 = _lstFeeFix[1].Number_Of_Patent;
                    pDetail.Fee_Id_11_Check = _lstFeeFix[1].Isuse;
                    pDetail.Fee_Id_11_Val = _lstFeeFix[1].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[1].Amount;
                }

                if (_lstFeeFix.Count > 2)
                {
                    pDetail.Fee_Id_12 = _lstFeeFix[2].Number_Of_Patent;
                    pDetail.Fee_Id_12_Check = _lstFeeFix[2].Isuse;
                    pDetail.Fee_Id_12_Val = _lstFeeFix[2].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[2].Amount;
                }

                if (_lstFeeFix.Count > 3)
                {
                    pDetail.Fee_Id_2 = _lstFeeFix[3].Number_Of_Patent;
                    pDetail.Fee_Id_2_Check = _lstFeeFix[3].Isuse;
                    pDetail.Fee_Id_2_Val = _lstFeeFix[3].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[3].Amount;
                }

                if (_lstFeeFix.Count > 4)
                {
                    pDetail.Fee_Id_3 = _lstFeeFix[4].Number_Of_Patent;
                    pDetail.Fee_Id_3_Check = _lstFeeFix[4].Isuse;
                    pDetail.Fee_Id_3_Val = _lstFeeFix[4].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[4].Amount;
                }

                if (_lstFeeFix.Count > 5)
                {
                    pDetail.Fee_Id_31 = _lstFeeFix[5].Number_Of_Patent;
                    pDetail.Fee_Id_31_Check = _lstFeeFix[5].Isuse;
                    pDetail.Fee_Id_31_Val = _lstFeeFix[5].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[5].Amount;
                }

                if (_lstFeeFix.Count > 6)
                {
                    pDetail.Fee_Id_32 = _lstFeeFix[6].Number_Of_Patent;
                    pDetail.Fee_Id_32_Check = _lstFeeFix[6].Isuse;
                    pDetail.Fee_Id_32_Val = _lstFeeFix[6].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[6].Amount;
                }

                pDetail.Total_Fee_Str = pDetail.Total_Fee.ToString("#,##0.##");

                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        public static void Prepare_Data_Export_C02(ref App_Detail_C02_Info pDetail, ApplicationHeaderInfo pInfo,
           List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {

                // copy Header
                App_Detail_C02_Info.CopyAppHeaderInfo(ref pDetail, pInfo);

                #region Tài liệu có trong đơn

                if (pAppDocumentInfo.Count > 0)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "C02_01")
                        {
                            pDetail.Doc_Id_1 = item.CHAR01;
                            pDetail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C02_02")
                        {
                            pDetail.Doc_Id_2 = item.CHAR01;
                            pDetail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C02_03")
                        {
                            pDetail.Doc_Id_3_Check = item.Isuse;
                            pDetail.Doc_Id_3 = item.CHAR01;
                        }
                        else if (item.Document_Id == "C02_04")
                        {
                            pDetail.Doc_Id_4 = item.CHAR01;
                            pDetail.Doc_Id_4_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C02_05")
                        {
                            pDetail.Doc_Id_5_Check = item.Isuse;
                            pDetail.Doc_Id_5 = item.CHAR01;
                        }

                        else if (item.Document_Id == "C02_06")
                        {
                            pDetail.Doc_Id_6 = item.CHAR01;
                            pDetail.Doc_Id_6_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C02_07")
                        {
                            pDetail.Doc_Id_7 = item.CHAR01;
                            pDetail.Doc_Id_7_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C02_08")
                        {
                            pDetail.Doc_Id_8 = item.CHAR01;
                            pDetail.Doc_Id_8_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "C02_09")
                        {
                            pDetail.Doc_Id_9 = item.CHAR01;
                            pDetail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C02_10")
                        {
                            pDetail.Doc_Id_10_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C02_11")
                        {
                            pDetail.Doc_Id_11 = item.CHAR01;
                            pDetail.Doc_Id_11_Check = item.Isuse;
                        }
                    }
                }

                #endregion

                #region phí
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_C02(pDetail, pAppDocumentInfo);

                if (_lstFeeFix.Count > 0)
                {
                    pDetail.Fee_Id_1 = _lstFeeFix[0].Number_Of_Patent;
                    pDetail.Fee_Id_1_Check = _lstFeeFix[0].Isuse;
                    pDetail.Fee_Id_1_Val = _lstFeeFix[0].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[0].Amount;
                }
                if (_lstFeeFix.Count > 1)
                {
                    pDetail.Fee_Id_2A = _lstFeeFix[1].Number_Of_Patent;
                    pDetail.Fee_Id_2A_Check = _lstFeeFix[1].Isuse;
                    pDetail.Fee_Id_2A_Val = _lstFeeFix[1].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[1].Amount;
                }

                if (_lstFeeFix.Count > 2)
                {
                    pDetail.Fee_Id_2B = _lstFeeFix[2].Number_Of_Patent;
                    pDetail.Fee_Id_2B_Check = _lstFeeFix[2].Isuse;
                    pDetail.Fee_Id_2B_Val = _lstFeeFix[2].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[2].Amount;
                }

                if (_lstFeeFix.Count > 3)
                {
                    pDetail.Fee_Id_2C = _lstFeeFix[3].Number_Of_Patent;
                    pDetail.Fee_Id_2C_Check = _lstFeeFix[3].Isuse;
                    pDetail.Fee_Id_2C_Val = _lstFeeFix[3].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[3].Amount;
                }


                if (_lstFeeFix.Count > 4)
                {
                    pDetail.Fee_Id_21 = _lstFeeFix[4].Number_Of_Patent;
                    pDetail.Fee_Id_21_Check = _lstFeeFix[4].Isuse;
                    pDetail.Fee_Id_21_Val = _lstFeeFix[4].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[4].Amount;
                }

                if (_lstFeeFix.Count > 5)
                {
                    pDetail.Fee_Id_3A = _lstFeeFix[5].Number_Of_Patent;
                    pDetail.Fee_Id_3A_Check = _lstFeeFix[5].Isuse;
                    pDetail.Fee_Id_3A_Val = _lstFeeFix[5].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[5].Amount;
                }
                if (_lstFeeFix.Count > 6)
                {
                    pDetail.Fee_Id_3B = _lstFeeFix[6].Number_Of_Patent;
                    pDetail.Fee_Id_3B_Check = _lstFeeFix[6].Isuse;
                    pDetail.Fee_Id_3B_Val = _lstFeeFix[6].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[6].Amount;
                }
                if (_lstFeeFix.Count > 7)
                {
                    pDetail.Fee_Id_3C = _lstFeeFix[7].Number_Of_Patent;
                    pDetail.Fee_Id_3C_Check = _lstFeeFix[7].Isuse;
                    pDetail.Fee_Id_3C_Val = _lstFeeFix[7].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[7].Amount;
                }

                if (_lstFeeFix.Count > 8)
                {
                    pDetail.Fee_Id_4 = _lstFeeFix[8].Number_Of_Patent;
                    pDetail.Fee_Id_4_Check = _lstFeeFix[8].Isuse;
                    pDetail.Fee_Id_4_Val = _lstFeeFix[8].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[8].Amount;
                }

                if (_lstFeeFix.Count > 9)
                {
                    pDetail.Fee_Id_5 = _lstFeeFix[9].Number_Of_Patent;
                    pDetail.Fee_Id_5_Check = _lstFeeFix[9].Isuse;
                    pDetail.Fee_Id_5_Val = _lstFeeFix[9].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[9].Amount;
                }



                pDetail.Total_Fee_Str = pDetail.Total_Fee.ToString("#,##0.##");

                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        public static void Prepare_Data_Export_C03(ref App_Detail_C03_Info pDetail, ApplicationHeaderInfo pInfo,
            List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {

                // copy Header
                App_Detail_C03_Info.CopyAppHeaderInfo(ref pDetail, pInfo);

                #region Tài liệu có trong đơn

                if (pAppDocumentInfo.Count > 0)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "C01_01")
                        {
                            pDetail.Doc_Id_1 = item.CHAR01;
                            pDetail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_02")
                        {
                            pDetail.Doc_Id_2 = item.CHAR01;
                            pDetail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_03")
                        {
                            pDetail.Doc_Id_3_Check = item.Isuse;
                            pDetail.Doc_Id_3 = item.CHAR01;
                        }
                        else if (item.Document_Id == "C01_04")
                        {
                            pDetail.Doc_Id_4 = item.CHAR01;
                            pDetail.Doc_Id_4_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_05")
                        {
                            pDetail.Doc_Id_5_Check = item.Isuse;
                            pDetail.Doc_Id_5 = item.CHAR01;
                        }

                        else if (item.Document_Id == "C01_06")
                        {
                            pDetail.Doc_Id_6 = item.CHAR01;
                            pDetail.Doc_Id_6_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_07")
                        {
                            pDetail.Doc_Id_7 = item.CHAR01;
                            pDetail.Doc_Id_7_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_08")
                        {
                            pDetail.Doc_Id_8 = item.CHAR01;
                            pDetail.Doc_Id_8_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "C01_09")
                        {
                            pDetail.Doc_Id_9 = item.CHAR01;
                            pDetail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_10")
                        {
                            pDetail.Doc_Id_10_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C01_11")
                        {
                            pDetail.Doc_Id_11 = item.CHAR01;
                            pDetail.Doc_Id_11_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "C01_12")
                        {
                            pDetail.Doc_Id_12 = item.CHAR01;
                            pDetail.Doc_Id_12_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "C01_13")
                        {
                            pDetail.Doc_Id_13 = item.CHAR01;
                            pDetail.Doc_Id_13_Check = item.Isuse;
                        }
                    }
                }

                #endregion

                #region phí
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_C03(pDetail, pAppDocumentInfo);

                if (_lstFeeFix.Count > 0)
                {
                    pDetail.Fee_Id_1 = _lstFeeFix[0].Number_Of_Patent;
                    pDetail.Fee_Id_1_Check = _lstFeeFix[0].Isuse;
                    pDetail.Fee_Id_1_Val = _lstFeeFix[0].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[0].Amount;
                }
                if (_lstFeeFix.Count > 1)
                {
                    pDetail.Fee_Id_11 = _lstFeeFix[1].Number_Of_Patent;
                    pDetail.Fee_Id_11_Check = _lstFeeFix[1].Isuse;
                    pDetail.Fee_Id_11_Val = _lstFeeFix[1].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[1].Amount;
                }

                if (_lstFeeFix.Count > 2)
                {
                    pDetail.Fee_Id_2 = _lstFeeFix[2].Number_Of_Patent;
                    pDetail.Fee_Id_2_Check = _lstFeeFix[2].Isuse;
                    pDetail.Fee_Id_2_Val = _lstFeeFix[2].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[2].Amount;
                }

                if (_lstFeeFix.Count > 3)
                {
                    pDetail.Fee_Id_3 = _lstFeeFix[3].Number_Of_Patent;
                    pDetail.Fee_Id_3_Check = _lstFeeFix[3].Isuse;
                    pDetail.Fee_Id_3_Val = _lstFeeFix[3].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[3].Amount;
                }

                pDetail.Total_Fee_Str = pDetail.Total_Fee.ToString("#,##0.##");

                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void Prepare_Data_Export_F04(ref App_Detail_F04_Info pDetail, ApplicationHeaderInfo pInfo,
          List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {

                // copy Header
                App_Detail_F04_Info.CopyAppHeaderInfo(ref pDetail, pInfo);

                #region Tài liệu có trong đơn

                if (pAppDocumentInfo.Count > 0)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "F04_01")
                        {
                            pDetail.Doc_Id_1 = item.CHAR01;
                            pDetail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_02")
                        {
                            pDetail.Doc_Id_2 = item.CHAR01;
                            pDetail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_03")
                        {
                            pDetail.Doc_Id_3_Check = item.Isuse;
                            pDetail.Doc_Id_3 = item.CHAR01;
                        }
                        else if (item.Document_Id == "F04_04")
                        {
                            pDetail.Doc_Id_4 = item.CHAR01;
                            pDetail.Doc_Id_4_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_05")
                        {
                            pDetail.Doc_Id_5_Check = item.Isuse;
                            pDetail.Doc_Id_5 = item.CHAR01;
                        }

                        else if (item.Document_Id == "F04_06")
                        {
                            pDetail.Doc_Id_6 = item.CHAR01;
                            pDetail.Doc_Id_6_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_07")
                        {
                            pDetail.Doc_Id_7 = item.CHAR01;
                            pDetail.Doc_Id_7_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_08")
                        {
                            pDetail.Doc_Id_8 = item.CHAR01;
                            pDetail.Doc_Id_8_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "F04_09")
                        {
                            pDetail.Doc_Id_9 = item.CHAR01;
                            pDetail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_10")
                        {
                            pDetail.Doc_Id_101 = item.CHAR01;
                            pDetail.Doc_Id_102 = item.CHAR02;
                            if (item.CHAR03 == null || item.CHAR03 == "" || item.CHAR03 == "__/__/____")
                            {
                                item.CHAR03 = "";
                            }
                            pDetail.Doc_Id_103 = item.CHAR03;
                            pDetail.Doc_Id_10_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_11")
                        {
                            pDetail.Doc_Id_11 = item.CHAR01;
                            pDetail.Doc_Id_11_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "F04_12")
                        {
                            pDetail.Doc_Id_12 = item.CHAR01;
                            pDetail.Doc_Id_12_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "F04_13")
                        {
                            pDetail.Doc_Id_13 = item.CHAR01;
                            pDetail.Doc_Id_13_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_14")
                        {
                            pDetail.Doc_Id_14 = item.CHAR01;
                            pDetail.Doc_Id_14_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_15")
                        {
                            pDetail.Doc_Id_151 = item.CHAR01;
                            pDetail.Doc_Id_152 = item.CHAR02;
                           
                            if (item.CHAR03 == null || item.CHAR03 == "" || item.CHAR03 == "__/__/____")
                            {
                                item.CHAR03 = "";
                            }
                            pDetail.Doc_Id_153 = item.CHAR03;
                            pDetail.Doc_Id_15_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_16")
                        {
                            pDetail.Doc_Id_16 = item.CHAR01;
                            pDetail.Doc_Id_16_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_17")
                        {
                            pDetail.Doc_Id_17 = item.CHAR01;
                            pDetail.Doc_Id_17_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_18")
                        {
                            pDetail.Doc_Id_18 = item.CHAR01;
                            pDetail.Doc_Id_18_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_19")
                        {
                            pDetail.Doc_Id_19 = item.CHAR01;
                            pDetail.Doc_Id_19_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "F04_20")
                        {
                            pDetail.Doc_Id_20 = item.CHAR01;
                            pDetail.Doc_Id_20_Check = item.Isuse;
                        }
                    }
                }

                #endregion

               
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void Prepare_Data_Export_C04(ref App_Detail_C04_Info pDetail, ApplicationHeaderInfo pInfo,
           List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {

                // copy Header
                App_Detail_C04_Info.CopyAppHeaderInfo(ref pDetail, pInfo);

                #region Tài liệu có trong đơn

                if (pAppDocumentInfo.Count > 0)
                {
                    foreach (AppDocumentInfo item in pAppDocumentInfo)
                    {
                        if (item.Document_Id == "C04_01")
                        {
                            pDetail.Doc_Id_1 = item.CHAR01;
                            pDetail.Doc_Id_12 = item.CHAR02;
                            pDetail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C04_02")
                        {
                            pDetail.Doc_Id_2 = item.CHAR01;
                            pDetail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C04_03")
                        {
                            pDetail.Doc_Id_3_Check = item.Isuse;
                            pDetail.Doc_Id_3 = item.CHAR01;
                        }
                        else if (item.Document_Id == "C04_04")
                        {
                            pDetail.Doc_Id_4 = item.CHAR01;
                            pDetail.Doc_Id_4_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C04_05")
                        {
                            pDetail.Doc_Id_5_Check = item.Isuse;
                            pDetail.Doc_Id_5 = item.CHAR01;
                        }

                        else if (item.Document_Id == "C04_06")
                        {
                            pDetail.Doc_Id_6 = item.CHAR01;
                            pDetail.Doc_Id_6_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C04_07")
                        {
                            pDetail.Doc_Id_7 = item.CHAR01;
                            pDetail.Doc_Id_7_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C04_08")
                        {
                            pDetail.Doc_Id_8 = item.CHAR01;
                            pDetail.Doc_Id_8_Check = item.Isuse;
                        }

                        else if (item.Document_Id == "C04_09")
                        {
                            pDetail.Doc_Id_9 = item.CHAR01;
                            pDetail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "C04_10")
                        {
                            pDetail.Doc_Id_10_Check = item.Isuse;
                        }

                    }
                }

                #endregion

                #region phí
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_C04(pDetail);

                if (_lstFeeFix.Count > 0)
                {
                    pDetail.Fee_Id_1 = _lstFeeFix[0].Number_Of_Patent;
                    pDetail.Fee_Id_1_Check = _lstFeeFix[0].Isuse;
                    pDetail.Fee_Id_1_Val = _lstFeeFix[0].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[0].Amount;
                }
                if (_lstFeeFix.Count > 1)
                {
                    pDetail.Fee_Id_2 = _lstFeeFix[1].Number_Of_Patent;
                    pDetail.Fee_Id_2_Check = _lstFeeFix[1].Isuse;
                    pDetail.Fee_Id_2_Val = _lstFeeFix[1].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[1].Amount;
                }

                if (_lstFeeFix.Count > 2)
                {
                    pDetail.Fee_Id_3 = _lstFeeFix[2].Number_Of_Patent;
                    pDetail.Fee_Id_3_Check = _lstFeeFix[2].Isuse;
                    pDetail.Fee_Id_3_Val = _lstFeeFix[2].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[2].Amount;
                }

                if (_lstFeeFix.Count > 3)
                {
                    pDetail.Fee_Id_4 = _lstFeeFix[3].Number_Of_Patent;
                    pDetail.Fee_Id_4_Check = _lstFeeFix[3].Isuse;
                    pDetail.Fee_Id_4_Val = _lstFeeFix[3].Amount.ToString("#,##0.##");
                    pDetail.Total_Fee = pDetail.Total_Fee + _lstFeeFix[3].Amount;
                }

                pDetail.Total_Fee_Str = pDetail.Total_Fee.ToString("#,##0.##");

                #endregion
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }


    public enum LangCode
    {
        VI_VN,
        EN_US
    }

    public static class FileAutoVersioningHelper
    {
        private readonly static string _version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string GeneratePath(string fileName)
        {
            try
            {
                return string.Format("{0}?v={1}", fileName, _version);
            }
            catch
            {
                return string.Format("{0}?v={1}", fileName, DateTime.Now.ToString("dd-MM-yyyy HH"));
            }
        }
    }

    public static class UrlHelperExtensions
    {
        public static string Stylesheet(this UrlHelper helper, string fileName)
        {

            return helper.Content(string.Format("{0}", FileAutoVersioningHelper.GeneratePath(fileName)));
        }

        public static string Script(this UrlHelper helper, string fileName)
        {
            return helper.Content(string.Format("{0}", FileAutoVersioningHelper.GeneratePath(fileName)));
        }
    }


    public class DaiDienChuDon
    {
        public static string DaiDienPL = "DDPL";
        public static string DaiDienSH = "DDSH";
        public static string DaiDienUyQuyen = "DDUQ";
    }


    public class ConvertData
    {

        #region Định dạng datetime

        public const string strDate = "dd/MM/yyyy";
        public const string strDate_Time = "dd/MM/yyyy HH:mm";
        public const string strTimeFormat = "HH:mm:ss";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static DateTime ConvertStringTime2Date(string strTime)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                return DateTime.ParseExact(CorrectStringTime(strTime), strTimeFormat, provider);
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }


        /// <summary>
        /// Chuan hoa chuoi ky tu Time truyen vao dam bao phai dung dinh dang HH:mm:ss
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        private static string CorrectStringTime(string strTime)
        {
            try
            {
                string _return = "00:00:00";
                string[] _arr = strTime.Split(':');

                if (_arr.Length >= 3)
                    _return = _arr[0].PadLeft(2, '0') + ":" + _arr[1].PadLeft(2, '0') + ":" + _arr[2].PadLeft(2, '0');
                return _return;
            }
            catch
            {
                return "00:00:00";
            }
        }

        public static string ConvertDate2String(DateTime p_date)
        {
            try
            {
                return p_date.ToString(strDate);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public static DateTime ConvertString2Date(string str)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                return DateTime.ParseExact(str, strDate, provider); // strDate 
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return DateTime.MinValue;
            }
        }

        public static DateTime Convert_String_2DateTime(string p_str_datetime)
        {
            try
            {
                string[] _arr_au_date = p_str_datetime.Trim().Split('-');
                string[] _arr_time = _arr_au_date[0].Split(':');
                DateTime _dt_au = ConvertData.ConvertString2Date(_arr_au_date[1].Trim());
                DateTime _dt = new DateTime(_dt_au.Year, _dt_au.Month, _dt_au.Day, Convert.ToInt16(_arr_time[0]), Convert.ToInt16(_arr_time[1]), 0);
                return _dt;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return DateTime.MinValue;
            }
        }

        public static DateTime ConvertString2Date_dd_MM_yyyy(string str)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                return DateTime.ParseExact(str, "dd-MM-yyyy", provider); // strDate 
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Convert string to datetime format dd/MM/yyyy HH:mm
        /// </summary>
        /// <param name="str">ví dụ 17/01/2015 09:10</param>
        /// <returns></returns>
        public static DateTime ConvertString2DateWithTime(string str)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                return DateTime.ParseExact(str, strDate_Time, provider);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }


        public const string strDate_1 = "yyyy/MM/dd";
        public static string ConvertDate2String_yyyyMMdd(DateTime p_date)
        {
            try
            {
                return p_date.ToString(strDate_1);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }
        #endregion

        #region Convert DataTable

        public static void ConvertArrayListToDataTable(ArrayList arrayList, ref DataTable p_dt)
        {
            //DataTable dt = new DataTable();

            if (arrayList.Count != 0)
            {
                ConvertObjectToDataTableSchema(arrayList[0], ref p_dt);
                FillData(arrayList, p_dt);
            }

            //return p_dt;
        }

        public static void ConvertObjectToDataTableSchema(Object o, ref DataTable dt)
        {
            //DataTable dt = new DataTable();
            PropertyInfo[] properties = o.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                DataColumn dc = new DataColumn(property.Name);
                dc.DataType = property.PropertyType; dt.Columns.Add(dc);
            }
            //return dt;
        }

        private static void FillData(ArrayList arrayList, DataTable dt)
        {
            foreach (Object o in arrayList)
            {
                DataRow dr = dt.NewRow();
                PropertyInfo[] properties = o.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    dr[property.Name] = property.GetValue(o, null);
                }
                dt.Rows.Add(dr);
            }
        }

        public static DataTable ConvertToDatatable<T>(IList<T> data, bool p_isAdd_STT = true)
        {
            try
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }

                int SoThuTu = 0;
                object[] values;
                if (p_isAdd_STT == true)
                {
                    table.Columns.Add("STT");
                    values = new object[props.Count + 1];
                }
                else
                    values = new object[props.Count];

                foreach (T item in data)
                {
                    SoThuTu++;

                    if (p_isAdd_STT == true)
                    {
                        for (int i = 0; i < values.Length - 1; i++)
                        {
                            values[i] = props[i].GetValue(item);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = props[i].GetValue(item);
                        }
                    }
                    

                    if (p_isAdd_STT == true)
                    {
                        values[values.Length - 1] = SoThuTu.ToString();
                    }

                    table.Rows.Add(values);
                }
                return table;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataTable();
            }

        }

        public static DataSet ConvertToDataSet<T>(IList<T> data, bool p_isAdd_STT = true)
        {
            try
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }

                int SoThuTu = 0;
                object[] values;
                if (p_isAdd_STT == true)
                {
                    table.Columns.Add("STT");
                    values = new object[props.Count + 1];
                }
                else
                {
                    values = new object[props.Count];
                }

                foreach (T item in data)
                {
                    SoThuTu++;
                    for (int i = 0; i <= values.Length - 1; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    if (p_isAdd_STT == true)
                    {
                        values[values.Length - 1] = SoThuTu.ToString();
                    }

                    table.Rows.Add(values);
                }

                DataSet _ds = new DataSet();
                _ds.Tables.Add(table);
                return _ds;
            }
            catch (Exception)
            {
                return new DataSet();
            }

        }

        #endregion
    }
}