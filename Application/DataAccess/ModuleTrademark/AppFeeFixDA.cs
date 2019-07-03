﻿using Common;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess.ModuleTrademark
{
    public class AppFeeFixDA
    {
        public int AppFeeFixInsertBatch(List<AppFeeFixInfo> pInfo ,string p_case_code)
        {
            try
            {
                int numberRecord = pInfo.Count;
                //decimal[] App_Header_Id = new decimal[numberRecord];
                decimal[] Fee_Id = new decimal[numberRecord];
                decimal[] Isuse = new decimal[numberRecord];
                decimal[] Number_Of_Patent = new decimal[numberRecord];
                decimal[] Amount = new decimal[numberRecord];
                string[] case_code = new string[numberRecord];

                for (int i = 0; i < pInfo.Count; i++)
                {
                    case_code[i] = p_case_code;
                    //App_Header_Id[i] = pAppHeaderid;
                    Fee_Id[i] = pInfo[i].Fee_Id;
                    Isuse[i] = pInfo[i].Isuse;
                    Number_Of_Patent[i] = pInfo[i].Number_Of_Patent ;
                    Amount[i] = pInfo[i].Amount;
                }
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                paramReturn.Size = 7;
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_FEE_FIX.PROC_APP_FEE_FIX_INSERT", numberRecord,
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, case_code, ParameterDirection.Input),
                    //new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_FEE_ID", OracleDbType.Decimal, Fee_Id, ParameterDirection.Input),
                    new OracleParameter("P_ISUSE", OracleDbType.Decimal, Isuse, ParameterDirection.Input),
                    new OracleParameter("P_NUMBER_OF_PATENT", OracleDbType.Decimal, Number_Of_Patent , ParameterDirection.Input),
                    new OracleParameter("P_AMOUNT", OracleDbType.Decimal, Amount, ParameterDirection.Input),
                    paramReturn);

                var result = ErrorCode.Error;
                Oracle.DataAccess.Types.OracleDecimal[] _ArrReturn = (Oracle.DataAccess.Types.OracleDecimal[])paramReturn.Value;
                foreach (Oracle.DataAccess.Types.OracleDecimal _item in _ArrReturn)
                {
                    if (Convert.ToInt32(_item.ToString()) < 0)
                    {
                        result = Convert.ToInt32(_item.ToString());
                        break;
                    }
                    else
                    {
                        result = ErrorCode.Success;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppFeeFixDelete (string p_case_code, string p_language_code)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_FEE_FIX.Proc_DeleteBy_Header",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input));
                return ErrorCode.Success;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public DataSet GetByCaseCode(string p_case_code)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_FEE_FIX.proc_get_by_case_code",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_2", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }
    }
}
