using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XPTable.Models;

namespace COMReservation
{
    public partial class FormMain : Form, IReserveCOM, ISettingHandle
    {
        private delegate void ThreadUpdateRemainTime();

        //Column Index
        private int COL_PORT = 0;
        private int COL_OWNER = 1;
        private int COL_REMAIN_TIME = 2;
        private int COL_DESCRIPTION = 3;
        private int COL_WAIT_LIST = 4;

        private COMItem m_currCOMIem = null;

        public FormMain()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;

            this.Text = "COM-Reversation (" + AppConfig.LoginUserName + ")";

            cboxEnableLogFilePath.Checked = true;
            tboxLogFilePath.Text = AppConfig.LogFilePath;

            cboxEnableLogLineFormat.Checked = false;
            tboxLogLineFormat.Text = AppConfig.LogLineFormat;
            tboxLogLineFormat.Enabled = false;
            cboxActionScripts.Items.Add("Browser Script...");

            listViewComTable.Columns.Add("Port", 40);
            listViewComTable.Columns.Add("Owner", 80);
            listViewComTable.Columns.Add("Remain Time", 90);
            listViewComTable.Columns.Add("Description", 240);
            listViewComTable.Columns.Add("Wait List", 120);

            groupActionButton.Enabled = groupCOMDetail.Enabled = false;
            rbtnAll.Checked = true;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            LocateSizeFitByTableSize();

            cboxBaud.Items.AddRange(AppConfig.FreqBaudrates);

            foreach (COMItem item in COMHandle.AllCOMs.Values)
            {
                cboxCOM.Items.Add(item.Port.ToString());
            }

            RefreshCOMInfo();

            this.Text = System.Environment.UserDomainName + "\\" + System.Environment.UserName;
            RefreshCOMInfo();

            //object[] plist = { this, System.EventArgs.Empty };
            //BackgroudUpdateRemainTime.listViewComTable.BeginInvoke(new System.EventHandler(BackgroudUpdateRemainTime), plist);
            //ThreadUpdateRemainTime func = new ThreadUpdateRemainTime(BackgroudUpdateRemainTime);
            //func.BeginInvoke(null, null);
            //listViewComTable.Invoke(func, null);

            MethodInvoker mi = new MethodInvoker(BackgroudUpdateRemainTime);
            mi.BeginInvoke(null, null);
        }

        private void BackgroudUpdateRemainTime()
        {
            while (true)
            {
                for (int r = 0; r < listViewComTable.Items.Count; r++)
                {
                    uint port = uint.Parse(listViewComTable.Items[r].SubItems[COL_PORT].Text);
                    COMItem item = COMHandle.FindCOM(port);
                    if (item != null)
                    {
                        listViewComTable.Items[r].SubItems[COL_REMAIN_TIME].Text = item.StrRemainTime;
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void RefreshCOMInfo()
        {
            try
            {
                //AppConfig.LoadComInfo();
                SortedList allCOMs = COMHandle.AllCOMs;
                listViewComTable.Items.Clear();
                foreach (COMItem comItem in allCOMs.Values)
                {
                    if (rbtnAll.Checked
                        || (rbtnAvaiable.Checked && comItem.IsAvaiable())
                        || (rbtnReservedByMe.Checked && comItem.Owner == AppConfig.LoginUserName))
                    {
                        comItem.RowInTable = AddTableRow(comItem);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnChangeRowHeight(int height)
        {
            if (listViewComTable.SmallImageList == null)
                listViewComTable.SmallImageList = new ImageList();
            listViewComTable.SmallImageList.ImageSize = new Size(1, AppConfig.COMListRowHeight);
        }

        private ListViewItem AddTableRow(COMItem comItem)
        {
            if (comItem == null)
                return null;

            ListViewItem lviewItem = new ListViewItem(new string[] {
                comItem.Port.ToString(),
                comItem.Owner,
                comItem.StrRemainTime,
                comItem.Description,
                comItem.WaitListString});

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
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, AppConfig.COMListRowHeight);
            listViewComTable.SmallImageList = imgList;

            //Table width
            int tableWidth = 0;
            for (int i = 0; i < listViewComTable.Columns.Count; i++)
            {
                tableWidth += listViewComTable.Columns[i].Width;
            }
            listViewComTable.Width = tableWidth + 28;

            //Table height
            listViewComTable.Height = AppConfig.COMListRowHeight * 21;

            listViewComTable.Location = new Point(10, maxTitleHeight + 10);

            groupCOMDetail.Location = new Point(listViewComTable.Location.X + listViewComTable.Width + 10, btnSetting.Location.Y + btnSetting.Height + 13);
            groupActionButton.Location = new Point(groupCOMDetail.Location.X, groupCOMDetail.Location.Y + groupCOMDetail.Height + 4);
            groupActionButton.Width = groupCOMDetail.Width;
            btnEdit.Location = new Point(groupCOMDetail.Location.X + groupCOMDetail.Width - btnEdit.Width,
                                         groupCOMDetail.Location.Y - btnEdit.Height);


            //Filter group
            groupFilter.Location = new Point(listViewComTable.Location.X + listViewComTable.Width - groupFilter.Width,
                                             btnSetting.Location.Y - 3);

            this.Width = groupCOMDetail.Location.X + groupCOMDetail.Width + 25;
            this.Height = listViewComTable.Location.Y + listViewComTable.Height + 43;
        }

        private void FormMain_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private COMItem GetCOMItem(int rowIndex)
        {
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
        }

        private void tableComList_CellClick(object sender, XPTable.Events.CellMouseEventArgs e)
        {
            COMItem item = GetCOMItem(e.Row);
            if (item == null)
                return;

            if (e.Row < 0 || e.Row >= listViewComTable.Items.Count)
            {
                groupActionButton.Enabled = groupCOMDetail.Enabled = true;
                return;
            }

            m_currCOMIem = item;

            groupActionButton.Enabled = groupCOMDetail.Enabled = true;

            cboxCOM.Text = item.Port.ToString();
            cboxBaud.Text = item.Baud.ToString();
            cboxSessionName.Text = "Serial-COM" + item.Port;
            dtpExpireTime.Value = DateTime.Now + new TimeSpan(4, 0, 0);

            if (AppConfig.LoginUserName == item.Owner)
            {
                btnReserve.Text = "-Release";
                btnReserve.Enabled = true;
                btnOpen.Enabled = true;
                btnAddWait.Enabled = false;
                groupCOMDetail.Enabled = false;
                btnEdit.Enabled = true;
            }
            else
            {
                btnReserve.Text = "+Reserve";
                btnReserve.Enabled = true;
                btnOpen.Enabled = false;
                btnAddWait.Enabled = true;
                groupCOMDetail.Enabled = true;
                btnEdit.Enabled = false;
            }



            /*
            if (COL_ACTION == e.Column)
            {
                ActionOnCom(e.Row);
            }
            else if (COL_OPEN_SECURECRT == e.Column)
            {
                uint port = uint.Parse(tableComList.TableModel.Rows[e.Row].Cells[COL_PORT].Text);
                string sessionName = "Serial-COM" + tableComList.TableModel.Rows[e.Row].Cells[COL_PORT].Text;
                bool   createdInTab = false;
                SecureCRTHandle.Open(sessionName, COMHandle.FindCOM(port), createdInTab);
            }
             * */
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
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
            COMItem comItem = COMHandle.FindCOM(port);
            if (comItem == null)
                return;
            if (btn.Text.StartsWith("+"))
            {
                COMHandle.Reserve(port, AppConfig.LoginUserName, dtpExpireTime.Value, tboxDescription.Text);
                /*if (comItem.RowInTable != null)
                {
                    comItem.RowInTable.Cells[COL_OWNER].Text = comItem.Owner;
                    comItem.RowInTable.Cells[COL_DESCRIPTION].Text = comItem.Description;
                }*/
            }
            else
            {
                COMHandle.Release(port, AppConfig.LoginUserName);
                /*if (comItem.RowInTable != null)
                {
                    comItem.RowInTable.Cells[COL_OWNER].Text = "";
                    comItem.RowInTable.Cells[COL_REMAIN_TIME].Text = "";
                }*/
            }
            AppConfig.SaveComInfo();

            RefreshCOMInfo();


        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            COMItem item = COMHandle.FindCOM(uint.Parse(cboxCOM.Text));
            SecureCRTHandle.Open(cboxSessionName.Text, item, cboxCreateInTab.Checked);
        }

        private void groupCOMDetail_Enter(object sender, EventArgs e)
        {

        }

        private void btnAddWait_Click(object sender, EventArgs e)
        {
            if (m_currCOMIem == null)
                return;

            if (m_currCOMIem.ContainsWait(AppConfig.LoginUserName))
            {
                return;
            }

            m_currCOMIem.AddWait(AppConfig.LoginUserName);
            m_currCOMIem.RowInTable.SubItems[COL_WAIT_LIST].Text = m_currCOMIem.WaitListString;
        }

        private void btnDeleteWait_Click(object sender, EventArgs e)
        {
            if (m_currCOMIem == null)
                return;

            if (!m_currCOMIem.ContainsWait(AppConfig.LoginUserName))
            {
                return;
            }

            m_currCOMIem.DeleteWait(AppConfig.LoginUserName);
            m_currCOMIem.RowInTable.SubItems[COL_WAIT_LIST].Text = m_currCOMIem.WaitListString;
        }

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
            new FormSetting(this).ShowDialog(this);
            RefreshCOMInfo();

        }

        private void listViewComTable_MouseClick(object sender, MouseEventArgs e)
        {
          
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

            m_currCOMIem = item;
            groupActionButton.Enabled = groupCOMDetail.Enabled = true;

            cboxCOM.Text = item.Port.ToString();
            cboxBaud.Text = item.Baud.ToString();
            cboxSessionName.Text = "Serial-COM" + item.Port;
            dtpExpireTime.Value = DateTime.Now + new TimeSpan(4, 0, 0);

            if (AppConfig.LoginUserName == item.Owner)
            {
                btnReserve.Text = "-Release";
                btnReserve.Enabled = true;
                btnOpen.Enabled = true;
                btnAddWait.Enabled = false;
                groupCOMDetail.Enabled = false;
                btnEdit.Enabled = true;
            }
            else
            {
                btnReserve.Text = "+Reserve";
                btnReserve.Enabled = true;
                btnOpen.Enabled = false;
                btnAddWait.Enabled = true;
                groupCOMDetail.Enabled = true;
                btnEdit.Enabled = false;
            }


        }


    }
}
