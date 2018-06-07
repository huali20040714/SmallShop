using SmallShop.Utilities;
using System.Web.Mvc;

namespace SmallShop.BackStage.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionHandleAttribute());
        }
    }
}
