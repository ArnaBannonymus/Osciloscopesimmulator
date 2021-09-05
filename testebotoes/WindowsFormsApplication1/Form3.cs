using Signals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using teste_botoes;
using WindowsFormsApplication1;
#if COM_OFFICE_DLL
using Excel = Microsoft.Office.Interop.Excel;
#endif



namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        private Signals.SignalType tipo;
        double offset, freq, ampl, mult;
        int ch;


        private Form frm1;


        public Form3(Form frm1, int ch, SignalGenerator sig)
        {
            InitializeComponent();
            this.ch = ch;
            this.frm1 = frm1;


            foreach (String item in System.Enum.GetNames(typeof(Signals.SignalType)))
            {
                if (item != System.Enum.GetName(typeof(SignalType), SignalType.ExcelCSV))
                {
                    cmb_stype.Items.Add(item);
                }
            }
            cmb_stype.SelectedIndex = 0;

            foreach (SignalType item in System.Enum.GetValues(typeof(SignalType)))
            {
                if (item==sig.SignalType)
                {
                    cmb_stype.SelectedItem = System.Enum.GetName(typeof(SignalType), item);
                    break;
                }
            }

            if (sig != null)
            {
                knob_offset.SetValue(Convert.ToInt16(sig.Offset));
                knob_ampl.SetValue(Convert.ToInt16(sig.Amplitude));
                knob_frq.SetValue(Convert.ToInt32(sig.Frequency/ sig.FrequencyMult*(double)10));
                radio_x1.Checked = sig.FrequencyMult == (double)1;
                radio_x10.Checked = sig.FrequencyMult == (double)10;
                radio_x100.Checked = sig.FrequencyMult == (double)100;
                radio_x1000.Checked = sig.FrequencyMult == (double)1000;
            }
            else
            {
                knob_offset.SetValue(0);
                knob_ampl.SetValue(1);
                knob_frq.SetValue(2);
                radio_x1.Checked = true;
            }


            update();
        }

        private void radiobutns_CheckedChanged(object sender, EventArgs e)
        {
            update();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }




        private void knob_offset_ValueChanged(object sender, EventArgs e)
        {
            update();
        }

        public void update()
        {

            bool en = cmb_stype.Text == System.Enum.GetName(typeof(SignalType), SignalType.ExcelCSV);

            if (radio_x1.Checked)
            {
                mult = 1;
            }
            else if (radio_x10.Checked)
            {
                mult = 10;
            }
            else if (radio_x100.Checked)
            {
                mult = 100;
            }
            else if (radio_x1000.Checked)
            {
                mult = 1000;
            }


            freq = (Convert.ToDouble(knob_frq.GetValue()) / (double)10) * mult;

            ampl = Convert.ToDouble(knob_ampl.GetValue());

            offset = Convert.ToDouble(knob_offset.GetValue());

            lbl_frq.Text = "" + freq + "Hz";
            lbl_offset.Text = "" + offset + "V";
            lbl_ampl.Text = "" + ampl + "V";

            //knob_ampl.Enabled = en;


        }



        private void knob_ampl_ValueChanged(object sender, EventArgs e)
        {
            update();
        }

        private void knob_frq_ValueChanged(object sender, EventArgs e)
        {
            update();
        }


        private void cmb_stype_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (SignalType item in System.Enum.GetValues(typeof(SignalType)))
            {
                if (cmb_stype.Text == System.Enum.GetName(typeof(SignalType), item))
                {
                    tipo = item;
                    update();
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (tipo == SignalType.DC)
            {
                ((Form1)frm1).criar_sinal(ch, tipo, offset);
            }
            else
            {
                ((Form1)frm1).criar_sinal(ch, tipo, freq, ampl, offset, mult);
            }
            this.Close();
        }

        


    }
}