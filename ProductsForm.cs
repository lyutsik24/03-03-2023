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
        public DataTable dataTable = new DataTable("Cort");
        public int CortCounter = 0;

        public ProductsForm()
        {
            InitializeComponent();
            CortDataTable();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            Settings();
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            IdProduct = Convert.ToInt32(dgvProducts.CurrentRow.Cells[2].Value);
            string ColumnName = dgvProducts.Columns[e.ColumnIndex].Name;
            if (ColumnName == "Add")
            {
                if (Convert.ToDouble(dgvProducts.CurrentRow.Cells["Qty"].Value) > 0)
                {
                    dataTable.Rows.Add(new Object[]{
                    ++CortCounter,
                    dgvProducts.CurrentRow.Cells["№"].Value,
                    dgvProducts.CurrentRow.Cells["Наименование"].Value,
                    dgvProducts.CurrentRow.Cells["Qty"].Value,
                    Convert.ToDouble(dgvProducts.CurrentRow.Cells["Qty"].Value) * Convert.ToDouble(dgvProducts.CurrentRow.Cells["Цена"].Value),
                });
                }
            }
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
                    dgvProducts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[0].DisplayIndex = 5;
                    dgvProducts.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[1].DisplayIndex = 4;
                    dgvProducts.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[2].DisplayIndex = 0;
                    dgvProducts.Columns[2].ReadOnly = true;
                    dgvProducts.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvProducts.Columns[3].DisplayIndex = 1;
                    dgvProducts.Columns[3].ReadOnly = true;
                    dgvProducts.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[4].DisplayIndex = 2;
                    dgvProducts.Columns[4].ReadOnly = true;
                    dgvProducts.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[5].DisplayIndex = 3;
                    dgvProducts.Columns[5].ReadOnly = true;
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

        private void dgvProducts_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Specification module = new Specification(this);
            module.ShowDialog();
        }

        private void CortDataTable()
        {
            DataColumn datatableColumnNumber = new DataColumn();
            DataColumn datatableColumnId = new DataColumn();
            DataColumn datatableColumnName = new DataColumn();
            DataColumn datatableColumnPrice = new DataColumn();
            DataColumn datatableColumnQty = new DataColumn();

            datatableColumnNumber.DataType = typeof(Int32);
            datatableColumnNumber.ColumnName = "№";
            dataTable.Columns.Add(datatableColumnNumber);

            datatableColumnId.DataType = typeof(Int32);
            datatableColumnId.ColumnName = "#";
            dataTable.Columns.Add(datatableColumnId);

            datatableColumnName.DataType = typeof(string);
            datatableColumnName.ColumnName = "Наименование";
            dataTable.Columns.Add(datatableColumnName);

            datatableColumnQty.DataType = typeof(Int32);
            datatableColumnQty.ColumnName = "Кол-во";
            dataTable.Columns.Add(datatableColumnQty);

            datatableColumnPrice.DataType = typeof(double);
            datatableColumnPrice.ColumnName = "Общая сумма";
            dataTable.Columns.Add(datatableColumnPrice);
        }

        private void btnCort_Click(object sender, EventArgs e)
        {
            Cort module = new Cort(this, dataTable);
            module.ShowDialog();
        }
    }
}
