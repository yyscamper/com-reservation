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
        private SortedList<uint, COMItem> m_selectedPorts = new SortedList<uint, COMItem>();
        private SortedList<uint, COMItem> m_removedPorts = new SortedList<uint, COMItem>();
        private bool m_portInfoDirtyFlag = false;
        private ISettingHandle m_settingHandle = null;

        public FormSetting(ISettingHandle handle)
        {
            InitializeComponent();
            lboxPorts.SelectionMode = SelectionMode.One;
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
            tboxGlobalHistoryFilePath.Text = AppConfig.HistoryFileName;
            tboxHistoryFolder.Text = AppConfig.HistoryFolder;

            btnPersonalSelectComAvaiableColor.BackColor = AppConfig.ColorComAvaiable;
            btnPersonalSelectComReservedByMeColor.BackColor = AppConfig.ColorComReservedByMeNotExpired;
            btnPersonalSelectComReservedByOtherColor.BackColor = AppConfig.ColorComReservedByOther;

            nudComListRowHeight.Minimum = 1;
            nudComListRowHeight.Maximum = 1024;
            nudComListRowHeight.Value = AppConfig.COMListRowHeight;

            cboxColorSchemes.Items.AddRange(AppConfig.ColorSchemeNames);
            cboxColorSchemes.Text = AppConfig.CurrentColorScheme;

            Version appVer = new Version(Application.ProductVersion);
            labelVersion.Text = appVer.ToString();

            SortedList<uint, COMItem> allComs = COMHandle.AllCOMs;
            foreach (COMItem item in allComs.Values)
            {
                m_selectedPorts.Add(item.Port, item);
            }
            RefreshComPortsListBoxes();
        }

        private void RefreshComPortsListBoxes()
        {
            lboxPorts.Items.Clear();
            foreach (uint port in m_selectedPorts.Keys)
            {
                lboxPorts.Items.Add(port.ToString());
            }
        }

        private void btnInit_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AppConfig.SecureCRTSessionDir = tboxPersonalSeesionDir.Text;
            AppConfig.DefaultBaud = uint.Parse(cboxPersonalDefaultBaudrate.Text);
            AppConfig.COMListRowHeight = (int)nudComListRowHeight.Value;
            AppConfig.ColorComAvaiable = btnPersonalSelectComAvaiableColor.BackColor;
            AppConfig.ColorComReservedByMeNotExpired = btnPersonalSelectComReservedByMeColor.BackColor;
            AppConfig.ColorComReservedByOther = btnPersonalSelectComReservedByOtherColor.BackColor;
            AppConfig.LogFilePath = tboxPersonalLogFilePath.Text;
            AppConfig.LogLineFormat = tboxPersonalLogLineFormat.Text;

            AppConfig.SecureCRTExeFilePath = tboxGlobalSecureCRTExeFilePath.Text;
            AppConfig.HistoryFileName = tboxGlobalHistoryFilePath.Text;
            AppConfig.HistoryFolder = tboxHistoryFolder.Text;

            try
            {
                AppConfig.SaveGlobalConfig();
                AppConfig.SavePersonalConfig();
                if (m_portInfoDirtyFlag || m_removedPorts.Count > 0)
                {
                    SavePortsInfo();
                }
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
            //ofd.Filter = "*.exe";
            if (DialogResult.OK != ofd.ShowDialog())
            {
                return;
            }

            tboxGlobalSecureCRTExeFilePath.Text = ofd.FileName;
        }

        private void btnGlobalBrowserHistoryFilePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = tboxGlobalHistoryFilePath.Text;
            if (DialogResult.OK != ofd.ShowDialog())
            {
                return;
            }
            tboxGlobalHistoryFilePath.Text = ofd.FileName;
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

        private void lboxPorts_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == Keys.Delete)
            {
                if (lboxPorts.SelectedIndices.Count <= 0)
                    return;
                
                if (DialogResult.Yes != MessageBox.Show("Are you sure to delete the port " + lboxPorts.SelectedItem.ToString() + "?", "Delete Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
                COMHandle.Remove(uint.Parse(lboxPorts.SelectedItem.ToString()));
                lboxPorts.Items.RemoveAt(lboxPorts.SelectedIndex);
            }
             * */
        }

        private void tabPage3_Leave(object sender, EventArgs e)
        {

        }

        private void btnPortsRangeAdd_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != MessageBox.Show("Are you sure want to initialize the COM ports? The removed ports list will be cleared!",
                  "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }

            m_portInfoDirtyFlag = true;
            m_selectedPorts.Clear();
            m_removedPorts.Clear();
            lboxRemovedPorts.Items.Clear();

            for (uint i = (uint)nudPortStart.Value; i <= (uint)nudPortEnd.Value; i++)
            {
                COMItem item;
                item = COMHandle.FindCOM(i);
                if (item == null)
                    item = new COMItem(i);
                m_selectedPorts.Add(i, item);
            }

            RefreshComPortsListBoxes();
        }

        private void btnRemovePort_Click(object sender, EventArgs e)
        {
            int cnt = lboxPorts.SelectedIndices.Count;
            for (int i = 0; i < cnt; i++)
            {
                int idx = lboxPorts.SelectedIndices[i];
                uint port = m_selectedPorts.Keys[idx];
                m_removedPorts.Add(port, m_selectedPorts[port]);
                m_selectedPorts.RemoveAt(idx);
                lboxPorts.Items.RemoveAt(idx);
            }

            if (cnt > 0)
            {
                RefreshRemovedPortsView();
            }
        }

        private void RefreshRemovedPortsView()
        {
            lboxRemovedPorts.Items.Clear();
            foreach (int i in m_removedPorts.Keys)
            {
                lboxRemovedPorts.Items.Add(i);
            }

        }

        private void btnResumePort_Click(object sender, EventArgs e)
        {
            int cnt = lboxRemovedPorts.SelectedIndices.Count;
            for (int i = 0; i < cnt; i++)
            {
                int idx = lboxRemovedPorts.SelectedIndices[i];
                uint port = m_removedPorts.Keys[idx];
                m_selectedPorts.Add(port, m_removedPorts[port]);
                m_removedPorts.RemoveAt(idx);
                lboxRemovedPorts.Items.RemoveAt(idx);
            }

            if (cnt > 0)
            {
                RefreshComPortsListBoxes();
            }
        }

        private void btnClearPorts_Click(object sender, EventArgs e)
        {
            m_selectedPorts.Clear();
            m_removedPorts.Clear();
            lboxPorts.Items.Clear();
            lboxRemovedPorts.Items.Clear();
            m_portInfoDirtyFlag = true;
        }

        private void SavePortsInfo()
        {
            COMHandle.Clear();
            StringBuilder strComs = new StringBuilder();

            foreach (int i in m_selectedPorts.Keys)
            {
                COMHandle.Add(m_selectedPorts[(uint)i]);
                strComs.Append(i.ToString() + ",");
            }

            if (strComs[strComs.Length - 1] == ',')
                strComs.Remove(strComs.Length - 1, 1);

            HistoryWritter.Write("Save the ports info: " + strComs.ToString());
            AppConfig.SaveComInfo();
        }

        private void btnSavePortsInfo_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != MessageBox.Show("Are you sure want to save the COM ports?",
                   "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }

            SavePortsInfo();
        }

        private void btnPortsSingleAdd_Click(object sender, EventArgs e)
        {
            uint port = 0xffffff;
            try
            {
                port = uint.Parse(tboxPortSingleAddInput.Text);
            }
            catch
            {
                MessageBox.Show("Invalid port number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (m_selectedPorts.Keys.Contains(port))
            {
                MessageBox.Show("The port " + port + " has already in the list!", "Add Port Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            COMItem item = COMHandle.FindCOM(port);
            if (item == null)
                item = new COMItem();

            m_portInfoDirtyFlag = true;
            m_selectedPorts.Add(port, item);
            RefreshComPortsListBoxes();
        }

        private void btnGlobalBrowserHistoryDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = AppConfig.HistoryFolder;
            if (DialogResult.OK != fbd.ShowDialog())
            {
                return;
            }

            tboxHistoryFolder.Text = fbd.SelectedPath;
        }
    }
}
