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
    public partial class Cort : Form
    {
        ProductsForm fromForm { get; set; }
        DataTable dt;

        public Cort(ProductsForm form, DataTable dataTable)
        {
            InitializeComponent();
            fromForm = form;
            dt = dataTable;
            dgvCort.DataSource = dt;
            dgvCort.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvCort.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvCort.Columns[1].Visible = false;
            dgvCort.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCort.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
