﻿using System;
using System.Web.Mvc;
using WebApps.AppStart;
using BussinessFacade.ModuleTrademark;
using ObjectInfos.ModuleTrademark;
using System.Collections.Generic;
using WebApps.CommonFunction;
using WebApps.Session;
using Common;
using ObjectInfos;
using System.Web;
using GemBox.Document;
using System.IO;
using System.Transactions;
using Common.CommonData;
using BussinessFacade.ModuleMemoryData;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Linq;
using BussinessFacade;
using System.Collections;

namespace WebApps.Areas.ChiDanDiaLy.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("ChiDanDiaLyRegistration", AreaPrefix = "geographical-indication")]
    [Route("{action}")]
    public class A05Controller : Controller
    {
        [HttpGet]
        [Route("register/{id}")]
        public ActionResult Index()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");

                SessionData.CurrentUser.chashFile.Clear();
                string AppCode = "";
                if (RouteData.Values.ContainsKey("id"))
                {
                    AppCode = RouteData.Values["id"].ToString().ToUpper();
                }
                ViewBag.AppCode = AppCode;
                ViewBag.TreeTitle = "Số hình ảnh công bố";
                ViewBag.TreeLevel = 1;// upload ảnh chỉ có  cấp 
                return PartialView("~/Areas/ChiDanDiaLy/Views/A05/_Partial_A05.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/ChiDanDiaLy/Views/A05/_Partial_A05.cshtml");
            }
        }

        

        [HttpPost]
        [Route("register")]
        public ActionResult Register(ApplicationHeaderInfo pInfo, A05_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo,
             List<Other_MasterInfo> pOther_MasterInfo,
             List<AppDocumentOthersInfo> pAppDocOtherInfo )
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                A05_BL objDetail = new A05_BL();
                AppDocumentBL objDoc = new AppDocumentBL();
                Other_Master_BL _Other_Master_BL = new Other_Master_BL();
                Author_BL _Author_BL = new Author_BL();
                AppClassDetailBL objClassDetail = new AppClassDetailBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();

                var CreatedBy = SessionData.CurrentUser.Username;

                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                decimal pReturn = ErrorCode.Success;
                int pAppHeaderID = 0;
                string p_case_code = "";

                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    if (pInfo.Created_By == null || pInfo.Created_By == "0" || pInfo.Created_By == "")
                    {
                        pInfo.Created_By = CreatedBy;
                    }

                    pInfo.Created_Date = CreatedDate;
                    pInfo.Send_Date = DateTime.Now;

                    //TRA RA ID CUA BANG KHI INSERT
                    pAppHeaderID = objBL.AppHeaderInsert(pInfo, ref p_case_code);
                    if (pAppHeaderID < 0)
                        goto Commit_Transaction;

                    // detail
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pAppHeaderID;
                        pDetail.Case_Code = p_case_code;
                        if (pDetail.pfileLogo != null)
                        {
                            pDetail.IMG_URL = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                        }
                        pReturn = objDetail.Insert(pDetail);

                     
                        if (pReturn < 0)
                            goto Commit_Transaction;
                    }

                   

                    if (pOther_MasterInfo != null && pOther_MasterInfo.Count > 0)
                    {
                        foreach (var item in pOther_MasterInfo)
                        {
                            item.Case_Code = p_case_code;
                            item.App_Header_Id = pAppHeaderID;
                        }

                        decimal _re = _Other_Master_BL.Insert(pOther_MasterInfo);
                        if (_re <= 0)
                            goto Commit_Transaction;
                    }


                    //tai lieu khac 
                    if (pReturn >= 0 && pAppDocOtherInfo != null && pAppDocOtherInfo.Count > 0)
                    {
                        #region Tài liệu khác
                        int check = 0;
                        foreach (var info in pAppDocOtherInfo)
                        {
                            string _keyfileupload = "";
                            if (info.keyFileUpload != null)
                            {
                                _keyfileupload = info.keyFileUpload;
                            }
                            if (SessionData.CurrentUser.chashFile.ContainsKey(_keyfileupload))
                            {
                                var _updateitem = SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                if (_updateitem.GetType() == typeof(string))
                                {
                                    string _url = (string)_updateitem;
                                    info.Filename = _url;
                                    check = 1;
                                }

                           
                            }
                            info.App_Header_Id = pAppHeaderID;
                            info.Language_Code = language;
                        }
                        if (check == 1)
                        {
                            pReturn = objDoc.AppDocumentOtherInsertBatch(pAppDocOtherInfo);
                        }
                        #endregion
                    }


                    
                    #region tính phí
                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_A05(pDetail, pAppDocumentInfo);
                    if (_lstFeeFix.Count > 0)
                    {
                        AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                        pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, p_case_code);
                        if (pReturn < 0)
                            goto Commit_Transaction;
                    }
                    #endregion

                    #region Tai lieu dinh kem 
                    if (pReturn >= 0 && pAppDocumentInfo != null)
                    {
                        if (pAppDocumentInfo.Count > 0)
                        {
                            foreach (var info in pAppDocumentInfo)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    var _updateitem = SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    if (_updateitem.GetType() == typeof(string))
                                    {
                                        string _url = (string)_updateitem;
                                        string[] _arr = _url.Split('/');
                                        string _filename = WebApps.Resources.Resource.FileDinhKem;
                                        if (_arr.Length > 0)
                                        {
                                            _filename = _arr[_arr.Length - 1];
                                        }

                                        info.Filename = _filename;
                                        info.Url_Hardcopy = _url;
                                        info.Status = 0;
                                    }
                                }
                                info.App_Header_Id = pAppHeaderID;
                                info.Document_Filing_Date = CommonFuc.CurrentDate();
                                info.Language_Code = language;
                            }
                            pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pAppHeaderID);

                        }
                    }
                #endregion

                //end
                Commit_Transaction:
                    if (pReturn < 0)
                    {
                        Transaction.Current.Rollback();
                        return Json(new { status = -1 });

                    }
                    else
                    {
                        scope.Complete();
                    }
                }
                return Json(new { status = pAppHeaderID });

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = ErrorCode.Error });
            }
        }

        [HttpPost]
        [Route("edit")]
        public ActionResult Edit(ApplicationHeaderInfo pInfo, A05_Info pDetail,
            List<AppDocumentInfo> pAppDocumentInfo, List<AppFeeFixInfo> pFeeFixInfo, List<Other_MasterInfo> pOther_MasterInfo,
             List<AppDocumentOthersInfo> pAppDocOtherInfo )
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppFeeFixBL objFeeFixBL = new AppFeeFixBL();
                A05_BL objDetail = new A05_BL();
                AppClassDetailBL objClassDetail = new AppClassDetailBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                decimal pReturn = ErrorCode.Success;
                int pAppHeaderID = (int)pInfo.Id;
                string p_case_code = pInfo.Case_Code;

                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    pInfo.Modify_By = SessionData.CurrentUser.Username;
                    pInfo.Modify_Date = SessionData.CurrentUser.CurrentDate;
                    pInfo.Send_Date = DateTime.Now;
                    pInfo.DDSHCN = "";
                    pInfo.MADDSHCN = "";
                    //TRA RA ID CUA BANG KHI INSERT
                    pReturn = objBL.AppHeaderUpdate(pInfo);
                    if (pReturn < 0)
                        goto Commit_Transaction;

                    // detail
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.Language_Code = language;
                        pDetail.App_Header_Id = pAppHeaderID;
                        pDetail.Case_Code = p_case_code;
                        if (pDetail.pfileLogo != null)
                        {
                            pDetail.IMG_URL = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                        }
                        else
                        {
                            pDetail.IMG_URL = pDetail.IMG_URLOrg;
                        }
                        pReturn = objDetail.UpDate(pDetail);

                        if (pReturn <= 0)
                            goto Commit_Transaction;
                    }

                   

                    // ok 
                    Other_Master_BL _Other_Master_BL = new Other_Master_BL();
                    _Other_Master_BL.Deleted(pInfo.Case_Code, language);
                    if (pOther_MasterInfo != null && pOther_MasterInfo.Count > 0)
                    {
                        foreach (var item in pOther_MasterInfo)
                        {
                            item.Case_Code = pInfo.Case_Code;
                            item.App_Header_Id = pAppHeaderID;
                        }

                        decimal _re = _Other_Master_BL.Insert(pOther_MasterInfo);
                        if (_re <= 0)
                            goto Commit_Transaction;
                    }

               
                   

                    //tai lieu khac 
                    #region Tài liệu khác
                    AppDocumentBL objDoc = new AppDocumentBL();
                    List<AppDocumentOthersInfo> Lst_Doc_Others = objDoc.DocumentOthers_GetByAppHeader(pInfo.Id, language);
                    List<AppDocumentOthersInfo> Lst_Doc_Others_Old = Lst_Doc_Others.FindAll(m => m.FILETYPE == 1).ToList();
                    Dictionary<decimal, AppDocumentOthersInfo> _dic_doc_others = new Dictionary<decimal, AppDocumentOthersInfo>();
                    foreach (AppDocumentOthersInfo item in Lst_Doc_Others_Old)
                    {
                        _dic_doc_others[item.Id] = item;
                    }

                    // xóa đi trước insert lại sau
                    objDoc.AppDocumentOtherDeletedByApp_Type(pInfo.Id, language, 1);

                    if (pReturn >= 0 && pAppDocOtherInfo != null && pAppDocOtherInfo.Count > 0)
                    {
                        int check = 0;
                        foreach (var info in pAppDocOtherInfo)
                        {
                            if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                            {
                                string _url = (string)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                info.Filename = _url;
                                check = 1;
                            }
                            else if (_dic_doc_others.ContainsKey(info.Id))
                            {
                                info.Filename = _dic_doc_others[info.Id].Filename;
                                check = 1;
                            }
                            info.App_Header_Id = pAppHeaderID;
                            info.Language_Code = language;
                        }
                        if (check == 1)
                        {
                            pReturn = objDoc.AppDocumentOtherInsertBatch(pAppDocOtherInfo);
                        }
                    }
                    #endregion
                     

                    #region tính phí
                    // xóa đi
                    AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                    _AppFeeFixBL.AppFeeFixDelete(pInfo.Case_Code, language);

                    List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_A05(pDetail, pAppDocumentInfo);
                    if (_lstFeeFix.Count > 0)
                    {
                        pReturn = _AppFeeFixBL.AppFeeFixInsertBath(_lstFeeFix, p_case_code);
                        if (pReturn < 0)
                            goto Commit_Transaction;
                    }
                    #endregion

                    #region Tai lieu dinh kem 
                    if (pReturn >= 0 && pAppDocumentInfo != null && pAppDocumentInfo.Count > 0)
                    {
                        // Get ra để map sau đó xóa đi để insert vào sau
                        AppDocumentBL _AppDocumentBL = new AppDocumentBL();
                        List<AppDocumentInfo> Lst_AppDoc = _AppDocumentBL.AppDocument_Getby_AppHeader(pDetail.App_Header_Id, language);
                        Dictionary<string, AppDocumentInfo> dic_appDoc = new Dictionary<string, AppDocumentInfo>();
                        foreach (AppDocumentInfo item in Lst_AppDoc)
                        {
                            dic_appDoc[item.Document_Id] = item;
                        }

                        // xóa đi trước
                        _AppDocumentBL.AppDocumentDelByApp(pDetail.App_Header_Id, language);

                        foreach (var info in pAppDocumentInfo)
                        {
                            info.App_Header_Id = pDetail.App_Header_Id;
                            if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                            {
                                var _updateitem = SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                if (_updateitem.GetType() == typeof(string))
                                {
                                    string _url = (string)_updateitem;
                                    string[] _arr = _url.Split('/');
                                    string _filename = WebApps.Resources.Resource.FileDinhKem;
                                    if (_arr.Length > 0)
                                    {
                                        _filename = _arr[_arr.Length - 1];
                                    }

                                    info.Filename = _filename;
                                    info.Url_Hardcopy = _url;
                                    info.Status = 0;
                                }
                            }
                            else
                            {
                                if (dic_appDoc.ContainsKey(info.Document_Id))
                                {
                                    info.Filename = dic_appDoc[info.Document_Id].Filename;
                                    info.Url_Hardcopy = dic_appDoc[info.Document_Id].Url_Hardcopy;
                                    info.Status = dic_appDoc[info.Document_Id].Status;
                                }
                            }

                            info.App_Header_Id = pAppHeaderID;
                            info.Document_Filing_Date = CommonFuc.CurrentDate();
                            info.Language_Code = language;
                        }
                        pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pAppHeaderID);
                    }
                #endregion

                //end
                Commit_Transaction:
                    if (pReturn < 0)
                    {
                        Transaction.Current.Rollback();
                        return Json(new { status = -1 });

                    }
                    else
                    {
                        scope.Complete();
                    }
                }
                return Json(new { status = pAppHeaderID });

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = ErrorCode.Error });
            }
        }

         

        [HttpPost]
        [Route("ket_xuat_file_IU")]
        public ActionResult ExportData_View_IU(ApplicationHeaderInfo pInfo, A05_Info pDetail,
           List<AppDocumentInfo> pAppDocumentInfo,
           List<AppFeeFixInfo> pFeeFixInfo,
           List<Other_MasterInfo> pOther_MasterInfo,
           List<AppClassDetailInfo> pAppClassInfo,
           List<AppDocumentOthersInfo> pAppDocOtherInfo 
           )
        {
            try
            {
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string language = AppsCommon.GetCurrentLang();
                var objBL = new A05_BL();
                List<A05_Info_Export> _lst = new List<A05_Info_Export>();

                string p_appCode = "A05_Preview";

                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A05_VN_" + _datetimenow + ".pdf");
                if (language == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A05_VN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A05_VN_" + _datetimenow + ".pdf";
                }
                else
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A05_EN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A05_EN_" + _datetimenow + ".pdf";
                }

                A05_Info_Export _A05_Info_Export = new A05_Info_Export();
                A05_Info_Export.CopyA05_Info(ref _A05_Info_Export, pDetail);


                // Phí cố định

                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_A05(pDetail, pAppDocumentInfo);
                Prepare_Data_Export_A05(ref _A05_Info_Export, pInfo, pAppDocumentInfo, _lstFeeFix, pOther_MasterInfo,
                       pAppDocOtherInfo );

                _lst.Add(_A05_Info_Export);
                DataSet _ds_all = ConvertData.ConvertToDataSet<A05_Info_Export>(_lst, false);
                //_ds_all.WriteXml(@"C:\inetpub\A05.xml", XmlWriteMode.WriteSchema);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "A05.rpt";
                if (pInfo.View_Language_Report == Language.LangEN)
                {
                    _tempfile = "A05_EN.rpt";
                }
                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table1";
                    oRpt.SetDataSource(_ds_all);
                }
                oRpt.Refresh();

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                System.IO.Stream oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat);
                byte[] byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
                System.IO.File.WriteAllBytes(fileName_pdf, byteArray.ToArray()); // Requires System.Linq



                return Json(new { success = 0 });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = 0 });
            }
        }


        [HttpPost]
        [Route("ket_xuat_file")]
        public ActionResult ExportData_View(decimal pAppHeaderId, string p_appCode, string p_Language)
        {
            try
            {
                string _datetimenow = DateTime.Now.ToString("ddMMyyyyHHmm");
                string language = AppsCommon.GetCurrentLang();

                var objBL = new A05_BL();
                List<A05_Info_Export> _lst = new List<A05_Info_Export>();

                List<AppDocumentInfo> appDocumentInfos = new List<AppDocumentInfo>();
                List<AppFeeFixInfo> _lst_appFeeFixInfos = new List<AppFeeFixInfo>();
                ApplicationHeaderInfo applicationHeaderInfo = new ApplicationHeaderInfo();
                List<Other_MasterInfo> _lst_Other_MasterInfo = new List<Other_MasterInfo>();
                List<AppDocumentOthersInfo> _LstDocumentOthersInfo = new List<AppDocumentOthersInfo>();
                A05_Info_Export app_Detail = objBL.GetByID_Exp(pAppHeaderId, language, ref applicationHeaderInfo, ref appDocumentInfos, ref _lst_appFeeFixInfos,
                    ref _lst_Other_MasterInfo,  ref _LstDocumentOthersInfo );

                string fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A05_VN_" + _datetimenow + ".pdf");
                if (app_Detail.Languague_Code == Language.LangVI)
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A05_VN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A05_VN_" + _datetimenow + ".pdf";
                }
                else
                {
                    fileName_pdf = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "A05_EN_" + _datetimenow + ".pdf");
                    SessionData.CurrentUser.FilePreview = "/Content/Export/" + "A05_EN_" + _datetimenow + ".pdf";
                }

                Prepare_Data_Export_A05(ref app_Detail, applicationHeaderInfo, appDocumentInfos, _lst_appFeeFixInfos,   _lst_Other_MasterInfo,
                      _LstDocumentOthersInfo );

                _lst.Add(app_Detail);
                DataSet _ds_all = ConvertData.ConvertToDataSet<A05_Info_Export>(_lst, false);
                _ds_all.WriteXml(@"C:\inetpub\A05.xml", XmlWriteMode.WriteSchema);
                CrystalDecisions.CrystalReports.Engine.ReportDocument oRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string _tempfile = "A05.rpt";
                if (p_Language == Language.LangEN)
                {
                    _tempfile = "A05_EN.rpt";
                }
                oRpt.Load(Path.Combine(Server.MapPath("~/Report/"), _tempfile));

                if (_ds_all != null)
                {
                    _ds_all.Tables[0].TableName = "Table1";
                    oRpt.SetDataSource(_ds_all);
                }
                oRpt.Refresh();

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                //oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, fileName_pdf);

                System.IO.Stream oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat);
                byte[] byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
                System.IO.File.WriteAllBytes(fileName_pdf, byteArray.ToArray()); // Requires System.Linq


                return Json(new { success = 0 });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = 0 });
            }
        }

        public static void Prepare_Data_Export_A05(ref A05_Info_Export app_Detail, ApplicationHeaderInfo applicationHeaderInfo,
           List<AppDocumentInfo> appDocumentInfos, List<AppFeeFixInfo> _lst_appFeeFixInfos, List<Other_MasterInfo> _lst_Other_MasterInfo,
             List<AppDocumentOthersInfo> _LstDocumentOthersInfo )
        {
            try
            {
                // copy Header
                A05_Info_Export.CopyAppHeaderInfo(ref app_Detail, applicationHeaderInfo);


                // copy class
                 
                // copy tác giả
                 

                 

              
                // copy chủ đơn khác
                if (_lst_Other_MasterInfo != null && _lst_Other_MasterInfo.Count > 1)
                {
                    A05_Info_Export.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[0], 0);
                }
                else
                {
                    A05_Info_Export.CopyOther_MasterInfo(ref app_Detail, null, 0);
                }

                if (_lst_Other_MasterInfo != null && _lst_Other_MasterInfo.Count > 2)
                {
                    A05_Info_Export.CopyOther_MasterInfo(ref app_Detail, _lst_Other_MasterInfo[1], 1);
                }
                else
                {
                    A05_Info_Export.CopyOther_MasterInfo(ref app_Detail, null, 1);
                }
 

                #region Tài liệu có trong đơn

                if (_LstDocumentOthersInfo != null)
                {
                    foreach (var item in _LstDocumentOthersInfo)
                    {
                        if (!string.IsNullOrEmpty(item.Documentname))
                        {
                            app_Detail.strDanhSachFileDinhKem += item.Documentname + " ; ";
                        }
                    }
                    if (!string.IsNullOrEmpty(app_Detail.strDanhSachFileDinhKem))
                    {
                        app_Detail.strDanhSachFileDinhKem = app_Detail.strDanhSachFileDinhKem.Substring(0, app_Detail.strDanhSachFileDinhKem.Length - 2);
                    }

                }

                if (appDocumentInfos != null)
                {
                    foreach (AppDocumentInfo item in appDocumentInfos)
                    {
                        if (item.Document_Id == "A05_01")
                        {
                            app_Detail.Doc_Id_1 = item.CHAR01;
                            app_Detail.Doc_Id_102 = item.CHAR02;
                            app_Detail.Doc_Id_1_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "A05_02")
                        {
                            app_Detail.Doc_Id_2 = item.CHAR01;
                            app_Detail.Doc_Id_202 = item.CHAR02;

                            app_Detail.Doc_Id_2_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "A05_03")
                        {
                            app_Detail.Doc_Id_3_Check = item.Isuse;
                            app_Detail.Doc_Id_3 = item.CHAR01;
                            app_Detail.Doc_Id_302 = item.CHAR02;
                        }
                        else if (item.Document_Id == "A05_04")
                        {
                            app_Detail.Doc_Id_4 = item.CHAR01;
                            app_Detail.Doc_Id_4_Check = item.Isuse;
                            app_Detail.Doc_Id_402 = item.CHAR02;
                        }
                        else if (item.Document_Id == "A05_05")
                        {
                            app_Detail.Doc_Id_5_Check = item.Isuse;
                            app_Detail.Doc_Id_5 = item.CHAR01;
                        }

                        else if (item.Document_Id == "A05_06")
                        {
                            app_Detail.Doc_Id_6_Check = item.Isuse;
                            app_Detail.Doc_Id_6 = item.CHAR01;
                        }
                        else if (item.Document_Id == "A05_07")
                        {
                            app_Detail.Doc_Id_7_Check = item.Isuse;
                            app_Detail.Doc_Id_7 = item.CHAR01;
                        }
                         

                        else if (item.Document_Id == "A05_08")
                        {
                            app_Detail.Doc_Id_8_Check = item.Isuse;
                            app_Detail.Doc_Id_8 = item.CHAR01;
                        }

                        else if (item.Document_Id == "A05_09")
                        {
                            app_Detail.Doc_Id_9 = item.CHAR01;
                            app_Detail.Doc_Id_9_Check = item.Isuse;
                        }
                        else if (item.Document_Id == "A05_10")
                        {
                            app_Detail.Doc_Id_10_Check = item.Isuse;
                            app_Detail.Doc_Id_10 = item.CHAR01;
                        }
                        else if (item.Document_Id == "A05_11")
                        {
                            app_Detail.Doc_Id_11_Check = item.Isuse;
                            app_Detail.Doc_Id_11 = item.CHAR01;
                        }
                        else if (item.Document_Id == "A05_12")
                        {
                            app_Detail.Doc_Id_12_Check = item.Isuse;
                            app_Detail.Doc_Id_12 = item.CHAR01;
                        }
                        else if (item.Document_Id == "A05_13")
                        {
                            app_Detail.Doc_Id_13_Check = item.Isuse;
                            app_Detail.Doc_Id_13 = item.CHAR01;
                        }

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

        [Route("Pre-View")]
        public ActionResult PreViewApplication(string p_appCode)
        {
            try
            {
                ViewBag.FileName = SessionData.CurrentUser.FilePreview;
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration/_PartialContentPreview.cshtml");
            }
        }

        [HttpPost]
        [Route("getFee")]
        public ActionResult GetFee(A05_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo)
        {
            try
            {
                List<AppFeeFixInfo> _lstFeeFix = Call_Fee.CallFee_A05(pDetail, pAppDocumentInfo);
                ViewBag.LstFeeFix = _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            var PartialTableListFees = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Patent/Views/Shared/_PartialTableListFees.cshtml");
            var json = Json(new { success = 1, PartialTableListFees });
            return json;
        }

        [HttpPost]
        [Route("getFeeView_View")]
        public ActionResult GetFee_View(ApplicationHeaderInfo pInfo)
        {
            try
            {
                AppFeeFixBL _AppFeeFixBL = new AppFeeFixBL();
                List<AppFeeFixInfo> _lstFeeFix = _AppFeeFixBL.GetByCaseCode(pInfo.Case_Code);
                ViewBag.LstFeeFix = _lstFeeFix;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            var PartialTableListFees = AppsCommon.RenderRazorViewToString(this.ControllerContext, "~/Areas/Patent/Views/Shared/_PartialTableListFees.cshtml");
            var json = Json(new { success = 1, PartialTableListFees });
            return json;
        }
    }
}