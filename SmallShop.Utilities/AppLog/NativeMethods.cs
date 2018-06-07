using System.Runtime.InteropServices;

namespace SmallShop.Utilities.Lib.AppLog
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();
    }
}
