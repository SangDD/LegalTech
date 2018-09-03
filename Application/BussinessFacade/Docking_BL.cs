﻿using Common;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessFacade
{
    public class Docking_BL
    {
        public List<Docking_Info> Docking_Search(string p_key_search, ref decimal p_total_record,
           string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                Docking_DA _da = new Docking_DA();
                DataSet _ds = _da.Docking_Search(p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<Docking_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Docking_Info>();
            }
        }

        public List<Docking_Info> Docking_GetAll()
        {
            try
            {
                Docking_DA _da = new Docking_DA();
                DataSet _ds = _da.Docking_GetAll();
                return CBO<Docking_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Docking_Info>();
            }
        }

        public Docking_Info Docking_GetBy_Id(decimal p_docking_id)
        {
            try
            {
                Docking_DA _da = new Docking_DA();
                DataSet _ds = _da.Docking_GetBy_Id(p_docking_id);
                return CBO<Docking_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new Docking_Info();
            }
        }

        public int Docking_Update_Status(decimal p_docking_id, string p_language_code, decimal p_status, string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                Docking_DA _da = new Docking_DA();
                return _da.Docking_Update_Status(p_docking_id, p_language_code, p_status, p_modify_by, p_modify_date);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Docking_Update_Delete(decimal p_docking_id, string p_language_code, string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                Docking_DA _da = new Docking_DA();
                return _da.Docking_Update_Delete(p_docking_id, p_language_code, p_modify_by, p_modify_date);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal Docking_Insert(Docking_Info p_Docking_Info)
        {
            try
            {
                Docking_DA _da = new Docking_DA();
                return _da.Docking_Insert(p_Docking_Info.Ap_Case_Code, p_Docking_Info.Docking_Type, p_Docking_Info.Document_Name, p_Docking_Info.Document_Type, p_Docking_Info.Status,
                    p_Docking_Info.Deadline, p_Docking_Info.Isshowcustomer, p_Docking_Info.In_Out_Date, p_Docking_Info.Created_By, 
                    p_Docking_Info.Created_Date, p_Docking_Info.Language_Code, p_Docking_Info.Url, p_Docking_Info.Notes, p_Docking_Info.Place_Submit, p_Docking_Info.FileName);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Docking_Update(Docking_Info p_Docking_Info)
        {
            try
            {
                Docking_DA _da = new Docking_DA();
                return _da.Docking_Update(p_Docking_Info.Docking_Id, p_Docking_Info.Docking_Type, p_Docking_Info.Document_Name, p_Docking_Info.Document_Type, p_Docking_Info.Status,
                    p_Docking_Info.Deadline, p_Docking_Info.Isshowcustomer, p_Docking_Info.In_Out_Date, p_Docking_Info.Modify_By, p_Docking_Info.Modify_Date, 
                    p_Docking_Info.Language_Code, p_Docking_Info.Url, p_Docking_Info.Notes, p_Docking_Info.Place_Submit, p_Docking_Info.FileName);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
    }
}
