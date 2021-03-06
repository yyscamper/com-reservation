﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.IO.Ports;

namespace COMReservation
{
    public static class SecureCRTHandle
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSessionName"></param>
        /// <param name="com"></param>
        /// <param name="createInTab"></param>
        /// <returns> The process ID</returns>
        public static int Open(string strSessionName, COMItem com, bool createInTab)
        {
            if (com == null)
                return COMItem.PROCESS_ID_INVALID;

            try
            {
                SerialPort serial = new SerialPort("COM" + com.Port);
                if (serial.IsOpen)
                {
                    throw new InvalidOperationException("Access is denied! The COM" + com.Port + " has been opened in other application.");
                }
            }
            catch (Exception err)
            {
                throw err;
            }

            if (!File.Exists(AppConfig.SecureCRTExeFilePath))
            {
                throw new Exception("Cannot find the SecureCRT.exe."); 
            }
            string cmdArg = ( createInTab ? "/T " : "" ) + " /S " + strSessionName;

            if (!File.Exists(AppConfig.SecureCRTSessionDir + "\\" + strSessionName + ".ini"))
            {
                try
                {
                    CreateSessionFile(strSessionName, com);
                }
                catch (Exception err)
                {
                    throw err;
                }
            }

            Process process = new Process();
            process.StartInfo.FileName = AppConfig.SecureCRTExeFilePath;
            process.StartInfo.Arguments = cmdArg;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            com.RtProcessId = process.Id;
            AppConfig.SaveComInfo();
            return process.Id;
        }

        static private void CreateSessionFile(string strSession, COMItem com)
        {
            string filePath = AppConfig.SecureCRTSessionDir + "\\" + strSession + ".ini";
            if (File.Exists(filePath))
            {
                return;
            }

            StringBuilder strb = new StringBuilder();
            strb.Append("D:\"Is Session\"=00000001\n");
            strb.Append("S:\"Protocol Name\"=Serial\n");
            strb.Append("D:\"Baud Rate\"=" + string.Format("{0:X8}", com.Baud) + "\n");
            strb.Append("D:\"Parity\"=00000000\n");
            strb.Append("D:\"Stop Bits\"=00000000\n");
            strb.Append("D:\"Data Bits\"=00000008\n");
            strb.Append("D:\"DSR Flow\"=00000000\n");
            strb.Append("D:\"DTR Flow Control\"=00000001\n");
            strb.Append("D:\"CTS Flow\"=00000000\n");
            strb.Append("D:\"RTS Flow Control\"=00000001\n");
            strb.Append("D:\"XON Flow\"=00000000\n");
            strb.Append("S:\"Com Port\"=COM" + com.Port.ToString() + "\n");
            strb.Append("D:\"Serial Break Length\"=00000064\n");
            strb.Append("D:\"Serial Driver Bug Mask\"=00000000\n");
            if (AppConfig.LogFilePath.Trim() != null)
            {
                strb.Append("S:\"Log Filename\"=" + AppConfig.LogFilePath + "\n");
            }

            if (AppConfig.LogLineFormat.Trim() != null)
            {
                strb.Append("S:\"Custom Log Message Each Line\"=" + AppConfig.LogLineFormat + "\n");
            }

            strb.Append("S:\"Color Scheme\"=" + AppConfig.CurrentColorScheme + "\n");

            try
            {
                FileStream file = File.Create(filePath);
                Byte[] data = new UTF8Encoding().GetBytes(strb.ToString());
                file.Write(data, 0, data.Length);
                file.Close();
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
