using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using MySql.Data.MySqlClient;
using FlappyBirdCS.Manager;
using log4net;
namespace FlappyBirdCS
{
    public partial class RecPassword : Form
    {
        string myConnectionString = Form3.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
        //LOLIB //logger = new LOLIB();
        //string LogId = Form3.LogId;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public RecPassword()
        {
            InitializeComponent();
        }
        string randomCode;
        public static string to;
        /*to send email from OutLook Add-In, use this : 
                     OutlookAddIn.ThisAddIn ThisAddIn; HERE*/
        private void buttons1_Click(object sender, EventArgs e)
        {
            MySqlConnection msq = new MySqlConnection(myConnectionString);
             

            try
            {
                string userMail = textBox1.Text;
                //string email = "";
               
                string queryS = "SELECT * FROM userflappy WHERE userflappy.email = '" + userMail + "';";
                MySqlCommand msc = new MySqlCommand(queryS, msq);

                Console.WriteLine(queryS);
                msq.Open();
                MySqlDataAdapter adapter;

                DataTable table = new DataTable();
                msc.ExecuteNonQuery();
                //logger.WriteOnLog(LogId, "Search User Email", 3);
                Log.Info("Cerco l'email dell'utente\n");
                //loggerS.WriteOnLogSetup(LogIdS, "Search User Email", 3);
                adapter = new MySqlDataAdapter(queryS, msq);
                adapter.Fill(table);
                if (table.Rows.Count <= 0)
                {
                    MessageBox.Show("The email is not present in the database");
                    //logger.WriteOnLog(LogId, "Error : No Email in database", 1);
                    Log.Error("L'email non è presente all'interno del Database" + "\n==========");
                   // //loggerS.WriteOnLogSetup(LogIdS, "Error : No Email in database", 1);
                }
                else
                {
                    /*to send email from OutLook Add-In, use this : 
                     ThisAddIn.CreateEmailItemAndSend("","","");HERE
                     
                     */
                    var fromAddress = new MailAddress("noreplyflappybirdcs@gmail.com", "FlappyBird Team");
                    var toAddress = new MailAddress(userMail);
                    const string fromPassword = "FlappyBird14cs";
                    const string subject = "Recovery Password";
                    Random rand = new Random();
                    randomCode = (rand.Next(999999)).ToString();
                    string content = "\nDownload the ultimate Flappy Bird game totally gratis on 'https://salvuccif.altervista.org/FirstPage/index.html'";
                    string body = "Your random code is : \n" + randomCode + "\n\n\n" + content;

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        //IsBodyHtml = true
                    })
                    {
                        smtp.Send(message);
                    }
                    MessageBox.Show("Email successfully sent with verify code");
                    //logger.WriteOnLog(LogId, "Email successfully sent", 3);
                    Log.Info("Email inviata con successo\n");
                    //loggerS.WriteOnLogSetup(LogIdS, "Email successfully sent", 3);
                    //this.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
                //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
                Log.Error("Error = " + ex.Message + "\n==========");
                //loggerS.WriteErrorOnLogSetup(LogIdS, 1, ex, "Error");
            }
            finally
            {
                if (msq.State == ConnectionState.Open)
                {
                    msq.Close();
                }
            }
            
        }

        private void buttons2_Click(object sender, EventArgs e)
        {
            //on click verify the code send by email and show the new form to reset password
            if (randomCode == (textBox2.Text).ToString())
            {
                to = textBox1.Text;
                NewPass newP = new NewPass();
                this.Hide();
                newP.Show();
                //logger.WriteOnLog(LogId, "Open NewPassword form ", 3);
                Log.Info("Apro NewPassword\n");
               // //loggerS.WriteOnLogSetup(LogIdS, "Open NewPassword form ", 3);
            }
            else
            {
                //logger.WriteOnLog(LogId, "Error : wrong Code", 1);
                //loggerS.WriteOnLogSetup(LogIdS, "Error : wrong Code", 1);
                Log.Error("Codice errato");
                MessageBox.Show("Wrong Code");
            }
           
        }
    }
}
