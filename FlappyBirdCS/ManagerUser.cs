using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using FlappyBirdCS.Manager;
using System.Security.Cryptography;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing.Printing;
using log4net;
namespace FlappyBirdCS
{
    public partial class ManagerUser : Form
    {
        //LOLIB //logger = new LOLIB();
        //string LogId = Form3.LogId;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ManagerUser()
        {
            InitializeComponent();
        }
        public static string UserValue, PassValue, EmailValue, ID, admin, automail,pwdDis;

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfdTXT = new SaveFileDialog();
            sfdTXT.Filter = "TXT (*.txt)|*.txt";
            sfdTXT.FileName = "ManagerUser.txt";
            if (sfdTXT.ShowDialog() == DialogResult.OK)
            {
                using (TextWriter tw = new StreamWriter("ManagerUser.txt"))
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "ManagerUser.pdf";
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
                                    //questo if per evitare problema con riferimento non impostato su instanmza di oggetto
                                    if (cell.Value != null)
                                    {
                                        pdfTable1.AddCell(cell.Value.ToString());
                                    }
                                    
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

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            //printDialog1.ShowDialog();
            //PrintDocument printDoc = new PrintDocument();
            //printDoc.DocumentName = "Print Document";
            //printDialog1.Document = printDoc;
            //printDialog1.AllowSelection = true;
            //printDialog1.AllowSomePages = true;
            ////Call ShowDialog  
            //if (printDialog1.ShowDialog() == DialogResult.OK) printDoc.Print();
             MessageBox.Show("Work in progress...");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void load(object sender, EventArgs e)
        {
            label1.Text = "[ '"+ Form4.ValueOfTxtBox1+"' ]";
            //logger.WriteOnLog(LogId, "Load User Data", 3);
            ////loggerS.WriteOnLogSetup(LogIdS, "Load User Data", 3);
        }
        
        private void showData()
        {
            string myConnectionString = Form3.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);
            
            try
            {


                string queryS = "SELECT Name,email as Email,Record,Admin,PwdDis as PasswordDisabled FROM userflappy ;";
                MySqlCommand msc = new MySqlCommand(queryS, msqData);

                Console.WriteLine(queryS);
                msqData.Open();
                msc.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter ad = new MySqlDataAdapter(msc);
                ad.Fill(dt);
                dataGridView1.DataSource = dt;//to show data from datatable, in form add datagridview
                                              //logger.WriteOnLog(LogId, "Load All User", 3);
                                              // //loggerS.WriteOnLogSetup(LogIdS, "Load All User", 3);
                Log.Info("Carico tutti gli utenti\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
                //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
                ////loggerS.WriteErrorOnLogSetup(LogIdS, 1, ex, "Error");
                Log.Error("Error = " + ex.Message + "\n==========");
            }
            finally
            {
                if (msqData.State == ConnectionState.Open)
                {
                    msqData.Close();
                }
            }
        }

        private void showUser(object sender, EventArgs e)
        {
            showData();
        }

        

        private void buttons3_Click(object sender, EventArgs e)
        {
            this.Close();
            //logger.WriteOnLog(LogId, "Close Manager User", 3);
           // //loggerS.WriteOnLogSetup(LogIdS, "Close Manager User", 3);
        }

        private void buttons4_Click(object sender, EventArgs e)
        {
            string myConnectionString = Form3.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);
            //string Username, Pass, Email;
            if (textBox1.Text == "")
            {
                MessageBox.Show("Choose an user");
            }
            try
            {


                string queryS = "SELECT * FROM userflappy WHERE Name = '"+textBox1.Text+"';";
                MySqlCommand msc = new MySqlCommand(queryS, msqData);
                
                
                Console.WriteLine(queryS);
                msqData.Open();
                MySqlDataReader reader = msc.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserValue = reader["Name"].ToString();
                        PassValue = reader["Password"].ToString();
                        EmailValue = reader["email"].ToString();
                        ID = reader["UID"].ToString();
                        admin = reader["Admin"].ToString();
                        pwdDis = reader["PwdDis"].ToString();

                    }
                    UserControl us = new UserControl();
                    us.Show();
                    //logger.WriteOnLog(LogId, "Show User Control", 3);
                    ////loggerS.WriteOnLogSetup(LogIdS, "Show User Control", 3);
                    Log.Info("Apro UserControl\n");
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("No User Found");
                }
                reader.Close();
                
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
    }
}
