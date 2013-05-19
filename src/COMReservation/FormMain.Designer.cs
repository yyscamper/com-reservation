namespace COMReservation
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cboxEnableLogLineFormat = new System.Windows.Forms.CheckBox();
            this.cboxEnableLogFilePath = new System.Windows.Forms.CheckBox();
            this.tboxLogLineFormat = new System.Windows.Forms.TextBox();
            this.tboxDescription = new System.Windows.Forms.TextBox();
            this.tboxLogFilePath = new System.Windows.Forms.TextBox();
            this.cboxCreateInTab = new System.Windows.Forms.CheckBox();
            this.dtpExpireTime = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboxActionScripts = new System.Windows.Forms.ComboBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.cboxBaud = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboxSessionName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cboxCOM = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReserve = new System.Windows.Forms.Button();
            this.groupCOMDetail = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lableLogFilePathHelp = new System.Windows.Forms.Label();
            this.groupActionButton = new System.Windows.Forms.GroupBox();
            this.btnDeleteWait = new System.Windows.Forms.Button();
            this.btnAddWait = new System.Windows.Forms.Button();
            this.groupFilter = new System.Windows.Forms.GroupBox();
            this.rbtnReservedByMe = new System.Windows.Forms.RadioButton();
            this.rbtnAvaiable = new System.Windows.Forms.RadioButton();
            this.rbtnAll = new System.Windows.Forms.RadioButton();
            this.btnSetting = new System.Windows.Forms.Button();
            this.ctxMenuLogHelp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnEdit = new System.Windows.Forms.Button();
            this.listViewComTable = new System.Windows.Forms.ListView();
            this.groupCOMDetail.SuspendLayout();
            this.groupActionButton.SuspendLayout();
            this.groupFilter.SuspendLayout();
            this.ctxMenuLogHelp.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(469, 11);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(61, 33);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cboxEnableLogLineFormat
            // 
            this.cboxEnableLogLineFormat.AutoSize = true;
            this.cboxEnableLogLineFormat.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxEnableLogLineFormat.Location = new System.Drawing.Point(7, 280);
            this.cboxEnableLogLineFormat.Name = "cboxEnableLogLineFormat";
            this.cboxEnableLogLineFormat.Size = new System.Drawing.Size(168, 21);
            this.cboxEnableLogLineFormat.TabIndex = 5;
            this.cboxEnableLogLineFormat.Text = "Timestamp At Each Line:";
            this.cboxEnableLogLineFormat.UseVisualStyleBackColor = true;
            this.cboxEnableLogLineFormat.CheckedChanged += new System.EventHandler(this.cboxEnableLogLineFormat_CheckedChanged);
            // 
            // cboxEnableLogFilePath
            // 
            this.cboxEnableLogFilePath.AutoSize = true;
            this.cboxEnableLogFilePath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxEnableLogFilePath.Location = new System.Drawing.Point(7, 227);
            this.cboxEnableLogFilePath.Name = "cboxEnableLogFilePath";
            this.cboxEnableLogFilePath.Size = new System.Drawing.Size(104, 21);
            this.cboxEnableLogFilePath.TabIndex = 5;
            this.cboxEnableLogFilePath.Text = "Log File Path:";
            this.cboxEnableLogFilePath.UseVisualStyleBackColor = true;
            this.cboxEnableLogFilePath.CheckedChanged += new System.EventHandler(this.cboxEnableLogFilePath_CheckedChanged);
            // 
            // tboxLogLineFormat
            // 
            this.tboxLogLineFormat.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tboxLogLineFormat.Location = new System.Drawing.Point(9, 302);
            this.tboxLogLineFormat.Name = "tboxLogLineFormat";
            this.tboxLogLineFormat.Size = new System.Drawing.Size(259, 23);
            this.tboxLogLineFormat.TabIndex = 4;
            // 
            // tboxDescription
            // 
            this.tboxDescription.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tboxDescription.Location = new System.Drawing.Point(113, 146);
            this.tboxDescription.Multiline = true;
            this.tboxDescription.Name = "tboxDescription";
            this.tboxDescription.Size = new System.Drawing.Size(155, 73);
            this.tboxDescription.TabIndex = 4;
            // 
            // tboxLogFilePath
            // 
            this.tboxLogFilePath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tboxLogFilePath.Location = new System.Drawing.Point(7, 249);
            this.tboxLogFilePath.Name = "tboxLogFilePath";
            this.tboxLogFilePath.Size = new System.Drawing.Size(261, 23);
            this.tboxLogFilePath.TabIndex = 4;
            // 
            // cboxCreateInTab
            // 
            this.cboxCreateInTab.AutoSize = true;
            this.cboxCreateInTab.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxCreateInTab.Location = new System.Drawing.Point(8, 383);
            this.cboxCreateInTab.Name = "cboxCreateInTab";
            this.cboxCreateInTab.Size = new System.Drawing.Size(112, 21);
            this.cboxCreateInTab.TabIndex = 3;
            this.cboxCreateInTab.Text = "Create In Tab?";
            this.cboxCreateInTab.UseVisualStyleBackColor = true;
            // 
            // dtpExpireTime
            // 
            this.dtpExpireTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpExpireTime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpExpireTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpireTime.Location = new System.Drawing.Point(113, 116);
            this.dtpExpireTime.Name = "dtpExpireTime";
            this.dtpExpireTime.Size = new System.Drawing.Size(155, 23);
            this.dtpExpireTime.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(7, 259);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 17);
            this.label8.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(30, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "ExpireTime";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(27, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Description";
            // 
            // cboxActionScripts
            // 
            this.cboxActionScripts.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxActionScripts.FormattingEnabled = true;
            this.cboxActionScripts.Location = new System.Drawing.Point(8, 351);
            this.cboxActionScripts.Name = "cboxActionScripts";
            this.cboxActionScripts.Size = new System.Drawing.Size(260, 25);
            this.cboxActionScripts.TabIndex = 1;
            this.cboxActionScripts.Text = "EnterPOST.vbs";
            // 
            // btnOpen
            // 
            this.btnOpen.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpen.Location = new System.Drawing.Point(142, 52);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(130, 33);
            this.btnOpen.TabIndex = 4;
            this.btnOpen.Text = "Open SecureCRT";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(9, 332);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 17);
            this.label11.TabIndex = 0;
            this.label11.Text = "Action:";
            // 
            // cboxBaud
            // 
            this.cboxBaud.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxBaud.FormattingEnabled = true;
            this.cboxBaud.Location = new System.Drawing.Point(113, 82);
            this.cboxBaud.Name = "cboxBaud";
            this.cboxBaud.Size = new System.Drawing.Size(155, 25);
            this.cboxBaud.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(66, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Baud:";
            // 
            // cboxSessionName
            // 
            this.cboxSessionName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxSessionName.FormattingEnabled = true;
            this.cboxSessionName.Location = new System.Drawing.Point(113, 48);
            this.cboxSessionName.Name = "cboxSessionName";
            this.cboxSessionName.Size = new System.Drawing.Size(155, 25);
            this.cboxSessionName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(7, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Session Name:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(10, 249);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 17);
            this.label9.TabIndex = 0;
            // 
            // cboxCOM
            // 
            this.cboxCOM.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxCOM.FormattingEnabled = true;
            this.cboxCOM.Location = new System.Drawing.Point(113, 15);
            this.cboxCOM.Name = "cboxCOM";
            this.cboxCOM.Size = new System.Drawing.Size(155, 25);
            this.cboxCOM.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(67, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "COM:";
            // 
            // btnReserve
            // 
            this.btnReserve.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReserve.Location = new System.Drawing.Point(140, 12);
            this.btnReserve.Name = "btnReserve";
            this.btnReserve.Size = new System.Drawing.Size(130, 34);
            this.btnReserve.TabIndex = 5;
            this.btnReserve.Text = "+Reserve";
            this.btnReserve.UseVisualStyleBackColor = true;
            this.btnReserve.Click += new System.EventHandler(this.btnReserve_Click);
            // 
            // groupCOMDetail
            // 
            this.groupCOMDetail.Controls.Add(this.label2);
            this.groupCOMDetail.Controls.Add(this.lableLogFilePathHelp);
            this.groupCOMDetail.Controls.Add(this.cboxEnableLogLineFormat);
            this.groupCOMDetail.Controls.Add(this.cboxEnableLogFilePath);
            this.groupCOMDetail.Controls.Add(this.tboxLogLineFormat);
            this.groupCOMDetail.Controls.Add(this.tboxDescription);
            this.groupCOMDetail.Controls.Add(this.tboxLogFilePath);
            this.groupCOMDetail.Controls.Add(this.cboxCreateInTab);
            this.groupCOMDetail.Controls.Add(this.dtpExpireTime);
            this.groupCOMDetail.Controls.Add(this.label8);
            this.groupCOMDetail.Controls.Add(this.label7);
            this.groupCOMDetail.Controls.Add(this.label6);
            this.groupCOMDetail.Controls.Add(this.cboxActionScripts);
            this.groupCOMDetail.Controls.Add(this.label11);
            this.groupCOMDetail.Controls.Add(this.cboxBaud);
            this.groupCOMDetail.Controls.Add(this.label4);
            this.groupCOMDetail.Controls.Add(this.cboxSessionName);
            this.groupCOMDetail.Controls.Add(this.label3);
            this.groupCOMDetail.Controls.Add(this.label9);
            this.groupCOMDetail.Controls.Add(this.cboxCOM);
            this.groupCOMDetail.Controls.Add(this.label1);
            this.groupCOMDetail.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupCOMDetail.Location = new System.Drawing.Point(538, 42);
            this.groupCOMDetail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupCOMDetail.Name = "groupCOMDetail";
            this.groupCOMDetail.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupCOMDetail.Size = new System.Drawing.Size(278, 409);
            this.groupCOMDetail.TabIndex = 2;
            this.groupCOMDetail.TabStop = false;
            this.groupCOMDetail.Enter += new System.EventHandler(this.groupCOMDetail_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(169, 281);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "?";
            this.label2.Click += new System.EventHandler(this.lableLogFilePathHelp_Click);
            // 
            // lableLogFilePathHelp
            // 
            this.lableLogFilePathHelp.AutoSize = true;
            this.lableLogFilePathHelp.Location = new System.Drawing.Point(106, 229);
            this.lableLogFilePathHelp.Name = "lableLogFilePathHelp";
            this.lableLogFilePathHelp.Size = new System.Drawing.Size(14, 17);
            this.lableLogFilePathHelp.TabIndex = 6;
            this.lableLogFilePathHelp.Text = "?";
            this.lableLogFilePathHelp.Click += new System.EventHandler(this.lableLogFilePathHelp_Click);
            // 
            // groupActionButton
            // 
            this.groupActionButton.Controls.Add(this.btnDeleteWait);
            this.groupActionButton.Controls.Add(this.btnAddWait);
            this.groupActionButton.Controls.Add(this.btnReserve);
            this.groupActionButton.Controls.Add(this.btnOpen);
            this.groupActionButton.Location = new System.Drawing.Point(538, 447);
            this.groupActionButton.Name = "groupActionButton";
            this.groupActionButton.Size = new System.Drawing.Size(278, 91);
            this.groupActionButton.TabIndex = 6;
            this.groupActionButton.TabStop = false;
            // 
            // btnDeleteWait
            // 
            this.btnDeleteWait.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeleteWait.Location = new System.Drawing.Point(7, 12);
            this.btnDeleteWait.Name = "btnDeleteWait";
            this.btnDeleteWait.Size = new System.Drawing.Size(129, 33);
            this.btnDeleteWait.TabIndex = 5;
            this.btnDeleteWait.Text = "-Wait";
            this.btnDeleteWait.UseVisualStyleBackColor = true;
            this.btnDeleteWait.Click += new System.EventHandler(this.btnDeleteWait_Click);
            // 
            // btnAddWait
            // 
            this.btnAddWait.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddWait.Location = new System.Drawing.Point(8, 51);
            this.btnAddWait.Name = "btnAddWait";
            this.btnAddWait.Size = new System.Drawing.Size(129, 33);
            this.btnAddWait.TabIndex = 5;
            this.btnAddWait.Text = "+Wait";
            this.btnAddWait.UseVisualStyleBackColor = true;
            this.btnAddWait.Click += new System.EventHandler(this.btnAddWait_Click);
            // 
            // groupFilter
            // 
            this.groupFilter.Controls.Add(this.rbtnReservedByMe);
            this.groupFilter.Controls.Add(this.rbtnAvaiable);
            this.groupFilter.Controls.Add(this.rbtnAll);
            this.groupFilter.Location = new System.Drawing.Point(192, 5);
            this.groupFilter.Name = "groupFilter";
            this.groupFilter.Size = new System.Drawing.Size(259, 39);
            this.groupFilter.TabIndex = 7;
            this.groupFilter.TabStop = false;
            // 
            // rbtnReservedByMe
            // 
            this.rbtnReservedByMe.AutoSize = true;
            this.rbtnReservedByMe.Location = new System.Drawing.Point(130, 12);
            this.rbtnReservedByMe.Name = "rbtnReservedByMe";
            this.rbtnReservedByMe.Size = new System.Drawing.Size(121, 21);
            this.rbtnReservedByMe.TabIndex = 0;
            this.rbtnReservedByMe.TabStop = true;
            this.rbtnReservedByMe.Text = "Reserved by Me";
            this.rbtnReservedByMe.UseVisualStyleBackColor = true;
            this.rbtnReservedByMe.CheckedChanged += new System.EventHandler(this.rbtnFilter_CheckedChanged);
            // 
            // rbtnAvaiable
            // 
            this.rbtnAvaiable.AutoSize = true;
            this.rbtnAvaiable.Location = new System.Drawing.Point(54, 12);
            this.rbtnAvaiable.Name = "rbtnAvaiable";
            this.rbtnAvaiable.Size = new System.Drawing.Size(75, 21);
            this.rbtnAvaiable.TabIndex = 0;
            this.rbtnAvaiable.TabStop = true;
            this.rbtnAvaiable.Text = "Avaiable";
            this.rbtnAvaiable.UseVisualStyleBackColor = true;
            this.rbtnAvaiable.CheckedChanged += new System.EventHandler(this.rbtnFilter_CheckedChanged);
            // 
            // rbtnAll
            // 
            this.rbtnAll.AutoSize = true;
            this.rbtnAll.Location = new System.Drawing.Point(8, 12);
            this.rbtnAll.Name = "rbtnAll";
            this.rbtnAll.Size = new System.Drawing.Size(40, 21);
            this.rbtnAll.TabIndex = 0;
            this.rbtnAll.TabStop = true;
            this.rbtnAll.Text = "All";
            this.rbtnAll.UseVisualStyleBackColor = true;
            this.rbtnAll.CheckedChanged += new System.EventHandler(this.rbtnFilter_CheckedChanged);
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(10, 11);
            this.btnSetting.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(62, 33);
            this.btnSetting.TabIndex = 0;
            this.btnSetting.Text = "Setting";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // ctxMenuLogHelp
            // 
            this.ctxMenuLogHelp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8});
            this.ctxMenuLogHelp.Name = "ctxMenuLogHelp";
            this.ctxMenuLogHelp.Size = new System.Drawing.Size(244, 180);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(243, 22);
            this.toolStripMenuItem1.Text = "%S - session name";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(243, 22);
            this.toolStripMenuItem2.Text = "%Y - four-digit year";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(243, 22);
            this.toolStripMenuItem3.Text = "%M - two-digit month";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(243, 22);
            this.toolStripMenuItem4.Text = "%D - two-digit day of month";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(243, 22);
            this.toolStripMenuItem5.Text = "%h - two-digit hour";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(243, 22);
            this.toolStripMenuItem6.Text = "%m - two-digit minute";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(243, 22);
            this.toolStripMenuItem7.Text = "%t - three-digit milliseconds";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(243, 22);
            this.toolStripMenuItem8.Text = "%% - percent (%)";
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(778, 14);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(40, 33);
            this.btnEdit.TabIndex = 0;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // listViewComTable
            // 
            this.listViewComTable.FullRowSelect = true;
            this.listViewComTable.Location = new System.Drawing.Point(10, 51);
            this.listViewComTable.MultiSelect = false;
            this.listViewComTable.Name = "listViewComTable";
            this.listViewComTable.Size = new System.Drawing.Size(520, 487);
            this.listViewComTable.TabIndex = 8;
            this.listViewComTable.UseCompatibleStateImageBehavior = false;
            this.listViewComTable.View = System.Windows.Forms.View.Details;
            this.listViewComTable.SelectedIndexChanged += new System.EventHandler(this.listViewComTable_SelectedIndexChanged);
            this.listViewComTable.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewComTable_MouseClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 546);
            this.Controls.Add(this.listViewComTable);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.groupFilter);
            this.Controls.Add(this.groupCOMDetail);
            this.Controls.Add(this.groupActionButton);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "COM Reservation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseClick);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.groupCOMDetail.ResumeLayout(false);
            this.groupCOMDetail.PerformLayout();
            this.groupActionButton.ResumeLayout(false);
            this.groupFilter.ResumeLayout(false);
            this.groupFilter.PerformLayout();
            this.ctxMenuLogHelp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox cboxEnableLogLineFormat;
        private System.Windows.Forms.CheckBox cboxEnableLogFilePath;
        private System.Windows.Forms.TextBox tboxLogLineFormat;
        private System.Windows.Forms.TextBox tboxDescription;
        private System.Windows.Forms.TextBox tboxLogFilePath;
        private System.Windows.Forms.CheckBox cboxCreateInTab;
        private System.Windows.Forms.DateTimePicker dtpExpireTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboxActionScripts;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboxBaud;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboxSessionName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboxCOM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReserve;
        private System.Windows.Forms.GroupBox groupCOMDetail;
        private System.Windows.Forms.GroupBox groupActionButton;
        private System.Windows.Forms.GroupBox groupFilter;
        private System.Windows.Forms.RadioButton rbtnReservedByMe;
        private System.Windows.Forms.RadioButton rbtnAvaiable;
        private System.Windows.Forms.RadioButton rbtnAll;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Button btnAddWait;
        private System.Windows.Forms.ContextMenuStrip ctxMenuLogHelp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lableLogFilePathHelp;
        private System.Windows.Forms.Button btnDeleteWait;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.ListView listViewComTable;
    }
}

