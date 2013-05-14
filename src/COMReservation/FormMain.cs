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
    public partial class FormMain : Form
    {
        private delegate void ThreadUpdateRemainTime();

        private string m_loginName = "";

        private string STR_RESERVE = "Reserve";
        private string STR_RELEASE = "Release";
        private Color COLOR_RESERVE = Color.Green;
        private Color COLOR_RELEASE = Color.Red;

        private int COL_SELECT = 0;
        private int COL_PORT = 1;
        private int COL_OWNER = 2;
        private int COL_REMAIN_TIME = 3;
        private int COL_PRIORITY = 4;
        private int COL_GROUP = 5;
        private int COL_DESCRIPTION = 6;
        private int COL_ACTION = 7;

        private XPTable.Models.ColumnModel m_columnModel;
        private XPTable.Models.TableModel m_tableModel;
        public FormMain()
        {
            InitializeComponent();

            m_loginName = System.Environment.UserName;

            m_columnModel = new XPTable.Models.ColumnModel();
            m_tableModel = new XPTable.Models.TableModel();

            m_columnModel.Columns.Add(new CheckBoxColumn(" "));
            m_columnModel.Columns.Add(new TextColumn("Port"));
            m_columnModel.Columns.Add(new TextColumn("Owner"));
            m_columnModel.Columns.Add(new TextColumn("Remain Time"));
            m_columnModel.Columns.Add(new TextColumn("Priority"));
            m_columnModel.Columns.Add(new TextColumn("Group"));
            m_columnModel.Columns.Add(new TextColumn("Description"));
            m_columnModel.Columns.Add(new ButtonColumn("Action"));

            tableComList.ColumnModel = m_columnModel;
            tableComList.TableModel = m_tableModel;

            tableComList.ColumnModel.Columns[COL_SELECT].Width = 28;
            tableComList.ColumnModel.Columns[COL_PORT].Width = 40;
            tableComList.ColumnModel.Columns[COL_OWNER].Width = 80;
            tableComList.ColumnModel.Columns[COL_REMAIN_TIME].Width = 90;
            tableComList.ColumnModel.Columns[COL_PRIORITY].Width = 60;
            tableComList.ColumnModel.Columns[COL_GROUP].Width = 120;
            tableComList.ColumnModel.Columns[COL_DESCRIPTION].Width = 240;
            tableComList.ColumnModel.Columns[COL_ACTION].Width = 80;

            m_tableModel.RowHeight = 28;

            int tableWidth = 0;
            for (int i = 0; i < tableComList.ColumnModel.Columns.Count; i++)
            {
                tableWidth += tableComList.ColumnModel.Columns[i].Width;
            }

            tableComList.Width = tableWidth + 4;

            this.groupBox1.Location = new Point(tableComList.Width + tableComList.Location.X + 10,
                        tableComList.Location.Y - 5);

            this.Width = tableComList.Width + 10 + this.groupBox1.Width + 34;
            this.Height = tableComList.Height + tableComList.Location.Y + 52;


            RefreshCOMInfo();
        }

        private void BackgroudUpdateRemainTime()
        {
            while (true)
            {
                for (int r = 0; r < tableComList.TableModel.Rows.Count; r++)
                {
                    uint port = uint.Parse(tableComList.TableModel.Rows[r].Cells[COL_PORT].Text);
                    COMItem item = COMHandle.FindCOM(port);
                    if (item != null)
                    {
                        tableComList.TableModel.Rows[r].Cells[COL_REMAIN_TIME].Text = item.StrRemainTime;
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void RefreshCOMInfo()
        {
            try
            {
                AppConfig.Load();
                Hashtable allCOMs = COMHandle.AllCOMs;
                tableComList.TableModel.Rows.Clear();
                foreach (COMItem comItem in allCOMs.Values)
                {
                    AddTableRow(comItem, false);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatActionCell( int rowIndex, bool canReserve)
        {
            Cell cel = tableComList.TableModel.Rows[rowIndex].Cells[COL_ACTION];

            if (canReserve)
            {
                cel.Text = STR_RESERVE;
                cel.BackColor = Color.White;
                cel.ForeColor = COLOR_RESERVE;
            }
            else
            {
                cel.Text = STR_RELEASE;
                cel.BackColor = Color.White;
                cel.ForeColor = COLOR_RELEASE;
            }
        }

        private void AddTableRow(COMItem comItem, bool isSelected = false)
        {
            AddTableRow(isSelected, comItem.Port, comItem.Owner, comItem.StrRemainTime, comItem.PriorityString, comItem.Group, comItem.Description);
        }


        private void AddTableRow(bool isSelected = false, uint port = 1, string owner = "", string remainTime = "", string priority = "", string group = "", string description = "")
        {
            bool isAvaiable = string.IsNullOrEmpty(owner);
            bool isReserve = (isAvaiable || !m_loginName.Equals(owner, StringComparison.InvariantCultureIgnoreCase));

            Row row = new Row();
            row.Cells.Add(new Cell("", isSelected));
            row.Cells.Add(new Cell(port.ToString()));
            row.Cells.Add(new Cell(owner));
            row.Cells.Add(new Cell(remainTime));
            row.Cells.Add(new Cell(priority));
            row.Cells.Add(new Cell(group));
            row.Cells.Add(new Cell(description));
            row.Cells.Add(new Cell());

            row.Cells[1].Editable = false;
            row.Cells[2].Editable = false; 
            row.Cells[3].Editable = false;
            row.Cells[4].Editable = false;
            row.Cells[5].Editable = false;

            if (String.IsNullOrEmpty(owner) || m_loginName.Equals(owner, StringComparison.InvariantCultureIgnoreCase))
            {
                row.Enabled = true;
            }
            else
            {
                row.Enabled = false;
            }
            tableComList.TableModel.Rows.Add(row);

            FormatActionCell(tableComList.TableModel.Rows.Count - 1, isReserve);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = System.Environment.UserDomainName + "\\" + System.Environment.UserName;
            RefreshCOMInfo();

            ThreadUpdateRemainTime func = new ThreadUpdateRemainTime(BackgroudUpdateRemainTime);
            func.BeginInvoke(null, null);
        }

        private void FormMain_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void ActionOnCom(int rowIndex)
        {
            if (rowIndex >= tableComList.TableModel.Rows.Count)
                return;

            Row  r = tableComList.TableModel.Rows[rowIndex];
            uint port = uint.Parse(r.Cells[COL_PORT].Text);
            bool isToReserve = (r.Cells[COL_ACTION].Text == STR_RESERVE);
            COMItem comItem = COMHandle.FindCOM(port);
            if (comItem == null)
                return;

            if (isToReserve)
            {
                new FormReserveParam().Show();
                r.Cells[COL_OWNER].Text = m_loginName;
                comItem.Owner = m_loginName;
                FormatActionCell(rowIndex, false);
            }
            else
            {
                if (DialogResult.Yes == MessageBox.Show("Are you sure want to release COM"  + port + " ?", "Confirm", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    r.Cells[COL_OWNER].Text = "";
                    comItem.Owner = "";
                    FormatActionCell(rowIndex, true);
                }
            }

            
        }

        private void tableComList_CellClick(object sender, XPTable.Events.CellMouseEventArgs e)
        {
            if (COL_ACTION == e.Column)
            {
                ActionOnCom(e.Row);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppConfig.Save();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshCOMInfo();
        }
    }
}
