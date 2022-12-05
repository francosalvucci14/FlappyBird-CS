using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlappyBirdCS
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        
        [STAThread]
        static void Main()
        {
            
            //Application.Run(new Form3());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            /*Process[] process = Process.GetProcessesByName(Application.ProductName); //Prevent multiple instance
            if (process.Length > 1)
            {
                MessageBox.Show("FlappyBirdCS  is already running. This instance will now close.", "{FlappyBirdCS}",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {
                Application.Run(new Form3());
            }*/
            Application.Run(new Form3());
        }
    }
}
