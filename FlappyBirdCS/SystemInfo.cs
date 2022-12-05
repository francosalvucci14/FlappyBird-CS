using FlappyBirdCS.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Policy;
using System.Reflection;
using System.Globalization;
using log4net;
namespace FlappyBirdCS
{
    public partial class SystemInfo : Form
    {
        public SystemInfo()
        {
            InitializeComponent();
            systemVersion();
        }
        string myConnectionString = Form3.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
        string version1,version2,build1,build2;
        // bool closed = true;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void buttons1_Click(object sender, EventArgs e)
        {
            /*if (closed)
            {
                
                closed = false;
                panel1.Width = 10;
            }
            else
            {
                panel1.Width = 376;
                closed = true;
            }*/
            UsedModules ud = new UsedModules();
            ud.Show();
        }

        //LOLIB //logger = new LOLIB();
    //string LogId = Form3.LogId;
        
        private void systemVersion()
        {
            try
            {
                MySqlConnection msq = new MySqlConnection(myConnectionString);
                string queryS = "SELECT * FROM version";
                MySqlCommand msc = new MySqlCommand(queryS, msq);
                //logger.WriteOnLog(LogId, "Prendo i dati per system info", 3);
                Console.WriteLine(queryS);
                msq.Open();
                MySqlDataReader reader = msc.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        version1 = reader["version1"].ToString();
                        version2 = reader["version2"].ToString();
                        build1 = reader["build1"].ToString();
                        build2 = reader["build2"].ToString();
                    }
                }
                reader.Close();

                //inserisco i dati di system info
                label5.Text = version1;
                label6.Text = version2;
                label7.Text = build1;
                label8.Text = build2;
            }          
            catch (Exception e) 
            {
                //logger.WriteErrorOnLog(LogId, 1, e, "Error");
                Log.Error("Error = " + e.Message + "\n==========");
            }
            
        }
       
    }
}
