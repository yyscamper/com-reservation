using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace COMReservation
{
    public class ProcessFileHandle
    {
        int _procId = -1;

        public ProcessFileHandle(int procId)
        {
            _procId = procId;
        }

        public ArrayList GetComFileHandle()
        {
            ArrayList handleList = new ArrayList();

            Process cmdProc = new Process();
            cmdProc.StartInfo.FileName = "cmd.exe";
            cmdProc.StartInfo.UseShellExecute = false;
            cmdProc.StartInfo.RedirectStandardInput = true;
            cmdProc.StartInfo.RedirectStandardOutput = true;
            cmdProc.StartInfo.RedirectStandardError = true;
            cmdProc.StartInfo.CreateNoWindow = true;
            cmdProc.Start();
            cmdProc.StandardInput.AutoFlush = true;

            cmdProc.StandardInput.WriteLine("handle -a -p " + _procId);
            cmdProc.StandardInput.WriteLine("exit");
            string strCmd = cmdProc.StandardOutput.ReadToEnd();
            cmdProc.WaitForExit();
            cmdProc.Close();

            string[] cmdResultArr = strCmd.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in cmdResultArr)
            {
                string[] itemArr = str.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (itemArr.Length > 2)
                {

                    string handleName = itemArr[itemArr.Length - 1];
                    if (handleName.StartsWith("\\Device\\", StringComparison.InvariantCultureIgnoreCase)
                        && (handleName.IndexOf("serial", StringComparison.InvariantCultureIgnoreCase) > 0
                        || handleName.IndexOf("mxuport", StringComparison.InvariantCultureIgnoreCase) > 0))
                    {
                        handleList.Add(handleName);
                    }
                }
            }

            return handleList;
        }
    }
}
