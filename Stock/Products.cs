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

namespace Stock
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

       

        private void Products_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadData();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=Stock;Integrated Security=True");
            con.Open();
            bool Status = false;
            if (comboBox1.SelectedIndex == 0)
                {
                Status = true;
            }
            else {
                Status = false;
            }
            var sqlQuery = "";
            if (IfProductExists(con, textBox1.Text))
            {
                sqlQuery= @"UPDATE [Products] SET [product Name] = '" + textBox2.Text + "',[product status] = '" + Status + "' WHERE [Product Code] = '"+textBox1.Text+"'";
            }
            else {
                sqlQuery = @"INSERT INTO [Stock].[dbo].[Products]([Product Code],[product Name],[product status]) VALUES
                             ('" + textBox1.Text + "','" + textBox2.Text + "','" + Status + "')";
            }
           
            SqlCommand cmd = new SqlCommand ( sqlQuery, con);
            cmd.ExecuteNonQuery();
            con.Close();
            LoadData();

            
        }
        private bool IfProductExists(SqlConnection con, string ProductCode) {
            SqlDataAdapter sda = new SqlDataAdapter(" select 1 from [Products] where [Product Code]='"+ProductCode+"'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else {
                return false;
                    }

        }
        public void LoadData() {

            SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=Stock;Integrated Security=True");
            SqlDataAdapter sda = new SqlDataAdapter(" select * from [Stock].[dbo].[Products]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            dataGridView1.Rows.Clear();

            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["Product Code"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["product Name"].ToString();
                if ( (bool) item["product status"])
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Deactive";
                }



            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "Active")
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }

        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "Dective")
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=Stock;Integrated Security=True");
            var sqlQuery = "";
            if (IfProductExists(con, textBox1.Text))
            {
                con.Open();
                sqlQuery = @"DELETE from [Products] WHERE [Product Code] = '" + textBox1.Text + "'";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                MessageBox.Show("Record Not Found!!");
            }

            
            LoadData();
        }
    }
}
