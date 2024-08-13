using PawWorld.Forms.Modules;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PawWorld.Forms
{
    public partial class CustomerForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;

        string title = "Paw World 🐾";

        public CustomerForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadCustomer();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModule module = new CustomerModule(this);
            module.ShowDialog();
        }

        #region
        public void LoadCustomer()
        {
            int i = 0;
            dgvCustomerInfo.Rows.Clear();
            string searchQuery = "%" + tbSearch.Text + "%"; 
            cm = new SqlCommand("SELECT * FROM tbCustomer WHERE CONCAT(ownerName,petName,type,phoneNumber,location,dateOfReceipt) LIKE '%" + tbSearch.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCustomerInfo.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), DateTime.Parse(dr[6].ToString()).ToShortDateString());
            }
            dr.Close();
            cn.Close();
        }
        private void dgvCustomerInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Handle the error, possibly by showing a message box or logging the error
            MessageBox.Show("An error occurred while displaying data. Please check the data format.");
            e.ThrowException = false; // Suppresses the default error dialog
        }
        #endregion

        private void dgvCustomerInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCustomerInfo.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                CustomerModule module = new CustomerModule(this);
                module.lbCID.Text = dgvCustomerInfo.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.tbNameCustomer.Text = dgvCustomerInfo.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.tbPetName.Text = dgvCustomerInfo.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.cbType.Text = dgvCustomerInfo.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.tbPhoneNumber.Text = dgvCustomerInfo.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.tbAddress.Text = dgvCustomerInfo.Rows[e.RowIndex].Cells[6].Value.ToString();
                module.dtpDoR.Text = dgvCustomerInfo.Rows[e.RowIndex].Cells[7].Value.ToString();

                module.btnSave.Visible = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();

            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xoá khách hàng này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    try
                    {
                        using (SqlCommand cm = new SqlCommand("DELETE FROM tbCustomer WHERE cid = @cid", cn))
                        {
                            cm.Parameters.AddWithValue("@cid", dgvCustomerInfo.Rows[e.RowIndex].Cells[1].Value.ToString());

                            cn.Open();
                            int rowsAffected = cm.ExecuteNonQuery();
                            cn.Close();


                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Đã gỡ khách hàng khỏi hệ thống!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy khách hàng để xóa theo yêu cầu.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cn.Close();
                    }
            }
            LoadCustomer();
        }
    }
}
