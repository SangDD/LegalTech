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

namespace WebApps.CommonFunction
{
    public class AppsCommon
    {
        public static string gKeyEncrypt = @"L2WcFveA50iHVuucn+bPUw==";

        public static string GetCurrentLang()
        {
            try
            {
                var culture = "";
                var httpCookie = HttpContext.Current.Request.Cookies["language"];
                var queryLanguage = "";
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
                Logger.LogException(ex);
                return Language.LangVI;
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
                //if (_ApplicationHeaderInfo.Customer_Country != Common.Common.Country_VietNam_Id)
                //    _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/Report/Biling_Report_EN.doc");
                DocumentModel document = DocumentModel.Load(_fileTemp);

                // Fill export_header
                string fileName_exp = "/Content/Export/" + "Biling_Search_Report" + p_case_code + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                string fileName_exp_doc = "/Content/Export/" + "Biling_Search_Report" + p_case_code + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

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
                document.MailMerge.Execute(new { Currency = _Billing_Header_Info.Currency });

                document.MailMerge.Execute(new { Deadline = _Billing_Header_Info.Deadline.ToString("dd/MM/yyyy") });
                document.MailMerge.Execute(new { Billing_Date = _Billing_Header_Info.Billing_Date.ToString("dd/MM/yyyy") });

                // lấy thông tin người dùng
                UserBL _UserBL = new UserBL();
                UserInfo userInfo = _UserBL.GetUserByUsername(SearchObject_Header_Info.CREATED_BY);
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

                return fileName_exp;
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
                    for (int i = 0; i < values.Length - 1; i++)
                    {
                        values[i] = props[i].GetValue(item);
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
                    for (int i = 0; i < values.Length - 1; i++)
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