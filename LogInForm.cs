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
    public partial class LogInForm : Form
    {
        public LogInForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ACER\Downloads\CIS\CIS-CanteenInventorySystem\dbInfo.mdf;Integrated Security=True";
            SqlConnection sqlCon = new SqlConnection(conStr);
            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM tblPassword WHERE Username = @Username AND Password = @Password", sqlCon);
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
            cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string userName = dr["Username"].ToString();
                MessageBox.Show("Log in Success!");
                HomeForm hf= new HomeForm();
                hf.Show();
            }
            else
            {
                MessageBox.Show("Log Failed");
            }
        }

        private void chbHide_CheckedChanged(object sender, EventArgs e)
        {
            if (chbHide.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
            }
        }

        private void ptbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblChangePass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangePassForm cPF = new ChangePassForm();
            cPF.Show();
            this.Hide();
        }

        public void Linis()
        {
            txtUsername.Clear();
            txtPassword.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Linis();
        }
    }
}
