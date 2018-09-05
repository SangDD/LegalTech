﻿using Common;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Billing_DA
    {
        public DataSet Billing_Search(string p_key_search, string p_from, string p_to, string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_biling_search",
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("p_sort_type", OracleDbType.Varchar2, p_sort_type, ParameterDirection.Input),
                    paramReturn,
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));

                p_total_record = Convert.ToDecimal(paramReturn.Value.ToString());
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet Billing_GetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_billing_GetAll",
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet Billing_GetBy_Id(decimal p_billing_id)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_billing_getbyid",
                    new OracleParameter("p_billing_id", OracleDbType.Decimal, p_billing_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


        public int Billing_Update_Status(decimal p_billing_id,string p_language_code, decimal p_status,string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_billing_updateStatus",
                    new OracleParameter("p_billing_id", OracleDbType.Decimal, p_billing_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_modify_date", OracleDbType.Date, p_modify_date, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Billing_Insert(string p_app_case_code, decimal p_docking_type, string p_document_name, decimal p_document_type, decimal p_status, 
            DateTime p_deadline, decimal p_isshowcustomer, DateTime p_in_out_date, string p_created_by, DateTime p_created_date, string p_language_code, 
            string p_url,string p_notes, decimal p_place_submit, string p_filename)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.Proc_billing_Insert",
                    new OracleParameter("p_app_case_code", OracleDbType.Varchar2, p_app_case_code, ParameterDirection.Input),
                    new OracleParameter("p_docking_type", OracleDbType.Decimal, p_docking_type, ParameterDirection.Input),
                    new OracleParameter("p_document_name", OracleDbType.Varchar2, p_document_name, ParameterDirection.Input),
                    new OracleParameter("p_document_type", OracleDbType.Decimal, p_document_type, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_deadline", OracleDbType.Date, p_deadline, ParameterDirection.Input),
                    new OracleParameter("p_isshowcustomer", OracleDbType.Decimal, p_isshowcustomer, ParameterDirection.Input),
                    new OracleParameter("p_in_out_date", OracleDbType.Date, p_in_out_date, ParameterDirection.Input),
                    new OracleParameter("p_created_by", OracleDbType.Varchar2, p_created_by, ParameterDirection.Input),
                    new OracleParameter("p_created_date", OracleDbType.Date, p_created_date, ParameterDirection.Input),
                    new OracleParameter("p_place_submit", OracleDbType.Decimal, p_place_submit, ParameterDirection.Input),
                    new OracleParameter("p_url", OracleDbType.Varchar2, p_url, ParameterDirection.Input),
                    new OracleParameter("p_filename", OracleDbType.Varchar2, p_filename, ParameterDirection.Input),
                    new OracleParameter("p_notes", OracleDbType.Varchar2, p_notes, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),

                    paramReturn);
               
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Billing_Update(decimal p_billing_id, decimal p_docking_type, string p_document_name, decimal p_document_type, decimal p_status,
            DateTime p_deadline, decimal p_isshowcustomer, DateTime p_in_out_date, string p_modify_by, DateTime p_modify_date, 
            string p_language_code, string p_url, string p_notes, decimal p_place_submit, string p_filename)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_billing_update",
                    new OracleParameter("p_billing_id", OracleDbType.Decimal, p_billing_id, ParameterDirection.Input),
                    new OracleParameter("p_docking_type", OracleDbType.Decimal, p_docking_type, ParameterDirection.Input),
                    new OracleParameter("p_document_name", OracleDbType.Varchar2, p_document_name, ParameterDirection.Input),
                    new OracleParameter("p_document_type", OracleDbType.Decimal, p_document_type, ParameterDirection.Input),
                    //new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_deadline", OracleDbType.Date, p_deadline, ParameterDirection.Input),
                    new OracleParameter("p_isshowcustomer", OracleDbType.Decimal, p_isshowcustomer, ParameterDirection.Input),
                    new OracleParameter("p_in_out_date", OracleDbType.Date, p_in_out_date, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_modify_date", OracleDbType.Date, p_modify_date, ParameterDirection.Input),
                    new OracleParameter("p_place_submit", OracleDbType.Decimal, p_place_submit, ParameterDirection.Input),
                    new OracleParameter("p_url", OracleDbType.Varchar2, p_url, ParameterDirection.Input),
                    new OracleParameter("p_filename", OracleDbType.Varchar2, p_filename, ParameterDirection.Input),
                    new OracleParameter("p_notes", OracleDbType.Varchar2, p_notes, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public int Billing_Update_Delete(decimal p_billing_id, string p_language_code, string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_billing_delete",
                    new OracleParameter("p_billing_id", OracleDbType.Decimal, p_billing_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_modify_date", OracleDbType.Date, p_modify_date, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
    }
}
