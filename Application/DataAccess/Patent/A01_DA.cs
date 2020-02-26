﻿using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Data;
namespace DataAccess
{
    public class A01_DA
    {
        public decimal Insert(A01_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_a01.Proc_App_Detail_A01_Insert",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_appno", OracleDbType.Varchar2, pInfo.Appno, ParameterDirection.Input),
                    new OracleParameter("p_patent_type", OracleDbType.Varchar2, pInfo.Patent_Type, ParameterDirection.Input),
                    new OracleParameter("p_patent_name", OracleDbType.Varchar2, pInfo.Patent_Name, ParameterDirection.Input),
                    new OracleParameter("p_source_pct", OracleDbType.Varchar2, pInfo.Source_PCT, ParameterDirection.Input),
                    new OracleParameter("p_pct_number", OracleDbType.Varchar2, pInfo.PCT_Number, ParameterDirection.Input),
                    new OracleParameter("p_pct_filling_date_qt", OracleDbType.Date, pInfo.PCT_Filling_Date_Qt, ParameterDirection.Input),
                    new OracleParameter("p_pct_number_qt", OracleDbType.Varchar2, pInfo.PCT_Number_Qt, ParameterDirection.Input),
                    new OracleParameter("p_pct_date", OracleDbType.Date, pInfo.PCT_Date, ParameterDirection.Input),
                    new OracleParameter("p_pct_vn_date", OracleDbType.Date, pInfo.PCT_VN_Date, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi", OracleDbType.Decimal, pInfo.PCT_Suadoi, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi_name", OracleDbType.Varchar2, pInfo.PCT_Suadoi_Name, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi_address", OracleDbType.Varchar2, pInfo.PCT_Suadoi_Address, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi_others", OracleDbType.Varchar2, pInfo.PCT_Suadoi_Others, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi_content", OracleDbType.Varchar2, pInfo.PCT_Suadoi_Content, ParameterDirection.Input),
                    new OracleParameter("p_source_dqsc", OracleDbType.Varchar2, pInfo.Source_DQSC, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_origin_app_no", OracleDbType.Varchar2, pInfo.DQSC_Origin_App_No, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_filling_date", OracleDbType.Date, pInfo.DQSC_Filling_Date, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_valid_before", OracleDbType.Decimal, pInfo.DQSC_Valid_Before, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_valid_after", OracleDbType.Decimal, pInfo.DQSC_Valid_After, ParameterDirection.Input),
                    new OracleParameter("p_source_gphi", OracleDbType.Varchar2, pInfo.Source_GPHI, ParameterDirection.Input),
                    new OracleParameter("p_gphi_origin_app_no", OracleDbType.Varchar2, pInfo.GPHI_Origin_App_No, ParameterDirection.Input),
                    new OracleParameter("p_gphi_filling_date", OracleDbType.Date, pInfo.GPHI_Filling_Date, ParameterDirection.Input),
                    new OracleParameter("p_gphi_valid_before", OracleDbType.Decimal, pInfo.GPHI_Valid_Before, ParameterDirection.Input),
                    new OracleParameter("p_gphi_valid_after", OracleDbType.Decimal, pInfo.GPHI_Valid_After, ParameterDirection.Input),

                    new OracleParameter("p_Point", OracleDbType.Decimal, pInfo.Point, ParameterDirection.Input),
                    new OracleParameter("p_ThamDinhNoiDung", OracleDbType.Varchar2, pInfo.ThamDinhNoiDung, ParameterDirection.Input),
                    new OracleParameter("p_ChuyenDoiDon", OracleDbType.Varchar2, pInfo.ChuyenDoiDon, ParameterDirection.Input),

                    new OracleParameter("p_class_type", OracleDbType.Varchar2, pInfo.Class_Type, ParameterDirection.Input),
                    new OracleParameter("p_class_content", OracleDbType.Varchar2, pInfo.Class_Content, ParameterDirection.Input),
                    new OracleParameter("p_Used_Special", OracleDbType.Decimal, pInfo.Used_Special, ParameterDirection.Input),
                    
                    paramReturn);
                var result = Convert.ToDecimal(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public decimal UpDate(A01_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_a01.proc_app_detail_a01_update",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("p_appno", OracleDbType.Varchar2, pInfo.Appno, ParameterDirection.Input),
                    new OracleParameter("p_patent_type", OracleDbType.Varchar2, pInfo.Patent_Type, ParameterDirection.Input),
                    new OracleParameter("p_patent_name", OracleDbType.Varchar2, pInfo.Patent_Name, ParameterDirection.Input),
                    new OracleParameter("p_source_pct", OracleDbType.Varchar2, pInfo.Source_PCT, ParameterDirection.Input),
                    new OracleParameter("p_pct_number", OracleDbType.Varchar2, pInfo.PCT_Number, ParameterDirection.Input),
                    new OracleParameter("p_pct_filling_date_qt", OracleDbType.Date, pInfo.PCT_Filling_Date_Qt, ParameterDirection.Input),
                    new OracleParameter("p_pct_number_qt", OracleDbType.Varchar2, pInfo.PCT_Number_Qt, ParameterDirection.Input),
                    new OracleParameter("p_pct_date", OracleDbType.Date, pInfo.PCT_Date, ParameterDirection.Input),
                    new OracleParameter("p_pct_vn_date", OracleDbType.Date, pInfo.PCT_VN_Date, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi", OracleDbType.Decimal, pInfo.PCT_Suadoi, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi_name", OracleDbType.Varchar2, pInfo.PCT_Suadoi_Name, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi_address", OracleDbType.Varchar2, pInfo.PCT_Suadoi_Address, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi_others", OracleDbType.Varchar2, pInfo.PCT_Suadoi_Others, ParameterDirection.Input),
                    new OracleParameter("p_pct_suadoi_content", OracleDbType.Varchar2, pInfo.PCT_Suadoi_Content, ParameterDirection.Input),
                    new OracleParameter("p_source_dqsc", OracleDbType.Varchar2, pInfo.Source_DQSC, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_origin_app_no", OracleDbType.Varchar2, pInfo.DQSC_Origin_App_No, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_filling_date", OracleDbType.Date, pInfo.DQSC_Filling_Date, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_valid_before", OracleDbType.Decimal, pInfo.DQSC_Valid_Before, ParameterDirection.Input),
                    new OracleParameter("p_dqsc_valid_after", OracleDbType.Decimal, pInfo.DQSC_Valid_After, ParameterDirection.Input),
                    new OracleParameter("p_source_gphi", OracleDbType.Varchar2, pInfo.Source_GPHI, ParameterDirection.Input),
                    new OracleParameter("p_gphi_origin_app_no", OracleDbType.Varchar2, pInfo.GPHI_Origin_App_No, ParameterDirection.Input),
                    new OracleParameter("p_gphi_filling_date", OracleDbType.Date, pInfo.GPHI_Filling_Date, ParameterDirection.Input),
                    new OracleParameter("p_gphi_valid_before", OracleDbType.Decimal, pInfo.GPHI_Valid_Before, ParameterDirection.Input),
                    new OracleParameter("p_gphi_valid_after", OracleDbType.Decimal, pInfo.GPHI_Valid_After, ParameterDirection.Input),

                    new OracleParameter("p_Point", OracleDbType.Decimal, pInfo.Point, ParameterDirection.Input),
                    new OracleParameter("p_ThamDinhNoiDung", OracleDbType.Varchar2, pInfo.ThamDinhNoiDung, ParameterDirection.Input),
                    new OracleParameter("p_ChuyenDoiDon", OracleDbType.Varchar2, pInfo.ChuyenDoiDon, ParameterDirection.Input),

                    new OracleParameter("p_class_type", OracleDbType.Varchar2, pInfo.Class_Type, ParameterDirection.Input),
                    new OracleParameter("p_class_content", OracleDbType.Varchar2, pInfo.Class_Content, ParameterDirection.Input),
                    new OracleParameter("p_Used_Special", OracleDbType.Decimal, pInfo.Used_Special, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToDecimal(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Deleted(decimal p_app_header_id, string pAppCode, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_a01.proc_app_detail_a01_delete",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                    paramReturn);

                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public DataSet GetByID(decimal p_app_header_id, string p_language_code)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_a01.Proc_GetBy_ID",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursorHeader", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_doc", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_fee", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_other_master", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_author", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_class", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_other_doc", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_uu_tien", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_image_public", OracleDbType.RefCursor, ParameterDirection.Output));
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

    }

    public class Pattent_Lao_DA
    {
        public decimal Insert(Pattent_Lao_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_pattent_lao.Proc_Insert",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                     new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                     new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                     new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                     new OracleParameter("p_appno", OracleDbType.Varchar2, pInfo.App_No, ParameterDirection.Input),
                     new OracleParameter("p_patent_type", OracleDbType.Varchar2, pInfo.Patent_Type, ParameterDirection.Input),
                     new OracleParameter("p_title", OracleDbType.Varchar2, pInfo.Title, ParameterDirection.Input),
                     new OracleParameter("p_request", OracleDbType.Varchar2, pInfo.Request, ParameterDirection.Input),
                     new OracleParameter("p_claims", OracleDbType.Varchar2, pInfo.Claims, ParameterDirection.Input),
                     new OracleParameter("p_abstract", OracleDbType.Varchar2, pInfo.Abstract, ParameterDirection.Input),
                     new OracleParameter("p_description", OracleDbType.Varchar2, pInfo.Description, ParameterDirection.Input),
                     new OracleParameter("p_drawing", OracleDbType.Decimal, pInfo.Drawing, ParameterDirection.Input),
                     new OracleParameter("p_number_industrial", OracleDbType.Decimal, pInfo.Number_Industrial, ParameterDirection.Input),
                     new OracleParameter("p_number_sheet", OracleDbType.Decimal, pInfo.Number_Sheet, ParameterDirection.Input),
                     new OracleParameter("p_multiple", OracleDbType.Varchar2, pInfo.Multiple, ParameterDirection.Input),
                     new OracleParameter("p_setofarticle", OracleDbType.Varchar2, pInfo.SetOfArticle, ParameterDirection.Input),
                     new OracleParameter("p_composition", OracleDbType.Varchar2, pInfo.Composition, ParameterDirection.Input),
                     new OracleParameter("p_class_content", OracleDbType.Varchar2, pInfo.Class_Content, ParameterDirection.Input),
                     new OracleParameter("p_used_special", OracleDbType.Decimal, pInfo.Used_Special, ParameterDirection.Input),
                     new OracleParameter("p_filling_fee", OracleDbType.Decimal, pInfo.Filling_Fee, ParameterDirection.Input),
                     new OracleParameter("p_basic_fee", OracleDbType.Decimal, pInfo.Basic_Fee, ParameterDirection.Input),
                     new OracleParameter("p_others_fee", OracleDbType.Decimal, pInfo.Others_Fee, ParameterDirection.Input),
                     new OracleParameter("p_name_of_print", OracleDbType.Varchar2, pInfo.Name_Of_Print, ParameterDirection.Input),
                     new OracleParameter("p_date_signed", OracleDbType.Date, pInfo.Date_Signed, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToDecimal(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal UpDate(Pattent_Lao_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_pattent_lao.proc_update",
                     new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                     new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Case_Code, ParameterDirection.Input),
                     new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                     new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                     new OracleParameter("p_appno", OracleDbType.Varchar2, pInfo.App_No, ParameterDirection.Input),
                     new OracleParameter("p_patent_type", OracleDbType.Varchar2, pInfo.Patent_Type, ParameterDirection.Input),
                     new OracleParameter("p_title", OracleDbType.Varchar2, pInfo.Title, ParameterDirection.Input),
                     new OracleParameter("p_request", OracleDbType.Varchar2, pInfo.Request, ParameterDirection.Input),
                     new OracleParameter("p_claims", OracleDbType.Varchar2, pInfo.Claims, ParameterDirection.Input),
                     new OracleParameter("p_abstract", OracleDbType.Varchar2, pInfo.Abstract, ParameterDirection.Input),
                     new OracleParameter("p_description", OracleDbType.Varchar2, pInfo.Description, ParameterDirection.Input),
                     new OracleParameter("p_drawing", OracleDbType.Decimal, pInfo.Drawing, ParameterDirection.Input),
                     new OracleParameter("p_number_industrial", OracleDbType.Decimal, pInfo.Number_Industrial, ParameterDirection.Input),
                     new OracleParameter("p_number_sheet", OracleDbType.Decimal, pInfo.Number_Sheet, ParameterDirection.Input),
                     new OracleParameter("p_multiple", OracleDbType.Varchar2, pInfo.Multiple, ParameterDirection.Input),
                     new OracleParameter("p_setofarticle", OracleDbType.Varchar2, pInfo.SetOfArticle, ParameterDirection.Input),
                     new OracleParameter("p_composition", OracleDbType.Varchar2, pInfo.Composition, ParameterDirection.Input),
                     new OracleParameter("p_class_content", OracleDbType.Varchar2, pInfo.Class_Content, ParameterDirection.Input),
                     new OracleParameter("p_used_special", OracleDbType.Decimal, pInfo.Used_Special, ParameterDirection.Input),
                     new OracleParameter("p_filling_fee", OracleDbType.Decimal, pInfo.Filling_Fee, ParameterDirection.Input),
                     new OracleParameter("p_basic_fee", OracleDbType.Decimal, pInfo.Basic_Fee, ParameterDirection.Input),
                     new OracleParameter("p_others_fee", OracleDbType.Decimal, pInfo.Others_Fee, ParameterDirection.Input),
                     new OracleParameter("p_name_of_print", OracleDbType.Varchar2, pInfo.Name_Of_Print, ParameterDirection.Input),
                     new OracleParameter("p_date_signed", OracleDbType.Date, pInfo.Date_Signed, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToDecimal(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Deleted(decimal p_app_header_id, string pAppCode, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_pattent_lao.proc_delete",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_appcode", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                    paramReturn);

                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public DataSet GetByID(decimal p_app_header_id, string p_language_code)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_a01.Proc_GetBy_ID",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursorHeader", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_doc", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_fee", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_other_master", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_author", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_class", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_other_doc", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_uu_tien", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_image_public", OracleDbType.RefCursor, ParameterDirection.Output));
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

    }
}
