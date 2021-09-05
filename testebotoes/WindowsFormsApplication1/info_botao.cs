using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using teste_botoes;

namespace WindowsFormsApplication1
{
    class info_botao
    {
        string[] imagens;
        object[] vals;
        int indice;
        DoubleBufferedPanel pan;
        float step, cur_step = 0, raio;
        Form1 frm;
        bool circular = true;
        String cmd = null;
        int indice_step = -1;

        public bool Circular
        {
            get { return circular; }
            set { circular = value; }
        }


        public info_botao(Form1 frm, Point p, int raio, Object[] vals)
        {
            this.frm = frm;
            this.pan = new DoubleBufferedPanel();
            this.pan.BackColor = System.Drawing.Color.Transparent;
            this.pan.Location = new System.Drawing.Point(p.X - raio, p.Y - raio);
            this.pan.Size = new Size(raio * 2, raio * 2);
            this.pan.Invalidate();
            this.pan.Click += new System.EventHandler(this.pan_Click);
            this.frm.MouseWheel += new MouseEventHandler(this.pan_Wheel);
            this.pan.Paint += new PaintEventHandler(this.pan_Paint);
            this.raio = (float)raio;
            this.step = (2f * (float)Math.PI) / vals.Length;
            this.frm.Controls.Add(this.pan);
            this.vals = vals;
        }

        public info_botao(Form1 frm, Point p, int raio, Object[] vals, String cmd)
            : this(frm, p, raio, vals)
        {
            this.cmd = cmd;
        }


        public info_botao(Form1 frm, Point p, int raio, float step, String cmd)
            : this(frm, p, raio, step)
        {
            this.cmd = cmd;
        }


        public info_botao(Form1 frm, Point p, int raio, float step, int indice_step)
            : this(frm, p, raio, step)
        {
            this.indice_step = indice_step;
        }

        public info_botao(Form1 frm, Point p, int raio, float step, int indice_step, String cmd)
            : this(frm, p, raio, step, indice_step)
        {
            this.cmd = cmd;
        }

        public info_botao(Form1 frm, Point p, int raio, float step)
        {
            this.frm = frm;
            this.pan = new DoubleBufferedPanel();
            this.pan.BackColor = System.Drawing.Color.Transparent;
            this.pan.Location = new System.Drawing.Point(p.X - raio, p.Y - raio);
            this.pan.Size = new Size(raio * 2, raio * 2);
            this.pan.Invalidate();
            this.pan.Click += new System.EventHandler(this.pan_Click);
            this.frm.MouseWheel += new MouseEventHandler(this.pan_Wheel);
            this.pan.Paint += new PaintEventHandler(this.pan_Paint);
            this.raio = (float)raio;
            this.step = step;
            this.frm.Controls.Add(this.pan);
        }


        public info_botao(Form1 frm, Point p, int raio, float step, Object[] vals, bool circular)
        {
            this.frm = frm;
            this.pan = new DoubleBufferedPanel();
            this.pan.BackColor = System.Drawing.Color.Transparent;
            this.pan.Location = new System.Drawing.Point(p.X - raio, p.Y - raio);
            this.pan.Size = new Size(raio * 2, raio * 2);
            this.pan.Invalidate();
            this.pan.Click += new System.EventHandler(this.pan_Click);
            this.frm.MouseWheel += new MouseEventHandler(this.pan_Wheel);
            this.pan.Paint += new PaintEventHandler(this.pan_Paint);
            this.raio = (float)raio;
            this.step = step;
            this.frm.Controls.Add(this.pan);
            this.vals = vals;
            this.circular = circular;
        }

        private void pan_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Right)
            {
                pan.Invalidate();
                cur_step += step;
                altera_indice(1);
            }
            if (me.Button == MouseButtons.Left)
            {
                pan.Invalidate();
                cur_step -= step;
                altera_indice(-1);
            }
            frm.panel_invalidate();
            cmdserialfloat();

        }

        private void cmdserialfloat()
        {
            cmdserial(String.Format("{0:0.###E+0}", val_actual()));
        }

        private void cmdserialfixo()
        {
            cmdserial("" + val_actual());
        }

        private void cmdserial(String val)
        {
            if (cmd != null)
            {
                if (frm.Flag_remote)
                {
                    new SerialOsc(frm.sconfig, cmd.Replace("?", val));
                }
            }
        }


        private void pan_Wheel(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.X >= pan.Location.X && me.X <= pan.Location.X + pan.Width && me.Y >= pan.Location.Y && me.Y <= pan.Location.Y + pan.Height)
            {
                pan.Invalidate();
                //cur_step += me.Delta * step;
                cur_step += Math.Sign(me.Delta) * step;
                //for (int i = 0; i < Math.Abs(me.Delta); i++)
                {
                    altera_indice(Math.Sign(me.Delta));
                }
                cmdserialfloat();
                frm.panel_invalidate();
            }
        }

        private void pan_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.DrawEllipse(new Pen(/*Color.FromArgb()*/Color.Gray, 2.0f), raio + (raio - 6.0f) * (float)Math.Cos(cur_step), raio - (raio - 6.0f) * (float)Math.Sin(cur_step), 2.0f, 2.0f);
        }

        public void setvalue(int ind)
        {

            if (ind >= vals.Length)
            {
                ind = vals.Length - 1;
            }
            else if (ind < 0)
            {
                ind = 0;
            }
            indice = ind;

        }

        public int getnumvals()
        {
            return vals.Length;
        }

        public info_botao(String path, String[] imagens)
        {
            this.imagens = imagens;
            for (int i = 0; i < imagens.Length; i++)
            {
                if (this.imagens[i] != null)
                {
                    this.imagens[i] = path + this.imagens[i];
                }
            }
            this.indice = 0;
        }

        public info_botao(String path, String[] imagens, object[] vals)
        {
            this.imagens = imagens;
            this.vals = vals;
            for (int i = 0; i < imagens.Length; i++)
            {
                if (this.imagens[i] != null)
                {
                    this.imagens[i] = path + this.imagens[i];
                }
            }
            this.indice = 0;
        }

        public info_botao(String path, String[] imagens, object[] vals, int indice_step)
            : this(path, imagens, vals)
        {
            this.indice_step = indice_step;
        }

        /*
       public info_botao(object[] vals)
       {
           this.vals = vals;
           this.indice = 0;
       }

       public void click_source(object sender, EventArgs e)
       {
           MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
           {
                   indice++;

                   if (indice == 2)
                   {
                       indice = 0;
                   }
           }
 
       }
       */

        public void altera_indice(int x)
        {
            indice += x * indice_step;
            if (indice == vals.Length)
            {
                indice = circular ? 0 : vals.Length - 1;
            }
            else if (indice < 0)
            {
                indice = circular ? vals.Length - 1 : 0;
            }
        }

        public void click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == MouseButtons.Left)
            {
                altera_indice(1);
            }
            if (me.Button == MouseButtons.Right)
            {
                altera_indice(-1);
            }

        }

        public String imagem_actual()
        {
            if (imagens == null)
            {
                return null;
            }
            else
            {
                return imagens[indice];
            }
        }

        public object val_actual()
        {
            return vals[indice];
        }


        public void reset()
        {
            indice = 0;
        }


        public void setarray(float min, float max)
        {
            this.setarray(min, max, imagens.Length);
        }

        public void setarray(int min, int max)
        {
            this.setarray((double)min, (double)max, imagens.Length);
            for (int i = 0; i < imagens.Length; i++)
            {
                vals[i] = (int)Math.Round((double)vals[i]);
            }
        }

        public void setarray(double min, double max, int len)
        {
            this.vals = new object[len];
            vals[0] = min;
            double intervalo = (max - min) / (double)(len - 1);
            //Console.WriteLine("vals:" + (min));
            for (int i = 1; i < len; i++)
            {
                vals[i] = min + intervalo * (double)i;
                //Console.WriteLine("vals:" + (min + intervalo * (double)i));
            }

        }

        public int Indice
        {
            get
            {
                return indice;
            }
        }


    }
}