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

namespace CIS_CanteenInventorySystem
{
    public partial class HomeForm : Form
    {
        DBConnect db = new DBConnect();

        public HomeForm()
        {
            InitializeComponent();           
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            InventoryForm iF = new InventoryForm();
            iF.Show();
            this.Hide();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            SalesForm sF = new SalesForm();
            sF.Show();
            this.Hide();
        }

        private void ptbIF_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {            
            db.OpenCon();
            string query = "Select Sum(ProdTotal) As total from tblSales";
            SqlCommand command = new SqlCommand(query, db.GetCon());
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                double sum = Convert.ToDouble(reader["total"]);
                txtValue.Text = "PHP " + sum.ToString();
            }
            reader.Close();
        }
    }
}
