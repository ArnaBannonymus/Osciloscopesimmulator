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

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();

            string pdf = "User_Guide.pdf";
            //MessageBox.Show(System.IO.Path.GetFullPath(pdf));
            axAcroPDF1.LoadFile(pdf);
            axAcroPDF1.Show();
        }

    }
}
