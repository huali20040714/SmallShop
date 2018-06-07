using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using SmallShop.Utilities;
using SmallShop.BackStage.Business;
using SmallShop.Entities;
using SmallShop.Data;
using SmallShop.BackStage.Web.Models;
using SmallShop.Utilities.DbProxy;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;

namespace SmallShop.BackStage.Web.Controllers
{
    [LoginValidate]
    public class SystemController : Controller
    {
        private static DataProvider provider = DataProvider.Instance;
        
        #region 角色管理

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.系统管理_角色管理)]
        public ActionResult Role()
        {
            BackStageLocation.Path = "Index/Role";

            return View();
        }

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.系统管理_角色管理)]
        public JsonResult GetRoles()
        {
            var ret = Result.Default;
            var queryParams = new QueryParams(WebHelper.Params);
            if (string.IsNullOrEmpty(queryParams.OrderBy))
            {
                queryParams.OrderBy = $"{RoleCols.Id}";
                queryParams.IsAsc = true;
            }
            queryParams.Table = $"{EntityType.Role}";
            var isInner = (-1).TryParse(WebHelper.Params["IsInner"]);
            if (isInner == 0 || isInner == 1)
                queryParams.WhereClause = $"[{RoleCols.IsInner}] = {isInner}";

            var list = DbProxy.GetPageRecords<RoleInfo>(queryParams);
            ret.Value = this.RenderView("Role.Partial", list);
            ret.Tag = queryParams.RecordCount;

            return Json(ret);
        }

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.系统管理_角色管理)]
        public ActionResult RoleEditor(int id)
        {
            var role = provider.GetRole(id);
            if (role == null)
                role = new RoleInfo();

            return View(role);
        }

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.系统管理_角色管理)]
        public JsonResult SaveRoleEditor(RoleInfo role)
        {
            if (role.Id > 0)
            {
                var _role = provider.GetRole(role.Id);
                if (_role == null)
                    return Json(Result.Error("编辑内容不存在"));

                var roles = provider.GetRoles($"{RoleCols.Name} = {role.Name.ToSafeSql()} and {RoleCols.Id} <> {role.Id}");
                if (roles.Count > 0)
                    return Json(Result.Error("角色名称不能重复"));

                provider.UpdateRole($"{RoleCols.Id} = {role.Id}", $"[{RoleCols.Name}] = {role.Name.ToSafeSql()}");
            }
            else
            {
                var roles = provider.GetRoles($"{RoleCols.Name} = {role.Name.ToSafeSql()}");
                if (roles.Count > 0)
                    return Json(Result.Error("角色名称不能重复"));

                role.IsInner = false;
                role.Permissions = "|";
                role.CreateTime = DateTime.Now;
                provider.CreateRole(role);
            }

            return Json(Result.Default);
        }

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.系统管理_角色管理)]
        public JsonResult RoleDelete(int id)
        {
            var roleUsers = provider.GetRoleUsers($"{RoleUserCols.RoleId} = {id}");
            if (roleUsers.Count > 0)
                return Json(Result.Error("该角色已分配用户"));

            var role = provider.GetRole(id);
            if (role == null)
                return Json(Result.Error("该角色不存在"));

            if (role.IsInner)
                return Json(Result.Error("内部角色不能删除"));

            provider.DeleteRoles($"{RoleCols.Id} = {id}");

            return Json(Result.Default);
        }

        #endregion

        #region 权限设置

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.系统管理_权限设置)]
        public ActionResult RolePermissionEditor()
        {
            BackStageLocation.Path = "Index/RolePermissionEditor";
            var model = new RolePermissionEditorModel()
            {
                RoleId = 0.TryParse(WebHelper.Params["Id"]),
                Pairs = provider.GetRoles().Select(r => new KeyValuePair<int, string>(r.Id, r.Name)).ToList()
            };

            return View(model);
        }

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.系统管理_权限设置)]
        public JsonResult BuildRolePermission()
        {
            int roleId = 0.TryParse(WebHelper.Params["RoleId"]);
            if (roleId <= 0)
                return Json(Result.Error("请选择一个角色"));

            //取得角色权限集合
            var role = provider.GetRole(roleId);
            if (role == null)
                return Json(Result.Error("未查询到角色"));

            var permissions = role.Permissions.Split('|').Where(p => !string.IsNullOrEmpty(p)).Select(p => 0.TryParse(p)).Cast<PermissionType>();
            //取得大权限
            var html = string.Empty;
            var allPermissionTypes = Enum.GetValues(typeof(PermissionType)).Cast<PermissionType>();
            foreach (var groupType in Enum.GetValues(typeof(PermissionType)).Cast<PermissionType>())
            {
                //生成tab，模100模得尽的表示是大权限
                if ((int)groupType % 100 == 0)
                {
                    #region 权限组里面的具体权限,被100整除

                    string tab = "", content = "", footer = "</div></div></div><div class='space-6'></div>";
                    //指示当前权限组中的权限是否有被选中过
                    bool hasCheckedInGroup = permissions.Any(p => p == groupType);
                    tab = string.Format(@"
                                <div class='tabbable'>
                                    <ul class='nav nav-tabs'>
                                        <li class='active'>
                                            <a href='javascript:void(0);'>
                                                <label><input class='ace' type='checkbox' {2} name='c{0}' value='{0}'><span class='lbl'> {1}</span></label>
                                            </a>
                                        </li>
                                    </ul>
                                    <div class='tab-content'>
                                        <div class='tab-pane in active'>
                                ", (int)groupType, groupType, hasCheckedInGroup ? "checked='checked'" : "");

                    //取得小权限
                    foreach (var permissionType in allPermissionTypes)
                    {
                        //每组权限的区间是100
                        if ((int)permissionType > (int)groupType && (int)permissionType < ((int)groupType + 100))
                        {
                            var itemName = permissionType.ToString().Replace(groupType + "_", "");  //权限名字【去掉组前缀】
                            bool isChecked = permissions.Any(p => p == permissionType);
                            content += string.Format(@"
                                    <label class='p-cell'>
                                        <input class='ace' type='checkbox' id='c{0}' name='c{0}' value='{0}' {2} data-tab='{3}'><span class='lbl' for='c{0}'> {1}</span>
                                    </label>",
                                    (int)permissionType, itemName, isChecked ? "checked='checked'" : "", (int)groupType);
                        }
                    }
                    html += tab + content + footer;

                    #endregion
                }
            }

            var ret = Result.Default;
            ret.Value = html;

            return Json(ret);
        }

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.系统管理_权限设置)]
        public JsonResult SaveRolePermission()
        {
            int roleId = 0.TryParse(WebHelper.Params["RoleId"]);
            var permissions = WebHelper.Params["Permissions"] ?? "|";
            if (roleId == 0)
                return Json(Result.Error("请选择一个角色"));

            var role = provider.GetRole(roleId);
            if (role == null)
                return Json(Result.Error("未查询到角色"));
            try
            {
                provider.UpdateRole($"{RoleCols.Id} = {role.Id}", $"{RoleCols.Permissions} = {permissions.ToSafeSql()}");

                return Json(Result.Default);
            }
            catch (Exception ex)
            {
                return Json(Result.Error(ex.Message));
            }
        }

        #endregion

        #region 操作日志管理

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.系统管理_操作日志)]
        public ActionResult OperationLog()
        {
            BackStageLocation.Path = "Index/OperationLog";

            return View();
        }

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.系统管理_操作日志)]
        public JsonResult GetOperationLogs()
        {
            var ret = Result.Default;
            var queryParams = new QueryParams(WebHelper.Params);
            if (string.IsNullOrEmpty(queryParams.OrderBy))
            {
                queryParams.OrderBy = $"{OperationLogCols.Id}";
                queryParams.IsAsc = false;
            }

            var isVague = 0.TryParse(WebHelper.Params["IsVague"]) == 1; //是否为模糊查询
            queryParams.Table = $"{EntityType.OperationLog}";
            queryParams.WhereClause = "CreateTime".ToSaftBetweenDateSql(WebHelper.Params["dtStart"], WebHelper.Params["dtEnd"], true);

            var operationType = (OperationType)(-1).TryParse(WebHelper.Params["OperationType"]);
            if ((int)operationType > 0)
                queryParams.WhereClause += $" and [{OperationLogCols.Type}] = {(int)operationType}";

            if (!string.IsNullOrEmpty(WebHelper.Params["BusinessName"]))
                queryParams.WhereClause += $" and BusinessName {WebHelper.Params["BusinessName"].ToSaftVagueSql(isVague)}";

            if (!string.IsNullOrEmpty(WebHelper.Params["LoginName"]))
                queryParams.WhereClause += $" and LoginName {WebHelper.Params["LoginName"].ToSaftVagueSql(isVague)}";

            if (!string.IsNullOrEmpty(WebHelper.Params["IP"]))
                queryParams.WhereClause += $" and IP {WebHelper.Params["IP"].ToSaftVagueSql(isVague)}";

            if (!string.IsNullOrEmpty(WebHelper.Params["Description"]))
                queryParams.WhereClause += $" and Description {WebHelper.Params["Description"].ToSaftVagueSql(isVague)}";

            var list = DbProxy.GetPageRecords<OperationLogInfo>(queryParams);
            ret.Value = this.RenderView("OperationLog.Partial", list);
            ret.Tag = queryParams.RecordCount;

            return Json(ret);
        }

        #endregion

        #region 异常日志管理

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.系统管理_异常日志)]
        public ActionResult ExceptionLog()
        {
            BackStageLocation.Path = "Index/ExceptionLog";

            return View();
        }

        [UserTypeAuthorize(UserType.后台管理)]
        [UserPermissionAuthorize(PermissionType.系统管理_异常日志)]
        public JsonResult GetExceptionLogs()
        {
            var ret = Result.Default;
            var queryParams = new QueryParams(WebHelper.Params);
            if (string.IsNullOrEmpty(queryParams.OrderBy))
            {
                queryParams.OrderBy = $"{ExceptionLogCols.Id}";
                queryParams.IsAsc = false;
            }
            var isVague = 0.TryParse(WebHelper.Params["IsVague"]) == 1; //是否为模糊查询
            queryParams.Table = $"{EntityType.ExceptionLog}";
            queryParams.SelectCols = $"[{ExceptionLogCols.Id}], [{ExceptionLogCols.LoginName}], [{ExceptionLogCols.Url}], [{ExceptionLogCols.Ip}], [{ExceptionLogCols.Message}], [{ExceptionLogCols.CreateTime}]";
            queryParams.WhereClause = "CreateTime".ToSaftBetweenDateSql(WebHelper.Params["dtStart"], WebHelper.Params["dtEnd"], true);

            if (!string.IsNullOrEmpty(WebHelper.Params["LoginName"]))
                queryParams.WhereClause += $" and LoginName {WebHelper.Params["LoginName"].ToSaftVagueSql(isVague)}";

            if (!string.IsNullOrEmpty(WebHelper.Params["Url"]))
                queryParams.WhereClause += $" and Url {WebHelper.Params["Url"].ToSaftVagueSql(isVague)}";

            if (!string.IsNullOrEmpty(WebHelper.Params["IP"]))
                queryParams.WhereClause += $" and IP {WebHelper.Params["IP"].ToSaftVagueSql(isVague)}";

            if (!string.IsNullOrEmpty(WebHelper.Params["Message"]))
                queryParams.WhereClause += $" and Message {WebHelper.Params["Message"].ToSaftVagueSql(isVague)}";

            var list = DbProxy.GetPageRecords<ExceptionLogInfo>(queryParams);
            ret.Value = this.RenderView("ExceptionLog.Partial", list);
            ret.Tag = queryParams.RecordCount;

            return Json(ret);
        }

        public ActionResult ExceptionLogDetail(int id)
        {
            var exceptionLog = provider.GetExceptionLog(id);

            return View(exceptionLog);
        }

        #endregion        
    }
}