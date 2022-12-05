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
using System.Security.Cryptography;
using log4net;
namespace FlappyBirdCS
{
    public partial class Registration : Form
    {
        //LOLIB //logger = new LOLIB();
        //string LogId = Form3.LogId;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Registration()
        {
            InitializeComponent();

        }

        private void changeTxtPw(object sender, EventArgs e)
        {
            passwordBox.PasswordChar = '*';
        }
        public static string userMail = "";
        private void buttons1_Click(object sender, EventArgs e)
        {
            string myConnectionString = Form3.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);
            //string pass, username;
            MySqlDataAdapter adapter;
            DataTable tableData = new DataTable();
            
            try
            {
                string queryS = "SELECT * FROM userflappy WHERE Name = '"+userBox.Text+"' AND Password = '"+passwordBox.Text+"' ;";
                string queryInsertUser = "INSERT INTO userflappy(UID,Name,Password,email,Record) VALUES (null,'"+userBox.Text+"', '"+passwordBox.Text+"' ,'"+emailBox.Text+"',0);";
                userMail = emailBox.Text;
                MySqlCommand msc = new MySqlCommand(queryS, msqData);

                Console.WriteLine(queryS);
                msqData.Open();
                //insert
                
                adapter = new MySqlDataAdapter(queryS, msqData);
                adapter.Fill(tableData);

                if (tableData.Rows.Count <= 0)
                {
                    if (userBox.Text == "" || passwordBox.Text == "")
                    {
                        MessageBox.Show("Insert an username or a password\nTry again");
                        //logger.WriteOnLog(LogId, "No username or password", 3);
                        Log.Error("Nessun username o password inseriti" + "\n==========");
                       // //loggerS.WriteOnLogSetup(LogIdS, "No username or password", 3);
                    }
                    else
                    {
                        MySqlCommand mscI = new MySqlCommand(queryInsertUser, msqData);
                        Console.WriteLine(queryInsertUser);
                        if (passwordBox.TextLength < 8)
                        {
                            MessageBox.Show("The password must be at least 8 characters long");
                            //logger.WriteOnLog(LogId, "Password must be at least 8 characters long", 3);
                            Log.Error("Formato password errato---Lunghezza minima 8 caratteri" + "\n==========");
                            ////loggerS.WriteOnLogSetup(LogIdS, "Password must be at least 8 characters long", 3);
                        }
                        else
                        {
                            mscI.ExecuteNonQuery();
                            MessageBox.Show("User insert corrected");
                            //logger.WriteOnLog(LogId, "User inser corrected", 3);
                            Log.Info("Utente inserito correttamente\n");
                           // //loggerS.WriteOnLogSetup(LogIdS, "User inser corrected", 3);
                            this.Close();
                        }
                        
                    }
                    
                    
                }
                else
                {

                    MessageBox.Show("User exist\nTry new username and password");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
                MessageBox.Show(ex.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                //logger.WriteErrorOnLog(LogId,1,ex,"Error");
                Log.Error("Error = " + ex.Message + "\n==========");
               // //loggerS.WriteErrorOnLogSetup(LogIdS, 1, ex, "Error");
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
