using SmallShop.Data;
using System.Diagnostics;

namespace SmallShop.BackStage.Business
{
    public static class CacheCenter
    {
        private static readonly DataProvider provider = DataProvider.Instance;

        private static string _Key
        {
            get
            {
                var st = new StackTrace();
                return st.GetFrame(1).GetMethod().Name;
            }
        }        
    }
}
