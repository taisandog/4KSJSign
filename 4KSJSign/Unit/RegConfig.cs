using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
namespace Buffalo.Kernel
{
	/// <summary>
	/// RegConfig 的摘要说明。
	/// </summary>
	public class RegConfig
	{
		//private string _autoRoot = Application.StartupPath + "\\4KSJSign.exe";
		//private string _keyName = "4KSJSign";//注册表键名
		private string _autoRoot = null;
		private string _keyName = null;//注册表键名
		/// <summary>
		/// 自启动注册表操作
		/// </summary>
		/// <param name="autoRoot">程序地址</param>
		/// <param name="keyName">注册表键名</param>
		public RegConfig(string autoRoot, string keyName)
		{
			_autoRoot = autoRoot;
			_keyName = keyName;
		}

		

        //private static RegistryKey _key= Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        /// <summary>
        /// 设置是否自启动
        /// </summary>
        public bool IsAutoRun
		{
			get
			{

                using (RegistryKey autoKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false))
                {
                    return FindValue(autoKey, _keyName);
                }
			}
			set
			{
                using (RegistryKey autoKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (value)
                    {
                        if (string.Equals(autoKey.GetValue(_keyName) as string, _autoRoot))
                        {
                            return;
                        }
                        autoKey.DeleteValue(_keyName, false);
                        autoKey.SetValue(_keyName, _autoRoot, RegistryValueKind.String);
                    }
                    else
                    {
                        autoKey.DeleteValue(_keyName, false);
                    }
                }
			}
		}
		private static readonly string LastListenStateFile = AppDomain.CurrentDomain.BaseDirectory + "\\LastListenState.sav";
		/// <summary>
		/// 保存最后状态
		/// </summary>
		/// <param name="state"></param>
		public static void SaveLastListenState(bool state) 
		{
			int content = state ? 1 : 0;
			using (StreamWriter sw = new StreamWriter(LastListenStateFile, false)) 
			{
				sw.Write(content);
			}
		}
		/// <summary>
		/// 加载最后状态
		/// </summary>
		/// <param name="state"></param>
		public static bool LoadLastListenState(bool state)
		{
			using (FileStream sr = new FileStream(LastListenStateFile,FileMode.Open,FileAccess.Read))
			{
				using (BinaryReader br = new BinaryReader(sr)) 
				{
					int content = br.ReadInt32();
					return content != 0;
				}
			}
		}
		/// <summary>
		/// 注册表键中查找指定的项
		/// </summary>
		/// <param name="key">注册表键</param>
		/// <param name="val">项名</param>
		/// <returns></returns>
		private static bool FindValue(RegistryKey key,string val)
		{
			string[] subkeys=key.GetValueNames();
			for(int i=0;i<subkeys.Length;i++)
			{
				if(val==subkeys[i])
				{
					return true;
				}
				
			}
			return false;
		}
	}
}
