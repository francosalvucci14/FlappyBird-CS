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
using MySql.Data.MySqlClient;
using System.Resources;
using Tulpep.NotificationWindow;
using log4net;
namespace FlappyBirdCS
{
    public partial class Form4 : Form
    {
        bool check;
        //LOLIB //logger = new LOLIB();
        //string LogId = Form3.LogId;
        
        public Form4()
        {
            InitializeComponent();
            check = false;
            

        }
        string myConnectionString = Form3.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ValueOfTxtBox1 = "";// varaible to pass data from this form to another, because it's is public.
        string appFilesPath = Form3.appFilesPath;
        string pwdDis;
        /*LOLIB //logger = new LOLIB();
        string LogId = LOLIB.CodeGen("FlappyBirdCS");*/
        private void buttons1_Click(object sender, EventArgs e)
        {
            /*
             * Inserire il controllo sulla password se viene disabilitata tramite la form apposita
             * Eseguire una query diversa, cercando solo tramite nome, senza password quindi
             * SELECT * FROM userflappy WHERE Name = '" + textBox1.Text + "';
             */
            
            
           
            MySqlConnection msq = new MySqlConnection(myConnectionString);
            try
            {
                //logger.WriteOnLog(LogId,"Controllo l'esistenza dell'utente",3);
                ////loggerS.WriteOnLogSetup(LogIdS, "Controllo l'esistenza dell'utente", 3);
                MySqlDataAdapter adapter;

                DataTable table = new DataTable();
                /*msq.Open();
                Console.WriteLine("Connection Open");*/
                //logger.WriteOnLog(LogId, "Cerca utente", 3);
                ////loggerS.WriteOnLogSetup(LogIdS, "Cerca utente", 3);
                string user = textBox1.Text;
                ValueOfTxtBox1 = textBox1.Text;
                string pass = textBox2.Text;
                Console.WriteLine("User : " + user);
                Console.WriteLine("Password : " + pass);
                string searchUserOnly = "SELECT * FROM userflappy WHERE Name = '" + textBox1.Text + "';";
                string queryS = "SELECT * FROM userflappy WHERE Name = '" + textBox1.Text + "' AND Password = '" + textBox2.Text + "';";
                
                Console.WriteLine(queryS);
                MySqlCommand mySqlCommand = new MySqlCommand(searchUserOnly, msq);
                msq.Open();
                MySqlDataReader reader = mySqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        pwdDis = reader["PwdDis"].ToString();

                    }

                }
                reader.Close();
                msq.Close();
                if (pwdDis == "0")
                {
                    adapter = new MySqlDataAdapter(queryS, msq);
                    adapter.Fill(table);

                    if (table.Rows.Count <= 0)
                    {
                        //MessageBox.Show("Username or Password are invalid");
                        ////logger.WriteOnLog(LogId, "User inesistente", 3);
                        //logger.WriteOnLog(LogId, "Error : Utente/Password inesistenti", 1);
                        Log.Error("Error\nUsername or Password are invalid\n==========");
                        ////loggerS.WriteOnLogSetup(LogIdS, "Error : Utente/Password inesistenti", 1);
                        string noentry = "\u26D4";
                        notifyIcon1.ShowBalloonTip(1000, noentry + "WAIT" + noentry, "Error : \nUsername or Password are invalid " + noentry + "!!!", ToolTipIcon.Error);
                    }
                    else
                    {
                        string happy = "\u263A";
                        notifyIcon1.ShowBalloonTip(1000, "Welcome", "Welcome back : " + textBox1.Text + "\nHave a good day and fun game " + happy + " !", ToolTipIcon.Info);


                        this.Close();


                    }
                    

                }
                else
                {
                    adapter = new MySqlDataAdapter(searchUserOnly, msq);
                    adapter.Fill(table);

                    if (table.Rows.Count <= 0)
                    {
                        //MessageBox.Show("Username or Password are invalid");
                        ////logger.WriteOnLog(LogId, "User inesistente", 3);
                        //logger.WriteOnLog(LogId, "Error : Utente/Password inesistenti", 1);
                        Log.Error("Error\nUsername or Password are invalid\n==========");
                        ////loggerS.WriteOnLogSetup(LogIdS, "Error : Utente/Password inesistenti", 1);
                        string noentry = "\u26D4";
                        notifyIcon1.ShowBalloonTip(1000, noentry + "WAIT" + noentry, "Error : \nUsername or Password are invalid " + noentry + "!!!", ToolTipIcon.Error);
                    }
                    else
                    {
                        string happy = "\u263A";
                        notifyIcon1.ShowBalloonTip(1000, "Welcome", "Welcome back : " + textBox1.Text + "\nHave a good day and fun game " + happy + " !", ToolTipIcon.Info);


                        this.Close();


                    }
                   
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
                MessageBox.Show("Error\nView log file for error");
                //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
                Log.Error("Error = " + ex.Message + "\n==========");
                ////loggerS.WriteErrorOnLogSetup(LogIdS, 1, ex, "Error");
            }
            finally
            {
                if (msq.State == ConnectionState.Open)
                {
                    msq.Close();
                }
            }
            
            /*
            try
            {
                MySqlCommand msc = msq.CreateCommand();
                msc.CommandText = "SELECT * FROM userflappy WHERE Name = @username AND Password = @password;";
                //UPDATE `userflappy` SET `Record` = '5' WHERE `userflappy`.`UID` = 2; command for update the table to set the record of the user. --change the UID and the Record obs
                msc.Parameters.AddWithValue("@username",user);
                msc.Parameters.AddWithValue("@password",pass);
                var query = msc.ExecuteNonQuery();
                if (query <= 1)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("User Not Found");
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
            }
            finally
            {
                if(msq.State == ConnectionState.Open)
                {
                    msq.Close();
                }
            }*/
        }

        private void changeText(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void label1_Click(object sender, EventArgs e)
        {
            RecPassword recp = new RecPassword();
            recp.Show();
            //logger.WriteOnLog(LogId, "Apro form per recupero password", 3);
            ////loggerS.WriteOnLogSetup(LogIdS, "Apro form per recupero password", 3);
        }

        private void changePass(object sender, EventArgs e)
        {
            

            if (check)
            {
                
                check = false;
                textBox2.PasswordChar = new char();
                Image img = Image.FromFile(appFilesPath + @"\Images\eye-slash-solid-removebg-preview.png");
                pictureBox1.Image = img;
                //logger.WriteOnLog(LogId, "Mostra password", 3);
                ////loggerS.WriteOnLogSetup(LogId, "Mostra password", 3);
            }
            else
            {
               
                check = true;
                textBox2.PasswordChar = '*';
                Image img = Image.FromFile(appFilesPath + @"\Images\eye-regular-removebg-preview.png");
                pictureBox1.Image = img;
                //logger.WriteOnLog(LogId, "Nascondi password", 3);
               // //loggerS.WriteOnLogSetup(LogIdS, "Nascondi password", 3);
            }
        }
        

       
    }
}
