using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlappyBirdCS.Manager;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using log4net;
namespace FlappyBirdCS
{
    public partial class RecordTable : Form
    {
        //LOLIB //logger = new LOLIB();
        //string LogId = Form3.LogId;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public RecordTable()
        {
            InitializeComponent();
        }
        
        private void showData()
        {
            string myConnectionString = Form3.connString;//$"server={Form3.dbServer};database={Form3.dbHost};uid={Form3.dbUser};pwd={Form3.dbPass};";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);

            try
            {
                
                
                string queryS = "SELECT Username, RecordN as Record  FROM recordxuser ;";
                MySqlCommand msc = new MySqlCommand(queryS, msqData);

                Console.WriteLine(queryS);
                msqData.Open();
                msc.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter ad = new MySqlDataAdapter(msc);
                ad.Fill(dt);
                dataGridView1.DataSource = dt;//to show data from datatable, in form add datagridview
                //logger.WriteOnLog(LogId, "Show All User ", 3);
                Log.Info("Carico i record di tutti gli utenti\n");
                ////loggerS.WriteOnLogSetup(LogIdS, "Show All User ", 3);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
                //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
                Log.Error("Error = " + ex.Message + "\n==========");
                ////loggerS.WriteErrorOnLogSetup(LogIdS, 1, ex, "Error");
            }
            finally
            {
                if (msqData.State == ConnectionState.Open)
                {
                    msqData.Close();
                }
            }
        }

        private void RecordTable_Load(object sender, EventArgs e)
        {           
                showData();
            /*dataGridView1.Columns["Username"].Width = 150;
            dataGridView1.Columns["Record"].Width = 150;*/
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfdTXT = new SaveFileDialog();
            sfdTXT.Filter = "TXT (*.txt)|*.txt";
            sfdTXT.FileName = "RecordTable.txt";
            if (sfdTXT.ShowDialog() == DialogResult.OK)
            {
                using (TextWriter tw = new StreamWriter("RecordTable.txt"))
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

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "RecordTables.pdf";
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
                            PdfPTable pdfTable1 = new PdfPTable(dataGridView1.Columns.Count);
                            pdfTable1.DefaultCell.Padding = 3;
                            pdfTable1.WidthPercentage = 100;
                            pdfTable1.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                pdfTable1.AddCell(cell);
                            }

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    pdfTable1.AddCell(cell.Value.ToString());
                                }
                            }

                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                pdfDoc.Add(pdfTable1);
                                pdfDoc.Close();
                                stream.Close();
                            }

                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                            Log.Error("Error = " + ex.Message + "\n==========");
                            //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
