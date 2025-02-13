﻿using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public string mySqlServerName = "sql6.freemysqlhosting.net";
        public string mySqlServerUserId = "sql6696982";
        public string mySqlServerPassword = "DJyeU2QQMU";
        public string mySqlDatabaseName = "sql6696982";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("datasource=" + mySqlServerName + ";port=3306;username=" + mySqlServerUserId + ";password=" + mySqlServerPassword + ";database=" + mySqlDatabaseName + ";");
            connection.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM sample_image", connection);
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void BTN_CHOOSE_IMAGE_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Select image(*.JpG; *.png; *.Gif)|*.jpg; *.png; *.gif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void InsertData()
        {
            try
            {
                // check if there is image
                if (!string.IsNullOrEmpty(openFileDialog1.FileName))
                {
                    // Read the image file into a byte array
                    byte[] imageData;
                    using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            imageData = br.ReadBytes((int)fs.Length);
                        }
                    }

                    
                    using (MySqlConnection connection = new MySqlConnection("datasource=" + mySqlServerName + ";port=3306;username=" + mySqlServerUserId + ";password=" + mySqlServerPassword + ";database=" + mySqlDatabaseName + ";"))
                    {
                        MySqlCommand command = connection.CreateCommand();

                                                command.CommandText = "INSERT INTO sample_image (images) VALUES (@image)";
                        command.Parameters.AddWithValue("@image", imageData);

                        
                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Image uploaded successfully!");

                    
                    ReloadDataGridView();
                }
                else
                {
                    MessageBox.Show("Please select an image first.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ReloadDataGridView()
        {
            try
            {
                
                DataTable dt = new DataTable();

               
                using (MySqlConnection connection = new MySqlConnection("datasource=" + mySqlServerName + ";port=3306;username=" + mySqlServerUserId + ";password=" + mySqlServerPassword + ";database=" + mySqlDatabaseName + ";"))
                {
                   
                    connection.Open();

                    
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM sample_image", connection))
                    {
                        
                        using (MySqlDataAdapter adp = new MySqlDataAdapter(cmd))
                        {
                            
                            adp.Fill(dt);
                        }
                    }
                }

                
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reloading DataGridView: " + ex.Message);
            }
        }


        private void create_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Insert button clicked!");
            InsertData();
        }
    }
}
