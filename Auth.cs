using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace _03_03_2023
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
            txtPass.UseSystemPasswordChar = true;
            txtLogin.Text = "admin";
            txtPass.Text = "admin";
        }

        private void BtnAuth_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text == "")
            {
                MessageBox.Show("Вы не ввели логин. Перепроверьте вводимые данные!");
            }
            else if (txtPass.Text == "")
            {
                MessageBox.Show("Вы не ввели пароль. Перепроверьте вводимые данные!");
            }
            else
            {
                using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.uchConnectionString))
                {
                    try
                    {
                        string query = @"SELECT `auth_role` 
                                        FROM `uch`.`auth` 
                                        WHERE `auth_log` = @user
                                        AND `auth_pwd` = @password;";
                        DataTable dt = new DataTable();
                        MySqlDataAdapter da = new MySqlDataAdapter(query, cn);
                        da.SelectCommand.Parameters.AddWithValue("@user", txtLogin.Text);
                        da.SelectCommand.Parameters.AddWithValue("@password", txtPass.Text);
                        da.Fill(dt);
                        if (dt.Rows.Count == 1)
                        {
                            User.UserRole = Convert.ToInt32(dt.Rows[0]["auth_role"]);
                            ProductsForm products = new ProductsForm();
                            this.Hide();
                            products.Show();
                        }
                        else
                        {
                            MessageBox.Show("Перепроверьте вводимые данные!", "Ошибка", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }

        private void Auth_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
