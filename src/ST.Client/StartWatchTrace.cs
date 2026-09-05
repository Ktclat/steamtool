using System.Linq;
using System.Diagnostics;

namespace System.Application
{
    /// <summary>
    /// 启动耗时跟踪
    /// </summary>
    public static class StartWatchTrace
    {
        static Stopwatch? sw;

        public static void Record(string? mark = null, bool dispose = false)
        {
            if (sw != null)
            {
                if (string.IsNullOrEmpty(mark))
                {
                    if (dispose) sw.Stop();
                    else sw.Restart();
                    return;
                }
                sw.Stop();
                var args = string.Join(" ", Environment.GetCommandLineArgs().Skip(1).Take(1));
                var msg = $"{(string.IsNullOrWhiteSpace(args) ? "" : args + " ")}mark: {mark}, value: {sw.ElapsedMilliseconds}";
                Debug.WriteLine(msg);
                Console.WriteLine(msg);
                if (!dispose) sw.Restart();
            }
            else
            {
                sw = Stopwatch.StartNew();
            }
        }

        public static new string ToString() => string.Empty;

        public static long ElapsedMilliseconds => sw == null ? 0L : sw.ElapsedMilliseconds;
    }
}
