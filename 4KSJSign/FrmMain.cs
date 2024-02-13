using Buffalo.Kernel;
using Buffalo.Kernel.TreadPoolManager;
using CefSharp;
using CefSharp.DevTools.DOM;
using CefSharp.WinForms;
using HtmlAgilityPack;
using Sign.Unit;
using System;
using System.Security.Policy;
using System.Text;
using System.Web;

namespace _4KSJSign
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        //private RegConfig _regConfig;
        private BlockThread _thd;
        ConfigSetting _setting;
        private bool _running;

        private UserInfo _curUser;
        //private List<UserInfo> _lstUser;
        //AutoResetEvent _autoEvent = new AutoResetEvent(false);
        private void btnStart_Click(object sender, EventArgs e)
        {

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
            //_autoEvent.Set();
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
            DateTime dtRndLast = DateTime.MinValue;//最后随机时间
            int tick = 10000;
            DateTime dtTick = DateTime.MinValue;
            int hour = (int)nudHour.Value;
            int minute = (int)nudMinute.Value;
            while (_running)
            {
                try
                {
                    now = DateTime.Now;
                    if (now.Subtract(dtLast).TotalMilliseconds < 10000)
                    {
                        continue;
                    }
                    if (DoSign(now, dtLast, hour, minute))
                    {
                        dtLast = now;
                    }
                    if (DoRandomTime(now, dtRndLast, ref hour, ref minute))
                    {
                        dtRndLast = now;
                        dtLast = DateTime.MinValue;
                    }
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
        }

        /// <summary>
        /// 判断签到
        /// </summary>
        /// <param name="now">当前时间</param>
        /// <param name="dtLast">最后签到时间</param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        private bool DoSign(DateTime now, DateTime dtLast, int hour, int minute)
        {
            if (now.Hour != hour)
            {
                return false;
            }
            if (now.Subtract(dtLast).TotalHours < 20)
            {
                return false;
            }
            if (now.Minute < minute || now.Minute > minute + 5)
            {
                return false;
            }
            string hash = null;
            mbDisplay.Log("开始全部签到");
            bool isSuccess = false;
            try
            {
                
                mbDisplay.Log("开始签到");

                isSuccess = CheckVisted(out hash);
               
                mbDisplay.Log(" 签到完毕");

                button1_Click(button1, new EventArgs());
                
            }
            catch (Exception ex)
            {
                mbDisplay.LogError(ex.ToString());
            }
            return true;
        }
        /// <summary>
        /// 重新随机时间
        /// </summary>
        private bool DoRandomTime(DateTime now, DateTime dtRndLast, ref int hour, ref int mintue)
        {
            if (now.Hour != 0)
            {
                return false;
            }
            if (now.Minute > 10)
            {
                return false;
            }
            if (now.Subtract(dtRndLast).TotalHours < 20)
            {
                return false;
            }
            Random rnd = new Random((int)DateTime.Now.Ticks);

            int chour = rnd.Next(10, 23);
            int cmintue = rnd.Next(0, 60);
            hour = chour;
            mintue = cmintue;
            this.Invoke(new Action(() =>
            {
                nudHour.Value = chour;
                nudMinute.Value = cmintue;

            }));
            _setting.Hour = chour;
            _setting.Minute = cmintue;
            _setting.SaveConfig();
            return true;
        }
        private void EnableStart(bool isEnable)
        {

            btnStart.Enabled = isEnable;
            btnStop.Enabled = !isEnable;
            nudHour.Enabled = isEnable;
            nudMinute.Enabled = isEnable;
            //chkAutoRun.Enabled = isEnable;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopThread();
            EnableStart(true);
        }
        //https://www.4ksj.com//qiandao/?mod=sign&operation=qiandao&formhash=43aee397&format=empty


        private bool CheckVisted(out string hash)
        {
            string chkurl = "https://www.4ksj.com/qiandao.php";
            hash = null;
            string html = LoadUrlHtml(chkurl);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
           
            HtmlNode node = null;//签到按钮标签
           
            bool needSign = LoadVisted(doc, out node);
            if (!needSign)
            {
                return false;
            }
            HtmlAttribute attr = node.Attributes["href"];
            if (attr == null) 
            {
                mbDisplay.Log(" 找不到签到链接");

                return false;
            }
            string url = "https://www.4ksj.com/"+ attr.Value;
            //string url = "https://www.4ksj.com/qiandao.php?sign=" + hash;
            html = LoadUrlHtml(url);
            return true;

        }

        private bool LoadVisted(HtmlAgilityPack.HtmlDocument doc, out HtmlNode signnode)
        {
            signnode = null;
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(@"//a[@class='btna']");//找到状态
            if (nodes == null && nodes.Count == 0)
            {
                mbDisplay.Log("找不到签到按钮");
                return false;
            }
            signnode= nodes[0];
            if (signnode.InnerText.Contains("今日已打卡")) 
            {
                mbDisplay.Log("已签到");
                return false;
            }

            return true;
        }

        private void Loginout(string hash)
        {

            string url = string.Format("https://www.4ksj.com/member.php?mod=logging&action=logout&formhash={0}", hash);
            string html = LoadUrlHtml(url);

        }

      

      
        /// <summary>
        /// 加载Url
        /// </summary>
        /// <param name="wbMain"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string LoadUrlHtml(string url)
        {
            url = url.Trim(' ', '\r', '\n', '/');


            for (int i = 0; i < 10; i++)
            {
                wbMain.Load(url);

                for (int j = 0; j < 10; j++)
                {
                    if (CompareUrl(url, wbMain.Address))
                    {
                        return ReadHtml();
                    }
                    Thread.Sleep(200);
                }
            }


            return null;
        }
        private bool CompareUrl(string source, string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return false;
            }
            address = address.Trim(' ', '\r', '\n', '/');
            return string.Equals(source, address, StringComparison.CurrentCultureIgnoreCase);
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
            Version? ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (ver != null)
            {
                this.Text = String.Format("4K世界签到助手:[v{0}]", ver.ToString());
            }

            string path = Application.StartupPath.TrimEnd('\\');
            bool hasSpace = path.Contains(' ');

            StringBuilder sbRoot = new StringBuilder();
            if (hasSpace)
            {
                sbRoot.Append("\"");
            }
            sbRoot.Append(path);
            sbRoot.Append("\\4KSJSign.exe");
            if (hasSpace)
            {
                sbRoot.Append("\"");
            }
            sbRoot.Append(" -auto");
            //_regConfig = new RegConfig(sbRoot.ToString(), "4KSJSign");
            //chkAutoRun.Checked = _regConfig.IsAutoRun;
            List<UserInfo> lstUser = UserInfo.LoadConfig();
            if (lstUser.Count > 0)
            {
                _curUser = lstUser[0];
                this.Text += "-" + _curUser.Name;

                mbDisplay.Log("已加载用户:" + _curUser.Name);

            }
            _setting = ConfigSetting.LoadConfig();
            nudHour.Value = _setting.Hour;
            nudMinute.Value = _setting.Minute;

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
            wbMain.Load("https://www.4ksj.com/");
        }

        public bool EvaluateJavaScript(string js)
        {
            try
            {

                var response = wbMain.EvaluateScriptAsync(js).Result;
                if (!response.Success)
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

        private bool DoLogin(string name, string password)
        {

            LoadUrlHtml("https://www.4ksj.com/member.php?mod=logging&action=login");
            Thread.Sleep(500);
            StringBuilder sbScript = new StringBuilder();
            sbScript.AppendLine("document.getElementsByName (\"username\")[0].value=\"" + name + "\";");
            sbScript.AppendLine("document.getElementsByName (\"password\")[0].value=\"" + password.Replace("\"", "\\\"") + "\";");
            //sbScript.AppendLine("document.getElementsByName (\"cookietime\")[0].checked=true;");

            sbScript.AppendLine("document.getElementsByName (\"loginsubmit\")[0].click();");
            //string script = "$(\"username_LOsCc\").val(\"taisandog\")";
            return EvaluateJavaScript(sbScript.ToString());

        }
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            //LoadUrlHtml("https://www.4ksj.com/");

        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopThread();
        }

        private void btnVerCode_Click(object sender, EventArgs e)
        {
            //_autoEvent.Set();

            if (_curUser == null)
            {
                mbDisplay.LogError("没有设置预登录，请自己登录");
                return;
            }
            DoLogin(_curUser.Name, _curUser.Password);


        }
    }
}