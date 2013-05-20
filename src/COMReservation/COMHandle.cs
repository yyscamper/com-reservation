using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using XPTable.Models;
using System.Windows.Forms;
using System.Diagnostics;

namespace COMReservation
{
    public enum COMPriority: int
    {
        HIGH    = 0,
        MIDDLE  = 1,
        LOW     = 2
    }

    public class COMItem
    {
        private uint            m_port;
        private string          m_owner = "";
        private COMPriority     m_priority = COMPriority.LOW;
        private string          m_group = "";
        private string          m_description = "";
        private DateTime        m_expireTime;
        private ArrayList       m_waitList = new ArrayList();
        private string          m_sessionName = "";
        private uint            m_baud = 115200;

#if USE_XPTABLE
        private Row             m_rowInTable = null;
#else
        private ListViewItem    m_rowInTable = null;
#endif
        private int m_processId = PROCESS_ID_INVALID;
        private string          m_strBaud = "115200";

        public static readonly  int PROCESS_ID_INVALID = -1;

        private static readonly string[] PriorityStrArr = new string[] {"High", "Middle", "Low"};

        public COMItem()
        {

        }

        public COMItem(uint port)
        {
            m_port = port;
        }

        public COMItem(uint port, string owner, COMPriority priority, string group, string description, DateTime expireTime)
        {
            m_port = port;
            m_owner = owner;
            m_priority = priority;
            m_group = group;
            m_description = description;
            m_expireTime = expireTime;
        }
#if USE_XPTABLE
        public Row RowInTable
#else
        public ListViewItem RowInTable
#endif
        {
            get { return m_rowInTable; }
            set { m_rowInTable = value; }
        }

        public string SessionName
        {
            get { return m_sessionName; }
            set { m_sessionName = value; }
        }

        public uint Baud
        {
            get { return m_baud; }
            set { m_baud = value; m_strBaud = value.ToString(); }
        }

        public string StrBaud
        {
            get { return m_strBaud; }
            set { m_strBaud = value; m_baud = uint.Parse(m_strBaud); }
        }

        public uint Port
        {
            get { return m_port; }
            set { m_port = value; }
        }

        public string Owner
        {
            get { return m_owner; }
            set { m_owner = value; }
        }

        public COMPriority Priority
        {
            get { return m_priority; }
            set { m_priority = value; }
        }

        public string PriorityString
        {
            get { return PriorityStrArr[(int)m_priority]; }
        }

        public string Group
        {
            get { return m_group; }
            set { m_group = value; }
        }

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        public DateTime ExpireTime
        {
            get { return m_expireTime; }
            set { m_expireTime = value; }
        }

        public Process SecureCrtProcess
        {
            get 
            {
                if (m_processId <= PROCESS_ID_INVALID)
                {
                    return null;
                }
                try
                {
                    return Process.GetProcessById(m_processId);
                }
                catch
                {
                    return null;
                }
            }
        }


        public int ProcessId
        {
            get { return m_processId; }
            set { m_processId = value; }
        }

        public int ThreadCount
        {
            get
            {
                
                Process proc = this.SecureCrtProcess;
                return (proc != null ? proc.Threads.Count : 0);
            }
        }

        public bool IsRunning
        {
            get { return (m_processId >= 0); }
        }

        public string ProcessInfo
        {
            get
            {
                Process proc = this.SecureCrtProcess;
                if (proc == null)
                    return "";
                else
                    return proc.Id.ToString() + "(" + proc.Threads.Count.ToString() + ")";
            }
        }

        public TimeSpan RemainTime
        {
            get 
            { 
                if (DateTime.Now >= m_expireTime)
                    return new TimeSpan(0);
                else
                    return (m_expireTime - DateTime.Now);
            }
        }

        public bool IsAvaiable()
        {
            return (m_owner == null || m_owner.Trim().Length == 0);
        }

        public string StrRemainTime
        {
            get 
            { 
                if (DateTime.Now >= m_expireTime)
                {
                    return "00 00:00:00";
                }
                else
                {
                    TimeSpan ts = m_expireTime - DateTime.Now;
                    return String.Format("{0:D2} {1:D2}:{2:D2}:{3:D2}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
                }
            }
        }

        public string WaitListString
        {
            get { return GetWaitListString(); }
            set
            {
                string[] strArr = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in strArr)
                {
                    m_waitList.Add(str);
                }
            }
        }

        public string GetWaitListString()
        {
            if (m_waitList.Count == 0)
                return string.Empty;

            StringBuilder strb = new StringBuilder();
            foreach (string str in m_waitList)
            {
                strb.Append(str);
                strb.Append(',');
            }
            strb.Remove(strb.Length - 1, 1);
            return strb.ToString();
        }

        public bool AddWait(string userName)
        {
            if (m_waitList.Contains(userName) || m_waitList.Count >= 10)
                return false;

            m_waitList.Add(userName);

            return true;
        }

        public bool ContainsWait(string userName)
        {
            return m_waitList.Contains(userName);
        }

        public bool DeleteWait(string userName)
        {
            if (m_waitList.Contains(userName))
                m_waitList.Remove(userName);

            return true;
        }

        public void ClearWait()
        {
            m_waitList.Clear();
        }

        public bool ContainWait(string name)
        {
            return m_waitList.Contains(name);
        }

        public bool CheckIllegal()
        {
            if (this.ThreadCount >= 18)
            {
                return true;
            }
            return false;
        }
    }

    static public class COMHandle
    {
        private static SortedList m_allCOMs = new SortedList();
        private static string m_dataFilePath = "com_reservation_data.xml";
        private static string m_historyFilePath = "com_reservation_history.xml";

        static public SortedList AllCOMs
        {
            get { return m_allCOMs; }
        }

        static public void Clear()
        {
            m_allCOMs.Clear();
        }

        static public void Add(COMItem comItem)
        {
            if (comItem != null && !m_allCOMs.ContainsKey(comItem.Port))
            {
                m_allCOMs.Add(comItem.Port, comItem);
            }
        }

        static public void KillProcess(COMItem item)
        {
            if (item != null)
            {
                Process proc = item.SecureCrtProcess;
                if (proc != null)
                    proc.Kill();
                item.ProcessId = COMItem.PROCESS_ID_INVALID;
            }
        }

        static void LoadData()
        {

        }
        
        static public COMItem FindCOM(uint port)
        {
            if (m_allCOMs.ContainsKey(port))
                return (COMItem)m_allCOMs[port];
            else
                return null;
        }

        static public void Reserve(uint port, string owner, DateTime expireTime, string description)
        {
            if (owner == null || expireTime <= DateTime.Now)
            {
                return;
            }
            COMItem comItem = FindCOM(port);
            if (comItem == null)
                return;

            comItem.Owner = owner;
            comItem.ExpireTime = expireTime;
            comItem.Description = description;
            AddHistory(owner + "successfully reserve the COM" + port);

            AppConfig.SaveComInfo();
        }

        static public void Release(uint port, string owner)
        {
            COMItem comItem = FindCOM(port);
            if (comItem == null)
                return;

            comItem.Owner = "";
            comItem.ExpireTime = DateTime.Now;
            comItem.ProcessId = COMItem.PROCESS_ID_INVALID;
            AppConfig.SaveComInfo();
        }


        static void AddHistory(string message)
        {
            DateTime t = DateTime.Now;
            FileStream historyFile = null;
            try
            {
                historyFile = File.Open(m_historyFilePath, FileMode.Append, FileAccess.Write);
                StringBuilder strb = new StringBuilder();
                strb.Append(t.ToLocalTime());
                strb.Append("  ");
                strb.Append(message);
                strb.Append("\n");

                historyFile.Write(new UTF8Encoding().GetBytes(strb.ToString()), 0, strb.Length);

                historyFile.Close();
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
