using System;
using System.Collections.Specialized;

namespace SmallShop.Utilities
{
    /// <summary>
    /// 查询参数类。
    /// </summary>
    [Serializable]
    public class QueryParams : IQuery
    {
        private int pageIndex = 1;

        private string[] m_KeyWords = new string[] { ";", "'", "--", "xp_", "XP_", "xP_", "Xp_" };

        public QueryParams()
        {
            PageSize = 10;
            PageIndex = 1;
            OrderBy = string.Empty;
        }

        public QueryParams(NameValueCollection _params)
            : this()
        {
            if (_params["pageSize"] != null)
                this.PageSize = int.Parse(_params["pageSize"]);

            if (_params["pageIndex"] != null)
                this.PageIndex = int.Parse(_params["pageIndex"]);

            if (_params["orderBy"] != null)
                this.OrderBy = _params["orderBy"].ToString();

            if (_params["isAsc"] != null)
            {
                if (_params["isAsc"].ToString() == "1" || _params["isAsc"].ToString().ToUpper() == "TRUE")
                    this.IsAsc = true;
                else
                    this.IsAsc = false;
            }
        }

        public static QueryParams Empty
        {
            get
            {
                return new QueryParams();
            }
        }

        /// <summary>
        /// 查询的表名称
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 查询查询字段
        /// </summary>
        public string SelectCols { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string WhereClause { get; set; }

        /// <summary>
        /// 获取或设置当前页索引,默认为第1页。
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (pageIndex == 0)
                    return 1;

                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }

        /// <summary>
        /// 获取或设置每页显示记录条数,默认为每页20条。
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 获取设置记录数。
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值表示按照哪个字段进行排序。
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 是否升序。
        /// </summary>
        public bool IsAsc { get; set; }

        /// <summary>
        /// 获取Orderby 子句
        /// </summary>
        public string GetOrderByClause()
        {
            string ret = string.Empty;
            if (string.IsNullOrEmpty(OrderBy))
                return "(select 0)";

            //过滤非安全字符
            string[] _keyWords = new string[] { ";", "'", "--", "xp_", "XP_", "xP_", "Xp_" };
            if (OrderBy.In(this.m_KeyWords))
                return "(select 0)";

            return string.Format(" {0} {1} ", OrderBy.ToSafeSql(false), IsAsc ? "ASC" : "DESC");
        }
    }
}
