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

namespace PawWorld.Forms.Forms
{
    public partial class DashBoard : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;

        string title = "Paw World 🐾";

        public DashBoard()
        {
            InitializeComponent();
            cn =new SqlConnection(dbcon.connection());
        }

        private void guna2CustomGradientPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        #region

        public int extractData(string type)
        {
            int data = 0;
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT ISNULL(SUM(pqty), 0) AS qty FROM tbProduct WHERE ptype = @type", cn);
                cm.Parameters.AddWithValue("@type", type);
                data = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
            return data;


        }
        #endregion

        private void DashBoard_Load(object sender, EventArgs e)
        {
            lbDog.Text = extractData("Dog").ToString();
            lbCat.Text = extractData("Cat").ToString();
            lbBird.Text = extractData("Bird").ToString();
            lbHamster.Text = extractData("Hamster").ToString();

            //cn.Close();
        }
    }
}
