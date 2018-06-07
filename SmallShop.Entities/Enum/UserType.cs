using System;

namespace SmallShop.Entities
{
    [Flags]
    public enum UserType
    {
        后台管理 = 1,
        管理子帐号 = 2,
        会员帐号 = 32,
    };
}
