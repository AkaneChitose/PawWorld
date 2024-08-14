using PawWorld.Forms;
using PawWorld.Forms.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PawWorld
{
    public partial class MainForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;

        string title = "Paw World 🐾";
        public MainForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            loadDailySale(this);
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

        #region func
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
        public void loadDailySale(Form childForm)
        {
            string sdate = DateTime.Now.ToString("ddMMyyyy");

            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT ISNULL(SUM(total), 0) AS total FROM tbCash WHERE transno LIKE '" +sdate + "%'", cn);
                cm.Parameters.AddWithValue("@price", sdate);
                lbTotalRecord.Text = double.Parse(cm.ExecuteReader().ToString()).ToString("#,##0.00");
                
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
            //return data;
        }
        
        #endregion func

        private void pnChild_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            openChildForm(new UserForm());
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerForm());
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Dispose();
            LoginForm login = new LoginForm();
            login.ShowDialog();
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            openChildForm(new CashForm(this));
        }

        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            openChildForm(new DashBoard());
        }
    }
}
