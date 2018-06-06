﻿using System;
using System.Collections.Generic;
using System.Data;
using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;

namespace DataAccess.ModuleTrademark
{
    public class AppClassDetail_DA
    {
        public int AppClassDetailInsertBatch(List<AppClassDetailInfo> pInfo, decimal pAppHeaderid)
        {
            try
            {
                int numberRecord = pInfo.Count;
                string[] TextInput = new string[numberRecord];
                decimal[] App_Header_Id = new decimal[numberRecord];
                string[] Code = new string[numberRecord];

                DateTime[] Document_Filling_Date = new DateTime[numberRecord];
                for (int i = 0; i < pInfo.Count; i++)
                {
                    App_Header_Id[i] = pAppHeaderid;
                    TextInput[i] = pInfo[i].Textinput;
                    Code[i] = pInfo[i].Code;
                }
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APPCLASS_DETAIL.PROC_APP_CLASS_DETAIL_INSERT", numberRecord,
                    new OracleParameter("P_TEXTINPUT", OracleDbType.Varchar2, TextInput, ParameterDirection.Input),
                    new OracleParameter("P_CODE", OracleDbType.Varchar2, Code, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, App_Header_Id, ParameterDirection.Input),
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


        public int AppClassDetailDeleted(decimal pAppHeaderID )
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APPCLASS_DETAIL.PROC_APP_CLASS_DETAIL_DELETE",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pAppHeaderID, ParameterDirection.Input),
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
