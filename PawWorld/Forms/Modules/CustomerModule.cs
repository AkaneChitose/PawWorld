using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PawWorld.Forms.Modules
{
    public partial class CustomerModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Paw World 🐾";
        bool check = false;

        CustomerForm customerForm;

        public CustomerModule(CustomerForm customer)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            customerForm = customer;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();

                if (check)
                {
                    string newCcode = GenerateUniqueID();
                    lbCID.Text = newCcode;

                    if (MessageBox.Show("Bạn thực sự muốn thêm sản phẩm này?", "Thêm sản phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // Prepare the SQL command for insertion
                        using (SqlCommand cm = new SqlCommand("INSERT INTO tbCustomer (cid, ownerName, petName, type, phoneNumber, location, dateOfReceipt) VALUES (@cid, @ownerNname, @petName, @type, @phoneNumber, @location, @dateOfReceipt)", cn))
                        {
                            cm.Parameters.AddWithValue("@cid", newCcode);
                            cm.Parameters.AddWithValue("@ownerNname", tbNameCustomer.Text);
                            cm.Parameters.AddWithValue("@petName", tbPetName.Text);
                            cm.Parameters.AddWithValue("@phoneNumber", tbPhoneNumber.Text);
                            cm.Parameters.AddWithValue("@location", tbAddress.Text);
                            cm.Parameters.AddWithValue("@dateOfReceipt", dtpDoR.Value);
                            cm.Parameters.AddWithValue("@type",cbType.SelectedIndex);


                            // Open connection and execute the command
                            cn.Open();
                            int rowsAffected = cm.ExecuteNonQuery();
                            cn.Close();

                            // Check if any rows were affected
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Thêm thành công!", title);
                                customerForm.LoadCustomer();
                                this.Dispose();
                            }
                            else
                            {
                                MessageBox.Show("Không thể thêm khách hàng, vui lòng thử lại", title);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", title);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();

                if (check)
                {
                    string cid = lbCID.Text;  // Use the existing cid instead of generating a new one

                    if (MessageBox.Show("Bạn thực sự muốn sửa thông tin khách hàng này?", "Sửa sản phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // Prepare the SQL command for updating
                        using (SqlCommand cm = new SqlCommand("UPDATE tbCustomer SET ownerName = @ownerName, petName = @petName, type = @type, phoneNumber = @phoneNumber, location = @location, dateOfReceipt = @dateOfReceipt WHERE cid = @cid", cn))
                        {
                            cm.Parameters.AddWithValue("@cid", cid);
                            cm.Parameters.AddWithValue("@ownerName", tbNameCustomer.Text);  // Corrected parameter name
                            cm.Parameters.AddWithValue("@petName", tbPetName.Text);
                            cm.Parameters.AddWithValue("@phoneNumber", tbPhoneNumber.Text);
                            cm.Parameters.AddWithValue("@location", tbAddress.Text);
                            cm.Parameters.AddWithValue("@type", cbType.SelectedIndex);
                            cm.Parameters.AddWithValue("@dateOfReceipt", dtpDoR.Value);

                            // Open connection and execute the command
                            cn.Open();
                            int rowsAffected = cm.ExecuteNonQuery();
                            cn.Close();

                            // Check if any rows were affected
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Cập nhật thành công!", title);
                                customerForm.LoadCustomer();
                                this.Dispose();
                            }
                            else
                            {
                                MessageBox.Show("Không thể cập nhật khách hàng, vui lòng thử lại", title);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", title);
            }

        }
        #region
        public void CheckField()
        {
            if (tbPetName.Text == "" | tbPhoneNumber.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo");
                return;
            }
            check = true;
        }

        private string GenerateUniqueID()
        {
            Random rnd = new Random();
            string newCcode;
            bool isUnique;

            do
            {
                newCcode = rnd.Next(80000000, 99999999).ToString();
                isUnique = CheckIDUnique(newCcode);
            } while (!isUnique);

            return newCcode;
        }

        private bool CheckIDUnique(string pcode)
        {
            bool isUnique = false;
            try
            {
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM tbProduct WHERE pcode = @pcode", cn);
                checkCmd.Parameters.AddWithValue("@pcode", pcode);
                cn.Open();
                int count = (int)checkCmd.ExecuteScalar();
                cn.Close();
                isUnique = count == 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
                cn.Close();
            }
            return isUnique;
        }
        #endregion

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
