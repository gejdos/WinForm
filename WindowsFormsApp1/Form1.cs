using System;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string host, db, port, query;
        private MySqlConnection conn;
        private MySqlCommand com;
        private MySqlDataReader reader;
        private object result;

        public Form1()
        {
            Form2 LoginForm = new Form2();
            host = ConfigurationManager.AppSettings["host"];
            db = ConfigurationManager.AppSettings["db"];
            port = ConfigurationManager.AppSettings["port"];


            LoginForm.ShowDialog();
          
            conn = new MySqlConnection("server=" + host + ";user=" + LoginForm.user + ";password=" + LoginForm.password + ";database=" + db + ";port=" + port + ";");

            LoginForm.Close();

            InitializeComponent();            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn.Open();

            query = "SELECT Short_DSP_Code FROM data GROUP BY Short_DSP_Code ORDER BY COUNT(DISTINCT DPS_NUM) DESC";
            com = new MySqlCommand(query, conn);
            reader = com.ExecuteReader();
          
            PartnerCb.Items.Add("ALL PARTNERS");

            while (reader.Read())
            {
              PartnerCb.Items.Add(reader[0]);
            }

            conn.Close();            
        }

        private void PartnerCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            dpsCb.Items.Clear();
            dpsCb.ResetText();
            textBox1.Clear();
            textBox2.Clear();

            conn.Open();

            if (PartnerCb.Text == "ALL PARTNERS")
            {
                query = "SELECT DISTINCT DPS_NUM FROM data ORDER BY DPS_NUM ASC";
            }
            else
            {
                query = "SELECT DISTINCT DPS_NUM FROM data WHERE Short_DSP_Code = @partner ORDER BY DPS_NUM ASC";
            }

            com = new MySqlCommand(query, conn);
            com.Parameters.AddWithValue("@partner", PartnerCb.Text);

            reader = com.ExecuteReader();
            while (reader.Read())
            {
                dpsCb.Items.Add(reader[0]);
            }

            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            
            if (PartnerCb.Text == "ALL PARTNERS")
            {
                query = "SELECT Country FROM data WHERE DPS_NUM = @dps";
            }
            else
            {
                query = "SELECT Country FROM data WHERE Short_DSP_Code = @partner AND DPS_NUM = @dps GROUP BY Short_DSP_Code";
            }
            com = new MySqlCommand(query, conn);

            try
            {
                com.Parameters.AddWithValue("@partner", PartnerCb.Text);
                com.Parameters.AddWithValue("@dps", dpsCb.Text);
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                result = com.ExecuteScalar();
                textBox1.Text = result.ToString();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("No country found.\n" + ex.Message.ToString());
            }

            conn.Close();
        }

        private void dpsCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();

            conn.Open();

            query = "SELECT (COUNT(*) - 1) FROM data WHERE DPS_NUM = @dps";
            com = new MySqlCommand(query, conn);
            com.Parameters.AddWithValue("@dps", dpsCb.Text);
            result = com.ExecuteScalar();

            textBox2.Text = result.ToString();

            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
