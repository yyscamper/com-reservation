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
#if USE_XPTABLE
using XPTable.Models;
using XPTable.Events;
#endif

namespace COMReservation
{
    public partial class FormMain : Form, IReserveCOM, ISettingHandle
    {
        private delegate void ThreadUpdateRemainTime();

        //Column Index
        private int COL_PORT = 0;
        private int COL_OWNER = 1;
        private int COL_BAUD = 2;
        private int COL_REMAIN_TIME = 3;
        private int COL_DESCRIPTION = 4;
        private int COL_PROCESS_INFO = 5;
        BackgroundWorker m_bgkWorker = null;
        private bool m_pauseBackgroundWorkFlag = false;
        private bool m_pauseBackgroundWorkDone = false;
        private bool m_backgroundWorkerInitReady = false;
#if USE_XPTABLE
        private TableModel  m_xpTableModel;
        private ColumnModel m_xpColumnModel;
        private XPTable.Models.Table m_xpTable;
#endif

        public FormMain()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;

            this.Text = "COM-Reversation (" + AppConfig.LoginUserName + ")";

            btnReleaseAll.Visible = true;
            cboxEnableLogFilePath.Checked = false;
            tboxLogFilePath.Text = AppConfig.LogFilePath;
            tboxLogFilePath.Enabled = false;

            cboxEnableLogLineFormat.Checked = false;
            tboxLogLineFormat.Text = AppConfig.LogLineFormat;
            tboxLogLineFormat.Enabled = false;
            cboxFilter.Text = Properties.Resources.strFilterByAll;

#if USE_XPTABLE
            Font font = new Font("Microsoft Yahei", 10, FontStyle.Regular);

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
            int[]    colWidths = new int[]   {    40,      80,     60,            90,           240,      240 };
            for (int i = 0; i < colNames.Length; i++)
            {
                m_xpColumnModel.Columns.Add(new TextColumn(colNames[i], colWidths[i]));
                m_xpColumnModel.Columns[i].Sortable = false;
            }
            m_xpTable.FullRowSelect = true;
            m_xpTableModel.RowHeight = AppConfig.COMListRowHeight;
            m_xpTable.CellClick += new XPTable.Events.CellMouseEventHandler(xpTable_CellClick);
            //m_xpTable.CellMouseHover += new XPTable.Events.CellMouseEventHandler(xpTable_CellMouseHover);
            m_xpTable.HoverTime = 500;
#else
            
            listViewComTable.Columns.Add("Port", 40);
            listViewComTable.Columns.Add("Owner", 80);
            listViewComTable.Columns.Add("Baud", 60);
            listViewComTable.Columns.Add("Remain Time", 90);
            listViewComTable.Columns.Add("Description", 240);
            listViewComTable.Columns.Add("Wait List", 120);
            listViewComTable.Columns.Add("Process", 80);
#endif

            cboxFilter.Items.AddRange(new string[] {
                Properties.Resources.strFilterByAll,
                Properties.Resources.strFilterAvaiable,
                Properties.Resources.strFilterReservedByMe,
                Properties.Resources.strFilterReservedByOthers,
                Properties.Resources.strFilterIllegal,
                Properties.Resources.strFilterRunning,
                Properties.Resources.strFilterTimeExpired});

            cboxExpireTimeUnit.Items.AddRange(new string[] { "Minutes", "Hours", "Days" });
            cboxExpireTimeUnit.SelectedIndex = 1;


            btnActionSecureCRT.Text = Properties.Resources.strActionSecureCrtOpen;

            groupActionButton.Enabled = groupCOMDetail.Enabled = true;
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }


        private void xpTable_CellMouseHover(object sender, CellMouseEventArgs e)
        {
            
            if (e.CellPos.Column == COL_PROCESS_INFO)
            {
                uint port = uint.Parse(m_xpTable.TableModel.Rows[e.CellPos.Row].Cells[COL_PORT].Text);
                COMItem item = COMHandle.FindCOM(port);
                string[] fileHandles = item.FileHandles;
                if (fileHandles == null || fileHandles.Length <= 0)
                {
                    statusLabelOpenedDevices.Text = "Opened COM Devices: none";
                    return;
                }

                StringBuilder sb = new StringBuilder("Opened COM Devices:");
                for (int i = 0; i < fileHandles.Length; i++)
                {
                    sb.Append(fileHandles[i]);
                    if (i < (fileHandles.Length - 1))
                        sb.Append(", ");
                }
                statusLabelOpenedDevices.Text = sb.ToString();

                /*
                ContextMenuStrip hoverMenu = new ContextMenuStrip();
                for (int i = 0; i < fileHandles.Length; i++)
                {
                    hoverMenu.Items.Add(fileHandles[i]);
                }
                hoverMenu.Show(m_xpTable, m_xpTable.Location.X + m_xpTable.Width - 200, m_xpTable.Location.Y + m_xpTable.RowHeight * e.CellPos.Row - 10);
                */
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            
            LocateSizeFitByTableSize();

            cboxBaud.Items.AddRange(AppConfig.FreqBaudrates);

            foreach (COMItem item in COMHandle.AllCOMs.Values)
            {
                cboxCOM.Items.Add(item.Port.ToString());
            }

            this.Text = Properties.Resources.strAppName + "(" + System.Environment.UserDomainName + "\\" + System.Environment.UserName + ")";
            RefreshCOMInfo();

            //object[] plist = { this, System.EventArgs.Empty };
            //BackgroudUpdateRemainTime.listViewComTable.BeginInvoke(new System.EventHandler(BackgroudUpdateRemainTime), plist);
            //ThreadUpdateRemainTime func = new ThreadUpdateRemainTime(BackgroudUpdateRemainTime);
            //func.BeginInvoke(null, null);
            //listViewComTable.Invoke(func, null);

            //MethodInvoker mi = new MethodInvoker(BackgroudUpdateView);
            //mi.BeginInvoke(null, null);

            m_bgkWorker = new BackgroundWorker();
            m_bgkWorker.DoWork += new DoWorkEventHandler(BackgroudUpdateView);
            m_bgkWorker.RunWorkerAsync();
            m_bgkWorker.WorkerSupportsCancellation = true;
            m_backgroundWorkerInitReady = true;
        }

        private string GetProcessString(int procId)
        {
            if (procId <= 0)
                return "";

            try
            {
                Process proc = Process.GetProcessById(procId);
                return (proc.Id.ToString() + "(" + proc.Threads.Count + ")");
            }
            catch
            {
                return "";
            }
        }

        private void BackgroudUpdateView(object sender, DoWorkEventArgs arg)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (true)
            {

                if (worker.CancellationPending) //Stop background refresh
                {
                    return;
                }

                if (m_pauseBackgroundWorkFlag)
                {
                    m_pauseBackgroundWorkDone = true;
                    System.Threading.Thread.Sleep(50);
                    continue;
                }

                AppConfig.LoadComInfo();

                btnReleaseAll.Enabled = (COMHandle.TotalNumberOfMyReserved > 0);
#if USE_XPTABLE
                for (int r = 0; r < m_xpTableModel.Rows.Count; r++)
                {
                    Row viewRow = m_xpTableModel.Rows[r];
                    uint port = 0;
                    try
                    {
                        port = uint.Parse(viewRow.Cells[COL_PORT].Text);
                    }
                    catch
                    {
                        continue;
                    }
                    COMItem comItem = COMHandle.FindCOM(port);
                    if (comItem == null)
                        continue;

                    if (!comItem.IsAvaiable() && comItem.IsExpired && !comItem.IsRunning)
                    {
                        COMHandle.Release(port, comItem.Owner);
                    }

                    RefreshSelectedComInfo();

                    viewRow.Cells[COL_REMAIN_TIME].Text = comItem.StrRemainTime;
                    viewRow.Cells[COL_PROCESS_INFO].Text = comItem.ProcessInfo;

                    if (m_xpTable.SelectedIndicies.Length > 0
                        && m_xpTable.SelectedIndicies[0] == comItem.Index)
                    {
                        if (AppConfig.LoginUserName == comItem.Owner)
                        {
                            if (comItem.SecureCrtProcess != null)
                            {
                                btnActionSecureCRT.Text = Properties.Resources.strActionSecureCrtKill;
                            }
                            else
                            {
                                btnActionSecureCRT.Text = Properties.Resources.strActionSecureCrtOpen;
                            }

                        }
                    }
#else

                for (int r = 0; r < listViewComTable.Items.Count; r++)
                {
                    ListViewItem viewRow = listViewComTable.Items[r];
                    uint port = 0;
                    try
                    {
                        port = uint.Parse(viewRow.SubItems[COL_PORT].Text);
                    }
                    catch
                    {
                        continue;
                    }
                    COMItem comItem = COMHandle.FindCOM(port);
                    if (comItem.Owner.Length <= 0)
                        continue;

                    if (comItem != null)
                    {
                        try
                        {
                            /*
                            ListViewItem lviewItem = new ListViewItem(new string[] {
                                comItem.Port.ToString(),
                                comItem.Owner,
                                comItem.StrBaud,
                                comItem.StrRemainTime,
                                comItem.Description,
                                comItem.WaitListString,
                                comItem.ProcessInfo});

                            listViewComTable.Items[r] = lviewItem;
                             * */
                            viewRow.SubItems[COL_REMAIN_TIME].Text = comItem.StrRemainTime;
                            viewRow.SubItems[COL_PROCESS_INFO].Text = comItem.ProcessInfo;
                            
                        }
                        catch
                        {
                        }
                    }

#endif
                }

                RefreshCOMInfoWithourClear();
                //listViewComTable.Items.Clear();
                //listViewComTable.Items.AddRange(arrItems);
                System.Threading.Thread.Sleep(10000);
            }
        }

        private void RefreshCOMInfo()
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
                AppConfig.LoadComInfo();

                int selRow = -1;
#if USE_XPTABLE
                if (m_xpTable.SelectedIndicies.Length > 0)
                    selRow = m_xpTableModel.Selections.SelectedItems[0].Index;

                m_xpTableModel.Rows.Clear();
#else
#endif
                SortedList<uint, COMItem> allCOMs = COMHandle.AllCOMs;
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

                if (m_backgroundWorkerInitReady && isNeedStartBackgroundWorker)
                    m_pauseBackgroundWorkFlag = false;

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnChangeRowHeight(int height)
        {
#if USE_XPTABLE
            m_xpTableModel.RowHeight = AppConfig.COMListRowHeight;
#else
            if (listViewComTable.SmallImageList == null)
                listViewComTable.SmallImageList = new ImageList();
            listViewComTable.SmallImageList.ImageSize = new Size(1, AppConfig.COMListRowHeight);
#endif
        }

        private void SetRowStyle(COMItem comItem, Row r)
        {
            Color rowBackColor = AppConfig.ColorComAvaiable;
            if (comItem.CheckIllegal())
            {
                rowBackColor = AppConfig.ColorComIllegal;
            }
            else if (comItem.IsAvaiable())
            {
                rowBackColor = AppConfig.ColorComAvaiable;
            }
            else if (comItem.Owner == AppConfig.LoginUserName)
            {
                rowBackColor = AppConfig.ColorComReservedByMeNotExpired;
            }
            else
            {
                rowBackColor = AppConfig.ColorComReservedByOther;
            }
            r.BackColor = rowBackColor;
        }

#if USE_XPTABLE
        private Row AddTableRow(COMItem comItem)
#else
        private ListViewItem AddTableRow(COMItem comItem)
#endif
        {
            if (comItem == null)
                return null;
#if USE_XPTABLE
            Row row = new Row(new string[] {
                comItem.Port.ToString(),
                comItem.Owner,
                comItem.StrBaud,
                comItem.StrRemainTime,
                comItem.Description,
                comItem.ProcessInfo});
            row.Editable = false;
            SetRowStyle(comItem, row);
            m_xpTableModel.Rows.Add(row);

            return row;
#else
            ListViewItem lviewItem = new ListViewItem(new string[] {
                comItem.Port.ToString(),
                comItem.Owner,
                comItem.StrBaud,
                comItem.StrRemainTime,
                comItem.Description,
                comItem.WaitListString,
                comItem.ProcessInfo});
                //(comItem.ProcessId == 0 ? "" : comItem.ProcessId.ToString()) });

            Color rowBackColor = AppConfig.ColorComAvaiable;
            if (comItem.IsAvaiable())
            {
                rowBackColor = AppConfig.ColorComAvaiable;
            }
            else if (comItem.Owner == AppConfig.LoginUserName)
            {
                rowBackColor = AppConfig.ColorComReservedByMe;
            }
            else
            {
                rowBackColor = AppConfig.ColorComReservedByOther;
            }
            lviewItem.BackColor = rowBackColor;
            listViewComTable.Items.Add(lviewItem);
            return lviewItem;
#endif
        }

        private void SetTableRow(Row row, COMItem comItem)
        {
            if (row == null || comItem == null)
                return;

            row.Cells[COL_PORT].Text = comItem.Port.ToString();
            row.Cells[COL_OWNER].Text = comItem.Owner;
            row.Cells[COL_BAUD].Text = comItem.StrBaud;
            row.Cells[COL_REMAIN_TIME].Text = comItem.StrRemainTime;
            row.Cells[COL_DESCRIPTION].Text = comItem.Description;
            row.Cells[COL_PROCESS_INFO].Text = comItem.ProcessInfo;

            SetRowStyle(comItem, row);
        }

        private void RefreshCOMInfoWithourClear()
        {
            SortedList<uint, COMItem> allComs = COMHandle.AllCOMs;
            int index = 0;
            try
            {
                foreach (Row row in m_xpTableModel.Rows)
                {
                    uint port = uint.Parse(row.Cells[COL_PORT].Text);
                    COMItem item = COMHandle.FindCOM(port);
                    SetTableRow(row, item);
                }
            }
            catch (System.InvalidOperationException err)
            {
            }
        }

        private void MonitorComStatusChange()
        {

        }

        /// <summary>
        /// Tests whether obj is a CellPadding structure with the same values as 
        /// this Padding structure
        /// </summary>
        /// <param name="obj">The Object to test</param>
        /// <returns>This method returns true if obj is a CellPadding structure 
        /// and its Left, Top, Right, and Bottom properties are equal to 
        /// the corresponding properties of this CellPadding structure; 
        /// otherwise, false</returns>
        private void LocateSizeFitByTableSize()
        {
            //Button
            btnSetting.Location = new Point(10, 10);
            btnRefresh.Location = new Point(btnSetting.Size.Width + btnSetting.Location.X + 10,
                                            btnSetting.Location.Y);

            int maxTitleHeight = Math.Max(Math.Max(btnSetting.Height, btnRefresh.Height), groupFilter.Height);

            //Row height of table 
#if USE_XPTABLE
            m_xpTableModel.RowHeight = AppConfig.COMListRowHeight;
#else
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, AppConfig.COMListRowHeight);
            listViewComTable.SmallImageList = imgList;
#endif
            //Table width
            int tableWidth = 0;
            int tableHeight = 0;
            Point tableLocation;

            tableHeight = AppConfig.COMListRowHeight * 21;
            tableLocation = new Point(10, maxTitleHeight + 10);

#if USE_XPTABLE
            for (int i = 0; i < m_xpColumnModel.Columns.Count; i++)
            {
                tableWidth += m_xpColumnModel.Columns[i].Width;
            }
            m_xpTable.Width = tableWidth + 28;
            m_xpTable.Height = tableHeight;
            m_xpTable.Location = tableLocation;
            //listViewComTable.Hide();

#else
            for (int i = 0; i < listViewComTable.Columns.Count; i++)
            {
                tableWidth += listViewComTable.Columns[i].Width;
            }
            listViewComTable.Width = tableWidth + 28;
#endif

            groupCOMDetail.Location = new Point(tableLocation.X + tableWidth + 40, btnSetting.Location.Y + btnSetting.Height + 3);
            groupActionButton.Location = new Point(groupCOMDetail.Location.X, groupCOMDetail.Location.Y + groupCOMDetail.Height + 4);
            groupActionButton.Width = groupCOMDetail.Width;


            //Filter group
            groupFilter.Location = new Point(tableLocation.X + tableWidth - groupFilter.Width + 26,
                                             btnSetting.Location.Y - 8);

            this.Width = groupCOMDetail.Location.X + groupCOMDetail.Width + 25;
            this.Height = tableLocation.Y + tableHeight + 43;
        }

        private void FormMain_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private COMItem GetCOMItem(int rowIndex)
        {
#if USE_XPTABLE
            if (rowIndex < 0 || rowIndex >= m_xpTableModel.Rows.Count)
            {
                return null;
            }

            try
            {
                uint port = uint.Parse(m_xpTableModel.Rows[rowIndex].Cells[COL_PORT].Text);
                return COMHandle.FindCOM(port);
            }
            catch
            {
                return null;
            }
#else
            if (rowIndex < 0 || rowIndex >= listViewComTable.Items.Count)
            {
                return null;
            }

            try
            {
                uint port = uint.Parse(listViewComTable.Items[rowIndex].SubItems[COL_PORT].Text);
                return COMHandle.FindCOM(port);
            }
            catch
            {
                return null;
            }
#endif
        }

        private COMItem GetSelectedCOMItem()
        {
            if (m_xpTable.SelectedIndicies.Length <= 0)
                return null;
            return GetCOMItem(m_xpTable.SelectedIndicies[0]);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            HistoryWritter.Write("close the application.");
            AppConfig.SaveComInfo();
            AppConfig.SaveGlobalConfig();
            AppConfig.SavePersonalConfig();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshCOMInfo();
        }

        public void ReserveCOMHandle(COMItem comItem, bool createInTab)
        {
            throw new NotImplementedException();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            //LocateSizeFit();
        }

        private void tableComList_DoubleClick(object sender, EventArgs e)
        {
            return;
        }

        private void lableLogFilePathHelp_Click(object sender, EventArgs e)
        {
            Label lb = (Label)sender;
            ctxMenuLogHelp.Show(lb, lb.Width, lb.Height);
        }

        private void btnReserve_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            uint port = uint.Parse(cboxCOM.Text);
            AppConfig.LoadComInfo();
            COMItem comItem = COMHandle.FindCOM(port);
            if (comItem == null)
                return;

            this.Enabled = false;

            if (btn.Text == Properties.Resources.strActionReserve)
            {
                COMHandle.Reserve(port, AppConfig.LoginUserName, ParseExpireTime(), tboxDescription.Text);
                btnActionSecureCRT.Enabled = true;
                btnActionSecureCRT.Text = Properties.Resources.strActionSecureCrtOpen;
            }
            else
            {
                COMHandle.Release(port, AppConfig.LoginUserName);
                btnActionSecureCRT.Enabled = false;
            }

            AppConfig.SaveComInfo();
            if (comItem.RowInTable != null)
            {
#if USE_XPTABLE
                comItem.RowInTable.Cells[COL_OWNER].Text = comItem.Owner;
                comItem.RowInTable.Cells[COL_DESCRIPTION].Text = comItem.Description;
                comItem.RowInTable.Cells[COL_BAUD].Text = comItem.StrBaud;
                comItem.RowInTable.Cells[COL_REMAIN_TIME].Text = comItem.StrRemainTime;    
#else

                comItem.RowInTable.SubItems[COL_OWNER].Text = comItem.Owner;
                comItem.RowInTable.SubItems[COL_DESCRIPTION].Text = comItem.Description;
                comItem.RowInTable.SubItems[COL_BAUD].Text = comItem.StrBaud;
                comItem.RowInTable.SubItems[COL_REMAIN_TIME].Text = comItem.StrRemainTime;
#endif
            }
            RefreshCOMInfoWithourClear();

            this.Enabled = true;

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            COMItem item = COMHandle.FindCOM(uint.Parse(cboxCOM.Text));
            if (btn.Text == Properties.Resources.strActionSecureCrtOpen)
            {
                try
                {
                    SecureCRTHandle.Open(cboxSessionName.Text, item, cboxCreateInTab.Checked);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (item.SecureCrtProcess != null)
                    btn.Text = Properties.Resources.strActionSecureCrtKill;
            }
            else
            {
                try
                {
                    try
                    {
                        COMHandle.KillProcess(item);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (item.SecureCrtProcess == null)
                        btn.Text = Properties.Resources.strActionSecureCrtOpen;
                }
                catch
                {
                }
            }

            RefreshCOMInfoWithourClear();
        }

        private void groupCOMDetail_Enter(object sender, EventArgs e)
        {

        }

        /*
        private void btnActionWait_Click(object sender, EventArgs e)
        {
            if (m_currCOMIem == null)
                return;

            if (btnActionWait.Text == Properties.Resources.strActionWaitAdd) //Add wait
            {
                if (m_currCOMIem.ContainsWait(AppConfig.LoginUserName)) //Already in wait-list
                {
                    return;
                }
                m_currCOMIem.AddWait(AppConfig.LoginUserName);
#if USE_XPTABLE
                m_currCOMIem.RowInTable.Cells[COL_WAIT_LIST].Text = m_currCOMIem.WaitListString;
#else
                m_currCOMIem.RowInTable.SubItems[COL_WAIT_LIST].Text = m_currCOMIem.WaitListString;
#endif
                btnActionWait.Text = Properties.Resources.strActionWaitRemove;
            }
            else //Remove wait
            {
                if (!m_currCOMIem.ContainsWait(AppConfig.LoginUserName)) //Not in wait-list
                {
                    return;
                }
                m_currCOMIem.DeleteWait(AppConfig.LoginUserName);
#if USE_XPTABLE
                m_currCOMIem.RowInTable.Cells[COL_WAIT_LIST].Text = m_currCOMIem.WaitListString;
#else
                m_currCOMIem.RowInTable.SubItems[COL_WAIT_LIST].Text = m_currCOMIem.WaitListString;
#endif
                btnActionWait.Text = Properties.Resources.strActionWaitAdd;
            }
        }
        */
        private void rbtnFilter_CheckedChanged(object sender, EventArgs e)
        {
            RefreshCOMInfo();
        }

        private void tableComList_Click(object sender, EventArgs e)
        {
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            groupCOMDetail.Enabled = true;
        }

        private void cboxEnableLogFilePath_CheckedChanged(object sender, EventArgs e)
        {
            tboxLogFilePath.Enabled = cboxEnableLogFilePath.Checked;
        }

        private void cboxEnableLogLineFormat_CheckedChanged(object sender, EventArgs e)
        {
            tboxLogLineFormat.Enabled = cboxEnableLogLineFormat.Checked;
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
            RefreshCOMInfo();

            if (m_backgroundWorkerInitReady)
            {
                m_pauseBackgroundWorkFlag = false;
            }
        }

        private void listViewComTable_MouseClick(object sender, MouseEventArgs e)
        {
          
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
                        ctrl.Enabled = true;
                    }
                }
            }
            else
            {
                if (AppConfig.LoginUserName == item.Owner)
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
                            ctrl.Enabled = (ctrl == cboxExpireTimeValue || ctrl == cboxExpireTimeUnit);
                        }
                    }
                    else
                    {
                        btnActionSecureCRT.Text = Properties.Resources.strActionSecureCrtOpen;
                        foreach (Control ctrl in groupCOMDetail.Controls)
                        {
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
                AppConfig.LoginUserName + "-COM" + item.Port,
                "BMC-COM" + item.Port,
                "BIOS-COM" + item.Port,
                "POST-COM" + item.Port,
                "SSP-COM" + item.Port,
                "COM" + item.Port + "-BMC",
                "COM" + item.Port + "-BIOS",
                "COM" + item.Port + "-POST",
                "COM" + item.Port + "-SSP",
                "COM" + item.Port + "-" + AppConfig.LoginUserName
            });

            cboxSessionName.Text = item.SessionName;
            //dtpExpireTime.Value = DateTime.Now + new TimeSpan(4, 0, 0);

            string[] fileHandles = item.FileHandles;
            if (fileHandles == null || fileHandles.Length <= 0)
            {
                statusLabelOpenedDevices.Text = "none";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < fileHandles.Length; i++)
                {
                    sb.Append(fileHandles[i]);
                    if (i < (fileHandles.Length - 1))
                        sb.Append(", ");
                }
                statusLabelOpenedDevices.Text = sb.ToString();
            }

            RefreshSelectedComInfo();

        }
        private void listViewComTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView lview = (ListView)sender;
            if (lview.SelectedItems.Count <= 0)
            {
                groupActionButton.Enabled = groupCOMDetail.Enabled = true;
                return;
            }

            COMItem item = GetCOMItem(lview.SelectedItems[0].Index);
            if (item == null)
                return;
            OnSelectCom(item);
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
                && item.Owner == AppConfig.LoginUserName)
            {
                return true;
            }
            else if (str == Properties.Resources.strFilterReservedByOthers
                && item.Owner.Length > 0 && item.Owner != AppConfig.LoginUserName)
            {
                return true;
            }
            else if (str == Properties.Resources.strFilterIllegal
                && item.CheckIllegal())
            {
                return true;
            }
            else if (str == Properties.Resources.strFilterRunning
                && item.SecureCrtProcess != null)
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

        private void cboxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxFilter.Text != Properties.Resources.strFilterReservedByMe)
                radioBtnReservedByMe.Checked = false;

            if (cboxFilter.Text != Properties.Resources.strFilterAvaiable)
                radioBtnAvaiable.Checked = false;

            if (cboxFilter.Text != Properties.Resources.strFilterByAll)
                radioBtnAllComs.Checked = false;

            RefreshCOMInfo();
        }

        private void cboxBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void xpTable_CellClick(object sender, XPTable.Events.CellMouseEventArgs e)
        {     
            if (m_xpTable.SelectedIndicies.Length <= 0)
                return;
            int selIndex = m_xpTable.SelectedIndicies[0];
            COMItem comItem = GetCOMItem(e.Row);
            OnSelectCom(comItem);
            CellPos startCellPos = new CellPos(selIndex, 0);
            CellPos endCellPos = new CellPos(selIndex, m_xpColumnModel.Columns.Count - 1);

            m_xpTableModel.Selections.SelectCells(startCellPos, endCellPos);
        }


        private void cboxSessionName_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        private void dtpExpireTime_ValueChanged(object sender, EventArgs e)
        {

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
            for (int i = min; i <= max; i+=step)
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

        private void btnReleaseAll_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Are you sure want to release all the COMs that you reserved?\n", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Enabled = false;
                COMHandle.ReleaseAllReservedByMe();
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            COMItem item = GetSelectedCOMItem();
            if (item == null)
                return;

            string name = null;
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 1)
                name = "Jobs";
            else
                name = "Bill Gates";

            if (comboBox1.SelectedItem.ToString().StartsWith("Reserve"))
            {
                COMHandle.Reserve(item.Port, name, ParseExpireTime(), tboxDescription.Text);
            }
            else
            {
                COMHandle.Release(item.Port, name);
            }

            AppConfig.SaveComInfo();
            if (item.RowInTable != null)
            {
#if USE_XPTABLE
                item.RowInTable.Cells[COL_OWNER].Text = item.Owner;
                item.RowInTable.Cells[COL_DESCRIPTION].Text = item.Description;
                item.RowInTable.Cells[COL_BAUD].Text = item.StrBaud;
                item.RowInTable.Cells[COL_REMAIN_TIME].Text = item.StrRemainTime;
#else

                comItem.RowInTable.SubItems[COL_OWNER].Text = comItem.Owner;
                comItem.RowInTable.SubItems[COL_DESCRIPTION].Text = comItem.Description;
                comItem.RowInTable.SubItems[COL_BAUD].Text = comItem.StrBaud;
                comItem.RowInTable.SubItems[COL_REMAIN_TIME].Text = comItem.StrRemainTime;
#endif
            }
            RefreshCOMInfo();
        }

        private void radioBtnAllComs_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnAllComs.Checked)
            {
                cboxFilter.Text = Properties.Resources.strFilterByAll;
            }
        }
    }
}
