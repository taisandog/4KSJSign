using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
namespace Buffalo.Kernel
{
	/// <summary>
	/// RegConfig ��ժҪ˵����
	/// </summary>
	public class RegConfig
	{
		//private string _autoRoot = Application.StartupPath + "\\4KSJSign.exe";
		//private string _keyName = "4KSJSign";//ע������
		private string _autoRoot = null;
		private string _keyName = null;//ע������
		/// <summary>
		/// ������ע������
		/// </summary>
		/// <param name="autoRoot">�����ַ</param>
		/// <param name="keyName">ע������</param>
		public RegConfig(string autoRoot, string keyName)
		{
			_autoRoot = autoRoot;
			_keyName = keyName;
		}

		

        //private static RegistryKey _key= Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        /// <summary>
        /// �����Ƿ�������
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
		/// �������״̬
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
		/// �������״̬
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
		/// ע�����в���ָ������
		/// </summary>
		/// <param name="key">ע����</param>
		/// <param name="val">����</param>
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
