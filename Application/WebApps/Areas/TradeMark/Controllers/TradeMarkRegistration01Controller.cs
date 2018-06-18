﻿namespace WebApps.Areas.TradeMark.Controllers
{
    using System;
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
    using BussinessFacade;
    using System.Web.Script.Serialization;

    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("TradeMarkRegistration", AreaPrefix = "trade-mark-01")]
    [Route("{action}")]
    public class TradeMarkRegistration01Controller : Controller
    {
 
       

        /// <summary>
        /// ID:ID của app_header_id 
        /// ID2: là appcode 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("request-for-trade-mark-edit/{id}/{id1}/{id2}")]
        public ActionResult TradeMarkForEdit()
        {
            decimal App_Header_Id = 0;
            string AppCode = "";
            int Status = 0;
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                SessionData.CurrentUser.chashFile.Clear();
                SessionData.CurrentUser.chashFileOther.Clear();

                if (RouteData.Values.ContainsKey("id"))
                {
                    App_Header_Id = CommonFuc.ConvertToDecimal(RouteData.Values["id"]);
                }
                if (RouteData.Values.ContainsKey("id1"))
                {
                    Status = CommonFuc.ConvertToInt(RouteData.Values["id1"]);
                }
                if (RouteData.Values.ContainsKey("id2"))
                {
                    AppCode = RouteData.Values["id2"].ToString().ToUpper();
                }

                if (AppCode == TradeMarkAppCode.AppCodeDangKyQuocTeNH)
                {
                    return TradeMarkSuaDon(App_Header_Id, AppCode, Status);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return TradeMarkSuaDon(App_Header_Id, AppCode, Status);
        }

        public ActionResult TradeMarkSuaDon(decimal pAppHeaderId, string pAppCode, int pStatus)
        {
            if (pAppCode == TradeMarkAppCode.AppCodeDangKynhanHieu)
            {
                var objBL = new AppDetail06DKQT_BL();
                string language = AppsCommon.GetCurrentLang();
                var ds06Dkqt = objBL.AppTM06DKQTGetByID(pAppHeaderId, language, pStatus);
                if (ds06Dkqt != null && ds06Dkqt.Tables.Count == 5)
                {
                    ViewBag.objAppHeaderInfo = CBO<AppDetail04NHInfo>.FillObjectFromDataTable(ds06Dkqt.Tables[0]);
                    ViewBag.lstDocumentInfo = CBO<AppDocumentInfo>.FillCollectionFromDataTable(ds06Dkqt.Tables[1]);
                    ViewBag.lstClassDetailInfo = CBO<AppClassDetailInfo>.FillCollectionFromDataTable(ds06Dkqt.Tables[2]);
                }
                AppDetail04NHBL _AppDetail04NHBL = new AppDetail04NHBL();
                List<AppDetail04NHInfo> _list04nh = new List<AppDetail04NHInfo>();
                // truyền vào trạng thái nào? để tạm thời = 1 cho có dữ liệu
                _list04nh = _AppDetail04NHBL.AppTM04NHSearchByStatus(1);
                ViewBag.ListAppDetail04NHInfo = _list04nh;
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration01/_PartialEditDangKyNhanHieu.cshtml");
            }
            else
            {
                //
                return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration01/_PartialEditDangKyNhanHieu.cshtml");
            }
        }

        public ActionResult AppDangKyNhanHieu()
        {
            try
            { 
                AppDetail04NHBL _AppDetail04NHBL = new AppDetail04NHBL();
                List<AppDetail04NHInfo> _list04nh = new List<AppDetail04NHInfo>();
                // truyền vào trạng thái nào? để tạm thời = 1 cho có dữ liệu
                _list04nh = _AppDetail04NHBL.AppTM04NHSearchByStatus(1);
                ViewBag.ListAppDetail04NHInfo = _list04nh;

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PartialView("~/Areas/TradeMark/Views/TradeMarkRegistration01/_PartalDangKyNhanHieu.cshtml");
        }



        [HttpPost]
        [Route("dang_ky_nhan_hieu")]
        public ActionResult AppDonDangKyInsert(ApplicationHeaderInfo pInfo, App_Detail_TM06DKQT_Info pDetail, List<AppDocumentInfo> pAppDocumentInfo,
         List<AppDocumentOthersInfo> pAppDocOtherInfo, List<AppClassDetailInfo> pAppClassInfo)
        {
            try
            {
                Application_Header_BL objBL = new Application_Header_BL();
                AppDetail06DKQT_BL objDetailBL = new AppDetail06DKQT_BL();
                AppClassDetailBL objClassDetail = new AppClassDetailBL();
                AppDocumentBL objDoc = new AppDocumentBL();
                if (pInfo == null || pDetail == null) return Json(new { status = ErrorCode.Error });
                string language = AppsCommon.GetCurrentLang();
                var CreatedBy = SessionData.CurrentUser.Username;
                var CreatedDate = SessionData.CurrentUser.CurrentDate;
                int pReturn = ErrorCode.Success;
                int pAppHeaderID = 0;
                using (var scope = new TransactionScope())
                {
                    //
                    pInfo.Languague_Code = language;
                    pInfo.Created_By = CreatedBy;
                    pInfo.Created_Date = CreatedDate;
                    //TRA RA ID CUA BANG KHI INSERT
                    pAppHeaderID = objBL.AppHeaderInsert(pInfo);
                    if (pAppHeaderID >= 0)
                    {
                        pDetail.Appcode = pInfo.Appcode;
                        pDetail.LANGUAGE_CODE = language;
                        pDetail.APP_HEADER_ID = pAppHeaderID;
                        if (pDetail.pfileLogo != null)
                        {
                            pDetail.LOGOURL = AppLoadHelpers.PushFileToServer(pDetail.pfileLogo, AppUpload.Logo);
                        }
                        pReturn = objDetailBL.App_Detail_06TMDKQT_Insert(pDetail);
                        //Thêm thông tin class
                        if (pReturn >= 0)
                        {
                            pReturn = objClassDetail.AppClassDetailInsertBatch(pAppClassInfo, pAppHeaderID, language);
                        }
                    }
                    //Tai lieu dinh kem 
                    if (pReturn >= 0 && pAppDocumentInfo != null)
                    {
                        if (pAppDocumentInfo.Count > 0)
                        {
                            foreach (var info in pAppDocumentInfo)
                            {
                                if (SessionData.CurrentUser.chashFile.ContainsKey(info.keyFileUpload))
                                {
                                    HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFile[info.keyFileUpload];
                                    info.Filename = pfiles.FileName;
                                    info.Url_Hardcopy = "~/Content/Archive/" + AppUpload.Document + pfiles.FileName;
                                    info.Status = 0;
                                }
                                info.App_Header_Id = pAppHeaderID;
                                info.Document_Filing_Date = CommonFuc.CurrentDate();
                                info.Language_Code = language;
                            }
                            pReturn = objDoc.AppDocumentInsertBath(pAppDocumentInfo, pAppHeaderID);

                        }
                    }
                    //tai lieu khac 
                    if (pReturn >= 0 && pAppDocOtherInfo != null)
                    {
                        if (pAppDocOtherInfo.Count > 0)
                        {
                            foreach (var info in pAppDocOtherInfo)
                            {
                                if (SessionData.CurrentUser.chashFileOther.ContainsKey(info.keyFileUpload))
                                {
                                    HttpPostedFileBase pfiles = (HttpPostedFileBase)SessionData.CurrentUser.chashFileOther[info.keyFileUpload];
                                    info.Filename = pfiles.FileName;
                                    info.Filename = "~/Content/Archive/" + AppUpload.Document + pfiles.FileName;
                                }
                                info.App_Header_Id = pAppHeaderID;
                                info.Language_Code = language;
                            }
                            pReturn = objDoc.AppDocumentOtherInsertBatch(pAppDocOtherInfo);
                        }
                    }
                    //end
                    if (pReturn < 0)
                    {
                        Transaction.Current.Rollback();
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
        [Route("ket_xuat_file")]
        public ActionResult ExportData(ApplicationHeaderInfo pInfo, List<AppFeeFixInfo> pFeeFixInfo, AppDetail01Info pDetailInfo)
        {
            try
            {
                string _fileTemp = System.Web.HttpContext.Current.Server.MapPath("/Content/AppForms/Request_for_amendment_of_application.doc");
                DocumentModel document = DocumentModel.Load(_fileTemp);

                // Fill export_header
                string fileName = System.Web.HttpContext.Current.Server.MapPath("/Content/Export/" + "Dang_ky_sua_doi_nhan_hieu_" + pInfo.Appcode + ".pdf");

                // Fill export_detail  

                pInfo.Status = 254;
                pInfo.Status_Form = 252;
                pInfo.Relationship = "11";
                document.MailMerge.Execute(pInfo);
                document.Save(fileName, SaveOptions.PdfDefault);
                //document.Save(fileName);


                byte[] fileContents;
                var options = SaveOptions.PdfDefault;
                // Save document to DOCX format in byte array.
                using (var stream = new MemoryStream())
                {
                    document.Save(stream, options);
                    fileContents = stream.ToArray();
                }
                Convert.ToBase64String(fileContents);

                return Json(new { success = 0 });

            }
            catch (Exception ex)
            {

                return Json(new { success = 0 });
            }
        }

        
    }
}