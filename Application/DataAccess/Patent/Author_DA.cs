﻿using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataAccess
{
    public class Author_DA
    {
        public decimal Insert(List<AuthorsInfo> pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_authors.Proc_Authors_Insert", pInfo.Count,
                    new OracleParameter("p_author_id", OracleDbType.Decimal, pInfo.Select(o => o.Author_Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.Select(o => o.App_Header_Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Select(o => o.Case_Code).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_author_name", OracleDbType.Varchar2, pInfo.Select(o => o.Author_Name).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_author_address", OracleDbType.Varchar2, pInfo.Select(o => o.Author_Address).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_author_phone", OracleDbType.Varchar2, pInfo.Select(o => o.Author_Phone).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_author_fax", OracleDbType.Varchar2, pInfo.Select(o => o.Author_Fax).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_author_email", OracleDbType.Varchar2, pInfo.Select(o => o.Author_Email).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_author_country", OracleDbType.Decimal, pInfo.Select(o => o.Author_Country).ToArray(), ParameterDirection.Input),
                    paramReturn);

                Oracle.DataAccess.Types.OracleDecimal[] totalReturn = (Oracle.DataAccess.Types.OracleDecimal[])paramReturn.Value;
                foreach (Oracle.DataAccess.Types.OracleDecimal _item in totalReturn)
                {
                    if (Convert.ToDecimal(_item.Value.ToString()) < 0)
                    {
                        return Convert.ToDecimal(_item.Value.ToString());
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal Update(AuthorsInfo pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_authors.proc_authors_update",
                    new OracleParameter("p_author_id", OracleDbType.Decimal, pInfo.Author_Id, ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Decimal, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("p_author_name", OracleDbType.Varchar2, pInfo.Author_Name, ParameterDirection.Input),
                    new OracleParameter("p_author_address", OracleDbType.Varchar2, pInfo.Author_Address, ParameterDirection.Input),
                    new OracleParameter("p_author_phone", OracleDbType.Varchar2, pInfo.Author_Phone, ParameterDirection.Input),
                    new OracleParameter("p_author_fax", OracleDbType.Varchar2, pInfo.Author_Fax, ParameterDirection.Input),
                    new OracleParameter("p_author_email", OracleDbType.Varchar2, pInfo.Author_Email, ParameterDirection.Input),
                    new OracleParameter("p_author_country", OracleDbType.Decimal, pInfo.Author_Country, ParameterDirection.Input),
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

        public int Deleted(string p_case_code, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_authors.Proc_delete",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
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

    }

    public class Inventor_DA
    {
        public decimal Insert(List<Inventor_Info> pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_inventors.Proc_Insert", pInfo.Count,
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Select(o => o.Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.Select(o => o.App_Header_Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, pInfo.Select(o => o.Case_Code).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_inventor_type", OracleDbType.Decimal, pInfo.Select(o => o.Inventor_Type).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_inventor_name", OracleDbType.Varchar2, pInfo.Select(o => o.Inventor_Name).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_inventor_address", OracleDbType.Varchar2, pInfo.Select(o => o.Inventor_Address).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_inventor_phone", OracleDbType.Varchar2, pInfo.Select(o => o.Inventor_Phone).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_inventor_fax", OracleDbType.Varchar2, pInfo.Select(o => o.Inventor_Fax).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_inventor_email", OracleDbType.Varchar2, pInfo.Select(o => o.Inventor_Email).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_country_nationality", OracleDbType.Decimal, pInfo.Select(o => o.Country_Nationality).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_country_residence", OracleDbType.Decimal, pInfo.Select(o => o.Country_Residence).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_country_incorporation", OracleDbType.Decimal, pInfo.Select(o => o.Country_Incorporation).ToArray(), ParameterDirection.Input),
                    paramReturn);

                Oracle.DataAccess.Types.OracleDecimal[] totalReturn = (Oracle.DataAccess.Types.OracleDecimal[])paramReturn.Value;
                foreach (Oracle.DataAccess.Types.OracleDecimal _item in totalReturn)
                {
                    if (Convert.ToDecimal(_item.Value.ToString()) < 0)
                    {
                        return Convert.ToDecimal(_item.Value.ToString());
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal Update(Inventor_Info pInfo)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_inventors.proc_update",
                    new OracleParameter("p_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("p_case_code", OracleDbType.Decimal, pInfo.Case_Code, ParameterDirection.Input),
                    new OracleParameter("p_inventor_type", OracleDbType.Decimal, pInfo.Inventor_Type, ParameterDirection.Input),
                    new OracleParameter("p_inventor_name", OracleDbType.Varchar2, pInfo.Inventor_Name, ParameterDirection.Input),
                    new OracleParameter("p_inventor_address", OracleDbType.Varchar2, pInfo.Inventor_Address, ParameterDirection.Input),
                    new OracleParameter("p_inventor_phone", OracleDbType.Varchar2, pInfo.Inventor_Phone, ParameterDirection.Input),
                    new OracleParameter("p_inventor_fax", OracleDbType.Varchar2, pInfo.Inventor_Fax, ParameterDirection.Input),
                    new OracleParameter("p_inventor_email", OracleDbType.Varchar2, pInfo.Inventor_Email, ParameterDirection.Input),
                    new OracleParameter("p_country_nationality", OracleDbType.Decimal, pInfo.Country_Nationality, ParameterDirection.Input),
                    new OracleParameter("p_country_residence", OracleDbType.Decimal, pInfo.Country_Residence, ParameterDirection.Input),
                    new OracleParameter("p_country_incorporation", OracleDbType.Decimal, pInfo.Country_Incorporation, ParameterDirection.Input),
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

        public int Deleted(string p_case_code, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_inventors.Proc_delete",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
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

    }
}
