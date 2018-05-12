﻿namespace BussinessFacade.ModuleMemoryData
{
    using System;
    using BussinessFacade.ModuleTrademark;
    using Common;

    using ModuleUsersAndRoles;
    using System.Collections;
    using ObjectInfos;
    using System.Collections.Generic;
    using System.Linq;

    public class MemoryData
	{
        public static Hashtable c_hs_Allcode = new Hashtable();

        public static void LoadAllMemoryData()
		{
			try
			{
                #region Allcode
                c_hs_Allcode.Clear();
                AllCodeBL _AllCodeBL = new AllCodeBL();
                List<AllCodeInfo> _lst_al = _AllCodeBL.AllCode_Gets_List();

                if (_lst_al.Count > 0)
                {
                    foreach (AllCodeInfo item in _lst_al)
                    {
                        if (c_hs_Allcode.ContainsKey(item.CdName + "|" + item.CdType) == false)
                        {
                            List<AllCodeInfo> _lst = new List<AllCodeInfo>();
                            _lst.Add(item);
                            c_hs_Allcode[item.CdName + "|" + item.CdType] = _lst;
                        }
                        else
                        {
                            List<AllCodeInfo> _lst = (List<AllCodeInfo>)c_hs_Allcode[item.CdName + "|" + item.CdType];
                            _lst.Add(item);
                        }
                    }
                }

                AllCodeBL.LoadAllCodeToMemory();
                #endregion

                MenuBL.LoadAllMenuToMemory();
				FunctionBL.LoadFunctionCollectionsToMemory();
                SysApplicationBL.SysApplicationAllOnMem();

            }
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
		}

        public static List<AllCodeInfo> AllCode_GetBy_CdTypeCdName(string p_cdname, string p_cdtype)
        {
            try
            {
                if (c_hs_Allcode.ContainsKey(p_cdname + "|" + p_cdtype))
                {
                    List<AllCodeInfo> _lst = new List<AllCodeInfo>();

                    List<AllCodeInfo> _lst_tem = (List<AllCodeInfo>)c_hs_Allcode[p_cdname + "|" + p_cdtype];

                    foreach (AllCodeInfo item in _lst_tem)
                        _lst.Add(item);

                    _lst = _lst.OrderBy(m => m.LstOdr).ToList();
                    return _lst;
                }
                else return new List<AllCodeInfo>();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AllCodeInfo>();
            }
        }
    }
}
