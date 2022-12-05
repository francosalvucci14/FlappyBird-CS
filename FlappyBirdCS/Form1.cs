using FlappyBirdCS.Manager;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace FlappyBirdCS
{
    public partial class Form1 : Form
    {

        //SMSCOMMS SMSEngine;
        int pipeSpeed = 8;
        int gravity = 1;
        int score = 0;
        int max = 0;
        string UserID;
        string username;
        static string server = Form3.dbServer;
        static string host = Form3.dbHost;
        static string dbUser = Form3.dbUser;
        static string dbPass = Form3.dbPass;

        //string myConnectionString = "server=localhost;database=flappy-bird;uid=root;pwd=;";

        string record;
        //string db;
        //LOLIB //logger = new LOLIB();
        //string LogId = Form3.LogId;
        /*LOLIB //loggerS = new LOLIB();
        string LogIdS = Form3.LogIdS;*/
        string appFilesPath = Form3.appFilesPath;

        string myConnectionString = $"server={server}; database={host};uid={dbUser};pwd={dbPass};";
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        //[DllImport("winmm.dll")]
        //static extern Int32 mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        public Form1()
        {
            InitializeComponent();
            Form3.soundTrackGame();

            //string c = "";

            //mciSendString(@"open C:\Users\Jono\Desktop\foghorn.wav type waveaudio alias foghorn", null, 0, IntPtr.Zero);
            //mciSendString(@"play foghorn", null, 0, IntPtr.Zero);
        }
        
        
        private void gameTimerEvent(object sender, EventArgs e)
        {
            //Log.Info("Connection String : " + myConnectionString.ToString());
            
            FlappyBird.Top += gravity;
            PipeDown.Left -= pipeSpeed;
            PipeTop.Left -= pipeSpeed;
            PipeDown2.Left -= pipeSpeed;
            scoreText.Text = "Score: " + score.ToString();
            speedLabel.Text = "Speed: " + pipeSpeed.ToString();
            //speedLabel.Text = "Record: " + max.ToString();
            userLabel.Text = "User : " + Form4.ValueOfTxtBox1;
            //insert the record label to show the recrod of the user
            //soundtrackGame();
            //File.WriteAllText(@"C:\Program Files (x86)\Salvucci Franco\DemoNewSetup\Data2.txt", $"THIRD\nServer={server}, Host={host}, User={dbUser}, Password={dbPass}");
            if (PipeDown.Left < -150) 
            {
                PipeDown.Left = 350;
                score++;
                soundScore();
            }
            if (PipeTop.Left < -180)
            { 
                PipeTop.Left = 415;
                score++;
                soundScore();
            }
            if (PipeDown2.Left < -200)
            {
                PipeDown2.Left = 480;
                score++;
                soundScore();
            }
         
            if (FlappyBird.Bounds.IntersectsWith(PipeDown.Bounds) || 
                FlappyBird.Bounds.IntersectsWith(PipeTop.Bounds) || 
                FlappyBird.Bounds.IntersectsWith(Ground.Bounds) || FlappyBird.Top < -25 || 
                FlappyBird.Bounds.IntersectsWith(PipeDown2.Bounds))
            {
                endGame();
            }
           
            if (score >= 10)
            {
                pipeSpeed = 15;
            }
            if (score >= 25)
            {
                pipeSpeed = 30;
            }
            

        }
        
        private void gamekeyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                gravity = -15;
                
            }
        }

        private void gamekeyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                gravity = 15;
                
            }
        }
        
        private void endGame()
        {
            gameTimer.Stop();
            //popupNotifier1.TitleText = "Game Over!";
            //popupNotifier1.ContentText = "Oh no! :( \nAnother game?";
            //popupNotifier1.IsRightToLeft = false;

            //popupNotifier1.Popup();
            string cry = "\uD83D\uDE22"; //
            notifyIcon1.ShowBalloonTip(1000,"Game Over", "Oh no " + cry + "\nAnother Game??", ToolTipIcon.Info);
            scoreText.Text += " Game Over!!!";
            //logger.WriteOnLog(LogId, "Game Over", 3);
            ////loggerS.WriteOnLogSetup(LogIdS, "Game Over", 3);

            try
            {
                // SoundPlayer spe = new SoundPlayer(appFilesPath + @"\Sound\sfx_hit.wav");
                //spe.Play();
                var p2 = new System.Windows.Media.MediaPlayer();

                p2.Open(new Uri(appFilesPath + @"\Sound\sfx_hit.wav"));
                p2.Play();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                MessageBox.Show(e.StackTrace);
                Console.WriteLine(e.InnerException);
                MessageBox.Show(e.Source);
                //logger.WriteErrorOnLog(LogId, 1, e, "Error");
               // //loggerS.WriteErrorOnLogSetup(LogIdS, 1, e, "Error");
            }
            mexRecord();
            MessageBox.Show(scoreText.Text);
            Log.Info("UTENTE : [" + Form4.ValueOfTxtBox1 + "] - SCORE : [" + score.ToString() + "]");
            mexRetryYN();
            

        }
        private void soundScore()
        {
            try
            {
                //SoundPlayer sp = new SoundPlayer(appFilesPath + @"\Sound\sfx_point.wav");
                //sp.Play();
                var p2 = new System.Windows.Media.MediaPlayer();

                p2.Open(new Uri(appFilesPath + @"\Sound\sfx_point.wav"));                
                p2.Play();
                // System.Threading.Thread.Sleep(1);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                MessageBox.Show(e.StackTrace);
                Console.WriteLine(e.InnerException);
                MessageBox.Show(e.Source);
                //logger.WriteErrorOnLog(LogId, 1, e, "Error");
                Log.Error("Error = " + e.Message + "\n ==========");
                ////loggerS.WriteErrorOnLogSetup(LogId, 1, e, "Error");
            }
            
        }

        private void soundtrackGame()
        {
            try
            {
                //SoundPlayer sp = new SoundPlayer(appFilesPath + @"\Sound\soundtrack.wav");
                // sp.Play();
                
                //da provare
                const bool loopPlayer = true;
                var p1 = new System.Windows.Media.MediaPlayer();
                
                    p1.Open(new Uri(appFilesPath + @"\Sound\soundtrack.wav"));
                    if (loopPlayer)
                        p1.MediaEnded += MediaPlayer_Loop;
                    p1.Play();
                    
                
                
                //p1.Open(new System.Uri(@"C:\windows\media\tada.wav"));
                //p1.Play();

                //// this sleep is here just so you can distinguish the two sounds playing simultaneously
                //System.Threading.Thread.Sleep(500);

                //var p2 = new System.Windows.Media.MediaPlayer();
                //p2.Open(new System.Uri(@"C:\windows\media\tada.wav"));
                //p2.Play();
                //System.Threading.Thread.Sleep(100);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                MessageBox.Show(e.StackTrace);
                Console.WriteLine(e.InnerException);
                MessageBox.Show(e.Source);
                Log.Error("Error = " + e.Message + "\n ==========");
            }

        }
        void MediaPlayer_Loop(object sender, EventArgs e)
        {
            MediaPlayer player = sender as MediaPlayer;
            if (player == null)
                return;

            player.Position = new TimeSpan(0);
            player.Play();
        }

        private void mexRetryYN()
        {
            string message = "Retry?";
            string title = "Game Over";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                Application.Restart();
                //logger.WriteOnLog(LogId, "Restart", 3);
                ////loggerS.WriteOnLogSetup(LogIdS, "Restart", 3);
                Log.Info("Restart\n");
            }
            else
            {
                Application.Exit();
                //logger.WriteOnLog(LogId, "Exit Application", 3);
                Log.Info("Esco\n==========ARRIVEDERCI==========");
                ////loggerS.WriteOnLogSetup(LogIdS, "Exit Application", 3);
            }

        }
        
        private void mexRecord()
        {
            MySqlConnection msq = new MySqlConnection(myConnectionString);
            string queryS = "SELECT * FROM userflappy WHERE userflappy.Name = '" + Form4.ValueOfTxtBox1 + "';";
            MySqlCommand msc = new MySqlCommand(queryS,msq);
            
            Console.WriteLine(queryS);
            msq.Open();
            MySqlDataReader reader = msc.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                     record = reader["Record"].ToString();
                    UserID = reader["UID"].ToString();
                    username = reader["Name"].ToString();
                }
            }
            reader.Close();
            /*adapter = new MySqlDataAdapter(queryS, msq);
            adapter.Fill(table);
            Console.WriteLine("Record : " + adapter.ToString());

            if (table.Rows.Count <= 0)
            {
                MessageBox.Show("No record set for this user");
            }
            else
            {
                MessageBox.Show("Record found");
                //prelevare il valore del campo record per poi fare l'if
                MySqlCommand mscS = msq.CreateCommand();
                mscS.CommandText = "SELECT Record FROM userflappy WHERE userflappy.Name = '" + Form4.ValueOfTxtBox1 + "';";
                MySqlDataReader reader =  mscS.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        record = reader["Record"].ToString();
                    }
                }

            }*/
            string message = "Set the record?";
            string title = "Set Record";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                MySqlDataAdapter adapter;

                DataTable table = new DataTable();
                if (score > int.Parse(record))//cambiare max nel valore del campo record
                {
                    max = score;
                    
                    /*con.Open();
                    string query = "UPDATE userflappy SET Record = " + max + " WHERE userflappy.Name = " + Form4.ValueOfTxtBox1 + ";";
                    string queryS = "SELECT Record FROM userflappy WHERE userflappy.Name = " + Form4.ValueOfTxtBox1 + ";";
                    */
             
                    
                    try
                    {
                        string queryI = "INSERT INTO recordxuser(RID,UserID,RecordN,Username) VALUES (NULL, " + UserID + ", " + score + ", '" + username + "')";
                        string queryU = "UPDATE userflappy SET Record = @score WHERE userflappy.Name = @user";
                        MySqlCommand mscU = new MySqlCommand(queryU,msq);
                        //UPDATE `userflappy` SET `Record` = '5' WHERE `userflappy`.`UID` = 2; command for update the table to set the record of the user. --change the UID and the Record obs
                        mscU.Parameters.AddWithValue("@user", Form4.ValueOfTxtBox1);
                        mscU.Parameters.AddWithValue("@score", max);
                        Console.WriteLine(mscU.CommandText.ToString());
                        Console.WriteLine(max.ToString());
                        mscU.ExecuteNonQuery();

                        string updateRecord = "UPDATE recordxuser SET RecordN = " + score + " WHERE UserID = '" + UserID + "';";
                        string querySelect = "SELECT * FROM recordxuser WHERE Username = '" + username + "'";
                        adapter = new MySqlDataAdapter(querySelect, msq);
                        adapter.Fill(table);

                        if (table.Rows.Count <= 0)
                        {
                            MySqlCommand mscI = new MySqlCommand(queryI, msq);
                            Console.WriteLine(mscI.CommandText.ToString());
                            mscI.ExecuteNonQuery();
                            //logger.WriteOnLog(LogId, "New Record", 3);
                            // //loggerS.WriteOnLogSetup(LogIdS, "New Record", 3);
                            Log.Info("Nuovo Record\n UTENTE : " + username + " --- PUNTEGGIO : " + score);

                        }
                        else
                        {
                            MySqlCommand UpdateRecord = new MySqlCommand(updateRecord, msq);
                            Console.WriteLine(UpdateRecord.CommandText.ToString());
                            UpdateRecord.ExecuteNonQuery();
                            //logger.WriteOnLog(LogId, "Update Record", 3);
                            Log.Info("Update del Record");
                           // //loggerS.WriteOnLogSetup(LogIdS, "Update Record", 3);

                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        Console.WriteLine(ex.Source);
                        Console.WriteLine(ex.InnerException);
                        //logger.WriteErrorOnLog(LogId, 1, ex, "Error");
                        Log.Error("Error = " + ex.Message + "\n ==========");
                       // //loggerS.WriteErrorOnLogSetup(LogIdS, 1, ex, "Error");
                    }
                    finally
                    {
                        if (msq.State == ConnectionState.Open)
                        {
                            msq.Close();
                        }
                    }
                }
                else if(score <= int.Parse(record))
                {
                    MessageBox.Show("Punteggio più basso del record precedente");
                    max = 0;
                }

            }
           
            

        }

        private void loadGame(object sender, EventArgs e)
        {
            //soundtrackGame();
            
           
        }

        
    }
}
