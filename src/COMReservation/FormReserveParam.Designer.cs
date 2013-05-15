namespace COMReservation
{
    partial class FormReserveParam
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tboxDescription = new System.Windows.Forms.TextBox();
            this.dtpExpireTime = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboxBaud = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboxSessionName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cboxCOM = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReserveOpenSecureCRT = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.rboxPriorityLow = new System.Windows.Forms.RadioButton();
            this.rboxPriorityNormal = new System.Windows.Forms.RadioButton();
            this.rboxPriorityHigh = new System.Windows.Forms.RadioButton();
            this.label17 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboxCreateInTab = new System.Windows.Forms.CheckBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.groupAdvance = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnReserve = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupAdvance.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rboxPriorityHigh);
            this.groupBox1.Controls.Add(this.rboxPriorityNormal);
            this.groupBox1.Controls.Add(this.rboxPriorityLow);
            this.groupBox1.Controls.Add(this.tboxDescription);
            this.groupBox1.Controls.Add(this.cboxCreateInTab);
            this.groupBox1.Controls.Add(this.dtpExpireTime);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboxBaud);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cboxSessionName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cboxCOM);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(8, 1);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(330, 310);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Basic";
            // 
            // tboxDescription
            // 
            this.tboxDescription.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tboxDescription.Location = new System.Drawing.Point(113, 176);
            this.tboxDescription.Multiline = true;
            this.tboxDescription.Name = "tboxDescription";
            this.tboxDescription.Size = new System.Drawing.Size(208, 99);
            this.tboxDescription.TabIndex = 4;
            // 
            // dtpExpireTime
            // 
            this.dtpExpireTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpExpireTime.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpExpireTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpireTime.Location = new System.Drawing.Point(113, 119);
            this.dtpExpireTime.Name = "dtpExpireTime";
            this.dtpExpireTime.Size = new System.Drawing.Size(208, 23);
            this.dtpExpireTime.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(7, 259);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 17);
            this.label8.TabIndex = 0;
            this.label8.Click += new System.EventHandler(this.label6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(30, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "ExpireTime";
            this.label7.Click += new System.EventHandler(this.label6_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(27, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Description";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(49, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Priority:";
            // 
            // cboxBaud
            // 
            this.cboxBaud.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxBaud.FormattingEnabled = true;
            this.cboxBaud.Location = new System.Drawing.Point(113, 83);
            this.cboxBaud.Name = "cboxBaud";
            this.cboxBaud.Size = new System.Drawing.Size(208, 25);
            this.cboxBaud.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(66, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Baud:";
            // 
            // cboxSessionName
            // 
            this.cboxSessionName.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxSessionName.FormattingEnabled = true;
            this.cboxSessionName.Location = new System.Drawing.Point(113, 50);
            this.cboxSessionName.Name = "cboxSessionName";
            this.cboxSessionName.Size = new System.Drawing.Size(208, 25);
            this.cboxSessionName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(7, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Session Name:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(10, 249);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 17);
            this.label9.TabIndex = 0;
            this.label9.Click += new System.EventHandler(this.label2_Click);
            // 
            // cboxCOM
            // 
            this.cboxCOM.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxCOM.FormattingEnabled = true;
            this.cboxCOM.Location = new System.Drawing.Point(113, 18);
            this.cboxCOM.Name = "cboxCOM";
            this.cboxCOM.Size = new System.Drawing.Size(208, 25);
            this.cboxCOM.TabIndex = 1;
            this.cboxCOM.SelectedIndexChanged += new System.EventHandler(this.cboxCOM_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(67, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "COM:";
            // 
            // btnReserveOpenSecureCRT
            // 
            this.btnReserveOpenSecureCRT.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReserveOpenSecureCRT.Location = new System.Drawing.Point(490, 315);
            this.btnReserveOpenSecureCRT.Name = "btnReserveOpenSecureCRT";
            this.btnReserveOpenSecureCRT.Size = new System.Drawing.Size(190, 43);
            this.btnReserveOpenSecureCRT.TabIndex = 1;
            this.btnReserveOpenSecureCRT.Text = " Reserve && Open SecureCRT";
            this.btnReserveOpenSecureCRT.UseVisualStyleBackColor = true;
            this.btnReserveOpenSecureCRT.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancle.Location = new System.Drawing.Point(336, 315);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(59, 43);
            this.btnCancle.TabIndex = 1;
            this.btnCancle.Text = "Cancle";
            this.btnCancle.UseVisualStyleBackColor = true;
            // 
            // rboxPriorityLow
            // 
            this.rboxPriorityLow.AutoSize = true;
            this.rboxPriorityLow.Location = new System.Drawing.Point(110, 148);
            this.rboxPriorityLow.Name = "rboxPriorityLow";
            this.rboxPriorityLow.Size = new System.Drawing.Size(49, 21);
            this.rboxPriorityLow.TabIndex = 6;
            this.rboxPriorityLow.TabStop = true;
            this.rboxPriorityLow.Text = "Low";
            this.rboxPriorityLow.UseVisualStyleBackColor = true;
            // 
            // rboxPriorityNormal
            // 
            this.rboxPriorityNormal.AutoSize = true;
            this.rboxPriorityNormal.Location = new System.Drawing.Point(161, 148);
            this.rboxPriorityNormal.Name = "rboxPriorityNormal";
            this.rboxPriorityNormal.Size = new System.Drawing.Size(70, 21);
            this.rboxPriorityNormal.TabIndex = 6;
            this.rboxPriorityNormal.TabStop = true;
            this.rboxPriorityNormal.Text = "Normal";
            this.rboxPriorityNormal.UseVisualStyleBackColor = true;
            // 
            // rboxPriorityHigh
            // 
            this.rboxPriorityHigh.AutoSize = true;
            this.rboxPriorityHigh.Location = new System.Drawing.Point(232, 148);
            this.rboxPriorityHigh.Name = "rboxPriorityHigh";
            this.rboxPriorityHigh.Size = new System.Drawing.Size(53, 21);
            this.rboxPriorityHigh.TabIndex = 6;
            this.rboxPriorityHigh.TabStop = true;
            this.rboxPriorityHigh.Text = "High";
            this.rboxPriorityHigh.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(10, 249);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(0, 17);
            this.label17.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(11, 146);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 17);
            this.label13.TabIndex = 0;
            this.label13.Text = "Run Script:";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 169);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(310, 25);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Text = "EnterPOST.vbs";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(7, 259);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 17);
            this.label2.TabIndex = 0;
            // 
            // cboxCreateInTab
            // 
            this.cboxCreateInTab.AutoSize = true;
            this.cboxCreateInTab.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxCreateInTab.Location = new System.Drawing.Point(33, 281);
            this.cboxCreateInTab.Name = "cboxCreateInTab";
            this.cboxCreateInTab.Size = new System.Drawing.Size(112, 21);
            this.cboxCreateInTab.TabIndex = 3;
            this.cboxCreateInTab.Text = "Create In Tab?";
            this.cboxCreateInTab.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            this.textBox5.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox5.Location = new System.Drawing.Point(12, 54);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(312, 23);
            this.textBox5.TabIndex = 4;
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox3.Location = new System.Drawing.Point(12, 111);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(312, 23);
            this.textBox3.TabIndex = 4;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox5.Location = new System.Drawing.Point(10, 27);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(104, 21);
            this.checkBox5.TabIndex = 5;
            this.checkBox5.Text = "Log File Path:";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox4.Location = new System.Drawing.Point(10, 86);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(140, 21);
            this.checkBox4.TabIndex = 5;
            this.checkBox4.Text = "Line Format of Log:";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // groupAdvance
            // 
            this.groupAdvance.Controls.Add(this.checkBox4);
            this.groupAdvance.Controls.Add(this.checkBox5);
            this.groupAdvance.Controls.Add(this.textBox3);
            this.groupAdvance.Controls.Add(this.textBox5);
            this.groupAdvance.Controls.Add(this.label2);
            this.groupAdvance.Controls.Add(this.comboBox1);
            this.groupAdvance.Controls.Add(this.label13);
            this.groupAdvance.Controls.Add(this.label17);
            this.groupAdvance.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupAdvance.Location = new System.Drawing.Point(346, 1);
            this.groupAdvance.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupAdvance.Name = "groupAdvance";
            this.groupAdvance.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupAdvance.Size = new System.Drawing.Size(333, 310);
            this.groupAdvance.TabIndex = 2;
            this.groupAdvance.TabStop = false;
            this.groupAdvance.Text = "Advance";
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClear.Location = new System.Drawing.Point(7, 315);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(59, 43);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnReserve
            // 
            this.btnReserve.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReserve.Location = new System.Drawing.Point(406, 315);
            this.btnReserve.Name = "btnReserve";
            this.btnReserve.Size = new System.Drawing.Size(75, 43);
            this.btnReserve.TabIndex = 1;
            this.btnReserve.Text = "Reserve";
            this.btnReserve.UseVisualStyleBackColor = true;
            // 
            // FormReserveParam
            // 
            this.AcceptButton = this.btnReserveOpenSecureCRT;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancle;
            this.ClientSize = new System.Drawing.Size(686, 365);
            this.Controls.Add(this.groupAdvance);
            this.Controls.Add(this.btnReserve);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnReserveOpenSecureCRT);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormReserveParam";
            this.Text = "Reserve Parameters";
            this.Load += new System.EventHandler(this.FormReserveParam_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupAdvance.ResumeLayout(false);
            this.groupAdvance.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboxCOM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboxBaud;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboxSessionName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpExpireTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tboxDescription;
        private System.Windows.Forms.Button btnReserveOpenSecureCRT;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rboxPriorityHigh;
        private System.Windows.Forms.RadioButton rboxPriorityNormal;
        private System.Windows.Forms.RadioButton rboxPriorityLow;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cboxCreateInTab;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.GroupBox groupAdvance;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnReserve;
    }
}