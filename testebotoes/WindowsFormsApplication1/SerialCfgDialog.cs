using Signals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using teste_botoes;



namespace WindowsFormsApplication1
{
    public partial class SerialCfgDialog : Form
    {
        private Form frm1;



        public SerialCfgDialog(Form frm1, String port, int baudrate)
        {
            InitializeComponent();
            this.frm1 = frm1;
            foreach (String s in System.IO.Ports.SerialPort.GetPortNames())
            {
                txtPort.Items.Add(s);
            }

            txtPort.SelectedItem = port;
            cmbbaudrate.SelectedItem = "" + baudrate;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String port = txtPort.Text;
            if (port != "")
            {
                int baudrate = Convert.ToInt32(cmbbaudrate.Text);
                ((Form1)frm1).set_serial_cfg(port, baudrate);
            }
            this.Close();
        }

        private void remote_control_CheckedChanged(object sender, EventArgs e)
        {
            ((Form1)frm1).Flag_remote = remote_control.Checked;
        }
    }
}
