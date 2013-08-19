using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMReservation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
       
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                AppConfig.LoadGlobalConfig();
                AppConfig.LoadPersonalConfig();
                AppConfig.LoadComInfo();

                HistoryWritter.Write("open the application.");
            }
            catch (Exception err)
            {
            //    if (DialogResult.No == MessageBox.Show("Found error:" + System.Environment.NewLine
            //                    + err.Message + System.Environment.NewLine + "It may because you are the first time to open this application."
            //         + System.Environment.NewLine + "Are you sure want to continue?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            //    {
            //        return;
            //    }
            }

            Application.Run(new FormMain());
        }
    }
}
