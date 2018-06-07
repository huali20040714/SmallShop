using System.Collections.Generic;

namespace SmallShop.BackStage.Business
{
    public class BackStageLocation
    {
        private static List<LocationNode> nodes = new List<LocationNode>();

        public static string Path { private get; set; }

        static BackStageLocation()
        {
            nodes.Add(new LocationNode { Name = "Index", Text = "用户首页", Url = "/Home/Index" });
            nodes.Add(new LocationNode { Name = "UpdatePassword", Text = "修改密码" });
            nodes.Add(new LocationNode { Name = "AgentManager", Text = "代理管理" });
            nodes.Add(new LocationNode { Name = "User", Text = "用户管理" });
            nodes.Add(new LocationNode { Name = "Agent", Text = "代理管理" });
            nodes.Add(new LocationNode { Name = "Member", Text = "会员管理" });
            nodes.Add(new LocationNode { Name = "SubAccount", Text = "子帐号管理" });


            nodes.Add(new LocationNode { Name = "OperationLog", Text = "操作日志" });
            nodes.Add(new LocationNode { Name = "ExceptionLog", Text = "异常日志" });
            nodes.Add(new LocationNode { Name = "Role", Text = "角色管理" });
            nodes.Add(new LocationNode { Name = "RolePermissionEditor", Text = "权限设置" });
        }

        public static string Write()
        {
            var path = Path;
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            Path = string.Empty;

            string[] names = path.Split(new char[] { '/' });
            string ret = string.Empty;

            for (int i = 0; i < names.Length; i++)
            {
                LocationNode node = nodes.Find(x => x.Name.ToUpper() == names[i].ToUpper());
                if (node == null)
                    continue;

                string href = string.Empty;
                if (string.IsNullOrEmpty(node.Url))
                    href = node.Text;
                else
                    href = string.Format("<a href='{0}'>{1}</a>", node.Url, node.Text);

                if (i == 0)
                {
                    ret += string.Format("<li><i class='icon-home home-icon'></i>{0}</li>", href);
                }
                else
                {
                    ret += string.Format("<li>{0}</li>", href);
                }
            }
            ret += string.Format("<script type='text/javascript'>initBreadcrumb('{0}')</script>", path);

            return ret;
        }
    }
}
