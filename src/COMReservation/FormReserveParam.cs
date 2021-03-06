﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace COMReservation
{
    public partial class FormReserveParam : Form
    {
        public FormReserveParam()
        {
            InitializeComponent();
        }

        public FormReserveParam(uint port)
        {
            InitializeComponent();
            cboxCOM.Text = port.ToString();
            cboxCOM.Enabled = false;
        }

        public FormReserveParam(string strPort)
        {
            InitializeComponent();
            cboxCOM.Text = strPort;
            cboxCOM.Enabled = false;
        }

        public FormReserveParam(COMItem item)
        {
            InitializeComponent();
            cboxCOM.Text = item.Port.ToString();
            cboxBaud.Text = item.Baud.ToString();
            cboxSessionName.Text = "Serial-COM" + cboxCOM.Text;
            cboxCreateInTab.Checked = false;
            tboxDescription.Text = "";
            cboxCOM.Enabled = false;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FormReserveParam_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                cboxCOM.Items.Add(i.ToString());
            }
            cboxSessionName.Items.Add("BMC");
            cboxSessionName.Items.Add("BIOS");
            cboxSessionName.Items.Add("POST");

            string[] arrStrBaud = new string[] { "9600", "115200"};
            cboxBaud.Items.AddRange(arrStrBaud);
            cboxBaud.Text = "115200";

            dtpExpireTime.Value = DateTime.Now + new TimeSpan(4, 0, 0);

            rboxPriorityLow.Checked = true;
        }

        private void cboxCOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboxSessionName.Text = "Serial-COM" + cboxCOM.Text;
            cboxSessionName.Items.Clear();
            cboxSessionName.Items.Add("COM" + cboxCOM.Text + "-BMC");
            cboxSessionName.Items.Add("COM" + cboxCOM.Text + "-BIOS");
            cboxSessionName.Items.Add("COM" + cboxCOM.Text + "-POST");
            cboxSessionName.Items.Add("COM" + cboxCOM.Text + "-SSP");
            cboxSessionName.Items.Add("COM" + cboxCOM.Text + "-Diag");
            cboxSessionName.Items.Add("COM" + cboxCOM.Text + "-OS");
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
