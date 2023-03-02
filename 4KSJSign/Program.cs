namespace _4KSJSign
{
    internal static class Program
    {
        public const string Version = "1.1";

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
            Application.Run(new FrmMain());
        }

        /// <summary>
        /// ÃüÁîÐÐÍøÂç»½ÐÑ¹¦ÄÜ
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