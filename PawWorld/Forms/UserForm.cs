using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PawWorld.Forms
{
    public partial class UserForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        SqlDataReader dr;

        string title = "Paw World 🐾";

        public UserForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadUser();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserModule module = new UserModule(this);
            module.ShowDialog();                        // Show Diaglog Events

        }

        private void dgvUserInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvUserInfo.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                UserModule module = new UserModule(this);
                module.lbUID.Text = dgvUserInfo.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.tbName.Text = dgvUserInfo.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.tbAddress.Text = dgvUserInfo.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.tbPhoneNumber.Text = dgvUserInfo.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.cbRole.Text = dgvUserInfo.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.dtpDoB.Text = dgvUserInfo.Rows[e.RowIndex].Cells[6].Value.ToString();
                module.tbPassword.Text = dgvUserInfo.Rows[e.RowIndex].Cells[7].Value.ToString();

                module.btnSave.Enabled = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
            if (MessageBox.Show("Bạn có thực sự muốn xóa nhân viên này?", "Xóa nhân viên", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // Using a parameterized query to prevent SQL injection
                    using (SqlCommand cm = new SqlCommand("DELETE FROM tbUser WHERE id = @id", cn))
                    {
                        cm.Parameters.AddWithValue("@id", dgvUserInfo.Rows[e.RowIndex].Cells[1].Value.ToString());

                        cn.Open();
                        int rowsAffected = cm.ExecuteNonQuery();
                        cn.Close();

                        // Check if any row was actually deleted
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa nhân viên thành công!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên để xóa.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cn.Close();
                }
            }
            LoadUser();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {

        }

        #region Method
        public void LoadUser()
        {
            int i = 0;
            dgvUserInfo.Rows.Clear();
            string searchQuery = "%" + tbSearch.Text + "%";
            cm = new SqlCommand("SELECT * FROM tbUser WHERE CONCAT(name,address,phone,dob,role) LIKE '%" + tbSearch.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvUserInfo.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }
        public int GetMaxUserId()
        {
            int maxId = 0;
            cm = new SqlCommand("SELECT MAX(id) FROM tbUser", cn);
            cn.Open();
            object result = cm.ExecuteScalar();
            if (result != DBNull.Value)
            {
                maxId = Convert.ToInt32(result);
            }
            cn.Close();
            return maxId;
        }
        #endregion Method
    }
}
