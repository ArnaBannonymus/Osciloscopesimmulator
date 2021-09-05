using Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using teste_botoes;
#if COM_OFFICE_DLL
using Excel = Microsoft.Office.Interop.Excel;
#endif


namespace WindowsFormsApplication1.OpenFiles
{
    class OpenFiles
    {
        public OpenFiles(Form frm1, int ch)
        {
#if COM_OFFICE_DLL
            Excel.Application xlApp;

            Excel.Workbook xlWorkBook;

            Excel.Worksheet xlWorkSheet;

            Excel.Range range;

            int row = 0, col = 0;

            double amplitude = 0;

            xlApp = new Excel.Application();

            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Excel Workbook(*.xlsx)|*.xlsx|Excel 97-2004 Workbook(*.xls)|*.xls|Excel Macro-Enabled Workbook(*.xlsm)|*.xlsm|Text Documents(*.txt)|*.txt|CSV(*.csv)|*.csv|" + "All files|*.txt;*.csv;*.xlsx;*.xls;*.xlsm";
            of.ShowDialog();


            if (of.FileName.Length > 0)
            {

                if (of.FileName.ToUpper().EndsWith(".CSV") || of.FileName.ToUpper().EndsWith(".TXT"))
                {

                    double v = 0, tamostra = 0;
                    string[] lines = System.IO.File.ReadAllLines(@of.FileName);
                    double[] d = new double[lines.Length];

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] vals = lines[i].Split(new char[] {/* ',', */ ';', '\t' });  // VER !!!
                        d[i] = Convert.ToDouble(vals[1]);
                        //amplitude = d[i];
                        //Console.WriteLine("d[" + i + "]=" + d[i] + ";");
                        if (i == 0)
                        {
                            v = Convert.ToDouble(vals[0]);
                        }
                        else if (i == 1)
                        {
                            tamostra = Math.Abs(v - Convert.ToDouble(vals[0]));
                        }
                    }

                    
                    ((Form1)frm1).criar_sinal(ch, SignalType.ExcelCSV, d, tamostra, (double)1 / (tamostra * (double)d.Length), amplitude);
                    //((Form1)frm1).criar_sinal(ch, SignalType.ExcelCSV, d, tamostra);

                }
                else
                {

                    xlWorkBook = xlApp.Workbooks.Open(of.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);



                    range = xlWorkSheet.UsedRange;

                    int i = 0;
                    double[] d = new double[range.Rows.Count];
                    int col_x = 1, col_y = 2;

                    double tamostra = Math.Abs((double)(range.Cells[1, col_x] as Excel.Range).Value2 - (double)(range.Cells[2, col_x] as Excel.Range).Value2);


                    for (row = 1; row <= range.Rows.Count; row++)
                    {
                        try
                        {
                            d[i] = (double)(range.Cells[row, col_y] as Excel.Range).Value2;
                            //Console.WriteLine("d[" + i + "]=" + d[i] + ";");
                            i++;
                            amplitude = (double)(range.Cells[range.Rows.Count, col_y] as Excel.Range).Value2;
                        }
                        catch
                        {
                            continue;
                        }

                    }


                    ((Form1)frm1).criar_sinal(ch, SignalType.ExcelCSV, d, tamostra,(double)1/(tamostra*(double)d.Length), amplitude);


                    xlApp.Quit();



                    releaseObject(xlWorkSheet);

                    releaseObject(xlWorkBook);

                    releaseObject(xlApp);
                }
            }
#endif
        }

        private void releaseObject(object obj)
        {

#if COM_OFFICE_DLL
            try
            {

                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);

                obj = null;

            }

            catch (Exception ex)
            {

                obj = null;

                MessageBox.Show("Unable to release the Object " + ex.ToString());

            }

            finally
            {

                GC.Collect();

            }
#endif

        }
    }
}
