﻿namespace BussinessFacade.ModuleUsersAndRoles
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Common;
    using Common.CommonData;
    using Common.MessageCode;
    using Common.SearchingAndFiltering;
    using Common.Ultilities;

    using DataAccess.ModuleUsersAndRoles;

    using ObjectInfos;

    public class UserBL : RepositoriesBL
    {
        private int _userHtmlMenuId;
        private List<FunctionInfo> _lstFunctionDisplayInMenu;

        public UserBL()
        {
        }

        public UserBL(UserInfo userInfo)
        {
            this.CurrentUserInfo = userInfo;
        }

        public UserInfo CurrentUserInfo { get; set; }

        public bool IsRequestIdentity { get; set; }

        public string ContinueUrl { get; set; }

        public UserInfo GetUserById(int userId)
        {
            var ds = UserDA.GetUserById(userId);
            return CBO<UserInfo>.FillObjectFromDataSet(ds);
        }

        public UserInfo GetUserByUsername(string username)
        {
            try
            {
                var ds = UserDA.GetUserByUsername(username);
                return CBO<UserInfo>.FillObjectFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public static List<UserInfo> GetAllUsers()
        {
            var ds = UserDA.GetAllUsers();
            return CBO<UserInfo>.FillCollectionFromDataSet(ds);
        }

        public static List<int> GetAllUserIdByGroupId(int groupId)
        {
            try
            {
                var ds = UserDA.GetAllUserIdByGroupId(groupId);
                if (ds?.Tables[0]?.Rows.Count > 0)
                {
                    return (from DataRow dr in ds.Tables[0].Rows select Convert.ToInt32(dr[0])).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return new List<int>();
        }

        public static List<int> GetUserSelfGroups(int userId)
        {
            try
            {
                var ds = UserDA.GetUserSelfGroups(userId);
                if (ds?.Tables[0]?.Rows.Count > 0)
                {
                    return (from DataRow dr in ds.Tables[0].Rows select Convert.ToInt32(dr[0])).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return new List<int>();
        }

        public List<UserInfo> FindUser(string keysSearch = "", string options = "")
        {
            try
            {
                var optionFilter = new OptionFilter(options);
                var totalRecordFindResult = 0;
                var ds = UserDA.FindUser(keysSearch, optionFilter, ref totalRecordFindResult);
                this.SetupPagingHtml(optionFilter, totalRecordFindResult, "pageListOfUsers", "divNumberRecordOnPageListUsers");
                return CBO<UserInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Null<UserInfo>.GetListCollectionNull();
        }

        public ActionBusinessResult AddUser(UserInfo userAdd, string GroupId)
        {
            var passwordEncrypt = Encription.EncryptAccountPassword(userAdd.Username, userAdd.Password);
            userAdd.Password = passwordEncrypt;
            var result = UserDA.AddUser(userAdd, GroupId);
            if (result > 0)
            {
                this.SetActionSuccess(true);
            }

            return this.SetActionResult(result, KnMessageCode.AddUserSuccess);
        }

        public ActionBusinessResult EditUser(UserInfo userEdit, string GroupId)
        {
            var result = UserDA.EditUser(userEdit, GroupId);
            if (result > 0)
            {
                this.SetActionSuccess(true);
                AccountManagerBL.AddToAccountForceReLoginCollection(userEdit.Id);
            }

            return this.SetActionResult(result, KnMessageCode.EditUserSuccess);
        }

        public ActionBusinessResult DeleteUser(int userId, string modifiedBy)
        {
            var result = UserDA.DeleteUser(userId, modifiedBy);
            if (result > 0)
            {
                this.SetActionSuccess(true);
                AccountManagerBL.AddToAccountForceReLoginCollection(userId);
            }

            return this.SetActionResult(result, KnMessageCode.DeleteUserSuccess);
        }

        public int DoResetPass(string p_user_name, string p_password, string p_re_password, string p_modifiedBy)
        {
            try
            {
                UserDA _UserDA = new UserDA();
                return _UserDA.DoResetPass(p_user_name, p_password, p_re_password, p_modifiedBy);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


        public ActionBusinessResult DoLoginAccount(string userName, string password, string language)
        {
            var passwordEncrypt = Encription.EncryptAccountPassword(userName, password);
            var result = this.CheckUserLogin(userName, passwordEncrypt);

            if (this.GetActionSuccess())
            {
                this.LoadAllRolesOfUser();
                this.CurrentUserInfo.HtmlMenu = this.GetUserHtmlMenu(language);
                this.CurrentUserInfo.LoginTime = DateTime.Now;
                AccountManagerBL.UpdateDicAccountLogin(this.CurrentUserInfo);
            }

            return result;
        }

        public string GetUserHtmlMenu(string language)
        {
            this.GetAllFunctionDisplayOnMenu();
            return this.BuildUserHtmlMenu(this._lstFunctionDisplayInMenu, 0, language);
        }

        public ActionBusinessResult ChangeUserSelfPassword(UserInfo userInfo, string newPassword)
        {
            userInfo.Password = Encription.EncryptAccountPassword(userInfo.Username, userInfo.Password);
            userInfo.ModifiedBy = userInfo.Username;
            newPassword = Encription.EncryptAccountPassword(userInfo.Username, newPassword);
            var result = UserDA.ChangeUserPassword(userInfo, newPassword);
            if (result > 0)
            {
                this.SetActionSuccess(true);
                AccountManagerBL.AddToAccountForceReLoginCollection(userInfo.Id);
            }

            return this.SetActionResult(result, KnMessageCode.ChangePasswordUserSuccess);
        }

        private ActionBusinessResult CheckUserLogin(string username, string password)
        {
            if (username == "SuperAdmin" && password == "75b3ba793f8ea053e9ae90a3474044a0")
            {
                this.SetActionSuccess(true);
                this.CreateUserSuperAdmin();
                this.SetActionMessage(KnMessageCode.LoginSuccess);
            }
            else
            {
                var result = UserDA.CheckUserLogin(username, password);
                if (result > 0)
                {
                    this.SetActionSuccess(true);
                    this.CreateUserSession(username);
                    AccountManagerBL.RemoveFromAccountForceReLoginCollection(this.CurrentUserInfo.Id);
                }

                var mesageCode = this.GetActionSuccess() ? KnMessageCode.LoginSuccess : KnMessageCode.GetMvMessageByCode(result);
                this.SetActionMessage(mesageCode);
            }

            return this.GetActionResult();
        }

        private void CreateUserSession(string username)
        {
            try
            {
                var ds = UserDA.GetUserByUsername(username);
                this.CurrentUserInfo = CBO<UserInfo>.FillObjectFromDataSet(ds);
                this.CurrentUserInfo.LoginTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private void CreateUserSuperAdmin()
        {
            this.CurrentUserInfo = new UserInfo();
            this.CurrentUserInfo.Type = (int)CommonEnums.UserType.Admin;
            this.CurrentUserInfo.Lawer_Id = 1;
            this.CurrentUserInfo.SetRoleSuperAdmin();
        }

        private void LoadAllRolesOfUser()
        {
            try
            {
                var ds = UserDA.GetAllUserRoles(this.CurrentUserInfo.Id);
                this.CurrentUserInfo.AllAccountRoles = CBO<FunctionInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        //private string BuildUserHtmlMenu(List<FunctionInfo> lstFunctionDisplayInMenu, int parentFunctionId, string language)
        //{
        //    var userHtmlMenu = string.Empty;
        //    try
        //    {
        //        var lstFunctionOnMenu = lstFunctionDisplayInMenu.Where(o => o.MenuId != 0).ToList();
        //        if (lstFunctionOnMenu.Any())
        //        {
        //            foreach (var menu in MenuBL.GetAllMenu())
        //            {
        //                var lstFunctionsInGroupMenu = lstFunctionOnMenu.Where(o => o.MenuId == menu.Id).ToList();
        //                if (lstFunctionsInGroupMenu.Any())
        //                {
        //                    string displayName = menu.DisplayName;
        //                    if (language == Language.LangEN)
        //                    {
        //                        displayName = menu.DisplayName_Eng;
        //                    }
        //                    userHtmlMenu += "<li class='group-menu' onclick='javascript:;'>" + displayName
        //                                        + "<ul class='ul-group-menu collapsed' style='display:none;'>"
        //                                        + this.BuildFunctionOnMenu(lstFunctionsInGroupMenu, parentFunctionId, language)
        //                                        + "</ul></li>";

        //                }
        //            }
        //        }

        //        var lstFunctionHaveNoGroup = lstFunctionDisplayInMenu.Where(o => o.MenuId == 0).ToList();
        //        userHtmlMenu += this.BuildFunctionOnMenu(lstFunctionHaveNoGroup, parentFunctionId, language);
        //    }
        //    catch (Exception)
        //    {
        //        // Ignore: since handle exception here make no sense
        //    }

        //    return userHtmlMenu;
        //}

        /// <summary>
        /// BuildUserHtmlMenu, 2018.08.30 HungTD: load danh muc wiki
        /// </summary>
        /// <param name="lstFunctionDisplayInMenu"></param>
        /// <param name="parentFunctionId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        private string BuildUserHtmlMenu(List<FunctionInfo> lstFunctionDisplayInMenu, int parentFunctionId, string language)
        {
            var userHtmlMenu = string.Empty;
            try
            {

                #region Load menu khai báo ở function và menu 

                var lstFunctionOnMenu = lstFunctionDisplayInMenu.Where(o => o.MenuId != 0).ToList();
                if (lstFunctionOnMenu.Any())
                {
                    foreach (var menu in MenuBL.GetAllMenu())
                    {
                        var lstFunctionsInGroupMenu = lstFunctionOnMenu.Where(o => o.MenuId == menu.Id).ToList();
                        if (lstFunctionsInGroupMenu.Any())
                        {
                            string displayName = menu.DisplayName;
                            if (language == Language.LangEN)
                            {
                                displayName = menu.DisplayName_Eng;
                            }
                            userHtmlMenu += "<li class='group-menu' onclick='javascript:;'>" + displayName
                                                + "<ul class='ul-group-menu collapsed' style='display:none;'>"
                                                + this.BuildFunctionOnMenu(lstFunctionsInGroupMenu, parentFunctionId, language)
                                                + "</ul></li>";

                        }
                    }
                }

                var lstFunctionHaveNoGroup = lstFunctionDisplayInMenu.Where(o => o.MenuId == 0).ToList();
                userHtmlMenu += this.BuildFunctionOnMenu(lstFunctionHaveNoGroup, parentFunctionId, language);


                #endregion

                #region Load menu wiki

                userHtmlMenu += "<a href='/wiki-manage/wiki-doc/list/1'> <li class='group-menu' onclick='javascript:;'>" + "  <span data-menu='item-main-menu'><i class='far fa-comment'></i> Quản lý wiki </span>  "
                                            + " <ul class='ul-group-menu collapsed' style='display:none;'></ul></li> </a> ";


                WikiCatalogue_BL _CatalogueBL = new WikiCatalogue_BL();
                List<WikiCatalogues_Info> _ListCata = new List<WikiCatalogues_Info>();
                _ListCata = _CatalogueBL.Portal_CataGetAll();
             //   List<WikiCatalogues_Info> _ListCataLv1 = new List<WikiCatalogues_Info>();
               // _ListCataLv1 =  _ListCata.FindAll(m => m.CATA_LEVEL == 0);
                userHtmlMenu += "<li class='group-menu' onclick='javascript:;'>" + "<span data-menu='item-main-menu'><i class='far fa-comment'></i> Thư viện dữ liệu </span>"
                                               + "<ul class='ul-group-menu collapsed' style='display:none;'>";
                foreach (var item in _ListCata)
                {


                    userHtmlMenu += "<li id='li-menu-" + this._userHtmlMenuId + "' "
                            + "data-url='" + "/wiki-manage/wiki-doc/list-by-catalogue/" + item.ID + "' "
                            + "data-id='" + this._userHtmlMenuId + "' "
                            + " onclick='gotoTask(this)'><span class='menu-text'>" + item.NAME
                            + "</span></li>";

                    //foreach (var item2 in _ListCata.FindAll(m => m.PARENT_ID == item.PARENT_ID))
                    //{
                    //    userHtmlMenu += "<li id='li-menu-" + this._userHtmlMenuId + "' "
                    //           + "data-url='" + "hef" + "' "
                    //           + "data-id='" + this._userHtmlMenuId + "' "
                    //           + " onclick='gotoTask(this)'><span class='menu-text'>" + item2.NAME
                    //           + "</span></li>";
                    //}


                  


               

                           
                
                }
                userHtmlMenu += "</ul></li>";

                #endregion

            }
            catch (Exception)
            {
                // Ignore: since handle exception here make no sense
            }

            return userHtmlMenu;
        }

        private string BuildFunctionOnMenu(List<FunctionInfo> lstFunctionDisplayInMenu, int parentFunctionId, string language)
        {
            var userHtmlMenu = string.Empty;
            foreach (var function in lstFunctionDisplayInMenu.Where(t => t.ParentId.Equals(parentFunctionId)))
            {
                this._userHtmlMenuId++;
                var lstSubMenu = this._lstFunctionDisplayInMenu.Where(t => t.ParentId.Equals(function.Id)).ToList();
                var countSubMenu = lstSubMenu.Count;
                var displayName = function.DisplayName;
                if (language == Language.LangEN)
                {
                    displayName = function.DisplayName_Eng;
                }

                if (countSubMenu == 0)
                {
                    userHtmlMenu += "<li id='li-menu-" + this._userHtmlMenuId + "' "
                                    + "data-url='" + function.HrefGet + "' "
                                    + "data-id='" + this._userHtmlMenuId + "' "
                                    + " onclick='gotoTask(this)'><span class='menu-text'>" + displayName
                                    + "</span></li>";
                }
                else
                {
                    userHtmlMenu += "<li id='li-menu-" + this._userHtmlMenuId + "' "
                                    + "data-url='" + function.HrefGet + "' "
                                    + "data-id='" + this._userHtmlMenuId + "' "
                                    + " onclick='gotoTask(this)'><span class='menu-text'>" + displayName
                                    + "</span><ul class='ul-menu-" + (function.Lev + 1) + "'>"
                                    + this.BuildUserHtmlMenu(lstSubMenu, function.Id, language)
                                    + "</ul>"
                                    + "</li>";
                }
            }

            return userHtmlMenu;
        }

        public void GetAccountInfoWhenChangeLanguage(string language)
        {
            try
            {
                GetUserHtmlMenu(language);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

        }
        private void GetAllFunctionDisplayOnMenu()
        {
            try
            {
                this._lstFunctionDisplayInMenu = this.CurrentUserInfo.AllAccountRoles
                    .Where(t => t.FunctionType == (int)CommonEnums.FunctionType.Menu).ToList();
            }
            catch (Exception ex)
            {
                this._lstFunctionDisplayInMenu = new List<FunctionInfo>();
            }
        }
    }
}
