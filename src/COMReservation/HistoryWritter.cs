using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace COMReservation
{
    public static class HistoryWritter
    {
        public static void Write(string message)
        {
            DateTime t = DateTime.Now;
            FileStream historyFile = null;
            try
            {
                if (!Directory.Exists(AppConfig.HistoryFolder))
                {
                    Directory.CreateDirectory(AppConfig.HistoryFolder);
                }
                
                if (File.Exists(AppConfig.HistoryFilePath))
                {
                    historyFile = File.OpenRead(AppConfig.HistoryFilePath);
                    if (historyFile.Length >= AppConfig.HistoryBackupThreshold)
                    {
                        string fBackupPath = AppConfig.HistoryFolder + "com_history_backup_" + DateTime.Now.ToFileTime() + ".log";
                        FileStream fbackup = File.Open(fBackupPath, FileMode.CreateNew, FileAccess.Write);
                        historyFile.CopyTo(fbackup);
                        fbackup.Close();
                        historyFile.Close();
                        historyFile = File.OpenWrite(AppConfig.HistoryFilePath);
                        historyFile.SetLength(0);
                    }
                    historyFile.Close();
                }
                StringBuilder strb = new StringBuilder();
                strb.Append(t.ToLocalTime());
                strb.Append("  ");
                strb.Append(AppConfig.LoginUserFullName);
                strb.Append(" ");
                strb.Append(message);
                strb.Append("\n");

                File.AppendAllText(AppConfig.HistoryFilePath, strb.ToString(), new UTF8Encoding());
            }
            catch (Exception err)
            {
                MessageBox.Show("Log history failed, error:"  + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
