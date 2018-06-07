namespace SmallShop.Utilities
{
    /// <summary>
    /// 查询接口。
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// 查询的表名称
        /// </summary>
        string Table { get; set; }

        /// <summary>
        /// 查询查询字段
        /// </summary>
        string SelectCols { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        string WhereClause { get; set; }

        /// <summary>
        /// 获取或设置当前页索引,默认为第1页。
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 获取或设置每页显示记录条数,默认为每页20条。
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 获取设置记录数。
        /// </summary>
        int RecordCount { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值表示按照哪个字段进行排序。
        /// </summary>
        string OrderBy { get; set; }

        /// <summary>
        /// 是否升序。
        /// </summary>
        bool IsAsc { get; set; }

        /// <summary>
        /// 获取Orderby 子句
        /// </summary>
        string GetOrderByClause();        
    }
}
