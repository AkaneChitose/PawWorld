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
    public partial class CashProductForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;

        string title = "Paw World 🐾";
        CashForm cash;

        public string uname;

        public CashProductForm(CashForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            cash = form;
            LoadProduct();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dgvProduct.Rows)
            {
                if (dr.IsNewRow) continue;

                // Check if the "Select" column exists and has a value
                if (dr.Cells["Select"] != null && dr.Cells["Select"].Value != null)
                {
                    bool chkbox = Convert.ToBoolean(dr.Cells["Select"].Value);
                    if (chkbox)
                    {
                        try
                        {
                            cm = new SqlCommand("INSERT INTO tbCash(transno, pcode, pname, qty, price, cashier) VALUES (@transno, @pcode, @pname, @qty, @price, @cashier)", cn);
                            cm.Parameters.AddWithValue("@transno", cash.lbTrans.Text);
                            //cm.Parameters.AddWithValue("@no", cash.lbTrans.Text);
                            cm.Parameters.AddWithValue("@pcode", dr.Cells[1].Value.ToString());
                            cm.Parameters.AddWithValue("@pname", dr.Cells[2].Value.ToString());
                            cm.Parameters.AddWithValue("@qty", 1);
                            cm.Parameters.AddWithValue("@price", Convert.ToDouble(dr.Cells[5].Value.ToString()));
                            cm.Parameters.AddWithValue("@cashier", uname);

                            cn.Open();
                            cm.ExecuteNonQuery();
                            cn.Close();
                        }
                        catch (Exception ex)
                        {
                            cn.Close();
                            MessageBox.Show(ex.Message, title);
                        }
                    }
                }

            }
            cash.loadCash();    
            this.Dispose();
        }
        #region
        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            string searchQuery = "%" + tbSearch.Text + "%";
            cm = new SqlCommand("SELECT pcode, pname, ptype, pcategory, pprice FROM tbProduct WHERE CONCAT(pcode, ptype, pcategory) LIKE '%" + tbSearch.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString());
            }
            dr.Close();
            cn.Close();
        }
        #endregion
    }
}
