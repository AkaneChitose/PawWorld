using System.Data.SqlClient;

namespace PawWorld
{
    class dbConnect
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        private string con;


        public string connection()
        {
            con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Final term\PawWorld_BU\PawWorld\dbBackUp.mdf;Integrated Security=True;Connect Timeout=30";
            return con;
        }
    }
}