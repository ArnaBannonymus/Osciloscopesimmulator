using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;
using Signals;
using System.Threading;
using System.Drawing.Drawing2D;
using teste_botoes;
using WindowsFormsApplication1.OpenFiles;
using System.IO.Ports;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Timers;



namespace teste_botoes
{
    public partial class Form1 : Form
    {

        string base_path = "pics\\";

        public bool Flag_remote = false;

        Bitmap scrBitmap, trcBitmap;

        public SerialConfig sconfig;

        int cursor1_y = 56;
        int cursor2_y = 168; //16
        int cursor1_x = 90;
        int cursor2_x = 210;

        info_botao pos_ch1;
        info_botao pos_ch2;
        info_botao onoff_ch1;
        info_botao power;
        info_botao onoff_ch2;
        info_botao pos_horiz;
        info_botao trigger;
        info_botao voltdiv_ch1;
        info_botao voltdiv_ch2;
        info_botao timedivis;
        info_botao menu_trigger;
        info_botao measure;
        info_botao cursor;
        info_botao cursor_variable;
        info_botao F1;
        info_botao F1_math;
        info_botao F1_measure;
        info_botao F1_cursor;
        info_botao F2;
        info_botao F2_math;
        info_botao F2_measure;
        info_botao F2_cursor;
        info_botao F2_trigger;
        info_botao F3;
        info_botao F3_measure;
        info_botao F3_cursor;
        info_botao F3_trigger;
        info_botao F4;
        info_botao F5;
        info_botao math;
        info_botao sinal_CH1;
        info_botao sinal_CH2;
        info_botao sinal_2V_CH1;
        info_botao sinal_2V_CH2;
        info_botao USB_ecra;

        private int resXquad = 300;
        private int resYquad = 224; //225
        private int numquadX = 10;
        private int numquadY = 8;
        private int amostras_ecran = 300;    //250

        private DoubleBufferedPanel panel1;
        private System.Windows.Forms.Panel panel2;


        double Vmaximo, Vminimo, Vpp, Vrms, Vavg, T;

        SignalGenerator sig, sig2;
        private bool stop;
        public bool[] final_aquisicao = new bool[] { true, true };

        //Form2 frm2;

        public Form1()
        {
            InitializeComponent();

            this.panel1 = new DoubleBufferedPanel();
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(80, 75);   // 68, 68
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(302, 228);   //resX + 2 * x, resY + 2 * y    380, 270
            this.panel1.TabIndex = 19;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.DoubleBuffered = true;


            this.Controls.Add(this.panel1);

            this.panel2 = new System.Windows.Forms.Panel();
            this.panel2.BackColor = System.Drawing.Color.MidnightBlue;
            this.panel2.Location = new System.Drawing.Point(69, 66);
            this.panel2.Name = "panel1";
            this.panel2.Size = new System.Drawing.Size(382, 275);
            this.panel2.TabIndex = 19;
            //this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);

            System.Windows.Forms.Timer tmr = null;
            tmr = new System.Windows.Forms.Timer();
            tmr.Interval = 1000;
            tmr.Tick += new EventHandler(timer1_Tick);
            tmr.Enabled = true;

            this.Controls.Add(this.panel2);

            USB.BackColor = Color.MidnightBlue;

            Datetime.BackColor = Color.MidnightBlue;

            //ch1_screen.Text = "CH1 ~ ";
            ch1_screen.BackColor = Color.MidnightBlue;
            ch1_screen.ForeColor = System.Drawing.Color.Orange;

            //ch2_screen.Text = "CH2";
            ch2_screen.BackColor = Color.MidnightBlue;
            ch2_screen.ForeColor = Color.Cyan;

            label4_math.Text = "2V";
            label4_math.TextAlign = ContentAlignment.MiddleCenter;

            main_screen.Text = "MAIN";
            main_screen.BackColor = Color.Red;
            main_screen.ForeColor = Color.White;

            time_screen.Text = "M";
            time_screen.BackColor = Color.Red;
            time_screen.ForeColor = Color.White;

            Trigger_screen.Text = "T";
            Trigger_screen.BackColor = Color.Red;
            Trigger_screen.ForeColor = Color.White;

            Edge_screen.BackColor = Color.MidnightBlue;
            Edge_screen.ForeColor = Color.White;

            divis_screen.BackColor = Color.MidnightBlue;
            divis_screen.ForeColor = Color.White;

            freq_screen.BackColor = Color.MidnightBlue;
            freq_screen.ForeColor = System.Drawing.Color.White;

            freq_screen2.BackColor = Color.MidnightBlue;
            freq_screen2.ForeColor = System.Drawing.Color.White;

            inicio_label(Label_On, 40, 40);

            inicio_label(AutoSet, 55, 25);
        
            inicio_label(USB, 25, 10);
            USB_ecra = new info_botao(base_path, new String[] { "USB.png" });
            updatelabel(USB, USB_ecra.imagem_actual());

            inicio_label(ponta_prova_CH1, 30, 45);
            sinal_2V_CH1 = new info_botao(base_path, new String[] { null, "ponta_prova.PNG" }, new object[] { false, true });
            updatelabel(ponta_prova_CH1, sinal_2V_CH1.imagem_actual());

            inicio_label(ponta_prova_CH2, 30, 45);
            sinal_2V_CH2 = new info_botao(base_path, new String[] { null, "ponta_prova_2.PNG" }, new object[] { false, true });
            updatelabel(ponta_prova_CH2, sinal_2V_CH2.imagem_actual());

            inicio_label(CH1_source, 49, 75);
            sinal_CH1 = new info_botao(base_path, new String[] { null, "cable1.PNG" }, new object[] { false, true });
            updatelabel(CH1_source, sinal_CH1.imagem_actual());


            inicio_label(CH2_source, 55, 75);
            sinal_CH2 = new info_botao(base_path, new String[] { null, "cable2.PNG" }, new object[] { false, true });
            updatelabel(CH2_source, sinal_CH2.imagem_actual());

            pos_ch1 = new info_botao(this, new Point(582, 194), 17, (float)Math.PI / 12.0f);
            pos_ch1.setarray(Convert.ToDouble(resYquad), 0, 41);
            pos_ch1.setvalue(pos_ch1.getnumvals() / 2);

            inicio_label(PowerOn, 25, 25);
            power = new info_botao(base_path, new String[] { null, "on_led.PNG" }, new object[] { false, true });


            pos_ch2 = new info_botao(this, new Point(709, 194), 17, (float)Math.PI / 12.0f);
            pos_ch2.setarray(Convert.ToDouble(resYquad), 0, 41);
            pos_ch2.setvalue(pos_ch2.getnumvals() / 2);

            cursor_variable = new info_botao(this, new Point(966, 56), 28, (float)Math.PI / 12.0f, new object[] { 1, 0, -1 }, false);
            //cursor_variable.setarray(0, resXquad - 1, resXquad);
            cursor_variable.setvalue(cursor_variable.getnumvals() / 2);

            inicio_label(CH1, 40, 30);
            onoff_ch1 = new info_botao(base_path, new String[] { null, "CH1.PNG" }, new object[] { false, true });
            updatelabel(CH1, onoff_ch1.imagem_actual());


            inicio_label(CH2, 40, 30);
            onoff_ch2 = new info_botao(base_path, new String[] { null, "CH2.PNG" }, new object[] { false, true });
            updatelabel(CH2, onoff_ch2.imagem_actual());


            voltdiv_ch1 = new info_botao(this, new Point(585, 303), 28, new Object[] { (double)0.002, (double)0.005, (double)0.01, (double)0.02, (double)0.05, (double)0.1, (double)0.2, (double)0.5, (double)1, (double)2, (double)5 }, ":CHANnel1:SCALe ?");
            voltdiv_ch1.setvalue(voltdiv_ch1.getnumvals());


            voltdiv_ch2 = new info_botao(this, new Point(710, 303), 28, new Object[] { (double)0.002, (double)0.005, (double)0.01, (double)0.02, (double)0.05, (double)0.1, (double)0.2, (double)0.5, (double)1, (double)2, (double)5 }, ":CHANnel2:SCALe ?");
            voltdiv_ch2.setvalue(voltdiv_ch2.getnumvals());

            timedivis = new info_botao(this, new Point(837, 303), 28, new Object[] { (double)0.000000001, (double)0.0000000025, (double)0.000000005, (double)0.000000010, (double)0.000000025, (double)0.000000050, (double)0.000000100, (double)0.000000250, (double)0.000000500, (double)0.000001, (double)0.0000025, (double)0.000005, (double)0.00001, (double)0.000025, (double)0.00005, (double)0.0001, (double)0.00025, (double)0.0005, (double)0.001, (double)0.0025, (double)0.005, (double)0.01, (double)0.025, (double)0.05, (double)0.1, (double)0.25, (double)0.5, (double)1, (double)2.5, (double)5, (double)10 }, ":TIMebase:SCALe ?");
            timedivis.setvalue(timedivis.getnumvals());
            //{ (double)1E-9, (double)0.000005, (double)0.00001, (double)0.000025, (double)0.00005, (double)0.0001, (double)0.00025, (double)0.0005 });

            pos_horiz = new info_botao(this, new Point(837, 194), 16, (float)Math.PI / 12.0f);
            pos_horiz.setarray(0, resXquad - 3, resXquad - 1);
            //pos_horiz.setvalue(pos_ch1.getnumvals() / 2);
            pos_horiz.Circular = false;
            pos_horiz.setvalue(0);

            trigger = new info_botao(this, new Point(964, 194), 16, (float)Math.PI / 12.0f);
            trigger.setarray((double)-0.5, (double)0.5, 41);
            trigger.Circular = false;
            trigger.setvalue((trigger.getnumvals() / 2));


            inicio_label(label11, 40, 25);
            menu_trigger = new info_botao(base_path, new String[] { null, "menu_trigger.png" }, new object[] { false, true });
            updatelabel(label11, menu_trigger.imagem_actual());

            inicio_label(label12, 55, 25);
            measure = new info_botao(base_path, new String[] { null, "measure.png" }, new object[] { false, true });
            updatelabel(label12, measure.imagem_actual());

            inicio_label(label13, 55, 25);
            cursor = new info_botao(base_path, new String[] { null, "Cursor.png" }, new object[] { false, true });
            updatelabel(label13, cursor.imagem_actual());

            inicio_label(label14, 40, 25);
            F1 = new info_botao(base_path, new String[] { null, null, null }, new object[] { 0, 1, 2 }, 1);
            //F1 = new info_botao(base_path, new String[] { null, "F1.png" }, new object[] { false, true });
            //updatelabel(label14, F1.imagem_actual());

            inicio_label(F1_click_measure, 40, 25);
            F1_measure = new info_botao(base_path, new String[] { null, null }, new object[] { 1, 2 }, 1);

            inicio_label(F1_click_math, 40, 25);
            F1_math = new info_botao(base_path, new String[] { null, null }, new object[] { 1, 2 }, 1);

            inicio_label(F1_click_cursor, 40, 25);
            F1_cursor = new info_botao(base_path, new String[] { null, null }, new object[] { 1, 2 }, 1);

            inicio_label(label15, 40, 30);
            F2 = new info_botao(base_path, new String[] { null, null }, new object[] { 1, -1 }, 1);
            //F2 = new info_botao(base_path, new String[] { null, "F2.png" }, new object[] { false, true });
            //updatelabel(label15, F2.imagem_actual());

            inicio_label(F2_click_measure, 40, 30);
            F2_measure = new info_botao(base_path, new String[] { null, null, null, null }, new object[] { 1, 2, 3, 4, 5 }, 1);

            F2_math = new info_botao(base_path, new String[] { null, null }, new object[] { 1, 2 }, 1);

            inicio_label(F2_click_cursor, 40, 30);
            F2_cursor = new info_botao(base_path, new String[] { null, null, null, null }, new object[] { 0, 1, 2, 3 }, 1);

            inicio_label(F2_click_trigger, 40, 30);
            F2_trigger = new info_botao(base_path, new String[] { null, null, null, null }, new object[] { 1, 2 }, 1);

            inicio_label(label16, 40, 30);
            F3 = new info_botao(base_path, new String[] { null, null, null }, new object[] { 0, 1, 2 }, 1);
            //F3 = new info_botao(base_path, new String[] { null, "F3.png" }, new object[] { false, true });
            //updatelabel(label16, F3.imagem_actual());

            inicio_label(F3_click_measure, 40, 30);
            F3_measure = new info_botao(base_path, new String[] { null, null }, new object[] { 1, 2 }, 1);

            inicio_label(F3_click_cursor, 40, 30);
            F3_cursor = new info_botao(base_path, new String[] { null, null, null, null }, new object[] { 0, 1, 2, 3 }, 1);

            inicio_label(F3_click_trigger, 40, 30);
            F3_trigger = new info_botao(base_path, new String[] { null, null, null, null }, new object[] { 1, -1 }, 1);

            inicio_label(label17, 40, 30);
            F4 = new info_botao(base_path, new String[] { null, null }, new object[] { 0, 1 });
            //F4 = new info_botao(base_path, new String[] { null, "F4.png" }, new object[] { false, true });
            //updatelabel(label17, F4.imagem_actual());

            inicio_label(label18, 40, 30);
            F5 = new info_botao(base_path, new String[] { null, null }, new object[] { 0, 1 });
            //F5 = new info_botao(base_path, new String[] { null, "F4.png" }, new object[] { false, true });
            //updatelabel(label18, F5.imagem_actual());

            inicio_label(label19, 39, 23);
            math = new info_botao(base_path, new String[] { null, "Math.png" }, new object[] { false, true });
            updatelabel(label19, math.imagem_actual());

            criar_sinal(1, SignalType.DC, 0);
            criar_sinal(2, SignalType.DC, 0);


            //cursor_variable_anterior = Convert.ToInt32(cursor_variable.val_actual());

            panel2.Visible = false;


            if (System.IO.Ports.SerialPort.GetPortNames().Length == 1)
            {
                set_serial_cfg(System.IO.Ports.SerialPort.GetPortNames()[0], 9600);
            }
            else
            {
                set_serial_cfg("", 9600);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime datetime = DateTime.Now;
            string format = "dd-MMM yy HH:mm";
            this.Datetime.Text = datetime.ToString(format);
        }

        private void measurements(SignalGenerator sinal)
        {
            double pprova = Math.Pow((double)10, -Convert.ToDouble(F3.val_actual()));

            if (sinal != null)
            {

                Vmaximo = 0;
                Vminimo = 0;
                Vrms = 0;

                for (int n = 0; n < amostras_ecran; n++)
                {
                    double y = sinal.amostra(Convert.ToDouble(n) * calcTPonto()) * pprova;

                    if (y > Vmaximo)
                    {
                        Vmaximo = y;

                    }
                    if (y < Vminimo)
                    {
                        Vminimo = y;
                    }

                    Vrms += Math.Pow(y, (double)2) * calcTPonto();
                }

                Vrms /= (double)timedivis.val_actual() * Convert.ToDouble(numquadX);

                Vrms = Math.Sqrt(Vrms);

                Vpp = Vmaximo - Vminimo;
            }

        }

        private double vmedio(SignalGenerator sinal, double pprova, double tdiv)
        {

            return sinal.vmedio * pprova;

        }

        public void criar_sinal(int ch, SignalType t, double f, double a, double o, double fm)
        {
            criar_sinal(ch, new SignalGenerator(t, f, a, o, Convert.ToDouble(timedivis.val_actual()) / ((double)resXquad / (double)numquadX), fm));
        }

        public void criar_sinal(int ch, SignalType t, double o)
        {
            criar_sinal(ch, new SignalGenerator(t, o));
        }

        private void criar_sinal(int ch, SignalGenerator s)
        {
            switch (ch)
            {
                case 1:
                    sig = s;
                    break;
                case 2:
                    sig2 = s;
                    break;
            }
            panel_invalidate();
        }

        public void criar_sinal(int ch, SignalType t, double[] d, double ta)
        {

            criar_sinal(ch, new SignalGenerator(t, d, ta));
        }

        public void criar_sinal(int ch, SignalType t, double[] d, double ta, double freq, double ampl)
        {

            criar_sinal(ch, new SignalGenerator(t, d, ta, freq, ampl));
            //freq_screen.Text = (double)1/ d[d.Length - 1] + "Hz";
        }


        private void inicio_label(Label label, int width, int height)
        {
            label.Text = "";
            label.AutoSize = false;
            label.Width = width;
            label.Height = height;
        }

        private void altera_menu_default(bool estado)
        {
            menu_default1.Visible = estado;
            menu_default2.Visible = estado;
            menu_default3.Visible = estado;
            menu_default4.Visible = estado;
            menu_lbl5.Visible = estado;
            menu_lbl6.Visible = estado;
            label1_default.Visible = estado;
            label2_default.Visible = estado;
            label3_default.Visible = estado;
            label14.Visible = estado;
            label15.Visible = estado;
            label16.Visible = estado;
        }

        private void altera_menu_measure(bool estado)
        {
            menu_measure1.Visible = estado;
            menu_measure2.Visible = estado;
            menu_measure3.Visible = estado;
            menu_measure4.Visible = estado;
            label1_measure.Visible = estado;
            label2_measure.Visible = estado;
            label3_measure.Visible = estado;
            label4_measure.Visible = estado;
            label5_measure.Visible = estado;
            F1_click_measure.Visible = estado;
            F2_click_measure.Visible = estado;
            F3_click_measure.Visible = estado;
        }

        private void altera_menu_math(bool estado)
        {
            menu_math1.Visible = estado;
            label1_math.Visible = estado;
            label2_math.Visible = estado;
            label3_math.Visible = estado;
            label4_math.Visible = estado;
            label5_math.Visible = estado;
            F1_click_math.Visible = estado;
        }

        private void altera_menu_cursor(bool estado)
        {
            menu_cursor1.Visible = estado;
            menu_cursor2.Visible = estado;
            menu_cursor3.Visible = estado;
            menu_cursor4.Visible = estado;
            label1_cursor.Visible = estado;
            label2_cursor.Visible = estado;
            label3_cursor.Visible = estado;
            label4_cursor.Visible = estado;
            label5_cursor.Visible = estado;
            F1_click_cursor.Visible = estado;
            F2_click_cursor.Visible = estado;
            F3_click_cursor.Visible = estado;
        }

        private void altera_menu_trigger(bool estado)
        {
            menu_trigger1.Visible = estado;
            menu_trigger2.Visible = estado;
            menu_trigger3.Visible = estado;
            label1_trigger.Visible = estado;
            label2_trigger.Visible = estado;
            label3_trigger.Visible = estado;
            label4_trigger.Visible = estado;
            label5_trigger.Visible = estado;
            F2_click_trigger.Visible = estado;
            F3_click_trigger.Visible = estado;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                onoff_ch1.click(sender, e);
                updatelabel(CH1, onoff_ch1.imagem_actual());
                menu_default1.Text = "CH 1";
                serialcmd((bool)onoff_ch1.val_actual() ? ":channel1:display 1" : ":channel1:display 0");
            }
            panel_invalidate();

        }


        private void label5_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                onoff_ch2.click(sender, e);
                updatelabel(CH2, onoff_ch2.imagem_actual());
                menu_default1.Text = "CH 2";
                serialcmd((bool)onoff_ch2.val_actual() ? ":channel2:display 1" : ":channel2:display 0");
            }
            panel_invalidate();
        }

        private void serialcmd(string cmd)
        {
            if (Flag_remote)
            {
                new SerialOsc(sconfig, cmd);
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                menu_trigger.click(sender, e);
                updatelabel(label11, menu_trigger.imagem_actual());
                bool b = (bool)menu_trigger.val_actual();
                altera_menus(false, b, false, false);
            }
            panel_invalidate();
        }

        private void AutoSet_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                serialcmd(":AUToset");

                double T = 0;

                if ((bool)onoff_ch1.val_actual() && (bool)onoff_ch2.val_actual())
                {
                    if (sig.SignalType != SignalType.DC && sig2.SignalType != SignalType.DC)
                    {
                        T = Math.Max(1 / sig.Frequency, 1 / sig2.Frequency);
                    }
                    else if (sig.SignalType != SignalType.DC)
                    {
                        T = 1 / sig.Frequency;
                    }
                    else if (sig2.SignalType != SignalType.DC)
                    {
                        T = 1 / sig2.Frequency;
                    }
                }
                else if ((bool)onoff_ch1.val_actual())
                {
                    if (sig.SignalType != SignalType.DC)
                    {
                        T = 1 / sig.Frequency;
                    }
                }
                else if ((bool)onoff_ch2.val_actual())
                {
                    if (sig2.SignalType != SignalType.DC)
                    {
                        T = 1 / sig2.Frequency;
                    }
                }


                int ind = timedivis.Indice;
                int n = 0;
                if (T > 0)
                {
                    timedivis.setvalue(0);
                    while (numquadX * Convert.ToDouble(timedivis.val_actual()) < (double)2 * T && n < timedivis.getnumvals())
                    {
                        timedivis.altera_indice(-1);
                        panel_invalidate();
                        n++;
                    }
                }
                if (T == 0 || n == timedivis.getnumvals())
                {
                    Console.WriteLine("Autoset nao conseguiu alterar tdiv");
                    timedivis.setvalue(ind);
                }

                n = 0;
                ind = voltdiv_ch1.Indice;
                voltdiv_ch1.setvalue(0);

                if ((bool)onoff_ch1.val_actual())
                {
                    do
                    {
                        voltdiv_ch1.altera_indice(-1);
                        panel_invalidate();
                        n++;
                    } while (numquadY / 2 * Convert.ToDouble(voltdiv_ch1.val_actual()) <= Math.Abs(sig.Amplitude) && n < voltdiv_ch1.getnumvals());
                }
                //voltdiv_ch1.altera_indice(1);
                if (n == voltdiv_ch1.getnumvals())
                {
                    Console.WriteLine("Autoset nao conseguiu alterar vdiv ch1");
                    voltdiv_ch1.setvalue(ind);
                }

                n = 0;
                ind = voltdiv_ch2.Indice;
                voltdiv_ch2.setvalue(0);
                if ((bool)onoff_ch2.val_actual())
                {
                    do
                    {
                        voltdiv_ch2.altera_indice(-1);
                        panel_invalidate();
                        n++;
                    } while (numquadY / 2 * Convert.ToDouble(voltdiv_ch2.val_actual()) <= Math.Abs(sig2.Amplitude) && n < voltdiv_ch2.getnumvals());
                }
                //voltdiv_ch2.altera_indice(-1);
                if (n == voltdiv_ch2.getnumvals())
                {
                    Console.WriteLine("Autoset nao conseguiu alterar vdiv ch1");
                    voltdiv_ch2.setvalue(ind);
                }

            }
            //panel_invalidate();
        }


        private void altera_menus(Boolean bcur, Boolean btrig, Boolean bmeas, Boolean bmath)
        {
            if (!bmeas)
            {
                measure.reset();
            }
            if (!bcur)
            {
                cursor.reset();
            }
            if (!bmath)
            {
                math.reset();
            }
            if (!btrig)
            {
                menu_trigger.reset();
            }

            updatelabel(label11, menu_trigger.imagem_actual());
            updatelabel(label12, measure.imagem_actual());
            updatelabel(label13, cursor.imagem_actual());
            updatelabel(label19, math.imagem_actual());

            altera_menu_default(!bcur && !btrig && !bmeas && !bmath);
            altera_menu_math(bmath);
            altera_menu_cursor(bcur);
            altera_menu_measure(bmeas);
            altera_menu_trigger(btrig);

        }


        private void label12_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                measure.click(sender, e);
                updatelabel(label12, measure.imagem_actual());
                bool b = (bool)measure.val_actual();
                altera_menus(false, false, b, false);
            }
            panel_invalidate();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                cursor.click(sender, e);
                updatelabel(label13, cursor.imagem_actual());
                bool b = (bool)cursor.val_actual();
                altera_menus(b, false, false, false);
            }
            panel_invalidate();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F1.click(sender, e);
                updatelabel(label14, F1.imagem_actual());
                if ((int)F1.val_actual() == 0)
                {
                    label1_default.Text = "AC";
                    serialcmd("channel1:coupling 0");
                    serialcmd("channel2:coupling 0");

                }

                else if ((int)F1.val_actual() == 1)
                {
                    label1_default.Text = "DC";
                    serialcmd("channel1:coupling 1");
                    serialcmd("channel2:coupling 1");
                }

                else if ((int)F1.val_actual() == 2)
                {
                    label1_default.Text = "GND";
                    serialcmd("channel1:coupling 2");
                    serialcmd("channel2:coupling 2");
                }

            }
            panel_invalidate();
        }

        private void F1_click_measure_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F1_measure.click(sender, e);
                if ((int)F1_measure.val_actual() == 1)
                {
                    label1_measure.Text = "CH1";
                }

                else if ((int)F1_measure.val_actual() == 2)
                {
                    label1_measure.Text = "CH2";
                }

            }
            panel_invalidate();
        }

        private void F1_click_cursor_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F1_cursor.click(sender, e);
                if ((int)F1_cursor.val_actual() == 1)
                {
                    label1_cursor.Text = "CH1";
                }

                else if ((int)F1_cursor.val_actual() == 2)
                {
                    label1_cursor.Text = "CH2";
                }

            }
            panel_invalidate();
        }

        private void F1_click_math_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F1_math.click(sender, e);
                if ((int)F1_math.val_actual() == 1)
                {
                    label1_math.Text = "+";
                    label2_math.Text = "CH1 + CH2";
                    //serialcmd(":channel2:math 0");
                }

                else if ((int)F1_math.val_actual() == 2)
                {
                    label1_math.Text = "-";
                    label2_math.Text = "CH1 - CH2";
                    //serialcmd(":channel2:math 1");
                }

            }
            panel_invalidate();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F2.click(sender, e);
                updatelabel(label15, F2.imagem_actual());
                if ((int)F2.val_actual() == 1)
                {
                    label2_default.Text = "Off";
                    serialcmd("channel1:invert 0");
                    serialcmd("channel2:invert 0");
                }

                else if ((int)F2.val_actual() == -1)
                {
                    label2_default.Text = "On";
                    serialcmd("channel1:invert 1");
                    serialcmd("channel2:invert 1");
                }
            }
            panel_invalidate();
        }

        private void F2_click_measure_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F2_measure.click(sender, e);
                paint_measure();
            }
            panel_invalidate();
        }

        private void F2_click_cursor_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F2_cursor.click(sender, e);
                switch ((int)F2_cursor.val_actual())
                {
                    case 1:
                        label2_cursor.Text = "| ";
                        break;
                    case 2:
                        label2_cursor.Text = " |";
                        break;
                    case 3:
                        label2_cursor.Text = "| |";
                        break;
                    default:
                        label2_cursor.Text = " ";
                        break;
                }
                cursor_variable.setvalue(cursor_variable.getnumvals() / 2);
            }
            panel_invalidate();
        }

        private void F2_click_trigger_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F2_trigger.click(sender, e);
                if ((int)F2_trigger.val_actual() == 1)
                {
                    label2_trigger.Text = "CH1";
                    //serialcmd(":trigger:source 0");
                }
                else if ((int)F2_trigger.val_actual() == 2)
                {
                    label2_trigger.Text = "CH2";
                    //serialcmd(":trigger:source 1");
                }
            }
            panel_invalidate();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F3.click(sender, e);
                updatelabel(label16, F3.imagem_actual());
                label3_default.Text = "x" + (int)Math.Pow((double)10, Convert.ToDouble(F3.val_actual()));
                switch ((int)F3.val_actual())
                {
                    case 0:
                        serialcmd(":channel1:probe 0");
                        serialcmd(":channel2:probe 0");
                        break;
                    case 1:
                        serialcmd(":channel1:probe 1");
                        serialcmd(":channel2:probe 1");
                        break;
                    case 2:
                        serialcmd(":channel1:probe 2");
                        serialcmd(":channel2:probe 2");
                        break;
                }
            }
            panel_invalidate();
        }

        private void F3_click_measure_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F3_measure.click(sender, e);
                paint_measure();
            }
            panel_invalidate();
        }

        private void paint_measure()
        {
            if ((int)F1_measure.val_actual() == 1)
            {
                measurements(sig);
            }
            else
            {
                measurements(sig2);
            }

            if ((int)F2_measure.val_actual() == 1)
            {
                label2_measure.Text = "\nVpp";
                Vpp = Math.Round(Vpp, 3);
                label4_measure.Text = "\n" + prefixo_unidades(Vpp) + "V";
            }
            else if ((int)F2_measure.val_actual() == 2)
            {
                label2_measure.Text = "\nVmax";
                Vmaximo = Math.Round(Vmaximo, 3);
                label4_measure.Text = "\n" + prefixo_unidades(Vmaximo) + "V";
            }
            else if ((int)F2_measure.val_actual() == 3)
            {
                label2_measure.Text = "\nVmin";
                Vminimo = Math.Round(Vminimo, 3);
                label4_measure.Text = "\n" + prefixo_unidades(Vminimo) + "V";
            }
            else if ((int)F2_measure.val_actual() == 4)
            {
                label2_measure.Text = "\nVrms";
                Vrms = Math.Round(Vrms, 3);
                label4_measure.Text = "\n" + prefixo_unidades(Vrms) + "V";
            }
            else if ((int)F2_measure.val_actual() == 5)
            {
                label2_measure.Text = "\nVavg";
                Vavg = Math.Round(Vavg, 3);
                label4_measure.Text = "\n" + prefixo_unidades(Vavg) + "V";
            }
            if ((int)F3_measure.val_actual() == 1)
            {
                label3_measure.Text = "\nFrequency";
                double f = 0;
                if ((int)F1_measure.val_actual() == 1)
                {
                    f = sig.Frequency;
                }
                else if ((int)F1_measure.val_actual() == 2)
                {
                    f = sig2.Frequency;
                }
                label5_measure.Text = "\n" + (f < 0 ? "" : (prefixo_unidades(Math.Round(f, 3)) + "Hz"));
            }

            else if ((int)F3_measure.val_actual() == 2)
            {
                label3_measure.Text = "\nPeriod";
                double T = -1;
                if ((int)F1_measure.val_actual() == 1)
                {
                    try
                    {
                        T = (double)1 / sig.Frequency;
                    }
                    catch (Exception exc)
                    {
                    }
                }
                else if ((int)F1_measure.val_actual() == 2)
                {
                    try
                    {
                        T = (double)1 / sig2.Frequency;
                    }
                    catch (Exception exc)
                    {
                    }
                }

                label5_measure.Text = "\n" + (T < 0 || (float)T == (1.0 / 0.0) ? "                    " : prefixo_unidades(T)) + "s";
            }
        }

        private void F3_click_cursor_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F3_cursor.click(sender, e);
                switch ((int)F3_cursor.val_actual())
                {
                    case 1:
                        label3_cursor.Text = "_____";
                        break;
                    case 2:
                        label3_cursor.Text = "\n_____";
                        break;
                    case 3:
                        label3_cursor.Text = "_____\n_____";
                        break;
                    default:
                        label3_cursor.Text = " ";
                        break;
                }
                cursor_variable.setvalue(cursor_variable.getnumvals() / 2);
            }
            panel_invalidate();
        }

        private void F3_click_trigger_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F3_trigger.click(sender, e);
                if ((int)F3_trigger.val_actual() == 1)
                {
                    label3_trigger.Text = "Positive";
                    serialcmd(":trigger:slop 0");
                    Edge_screen.Text = "EDGE  /";
                }

                else if ((int)F3_trigger.val_actual() == -1)
                {
                    label3_trigger.Text = "Negative";
                    serialcmd(":trigger:slop 1");
                    Edge_screen.Text = "EDGE" + "  " + @"\";
                }
            }
            panel_invalidate();
        }

        private void label17_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F4.click(sender, e);
                updatelabel(label17, F4.imagem_actual());
            }
            panel_invalidate();
        }

        private void label18_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                F5.click(sender, e);
                updatelabel(label18, F5.imagem_actual());
            }
            panel_invalidate();
        }

        private void label19_Click(object sender, EventArgs e)
        {
            if ((bool)power.val_actual())
            {
                math.click(sender, e);
                updatelabel(label19, math.imagem_actual());
                bool b = (bool)math.val_actual();
                altera_menus(false, false, false, b);
            }
            panel_invalidate();
        }

        private void CH1_source_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                sinal_CH1.click(sender, e);
                updatelabel(CH1_source, sinal_CH1.imagem_actual());
                panel_invalidate();
            }
            else if (me.Button == MouseButtons.Right)
            {
                //new Form3(this, 1, sig).ShowDialog();
                //MenuItem osc;
                MenuItem[] menuItems = new MenuItem[]{
               new MenuItem("File",this.ctxmenu_ch1_file), 
			   new MenuItem("Calibration",this.ctxmenu_ch1_cal),
               new MenuItem("Signal Generator",this.ctxmenu_ch1_sg), 
               new MenuItem("Real Oscilloscope", this.ctxmenu_ch1_Oscilloscope), 
                //osc=new MenuItem("Oscilloscope", this.ctxmenu_ch1_Oscilloscope),  
                /*new MenuItem("")*/};
                //osc.Enabled = Flag_remote;
                ContextMenu buttonMenu = new ContextMenu(menuItems);

                buttonMenu.Show(CH1, new System.Drawing.Point(20, 20));

                panel_invalidate();
            }
        }

        private void CH2_source_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                sinal_CH2.click(sender, e);
                updatelabel(CH2_source, sinal_CH2.imagem_actual());
                panel_invalidate();
            }
            else if (me.Button == MouseButtons.Right)
            {
                //new Form3(this, 2, sig2).ShowDialog();
                //Declare the menu items and the shortcut menu.
                MenuItem[] menuItems = new MenuItem[]{
               new MenuItem("File",this.ctxmenu_ch2_file), 
			   new MenuItem("Calibration",this.ctxmenu_ch2_cal),
               new MenuItem("Signal Generator",this.ctxmenu_ch2_sg), 
               new MenuItem("Real Oscilloscope", this.ctxmenu_ch2_Oscilloscope),  
               /*new MenuItem("")*/};

                ContextMenu buttonMenu = new ContextMenu(menuItems);

                buttonMenu.Show(CH2, new System.Drawing.Point(20, 20));

                panel_invalidate();
            }

        }

        private void ctxmenu_ch1_sg(object sender, EventArgs e)
        {
            new Form3(this, 1, sig).ShowDialog();
            sinal_2V_CH1.reset();
            updatelabel(ponta_prova_CH1, sinal_2V_CH1.imagem_actual());
        }
        private void ctxmenu_ch2_sg(object sender, EventArgs e)
        {
            new Form3(this, 2, sig2).ShowDialog();
            sinal_2V_CH2.reset();
            updatelabel(ponta_prova_CH2, sinal_2V_CH2.imagem_actual());
        }
        private void ctxmenu_ch1_file(object sender, EventArgs e)
        {
            new OpenFiles(this, 1);
            sinal_2V_CH1.reset();
            updatelabel(ponta_prova_CH1, sinal_2V_CH1.imagem_actual());
        }
        private void ctxmenu_ch2_file(object sender, EventArgs e)
        {
            new OpenFiles(this, 2);
            sinal_2V_CH2.reset();
            updatelabel(ponta_prova_CH2, sinal_2V_CH2.imagem_actual());
        }
        private void ctxmenu_ch1_cal(object sender, EventArgs e)    // VER!!!
        {
            sinal_2V_CH1.setvalue(1);
            updatelabel(ponta_prova_CH1, sinal_2V_CH1.imagem_actual());
            criar_sinal(1, SignalType.Square, 1000, 1, 0, 1);
        }
        private void ctxmenu_ch2_cal(object sender, EventArgs e)  // VER!!!
        {
            sinal_2V_CH2.setvalue(1);
            updatelabel(ponta_prova_CH2, sinal_2V_CH2.imagem_actual());
            criar_sinal(2, SignalType.Square, 1000, 1, 0, 1);
        }

        private void ctxmenu_ch1_Oscilloscope(object sender, EventArgs e)
        {
            if (sconfig.port == "")
            {
                new SerialCfgDialog(this, sconfig.port, sconfig.baudrate).ShowDialog();
            }

            //new SerialOsc(sconfig, this, 1);
            //return;
            System.Timers.Timer aTimer = new System.Timers.Timer(1000);

            // Create a timer with a two second interval.
            aTimer.Elapsed += OnTimedEvent_ch1;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;

            // If the timer is declared in a long-running method, use KeepAlive to prevent garbage collection
            // from occurring before the method ends. 
            //GC.KeepAlive(aTimer) 
            //new SerialOsc(sconfig, this, 1);
            sinal_2V_CH1.reset();
            updatelabel(ponta_prova_CH1, sinal_2V_CH1.imagem_actual());
        }




        private void OnTimedEvent_ch1(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (!final_aquisicao[0])
            {
                return;
            }
            final_aquisicao[0] = false;
            new SerialOsc(sconfig, this, 1);
            //Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        }
        private void OnTimedEvent_ch2(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (!final_aquisicao[1])
            {
                return;
            }
            final_aquisicao[1] = false;
            new SerialOsc(sconfig, this, 2);
            //Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        }


        private void ctxmenu_ch2_Oscilloscope(object sender, EventArgs e)
        {

            if (sconfig.port == "")
            {
                new SerialCfgDialog(this, sconfig.port, sconfig.baudrate).ShowDialog();
            }

            //new SerialOsc(sconfig, this, 2);
            //return;
            System.Timers.Timer aTimer = new System.Timers.Timer(1000);

            // Create a timer with a two second interval.
            aTimer.Elapsed += OnTimedEvent_ch2;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;

            // If the timer is declared in a long-running method, use KeepAlive to prevent garbage collection
            // from occurring before the method ends. 
            //GC.KeepAlive(aTimer) 
            //new SerialOsc(sconfig, this, 1);
            sinal_2V_CH2.reset();
            updatelabel(ponta_prova_CH2, sinal_2V_CH2.imagem_actual());
        }

        /*
        private void ponta_prova_Click(object sender, EventArgs e)
        {
            sinal_2V.click(sender, e);
            updatelabel(ponta_prova, sinal_2V.imagem_actual());

            if ((bool)sinal_2V.val_actual())
            {
                sig_ant = sig;
                sig = new SignalGenerator(SignalType.Square, 1000, 2, 0, 1, 1);
                freq_screen.Text = "1000" + "Hz";
            }
            else
            {
                sig = sig_ant;
            }

            panel_invalidate();

        }
        */

        private static string formatResourceName(string resourceName)
        {

            return Assembly.GetExecutingAssembly().GetName().Name.Replace(" ", "_") + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
        }

        private void updatelabel(Label label, string res)
        {
            if (res != null)
            {

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(formatResourceName(res)))
                {
                    label.Image = Image.FromStream(stream);
                }
            }
            else
            {
                label.Image = null;
            }
        }


        private void Label_On_Click(object sender, EventArgs e)
        {
            power.click(sender, e);
            updatelabel(PowerOn, power.imagem_actual());
            if (!(bool)power.val_actual())
            {
                onoff_ch1.reset();
                updatelabel(CH1, onoff_ch1.imagem_actual());
                onoff_ch2.reset();
                updatelabel(CH2, onoff_ch2.imagem_actual());
                menu_trigger.reset();
                updatelabel(label11, menu_trigger.imagem_actual());
                measure.reset();
                updatelabel(label12, measure.imagem_actual());
                cursor.reset();
                updatelabel(label13, cursor.imagem_actual());
                F1.reset();
                updatelabel(label14, F1.imagem_actual());
                F2.reset();
                updatelabel(label15, F2.imagem_actual());
                F3.reset();
                updatelabel(label16, F2.imagem_actual());
                F4.reset();
                updatelabel(label17, F4.imagem_actual());
                F5.reset();
                updatelabel(label18, F5.imagem_actual());
                math.reset();
                updatelabel(label19, math.imagem_actual());
                altera_menu_cursor(false);
                altera_menu_measure(false);
                altera_menu_trigger(false);
                altera_menu_math(false);

            }

            ch1_screen.Visible = (bool)power.val_actual();
            ch2_screen.Visible = (bool)power.val_actual();
            main_screen.Visible = (bool)power.val_actual();
            time_screen.Visible = (bool)power.val_actual();
            Trigger_screen.Visible = (bool)power.val_actual();
            Edge_screen.Visible = (bool)power.val_actual();
            divis_screen.Visible = (bool)power.val_actual();
            freq_screen.Visible = (bool)power.val_actual();
            freq_screen2.Visible = (bool)power.val_actual();
            panel2.Visible = (bool)power.val_actual();
            menu_default1.Visible = (bool)power.val_actual();
            menu_default2.Visible = (bool)power.val_actual();
            menu_default3.Visible = (bool)power.val_actual();
            menu_default4.Visible = (bool)power.val_actual();
            menu_lbl5.Visible = (bool)power.val_actual();
            menu_lbl6.Visible = (bool)power.val_actual();
            label1_default.Visible = (bool)power.val_actual();
            label2_default.Visible = (bool)power.val_actual();
            label3_default.Visible = (bool)power.val_actual();
            Datetime.Visible = (bool)power.val_actual();
            USB.Visible = (bool)power.val_actual();
            dataToolStripMenuItem.Enabled = (bool)power.val_actual();
            fromCH1ToolStripMenuItem.Enabled = (bool)power.val_actual();
            fromCH2ToolStripMenuItem.Enabled = (bool)power.val_actual();

            panel_invalidate();
        }

        public void panel_invalidate()
        {
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            quadricula(e);

            paint_sinal(e, sig, (bool)onoff_ch1.val_actual() && (bool)sinal_CH1.val_actual(), Color.Orange, Convert.ToDouble(F2.val_actual()), Convert.ToInt32(F1.val_actual()), Convert.ToDouble(timedivis.val_actual()), Math.Pow((double)10, -Convert.ToDouble(F3.val_actual())), Convert.ToDouble(voltdiv_ch1.val_actual()), Convert.ToInt32(pos_ch1.val_actual()), Convert.ToInt32(pos_horiz.val_actual()));
            paint_sinal(e, sig2, (bool)onoff_ch2.val_actual() && (bool)sinal_CH2.val_actual(), Color.Cyan, Convert.ToDouble(F2.val_actual()), Convert.ToInt32(F1.val_actual()), Convert.ToDouble(timedivis.val_actual()), Math.Pow((double)10, -Convert.ToDouble(F3.val_actual())), Convert.ToDouble(voltdiv_ch2.val_actual()), Convert.ToInt32(pos_ch2.val_actual()), Convert.ToInt32(pos_horiz.val_actual()));


            if ((bool)math.val_actual())
            {
                switch ((int)F1_math.val_actual())
                {
                    case 1:
                        sig.setOper(OperType.Sum, sig2);
                        break;
                    case 2:
                        sig.setOper(OperType.Sub, sig2);
                        break;
                    case 3:
                        sig.setOper(OperType.Mult, sig2);
                        break;
                    case 4:
                        sig.setOper(OperType.Div, sig2);
                        break;
                }
                paint_sinal(e, sig, (bool)onoff_ch1.val_actual() && (bool)sinal_CH1.val_actual(), Color.Red, Convert.ToDouble(F2.val_actual()), Convert.ToInt32(F1.val_actual()), Convert.ToDouble(timedivis.val_actual()), Math.Pow((double)10, -Convert.ToDouble(F3.val_actual())), 2, Convert.ToInt32(pos_ch1.val_actual()), Convert.ToInt32(pos_horiz.val_actual()));
                sig.unsetOper();
            }

            paint_freq(sig, freq_screen);
            paint_freq(sig2, freq_screen2);

            paint_measure();

            cursores(e, (bool)cursor.val_actual());
            position_cursor(e);

        }

        private void position_cursor(PaintEventArgs e)
        {
            int tpos_trigger = resYquad / 2 - Convert.ToInt32(Convert.ToDouble(trigger.val_actual()) * (double)resYquad);
            int tpos_ch1 = Convert.ToInt32(Convert.ToDouble(pos_ch1.val_actual()));
            int tpos_ch2 = Convert.ToInt32(Convert.ToDouble(pos_ch2.val_actual()));

            Pen tpen = new Pen(Color.Yellow, 1);
            Pen ppen1 = new Pen(Color.Orange, 1);
            Pen ppen2 = new Pen(Color.Cyan, 1);


            Point t1 = new Point(300, (tpos_trigger - 4));
            Point t2 = new Point(300, (tpos_trigger + 4));
            Point t3 = new Point(296, (tpos_trigger));

            Point p1 = new Point(0, (tpos_ch1 - 4));
            Point p2 = new Point(0, (tpos_ch1 + 4));
            Point p3 = new Point(4, (tpos_ch1));

            Point pp1 = new Point(0, (tpos_ch2 - 4));
            Point pp2 = new Point(0, (tpos_ch2 + 4));
            Point pp3 = new Point(4, (tpos_ch2));


            if ((bool)power.val_actual())
            {
                e.Graphics.DrawLine(tpen, t1, t2);
                e.Graphics.DrawLine(tpen, t2, t3);
                e.Graphics.DrawLine(tpen, t3, t1);

                if ((bool)onoff_ch1.val_actual())
                {
                    e.Graphics.DrawLine(ppen1, p1, p2);
                    e.Graphics.DrawLine(ppen1, p2, p3);
                    e.Graphics.DrawLine(ppen1, p3, p1);
                }
                if ((bool)onoff_ch2.val_actual())
                {
                    e.Graphics.DrawLine(ppen2, pp1, pp2);
                    e.Graphics.DrawLine(ppen2, pp2, pp3);
                    e.Graphics.DrawLine(ppen2, pp3, pp1);
                }
            }

        }


        private void paint_freq(SignalGenerator sig, Label freq)
        {
            freq.Text = sig.Frequency < 0 ? "" : sig.Frequency < 20 ? "<20Hz" : (prefixo_unidades(Math.Round(sig.Frequency, 3)) + "Hz");
        }

        void label_volt()
        {

            ch1_screen.Text = "CH1 ~ " + prefixo_unidades(Convert.ToDouble(voltdiv_ch1.val_actual())) + "V";
            ch2_screen.Text = "CH1 ~ " + prefixo_unidades(Convert.ToDouble(voltdiv_ch2.val_actual())) + "V";

            if ((int)F1.val_actual() == 1)
            {
                ch1_screen.Text = ch1_screen.Text.Replace("~", "=");
                ch2_screen.Text = ch2_screen.Text.Replace("~", "=");
            }


        }

        String prefixo_unidades(double x)
        {
            if (x >= (double)1 && x < (double)1000)
            {
                return x + "";
            }
            else if (x >= (double)1E3 && x < (double)1E6)
            {
                return (x * (double)1E-3) + "K";
            }
            else if (x >= (double)1E6 && x < (double)1E9)
            {
                return (x * (double)1E-6) + "M";
            }
            else if (x >= 1E-3 && x < 1 || x <= -1E-3 && x > -1)
            {
                return (x * (double)1E3) + "m";
            }
            else if (x >= 1E-6 && x < 1E-3 || x <= -1E-6 && x > -1E-3)
            {
                return (x * (double)1E6) + "u";
            }
            else if (x >= 1E-9 && x < 1E-6)
            {
                return (x * (double)1E9) + "n";
            }
            else
            {
                return x + "";
            }
        }



        private void cursores(PaintEventArgs e, bool enable)
        {

            if (!enable && ((int)F2_cursor.val_actual() == 0 && (int)F3_cursor.val_actual() == 0))
            {
                label4_cursor.Text = "";
                label5_cursor.Text = "";
                return;
            }


            switch ((int)F2_cursor.val_actual())
            {
                case 1:
                    cursor1_x = desenha_cursor(e, Convert.ToInt32(cursor_variable.val_actual()), cursor1_x, resXquad, true, resXquad);
                    cursor2_x = desenha_cursor(e, 0, cursor2_x, resYquad, true, resXquad);
                    serialcmd(":cursor:x1position " + Convert.ToInt32(cursor1_x * 249 / resXquad + 1));
                    break;
                case 2:
                    cursor2_x = desenha_cursor(e, Convert.ToInt32(cursor_variable.val_actual()), cursor2_x, resXquad, true, resXquad);
                    cursor1_x = desenha_cursor(e, 0, cursor1_x, resYquad, true, resXquad);
                    serialcmd(":cursor:x2position " + Convert.ToInt32(cursor2_x * 249 / resXquad + 1));
                    break;
                case 3:
                    cursor1_x = desenha_cursor(e, Convert.ToInt32(cursor_variable.val_actual()), cursor1_x, resXquad, true, resXquad);
                    cursor2_x = desenha_cursor(e, Convert.ToInt32(cursor_variable.val_actual()), cursor2_x, resXquad, true, resXquad);
                    serialcmd(":cursor:x1position " + Convert.ToInt32(cursor1_x * 249 / resXquad + 1));
                    serialcmd(":cursor:x2position " + Convert.ToInt32(cursor2_x * 249 / resXquad + 1));
                    break;
            }
            if ((int)F2_cursor.val_actual() == 0)
            {
                label4_cursor.Text = "";
            }
            else
            {
                label4_cursor.Text = val_cursor_text((double)timedivis.val_actual(), cursor1_x, cursor2_x, resXquad, numquadX, "T", "s");
            }

            switch ((int)F3_cursor.val_actual())
            {
                case 1:
                    cursor1_y = desenha_cursor(e, Convert.ToInt32(cursor_variable.val_actual()), cursor1_y, resXquad, false, resYquad);
                    cursor2_y = desenha_cursor(e, 0, cursor2_y, resXquad, false, resYquad);
                    serialcmd(":cursor:y1position " + Convert.ToInt32(cursor1_y * 199 / resYquad + 1));
                    break;
                case 2:
                    cursor2_y = desenha_cursor(e, Convert.ToInt32(cursor_variable.val_actual()), cursor2_y, resXquad, false, resYquad);
                    cursor1_y = desenha_cursor(e, 0, cursor1_y, resXquad, false, resYquad);
                    serialcmd(":cursor:y2position " + Convert.ToInt32(cursor2_y * 199 / resYquad + 1));
                    break;
                case 3:
                    cursor1_y = desenha_cursor(e, Convert.ToInt32(cursor_variable.val_actual()), cursor1_y, resXquad, false, resYquad);
                    cursor2_y = desenha_cursor(e, Convert.ToInt32(cursor_variable.val_actual()), cursor2_y, resXquad, false, resYquad);
                    serialcmd(":cursor:y1position " + Convert.ToInt32(cursor1_y * 199 / resYquad + 1));
                    serialcmd(":cursor:y2position " + Convert.ToInt32(cursor2_y * 199 / resYquad + 1));
                    break;
            }
            if ((int)F3_cursor.val_actual() == 0)
            {
                label5_cursor.Text = "";
            }
            else
            {
                label5_cursor.Text = val_cursor_text((int)F1_cursor.val_actual() == 1 ? (double)voltdiv_ch1.val_actual() : (double)voltdiv_ch2.val_actual(), cursor1_y, cursor2_y, resYquad, numquadY, "V", "V");
            }

            cursor_variable.setvalue(cursor_variable.getnumvals() / 2);

        }

        void quadricula(PaintEventArgs e)
        {
            float[] dashValues = { 2, 2, 2, 2 };

            Pen whitePen = new Pen(new SolidBrush(Color.White));

            whitePen.DashPattern = dashValues;

            label_volt();
            divis_screen.Text = prefixo_unidades(Convert.ToDouble(timedivis.val_actual())) + "s";

            if ((bool)power.val_actual())
            {
                for (int i = 0; i <= numquadY; i++)
                {
                    e.Graphics.DrawLine(whitePen, new Point(0, i * resYquad / numquadY), new Point(resXquad, i * resYquad / numquadY));
                    if (i == 3)
                    {
                        whitePen.Width = 2;
                    }
                    else
                    {
                        whitePen.Width = 0;
                    }
                }
                for (int i = 0; i <= numquadX; i++)
                {
                    e.Graphics.DrawLine(whitePen, new Point(i * resXquad / numquadX, 0), new Point(i * resXquad / numquadX, resYquad));
                    if (i == 4)
                    {
                        whitePen.Width = 2;
                    }
                    else
                    {
                        whitePen.Width = 0;
                    }
                }

            }
        }


        private double val_cursor(double escala, int curpos, int resmax, int numdiv, String var)
        {
            double operacao = 0;

            if (var == "T")
            {
                operacao = Math.Round(escala * ((double)curpos - (double)resmax / 2) / ((double)resmax / (double)numdiv), 3);
            }
            else if (var == "V")
            {
                operacao = Math.Round(escala * ((double)resmax / 2 - (double)curpos) / ((double)resmax / (double)numdiv), 3);
            }
            return operacao;
        }


        private String val_cursor_text(double escala, int curpos1, int curpos2, int resmax, int numdiv, String var, String un)
        {

            double x1 = val_cursor(escala, curpos1, resmax, numdiv, var);
            double x2 = val_cursor(escala, curpos2, resmax, numdiv, var);
            double x3 = 0;
            if (var == "T")
            {
                x3 = Math.Round(x2 - x1, 2);
            }
            if (var == "V")
            {
                x3 = Math.Round(x1 - x2, 2);
            }

            return var + "1: " + prefixo_unidades(x1) + un + "\n" + var + "2: " + prefixo_unidades(x2) + un + "\n" + "Δ:  " + prefixo_unidades(x3) + un;
        }


        private int desenha_cursor(PaintEventArgs e, int incpos, int curpos, int resmax, bool vert, int res)
        {

            Pen RedPen = new Pen(new SolidBrush(Color.Red));
            RedPen.Width = 2f;

            Pen BluePen = new Pen(new SolidBrush(Color.LightBlue));
            BluePen.Width = 2f;

            curpos += incpos;

            if (curpos >= res)
            {
                curpos = 0;
            }
            else if (curpos < 0)
            {
                curpos = res - 1;
            }

            if (vert)
            {
                e.Graphics.DrawLine(BluePen, new Point(curpos, 0), new Point(curpos, resmax));
            }
            else
            {
                e.Graphics.DrawLine(RedPen, new Point(0, curpos), new Point(resmax, curpos));
            }

            return curpos;

        }

        private void paint_sinal(PaintEventArgs e, SignalGenerator s, bool enable, Color col, double sinal, int acop, double tdiv, double pprova, double vdiv, int posY, int posX)
        {

            if (s == null || !enable)
            {
                return;
            }


            Pen pen_dot = new Pen(col, 2);


            int n = 0;

            GraphicsPath path = new GraphicsPath();

            Point[] points = new Point[amostras_ecran - posX];

            /*
            Console.WriteLine("tdiv    =" + (double)timedivis.val_actual());
            Console.WriteLine("tponto  =" + t_ponto);
            Console.WriteLine("tamostra=" + sig.tempoamostra);
            */
            double valor_medio = 0;
            double t0 = 0;

            if (acop == 2)    //GND
            {
                pprova = 0;
            }
            else if (acop == 0)    //AC
            {
                valor_medio = vmedio(s, pprova, tdiv);
            }

            double trigger_level = (Convert.ToDouble(trigger.val_actual())) * (double)resYquad;

            //Console.WriteLine("trigger " + trigger_level);
            double y0 = 0;

            for (int i = -resXquad; i < resXquad; i++)
            {
                double t = Convert.ToDouble(i) * calcTPonto(tdiv);
                // Console.WriteLine("y=" + calc_y(s, sinal, tdiv, pprova, vdiv, t, valor_medio)); 
                double y = calc_y(s, sinal, tdiv, pprova, vdiv, t, valor_medio);

                //Console.WriteLine("y=" + y + "   t=" + t); 

                if ((int)F3_trigger.val_actual() == 1)
                {
                    if (i != -resXquad && y > y0 && y > trigger_level && Math.Sign(y) == Math.Sign(trigger_level))
                    {
                        t0 = t;
                        //Console.WriteLine("y=" + y + "   t0=" + t0);
                        break;
                    }
                }
                else if ((int)F3_trigger.val_actual() == -1)
                {
                    if (y == 0)
                    {
                        trigger_level = -1;
                    }
                    if (i != -resXquad && y < y0 && y < trigger_level && Math.Sign(y) == Math.Sign(trigger_level))
                    {
                        t0 = t;
                        //Console.WriteLine("y=" + y + "   t0=" + t0);
                        break;
                    }
                }
                y0 = y;
            }

            while (n < amostras_ecran - posX && n < points.Length)
            {
                double t = Convert.ToDouble(n) * calcTPonto(tdiv);
                double y = calc_y(s, sinal, tdiv, pprova, vdiv, t + t0, valor_medio);
                points[n] = new Point(n + posX, posY - Convert.ToInt32(y));
                //Console.WriteLine(">t=" + (t + t0 )+ "   >y=" +y); 
                n++;
            }


            Point[] points1 = new Point[(points.Length / 3) * 3 - 2];

            Array.Copy(points, points1, points1.Length);

            if (s.SignalType == SignalType.ExcelCSV)
            {
                e.Graphics.DrawBeziers(pen_dot, points1);
            }
            else
            {
                path.AddLines(points);
                e.Graphics.DrawPath(pen_dot, path);
            }

        }

        private double calc_y(SignalGenerator s, double sinal, double tdiv, double pprova, double vdiv, double t, double med)
        {
            return (s.amostra(t) * pprova - med) * sinal * Convert.ToDouble(resYquad / numquadY) / vdiv;
        }



        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {

            string exitMessageText = "Are you sure you want to exit?";
            string exitCaption = "Exit";
            MessageBoxButtons button = MessageBoxButtons.YesNo;
            DialogResult res = MessageBox.Show(exitMessageText, exitCaption, button, MessageBoxIcon.Exclamation);
            if (res == DialogResult.Yes)
            {
                e.Cancel = false;
                stop = false;
                Application.ExitThread();
            }
            else if (res == DialogResult.No)
            {
                e.Cancel = true;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void set_serial_cfg(String port, int baudrate)
        {
            sconfig = new SerialConfig(port, baudrate, Parity.None, 8, StopBits.One);

        }


        private void serialDefsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SerialCfgDialog(this, sconfig.port, sconfig.baudrate).ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }


        private double calcTPonto()
        {
            return calcTPonto((double)timedivis.val_actual());
        }
        private double calcTPonto(double tdiv)
        {
            return tdiv / ((double)resXquad / (double)numquadX);
        }


        private void dataToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void saveSignal(SignalGenerator s)
        {
            SaveFileDialog SaveData = new SaveFileDialog();
            String filter = "CSV file(*.csv)|*.csv|Text Documents(*.txt)|*.txt";
            SaveData.Filter = filter;

            dataToolStripMenuItem.Enabled = true;
            try
            {
                if (SaveData.ShowDialog() == DialogResult.OK)
                {
                    String path = SaveData.FileName;
                    StreamWriter sw = new StreamWriter(path);

                    Double t = 0;
                    Double ta = s.SignalType == SignalType.ExcelCSV ? s.tempoamostra : (1 / s.Frequency) / resXquad;

                    while (t < 1 / s.Frequency)
                    {
                        sw.WriteLine("" + t + ";" + s.amostra(t));
                        t += ta;
                    }

                    sw.Close();

                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error");
            }
        }




        private Bitmap PrintScreen(int Width, int Height, int Xsource, int Ysource)
        {

            Bitmap scr = new Bitmap(Width, Height);
            Graphics grp = Graphics.FromImage(scr as Image);
            grp.CopyFromScreen(Xsource, Ysource, 0, 0, scr.Size);

            return scr;
        }

        private void saveImage(Bitmap bmp)
        {


            SaveFileDialog print = new SaveFileDialog();
            print.Filter = "JPEG(*.jpeg)|*.jpeg|PNG(*.png)|*.png";

            if (print.ShowDialog() == DialogResult.OK)
            {
                //                System.Threading.Thread.Sleep(1000);

                if (print.Filter == "JPEG(*.jpeg)|*.jpeg")
                {
                    bmp.Save(print.FileName, ImageFormat.Jpeg);
                }
                else
                {
                    bmp.Save(print.FileName, ImageFormat.Png);
                }

            }


        }



        private void screenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveImage(scrBitmap);
        }

        private void traceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveImage(trcBitmap);
        }

        private void fromCH1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSignal(sig);
        }

        private void fromCH2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSignal(sig2);
        }

        private void vvvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SerialCfgDialog(this, sconfig.port, sconfig.baudrate).ShowDialog(); 
        }

        private void downloadDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process myProcess = new Process();

            try
            {

                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = "http://www.gwinstek.com/en-global/Support/download/NULL/2/13/3/NULL/1";
                myProcess.Start();
            }
            catch (Exception exec)
            {
                MessageBox.Show(exec.Message);
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string helpFilePath = "file://C:\\Users\\JoaoCostaPereira\\Desktop\\Projecto_Final\\Manual\\OscilloscopeSimulator.chm";

            string helpFile = @"help.chm";

            string path = Path.GetFullPath(helpFile);

            Help.ShowHelp(this, path);

        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().Show();
        }

        private void sinaisToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            trcBitmap = PrintScreen(panel1.Width, panel1.Height, RectangleToScreen(this.ClientRectangle).Left + panel1.Location.X, RectangleToScreen(this.ClientRectangle).Top + panel1.Location.Y);
            scrBitmap = PrintScreen(panel2.Width, panel2.Height, RectangleToScreen(this.ClientRectangle).Left + panel2.Location.X, RectangleToScreen(this.ClientRectangle).Top + panel2.Location.Y);
        }
    }

    public class MyEventArgs : EventArgs
    {
        public String id;
        public MyEventArgs(String id)
        {
            this.id = id;
        }
    }


    public class SerialConfig
    {
        public String port;
        public int baudrate;
        public Parity parity;
        public int databits;
        public StopBits stopbits;

        public SerialConfig(String port, int baudrate, Parity parity, int databits, StopBits stopbits)
        {
            this.port = port;
            this.baudrate = baudrate;
            this.parity = parity;
            this.databits = databits;
            this.stopbits = stopbits;
        }


    }




}