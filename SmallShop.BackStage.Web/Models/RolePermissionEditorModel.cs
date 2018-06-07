using System.Collections.Generic;

namespace SmallShop.BackStage.Web.Models
{
    public class RolePermissionEditorModel
    {
        public List<KeyValuePair<int, string>> Pairs { get; set; }

        public int RoleId { get; set; }
    }
}