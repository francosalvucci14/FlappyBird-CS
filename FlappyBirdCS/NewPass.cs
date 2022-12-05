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
using log4net;
namespace FlappyBirdCS
{
    public partial class NewPass : Form
    {
        string emailUser = RecPassword.to;
        string connString = Form3.connString;//"server=localhost;database=flappy-bird;uid=root;pwd=;";
        //LOLIB //logger = new LOLIB();
        //string LogId = Form3.LogId;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public NewPass()
        {
            InitializeComponent();
        }

        private void buttons1_Click(object sender, EventArgs e)
        {
            if (textNewP.Text == textNewC.Text)
            {
                MySqlConnection mscR = new MySqlConnection(connString);
                string queryUpdatePassword = "UPDATE userflappy SET Password = '" +textNewP.Text + "' WHERE Email = '" + emailUser+ "';";
                mscR.Open();
                MySqlCommand com = new MySqlCommand(queryUpdatePassword,mscR);
                com.ExecuteNonQuery();
                mscR.Close();
                //logger.WriteOnLog(LogId, "Set new password to the user", 3);
                ////loggerS.WriteOnLogSetup(LogIdS, "Set new password to the user", 3);
                MessageBox.Show("Reset successful");
                Log.Info("Reset password avvenuto con successo\n");
                this.Close();
            }
            else
            {
                MessageBox.Show("Error, passwords must be identical\nPlease try again");
                Log.Error("La password deve essere uguale\nRiprova" + "\n==========");
                //logger.WriteOnLog(LogId, "Error : Password are invalid", 1);
                ////loggerS.WriteOnLogSetup(LogIdS, "Error : Password are invalid", 1);
            }
        }

        private void changePassText(object sender, EventArgs e)
        {
            textNewC.PasswordChar = '*';
            textNewP.PasswordChar = '*';
        }
    }
}
