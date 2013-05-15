using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace COMReservation
{
    static class AppConfig
    {
        private static uint m_portStart = 1;
        private static uint m_portEnd = 80;
        private static uint m_defaultBaudrate = 115200;
        private static string m_appName = "COM-Reservation";
        private static string m_loginName = "unknown";

        private static string m_longTimeFormat = "yyyy-MM-dd hh:mm:ss";
        private static string m_shortTimeFormat = "MM/dd hh:mm";
        private static string m_appConfigFilePath = "./com_reservation.xml";

        static private string m_secureCrtPath = "C:\\Program Files\\SecureCRT\\SecureCRT.exe";
        static private string m_secureCrtSessionDir = "Config/Session/";

        static AppConfig()
        {
            m_loginName = System.Environment.UserName;
            m_secureCrtSessionDir = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VanDyke\\Config\\Sessions\\";
            
            
            COMHandle.Add(new COMItem(1, "YuanYu", COMPriority.HIGH, "BC_GROUP", "For random", new DateTime(2013, 5, 17, 14, 59, 0)));
            COMHandle.Add(new COMItem(2, "Leo", COMPriority.LOW, "JF_GROUP", "For Airave", new DateTime(2013, 5, 15, 23, 20, 0)));
            COMHandle.Add(new COMItem(3, "Andy", COMPriority.HIGHEST, "DV_GROUP", "For bug#1", new DateTime(2013, 5, 15, 10, 59, 0)));
            COMHandle.Add(new COMItem(4, "YuanYu", COMPriority.LOWEST, "MG_GROUP", "For thing that", new DateTime(2013, 5, 15, 23, 35, 0)));
            COMHandle.Add(new COMItem(5, "Simon", COMPriority.MIDDLE, "EV_GROUP", "For tody's work", new DateTime(2013, 5, 16, 9, 8, 0)));
            COMHandle.Add(new COMItem(6, "YuanYu", COMPriority.HIGH, "SB_GROUP", "For my personal", new DateTime(2013, 5, 20, 10, 01, 0)));
        }

        static public string SecureCRTSessionDir
        {
            get { return m_secureCrtSessionDir; }
            set { m_secureCrtSessionDir = value; }
        }

        static public string SecureCRTExeFilePath
        {
            get { return m_secureCrtPath; }
            set { m_secureCrtPath = value; }
        }

        static public uint StartPort
        {
            get { return m_portStart; }
            set { m_portStart = value; }
        }

        static public uint EndPort
        {
            get { return m_portEnd; }
            set { m_portEnd = value; }
        }

        static public void Load()
        {
            COMHandle.Clear();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_appConfigFilePath);
                XmlNodeList nodeList = xmlDoc.SelectSingleNode(m_appName).ChildNodes;
                foreach (XmlElement elem in nodeList)
                {
                    if (elem.Name == "com_list")
                    {
                        //

                        XmlNodeList comNodeList = elem.ChildNodes;
                        foreach (XmlElement comNode in comNodeList)
                        {
                            string comName = comNode.Name;
                            try
                            {
                                uint port = uint.Parse(comNode.Name.Substring("COM".Length));
                                COMItem comItem = new COMItem();
                                comItem.Port = port;
                                comItem.Owner = comNode.GetAttribute("owner");
                                comItem.Group = comNode.GetAttribute("group");
                                comItem.Priority = (COMPriority)int.Parse(comNode.GetAttribute("priority"));
                                comItem.Description = comNode.GetAttribute("description");
                                comItem.ExpireTime = DateTime.ParseExact(comNode.GetAttribute("expire_time"), m_longTimeFormat, null, System.Globalization.DateTimeStyles.AllowInnerWhite);
                                COMHandle.Add(comItem);
                            }
                            catch
                            {
                                //continue;
                            }
                        }
                       
                        //for (uint i = 1; i < 100; i++)
                        //{
                        //    COMHandle.Add(new COMItem(i));
                        //}

                    }

                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        static public void Save()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement rootNode = xmlDoc.CreateElement(m_appName);
            xmlDoc.AppendChild(rootNode);

            XmlElement node = xmlDoc.CreateElement("app_version");
            node.InnerText = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("modify_time");
            node.InnerText = DateTime.Now.ToString();
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("modify_user");
            node.InnerText = m_loginName;
            rootNode.AppendChild(node);

            XmlElement comNode = xmlDoc.CreateElement("com_list");
            rootNode.AppendChild(comNode);

            Hashtable allComs = COMHandle.AllCOMs;
            foreach (COMItem comItem in allComs.Values)
            {
                node = xmlDoc.CreateElement("COM" + comItem.Port);
                node.SetAttribute("owner", comItem.Owner);
                node.SetAttribute("group", comItem.Group);
                node.SetAttribute("priority", ((int)(comItem.Priority)).ToString());
                node.SetAttribute("expire_time", comItem.ExpireTime.ToString(m_longTimeFormat));
                node.SetAttribute("description", comItem.Description);
                comNode.AppendChild(node);
            }

            try
            {
                xmlDoc.Save(m_appConfigFilePath);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

    }
}
