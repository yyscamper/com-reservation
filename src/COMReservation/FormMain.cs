using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XPTable.Models;
using XPTable.Events;

namespace COMReservation
{
    public partial class FormMain : Form, ISettingHandle
    {
        #region fileds

        //Column Index
        private int COL_PORT = 0;  //Port Number
        private int COL_OWNER = 1; //Owner
        private int COL_BAUD = 2;  //Baud rate
        private int COL_REMAIN_TIME = 3; //Remaining Time
        private int COL_DESCRIPTION = 4; //Description
        private int COL_PROCESS_INFO = 5; //Process Information

        //BackgroundWorder to update COM information without stop
        BackgroundWorker m_bgkWorker = null;
        private bool m_pauseBackgroundWorkFlag = false; //The flag to pause background worker
        private bool m_pauseBackgroundWorkDone = false; //Inidiate whether the background worker has done pause
        private bool m_backgroundWorkerInitReady = false; //Inidiate whether the background worker has been initialized

        BackgroundWorker m_bgkWorkerUpdateProcessInfo = null;
        private bool m_updaetProcessInfoBegin = true;
        private bool m_updateProcessInfoDone = false;

        //XPTable components
        private TableModel  m_xpTableModel;
        private ColumnModel m_xpColumnModel;
        private XPTable.Models.Table m_xpTable;

        //Other
        private int m_lastFilterIndex = -100; //Last selected filter index
        private ArrayList m_portList = new ArrayList();

        #endregion

        #region form_main

        public FormMain()
        {
            InitializeComponent();

            //Form initialization
            this.Text = Properties.Resources.strAppName + "(" + System.Environment.UserDomainName + "\\" + System.Environment.UserName + ")";

            Control.CheckForIllegalCrossThreadCalls = false; //Ingore the thread inter-conflication
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            //XTable initialization
            m_xpTable = new Table();
            m_xpColumnModel = new XPTable.Models.ColumnModel();
            m_xpTableModel = new XPTable.Models.TableModel();
            m_xpTable.TableModel = m_xpTableModel;
            m_xpTable.ColumnModel = m_xpColumnModel;
            XPTable.Renderers.HeaderRenderer headRen = new XPTable.Renderers.XPHeaderRenderer();
            headRen.Font = new Font("Microsoft Yahei", 10, FontStyle.Regular);
            m_xpTable.HeaderRenderer = headRen;
            this.Controls.Add(m_xpTable);
            string[] colNames = new string[] {"Port", "Owner", "Baud", "Remain Time", "Description", "Process"};
            int[]    colWidths = new int[]   {    40,      160,     60,            90,           180,      200 };
            for (int i = 0; i < colNames.Length; i++)
            {
                m_xpColumnModel.Columns.Add(new TextColumn(colNames[i], colWidths[i]));
                m_xpColumnModel.Columns[i].Sortable = false;
            }
            m_xpTable.FullRowSelect = true;
            m_xpTableModel.RowHeight = AppConfig.COMListRowHeight;
            m_xpTable.CellClick += new XPTable.Events.CellMouseEventHandler(xpTable_CellClick);
            //m_xpTable.CellMouseHover += new XPTable.Events.CellMouseEventHandler(xpTable_CellMouseHover);
            //m_xpTable.HoverTime = 500;

            //Filter
            cboxFilter.Items.AddRange(new string[] {
                Properties.Resources.strFilterByAll,
                Properties.Resources.strFilterAvaiable,
                Properties.Resources.strFilterReservedByMe,
                Properties.Resources.strFilterReservedByOthers,
                Properties.Resources.strFilterIllegal,
                Properties.Resources.strFilterRunning,
                Properties.Resources.strFilterTimeExpired});
            cboxFilter.Text = Properties.Resources.strFilterByAll;
            radioBtnAllComs.Checked = true;

            //COM Info
            cboxExpireTimeUnit.Items.AddRange(new string[] { "Minutes", "Hours", "Days" });
            cboxExpireTimeUnit.SelectedIndex = 1;
            cboxEnableLogFilePath.Checked = false;
            tboxLogFilePath.Text = AppConfig.LogFilePath;
            tboxLogFilePath.Enabled = false;
            cboxEnableLogLineFormat.Checked = false;
            tboxLogLineFormat.Text = AppConfig.LogLineFormat;
            tboxLogLineFormat.Enabled = false;
            cboxBaud.Items.AddRange(AppConfig.FreqBaudrates);
            cboxCOM.Enabled = false;

            //Action Buttons
            groupActionButton.Enabled = groupCOMDetail.Enabled = true;
            btnReleaseAll.Visible = true;
            btnActionSecureCRT.Text = Properties.Resources.strActionSecureCrtOpen;
            
            //Adjust the components' location and size
            LocateSizeFitByTableSize();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //Load COM info
            ReloadComInfo();

            //Initialize and run the background worker
            m_bgkWorker = new BackgroundWorker();
            m_bgkWorker.DoWork += new DoWorkEventHandler(BackgroudUpdateView);
            m_bgkWorker.RunWorkerAsync();
            m_bgkWorker.WorkerSupportsCancellation = true;
            m_backgroundWorkerInitReady = true;

            m_bgkWorkerUpdateProcessInfo = new BackgroundWorker();
            m_bgkWorkerUpdateProcessInfo.DoWork += new DoWorkEventHandler(BackgroudUpdateProcessInfo);
            m_bgkWorkerUpdateProcessInfo.RunWorkerAsync();
            m_bgkWorkerUpdateProcessInfo.WorkerSupportsCancellation = true;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            HistoryWritter.Write("close the application.");
            AppConfig.SaveComInfo();
            AppConfig.SaveGlobalConfig();
            AppConfig.SavePersonalConfig();
        }

        private void LocateSizeFitByTableSize()
        {
            //Button
            btnSetting.Location = new Point(10, 10);
            btnRefresh.Location = new Point(btnSetting.Size.Width + btnSetting.Location.X + 10,
                                            btnSetting.Location.Y);

            int maxTitleHeight = Math.Max(Math.Max(btnSetting.Height, btnRefresh.Height), groupFilter.Height);

            //Row height of table 
            m_xpTableModel.RowHeight = AppConfig.COMListRowHeight;

            //Table width
            int tableWidth = 0;
            Point tableLocation;

            tableLocation = new Point(10, maxTitleHeight + 10);
            for (int i = 0; i < m_xpColumnModel.Columns.Count; i++)
            {
                tableWidth += m_xpColumnModel.Columns[i].Width;
            }
            m_xpTable.Width = tableWidth + 20;
            m_xpTable.Location = tableLocation;

            groupCOMDetail.Location = new Point(tableLocation.X + tableWidth + 40, btnSetting.Location.Y + btnSetting.Height + 3);
            groupActionButton.Location = new Point(groupCOMDetail.Location.X, groupCOMDetail.Location.Y + groupCOMDetail.Height + 4);
            groupActionButton.Width = groupCOMDetail.Width;

            m_xpTable.Height = groupActionButton.Height + groupCOMDetail.Height;

            //Filter group
            groupFilter.Location = new Point(tableLocation.X + tableWidth - groupFilter.Width + 26,
                                             btnSetting.Location.Y - 8);

            this.Width = groupCOMDetail.Location.X + groupCOMDetail.Width + 25;
            this.Height = tableLocation.Y + m_xpTable.Height + 54;
        }
        #endregion

        #region background_worker
        
        private void BackgroudUpdateProcessInfo(object sender, DoWorkEventArgs arg)
        {
            while (true)
            {
                /*while(!m_updaetProcessInfoBegin)
                {
                    System.Threading.Thread.Sleep(100);
                }*/

               // m_updateProcessInfoDone = false;
               // m_updaetProcessInfoBegin = false;

                for (int r = 0; r < m_xpTableModel.Rows.Count; r++)
                {
                    Row viewRow = m_xpTableModel.Rows[r];

                    //Find the specified COMItem for the row
                    COMItem comItem = COMHandle.FindCom(viewRow.Cells[COL_PORT].Text);
                    if (comItem == null)
                        continue;

                    comItem.Update(); //Update COM info
                }

                //m_updateProcessInfoDone = true;

                System.Threading.Thread.Sleep(200);
            }
        }

        private void BackgroudUpdateView(object sender, DoWorkEventArgs arg)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            
            while (true)
            {
                if (worker.CancellationPending) //Stop background refresh
                    return;

                //Wait until the pause flag is cleared
                if (m_pauseBackgroundWorkFlag)
                {
                    m_pauseBackgroundWorkDone = true;
                    System.Threading.Thread.Sleep(50);
                    continue;
                }

                //Read the COM info from file because other user may has modification
                AppConfig.LoadComInfo();
                SortedList<uint, COMItem> allComs = COMHandle.AllCOMs;

                btnReleaseAll.Enabled = (COMHandle.TotalNumberOfMyReserved > 0);
                
                for (int r = 0; r < m_xpTableModel.Rows.Count; r++)
                {
                    Row viewRow = m_xpTableModel.Rows[r];

                    //Find the specified COMItem for the row
                    COMItem comItem = COMHandle.FindCom(viewRow.Cells[COL_PORT].Text);
                    if (comItem == null)
                        continue;

                    //comItem.Update(); //Update COM info

                    //Automatically release the port if time expired and no process is opened
                    if (!comItem.IsAvaiable() && comItem.IsExpired && !comItem.IsRunning)
                    {
                        COMHandle.Release(comItem.Port, comItem.Owner);
                    }

                    RefreshSelectedComInfo();
                    SetTableRow(viewRow, comItem);
                }

                System.Threading.Thread.Sleep(1000);
            }
        }
    
        #endregion

        #region xpTable_functions

        private void xpTable_CellClick(object sender, XPTable.Events.CellMouseEventArgs e)
        {
            if (m_xpTable.SelectedIndicies.Length <= 0)
                return;
            int selIndex = m_xpTable.SelectedIndicies[0];
            COMItem comItem = GetComItemOfRow(e.Row);
            if (comItem == null)
                return;

            //comItem.Update();
            OnSelectCom(comItem);
            RefreshSelectedComInfo();
            //CellPos startCellPos = new CellPos(selIndex, 0);
            //CellPos endCellPos = new CellPos(selIndex, m_xpColumnModel.Columns.Count - 1);
            //m_xpTableModel.Selections.SelectCells(startCellPos, endCellPos);
        }

        public void OnChangeRowHeight(int height)
        {
            if (height > 0)
            {
                AppConfig.COMListRowHeight = height;
                m_xpTableModel.RowHeight = height;
            }
        }

        private void SetRowStyle(COMItem comItem, Row r) //Set every row with different color
        { 
            Color rowBackColor = AppConfig.ColorComAvaiable;
            if (comItem.CheckIllegal())
                rowBackColor = AppConfig.ColorComIllegal;
            else if (comItem.IsAvaiable())
                rowBackColor = AppConfig.ColorComAvaiable;
            else if (comItem.Owner == AppConfig.LoginUserFullName)
                rowBackColor = AppConfig.ColorComReservedByMeNotExpired;
            else
                rowBackColor = AppConfig.ColorComReservedByOther;

            r.BackColor = rowBackColor;
        }

        private Row AddTableRow(COMItem comItem)
        {
            if (comItem == null)
                return null;

            Row row = new Row(new string[] {
                comItem.Port.ToString(),
                comItem.Owner,
                comItem.Baud,
                comItem.StrRemainTime,
                comItem.Description,
                comItem.ProcessInfo});
            row.Editable = false;
            
            SetRowStyle(comItem, row);
            m_xpTableModel.Rows.Add(row);

            return row;
        }

        private void SetTableRow(Row row, COMItem comItem)
        {
            if (row == null || comItem == null)
                return;

            row.Cells[COL_PORT].Text = comItem.Port.ToString();
            row.Cells[COL_OWNER].Text = comItem.Owner;
            row.Cells[COL_BAUD].Text = comItem.Baud;
            row.Cells[COL_REMAIN_TIME].Text = comItem.StrRemainTime;
            row.Cells[COL_DESCRIPTION].Text = comItem.Description;
            row.Cells[COL_PROCESS_INFO].Text = comItem.ProcessInfo;

            SetRowStyle(comItem, row);
        }

        private COMItem GetComItemOfRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= m_xpTableModel.Rows.Count)
                return null;

            return COMHandle.FindCom(m_xpTableModel.Rows[rowIndex].Cells[COL_PORT].Text);
        }

        private COMItem GetSelectedCOMItem()
        {
            if (m_xpTable.SelectedIndicies.Length <= 0)
                return null;
            return GetComItemOfRow(m_xpTable.SelectedIndicies[0]);
        }

        private void RefreshSelectedComInfo()
        {
            COMItem item = GetSelectedCOMItem();
            if (item == null)
                return;

            if (item.IsAvaiable())
            {
                if (!item.IsRunning)
                {
                    btnActionSecureCRT.Enabled = false;
                    btnReserve.Enabled = true;
                    btnReserve.Text = Properties.Resources.strActionReserve;
                    btnReschedule.Enabled = false;

                    foreach (Control ctrl in groupCOMDetail.Controls)
                    {
                        if (ctrl != cboxCOM)
                            ctrl.Enabled = true;
                    }
                }
            }
            else
            {
                if (AppConfig.LoginUserFullName == item.Owner)
                {
                    btnActionSecureCRT.Enabled = true;
                    btnReserve.Enabled = true;
                    btnReserve.Text = Properties.Resources.strActionRelease;
                    btnReschedule.Enabled = true;

                    if (item.IsRunning)
                    {
                        btnActionSecureCRT.Text = Properties.Resources.strActionSecureCrtKill;
                        foreach (Control ctrl in groupCOMDetail.Controls)
                        {
                            if (ctrl != cboxCOM)
                                ctrl.Enabled = (ctrl == cboxExpireTimeValue || ctrl == cboxExpireTimeUnit);
                        }
                    }
                    else
                    {
                        btnActionSecureCRT.Text = Properties.Resources.strActionSecureCrtOpen;
                        foreach (Control ctrl in groupCOMDetail.Controls)
                        {
                            if (ctrl != cboxCOM)
                                ctrl.Enabled = true;
                        }
                    }
                }
                else
                {
                    btnActionSecureCRT.Enabled = false;
                    btnReserve.Enabled = false;
                    btnReschedule.Enabled = false;
                    foreach (Control ctrl in groupCOMDetail.Controls)
                    {
                        ctrl.Enabled = false;
                    }
                }
            }
        }

        private void OnSelectCom(COMItem item)
        {
            if (item == null)
                return;

            cboxCOM.Text = item.Port.ToString();
            cboxCOM.Enabled = false;
            cboxBaud.Text = item.Baud.ToString();
            cboxSessionName.Items.Clear();
            cboxSessionName.Items.AddRange(new string[] {
                "Serial-COM" + item.Port,
                AppConfig.LoginUserFullName + "-COM" + item.Port,
                "BMC-COM" + item.Port,
                "BIOS-COM" + item.Port,
                "POST-COM" + item.Port,
                "SSP-COM" + item.Port,
                "COM" + item.Port + "-BMC",
                "COM" + item.Port + "-BIOS",
                "COM" + item.Port + "-POST",
                "COM" + item.Port + "-SSP",
                "COM" + item.Port + "-" + AppConfig.LoginUserFullName
            });

            cboxSessionName.Text = item.SessionName;
            //dtpExpireTime.Value = DateTime.Now + new TimeSpan(4, 0, 0);

            string[] procPorts = item.ProcessOpenedPorts;
            if (procPorts == null || procPorts.Length <= 0)
            {
                statusLabelOpenedDevices.Text = "none";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < procPorts.Length; i++)
                {
                    sb.Append("COM" + procPorts[i]);
                    if (i < (procPorts.Length - 1))
                        sb.Append(", ");
                }
                statusLabelOpenedDevices.Text = sb.ToString();
            }

            RefreshSelectedComInfo();

        }
        #endregion

        #region com_function

        private void ReloadComInfo()
        {
            try
            {
                bool isNeedStartBackgroundWorker = false;
                if (m_backgroundWorkerInitReady && !m_pauseBackgroundWorkFlag)
                {
                    m_pauseBackgroundWorkDone = false;
                    m_pauseBackgroundWorkFlag = true;
                    while (!m_pauseBackgroundWorkDone) ;

                    isNeedStartBackgroundWorker = true;
                }

                if (m_lastFilterIndex != cboxFilter.SelectedIndex || AppConfig.HasModificationComInfoFile())
                {
                    m_lastFilterIndex = cboxFilter.SelectedIndex;

                    AppConfig.LoadComInfo();

                    int selRow = -1;
                    if (m_xpTable.SelectedIndicies.Length > 0)
                        selRow = m_xpTableModel.Selections.SelectedItems[0].Index;

                    m_xpTableModel.Rows.Clear();
                    SortedList<uint, COMItem> allCOMs = COMHandle.AllCOMs;
                    foreach (COMItem comItem in allCOMs.Values)
                    {
                        comItem.Update();
                    }

                    foreach (COMItem comItem in allCOMs.Values)
                    {
                        if (CheckFilter(comItem))
                        {
                            comItem.RowInTable = AddTableRow(comItem);
                            //comItem.RowInTable.EnsureVisible();
                        }
                    }

                    if (selRow > 0)
                    {
                        m_xpTableModel.Selections.SelectCells(new CellPos(selRow, 0), new CellPos(selRow, m_xpColumnModel.Columns.Count - 1));
                    }
                }

                if (m_backgroundWorkerInitReady && isNeedStartBackgroundWorker)
                    m_pauseBackgroundWorkFlag = false;

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region filter

        private void rbtnFilter_CheckedChanged(object sender, EventArgs e)
        {
            ReloadComInfo();
        }

        private void radioBtnAllComs_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnAllComs.Checked)
            {
                cboxFilter.Text = Properties.Resources.strFilterByAll;
            }
        }

        private void radioBtnAvaiable_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnAvaiable.Checked)
                cboxFilter.Text = Properties.Resources.strFilterAvaiable;
        }

        private void radioBtnReservedByMe_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnReservedByMe.Checked)
            {
                cboxFilter.Text = Properties.Resources.strFilterReservedByMe;
            }
        }

        private void cboxFilter_SelectedContentChanged(object sender, EventArgs e)
        {
            m_xpTable.Enabled = false;

            radioBtnReservedByMe.Checked = (cboxFilter.Text == Properties.Resources.strFilterReservedByMe);
            radioBtnAvaiable.Checked = (cboxFilter.Text == Properties.Resources.strFilterAvaiable);
            radioBtnAllComs.Checked = (cboxFilter.Text == Properties.Resources.strFilterByAll);

            ReloadComInfo();

            m_xpTable.Enabled = true;
        }

        private bool CheckFilter(COMItem item)
        {
            if (item == null)
                return false;

            string str = cboxFilter.Text;
            if (str == Properties.Resources.strFilterByAll)
            {
                return true;
            }
            else if (str == Properties.Resources.strFilterReservedByMe
                && item.Owner == AppConfig.LoginUserFullName)
            {
                return true;
            }
            else if (str == Properties.Resources.strFilterReservedByOthers
                && item.Owner.Length > 0 && item.Owner != AppConfig.LoginUserFullName)
            {
                return true;
            }
            else if (str == Properties.Resources.strFilterIllegal
                && item.RtCheckIllegal())
            {
                return true;
            }
            else if (str == Properties.Resources.strFilterRunning
                && item.RtSecureCrtProcess != null)
            {
                return true;
            }
            else if (str == Properties.Resources.strFilterTimeExpired
                && item.RemainTime <= new TimeSpan(0) && !item.IsAvaiable())
            {
                return true;
            }
            else if (str == Properties.Resources.strFilterAvaiable
                && item.Owner.Trim().Length == 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region action_button

        private void btnReserveRelease_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            AppConfig.LoadComInfo();

            COMItem comItem = COMHandle.FindCom(cboxCOM.Text);
            if (comItem == null)
                return;

            comItem.Update();

            this.Enabled = false;

            if (btn.Text == Properties.Resources.strActionReserve)
            {
                if (!COMHandle.CheckPortFree(comItem.Port))
                {
                    string appName = string.Empty;
                    Process proc = null;
                    string owner = Utility.GetPortOwner(comItem.Port.ToString(), ref appName, ref proc);
                    if (owner != null)
                    {
                        Utility.ShowErrorDialog("This port has already be occupied by " + owner + " using " 
                            + appName + " illegally (ProcessId=" + proc.Id + " ProcessName=" + proc.ProcessName + ")! please contact him/her.");
                    }

                    this.Enabled = true;
                    return;
                }

                COMHandle.Reserve(comItem.Port, AppConfig.LoginUserFullName, ParseExpireTime(), tboxDescription.Text);
                btnActionSecureCRT.Enabled = true;
                btnActionSecureCRT.Text = Properties.Resources.strActionSecureCrtOpen;
            }
            else
            {
                if (comItem.Owner == AppConfig.LoginUserFullName)
                {
                    if (comItem.IsRunning)
                    {
                        MessageBox.Show("Please close/kill the specified SecureCRT process before release.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                    COMHandle.Release(comItem.Port, AppConfig.LoginUserFullName);
                    btnActionSecureCRT.Enabled = false;
                }
            }

            AppConfig.SaveComInfo();

            this.Enabled = true;
        }

        private void btnOpenKill_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            COMItem item = COMHandle.FindCom(uint.Parse(cboxCOM.Text));
            if (item == null)
                return;

            this.Enabled = false;

            item.Update();

            if (btn.Text == Properties.Resources.strActionSecureCrtOpen)
            {
                string appName = string.Empty;
                Process proc = null;
                string owner = Utility.GetPortOwner(cboxCOM.Text, ref appName, ref proc);
                if (owner != null)
                {
                    Utility.ShowErrorDialog(owner + " is occuping the COM" + cboxCOM.Text + " using " + appName + "(ProcessId=" + proc.Id + " ProcessName=" + proc.ProcessName + "), you cannot open it!");
                    this.Enabled = true;
                    return;
                }

                try
                {
                    SecureCRTHandle.Open(cboxSessionName.Text, item, cboxCreateInTab.Checked);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = true;
                    return;
                }
                if (item.RtIsRunning)
                    btn.Text = Properties.Resources.strActionSecureCrtKill;
            }
            else
            {
                try
                {
                    COMHandle.KillProcess(item);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = true;
                    return;
                }
                if (!item.RtIsRunning)
                    btn.Text = Properties.Resources.strActionSecureCrtOpen;
            }

            this.Enabled = true;
        }

        private void btnReleaseAll_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Are you sure want to release all the COMs that you reserved?\n", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Enabled = false;
                try
                {
                    COMHandle.ReleaseAllReservedByMe();
                }
                catch (Exception err)
                {
                    Utility.ShowErrorDialog(err.Message);
                    this.Enabled = true;
                    return;
                }
                
                this.Enabled = true;
            }
        }

        private void btnReschedule_Click(object sender, EventArgs e)
        {
            COMItem item = GetSelectedCOMItem();
            if (item == null)
                return;

            item.ExpireTime = ParseExpireTime();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ReloadComInfo();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (m_backgroundWorkerInitReady)
            {
                m_pauseBackgroundWorkDone = false;
                m_pauseBackgroundWorkFlag = true;
                while (!m_pauseBackgroundWorkDone) ;
            }
            new FormSetting(this).ShowDialog(this);
            ReloadComInfo();

            if (m_backgroundWorkerInitReady)
            {
                m_pauseBackgroundWorkFlag = false;
            }
        }

        private void btnSearchOwner_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            string appName = string.Empty;
            Process proc = null;
            string owner = Utility.GetPortOwner(cboxCOM.Text, ref appName, ref proc);
            if (owner != null)
            {
                Utility.ShowInfoDialog(owner + " is occuping the COM" + cboxCOM + " using " + appName + "(ProcessId=" + proc.Id + " ProcessName=" + proc.ProcessName + ")!");
            }
            else
            {
                Utility.ShowInfoDialog("Cannot find the owner of COM" + cboxCOM.Text + "!");
            }
            this.Enabled = true;
        }

        #endregion

        #region com_detail

        private void lableLogFilePathHelp_Click(object sender, EventArgs e)
        {
            Label lb = (Label)sender;
            ctxMenuLogHelp.Show(lb, lb.Width, lb.Height);
        }

        private void cboxEnableLogFilePath_CheckedChanged(object sender, EventArgs e)
        {
            tboxLogFilePath.Enabled = cboxEnableLogFilePath.Checked;
        }

        private void cboxEnableLogLineFormat_CheckedChanged(object sender, EventArgs e)
        {
            tboxLogLineFormat.Enabled = cboxEnableLogLineFormat.Checked;
        }

        private void cboxExpireTimeUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            int min, max, step, setval;
            if (cboxExpireTimeUnit.Text.StartsWith("Minute"))
            {
                min = 10;
                step = 10;
                max = 60;
                setval = 30;

            }
            else if (cboxExpireTimeUnit.Text.StartsWith("Hour"))
            {
                min = 1;
                step = 1;
                max = 24;
                setval = 6;
            }
            else
            {
                min = 1;
                step = 1;
                max = 30;
                setval = 1;
            }

            cboxExpireTimeValue.Items.Clear();
            for (int i = min; i <= max; i += step)
            {
                cboxExpireTimeValue.Items.Add(i.ToString());
            }
            cboxExpireTimeValue.Text = setval.ToString();
        }

        private DateTime ParseExpireTime()
        {
            DateTime tnow = DateTime.Now;
            DateTime tnew;
            double val = double.Parse(cboxExpireTimeValue.Text);

            if (cboxExpireTimeUnit.SelectedIndex == 0)
            {
                tnew = tnow.AddMinutes(val);
            }
            else if (cboxExpireTimeUnit.SelectedIndex == 1)
            {
                tnew = tnow.AddHours(val);
            }
            else
            {
                tnew = tnow.AddDays(val);
            }

            return tnew;
        }

        #endregion

        #region misc

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            COMItem item = GetSelectedCOMItem();
            if (item == null)
                return;

            string name = null;
            if (comboTest.SelectedIndex == 0 || comboTest.SelectedIndex == 1)
                name = "Jobs";
            else
                name = "Bill Gates";

            if (comboTest.SelectedItem.ToString().StartsWith("Reserve"))
            {
                COMHandle.Reserve(item.Port, name, ParseExpireTime(), tboxDescription.Text);
            }
            else
            {
                try
                {
                    COMHandle.Release(item.Port, name);
                }
                catch (Exception err)
                {
                    Utility.ShowErrorDialog(err.Message);
                }
            }

            AppConfig.SaveComInfo();
        }

        #endregion

    }
}
