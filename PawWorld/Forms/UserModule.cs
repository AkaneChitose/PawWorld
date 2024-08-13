using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.Web.Security;

namespace PawWorld.Forms
{
    public partial class UserModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        string title = "Paw World 🐾";
        bool check = false;

        UserForm userForm;
        public UserModule(UserForm user)
        {
            InitializeComponent();
            cn =new SqlConnection(dbcon.connection());
            userForm = user;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRole.Text== "Tạp vụ")
            {
                lbPassword.Visible = false;
                tbPassword.Visible = false;
            }
            else
            {
                lbPassword.Visible = true;
                tbPassword.Visible = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();
                if (check)
                {
                    if (MessageBox.Show("Bạn có muốn sửa thông tin cho nhân viên này không?", "Sửa thông tin nhân viên", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE tbUSER SET name=@name, address=@address, phone=@phone, role=@role, dob=@dob, password=@password WHERE id=@id", cn);
                        cm.Parameters.AddWithValue("@name", tbName.Text);
                        cm.Parameters.AddWithValue("@address", tbAddress.Text);
                        cm.Parameters.AddWithValue("@phone", tbPhoneNumber.Text);
                        cm.Parameters.AddWithValue("@role", cbRole.Text);
                        cm.Parameters.AddWithValue("@dob", dtpDoB.Value);
                        cm.Parameters.AddWithValue("@password", tbPassword.Text);
                        cm.Parameters.AddWithValue("@id", lbUID.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Sửa thành công!", title);
                        Clear();
                        userForm.LoadUser();
                        this.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
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
                    string newId = GenerateUniqueID();
                    lbUID.Text = newId;

                    if (MessageBox.Show("Bạn thực sự muốn thêm nhân viên này?", "Thêm ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO tbUSER(id, name, address, phone, role, dob, password) VALUES(@id, @name, @address, @phone, @role, @dob, @password)", cn);
                        cm.Parameters.AddWithValue("@id", newId);
                        cm.Parameters.AddWithValue("@name", tbName.Text);
                        cm.Parameters.AddWithValue("@address", tbAddress.Text);
                        cm.Parameters.AddWithValue("@phone", tbPhoneNumber.Text);
                        cm.Parameters.AddWithValue("@role", cbRole.Text);
                        cm.Parameters.AddWithValue("@dob", dtpDoB.Value);
                        cm.Parameters.AddWithValue("@password", tbPassword.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Thêm thành công!", title);
                        Clear();
                        userForm.LoadUser();
                        this.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }
        #region Method
        public void Clear()
        {
            tbName.Clear();
            tbAddress.Clear();
            tbPassword.Clear();
            tbPhoneNumber.Clear();
            dtpDoB.Value = DateTime.Today;
            cbRole.SelectedIndex = 0;
        }

        //Kiểm tra trường nhập vào
        public void CheckField()
        {
            if (tbName.Text == "" | tbAddress.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo");
                return;
            }
            if (CheckAge(dtpDoB.Value) < 18)
            {
                MessageBox.Show("Không thể tuyển trẻ dưới vị thành niên, vui lòng thử lại", "Cảnh báo");
                return;
            }
            check = true;
        }
        //Kiểm tra tuổi

        private static int CheckAge(DateTime dateOfBirth)
        {
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;
            return age;
        }

        private string GenerateUniqueID()
        {
            Random rnd = new Random();
            string newId;
            bool isUnique;

            do
            {
                newId = rnd.Next(10000000, 99999999).ToString();
                isUnique = CheckIDUnique(newId);
            } while (!isUnique);

            return newId;
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

        #endregion Method
    }
}
