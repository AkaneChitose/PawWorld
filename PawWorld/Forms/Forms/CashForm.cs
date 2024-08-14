using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PawWorld.Forms.Forms
{
    public partial class CashForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;
        
        MainForm main;

        string title = "Paw World 🐾";

        public CashForm(MainForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            main = form;
            getTranso();
            loadCash();
        }

        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCash.Columns[e.ColumnIndex].Name;
            removeitem:
            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this cash?", "Delete Cash", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbcon.executeQuery("DELETE FROM tbCash WHERE cashid LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    MessageBox.Show("Cash record has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (colName == "Increase")
            {
                int i = checkPqty(dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString());
                if (int.Parse(dgvCash.Rows[e.RowIndex].Cells[4].Value.ToString()) < i)
                {
                    dbcon.executeQuery("UPDATE tbCash SET qty = qty + " + 1 + " WHERE cashid LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                }
                else
                {
                    MessageBox.Show("Remaining quantity on hand is " + i + "!", "Out of Stock ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (colName == "Decrease")
            {
                if (int.Parse(dgvCash.Rows[e.RowIndex].Cells[4].Value.ToString()) == 1)
                {
                    colName = "Delete";
                    goto removeitem;
                }
                dbcon.executeQuery("UPDATE tbCash SET qty = qty - " + 1 + " WHERE cashid LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
            }
            loadCash();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CashProductForm product = new CashProductForm(this);
            product.uname = main.lbCashierName.Text;
            product.ShowDialog();
            
        }

        private void bthCash_Click(object sender, EventArgs e)
        {
            //CashCustomer customer = new CashCustomer(this);
            //customer.ShowDialog();

            if (MessageBox.Show("Are you sure you want to cash this product?", "Cashing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                getTranso();
                //main.loadDailySale();
                for (int i = 0; i < dgvCash.Rows.Count; i++)
                {
                    dbcon.executeQuery("UPDATE tbProduct SET pqty= pqty - " + int.Parse(dgvCash.Rows[i].Cells[4].Value.ToString()) + " WHERE pcode LIKE " + dgvCash.Rows[i].Cells[2].Value.ToString() + "");
                }
                dgvCash.Rows.Clear();
            }
        }

        #region
        public void getTranso()
        {
            try
            {
                string sdate = DateTime.Now.ToString("ddMMyyy");
                int count;
                string no;
                cn.Open();
                cm = new SqlCommand("Select TOP 1 transno FROM tbCash WHERE transno LIKE '" + sdate + "%' ORDER BY cashid DESC", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                

                if (dr.HasRows)
                {
                    no = dr[0].ToString();
                    count = int.Parse(no.Substring(8, 4));
                    lbTrans.Text = sdate + (count + 1);
                }
                else
                {
                    no = sdate + "1001";
                    lbTrans.Text = no;
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
        public void loadCash() 
        {
            try
            {
                double total = 0;
                int i = 0;
                dgvCash.Rows.Clear();
                //string searchQuery = "%" + tb.Text + "%";
                cm = new SqlCommand("SELECT cashid, pcode, pname, qty, price, total, c.ownerName, cashier FROM tbCash as cash LEFT JOIN tbCustomer c ON cash.cid = c.id WHERE transno LIKE '" + lbTrans.Text + "'", cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvCash.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                    total += double.Parse(dr[5].ToString());
                }
                dr.Close();
                cn.Close();
                lbTotal.Text = total.ToString("#,##0.000");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        public int checkPqty(string pcode)
        {
            int i = 0;
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT pqty FROM tbProduct WHERE pcode LIKE '" + pcode + "'", cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
            return i;
        }
        #endregion
    }
}
