using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sign.Unit
{
    /// <summary>
    /// 配置
    /// </summary>
    public class ConfigSetting
    {

        private static readonly string LastConfigFile = AppDomain.CurrentDomain.BaseDirectory + "\\config.json";
        /// <summary>
        /// 读取配置
        /// </summary>
        /// <returns></returns>
        public static ConfigSetting LoadConfig()
        {
            if (!File.Exists(LastConfigFile))
            {
                return new ConfigSetting();
            }
            try
            {
                string content = File.ReadAllText(LastConfigFile, Encoding.UTF8);
                return JsonConvert.DeserializeObject<ConfigSetting>(content);
            }
            catch (Exception ex) { }
            return new ConfigSetting();
        }

        /// <summary>
        /// 写入配置
        /// </summary>
        /// <returns></returns>
        public void SaveConfig()
        {

            string content = JsonConvert.SerializeObject(this);
            File.WriteAllText(LastConfigFile, content, Encoding.UTF8);
        }
        /// <summary>
        /// 时
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// 分
        /// </summary>
        public int Minute { get; set; }
    }
}
