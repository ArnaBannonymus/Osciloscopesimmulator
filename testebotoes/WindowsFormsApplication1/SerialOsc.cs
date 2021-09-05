using Signals;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using teste_botoes;

namespace WindowsFormsApplication1
{
    class SerialOsc
    {
        private int ch;
        private Form frm1;
        SerialPort sport;
        private byte[] raw;
        int step;
        double[] doubledata;
        double amplitude;
        private int lf_pos;
        private Single tamostra;


        public SerialOsc(SerialConfig sportcfg, String cmd)
        {
            try
            {
                serialport_connect(sportcfg.port, sportcfg.baudrate, sportcfg.parity, sportcfg.databits, sportcfg.stopbits);
                sport.WriteLine(cmd);
                step = -1;
                sport.Close();
            }
            catch (Exception ex)
            {
            }

        }

        public SerialOsc(SerialConfig sportcfg, Form frm1, int ch)
        {
            this.frm1 = frm1;
            this.ch = ch;

            try
            {
                serialport_connect(sportcfg.port, sportcfg.baudrate, sportcfg.parity, sportcfg.databits, sportcfg.stopbits);

                sport.WriteLine(":measure:source " + ch);

                sport.WriteLine(":acquire" + ch + ":memory?;:channel" + ch + ":scale?;:measure:frequency?");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("First select a COM Port on menu bar -> Communication options", "Error");
            }

            this.ch = ch;

            this.raw = new byte[0];

            step = 0;
        }

        public void serialport_connect(String port, int baudrate, Parity parity, int databits, StopBits stopbits)
        {
            try
            {
                sport = new System.IO.Ports.SerialPort(port, baudrate, parity, databits, stopbits);
                sport.Open();
                sport.DataReceived += new SerialDataReceivedEventHandler(sport_DataReceived);
            }
            catch (Exception ex)
            {
            }
        }

        private void sport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            String dtn = dt.ToShortTimeString();


            if (this.frm1 != null)
            {

                //System.Threading.Thread.Sleep(2000);


                int bytes = sport.BytesToRead;


                if (bytes > 0)
                {
                    byte[] raw_parc = new byte[bytes];

                    byte[] raw_temp = new byte[raw.Length];

                    Array.Copy(raw, raw_temp, raw.Length);

                    sport.Read(raw_parc, 0, bytes);

                    raw = new byte[bytes + raw_temp.Length];

                    Array.Copy(raw_temp, raw, raw_temp.Length);

                    Array.Copy(raw_parc, 0, raw, raw_temp.Length, raw_parc.Length);
                }




                if (raw.Length>1 && raw[raw.Length - 1] == 0x0A)
                {


                    if (step == 0)
                    {
                        //obter AMOSTRAS
                        int datasizedigit = raw[1] - '0';
                        byte[] baux = new byte[datasizedigit];


                        Array.Copy(raw, 2, baux, 0, datasizedigit);
                        int datasize = Convert.ToInt32(System.Text.Encoding.ASCII.GetString(baux));


                        //determinar o "timer interval". tem de ser Single!
                        tamostra = BitConverter.ToSingle(new byte[] { raw[datasizedigit + 5], raw[datasizedigit + 4], raw[datasizedigit + 3], raw[datasizedigit + 2] }, 0);

                        //Console.WriteLine("x=" + tamostra);


                        String cmd = ":display:" + (ch - 1);

                        int ch_raw = raw[datasizedigit + 6];


                        doubledata = new double[(datasize - 8) / 2];
                        int j = 0;
                        int amostra=0;
                        for (int i = datasizedigit + 11; i < datasize; i = i + 2)
                        {
                            byte[] a = new byte[2];

                            Array.Copy(raw, i, a, 0, 2);
                            int amostra1 = BitConverter.ToInt16(a, 0);
                            if (amostra1 <200 && amostra1>-200)
                            { 
                                //Console.WriteLine("y=" + amostra);
                                amostra = amostra1;
                            }
                            doubledata[j] = Convert.ToDouble(amostra);
                            //Console.WriteLine("y=" + doubledata[j]);
                            j++;
                        }

                        step++;

                        lf_pos = datasizedigit + 1 + datasize;

                    }


                    // obter ESCALA
                    if (step == 1)
                    {

                        int lf_pos_next = -1;
                        for (int i = lf_pos + 1; i < raw.Length; i++)
                        {
                            if (raw[i] == 0x0A)
                            {
                                lf_pos_next = i;
                                break;
                            }
                        }
                        if (lf_pos_next >= 0)
                        {

                            byte[] raw_temp = new byte[lf_pos_next - lf_pos - 1];

                            Array.Copy(raw, lf_pos + 1, raw_temp, 0, raw_temp.Length);

                            String s = System.Text.Encoding.UTF8.GetString(raw_temp);

                            double dval = Double.Parse(s, System.Globalization.CultureInfo.GetCultureInfo("en-US"));

                            //Console.WriteLine("escala=" + dval);

                            for (int i = 0; i < doubledata.Length; i++)
                            {
                                doubledata[i] *= (dval / (double)25);
                                //Console.WriteLine(doubledata[i]);
                            }
                            lf_pos = lf_pos_next;
                            step++;
                        }
                    }


                    //obter FREQUENCIA
                    if (step == 2)
                    {
                        int lf_pos_next = -1;
                        for (int i = lf_pos + 1; i < raw.Length; i++)
                        {
                            if (raw[i] == 0x0A)
                            {
                                lf_pos_next = i;
                                break;
                            }
                        }
                        if (lf_pos_next >= 0)
                        {

                            byte[] raw_temp = new byte[lf_pos_next - lf_pos - 1];

                            Array.Copy(raw, lf_pos + 1, raw_temp, 0, raw_temp.Length);

                            String s = System.Text.Encoding.UTF8.GetString(raw_temp);

                            double dval;

                            try
                            {
                                dval = Double.Parse(s, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                            }
                            catch (Exception ex)
                            {
                                dval = -1;
                            }
                            Console.WriteLine("freq=" + dval);


                            ((Form1)frm1).criar_sinal(ch, SignalType.ExcelCSV, doubledata, (double)tamostra, dval, amplitude);

                            step++;
                        }

                    }
                }

            }

            if (sport.IsOpen && (step == 3 || step < 0))
            {
                sport.Close();
                ((Form1)frm1).final_aquisicao[ch-1] = true;
            }

        }

    }

}
