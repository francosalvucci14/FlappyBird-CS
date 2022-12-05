using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//using SalvucciFranco.GetUserData;
namespace FlappyBirdCS
{
    [RunInstaller(true)]
    public partial class AppInstaller : System.Configuration.Install.Installer
    {
        public AppInstaller()
        {
            InitializeComponent();

        }
        public static string server = "";
        public static string host = "";
        public static string user = "";
        public static string pass = "";
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
             server = Context.Parameters["Server_IP"];
            host = Context.Parameters["Host"];
           user = Context.Parameters["User"];
            pass = Context.Parameters["Password"];

            //bool check = AppHelper.Save(server,host,user,pass);

            //string con = Form3.SaveData(server,host,user,pass);
            //GetUserData.SaveData(server, host, user, pass);
            //Form3 f = new Form3(server,host,user,pass);
            //Program.a = server;
            //File.WriteAllText(@"C:\Program Files (x86)\Salvucci Franco\DemoNewSetup\InfoData3.txt", $"Server={server},Host={host},user={user},pass={pass}");
           AppHelper.Save(server, host, user, pass);
            
            /*AppHelper.dbServer = server;
            AppHelper.dbHost = host;
            AppHelper.dbUser = user;
            AppHelper.dbPass = pass;*/
            
           //MessageBox.Show($"Server={ server},Host={host},user={user},pass={pass}\n","Dati salvati correttamente");
            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\SalvucciFranco\FlappyBirdCS\Connection");
            key.SetValue("Database",host);
            key.SetValue("Server", server);
            key.SetValue("User", user);
            key.SetValue("Password",pass);
            key.Close();
        }
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\SalvucciFranco\FlappyBirdCS\Connection");
            key.DeleteValue("Database");
            key.DeleteValue("Server");
            key.DeleteValue("User");
            key.DeleteValue("Password");
            key.Close();
        }
    }
}
