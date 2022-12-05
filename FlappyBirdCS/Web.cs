using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
namespace FlappyBirdCS
{
    public partial class Web : Form
    {
        public Web()
        {
            InitializeComponent();
        }
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private void buttons1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://salvuccif.altervista.org/");
            Log.Info("Apro Web Page\n");
        }
    }
}
