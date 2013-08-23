using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;

namespace COMReservation
{
    static public class  DeviceMapTable
    {
        private static Dictionary<string, string> _table;

        static DeviceMapTable()
        {
            _table = new Dictionary<string, string>();
        }

        public static bool IsValid
        {
            get { return (_table != null && _table.Count > 0); }
        }

        public static void Add(string port, string deviceName)
        {
            _table.Add(deviceName, port);
        }

        public static string GetPort(string deviceName)
        {
            return _table[deviceName];
        }

        public static string GetDeviceName(string port)
        {
            foreach (string devName in _table.Keys)
            {
                if (port == _table[devName])
                    return devName;
            }

            return null;
        }

        public static string[] GetOpenedPort(string[] fileHandles)
        {
            if (fileHandles == null || fileHandles.Length <= 0)
                return null;

            ArrayList al = new ArrayList();
            foreach (string handle in fileHandles)
            {
                if (_table.ContainsKey(handle))
                {
                    al.Add(_table[handle]);
                }
            }

            string[] strArr = new string[al.Count];
            al.CopyTo(strArr);
            return strArr;
        }

        public static string[] GetOpenedPort(ArrayList fileHandles)
        {
            if (fileHandles == null || fileHandles.Count <= 0)
                return null;

            ArrayList al = new ArrayList();
            foreach (string handle in fileHandles)
            {
                if (_table.ContainsKey(handle))
                {
                    al.Add(_table[handle]);
                }
            }

            string[] strArr = new string[al.Count];
            al.CopyTo(strArr);
            return strArr;
        }

        public static void Load(string path)
        {
            try
            {
                StreamReader fs = File.OpenText(path);
                _table.Clear();
                fs.ReadLine(); //version number
                while (!fs.EndOfStream)
                {
                    string line = fs.ReadLine();
                    int idx = line.IndexOf(':');
                    if (idx > 0 || idx < (line.Length - 1))
                    {
                        _table.Add(line.Substring(idx + 1), line.Substring(0, idx));
                    }
                }
                fs.Close();
            }
            catch
            {
                _table.Clear();
            }
        }

        public static void BuildTable(SortedList<uint, COMItem> allComs, ref ArrayList errorPortsInfo)
        {
            ArrayList errPorts = new ArrayList();
            ProcessFileHandle proc = new ProcessFileHandle(System.Diagnostics.Process.GetCurrentProcess().Id);
            _table.Clear();
            foreach (COMItem item in allComs.Values)
            {
                uint port = item.Port;
                try
                {
                    SerialPort sp = new SerialPort("COM" + port);
                    sp.Open();
                    ArrayList handles = proc.GetComFileHandle();
                    if (handles != null && handles.Count > 0)
                    {
                        _table.Add(handles[0].ToString(), port.ToString());
                    }
                    sp.Close();
                }
                catch
                {
                    /*
                    if (port >= 29 && port <= 92)
                    {
                        _table.Add("\\Device\\mxuport00" + (port-29).ToString("D2"), port.ToString());
                    }
                    */
                    errPorts.Add(port);
                }
            }
            errorPortsInfo = errPorts;
        }

        public static void BuildTableFile(string path, SortedList<uint, COMItem> allComs, ref ArrayList errorPortsInfo)
        {
            StreamWriter sw = new StreamWriter(path);
            BuildTable(allComs, ref errorPortsInfo);
            sw.WriteLine("version=1.0");
            foreach (string key in _table.Keys)
            {
                sw.WriteLine(_table[key] + ":" + key);
            }
            sw.Flush();
            sw.Close();
        }
    }
}
