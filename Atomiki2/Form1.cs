using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;



namespace Atomiki2
{
    public partial class Form1 : Form
    {
        String connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Nikos\source\repos\Atomiki2\Atomiki2\Database1.mdb";

        OleDbConnection connection;

        DataTable contacts = new DataTable();
        string Link = "Select * from Contacts";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = null;





            connection = new OleDbConnection(connectionString);

            
            OleDbCommand oleDb = new OleDbCommand(Link, connection);
            connection.Open();
            oleDb.CommandType = CommandType.Text;
            OleDbDataAdapter adapter = new OleDbDataAdapter(oleDb);
            //DataTable contacts = new DataTable();
            adapter.Fill(contacts);
            dataGridView1.DataSource = contacts;

            label13.Hide();
            label14.Hide();
            WindowsMediaPlayer1.Hide();
            connection.Close();

            textBox10.Enabled = false;
            textBox9.Enabled = false;
            textBox8.Enabled = false;

        }



        private void button1_Click(object sender, EventArgs e)
        {
           
            String Fname = textBox1.Text;
            String Lname = textBox2.Text;
            String Email = textBox3.Text;
            String PhoneNum = textBox4.Text;
            String adrrs = textBox5.Text;
            String PhotoPath = textBox6.Text;  
            String MusicPath =  "C:"+textBox7.Text.Substring(textBox7.Text.LastIndexOf("\\") + 1);

           





            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "") //Fix THIS , Make it so it cathes email or number
            {
                MessageBox.Show("PLease fill the Nessacery information");
                return;
            }





                int value;
           
                if (!int.TryParse(PhoneNum,out value))
                {
                    label14.Show();
                    MessageBox.Show("Please Enter Credentials with correct form ");
                    return;
                }
                if (!ValidateEmail(Email))
                {
                    label13.Show();
                    MessageBox.Show("Please Enter Credentials with correct form ");
                    return;
                }







            try
            {
                connection.Open();

                String query = "Insert into Contacts([First Name],[Last Name],Email,Address,Photopath,Musicpath,Birthday,PhoneNum)"+ "values('"+ Fname +"', '"+Lname+"' , '"+Email+"', '"+adrrs+"','"+PhotoPath+"','"+MusicPath+"', '"+ dateTimePicker1.Value.ToShortDateString()+"' , '" +value+"')";

                OleDbCommand oleDbCommand = new OleDbCommand(query, connection);

                oleDbCommand.ExecuteNonQuery();

                MessageBox.Show("New Contact Added !");
            }
            catch ( Exception ex)
            {

                MessageBox.Show(ex.Message , "Message" , MessageBoxButtons.OK , MessageBoxIcon.Error);

            }
            OleDbCommand oleDbCommand2 = new OleDbCommand(Link, connection);
            OleDbDataReader reader = oleDbCommand2.ExecuteReader();
            while (reader.Read()) 
            {
               
           //Fill this to fix the birthday problem
            }
          



            connection.Close();

           Update_DataGrid();


        }


        private void Update_DataGrid() {


            OleDbCommand oleDb = new OleDbCommand(Link, connection);
            connection.Open();
            oleDb.CommandType = CommandType.Text;
            OleDbDataAdapter adapter = new OleDbDataAdapter(oleDb);
            DataTable contacts = new DataTable();
            adapter.Fill(contacts);
            dataGridView1.DataSource = contacts;
            connection.Close();

        }





        private void button2_Click(object sender, EventArgs e)
        {
            //Lets you select a picture and gets the file path
            OpenFileDialog openFileDialog = new OpenFileDialog {

                Title = "Browse Pictures",

                CheckFileExists = true,
                CheckPathExists = true,


                DefaultExt = "png",
                Filter = "png files (*.png)|*.png",
                ValidateNames = true,
                Multiselect = false,
                


                ReadOnlyChecked = true,
                ShowReadOnly = true

            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = openFileDialog.FileName;
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Lets you select a Song and gets the file path
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse songs",

                CheckFileExists = true,
                CheckPathExists = true,


                DefaultExt = "mp3",
                Filter = "mp3 files (*.mp3)|*.mp3",
                FilterIndex = 2,
                RestoreDirectory = true,


                ReadOnlyChecked = true,
                ShowReadOnly = true

            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox7.Text = openFileDialog.FileName;
            }
        }

       
        private void button6_Click(object sender, EventArgs e)
        {
            string txt = "You sure you want to exit ?";
            MessageBox.Show( txt,"Message",MessageBoxButtons.OK,MessageBoxIcon.Question);
            Application.Exit();
        }


        public static bool ValidateEmail(string email)
        {
            System.Text.RegularExpressions.Regex emailRegex = new System.Text.RegularExpressions.Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            System.Text.RegularExpressions.Match emailMatch = emailRegex.Match(email);
            return emailMatch.Success;
        }

        

        private void button7_Click(object sender, EventArgs e)
        {
            

            DataView view = contacts.DefaultView;
            if (radioButton1.Checked == true)
            {
                view.RowFilter = "[First Name] LIKE '%" + textBox10.Text + "%'";

            }
            if (radioButton2.Checked == true)
            {
                view.RowFilter = "[Last Name] LIKE '%" + textBox9.Text + "%'";

            }
            if (radioButton3.Checked == true)
            {
                int val = Convert.ToInt32(textBox8.Text);
                view.RowFilter = "PhoneNum LIKE '%" + val + "%'";
            }

        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            //Showing all contacts 
            textBox9.Text = "";
            textBox8.Text = "";
            textBox10.Text = "";
            button7_Click(sender, e);
         }

        

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == false)
            {
                textBox10.Enabled = false;

            }
            else
            {
                textBox10.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == false)
            {
                textBox9.Enabled = false;
            }
            else
            {
                textBox9.Enabled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == false)
            {
                textBox8.Enabled = false;
            }
            else {
                textBox8.Enabled = true;
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            //Delete the contact that is choosen based on id
            String name = textBox1.Text;
            String Lname = textBox2.Text;
            String email = textBox3.Text;
            String phone = textBox4.Text;
            String query = "DELETE FROM Contacts Where id="+id;
            try
            {
                OleDbCommand command = new OleDbCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
                Form1_Load(sender, e);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally {
                connection.Close();
            }

            Update_DataGrid();    //Calls Function to Refresh the datagrid

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Gets the data of a contact when one of its cells is clicked and puts them in the textsboxes corespoding to its data Cell 
            int row_num;
            if(e.RowIndex >= 0)
            {
                row_num = e.RowIndex;
                DataGridViewRow viewRow = dataGridView1.Rows[row_num];
                 id = viewRow.Cells[0].Value.ToString();
               
                textBox1.Text = viewRow.Cells[1].Value.ToString();
                textBox2.Text = viewRow.Cells[2].Value.ToString();
                textBox3.Text = viewRow.Cells[3].Value.ToString();
                textBox4.Text = viewRow.Cells[8].Value.ToString();
                textBox5.Text = viewRow.Cells[4].Value.ToString();
                textBox6.Text = viewRow.Cells[5].Value.ToString();
                textBox7.Text = viewRow.Cells[6].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(viewRow.Cells[7].Value);
                try
                {
                    pictureBox1.Image = Image.FromFile(viewRow.Cells[5].Value.ToString());
                }
                catch (Exception ex)
                {
                    pictureBox1.Image = Properties.Resources.ContactNotFound;
                  
                   
                }
                try
                {

                    WindowsMediaPlayer1.URL = textBox7.Text;
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        string id;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellContentClick(sender, e);
        }

        private void Edit_Click_1(object sender, EventArgs e)
        {
            try
            {

                

                String update = "UPDATE Contacts SET [First Name] ='"+textBox1.Text+"',[Last Name]='"+textBox2.Text+"',Email='"+textBox3.Text+"',Address='"+textBox5.Text+"',Photopath='"+textBox6.Text+"',Musicpath='"+textBox7.Text+"',Birthday='"+dateTimePicker1.Value.ToShortDateString()+"',PhoneNum='"+textBox4.Text+"' WHERE id =?";

                connection.Open();

                OleDbCommand oleDb = new OleDbCommand(update, connection);

                oleDb.Parameters.AddWithValue("id", Convert.ToInt32(id));
                oleDb.Parameters.AddWithValue("[First Name]", textBox1.Text);
                oleDb.Parameters.AddWithValue("[Last Name]", textBox2.Text);
                oleDb.Parameters.AddWithValue("Email", textBox3.Text);
                oleDb.Parameters.AddWithValue("Address", textBox5.Text);
                oleDb.Parameters.AddWithValue("PhotoPath", textBox6.Text);
                oleDb.Parameters.AddWithValue("Musicpath", textBox7.Text);
                oleDb.Parameters.AddWithValue("Birthday", dateTimePicker1.Value);
                oleDb.Parameters.AddWithValue("PhoneNum", textBox4.Text);

                oleDb.ExecuteNonQuery();


                MessageBox.Show("Updated a Contact!");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally {

                connection.Close();

            }

            Update_DataGrid();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            this.dateTimePicker1.Value = DateTime.Today;
        }
    }
}
