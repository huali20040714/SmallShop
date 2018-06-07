using SmallShop.Entities;
using SmallShop.Utilities.DbProxy;
using System.Collections.Generic;

namespace SmallShop.Data
{
    public partial class DataProvider
    {
        #region 获取登录用户权限

        public List<RoleInfo> GetRoleUserPermissions(int userId)
        {
            string sql = $@"SELECT r.* FROM [RoleUser] ru with(nolock) 
                            inner join [Role] r with(nolock) on r.Id = ru.RoleId
                            where ru.UserId = {userId}";

            return DbProxy.ReadAll<RoleInfo>(sql);
        }

        #endregion
    }
}
