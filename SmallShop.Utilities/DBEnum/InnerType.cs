namespace SmallShop.Utilities
{
    /// <summary>
    /// 是否为内部角色（由于数据库的bit类型就是0和1）
    /// </summary>
    public enum InnerType
    {
        普通 = 0,
        内置 = 1,
    };
}
