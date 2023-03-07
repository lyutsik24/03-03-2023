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
    public partial class Specification : Form
    {
        private ProductsForm fromForm { get; set; }
        private int? id;

        public Specification(ProductsForm form)
        {
            InitializeComponent();
            fromForm = form;
        }

        private void Specification_Load(object sender, EventArgs e)
        {
            id = fromForm.IdProduct;
            lblPcode.Text = id.ToString();
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.uchConnectionString))
            {
                string queary = @"SELECT `item_id` AS `№`, 
			                            `item_name` AS `Наименование`, 
                                        `item_desc` AS `Описание`, 
                                        `item_cost` AS `Цена` 
	                            FROM `uch`.`items`
                                WHERE `item_id` = @id";
                try
                {
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(queary, cn);
                    da.SelectCommand.Parameters.AddWithValue("@id", id);
                    da.Fill(dt);
                    txtName.Text = dt.Rows[0]["Наименование"].ToString();
                    txtDesc.Text = dt.Rows[0]["Описание"].ToString();
                    txtPrice.Text = dt.Rows[0]["Цена"].ToString();
                    if (User.UserRole == 2)
                    {
                        txtPrice.ReadOnly = true;
                        txtName.ReadOnly = true;
                        txtDesc.ReadOnly = true;
                        btnEdit.Hide();

                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cn.Close();
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            fromForm.Settings();
            this.Hide();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.uchConnectionString))
            {
                string queary = @"UPDATE `uch`.`items` 
                                SET `item_name` = @name, 
		                            `item_desc` = @desc, 
                                    `item_cost` = @price
                                WHERE (`item_id` = @pcode)";
                MySqlCommand cmd = new MySqlCommand(queary, cn);
                try
                {
                    cmd.Parameters.AddWithValue("@pcode", int.Parse(lblPcode.Text));
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@desc", txtDesc.Text);
                    cmd.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cn.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Данные товара успешно изменены.");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cn.Close();
                }
            }
        }
    }
}
