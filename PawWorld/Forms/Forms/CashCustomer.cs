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
    public partial class CashCustomer : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;

        MainForm main;

        string title = "Paw World 🐾";

        CashForm cash;

        public CashCustomer(CashForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            cash = form;
            LoadCustomer();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
            
        }
        #region
        public void LoadCustomer()
        {

            try
            {
                int i = 0;
                dgvCustomer.Rows.Clear();
                cm = new SqlCommand("SELECT id,ownerName,phoneNumber FROM tbCustomer WHERE name LIKE '%" + tbSearch.Text + "%'", cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                dbcon.executeQuery("UPDATE tbCash SET cashid=" + dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString() + " WHERE transno=" + cash.lbTrans.Text + "");
                cash.loadCash();
                this.Dispose();
            }
        }
    }
}
