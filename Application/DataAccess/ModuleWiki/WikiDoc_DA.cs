﻿using Common;
using Common.SearchingAndFiltering;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Converters;
namespace DataAccess
{
  public class WikiDoc_DA
    {
        public decimal WikiDoc_Insert(string P_TITLE, string P_TITLE_EN, string P_HEADER,string P_HEADER_EN, string P_CONTENT, string P_CONTENT_EN, string P_LANGUAGE_CODE,
          string P_CREATED_BY, DateTime P_CREATED_DATE, string P_HASHTAG, string P_FILE_URL01, string P_FILE_URL02,
          string P_FILE_URL03, decimal P_CATA_ID, decimal P_STATUS)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_DOCS.PROC_WIKI_DOCS_INSERT",
                    new OracleParameter("P_TITLE", OracleDbType.Varchar2, P_TITLE, ParameterDirection.Input),
                    new OracleParameter("P_TITLE_EN", OracleDbType.Varchar2, P_TITLE_EN, ParameterDirection.Input),
                    new OracleParameter("P_HEADER", OracleDbType.Varchar2, P_HEADER, ParameterDirection.Input),
                    new OracleParameter("P_HEADER_EN", OracleDbType.Varchar2, P_HEADER_EN, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT", OracleDbType.Clob, P_CONTENT, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT_EN", OracleDbType.Clob, P_CONTENT_EN, ParameterDirection.Input),
                    new OracleParameter("P_CREATED_BY", OracleDbType.Varchar2, P_CREATED_BY, ParameterDirection.Input),
                    new OracleParameter("P_CREATED_DATE", OracleDbType.Date, P_CREATED_DATE, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, P_LANGUAGE_CODE, ParameterDirection.Input),
                    new OracleParameter("P_HASHTAG", OracleDbType.Varchar2, P_HASHTAG, ParameterDirection.Input),
                    new OracleParameter("P_FILE_URL01", OracleDbType.Varchar2, P_FILE_URL01, ParameterDirection.Input),
                    new OracleParameter("P_FILE_URL02", OracleDbType.Varchar2, P_FILE_URL02, ParameterDirection.Input),
                    new OracleParameter("P_FILE_URL03", OracleDbType.Varchar2, P_FILE_URL03, ParameterDirection.Input),                    
                    new OracleParameter("P_CATA_ID", OracleDbType.Decimal, P_CATA_ID, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, P_STATUS, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal WikiDoc_Update(decimal P_ID, string P_TITLE,string P_TITLE_EN, string P_HEADER,string P_HEADER_EN, string P_CONTENT, string P_CONTENT_EN, string P_LANGUAGE_CODE,
         string P_MODIFIED_BY, DateTime P_MODIFIED_DATE, string P_HASHTAG, string P_FILE_URL01, string P_FILE_URL02,
         string P_FILE_URL03, decimal P_CATA_ID, decimal P_STATUS, string P_NOTE)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_DOCS.PROC_WIKI_DOCS_UPDATE",
                    new OracleParameter("P_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                    new OracleParameter("P_TITLE", OracleDbType.Varchar2, P_TITLE, ParameterDirection.Input),
                    new OracleParameter("P_TITLE_EN", OracleDbType.Varchar2, P_TITLE_EN, ParameterDirection.Input),
                    new OracleParameter("P_HEADER", OracleDbType.Varchar2, P_HEADER, ParameterDirection.Input),
                    new OracleParameter("P_HEADER_EN", OracleDbType.Varchar2, P_HEADER_EN, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT", OracleDbType.Clob, P_CONTENT, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT_EN", OracleDbType.Clob, P_CONTENT_EN, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIED_BY", OracleDbType.Varchar2, P_MODIFIED_BY, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIED_DATE", OracleDbType.Date, P_MODIFIED_DATE, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, P_LANGUAGE_CODE, ParameterDirection.Input),
                    new OracleParameter("P_HASHTAG", OracleDbType.Varchar2, P_HASHTAG, ParameterDirection.Input),
                    new OracleParameter("P_FILE_URL01", OracleDbType.Varchar2, P_FILE_URL01, ParameterDirection.Input),
                    new OracleParameter("P_FILE_URL02", OracleDbType.Varchar2, P_FILE_URL02, ParameterDirection.Input),
                    new OracleParameter("P_FILE_URL03", OracleDbType.Varchar2, P_FILE_URL03, ParameterDirection.Input),
                    new OracleParameter("P_CATA_ID", OracleDbType.Decimal, P_CATA_ID, ParameterDirection.Input),                 
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, P_STATUS, ParameterDirection.Input),
                    new OracleParameter("P_NOTE", OracleDbType.Varchar2, P_NOTE, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public DataSet WikiDoc_Search(string keysSearch, OptionFilter options, ref decimal totalRecord)
        {
            try
            {
                DataSet _Ds = new DataSet();
                OracleParameter paramReturn = new OracleParameter("P_TOTAL_RECORD", OracleDbType.Decimal, ParameterDirection.Output);
                 _Ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_DOCS.PROC_WIKI_DOC_SEARCH",
                 new OracleParameter("P_KEY_SEARCH", OracleDbType.Varchar2, keysSearch.ToFillKeySearch(), ParameterDirection.Input),
                 new OracleParameter("P_FROM", OracleDbType.Varchar2, options.StartAt.ToString(), ParameterDirection.Input),
                 new OracleParameter("P_TO", OracleDbType.Varchar2, options.EndAt.ToString(), ParameterDirection.Input),
                 new OracleParameter("P_SORT_TYPE", OracleDbType.Varchar2, options.OrderBy, ParameterDirection.Input),
                 paramReturn,

                new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                totalRecord = Convert.ToDecimal(paramReturn.Value.ToString());
                return _Ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet HomeWikiDoc_Search(string p_key_search, string p_from, string p_to, string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("P_TOTAL_RECORD", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_DOCS.PROC_WIKI_DOC_SEARCH",
                    new OracleParameter("P_KEY_SEARCH", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("P_FROM", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("P_TO", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("P_SORT_TYPE", OracleDbType.Varchar2, p_sort_type, ParameterDirection.Input),
                    paramReturn,
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)
                   );
                p_total_record = Convert.ToDecimal(paramReturn.Value.ToString());
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal WikiDoc_Delete(decimal P_ID)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_DOCS.PROC_WIKI_DOCS_DELETE",
                    new OracleParameter("P_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public DataSet WikiDoc_GetById(decimal P_ID)
        {
            try
            {
                DataSet _Ds = new DataSet();
                _Ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_DOCS.PROC_WIKI_DOCS_GETBYID",
                new OracleParameter("P_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return _Ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal WikiDoc_Update_HashTag(decimal P_ID, string P_HASHTAG)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_DOCS.PROC_WIKI_DOCS_UPDATE_HASHTAG",
                    new OracleParameter("P_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                    new OracleParameter("P_HASHTAG", OracleDbType.Varchar2, P_HASHTAG, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


        public DataSet WikiDoc_GetBy_CataID(decimal p_cata_id)
        {
            try
            {
                DataSet _Ds = new DataSet();
                _Ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_PORTAL.PROC_GET_DOC_BY_CATA_ID",
                new OracleParameter("P_CATA_ID", OracleDbType.Decimal, p_cata_id, ParameterDirection.Input),
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return _Ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet PortalWikiDoc_GetById(decimal P_ID)
        {
            try
            {
                DataSet _Ds = new DataSet();
                _Ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_DOCS.PROC_PORTAL_WIKI_DOCS_GETBYID",
                new OracleParameter("P_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return _Ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        
        public DataSet PortalWikiDoc_Search(string keysSearch, OptionFilter options, ref int totalRecord)
        {
            try
            {
                DataSet _Ds = new DataSet();
                OracleParameter paramReturn = new OracleParameter("P_TOTAL_RECORD", OracleDbType.Decimal, ParameterDirection.Output);
                _Ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_PORTAL.PROC_WIKI_DOC_SEARCH",
                new OracleParameter("P_KEY_SEARCH", OracleDbType.Varchar2, keysSearch.ToFillKeySearch(), ParameterDirection.Input),
                new OracleParameter("P_FROM", OracleDbType.Varchar2, options.StartAt.ToString(), ParameterDirection.Input),
                new OracleParameter("P_TO", OracleDbType.Varchar2, options.EndAt.ToString(), ParameterDirection.Input),
                new OracleParameter("P_SORT_TYPE", OracleDbType.Varchar2, options.OrderBy, ParameterDirection.Input),
                paramReturn,
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                totalRecord = Convert.ToInt32(paramReturn.Value.ToString());
                return _Ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet WikiVoting(decimal P_DOCID, string P_USERID, decimal P_POINT)
        {
            try
            {
                DataSet _Ds = new DataSet();
                _Ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_PORTAL.PROC_VOTING",
                new OracleParameter("P_DOCID", OracleDbType.Decimal, P_DOCID, ParameterDirection.Input),
                new OracleParameter("P_USERID", OracleDbType.Varchar2, P_USERID, ParameterDirection.Input),
                new OracleParameter("P_POINT", OracleDbType.Decimal, P_POINT, ParameterDirection.Input),
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return _Ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal WikiDoc_Update_Status(decimal P_ID, decimal p_status, string p_note, string p_modified_by)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_DOCS.PROC_WIKI_DOCS_APPRO_OR_REFUSE",
                    new OracleParameter("P_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Varchar2, p_status, ParameterDirection.Input),
                    new OracleParameter("P_NOTE", OracleDbType.Varchar2, p_note, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIED_BY", OracleDbType.Varchar2, p_modified_by, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIED_DATE", OracleDbType.Date, DateTime.Now, ParameterDirection.Input),
                    paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public DataSet PortalWikiDoc_GetByCaseCode(string p_casecode)
        {
            try
            {
                DataSet _Ds = new DataSet();
                _Ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_WIKI_DOCS.PROC_WIKI_DOCS_GETBY_CASECODE",
                new OracleParameter("P_CASECODE", OracleDbType.Varchar2, p_casecode, ParameterDirection.Input),
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return _Ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

    }
}
