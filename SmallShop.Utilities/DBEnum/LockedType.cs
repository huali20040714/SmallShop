namespace SmallShop.Utilities
{
    /// <summary>
    /// 是否已经锁定（由于数据库的bit类型就是0和1）
    /// </summary>
    public enum LockedType
    {
        未锁定 = 0,
        已锁定 = 1,
    };
}
