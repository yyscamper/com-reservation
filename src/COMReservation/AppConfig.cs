using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace COMReservation
{
    static class AppConfig
    {
        //Numbers
        private static uint m_portStart = 1;
        private static uint m_portEnd = 80;
        private static uint m_defaultBaudrate = 115200;  
  
        //Dimension
        private static int m_comListRowHeight = 28;

        //File & Folder
        private static string m_secureCRTSessionDir;
        private static string m_secureCRTExeFilePath;
        private static string m_appDir;
        private static string m_comInfoFilePath;
        private static string m_globalConfigFilePath;
        private static string m_personalConfigFilePath;
        private static string m_historyFileName;
        private static string m_historyFolder = "./history/";
        private static string m_logFilePath;
        private static string m_logLineFormat;
        private static long m_historyBackupThreshold = 1024 * 1204;

        //System Environment
        private static string m_appName = "COM-Reservation";
        private static string m_userLoginName = "unknown";
        private static string m_userDomainName = "unknown";
        private static string m_appVersion;

        //Colors
        private static Color m_colorComAvaiable = Color.White;
        private static Color m_colorComReservedByMeNotExpired = Color.LightGreen;
        private static Color m_colorComReservedByOther = Color.SandyBrown;
        private static Color m_colorComReservedByMeAndExpired = Color.Yellow;
        private static Color m_colorComIllegal = Color.Red;

        //Others
        private static string m_longTimeFormat = "yyyy-MM-dd HH:mm:ss";
        private static string m_shortTimeFormat = "MM/dd hh:mm";
        private static string[] m_freqBaudrates = new string[] { "4800", "9600", "19200", "38400", "115200", "380400" };
        private static string[] m_allColorSchemes = new string[] { "Black / Cyan", "BLack / Floral White", "Floral / Dark Cyan", 
                                        "Monochrome", "Traditional", "White / Black", "Windows", "Yellow / Black" };
        private static string m_currentColorScheme = "";

        private static DateTime m_preComInfoFileModifyTime = new DateTime(1, 1, 1, 1, 1, 1);

        private static byte[] _key1 = { 0x34, 0x9B, 0xC0, 0x02, 0x79, 0xE0, 0x99, 0xFF, 0xAB, 0xC0, 0xAB, 0x89, 0x09, 0xCD, 0x98, 0xEF };
        private static string m_strKey = "9q12otjj1q02,;[]ff2fpaj'";

        #region Constructor

        static AppConfig()
        {
            m_userLoginName = System.Environment.UserName;
            m_userDomainName = System.Environment.UserDomainName;

            m_secureCRTSessionDir = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VanDyke\\Config\\Sessions";
            //m_secureCRTExeFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\SecureCRT\\SecureCRT.exe";
            m_secureCRTExeFilePath = "D:\\Program Files\\SecureCRT\\SecureCRT.exe";
            m_appDir = System.Environment.CurrentDirectory;
            m_appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            m_historyFileName = "com_history.log";
            m_globalConfigFilePath = "./com_global_config.dat";
            //m_personalConfigFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\com_reservation\\com_config.xml";
            m_personalConfigFilePath = "./com_config_" + AppConfig.LoginUserName + ".xml";
            
            m_comInfoFilePath = "./all_coms_info.dat";
            m_logFilePath = ".\\" + m_userLoginName + "\\%S-%Y%M%D-%h%m%s.log";
            m_logLineFormat = "[%h%m%s%t] ";

            /*
            COMHandle.Add(new COMItem(1, "YuanYu", COMPriority.HIGH, "BC_GROUP", "For random", new DateTime(2013, 5, 17, 14, 59, 0)));
            COMHandle.Add(new COMItem(2, "Leo", COMPriority.LOW, "JF_GROUP", "For Airave", new DateTime(2013, 5, 15, 23, 20, 0)));
            COMHandle.Add(new COMItem(3, "Andy", COMPriority.HIGHEST, "DV_GROUP", "For bug#1", new DateTime(2013, 5, 15, 10, 59, 0)));
            COMHandle.Add(new COMItem(4, "YuanYu", COMPriority.LOWEST, "MG_GROUP", "For thing that", new DateTime(2013, 5, 15, 23, 35, 0)));
            COMHandle.Add(new COMItem(5, "Simon", COMPriority.MIDDLE, "EV_GROUP", "For tody's work", new DateTime(2013, 5, 16, 9, 8, 0)));
            COMHandle.Add(new COMItem(6, "YuanYu", COMPriority.HIGH, "SB_GROUP", "For my personal", new DateTime(2013, 5, 20, 10, 01, 0)));
            */
        }

        #endregion
 
        #region Attributes

        static public string[] FreqBaudrates
        {
            get { return m_freqBaudrates; }
        }

        static public int COMListRowHeight
        {
            get {return m_comListRowHeight; }
            set { m_comListRowHeight = value; }
        }

        static public string LoginUserName
        {
            get { return m_userLoginName; }
            set { m_userLoginName = value; }
        }

        static public string LoginUserDomain
        {
            get { return m_userDomainName; }
            set { m_userDomainName = value; }
        }

        static public string LoginUserFullName
        {
            get { return m_userDomainName + "\\" + m_userLoginName; }
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

        static public string SecureCRTExeFilePath
        {
            get { return m_secureCRTExeFilePath; }
            set { m_secureCRTExeFilePath = value; }
        }

        static public string SecureCRTSessionDir
        {
            get { return m_secureCRTSessionDir; }
            set { m_secureCRTSessionDir = value; }
        }
        
        static public string LogFilePath
        {
            get { return m_logFilePath; }
            set { m_logFilePath = value; }
        }

        static public string LogLineFormat
        {
            get { return m_logLineFormat; }
            set { m_logLineFormat = value; }
        }


        static public Color ColorComAvaiable
        {
            get { return m_colorComAvaiable; }
            set { m_colorComAvaiable = value; }
        }

        static public Color ColorComReservedByMeNotExpired
        {
            get { return m_colorComReservedByMeNotExpired; }
            set { m_colorComReservedByMeNotExpired = value; }
        }

        static public Color ColorComReservedByMeExpired
        {
            get { return m_colorComReservedByMeAndExpired; }
            set { m_colorComReservedByMeAndExpired = value; }
        }

        static public Color ColorComReservedByOther
        {
            get { return m_colorComReservedByOther; }
            set { m_colorComReservedByOther = value; }
        }

        static public Color ColorComIllegal
        {
            get { return m_colorComIllegal; }
            set { m_colorComIllegal = value; }
        }

        static public uint DefaultBaud
        {
            get { return m_defaultBaudrate; }
            set { m_defaultBaudrate = value; }
        }

        static public string HistoryFilePath
        {
            get { return m_historyFolder + m_historyFileName; }
        }

        static public string HistoryFileName
        {
            get { return m_historyFileName; }
            set { m_historyFileName = value; }
        }

        static public string HistoryFolder
        {
            get { return m_historyFolder; }
            set { m_historyFolder = value; }
        }

        static public long HistoryBackupThreshold
        {
            get { return m_historyBackupThreshold; }
            set { m_historyBackupThreshold = value; }
        }


        static public string[] ColorSchemeNames
        {

            get { return m_allColorSchemes; }
        }

        static public string CurrentColorScheme
        {
            get
            {
                if (!m_allColorSchemes.Contains(m_currentColorScheme))
                    m_currentColorScheme = "Traditional";
                return m_currentColorScheme;
            }
            set
            {
                if (!m_allColorSchemes.Contains(value))
                    return;
                else
                    m_currentColorScheme = value;
            }
        }


        #endregion

        #region function_file_load_save

        static public void LoadComInfo()
        {
            //XmlDocument doc = LoadEncryptedXmlFile("./encrypteddata.dat");

            int comIndex = 0;

            if (m_preComInfoFileModifyTime == File.GetLastWriteTime(m_comInfoFilePath))
                return;

            m_preComInfoFileModifyTime = File.GetLastWriteTime(m_comInfoFilePath);

            try
            {
                //XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(m_comInfoFilePath);
                XmlDocument xmlDoc = LoadEncryptedXmlFile(m_comInfoFilePath);

                XmlNodeList nodeList = xmlDoc.SelectSingleNode(m_appName).ChildNodes;
                foreach (XmlElement elem in nodeList)
                {
                    if (elem.Name == "com_list")
                    {
                        XmlNodeList comNodeList = elem.ChildNodes;
                        COMHandle.Clear();
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
                                comItem.WaitListString = comNode.GetAttribute("wait_list");
                                if (comNode.HasAttribute("expire_time"))
                                    comItem.ExpireTime = DateTime.ParseExact(comNode.GetAttribute("expire_time"),
                                                            m_longTimeFormat, null, System.Globalization.DateTimeStyles.AllowInnerWhite);
                                
                                if (comNode.HasAttribute("session_name"))
                                {
                                    comItem.SessionName = comNode.GetAttribute("session_name");
                                }
                                if (comItem.SessionName.Trim().Length == 0)
                                {
                                    comItem.SessionName = "Serial-COM" + port;
                                }
                                
                                if (comNode.HasAttribute("process_id"))
                                {
                                    comItem.ProcessId = int.Parse(comNode.GetAttribute("process_id"));

                                    if (comItem.ProcessId != COMItem.PROCESS_ID_INVALID)
                                    {
                                        try
                                        {
                                            Process proc = Process.GetProcessById(comItem.ProcessId);
                                        }
                                        catch
                                        {
                                            comItem.ProcessId = COMItem.PROCESS_ID_INVALID;
                                            comItem.ExpireTime = DateTime.Now;
                                        }
                                    }
                                }
                                comItem.Index = comIndex++;
                                COMHandle.Add(comItem);
                            }
                            catch (Exception err)
                            {
                                throw err;
                            }
                        }

                        //for (uint i = 7; i < 100; i++)
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

        static public void SaveComInfo()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement rootNode = xmlDoc.CreateElement(m_appName);
            xmlDoc.AppendChild(rootNode);

            XmlElement node = xmlDoc.CreateElement("app_version");
            node.InnerText = m_appVersion;
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("modify_user_name");
            node.InnerText = m_userLoginName;
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("modify_user_domain");
            node.InnerText = m_userDomainName;
            rootNode.AppendChild(node);

            XmlElement comNode = xmlDoc.CreateElement("com_list");
            rootNode.AppendChild(comNode);

            SortedList<uint, COMItem> allComs = COMHandle.AllCOMs;
            foreach (COMItem comItem in allComs.Values)
            {
                node = xmlDoc.CreateElement("COM" + comItem.Port);
                node.SetAttribute("owner", comItem.Owner);
                node.SetAttribute("session_name", comItem.SessionName);
                node.SetAttribute("group", comItem.Group);
                node.SetAttribute("priority", ((int)(comItem.Priority)).ToString());
                string ttest = comItem.ExpireTime.ToString(m_longTimeFormat);
                node.SetAttribute("expire_time", comItem.ExpireTime.ToString(m_longTimeFormat));
                node.SetAttribute("description", comItem.Description);
                node.SetAttribute("wait_list", comItem.WaitListString);
                node.SetAttribute("process_id", comItem.ProcessId.ToString());
                comNode.AppendChild(node);
            }

            try
            {
                //xmlDoc.Save(m_comInfoFilePath);

                WriteEncryptedXmlFile(xmlDoc, m_comInfoFilePath);
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        static void ChangeComInfo(uint port, string item, object value)
        {

        }

        static private byte[] EncryptData(byte[] plainText, string strKey)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.IV = _key1;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(plainText, 0, plainText.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组  
            cs.Close();
            ms.Close();
            return cipherBytes;
        }

        static private byte[] EncryptData(string plainText, string strKey)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);   
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.IV = _key1;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组  
            cs.Close();
            ms.Close();
            return cipherBytes; 
        }

        static public byte[] DecryptData(byte[] cipherText, string strKey)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.IV = _key1;
            byte[] decryptBytes = new byte[cipherText.Length];
            MemoryStream ms = new MemoryStream(cipherText);
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
            cs.Read(decryptBytes, 0, decryptBytes.Length);
            cs.Close();
            ms.Close();
            return decryptBytes;
        }  

        static private void WriteEncryptedXmlFile(XmlDocument doc, string filePath)
        {
            StringWriter sw = null;
            XmlTextWriter tx = null;
            FileStream fs = null;
            BinaryWriter bw = null;

            try
            {
                sw = new StringWriter();
                tx = new XmlTextWriter(sw);
                doc.WriteTo(tx);
                string strXmlText = sw.ToString();
                byte[] cipherData = EncryptData(strXmlText, m_strKey);
                fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                bw = new BinaryWriter(fs);
                bw.Write(cipherData);
                bw.Flush();

                doc.Save(filePath + ".xml");
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (sw != null)
                    sw.Close();
                if (tx != null)
                    tx.Close();
                if (fs != null)
                    fs.Close();
                if (bw != null)
                    bw.Close();
            }
        }

        static private XmlDocument LoadEncryptedXmlFile(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] data = new byte[fs.Length];
            if (data.Length != br.Read(data, 0, data.Length))
            {
                throw new XmlException("Don't read enough data for decrypt!");
            }

            byte[] deData = DecryptData(data, m_strKey);
            string xmlStr = Encoding.UTF8.GetString(deData);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);

            br.Close();
            fs.Close();
            return xmlDoc;
        }

        static public void LoadPersonalConfig()
        {
           
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(m_personalConfigFilePath);
                XmlNodeList nodeList = xmlDoc.SelectSingleNode(m_appName).ChildNodes;
                foreach (XmlElement elem in nodeList)
                {
                    if (elem.Name == "default_baud_rate")
                    {
                        m_defaultBaudrate = uint.Parse(elem.InnerText);
                    }
                    else if (elem.Name == "com_table_row_height")
                    {
                        m_comListRowHeight = int.Parse(elem.InnerText);
                    }
                    else if (elem.Name == "securecrt_session_dir")
                    {
                        m_secureCRTSessionDir = elem.InnerText;
                    }
                    else if (elem.Name == "log_file_path")
                    {
                        m_logFilePath = elem.InnerText;
                    }
                    else if (elem.Name == "log_line_format")
                    {
                        m_logLineFormat = elem.InnerText;
                    }
                    else if (elem.Name == "current_color_scheme")
                    {
                        m_currentColorScheme = elem.InnerText;
                    }
                    else if (elem.Name == "color_com_avaiable")
                    {
                        m_colorComAvaiable = ColorTranslator.FromHtml(elem.InnerText);
                    }
                    else if (elem.Name == "color_com_reserved_by_me")
                    {
                        m_colorComReservedByMeNotExpired = ColorTranslator.FromHtml(elem.InnerText);
                    }
                    else if (elem.Name == "color_com_reserved_by_others")
                    {
                        m_colorComReservedByOther = ColorTranslator.FromHtml(elem.InnerText);
                    }
                    else if (elem.Name == "color_com_illegal")
                    {
                        m_colorComIllegal = ColorTranslator.FromHtml(elem.InnerText);
                    }


                }
            }
            catch (Exception err)
            {
                //throw err;
            }
        }

        static public void SavePersonalConfig()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement rootNode = xmlDoc.CreateElement(m_appName);
            xmlDoc.AppendChild(rootNode);

            XmlElement node = xmlDoc.CreateElement("app_version");
            node.InnerText = m_appVersion;
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("default_baud_rate");
            node.InnerText = m_defaultBaudrate.ToString();
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("com_table_row_height");
            node.InnerText = m_comListRowHeight.ToString();
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("securecrt_session_dir");
            node.InnerText = m_secureCRTSessionDir;
            rootNode.AppendChild(node);
            
            node = xmlDoc.CreateElement("log_file_path");
            node.InnerText = m_logFilePath;
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("log_line_format");
            node.InnerText = m_logLineFormat;
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("current_color_scheme");
            node.InnerText = m_currentColorScheme;
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("color_com_avaiable");
            node.InnerText = ColorTranslator.ToHtml(m_colorComAvaiable);
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("color_com_reserved_by_me");
            node.InnerText = ColorTranslator.ToHtml(m_colorComReservedByMeNotExpired);
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("color_com_reserved_by_others");
            node.InnerText = ColorTranslator.ToHtml(m_colorComReservedByOther);
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("color_com_illegal");
            node.InnerText = ColorTranslator.ToHtml(m_colorComIllegal);
            rootNode.AppendChild(node);

            try
            {
                xmlDoc.Save(m_personalConfigFilePath);
            }
            catch (Exception err)
            {
                throw err;
            }
            
        }

        static public void LoadGlobalConfig()
        {
            try
            {
                XmlDocument xmlDoc = LoadEncryptedXmlFile(m_globalConfigFilePath);
                XmlNodeList nodeList = xmlDoc.SelectSingleNode(m_appName).ChildNodes;
                foreach (XmlElement elem in nodeList)
                {
                    if (elem.Name == "securecrt_exe_file")
                    {
                        m_secureCRTExeFilePath = elem.InnerText;
                    }
                    else if (elem.Name == "history_folder")
                    {
                        m_historyFolder = elem.InnerText;
                    }
                    else if (elem.Name == "history_file_name")
                    {
                        m_historyFileName = elem.InnerText;
                    }
                }
            }
            catch (Exception err)
            {
                //throw err;
            }
        }

        static public void SaveGlobalConfig()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement rootNode = xmlDoc.CreateElement(m_appName);
            xmlDoc.AppendChild(rootNode);

            XmlElement node = xmlDoc.CreateElement("app_version");
            node.InnerText = m_appVersion;
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("securecrt_exe_file");
            node.InnerText = m_secureCRTExeFilePath;
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("history_folder");
            node.InnerText = m_historyFolder;
            rootNode.AppendChild(node);

            node = xmlDoc.CreateElement("history_file_name");
            node.InnerText = m_historyFileName;
            rootNode.AppendChild(node);

            try
            {
                //xmlDoc.Save(m_globalConfigFilePath);
                WriteEncryptedXmlFile(xmlDoc, m_globalConfigFilePath);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        static public void ChangeGlobalConfig(string item_name, object value)
        {

        }

        #endregion

    }
}
