using CefSharp.WinForms;
using CefSharp;
using Buffalo.Kernel;

namespace _4KSJSign
{
    internal static class Program
    {


        public static bool IsAuto = false;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            IsAuto = LoadAuto(args);
            InitializeCefSharp();
            Application.Run(new FrmMain());
        }
        private static void InitializeCefSharp()
        {
            // 创建 CefSettings 对象
            var settings = new CefSettings();

            // 设置缓存目录。
            // 你可以使用 Environment.GetFolderPath 获取特殊文件夹，或者直接指定一个路径。
            // 例如，将其设置为应用程序数据目录下的一个子文件夹：
           
            settings.CachePath = CommonMethods.GetBaseRoot("webCache"); ;
            
                Cef.Initialize(settings);
            
        }
        /// <summary>
        /// 命令行网络唤醒功能
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool LoadAuto(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                return false;
            }
            if (!string.Equals(args[0], "-auto", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }


            return true;
        }
    }
}