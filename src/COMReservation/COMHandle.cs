using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace COMReservation
{
    public enum COMPriority: int
    {
        HIGHEST = 0,
        HIGH    = 1,
        MIDDLE  = 2,
        LOW     = 3,
        LOWEST  = 4
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
        private uint m_baud;

        private static readonly string[] PriorityStrArr = new string[] { "Highest", "High", "Middle", "Low", "Lowest" };

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

        public string SessionName
        {
            get { return m_sessionName; }
            set { m_sessionName = value; }
        }

        public uint Baud
        {
            get { return m_baud; }
            set { m_baud = value; }
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

        public string StrRemainTime
        {
            get 
            { 
                if (DateTime.Now >= m_expireTime)
                {
                    return "0 00:00:00";
                }
                else
                {
                    TimeSpan ts = m_expireTime - DateTime.Now;
                    return String.Format("{0:D2} {1:D2}:{2:D2}:{3:D2}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
                }
            }
        }

        public ArrayList WaitList
        {
            get { return m_waitList; }
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
    }

    static public class COMHandle
    {
        private static Hashtable m_allCOMs = new Hashtable();
        private static string m_dataFilePath = "com_reservation_data.xml";
        private static string m_historyFilePath = "com_reservation_history.xml";

        static public Hashtable AllCOMs
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
            if (owner == null || expireTime >= DateTime.Now)
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
        }

        static public void Release(uint port, string owner)
        {
            COMItem comItem = FindCOM(port);
            if (comItem == null)
                return;

            comItem.Owner = "";
            comItem.ExpireTime = DateTime.Now;
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
