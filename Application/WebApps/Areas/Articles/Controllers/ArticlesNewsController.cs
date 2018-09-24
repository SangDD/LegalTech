﻿using Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApps.AppStart;
using ObjectInfos;
using BussinessFacade.ModuleTrademark;
using WebApps.Session;
using BussinessFacade;

namespace WebApps.Areas.Articles.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("tintuc", AreaPrefix = "quan-ly-tin")]
    [Route("{action}")]
    public class ArticlesNewsController : Controller
    {
        // GET: Articles/ArticlesNews
        [HttpGet]
        [Route("danh-sach-tin")]
        public ActionResult GetListArticles()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                    return Redirect("/");
                decimal _total_record = 0;
                NewsBL objBL = new NewsBL();
                string _status = "ALL";
                ViewBag.Status = _status;
                string _keySearch = "ALL|" + _status;
                List<NewsInfo> _lst = objBL.ArticleHomeSearch(_keySearch, ref _total_record);
                string htmlPaging = CommonFuc.Get_HtmlPaging<NewsInfo>((int)_total_record, 1, "Tin");
                ViewBag.Obj = _lst;
                ViewBag.Paging = htmlPaging;
                ViewBag.SumRecord = _total_record;
                return View("~/Areas/Articles/Views/ArticlesNews/GetListArticles.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return View();
            }
        }
    }
}