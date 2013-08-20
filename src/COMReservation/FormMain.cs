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
    public partial class FormMain : Form, IReserveCOM, ISettingHandle
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
        
        //XPTable components
        private TableModel  m_xpTableModel;
        private ColumnModel m_xpColumnModel;
        private XPTable.Models.Table m_xpTable;

        //Other
        private int m_lastFilterIndex = -100; //Last selected filter index
        private ArrayList m_portList = new ArrayList();

        #endregion

        #region construcator_load

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
        }

        #endregion


        #region Background_Worker

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

                    comItem.Update(); //Update COM info

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

        #region XpTable_Callback

        #endregion


        #region Com_function

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
                else
                {
                    RefreshCOMInfoWithourClear();
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
            SortedList<Row, COMItem> filterComs = new SortedList<Row, COMItem>();

            try
            {
                foreach (Row row in m_xpTableModel.Rows)
                {
                    uint port = uint.Parse(row.Cells[COL_PORT].Text);
                    COMItem item = COMHandle.FindCom(port);
                    if (item != null)
                    {
                        filterComs.Add(row, item);
                        item.Update();
                    }
                }

                foreach (Row row in filterComs.Keys)
                {
                    SetTableRow(row, filterComs[row]);
                }
                /*
                foreach (Row row in m_xpTableModel.Rows)
                {
                    uint port = uint.Parse(row.Cells[COL_PORT].Text);
                    COMItem item = COMHandle.FindCOM(port);
                    SetTableRow(row, item);
                }
                */
            }
            catch
            {
            }
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
                return COMHandle.FindCom(port);
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
            ReloadComInfo();
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
            COMItem comItem = COMHandle.FindCom(port);
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
            COMItem item = COMHandle.FindCom(uint.Parse(cboxCOM.Text));
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
                if (item.RtSecureCrtProcess != null)
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
                    if (item.RtSecureCrtProcess == null)
                        btn.Text = Properties.Resources.strActionSecureCrtOpen;
                }
                catch
                {
                }
            }

            RefreshCOMInfoWithourClear();
        }

        private void rbtnFilter_CheckedChanged(object sender, EventArgs e)
        {
            ReloadComInfo();
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
            ReloadComInfo();

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
                if (!item.RtIsRunning)
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

                    if (item.RtIsRunning)
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

            string[] fileHandles = item.RtFileHandles;
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

        private void cboxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_xpTable.Enabled = false;

            if (cboxFilter.Text != Properties.Resources.strFilterReservedByMe)
                radioBtnReservedByMe.Checked = false;

            if (cboxFilter.Text != Properties.Resources.strFilterAvaiable)
                radioBtnAvaiable.Checked = false;

            if (cboxFilter.Text != Properties.Resources.strFilterByAll)
                radioBtnAllComs.Checked = false;

            ReloadComInfo();

            m_xpTable.Enabled = true;
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
            ReloadComInfo();
        }

        private void radioBtnAllComs_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnAllComs.Checked)
            {
                cboxFilter.Text = Properties.Resources.strFilterByAll;
            }
        }

        private void cboxFilter_TextUpdate(object sender, EventArgs e)
        {
            cboxFilter_SelectedIndexChanged(cboxFilter, e);
        }
    }
}
