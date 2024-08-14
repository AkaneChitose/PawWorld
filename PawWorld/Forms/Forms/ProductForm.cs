using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PawWorld.Forms
{
    public partial class ProductForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;

        string title = "Paw World 🐾";

        public ProductForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadProduct();
        }

        public bool isFullScreen { get; private set; }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnFullScreen_Click(object sender, EventArgs e)
        {

        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductModule module = new ProductModule(this);
            module.ShowDialog();
        }

        public void dgvProductInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProductInfo.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ProductModule module = new ProductModule(this);
                module.lbPcode.Text = dgvProductInfo.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.tbType.Text = dgvProductInfo.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.cbCategory.Text = dgvProductInfo.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.tbName.Text = dgvProductInfo.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.tbPrice.Text = dgvProductInfo.Rows[e.RowIndex].Cells[6].Value.ToString();
                module.nudQty.Text = dgvProductInfo.Rows[e.RowIndex].Cells[5].Value.ToString();

                module.btnSave.Visible = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlCommand cm = new SqlCommand("DELETE FROM tbProduct WHERE pcode = @pcode", cn))
                        {
                            cm.Parameters.AddWithValue("@pcode", dgvProductInfo.Rows[e.RowIndex].Cells[1].Value.ToString());

                            cn.Open();
                            int rowsAffected = cm.ExecuteNonQuery();
                            cn.Close();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa sản phẩm thành công!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy sản phẩm để xóa.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cn.Close();
                    }
                }
            }
            LoadProduct();
        }




        #region Method
        public void LoadProduct()
        {
            int i = 0;
            dgvProductInfo.Rows.Clear();
            string searchQuery = "%" + tbSearch.Text + "%";
            cm = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(pcode, pname, pcategory, ptype) LIKE '%" + tbSearch.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProductInfo.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public int GetMaxProductId()
        {
            int maxPcode = 0;
            cm = new SqlCommand("SELECT MAX(pcode) FROM tbProduct", cn);
            cn.Open();
            object result = cm.ExecuteScalar();
            if (result != DBNull.Value)
            {
                maxPcode = Convert.ToInt32(result);
            }
            cn.Close();
            return maxPcode;
        }
        #endregion Method
    }
}
