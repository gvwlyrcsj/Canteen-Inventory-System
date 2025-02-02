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
using System.Drawing.Printing;
using System.Diagnostics;

namespace CIS_CanteenInventorySystem
{
    public partial class SalesForm : Form
    {
        DBConnect db = new DBConnect();

        public SalesForm()
        {
            InitializeComponent(); 
        }

        private void getTableSales()
        {
            string insertSales = "Select * from tblSales";
            SqlCommand cmd = new SqlCommand(insertSales, db.GetCon());
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dGridViewAdd.DataSource = dt;
        }

        private void getTable()
        {
            string Querry = "Select ProductID, ProductName, Quantity, Price from tblInventory";
            SqlCommand cmd = new SqlCommand(Querry, db.GetCon());
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dGridViewInventory.DataSource = dt;
        }

        private void getFTotal() 
        {
            db.OpenCon();
            string query = "Select Sum(ProdTotal) As total from tblSales;";
            SqlCommand command = new SqlCommand(query, db.GetCon());
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                double sum = Convert.ToDouble(reader["total"]);
                txtFTotal.Text = sum.ToString();
            }
            reader.Close();
        }

        private void SalesForm_Load(object sender, EventArgs e)
        {
            getTable();
            getTableSales();
            getFTotal();
        }

        int grandTotal = 0;

        private void DisplayReceipt()
        {
            string Query = "Select * from tblSales";
            SqlDataAdapter adapter = new SqlDataAdapter(Query, db.GetCon());
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            var ds = new DataSet();
            adapter.Fill(ds);
            dGridViewAdd.DataSource = ds.Tables[0];
            db.CloseCon();
        }

        private void dGridViewInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = e.RowIndex;
                DataGridViewRow selectedRows = dGridViewInventory.Rows[index];
                txtProductID.Text = selectedRows.Cells[0].Value.ToString();
                txtProduct.Text = selectedRows.Cells[1].Value.ToString();
                txtPrice.Text = selectedRows.Cells[3].Value.ToString();
            }
            catch
            {

            }
        }

        private void ptbIF_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            HomeForm hF = new HomeForm();
            hF.Show();
            this.Hide();
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

        private void txtQuanti_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtQuanti.Text))
            {
                double price;
                double quantity;

                if (double.TryParse(txtPrice.Text, out price) && double.TryParse(txtQuanti.Text, out quantity))
                {
                    double total = price * quantity;
                    txtTotal.Text = total.ToString();
                }
                else
                {
                    txtTotal.Text = string.Empty; 
                }
            }
            else
            {
                txtTotal.Text = string.Empty; 
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int price;
            int quantity;
            if (int.TryParse(txtPrice.Text, out price) && int.TryParse(txtQuanti.Text, out quantity))
            {
                string insertSales = "Insert into tblSales Values ('" + txtProductID.Text +
                                     "', '" + txtProduct.Text +
                                     "', '" + txtPrice.Text +
                                     "', '" + txtQuanti.Text +
                                     "', '" + txtTotal.Text + "')";
                SqlCommand cmd = new SqlCommand(insertSales, db.GetCon());
                db.OpenCon();
                cmd.ExecuteNonQuery();
                db.CloseCon();
                getTableSales();

                int total = price * quantity;
                grandTotal += total;
                txtAmount.Text = "Php " + grandTotal;

                DataGridViewRow addRow = new DataGridViewRow();
                addRow.CreateCells(dGridViewReceipt);
                addRow.Cells[0].Value = txtProductID.Text;
                addRow.Cells[1].Value = txtProduct.Text;
                addRow.Cells[2].Value = txtPrice.Text;
                addRow.Cells[3].Value = txtQuanti.Text;
                addRow.Cells[4].Value = txtTotal.Text;
                dGridViewReceipt.Rows.Add(addRow);
            }

            else
            {
                MessageBox.Show("Invalid input.");
            } 
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (txtQuanti.Text == "")
            {
                MessageBox.Show("Missing Quantity");
            }

            try
            {
                db.GetCon().Open();
                string Query = "Select Quantity from tblInventory where ProductID = '" + txtProductID.Text + "'"; 
                SqlCommand cmd1 = new SqlCommand(Query, db.GetCon());
                int totalq = (Int32)cmd1.ExecuteScalar();
                int finalq = totalq - Int32.Parse(txtQuanti.Text);

                string Querry = "Update tblInventory set quantity = '" + finalq + "' where ProductID = '" + txtProductID.Text + "'";
                SqlCommand cmd2 = new SqlCommand(Querry, db.GetCon());
                cmd2.ExecuteNonQuery();
                getTable();
            }

            catch
            {

            }
            getFTotal();

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1;
            printPreviewDialog1.ShowDialog();
            dGridViewReceipt.Rows.Clear();
        }

        private void btnHome_Click_1(object sender, EventArgs e)
        {
            HomeForm hF = new HomeForm();
            hF.Show();
            this.Hide();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap imagebmp = new Bitmap(dGridViewReceipt.Width, dGridViewReceipt.Height);
            dGridViewReceipt.DrawToBitmap(imagebmp, new Rectangle(0,0, dGridViewReceipt.Width,dGridViewReceipt.Height));
            e.Graphics.DrawImage(imagebmp, 120, 20);
        }

        public void Linis()
        {
            txtProductID.Clear();
            txtProduct.Clear();
            txtPrice.Clear();
        }

        private void txtQuanti_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            dGridViewReceipt.Rows.Clear();
            Linis();
            txtQuanti.Clear();
            txtTotal.Clear();
            txtAmount.Clear();
        }
    }
}