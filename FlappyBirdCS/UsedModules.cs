using FlappyBirdCS.Manager;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Diagnostics;
using log4net;
using Microsoft.Office;
namespace FlappyBirdCS
{
    public partial class UsedModules : Form
    {
        public UsedModules()
        {
            InitializeComponent();
            
        }

        //LOLIB //logger = new LOLIB();
        //string LogId = Form3.LogId;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\packages.config";
        
        private void LoadPackages(object sender, EventArgs e)
        {
            //show xml packages data
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(appPath);
                
                //Log.Info(appPath);
                //logger.WriteOnLog(LogId, "Carico i dati", 3);
                Log.Info("Carico i dati\n");
                ////logger.WriteOnLog(LogId, $"Dati caricati : {result}", 3);
                dataGridView1.DataSource = ds.Tables[0];
                //DisplayInfo();
                
            }
            catch (Exception ex)
            {
                //"Error while fetching the record" + ex.Message;
                //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
                Log.Error("Error = " + ex.Message + "\n==========");
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "UsedModules.pdf";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            PdfPTable pdfTable = new PdfPTable(dataGridView1.Columns.Count);
                            pdfTable.DefaultCell.Padding = 3;
                            pdfTable.WidthPercentage = 100;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                pdfTable.AddCell(cell);
                            }

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    pdfTable.AddCell(cell.Value.ToString());
                                }
                            }

                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                                stream.Close();
                            }

                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                            Log.Error("Error = " + ex.Message + "\n==========");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog sfdTXT = new SaveFileDialog();
            sfdTXT.Filter = "TXT (*.txt)|*.txt";
            sfdTXT.FileName = "Used-Modules.txt";
            if (sfdTXT.ShowDialog() == DialogResult.OK)
            {
                using (TextWriter tw = new StreamWriter("Used-Modules.txt"))
                {
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            tw.Write($"{dataGridView1.Rows[i].Cells[j].Value.ToString()}");

                            if (j != dataGridView1.Columns.Count - 1)
                            {
                                tw.Write("---");
                            }
                        }
                        tw.WriteLine();
                    }
                }
                MessageBox.Show("saved successfully");
            }
            
            
        }

        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {
                worksheet = workbook.ActiveSheet;

                worksheet.Name = "UsedModules";

                int cellRowIndex = 1;
                int cellColumnIndex = 1;

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        if (cellRowIndex == 1)
                        {
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGridView1.Columns[j].HeaderText;
                        }
                        else
                        {
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                        }
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 1;
                saveDialog.FileName = "UsedModules.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Export Successful!");
                }
            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error("Error = " + ex.Message + "\n==========");
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }
        }
        /*private void DisplayInfo()
{
   Int32 indent = 0;
   Assembly a = typeof(UsedModules).Assembly;
   Display(indent, "Assembly identity={0}", a.FullName);

   Display(indent, "Referenced assemblies:");
   foreach (AssemblyName an in a.GetReferencedAssemblies())
   {
       Display(indent + 1, "Name={0}, Version={1}", an.Name, an.Version);
   }
   Display(indent, "");
   foreach(Assembly b in AppDomain.CurrentDomain.GetAssemblies())
   {
       //Display(indent, "Assembly: {0}", b);

       // Display information about each module of this assembly.
       foreach (Module m in b.GetModules(true))
       {
          // Display(indent + 1, "Module: {0}", m.Name);
       }
       indent += 1;
   }

}
public static void Display(Int32 indent, string format, params object[] param)

{
   //Console.Write(new string(' ', indent * 2));
  // Console.WriteLine(format, param);
   using (TextWriter twDLL = new StreamWriter("TestDLL.txt"))
   {
       twDLL.Write(new string(' ', indent * 2));
       twDLL.WriteLine(format,param);
   }
}

private void buttons1_Click(object sender, EventArgs e)
{
   DisplayInfo();
}*/
    }
}
