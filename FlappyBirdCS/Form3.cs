using FlappyBirdCS.Manager;
using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using log4net;
using Tulpep.NotificationWindow;
using System.Globalization;
using System.Threading;
using Microsoft.Win32;
using System.Windows.Media;
//using SalvucciFranco.GetUserData;
namespace FlappyBirdCS
{

    public partial class Form3 : Form
    {
        int pw;
        bool hided;
        //Declare an instance for log4net
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //LOLIB //logger = new LOLIB();
        //public static string LogId = LOLIB.CodeGen("FlappyBirdCS");
        public static string appFilesPath = Path.GetDirectoryName(Application.ExecutablePath);



        public static string dbServer = "";
        public static string dbHost = "";
        public static string dbUser = "";
        public static string dbPass = "";
        //string rootTXT;
        public static string connString;
        /**/
        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SalvucciFranco\FlappyBirdCS\Connection",true);
        const bool loopPlayer = true;
        static MediaPlayer p1 = new System.Windows.Media.MediaPlayer();
        public Form3()
        {
            InitializeComponent();
            soundTrackGame();
            //loadData();
            pw = panel1.Width;
            hided = false;

            //AppHelper.Save("127.0.0.1", null, null, null);
            //AppHelper a = new AppHelper();

            //connString = AppHelper.LoadData();
            /*dbServer = AppInstaller.server;
            dbHost = AppInstaller.host;
            dbUser = AppInstaller.user;
            dbPass = AppInstaller.pass;*/
            //try
            //{
            //    dbServer = key.GetValue("Server").ToString();
            //    dbHost = key.GetValue("Database").ToString();
            //    dbUser = key.GetValue("User").ToString();
            //    dbPass = key.GetValue("Password").ToString();
            //    key.Close();

            //}
            //catch(Exception e)
            //{
            //    MessageBox.Show(e.Message,"Errore",MessageBoxButtons.OK,MessageBoxIcon.Error);
            //}
            dbServer = "127.0.0.1";
            dbHost = "flappy-bird";
            dbUser = "root";
            dbPass = "";

            /*if (File.Exists(rootTXT))
            {
                connString = File.ReadAllText(rootTXT);
            }*/
            connString = $"Server={dbServer};database={dbHost};uid={dbUser};pwd={dbPass}";
            Log.Info(connString);
            //MessageBox.Show($"Dati : {dbServer} -- Form3Host : {dbHost}" );

        }
       /* public Form3(string server,string host,string user,string pass)
        {
            
            connString = $"server={dbServer};database={dbHost};uid={dbUser};pwd={dbPass}";
            File.WriteAllText(@"C:\Program Files (x86)\Salvucci Franco\DemoNewSetup\InfoData2.txt", $"Server={dbServer};database={host};uid={user};pwd={pass}\n" + connString);
        }*/
        Buttons.Buttons buttons = new Buttons.Buttons();
        string Admin;
        
        
        public static void soundTrackGame()
        {
            try
            {
                //SoundPlayer sp = new SoundPlayer(@"C:\Users\FSalvucci\source\repos\FlappyBirdCS\FlappyBirdCS\Sound\soundtrack.wav");
                //sp.PlayLooping();
                //test
                

                p1.Open(new Uri(appFilesPath + @"\Sound\soundtrack.wav"));
                if (loopPlayer)
                    p1.MediaEnded += MediaPlayer_Loop;
                p1.Play();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.Source);
                MessageBox.Show("Error : " + e.Message + "\n");
                //logger.WriteErrorOnLog(LogId,1,e,"Error");
                Log.Error(e.Message + "\n==========");
                //loggerS.WriteErrorOnLogSetup(LogIdS, 1, e, "Error");
            }

        }
        static void MediaPlayer_Loop(object sender, EventArgs e)
        {
            MediaPlayer player = sender as MediaPlayer;
            if (player == null)
                return;

            player.Position = new TimeSpan(0);
            player.Play();
        }
        private void buttons1_Click(object sender, EventArgs e)
        {
            //open form to start game
            Form1 f1 = new Form1();
            f1.Show();
            Log.Info("Apro form 1 per iniziare il gioco\n");
            //logger.WriteOnLog(LogId,"Apro form 1 per iniziare il gioco",3);
            //loggerS.WriteOnLogSetup(LogIdS, "Apro form 1 per iniziare il gioco", 3);
        }

        private void buttons2_Click(object sender, EventArgs e)
        {
            //exit application
            Application.Exit();
            Log.Info("Esco dal gioco\n==========ARRIVEDERCI==========");

            //logger.WriteOnLog(LogId, "Esco dal gioco", 3);
            //loggerS.WriteOnLogSetup(LogIdS, "Esco dal gioco", 3);


        }

        private void buttons3_Click(object sender, EventArgs e)
        {
            //open my portfolio web
            Web wb = new Web();
            wb.Show();
            //logger.WriteOnLog(LogId, "Apro form web", 3);

            Log.Info("Apro la webpage\n");
            //System.Diagnostics.Process.Start("http://salvuccif.altervista.org/");
            //logger.WriteOnLog(LogId, "Apro form per visualizzazione web", 3);

        }

        private void buttons4_Click(object sender, EventArgs e)
        {
            //open form to login/registration

            string message = "Are you logged in?";
            string title = "Login/Registration";
            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                Form4 f4 = new Form4();
                f4.Show();
                Log.Info("Apro form per login\n");
                //logger.WriteOnLog(LogId, "Apro form per login", 3);
                //loggerS.WriteOnLogSetup(LogIdS, "Apro form per login", 3);
            }
            else if (result == DialogResult.No)
            {
                Registration r = new Registration();
                r.Show();
                Log.Info("Apro form per registrazione\n");
                //logger.WriteOnLog(LogId, "Apro form per registrazione", 3);
                //loggerS.WriteOnLogSetup(LogIdS, "Apro form per registrazione", 3);
            }

        }

        private void muteAudio(object sender, EventArgs e)
        {
            Log.Info("Muto l'audio\n");
            //logger.WriteOnLog(LogId, "Muto l'audio", 3);
            ///loggerS.WriteOnLogSetup(LogIdS, "Muto l'audio", 3);

            try
            {
                p1.Stop();
                /*SoundPlayer sp = new SoundPlayer(appFilesPath + @"\Sound\soundtrack.wav");
                sp.Stop();*/
            }
            catch (Exception es)
            {
                Console.WriteLine(es.Message);
                Console.WriteLine(es.StackTrace);
                Console.WriteLine(es.InnerException);
                Console.WriteLine(es.Source);
                Log.Error(es.Message + "\n==========");
                //logger.WriteErrorOnLog(LogId, 1, es, "Error");
                // //loggerS.WriteErrorOnLogSetup(LogIdS, 1, es, "Error");
            }


        }

        private void unMuteAudio(object sender, EventArgs e)
        {
            Log.Info("Riattivo audio\n");
            //logger.WriteOnLog(LogId, "Riattivo l'audio", 3);
            //loggerS.WriteOnLogSetup(LogIdS, "Riattivo l'audio", 3);
            try
            {
                /*SoundPlayer sp = new SoundPlayer(appFilesPath + @"\Sound\soundtrack.wav");
                sp.PlayLooping();*/
                p1.Open(new Uri(appFilesPath + @"\Sound\soundtrack.wav"));
                if (loopPlayer)
                    p1.MediaEnded += MediaPlayer_Loop;
                p1.Play();
            }
            catch (Exception es)
            {
                Console.WriteLine(es.Message);
                Console.WriteLine(es.StackTrace);
                Console.WriteLine(es.InnerException);
                Console.WriteLine(es.Source);
                Log.Error(es.Message + "\n==========");
                //logger.WriteErrorOnLog(LogId, 1, es, "Error");
                //loggerS.WriteErrorOnLogSetup(LogIdS, 1, es, "Error");
            }
        }

        private void buttons5_Click(object sender, EventArgs e)
        {
            //show the reord table
            RecordTable rt = new RecordTable();
            rt.Show();
            Log.Info("Apro record table\n");
            // //loggerS.WriteOnLogSetup(LogIdS, "Apro form per tabella dei record", 3);
            //logger.WriteOnLog(LogId, "Apro form per tabella dei record", 3);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            //after 72; 175
            //before 10; 175
            //button1.BackgroundImageLayout = ImageLayout.Stretch;
            if (hided)
            {
                panel1.Width = panel1.Width = 10;
                hided = false;
                panel1.Update();
                Image img = Image.FromFile(appFilesPath + @"\Images\hamburgerMenu-removebg-preview.png");
                Log.Info("Chiudo menu\n");
                //logger.WriteOnLog(LogId, "Chiudo menu", 3);
                // //loggerS.WriteOnLogSetup(LogIdS, "Apro menu", 3);
                pictureBox9.Image = img;
            }
            else
            {
                panel1.Width = panel1.Width = 72;
                hided = true;
                panel1.Update();
                Image img = Image.FromFile(appFilesPath + @"\Images\close-removebg-preview.png");
                Log.Info("Apro menu\n");
                //logger.WriteOnLog(LogId, "Apro menu", 3);
                //loggerS.WriteOnLogSetup(LogIdS, "Chiudo menu", 3);
                pictureBox9.Image = img;
            }
        }



        private void pictureBox8_Click(object sender, EventArgs e)
        {
            
            try
            {
                //logger.WriteOnLog(LogId, "Apro user control", 3);
                // //loggerS.WriteOnLogSetup(LogIdS, "Apro user control", 3);
                Log.Info("Apro user control\n");
                string myConnectionString = connString;//"Server=" + dbServer + ";database=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";";//"Server=localhost;database=flappy-bird;uid=root;pwd=;";
                MySqlConnection msq = new MySqlConnection(myConnectionString);
                //Log.Info("Connection string : " + myConnectionString);
                string queryS = "SELECT * FROM userflappy WHERE userflappy.Name = '" + Form4.ValueOfTxtBox1 + "';";
                MySqlCommand msc = new MySqlCommand(queryS, msq);

                Console.WriteLine(queryS);
                msq.Open();
                MySqlDataReader reader = msc.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Admin = reader["Admin"].ToString();

                    }
                }
                reader.Close();

                if (Admin == "1")
                {
                    ManagerUser mu = new ManagerUser();
                    mu.Show();
                    //logger.WriteOnLog(LogId, "Apro manager user", 3);
                    Log.Info("Apro manager user\n");
                    // //loggerS.WriteOnLogSetup(LogIdS, "Apro manager user", 3);
                }
                else
                {
                    MessageBox.Show("No admin privileges");
                    Log.Error("Error\nNo admin privileges" + "\n==========");
                    //logger.WriteOnLog(LogId, "Errore : Nessun privilegio admin", 1);
                    // //loggerS.WriteOnLogSetup(LogIdS, "Errore : Nessun privilegio admin", 1);
                }
            }
            catch (Exception Ex)
            {
                //logger.WriteErrorOnLog(LogId, 1, Ex, "Error");
                Log.Error(Ex.Message + "\n==========");
                // //loggerS.WriteErrorOnLogSetup(LogIdS, 1, Ex, "Error");
            }

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            //show the reord table
            SystemInfo si = new SystemInfo();
            si.Show();
            Log.Info("Apro system info\n");
            // //loggerS.WriteOnLogSetup(LogIdS, "Apro form per tabella dei record", 3);
            //logger.WriteOnLog(LogId, "Apro form per info sistema", 3);
        }

    }
}
