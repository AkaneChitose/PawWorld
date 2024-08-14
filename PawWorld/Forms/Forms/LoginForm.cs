using PawWorld.Forms;
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
    public partial class LoginForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;

        string title = "Paw World 🐾";

        public LoginForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnForgotPassword_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Báo với chủ cửa hàng để được xử lí", "Forget Password", MessageBoxButtons.OK);
        }

        private void UserModule_Load(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnMinimized_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string _name = "", _role = "";
                cn.Open();
                cm = new SqlCommand("SELECT name, role FROM tbUser WHERE name=@name and password=@password", cn);
                cm.Parameters.AddWithValue("@name",tbName.Text);
                cm.Parameters.AddWithValue("@password", tbPassword.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    _name = dr["name"].ToString();
                    _role = dr["role"].ToString();
                    MessageBox.Show("Welcome "+_name+"|", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MainForm main = new MainForm();
                    main.lbCashierName.Text = _name;
                    main.lbRole.Text = _role;
                    if(_role == "Quản lí")
                    {
                        main.btnUser.Enabled = true;
                    }
                    this.Hide();
                    main.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Sai gòi bé ơi", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch(Exception ex) 
            { 
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }
    }
}
