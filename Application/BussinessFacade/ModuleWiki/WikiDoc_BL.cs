﻿using Common;
using Common.SearchingAndFiltering;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using ZetaCompressionLibrary;

namespace BussinessFacade
{
   public class WikiDoc_BL: RepositoriesBL
    {
        public decimal WikiDoc_Insert(WikiDoc_Info p_ObjInffo)
        {
            try
            {
                WikiDoc_DA _da = new WikiDoc_DA();
                return _da.WikiDoc_Insert(p_ObjInffo.TITLE, p_ObjInffo.CONTENT, p_ObjInffo.LANGUAGE_CODE, p_ObjInffo.CREATED_BY,
                    p_ObjInffo.CREATED_DATE, p_ObjInffo.HASHTAG, p_ObjInffo.FILE_URL01, p_ObjInffo.FILE_URL02, p_ObjInffo.FILE_URL03,
                    p_ObjInffo.CATA_ID, p_ObjInffo.STATUS);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal WikiDoc_Update(WikiDoc_Info p_ObjInffo)
        {
            try
            {
                WikiDoc_DA _da = new WikiDoc_DA();
                return _da.WikiDoc_Update(p_ObjInffo.ID, p_ObjInffo.TITLE, p_ObjInffo.CONTENT, p_ObjInffo.LANGUAGE_CODE, p_ObjInffo.MODIFIED_BY,
                    p_ObjInffo.MODIFIED_DATE, p_ObjInffo.HASHTAG, p_ObjInffo.FILE_URL01, p_ObjInffo.FILE_URL02, p_ObjInffo.FILE_URL03,
                    p_ObjInffo.CATA_ID, p_ObjInffo.STATUS, p_ObjInffo.REFUSE_REASON);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal WikiDoc_Update_HashTag(decimal p_id, string p_hashtag)
        {
            try
            {
                WikiDoc_DA _da = new WikiDoc_DA();
                return _da.WikiDoc_Update_HashTag(p_id, p_hashtag);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal WikiDoc_Delete(decimal P_ID)
        {
            try
            {
                WikiDoc_DA _da = new WikiDoc_DA();
                return _da.WikiDoc_Delete(P_ID);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public List<WikiDoc_Info> WikiDoc_Search(string P_KEY_SEARCH = "", string OPTIONS = "")
        {
            try
            {
                WikiDoc_DA _da = new WikiDoc_DA();
                var optionFilter = new OptionFilter(OPTIONS);
                var totalRecordFindResult = 0;
                var ds = _da.WikiDoc_Search(P_KEY_SEARCH, optionFilter, ref totalRecordFindResult);
                this.SetupPagingHtml(optionFilter, totalRecordFindResult, "pageListOfObjects", "divNumberRecordOnPageListObjects");
                return CBO<WikiDoc_Info>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<WikiDoc_Info>();
            }
        }

        public WikiDoc_Info WikiDoc_GetById(decimal P_ID)
        {
            try
            {
                WikiDoc_DA _da = new WikiDoc_DA();
                var ds = _da.WikiDoc_GetById(P_ID);
                return CBO<WikiDoc_Info>.FillObjectFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new WikiDoc_Info();
            }
        }

        public List<WikiDoc_Info> WikiDoc_GetBy_CataID(decimal p_id)
        {
            try
            {
                WikiDoc_DA _da = new WikiDoc_DA();
                var ds = _da.WikiDoc_GetBy_CataID(p_id);
                return CBO<WikiDoc_Info>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<WikiDoc_Info>();
            }
        }

        public WikiDoc_Info PortalWikiDoc_GetById(decimal P_ID)
        {
            try
            {
                WikiDoc_DA _da = new WikiDoc_DA();
                var ds = _da.PortalWikiDoc_GetById(P_ID);
                return CBO<WikiDoc_Info>.FillObjectFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new WikiDoc_Info();
            }
        }

        public List<WikiDoc_Info> PortalWikiDoc_Search(string P_KEY_SEARCH = "", string OPTIONS = "")
        {
            try
            {
                WikiDoc_DA _da = new WikiDoc_DA();
                var optionFilter = new OptionFilter(OPTIONS);
                var totalRecordFindResult = 0;
                var ds = _da.PortalWikiDoc_Search(P_KEY_SEARCH, optionFilter, ref totalRecordFindResult);
                this.SetupPagingHtml(optionFilter, totalRecordFindResult, "pageListOfObjects", "divNumberRecordOnPageListObjects");
                return CBO<WikiDoc_Info>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<WikiDoc_Info>();
            }
        }
    }
}
