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
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized; // Change to full screen
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal; // Change to default (normal) state
            }
        }

        private void btnMinimized_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnStore_Click(object sender, EventArgs e)
        {
            openChildForm(new ProductForm());
            //lbTitle.Text = "CỬA HÀNG";
        }

        #region
        private Form activeForm = null;
        public void openChildForm(Form childForm)
        {
            if (activeForm != null)
            
            activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            activeForm.FormBorderStyle = FormBorderStyle.None;
            activeForm.Dock = DockStyle.Fill;
            //lbTitle.Text = childForm.Text;
            //lbWelcome.Visible = false;
            pnChild.Controls.Add(childForm);
            pnChild.Tag= childForm;
            childForm.Show();
        }
        #endregion

        private void pnChild_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            openChildForm(new UserForm());
        }
    }
}
