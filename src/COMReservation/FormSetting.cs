using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace COMReservation
{
    public partial class FormSetting : Form
    {
        private SortedList m_leftPorts = new SortedList();
        private SortedList m_rightPorts = new SortedList();
        private ISettingHandle m_settingHandle = null;

        public FormSetting(ISettingHandle handle)
        {
            InitializeComponent();
            lboxPorts.SelectionMode = SelectionMode.MultiSimple;
            lboxSelectPorts.SelectionMode = SelectionMode.MultiSimple;
            m_settingHandle = handle;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            nudPortStart.Minimum = 1;
            nudPortStart.Maximum = 1000;
            nudPortStart.Value = 1;
            nudPortEnd.Minimum = 1;
            nudPortEnd.Maximum = 1000;
            nudPortEnd.Value = 100;

            tboxPersonalLogFilePath.Text = AppConfig.LogFilePath;
            tboxPersonalSeesionDir.ReadOnly = true;
            tboxPersonalLogLineFormat.Text = AppConfig.LogLineFormat;
            tboxPersonalSeesionDir.Text = AppConfig.SecureCRTSessionDir;

            cboxPersonalDefaultBaudrate.Items.AddRange(AppConfig.FreqBaudrates);
            cboxPersonalDefaultBaudrate.Text = AppConfig.DefaultBaud.ToString();

            tboxGlobalSecureCRTExeFilePath.Text = AppConfig.SecureCRTExeFilePath;
            tboxGlobalSecureCRTExeFilePath.ReadOnly = true;
            tboxGlobalHistoryDir.Text = AppConfig.SecureCRTExeFilePath;

            btnPersonalSelectComAvaiableColor.BackColor = AppConfig.ColorComAvaiable;
            btnPersonalSelectComReservedByMeColor.BackColor = AppConfig.ColorComReservedByMe;
            btnPersonalSelectComReservedByOtherColor.BackColor = AppConfig.ColorComReservedByOther;

            nudComListRowHeight.Minimum = 1;
            nudComListRowHeight.Maximum = 1024;
            nudComListRowHeight.Value = AppConfig.COMListRowHeight;
            

            Version appVer = new Version(Application.ProductVersion);
            labelVersion.Text = appVer.ToString();

            LoadAllComs();

        }

        private void LoadAllComs()
        {
            m_leftPorts.Clear();
            m_rightPorts.Clear();

            lboxPorts.Items.Clear();
            lboxSelectPorts.Items.Clear();

            foreach (COMItem item in COMHandle.AllCOMs.Values)
            {
                lboxSelectPorts.Items.Add(item.Port.ToString());
            }
        }

        private void RefreshLeftRightListView()
        {

        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != MessageBox.Show("Are you sure want to re-initialite the COM ports? if so, all current status of ports will be removed.",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }

            COMHandle.Clear();
            for (uint i = (uint)nudPortStart.Value; i <= (uint)nudPortEnd.Value; i++)
            {
                COMHandle.Add(new COMItem(i));
            }

            LoadAllComs();
        }

        private void btnToRight_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lboxPorts.SelectedItems.Count; i++)
            {
                uint port = uint.Parse(lboxPorts.SelectedItems[i].ToString());
                m_leftPorts.Remove(port);
                m_rightPorts.Add(port, port);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AppConfig.SecureCRTSessionDir = tboxPersonalSeesionDir.Text;
            AppConfig.DefaultBaud = uint.Parse(cboxPersonalDefaultBaudrate.Text);
            AppConfig.COMListRowHeight = (int)nudComListRowHeight.Value;
            AppConfig.ColorComAvaiable = btnPersonalSelectComAvaiableColor.BackColor;
            AppConfig.ColorComReservedByMe = btnPersonalSelectComReservedByMeColor.BackColor;
            AppConfig.ColorComReservedByOther = btnPersonalSelectComReservedByOtherColor.BackColor;
            AppConfig.LogFilePath = tboxPersonalLogFilePath.Text;
            AppConfig.LogLineFormat = tboxPersonalLogLineFormat.Text;

            AppConfig.SecureCRTExeFilePath = tboxGlobalSecureCRTExeFilePath.Text;
            AppConfig.HistoryFolder = tboxGlobalHistoryDir.Text;

            try
            {
                AppConfig.SaveGlobalConfig();
                AppConfig.SavePersonalConfig();
            }
            catch (Exception err)
            {
                MessageBox.Show("Error found while saving configure file! error string:" + err.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }

        private void btnPersonalSelectComColor_Click(object sender, EventArgs e)
        {
            Button btn = ((Button)sender);
            ColorDialog colord = new ColorDialog();
            colord.Color = btn.BackColor;
            if (DialogResult.OK != colord.ShowDialog())
            {
                return;
            }
            btn.BackColor = colord.Color;
        }

        private void btnBrowserSessionDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = tboxPersonalSeesionDir.Text;
            //fbd.RootFolder = Environment.SpecialFolder.Personal;
            if (DialogResult.OK != fbd.ShowDialog())
            {
                return;
            }

            tboxPersonalSeesionDir.Text = fbd.SelectedPath;
        }

        private void btnGlobalBrowserSecureCRTExeFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = tboxGlobalSecureCRTExeFilePath.Text;
            ofd.Filter = "*.exe";
            if (DialogResult.OK != ofd.ShowDialog())
            {
                return;
            }

            tboxGlobalSecureCRTExeFilePath.Text = ofd.FileName;
        }

        private void btnGlobalBrowserHistoryDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = tboxGlobalHistoryDir.Text;
            if (DialogResult.OK != fbd.ShowDialog())
            {
                return;
            }

            tboxGlobalHistoryDir.Text = fbd.SelectedPath;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nudComListRowHeight_ValueChanged(object sender, EventArgs e)
        {
            if (m_settingHandle != null)
                m_settingHandle.OnChangeRowHeight((int)nudComListRowHeight.Value);
        }
    }
}
