using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _03_03_2023
{
    public partial class ProductsForm : Form
    {
        public int? IdProduct;
        bool ProductSelected;

        public ProductsForm()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            Settings();
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvProducts.Rows[e.RowIndex];
                IdProduct = Convert.ToInt32(selectedRow.Cells[0].Value);
                ProductSelected = true;
            }
            Specification module = new Specification(this);
            module.ShowDialog();
        }

        private void btn_Logout_Click(object sender, EventArgs e)
        {
            Auth auth = new Auth();
            this.Hide();
            auth.Show();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.uchConnectionString))
            {
                try
                {
                    DialogResult res = MessageBox.Show("Вы уверены что хотите удалить данный продукт?", "Product Deleted", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        string queary = "DELETE FROM `uch`.`items` WHERE (`item_id` = @id)";
                        MySqlCommand cm = new MySqlCommand(queary, cn);
                        cm.Parameters.AddWithValue("@id", IdProduct);
                        cn.Open();
                        if (cm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Продукт удалён!");
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Settings();
                    cn.Close();
                }
            }
        }

        public void Settings()
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.uchConnectionString))
            {
                try
                {
                    string query = @"SELECT `item_id` AS `№`, 
			                            `item_name` AS `Наименование`, 
                                        `item_desc` AS `Описание`, 
                                        `item_cost` AS `Цена` 
	                                FROM `uch`.`items`;";
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(query, cn);
                    da.Fill(dt);
                    dgvProducts.DataSource = dt;
                    //dgvProducts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    //dgvProducts.Columns[0].DisplayIndex = 4;
                    dgvProducts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[0].DisplayIndex = 0;
                    dgvProducts.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvProducts.Columns[1].DisplayIndex = 1;
                    dgvProducts.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[2].DisplayIndex = 2;
                    dgvProducts.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[3].DisplayIndex = 3;
                    if (User.UserRole == 2)
                    {
                        btn_Delete.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Products_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
