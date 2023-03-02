using Buffalo.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _4KSJSign
{
    public class UserInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns></returns>
        public static List<UserInfo> LoadConfig() 
        {
            List<UserInfo> lstUser = new List<UserInfo>();
            string file = Path.Combine(CommonMethods.GetBaseRoot(), "App_Data/user.xml");
            if (!File.Exists(file)) 
            {
                return lstUser;
            }
            XmlDocument xml = new XmlDocument();
            xml.Load(file);
            XmlNodeList nodeUsers = xml.GetElementsByTagName("user");
            XmlAttribute attr;

            foreach(XmlNode nodeUser in nodeUsers) 
            {
                UserInfo userInfo = new UserInfo();
                attr = nodeUser.Attributes["name"];
                if (attr == null) 
                {
                    continue;
                }
                userInfo.Name = attr.InnerText;

                attr = nodeUser.Attributes["pwd"];
                if (attr == null)
                {
                    continue;
                }
                userInfo.Password = attr.InnerText;
                lstUser.Add(userInfo);

            }
            return lstUser;
        }
    }
}
