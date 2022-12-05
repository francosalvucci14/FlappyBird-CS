using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlappyBirdCS
{
    public class AppHelper
    {
        /* private static string _dbServer;
         private static string _dbHost;
         private static string _dbUser;
         private static string _dbPass;*/
        public static string DBserver;
        public static string DBhost;
        public static string DBuser;
        public static string DBpass;
        public static void Save(string server,string host,string user,string pass)
        {

            /*_dbHost = host;
            _dbServer = server;
            _dbUser = user;
            _dbPass = pass;*/
            
            /*DBserver = server;
            DBhost = host;
            DBuser = user;
            DBpass = pass;
            File.WriteAllText(@"C:\Program Files (x86)\Salvucci Franco\DemoNewSetup\InfoData.txt", $"Server={DBserver};database={DBhost};uid={DBuser};pwd={DBpass}");*/
        }
        public static string LoadData()
        {
            string rootTXT = @"C:\Program Files (x86)\Salvucci Franco\DemoNewSetup\InfoData.txt";
            
                // Read entire text file content in one string    
                string text = File.ReadAllText(rootTXT);
                return text;
            
           
        }
        
        /*public string dbServer
        {
            get => _dbServer; 
            set => _dbServer = value; 
            
        }

        public  string dbHost
        {
            get { return _dbHost; }
            set { _dbHost = value; }

        }
        public  string dbUser
        {
            get { return _dbUser; }
            set { _dbUser = value; }

        }
        public  string dbPass
        {
            get { return _dbPass; }
            set { _dbPass = value; }

        }*/
        /*public static string dbServer
        {
            get => _dbServer;
            set => _dbServer = value;

        }
        public static string dbHost
        {
            get => _dbHost;
            set => _dbHost = value;
        }
        public static string dbUser
        {
            get => _dbUser;
            set => _dbUser = value;
        }
        public static string dbPass
        {
            get => _dbPass;
            set => _dbPass = value;
        }*/
    }
}
