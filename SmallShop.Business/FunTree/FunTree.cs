using System;
using System.Collections.Generic;
using System.Text;

namespace SmallShop.BackStage.Business
{
    /// <summary>
    /// 功能树
    /// </summary>
    [Serializable]
    public class FunTree : FunNode
    {
        /// <summary>
        /// 通过URL获取子功能节点
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public FunNode FindSubNode(string url)
        {
            return FindNode(this.SubNodes, url);
        }

        /// <summary>
        /// 通过权限构建子树
        /// </summary>
        public FunTree BuildSubTree(string permissions)
        {
            FunTree tree = new FunTree();
            tree.SubNodes.AddRange(BuildSubNodesByPermissions(this.SubNodes, permissions));

            return tree;
        }

        /// <summary>
        /// 根据权限递归查找节点
        /// </summary>
        /// <param name="definedNodes">功能树上预定义的节点集</param>
        /// <param name="permissions">当前用户权限</param>
        /// <returns>拥有权限的节点集</returns>
        private List<FunNode> BuildSubNodesByPermissions(List<FunNode> definedNodes, string permissions)
        {
            List<FunNode> authenticatedNodes = new List<FunNode>();
            if (definedNodes.Count == 0)
                return authenticatedNodes;

            foreach (var definedNode in definedNodes)
            {
                //是否有权限，没有定义权限代表有权限
                bool hasPermission = false;
                if (string.IsNullOrEmpty(definedNode.Permissions))
                {
                    hasPermission = true;
                }
                else
                {
                    string[] pList = definedNode.Permissions.Split(',');
                    foreach (var p in pList)
                    {
                        PermissionType permission = new PermissionType();
                        System.Enum.TryParse(p, out permission);
                        if (permissions.Contains("|" + (int)permission + "|"))
                        {
                            hasPermission = true;
                            break;
                        }
                    }
                }

                if (hasPermission)
                {
                    FunNode note = new FunNode
                    {
                        ClassName = definedNode.ClassName,
                        Permissions = definedNode.Permissions,
                        Menu = definedNode.Menu,
                        Title = definedNode.Title,
                        Url = definedNode.Url
                    };

                    List<FunNode> subNodes = BuildSubNodesByPermissions(definedNode.SubNodes, permissions);

                    //如果功能树定义子功能, 那么至少有一个有权限的节点才能显示父节点
                    if (definedNode.SubNodes.Count > 0)
                    {
                        if (subNodes.Count > 0)
                        {
                            note.SubNodes.AddRange(subNodes);
                            authenticatedNodes.Add(note);
                        }
                    }
                    else
                    {
                        authenticatedNodes.Add(note);
                    }

                }
            }

            return authenticatedNodes;
        }

        //递归查找节点
        private FunNode FindNode(List<FunNode> parentNodes, string url)
        {
            if (parentNodes.Count == 0)
                return null;

            FunNode ret = null;
            foreach (var node in parentNodes)
            {
                if (node.Url != null && node.Url.ToLower() == url.ToLower())
                    return node;

                ret = FindNode(node.SubNodes, url);
            }

            return ret;
        }

        //递归输出左边菜单列表【最多三级】
        public string RenderHtmlLeftMenu(List<FunNode> nodes)
        {
            if (nodes == null || nodes.Count == 0)
                return "";

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < nodes.Count; i++)
            {
                FunNode node = nodes[i];
                if (node.SubNodes == null || node.SubNodes.Count == 0)
                {
                    sb.AppendFormat("<li menu='{3}'><a href='{0}' target='{4}'><i class='{1}'></i><span class='menu-text'>{2}</span></a></li>", node.Url, node.ClassName, node.Title, node.Menu, node.Menu == "_blank" ? "_blank" : "_self");
                }
                else
                {
                    sb.Append("<li>");
                    sb.AppendFormat("<a href='#' class='dropdown-toggle'><i class='{0}'></i><span class='menu-text'>{1}</span><b class='arrow icon-angle-down'></b></a>", node.ClassName, node.Title);
                    sb.Append("<ul class='submenu'>");
                    foreach (FunNode item in node.SubNodes)
                    {

                        if (item.SubNodes == null || item.SubNodes.Count == 0)
                        {
                            sb.AppendFormat("<li menu='{3}'><a href='{0}' target='{4}'><i class='{1}'></i>{2}</a></li>", item.Url, item.ClassName, item.Title, item.Menu, item.Menu == "_blank" ? "_blank" : "_self");
                        }
                        else
                        {
                            sb.Append("<li>");
                            sb.AppendFormat("<a href='#' class='dropdown-toggle'><i class='{0}'></i>{1}<b class='arrow icon-angle-down'></b></a>", item.ClassName, item.Title);
                            sb.Append("<ul class='submenu'>");
                            foreach (FunNode item1 in item.SubNodes)
                            {
                                sb.AppendFormat("<li menu='{3}'><a href='{0}'><i class='{1}'></i>{2}</a></li>", item1.Url, item1.ClassName, item1.Title, item1.Menu);
                            }
                            sb.Append("</ul>");
                            sb.Append("</li>");
                        }
                    }
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }
            }
            
            return sb.ToString();
        }
    }
}
