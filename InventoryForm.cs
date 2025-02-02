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
    public partial class InventoryForm : Form
    {
        DBConnect db = new DBConnect();
        DataTable dt = new DataTable();

        public InventoryForm()
        {
            InitializeComponent();
        }

        private void getTable()
        {
            string Querry = "Select * from tblInventory";
            SqlCommand cmd = new SqlCommand(Querry, db.GetCon());
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dGridViewInventory.DataSource = dt; 
        }

        public void Linis()
        {
            txtProductID.Clear();
            txtProduct.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string Query = "Insert into tblInventory Values ('" + txtProductID.Text +
                                                                 "', '" + txtProduct.Text +
                                                                 "', '" + cmbCategory.Text +
                                                                 "', '" + txtQuantity.Text +
                                                                 "', '" + txtPrice.Text + "')";
                SqlCommand cmd = new SqlCommand(Query, db.GetCon());
                db.OpenCon();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data has been saved successfully!");
                Linis();
                dGridViewInventory.Refresh();
                db.CloseCon();
                getTable();
            }

            catch 
            {
                MessageBox.Show("Invalid input format.");
            }
        }


        private void InventoryForm_Load(object sender, EventArgs e)
        {
            getTable();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string Query = "Delete from tblInventory Where ProductID = '" + txtProductID.Text + "'";
                SqlCommand cmd = new SqlCommand(Query, db.GetCon());
                db.OpenCon();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data has been deleted successfully!");
                Linis();
                db.CloseCon();
                getTable();
            }
            catch
            {
                MessageBox.Show("Invalid input format.");
            }
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            SalesForm sF = new SalesForm();
            sF.Show();
            this.Hide();
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            InventoryForm iF = new InventoryForm();
            iF.Show();
            this.Hide();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            HomeForm hF = new HomeForm();
            hF.Show();
            this.Hide();
        }

        private void dGridViewInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = e.RowIndex;
                DataGridViewRow selectedRows = dGridViewInventory.Rows[index];
                txtProductID.Text = selectedRows.Cells[0].Value.ToString();
                txtProduct.Text = selectedRows.Cells[1].Value.ToString();
                cmbCategory.Text = selectedRows.Cells[2].Value.ToString();
                txtQuantity.Text = selectedRows.Cells[3].Value.ToString();
                txtPrice.Text = selectedRows.Cells[4].Value.ToString();
            }
            catch
            {

            }
        }

        private void txtProductID_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void ptbIF_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProductID.Text) || string.IsNullOrEmpty(txtProduct.Text) || string.IsNullOrEmpty(cmbCategory.Text) || string.IsNullOrEmpty(txtQuantity.Text) || string.IsNullOrEmpty(txtPrice.Text))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            try
            {
                string Query = "Update tblInventory Set ProductName= '" + txtProduct.Text +
                                     "', Category= '" + cmbCategory.Text +
                                     "', Quantity= '" + txtQuantity.Text +
                                     "', Price= '" + txtPrice.Text +
                                     "' Where ProductID= '" + txtProductID.Text + "'";
                SqlCommand cmd = new SqlCommand(Query, db.GetCon());
                db.OpenCon();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data has been updated successfully!");
                Linis();
                dGridViewInventory.Refresh();
                db.CloseCon();
                getTable();
            }

            catch
            {
                MessageBox.Show("Invalid input format.");
            }
        }
    }
}
