using Buffalo.Kernel;
using Buffalo.Kernel.TreadPoolManager;
using CefSharp;
using CefSharp.WinForms;
using HtmlAgilityPack;
using Sign.Unit;
using System.Text;

namespace _4KSJSign
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        private RegConfig _regConfig;
        private BlockThread _thd;
        ConfigSetting _setting;
        private bool _running;

        private List<UserInfo> _lstUser;

        private void btnStart_Click(object sender, EventArgs e)
        {
            _regConfig.IsAutoRun = chkAutoRun.Checked;
            _setting.Hour = (int)nudHour.Value;
            _setting.Minute = (int)nudMinute.Value;
            _setting.SaveConfig();
            StartThread();
           
            EnableStart(false);
        }

        private void StartThread() 
        {
            StopThread();
            _running = true;
            _thd = BlockThread.Create(RunMain);
            _thd.StartThread();


        }

        private void StopThread() 
        {
            _running = false;
            if (_thd != null)
            {
                _thd.StopThread();
            }
            _thd = null;
        }

        private void RunMain() 
        {
            DateTime dtLast = DateTime.MinValue;
            DateTime now = DateTime.MinValue;
            string hash = null;
            int hour=(int)nudHour.Value;
            int minute=(int)nudMinute.Value;
            while (_running) 
            {
                try
                {
                    now = DateTime.Now;
                    if (now.Hour != hour)
                    {
                        continue;
                    }
                    if (now.Subtract(dtLast).TotalHours<20) 
                    {
                        continue;
                    }
                    if(now.Minute< minute || now.Minute > minute + 5) 
                    {
                        continue;
                    }
                    
                    try
                    {
                        mbDisplay.Log("开始签到");
                        foreach (UserInfo info in _lstUser)
                        {
                            mbDisplay.Log(info.Name+"开始签到");
                            if (!DoLogin(info.Name, info.Password))
                            {
                                mbDisplay.Log(info.Name + " 登录失败");
                                continue;
                            }
                            CheckVisted(out hash);
                            Loginout(hash);
                            mbDisplay.Log(info.Name+" 签到完毕");
                            while (wbMain.GetBrowser().IsLoading) 
                            {
                                Thread.Sleep(200);
                            }
                            Cef.GetGlobalCookieManager().DeleteCookies("", "");
                        }
                        dtLast = now;
                    }
                    catch (Exception ex)
                    {
                        mbDisplay.LogError(ex.ToString());
                    }
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
        }

        private void EnableStart(bool isEnable) 
        {

            btnStart.Enabled = isEnable;
            btnStop.Enabled = !isEnable;
            nudHour.Enabled = isEnable;
            nudMinute.Enabled = isEnable;
            chkAutoRun.Enabled = isEnable;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopThread();
            EnableStart(true);
        }
        //https://www.4ksj.com//qiandao/?mod=sign&operation=qiandao&formhash=43aee397&format=empty
       

        private void CheckVisted(out string hash) 
        {
            hash = null;
            string html = LoadUrlHtml("https://www.4ksj.com/qiandao/");
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(@"//span[@class='btn btnvisted']");//找到状态
            if (nodes!=null && nodes.Count > 0)
            {
                mbDisplay.Log("已经签到");
                return;
            }
            if (!_running) 
            {
                return;
            }
            HtmlNode node = FindHash(doc);
            if (node == null) 
            {
                mbDisplay.LogError("找不到哈希值");
                return;
            }
            if (!_running)
            {
                return;
            }
            hash = node.GetAttributeValue("value","");
            if (string.IsNullOrWhiteSpace(hash)) 
            {
                mbDisplay.LogError("找不到哈希值");
                return;
            }
            if (!_running)
            {
                return;
            }
            string url = string.Format("https://www.4ksj.com/qiandao/?mod=sign&operation=qiandao&formhash={0}&format=empty", hash);
            html = LoadUrlHtml(url);
            

           
        }

        private void Loginout(string hash) 
        {

            string url = string.Format("https://www.4ksj.com/member.php?mod=logging&action=logout&formhash={0}", hash);
            string html = LoadUrlHtml(url);

        }

        private HtmlNode FindHash(HtmlAgilityPack.HtmlDocument doc) 
        {
            string text = null;
            foreach (HtmlNode node in doc.DocumentNode.Descendants("input"))
            {
                text = node.GetAttributeValue("type", "");
                if (!string.Equals(text,"hidden",StringComparison.CurrentCultureIgnoreCase)) 
                {
                    continue;
                }
                text = node.GetAttributeValue("name", "");
                if (!string.Equals(text, "formhash", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                return node;
            }
            return null;
        }

        /// <summary>
        /// 加载Url
        /// </summary>
        /// <param name="wbMain"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string LoadUrlHtml(string url)
        {
            
            
            for (int i = 0; i < 10; i++)
            {
                wbMain.Load(url);
                
                for (int j = 0; j < 10; j++)
                {
                    if(wbMain.Address == url) 
                    {
                        return ReadHtml();
                    }
                    Thread.Sleep(200);
                }
            }
            
            
            return null;
        }
        /// <summary>
        /// 读取Html
        /// </summary>
        /// <param name="wbMain"></param>
        /// <returns></returns>
        protected string ReadHtml()
        {
            string html = null;
            bool isLoading = true;
            while (isLoading || string.IsNullOrEmpty(html))
            {
                
                isLoading = wbMain.IsLoading;
                if (!isLoading)//等待加载完毕
                {
                    html = wbMain.GetSourceAsync().Result;
                }
                Thread.Sleep(300);
            }
            return html;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("4K世界签到助手:[v{0}]", Program.Version);
            _regConfig = new RegConfig(Application.StartupPath + "\\4KSJSign.exe -auto", "4KSJSign");
            chkAutoRun.Checked = _regConfig.IsAutoRun;
            _lstUser = UserInfo.LoadConfig();

            _setting = ConfigSetting.LoadConfig();
            nudHour.Value = _setting.Hour;
            nudMinute.Value=_setting.Minute;

            if (Program.IsAuto)
            {
                btnStart_Click(btnStart, new EventArgs());
            }
            else
            {
                EnableStart(true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadUrlHtml("https://www.4ksj.com/");
            
        }

        public bool EvaluateJavaScript(string js)
        {
            try
            {
               
                var response = wbMain.EvaluateScriptAsync(js).Result;
                if (!response.Success )
                {
                    return false;
                }
                Thread.Sleep(500);
                //var EvaluateJavaScriptResult = response.Success ? (response.Result ?? "null") : response.Message;
                while (wbMain.IsLoading)
                {
                    Thread.Sleep(200);
                }
            }
            catch (Exception e)
            {
                mbDisplay.LogError("Error while evaluating Javascript: " + e.Message);
            }
            return true;
        }

        private bool DoLogin(string name,string password) 
        {
            string html=LoadUrlHtml("https://www.4ksj.com/member.php?mod=logging&action=login");
            Thread.Sleep(500);
            StringBuilder sbScript=new StringBuilder();
            sbScript.AppendLine("document.getElementsByName (\"username\")[0].value=\""+name+"\";");
            sbScript.AppendLine("document.getElementsByName (\"password\")[0].value=\""+password.Replace("\"","\\\"")+"\";");
            //sbScript.AppendLine("document.getElementsByName (\"cookietime\")[0].checked=true;");
            
            sbScript.AppendLine("document.getElementsByName (\"loginsubmit\")[0].click();");
            //string script = "$(\"username_LOsCc\").val(\"taisandog\")";
            return EvaluateJavaScript(sbScript.ToString());

        }
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            //LoadUrlHtml("https://www.4ksj.com/");
            mbDisplay.Log("已加载用户个数:" + _lstUser.Count);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopThread();
        }
    }
}