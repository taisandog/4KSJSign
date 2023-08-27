namespace _4KSJSign
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            groupBox1 = new GroupBox();
            panel1 = new Panel();
            chkAutoRun = new CheckBox();
            nudMinute = new NumericUpDown();
            label2 = new Label();
            btnStop = new Button();
            label1 = new Label();
            btnStart = new Button();
            nudHour = new NumericUpDown();
            button1 = new Button();
            groupBox2 = new GroupBox();
            mbDisplay = new Library.MessageBox();
            groupBox3 = new GroupBox();
            wbMain = new CefSharp.WinForms.ChromiumWebBrowser();
            btnVerCode = new Button();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMinute).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudHour).BeginInit();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnVerCode);
            groupBox1.Controls.Add(panel1);
            groupBox1.Controls.Add(button1);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(800, 57);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "操作";
            // 
            // panel1
            // 
            panel1.Controls.Add(chkAutoRun);
            panel1.Controls.Add(nudMinute);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(btnStop);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnStart);
            panel1.Controls.Add(nudHour);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(314, 19);
            panel1.Name = "panel1";
            panel1.Size = new Size(483, 35);
            panel1.TabIndex = 5;
            // 
            // chkAutoRun
            // 
            chkAutoRun.AutoSize = true;
            chkAutoRun.Location = new Point(3, 9);
            chkAutoRun.Name = "chkAutoRun";
            chkAutoRun.Size = new Size(75, 21);
            chkAutoRun.TabIndex = 7;
            chkAutoRun.Text = "开机启动";
            chkAutoRun.UseVisualStyleBackColor = true;
            // 
            // nudMinute
            // 
            nudMinute.Location = new Point(250, 7);
            nudMinute.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            nudMinute.Name = "nudMinute";
            nudMinute.Size = new Size(46, 23);
            nudMinute.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(233, 10);
            label2.Name = "label2";
            label2.Size = new Size(11, 17);
            label2.TabIndex = 5;
            label2.Text = ":";
            // 
            // btnStop
            // 
            btnStop.Location = new Point(393, 3);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 29);
            btnStop.TabIndex = 2;
            btnStop.Text = "停止";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(117, 10);
            label1.Name = "label1";
            label1.Size = new Size(59, 17);
            label1.TabIndex = 4;
            label1.Text = "启动时间:";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(306, 3);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(81, 29);
            btnStart.TabIndex = 0;
            btnStart.Text = "开始";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // nudHour
            // 
            nudHour.Location = new Point(182, 7);
            nudHour.Maximum = new decimal(new int[] { 23, 0, 0, 0 });
            nudHour.Name = "nudHour";
            nudHour.Size = new Size(46, 23);
            nudHour.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(12, 22);
            button1.Name = "button1";
            button1.Size = new Size(75, 26);
            button1.TabIndex = 1;
            button1.Text = "转到首页";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(mbDisplay);
            groupBox2.Dock = DockStyle.Bottom;
            groupBox2.Location = new Point(0, 298);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(800, 152);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "日志";
            // 
            // mbDisplay
            // 
            mbDisplay.Dock = DockStyle.Fill;
            mbDisplay.Font = new Font("宋体", 10F, FontStyle.Regular, GraphicsUnit.Point);
            mbDisplay.Location = new Point(3, 19);
            mbDisplay.Name = "mbDisplay";
            mbDisplay.ShowError = true;
            mbDisplay.ShowLog = true;
            mbDisplay.ShowWarning = true;
            mbDisplay.Size = new Size(794, 130);
            mbDisplay.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(wbMain);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(0, 57);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(800, 241);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            // 
            // wbMain
            // 
            wbMain.ActivateBrowserOnCreation = false;
            wbMain.Dock = DockStyle.Fill;
            wbMain.Location = new Point(3, 19);
            wbMain.Name = "wbMain";
            wbMain.Size = new Size(794, 219);
            wbMain.TabIndex = 0;
            wbMain.Text = "wbMain";
            // 
            // btnVerCode
            // 
            btnVerCode.Location = new Point(93, 22);
            btnVerCode.Name = "btnVerCode";
            btnVerCode.Size = new Size(98, 26);
            btnVerCode.TabIndex = 8;
            btnVerCode.Text = "手动验证完毕";
            btnVerCode.UseVisualStyleBackColor = true;
            btnVerCode.Click += btnVerCode_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmMain";
            Text = "4K世界签到助手";
            FormClosing += FrmMain_FormClosing;
            Load += FrmMain_Load;
            Shown += FrmMain_Shown;
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMinute).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudHour).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Button btnStop;
        private Button button1;
        private Button btnStart;
        private CefSharp.WinForms.ChromiumWebBrowser wbMain;
        private Library.MessageBox mbDisplay;
        private Panel panel1;
        private Label label1;
        private NumericUpDown nudHour;
        private NumericUpDown nudMinute;
        private Label label2;
        private CheckBox chkAutoRun;
        private Button btnVerCode;
    }
}