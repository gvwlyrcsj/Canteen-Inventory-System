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
    public partial class ChangePassForm : Form
    {
        public ChangePassForm()
        {
            InitializeComponent();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            string conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ACER\Downloads\CIS\CIS-CanteenInventorySystem\dbInfo.mdf;Integrated Security=True";
            SqlConnection sqlCon = new SqlConnection(conStr);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM tblPassword WHERE Username = @Username AND Password = @Password", sqlCon);
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
            cmd.Parameters.AddWithValue("@Password", txtCurPass.Text); 
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Credentials has been verified!");
                groupBox1.Visible = true;
            }
            else
            {
                MessageBox.Show("Wrong Credentials.");
            }
            dr.Close();
            sqlCon.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chb1.Checked)
            {
                txtCurPass.PasswordChar = '\0';
            }
            else
            {
                txtCurPass.PasswordChar = '*';
            }
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            if (txtNewPass.Text == txtConPass.Text)
            {
                string conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ACER\Downloads\CIS\CIS-CanteenInventorySystem\dbInfo.mdf;Integrated Security=True";
                SqlConnection sqlCon = new SqlConnection(conStr);
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("Update tblPassword set Password ='" + txtNewPass.Text + "' where Username = '" + txtUsername.Text + "'", sqlCon);
                SqlDataReader dr = cmd.ExecuteReader();
                MessageBox.Show("Password has been changed!");
                LogInForm lF = new LogInForm();
                lF.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Password does not match.");
            }
        }

        private void chb2_CheckedChanged(object sender, EventArgs e)
        {
            if (chb2.Checked)
            {
                txtNewPass.PasswordChar = '\0';
                txtConPass.PasswordChar = '\0';
            }
            else
            {
                txtCurPass.PasswordChar = '*';
                txtNewPass.PasswordChar = '*';
            }
        }
    }
}
