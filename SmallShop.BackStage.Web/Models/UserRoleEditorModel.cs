using SmallShop.Entities;
using System.Collections.Generic;

namespace SmallShop.BackStage.Web.Models
{
    public class UserRoleEditorModel
    {
        public int UserId { get; set; }

        public List<RoleInfo> Roles { get; set; }

        public List<RoleUserInfo> RoleUser { get; set; }
    }
}