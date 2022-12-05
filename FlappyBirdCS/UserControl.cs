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
using System.Net.Http;
using log4net;

namespace FlappyBirdCS
{
    public partial class UserControl : Form
    {
        //LOLIB //logger = new LOLIB();
        //string LogId = Form3.LogId;
        
        public UserControl()
        {
            InitializeComponent();
        }
        string user, pass, email,id, admin, automail,pwdDis;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //OutlookAddIn.ThisAddIn ThisAddIn;
        private void eliminaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //elimina utente
            string message = "You want to delete this account?";
            string title = "Delete account";
            string myConnectionString = Form3.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);
            //string Username, Pass, Email;
            //ThisAddIn.CreateEmailItemAndSend("","","");
            MessageBoxButtons b = MessageBoxButtons.YesNo;
            DialogResult dr = MessageBox.Show(message, title, b);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string queryS = "DELETE FROM recordxuser WHERE Username = '" + textBox1.Text + "';";
                    MySqlCommand msc = new MySqlCommand(queryS, msqData);

                    Console.WriteLine(queryS);
                    string queryS2 = "DELETE FROM userflappy WHERE Name = '" + textBox1.Text + "';";
                    MySqlCommand msc2 = new MySqlCommand(queryS2, msqData);
                    Console.WriteLine(queryS2);
                    msqData.Open();
                    msc.ExecuteNonQuery();
                    msc2.ExecuteNonQuery();
                    MessageBox.Show("User delete successfully");
                    //logger.WriteOnLog(LogId, 3, "Delete successfully");
                    Log.Info("Cancellazione avvenuta con successo dell'utente :  [" + textBox1.Text + "]\n");
                    ////loggerS.WriteOnLogSetup(LogIdS, 3, "Delete successfully");
                    this.Close();
                   
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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void modificaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //modifica utente
            string myConnectionString = Form3.connString;// "server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);
            //string Username, Pass, Email;
            if (radioButton1.Checked)
            {
                string queryS = "UPDATE userflappy SET Admin = 1 WHERE Name = '" + textBox1.Text + "';";
                MySqlCommand msc = new MySqlCommand(queryS, msqData);

                Console.WriteLine(queryS);
                msqData.Open();
                msc.ExecuteNonQuery();
                MessageBox.Show("Modification made successfully");
                //logger.WriteOnLog(LogId, 3, "Modification made successfully");
                Log.Info("Modifica effettuata(ADMIN) con successo dell'utente : [" + textBox1.Text + "]\n");
                ////loggerS.WriteOnLogSetup(LogIdS, 3, "Modification made successfully");
                msqData.Close();
            }
            if (checkBox1.Checked)
            {
                string queryU = "UPDATE userflappy SET PwdDis = 1 WHERE Name = '" + textBox1.Text + "';";
                MySqlCommand mscU = new MySqlCommand(queryU, msqData);

                Console.WriteLine(queryU);
                msqData.Open();
                mscU.ExecuteNonQuery();
                MessageBox.Show("Modification made successfully");
                //logger.WriteOnLog(LogId, 3, "Modification made successfully");
                Log.Info("Modifica effettuata(PwdDis) con successo dell'utente : [" + textBox1.Text + "]\n");
                // //loggerS.WriteOnLogSetup(LogIdS, 3, "Modification made successfully");
                msqData.Close();
            }
            else
            {
                string queryU2 = "UPDATE userflappy SET PwdDis = 0 WHERE Name = '" + textBox1.Text + "';";
                MySqlCommand mscU = new MySqlCommand(queryU2, msqData);

                //Console.WriteLine(queryU2);
                msqData.Open();
                mscU.ExecuteNonQuery();
                MessageBox.Show("Modification made successfully");
                //logger.WriteOnLog(LogId, 3, "Modification made successfully");
                Log.Info("Modifica effettuata(PwdDis) con successo dell'utente : [" + textBox1.Text + "]\n");
                // //loggerS.WriteOnLogSetup(LogIdS, 3, "Modification made successfully");
                msqData.Close();
            }
            try
            {


                string queryS = "UPDATE userflappy SET Name = '" + textBox1.Text + "', Password = '" + textBox2.Text + "', email = '" + textBox3.Text + "' WHERE UID = '" + id + "';";
                string queryI2 = "UPDATE recordxuser SET Username = '" + textBox1.Text + "' WHERE UserID = '" + id + "';";
                MySqlCommand msc = new MySqlCommand(queryS, msqData);
                MySqlCommand msc2 = new MySqlCommand(queryI2, msqData);
                Console.WriteLine(queryS);
                Console.WriteLine(queryI2);
                msqData.Open();
                msc.ExecuteNonQuery();
                msc2.ExecuteNonQuery();
                this.Close();
                MessageBox.Show("Modification made successfully");
                //logger.WriteOnLog(LogId, 3, "Modification made successfully");
                Log.Info("Modifica effettuata con successo dell'utente : [" + textBox1.Text + "] per tutti i campi \n");
                // //loggerS.WriteOnLogSetup(LogIdS, 3, "Modification made successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
                //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
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

        private void buttons3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttons1_Click(object sender, EventArgs e)
        {
            string message = "You want to delete this account?";
            string title = "Delete account";
            string myConnectionString = Form3.connString; //"server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);
            //string Username, Pass, Email;
            MessageBoxButtons b = MessageBoxButtons.YesNo;
            DialogResult dr = MessageBox.Show(message, title, b);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string queryS = "DELETE FROM recordxuser WHERE Username = '" + textBox1.Text + "';";
                    MySqlCommand msc = new MySqlCommand(queryS, msqData);

                    Console.WriteLine(queryS);
                    string queryS2 = "DELETE FROM userflappy WHERE Name = '" + textBox1.Text + "';";
                    MySqlCommand msc2 = new MySqlCommand(queryS2, msqData);
                    Console.WriteLine(queryS2);
                    msqData.Open();
                    msc.ExecuteNonQuery();
                    msc2.ExecuteNonQuery();
                    MessageBox.Show("User delete successfully");
                    //logger.WriteOnLog(LogId, 3, "Delete successfully");
                    Log.Info("Cancellazione avvenuta con successo dell'utente :  [" + textBox1.Text + "]\n");
                    ////loggerS.WriteOnLogSetup(LogIdS, 3, "Delete successfully");
                    this.Close();
                    
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

        

        private void buttons2_Click(object sender, EventArgs e)
        {
            string myConnectionString = Form3.connString; //"server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msqData = new MySqlConnection(myConnectionString);
            //string Username, Pass, Email;
            if (radioButton1.Checked)
            {
                string queryS = "UPDATE userflappy SET Admin = 1 WHERE Name = '" + textBox1.Text + "';";
                MySqlCommand msc = new MySqlCommand(queryS, msqData);

                Console.WriteLine(queryS);
                msqData.Open();
                msc.ExecuteNonQuery();
                MessageBox.Show("Modification made successfully");
                //logger.WriteOnLog(LogId, 3, "Modification made successfully");
                Log.Info("Modifica effettuata(ADMIN) con successo dell'utente : [" + textBox1.Text + "]\n");
                ////loggerS.WriteOnLogSetup(LogIdS, 3, "Modification made successfully");
                msqData.Close();
            }
            if (checkBox1.Checked)
            {
                string queryU = "UPDATE userflappy SET PwdDis = 1 WHERE Name = '" + textBox1.Text + "';";
                MySqlCommand mscU = new MySqlCommand(queryU, msqData);

                Console.WriteLine(queryU);
                msqData.Open();
                mscU.ExecuteNonQuery();
                MessageBox.Show("Modification made successfully");
                //logger.WriteOnLog(LogId, 3, "Modification made successfully");
                Log.Info("Modifica effettuata(PwdDis) con successo dell'utente : [" + textBox1.Text + "]\n");
                // //loggerS.WriteOnLogSetup(LogIdS, 3, "Modification made successfully");
                msqData.Close();
            }
            else
            {
                string queryU2 = "UPDATE userflappy SET PwdDis = 0 WHERE Name = '" + textBox1.Text + "';";
                MySqlCommand mscU = new MySqlCommand(queryU2, msqData);

                //Console.WriteLine(queryU2);
                msqData.Open();
                mscU.ExecuteNonQuery();
                MessageBox.Show("Modification made successfully");
                //logger.WriteOnLog(LogId, 3, "Modification made successfully");
                Log.Info("Modifica effettuata(PwdDis) con successo dell'utente : [" + textBox1.Text + "]\n");
                // //loggerS.WriteOnLogSetup(LogIdS, 3, "Modification made successfully");
                msqData.Close();
            }
            try
            {


                string queryS = "UPDATE userflappy SET Name = '" + textBox1.Text + "', Password = '"+textBox2.Text+"', email = '"+textBox3.Text+"' WHERE UID = '"+id+"';";
                string queryI2 = "UPDATE recordxuser SET Username = '" + textBox1.Text + "' WHERE UserID = '" + id + "';";
                MySqlCommand msc = new MySqlCommand(queryS, msqData);
                MySqlCommand msc2 = new MySqlCommand(queryI2, msqData);
                Console.WriteLine(queryS);
                Console.WriteLine(queryI2);
                msqData.Open();
                msc.ExecuteNonQuery();
                msc2.ExecuteNonQuery();
                this.Close();
                MessageBox.Show("Modification made successfully");
                //logger.WriteOnLog(LogId, 3, "Modification made successfully");
                Log.Info("Modifica effettuata con successo dell'utente : [" + textBox1.Text + "] per tutti i campi \n");
                // //loggerS.WriteOnLogSetup(LogIdS, 3, "Modification made successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
                //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
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

        private void loadUserData(object sender, EventArgs e)
        {
            user = ManagerUser.UserValue;
            pass = ManagerUser.PassValue;
            email = ManagerUser.EmailValue;
            pwdDis = ManagerUser.pwdDis;
            textBox1.Text = user;
            /* byte[] bytePassword = Encoding.UTF8.GetBytes(pass);
             string hashPassword = BitConverter.ToString(bytePassword).Replace("-", "");*/
            //string conPass = ConvMD5.MySQLEscape(pass);
            textBox2.Text = pass;
            textBox2.PasswordChar = '*';
            textBox3.Text = email;
            id = ManagerUser.ID;
            admin = ManagerUser.admin;
            automail = ManagerUser.automail;
            if (admin == "1")
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton1.Checked = false;
            }
            if (pwdDis == "1")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            //logger.WriteOnLog(LogId, 3, "Load User data");
            Log.Info("Carico i dati dell'utente\n");
            // //loggerS.WriteOnLogSetup(LogIdS, 3, "Load User data");
        }
    }
}
