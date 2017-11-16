using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public string password, user;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string host = ConfigurationManager.AppSettings["host"];
            string db = ConfigurationManager.AppSettings["db"];
            string port = ConfigurationManager.AppSettings["port"];

            user = textBox1.Text;
            password = textBox2.Text;

            MySqlConnection conn = new MySqlConnection("server = " + host + "; user = " + user + "; password = " + password
                + "; database = " + db + ";port=" + port + ";");

            try
            {
                conn.Open();
                conn.Close();
                MessageBox.Show("Connected!");
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
