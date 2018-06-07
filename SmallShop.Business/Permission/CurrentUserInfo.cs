using System;
using System.Collections.Generic;
using SmallShop.Entities;

namespace SmallShop.BackStage.Business
{
    [Serializable]
    public class CurrentUserInfo
    {
        public CurrentUserInfo()
        {
            Permissions = string.Empty;
            FunTreeHtml = string.Empty;
        }

        /// <summary>
        /// 登录用户
        /// </summary>
        public UserInfo User { get; set; }

        /// <summary>
        /// 授权帐号
        /// </summary>
        public UserInfo MainUser { get; set; }

        /// <summary>
        /// 用户所属角色的权限的集合
        /// </summary>
        public string Permissions { get; set; }

        public string FunTreeHtml { get; set; }

        /// <summary>
        /// 当前用户是否有权限
        /// </summary>
        public bool HasPermission(PermissionType permissionType)
        {
            if (string.IsNullOrEmpty(Permissions))
                return false;

            return Permissions.Contains(string.Format("|{0}|", (int)permissionType));
        }
    }
}
