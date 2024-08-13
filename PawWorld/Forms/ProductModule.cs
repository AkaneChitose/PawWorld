using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace PawWorld.Forms
{
    public partial class ProductModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();

        string title = "Paw World 🐾";
        bool check = false;

        ProductForm product;

        public ProductModule(ProductForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            product = form;
            cbCategory.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(dbcon.connection()))
            {
                try
                {
                    CheckField();

                    if (check)
                    {
                        string newPcode = lbPcode.Text; // Assuming the code is already set

                        if (MessageBox.Show("Bạn thực sự muốn sửa sản phẩm này?", "Sửa sản phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            // Prepare the SQL command
                            using (SqlCommand cm = new SqlCommand("UPDATE tbProduct SET pname=@pname, ptype=@ptype, pcategory=@pcategory, pqty=@pqty, pprice=@pprice WHERE pcode=@pcode", cn))
                            {
                                cm.Parameters.AddWithValue("@pcode", newPcode);
                                cm.Parameters.AddWithValue("@pname", tbName.Text);
                                cm.Parameters.AddWithValue("@ptype", cbCategory.Text);
                                cm.Parameters.AddWithValue("@pcategory", tbType.Text);
                                cm.Parameters.AddWithValue("@pqty", (int)nudQty.Value);

                                // Parse and format the price correctly
                                string priceText = tbPrice.Text.Replace(" VND", "").Replace(",", "").Trim();
                                if (double.TryParse(priceText, out double price))
                                {
                                    cm.Parameters.AddWithValue("@pprice", price);
                                }
                                else
                                {
                                    MessageBox.Show("Giá không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                // Open connection and execute the command
                                cn.Open();
                                int rowsAffected = cm.ExecuteNonQuery();

                                // Check if any rows were affected
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Sửa thành công!", title);
                                    product.LoadProduct();
                                    this.Dispose();
                                }
                                else
                                {
                                    MessageBox.Show("Không tìm thấy sản phẩm để cập nhật.", title);
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

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #region Method


        public void CheckField()
        {
            if (tbName.Text == "" | tbPrice.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo");
                return;
            }
            check = true;
        }

        private string GenerateUniqueID()
        {
            Random rnd = new Random();
            string newPcode;
            bool isUnique;

            do
            {
                newPcode = rnd.Next(80000000, 89999999).ToString();
                isUnique = CheckIDUnique(newPcode);
            } while (!isUnique);

            return newPcode;
        }

        private bool CheckIDUnique(string id)
        {
            bool isUnique = false;
            try
            {
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM tbUser WHERE id = @id", cn);
                checkCmd.Parameters.AddWithValue("@id", id);
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

        private void tbPrice_TextChanged(object sender, EventArgs e)
        {
            // Remove the "VND" suffix and any existing commas to parse the number correctly
            string input = tbPrice.Text.Replace(" VND", "").Replace(",", "").Trim();

            if (double.TryParse(input, out double value))
            {
                // Format the number with commas and add the "VND" suffix
                tbPrice.Text = String.Format("{0:N0} VND", value);

                // Correct the cursor position after formatting
                tbPrice.SelectionStart = tbPrice.Text.Length - 4; // "- 4" accounts for the " VND" suffix
            }
        }

        #endregion Method

        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();
                if (check)
                {
                    // Assuming lbPcode.Text contains the product code to update
                    string pcode = lbPcode.Text;

                    if (MessageBox.Show("Bạn thực sự muốn sửa sản phẩm này?", "Sửa sản phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (SqlCommand cm = new SqlCommand("UPDATE tbProduct SET pname=@pname, ptype=@ptype, pcategory=@pcategory, pqty=@pqty, pprice=@pprice WHERE pcode=@pcode", cn))
                        {
                            cm.Parameters.AddWithValue("@pcode", pcode);
                            cm.Parameters.AddWithValue("@pname", tbName.Text);
                            cm.Parameters.AddWithValue("@ptype", cbCategory.Text);
                            cm.Parameters.AddWithValue("@pcategory", tbType.Text);
                            cm.Parameters.AddWithValue("@pqty", (int)nudQty.Value);

                            // Parse and format the price correctly
                            string priceText = tbPrice.Text.Replace(" VND", "").Replace(",", "").Trim();
                            if (double.TryParse(priceText, out double price))
                            {
                                cm.Parameters.AddWithValue("@pprice", price);
                            }
                            else
                            {
                                MessageBox.Show("Giá không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Open connection and execute the command
                            cn.Open();
                            int rowsAffected = cm.ExecuteNonQuery();
                            cn.Close();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Sửa thành công!", title);
                                product.LoadProduct(); // Refresh product data
                                this.Dispose();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy sản phẩm để cập nhật.", title);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show($"Lỗi: {ex.Message}", title);
            }
        }
    }
}
