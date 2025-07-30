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
            // ���� CefSettings ����
            var settings = new CefSettings();

            // ���û���Ŀ¼��
            // �����ʹ�� Environment.GetFolderPath ��ȡ�����ļ��У�����ֱ��ָ��һ��·����
            // ���磬��������ΪӦ�ó�������Ŀ¼�µ�һ�����ļ��У�
           
            settings.CachePath = CommonMethods.GetBaseRoot("webCache"); ;
            
                Cef.Initialize(settings);
            
        }
        /// <summary>
        /// ���������绽�ѹ���
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