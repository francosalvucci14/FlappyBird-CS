using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistryKeyTool
{
    public partial class Form1 : Form
    {
        string server, user, db, pass;
        string registryPath;
        public Form1()
        {
            InitializeComponent();
        }

        private void changeColor(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                passTXT.BackColor = Color.Black;
            }
            else
            {
                passTXT.BackColor = Color.White;
            }
            
        }

        private void exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void save(object sender, EventArgs e)
        {
            
            server = serverTXT.Text.ToString();
            db = dbTXT.Text.ToString();
            user = userTXT.Text.ToString();
            pass = passTXT.Text.ToString();
            registryPath = regeditTXT.Text.ToString();
            if (server == "")
            {
                MessageBox.Show("Server is null","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                
            }
            if (db == "")
            {
                MessageBox.Show("Database is null","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            if (user == "")
            {
                MessageBox.Show("User is null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            if (passTXT.BackColor != Color.Black)
            {
                    MessageBox.Show("Password is null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            
            
            if (registryPath == "")
            {
                MessageBox.Show("Select any Registry Key Path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            //MessageBox.Show(registryPath);
            try
            {
                RegistryKey key = Registry.LocalMachine.CreateSubKey(registryPath);
                /*key.DeleteValue("Database");
                key.DeleteValue("Server");
                key.DeleteValue("User");
                key.DeleteValue("Password");*/
                key.SetValue("IP", server);
                key.SetValue("DB", db);
                key.SetValue("User", user);
                key.SetValue("DbPassword", pass);
                key.Close();
                MessageBox.Show("Registry Key changed","Done",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Exception : " + ex.Message);
                
            }
        }

        private void reset(object sender, EventArgs e)
        {
            serverTXT.Text = "";
            dbTXT.Text = "";
            userTXT.Text = "";
            passTXT.Text = "";
            regeditTXT.Text = "";
        }
    }
}
