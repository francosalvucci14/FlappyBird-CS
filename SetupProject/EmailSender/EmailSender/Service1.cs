using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Net.Mail;
using System.Net;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Security;

namespace EmailSender
{
    public partial class Service1 : ServiceBase
    {
        System.Diagnostics.EventLog FlappyLog = new System.Diagnostics.EventLog();
        
        public Service1()
        {
            InitializeComponent();
            
            /*if (!System.Diagnostics.EventLog.SourceExists("FlappyBirdMail"))
            {
                System.Diagnostics.EventLog.CreateEventSource("FlappyBirdMail", "FlappyBirdScheduler");
            }
            
            FlappyLog.Source = "FlappyBirdMail";
            FlappyLog.Log = "FlappyBirdScheduler";*/

        }
        
        protected override void OnStart(string[] args)
        {
            try
            {
                //FlappyLog.WriteEntry("In OnStart --- Sending Mail");

                System.Timers.Timer time = new System.Timers.Timer();

                time.Start();

                time.Interval = 300000;

                time.Elapsed += Time_Elapsed;
            }
            catch (SecurityException s)
            {
                Console.WriteLine(s.ToString());
            }
            
            
        }

        public void Time_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {//------------disabilitato per soluzione migliore--------------


            //.WriteEntry("Mail Sending on " + DateTime.Now.ToString());
            //string toEmail = SearchUserEmail();
            //string toEmail = SearchUserEmailAndSend();
            //int n = getEmailNumber();
            //string[] email = new String[n];
            //FlappyLog.WriteEntry("Mail : " + toEmail);
            //string[] split = toEmail.Split(new Char[] { '|' });


            /*string delimStr = "|";
            char[] delimiter = delimStr.ToCharArray();
            string[] split = null;
            for (int x = 1; x<toEmail.Length; x++)
            {
                split = toEmail.Split(delimiter,x);
                foreach (string emails in split)
                {      
                        SendEmail(emails, "noreplyflappybirdcs@gmail.com", "Automatic Mail sending", "Successfully working contact with" + emails);

                }

            }*/
            SearchUserEmailAndSend();//richiamo funzioe ce cerca le email degli utenti abilitiati alla email automatica e la mando.
        }

        protected override void OnStop()
        {

        }
        public bool SendEmail(string strTo, string strFrom, string strSubject, string strBody)
        {

            bool flag = false;
            
            try
            {
                var fromAddress = new MailAddress("noreplyflappybirdcs@gmail.com", "FlappyBird Team");
                var toAddress = new MailAddress(strTo);
                const string fromPassword = "FlappyBird14cs";
                //const string subject = "Recovery Password";
                // string body = "Automatic Email Sender";
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
                    Subject = strSubject,
                    Body = strBody,
                    
                    //IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
                flag = true;
               //FlappyLog.WriteEntry("Mail Send Successfully");
            }
            catch (Exception ex)
            {
                //FlappyLog.WriteEntry("Error occured: " + ex.ToString());
                //Response.Write(ex.Message);
                Console.WriteLine(ex.ToString());
            }

            return flag;
        }

        public void SearchUserEmailAndSend()
        {
            string myConnectionString = "server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msq = new MySqlConnection(myConnectionString);
            string queryS = "SELECT * FROM userflappy WHERE AutoMail = 1;";
            MySqlCommand msc = new MySqlCommand(queryS, msq);

            Console.WriteLine(queryS);
            msq.Open();
            MySqlDataReader reader = msc.ExecuteReader();
            string componeEmailUser = "";
            string emailUser = "";
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    emailUser = reader["email"].ToString();
                    componeEmailUser = emailUser + "|";
                    SendEmail(emailUser, "noreplyflappybirdcs@gmail.com", "Automatic Mail sending", "Successfully working contact with " + emailUser);
                }
            }
            reader.Close();
            //return componeEmailUser;
        }

        //disabilitato 
        /*private int getEmailNumber()
        {

            //SELECT count(*) FROM `userflappy` WHERE email is not null
            string myConnectionString = "server=localhost;database=flappy-bird;uid=root;pwd=;";
            MySqlConnection msq2 = new MySqlConnection(myConnectionString);
            string queryS = "SELECT count(*) FROM userflappy WHERE email is not null";
            MySqlCommand msc = new MySqlCommand(queryS, msq2);

            Console.WriteLine(queryS);
            msq2.Open();
            int nEmail = msc.ExecuteNonQuery();
            return nEmail;
        }*/
        
    }
}
