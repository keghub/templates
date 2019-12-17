using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace EMG.Hosted_Service
{
    public static class ExitHelper
    {
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, bool add);

        public static IHost AddExitHandler(this IHost host, Func<IHost, CtrlTypes, Task<bool>> handler)
        {
            SetConsoleCtrlHandler(types => handler(host, types).GetAwaiter().GetResult(), true);

            return host;
        }

        public static IHost AddDefaultExitHandler(this IHost host)
        {
            AddExitHandler(host, async (h, t) =>
            {
                await h.StopAsync();
                return true;
            });

            return host;
        }
    }

    public delegate bool HandlerRoutine(CtrlTypes ctrlTypes);

    public enum CtrlTypes
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT,
        CTRL_CLOSE_EVENT,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT
    }
}