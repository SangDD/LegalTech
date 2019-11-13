﻿namespace DataAccess.ModuleMemoryData
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Common;
    using Oracle.DataAccess.Client;

    public class AllCodeDA
    {
        public static DataSet GetAllInAllCode()
        {
            var ds = new DataSet();
            try
            {
                ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_allcode.proc_AllCode_GetAll",
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return ds;
        }

        public static DataSet Get_All_Injection()
        {
            var ds = new DataSet();
            try
            {
                ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_allcode.proc_injection",
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return ds;
        }

        public static DataSet Country_GetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_allcode.proc_Country_GetAll",
                 new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public static DataSet Nation_Represent_GetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_allcode.proc_nation_represent_getall",
                 new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }
    }
}
