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
using System.IO.Ports;

namespace COMReservation
{
    public class COMItem
    {
        private string          m_strPort;
        private uint            m_port;
        private string          m_owner = "";
        private string          m_group = "";
        private string          m_description = "";
        private DateTime        m_expireTime;
        private string          m_sessionName = "";
        private string          m_baud = "115200";
        private int             m_index = 0;
        private ProcessFileHandle   m_fileHandle = null;
        private Row             m_rowInTable = null;
        private int             m_processId = PROCESS_ID_INVALID;
        private Process         m_process = null;
        private string[]        m_openedDeviceNames = null;
        private string[]        m_procOpenedPorts = null;
        private string          m_processInfo = string.Empty;
        public static readonly  int PROCESS_ID_INVALID = -1;
        
        public COMItem()
        {

        }

        public COMItem(string port)
        {
            StrPort = port;
        }

        public COMItem(uint port)
        {
            Port = port;
        }

        public COMItem(string port, string owner, string group, string description, DateTime expireTime)
        {
            StrPort = port;
            m_owner = owner;
            m_group = group;
            m_description = description;
            m_expireTime = expireTime;
        }

        public COMItem(uint port, string owner, string group, string description, DateTime expireTime)
        {
            Port = port;
            m_owner = owner;
            m_group = group;
            m_description = description;
            m_expireTime = expireTime;
        }

        public string StrPort
        {
            get { return m_strPort; }
            set 
            {
                try
                {
                    m_port = uint.Parse(m_strPort);
                }
                catch
                {
                    m_strPort = string.Empty;
                    m_port = 0;
                    return;
                }

                m_strPort = value;
            }
        }

        public uint Port
        {
            get { return m_port; }
            set
            {
                m_port = value;
                m_strPort = m_port.ToString();
            }
        }

        public int Index
        {
            get { return m_index; }
            set { m_index = value; }
        }

        public Row RowInTable
        {
            get { return m_rowInTable; }
            set { m_rowInTable = value; }
        }

        public string SessionName
        {
            get { return m_sessionName; }
            set { m_sessionName = value; }
        }

        public string Baud
        {
            get { return m_baud; }
            set { m_baud = value; }
        }

        public string Owner
        {
            get { return m_owner; }
            set { m_owner = value; }
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

        public void Update()
        {
            m_process = RtSecureCrtProcess;
            if (m_process != null)
            {
                try
                {
                    m_openedDeviceNames = RtFileHandles;
                    m_procOpenedPorts = DeviceMapTable.GetOpenedPort(m_openedDeviceNames);
                    bool foundPort = false;
                    foreach (string sport in m_procOpenedPorts)
                    {
                        COMItem item = COMHandle.FindCom(sport);
                        if (item != null)
                        {
                            if (!item.IsRunning)
                                item.RtProcessId = m_processId;
                            if (m_port.ToString() == sport)
                                foundPort = true;
                        }
                    }
                    if (!foundPort)
                    {
                        m_owner = "";
                        m_processId = PROCESS_ID_INVALID;
                        m_expireTime = new DateTime(1, 1, 1, 1, 1, 1);
                    }
                }
                catch
                {
                    m_openedDeviceNames = null;
                    m_procOpenedPorts = null;
                }
            }
            else
            {
                m_openedDeviceNames = null;
                m_procOpenedPorts = null;
            }
        }

        public Process SecureCrtProcess
        {
            get { return m_process; }
        }

        public Process RtSecureCrtProcess //Real Time SecureCRT Process
        {
            get 
            {
                if (m_processId <= PROCESS_ID_INVALID)
                {
                    return null;
                }
                try
                {
                    Process proc = Process.GetProcessById(m_processId);
                    if (proc.ProcessName.Contains("SecureCRT"))
                    {
                        return proc;
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }

        public string[] FileHandles
        {
            get { return m_openedDeviceNames; }
        }

        public string[] ProcessOpenedPorts
        {
            get { return m_procOpenedPorts; }
        }

        public string[] RtFileHandles //Real time file handles
        {
            get
            {
                if (m_fileHandle != null)
                {
                    ArrayList al = m_fileHandle.GetComFileHandle();
                    if (al != null)
                    {
                        string[] arr = new string[al.Count];
                        al.CopyTo(arr);
                        return arr;
                    }
                }
                return null;
            }
        }

        public int RtProcessId //Real time process ID
        {
            get { return m_processId; }
            set 
            {
                m_fileHandle = null;
                int procId = value;
                try
                {
                    if (procId <= PROCESS_ID_INVALID)
                        return;

                    Process proc = Process.GetProcessById(procId);
                    if (proc.ProcessName.Contains("SecureCRT"))
                    {
                        m_processId = value;
                        m_fileHandle = new ProcessFileHandle(m_processId);
                    }
                    else
                    {
                        m_processId = PROCESS_ID_INVALID;
                    }
                }
                catch
                {
                    m_processId = PROCESS_ID_INVALID;
                }
            }
        }

        public bool IsRunning
        {
            get { return m_process != null; }
        }

        public bool RtIsRunning //Real time is Running
        {
            get { return (this.RtSecureCrtProcess != null); }
        }

        public string ProcessInfo
        {
            get
            {
                if (m_process == null)
                {
                    return string.Empty;
                }

                string handleCnt = "?";
                if (m_openedDeviceNames != null)
                {
                    handleCnt = m_openedDeviceNames.Length.ToString();
                }
                return m_process.ProcessName + " " + m_process.Id.ToString() + "(" + handleCnt + ")";
            }
        }

        public string RtProcessInfo //Real time process info
        {
            get
            {
                Process proc = this.RtSecureCrtProcess;
                if (proc == null)
                    return "";
                else
                {
                    string handleCnt = "?";
                    if (m_fileHandle != null)
                    {
                        ArrayList list = m_fileHandle.GetComFileHandle();
                        if (list != null)
                        {
                            handleCnt = list.Count.ToString();
                        }
                    }
                    return proc.ProcessName + " " + proc.Id.ToString() + "(" + handleCnt + ")";
                }
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
                    //return "00 00:00:00";
                    return string.Empty;
                }
                else
                {
                    TimeSpan ts = m_expireTime - DateTime.Now;
                    return String.Format("{0:D2} {1:D2}:{2:D2}:{3:D2}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
                }
            }
        }

        public bool IsExpired
        {
            get { return (m_expireTime <= DateTime.Now); }
        }

        public bool CheckIllegal()
        {
            if (this.IsExpired && this.IsRunning)
                return true;

            if (this.IsRunning && m_procOpenedPorts != null)
            {
                foreach (string port in m_procOpenedPorts)
                {
                    COMItem item = COMHandle.FindCom(port);
                    if (item == null)
                        return true;

                    if (item.Owner != Utility.GetProcessOwner(m_processId))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool RtCheckIllegal() //Real time Check Illegal
        {
            return (this.IsExpired && this.RtIsRunning);
        }
    }

    static public class COMHandle
    {
        private static SortedList<uint, COMItem> m_allCOMs = new SortedList<uint, COMItem>();

        static public SortedList<uint, COMItem> AllCOMs
        {
            get { return m_allCOMs; }
        }

        static public int TotalNumOfPorts
        {
            get { return m_allCOMs.Count; }
        }

        static public int TotalNumberOfMyReserved
        {
            get
            {
                int cnt = 0;
                foreach (COMItem item in m_allCOMs.Values)
                {
                    if (item.Owner == AppConfig.LoginUserFullName)
                    {
                        cnt++;
                    }
                }

                return cnt;
            }
        }

        static public COMItem ItemAt(int index)
        {
            if (index < 0 || index >= m_allCOMs.Count)
            {
                return null;
            }
            return (COMItem)(m_allCOMs[(uint)index]);
        }

        static public void Remove(uint port)
        {
            if (!m_allCOMs.ContainsKey(port))
            {
                return;
            }
            m_allCOMs.Remove(port);
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
                Process proc = item.RtSecureCrtProcess;
                if (proc != null)
                    proc.Kill();
                item.RtProcessId = COMItem.PROCESS_ID_INVALID;
            }
        }

        static void LoadData()
        {

        }
        
        static public COMItem FindCom(uint port)
        {
            if (m_allCOMs.ContainsKey(port))
                return (COMItem)m_allCOMs[port];
            else
                return null;
        }

        static public COMItem FindCom(string portStr)
        {
            uint port;
            try
            {
                port = uint.Parse(portStr);
                return FindCom(port);
            }
            catch
            {
                return null;
            }
        }

        static public bool CheckPortFree(uint port)
        {
            SerialPort serial = new SerialPort("COM" + port);
            try
            {
                serial.Open();
            }
            catch
            {
                return false;
            }
            serial.Close();
            return true;
        }

        static public void Reserve(uint port, string owner, DateTime expireTime, string description)
        {
            if (owner == null || expireTime <= DateTime.Now)
            {
                return;
            }
            COMItem comItem = FindCom(port);
            if (comItem == null)
                return;

            comItem.Owner = owner;
            comItem.ExpireTime = expireTime;
            comItem.Description = description;
            HistoryWritter.Write(" successfully reserve the COM" + port + ".");

            AppConfig.SaveComInfo();
        }

        static public void Release(uint port, string owner)
        {
            COMItem comItem = FindCom(port);
            if (comItem == null)
                return;

            if (comItem.IsRunning)
            {
                throw new Exception("You should kill/close the process that open the COM" + port.ToString() + " before release it!");
            }

            comItem.Owner = "";
            comItem.ExpireTime = DateTime.Now;
            comItem.RtProcessId = COMItem.PROCESS_ID_INVALID;
            HistoryWritter.Write("released the COM" + port + ".");
            AppConfig.SaveComInfo();
        }

        static public void ReleaseAllReservedByMe()
        {
            HistoryWritter.Write("try to release all COMs that reserved by me.");
            foreach (COMItem item in m_allCOMs.Values)
            {
                if (item.Owner == AppConfig.LoginUserFullName)
                    Release(item.Port, item.Owner);
            }
    }

        /*
        static public void Classify()
        {
            foreach (COMItem item in m_allCOMs.Values)
            {
                if (item.IsAvaiable())
                {
                    m_avaiableComs.Add(item.Port, item);
                }
                else
                {
                    if (item.IsRunning)
                        m_runningComs.Add(item.Port, item);

                    if (item.CheckIllegal())
                        m_illegalComs.Add(item.Port, item);

                    if (item.IsExpired)
                        m_expiredComs.Add(item.Port, item);

                    if (item.Owner == AppConfig.LoginUserName)
                        m_reservedByMeComs.Add(item.Port, item);
                    else
                        m_reservedByOtherComs.Add(item.Port, item);
                }
            }
        }
        */
    }
}
