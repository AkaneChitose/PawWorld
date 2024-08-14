using PawWorld.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PawWorld
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void progressBar_Click(object sender, EventArgs e)
        {
            
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = true;
            progressBar.Increment(2);
            if (progressBar.Value == 100)
            {
                timer.Enabled = false;
                MainForm form = new MainForm();
                form.Show();
                this.Hide();
            }
        }
    }
}
