using System;
using System.Collections.Generic;

namespace SmallShop.BackStage.Business
{
    /// <summary>
    /// 功能树节点
    /// </summary>
    [Serializable]
    public class FunNode
    {
        public FunNode()
        {
            SubNodes = new List<FunNode>();
        }

        public string ClassName { get; set; }

        public string Menu { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string Permissions { get; set; }

        public List<FunNode> SubNodes { get; set; }
    }
}
