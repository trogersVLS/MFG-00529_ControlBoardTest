using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace ControlBoardTest
{
    public partial class AddUserForm : Form
    {

        public USER user;

        string DB_FILE;
        const string ErrorMessage = "Cannot connect to database! Please contact the Ventec Life Systems engineer responsible for this tool";
        SQLiteConnection db_con;

        private bool success;
        public AddUserForm(string db_filepath)
        {
            this.DB_FILE = db_filepath;
            InitializeComponent();


            if (File.Exists(DB_FILE))
            {
                this.db_con = new SQLiteConnection("Data Source=" + DB_FILE + ";Version=3");
            }
            else
            {
                //Display error message, and close application

                MessageBox.Show(ErrorMessage, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void Button_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        public bool ShowForm()
        {

            var output = this.ShowDialog();

            return this.success;
        }


        private void Check_Admin_CheckedChanged(object sender, EventArgs e)
        {
            this.user.admin = this.Check_Admin.Checked;
        }

        private void Button_Confirm_Click(object sender, EventArgs e)
        {
            string user = this.Field_User.Text;
            string pass = this.Field_Pass.Text;
            bool admin = this.Check_Admin.Checked;

            bool good_username = false;

            if ((user != null) && (pass != null))
            {
                string db_cmd = "SELECT * FROM USERS WHERE USERNAME LIKE \'" + user + "\';";

                var cmd = new SQLiteCommand(db_cmd, this.db_con);
                this.db_con.Open();
                var reader = cmd.ExecuteReader();
                if(reader.HasRows)
                {
                    MessageBox.Show("Unable to add user, username already exists in database");
                    good_username = false;
                }
                else
                {
                    good_username = true;
                }
                reader.Close();
                this.db_con.Close();

                if (good_username)
                {   bool privilege = this.Check_Admin.Checked;
                    this.user = new USER(user, admin);

                    db_cmd = "insert into USERS(USERNAME, PASSWORD, ADMIN) VALUES(\'" + user + "\',\'" + pass + "\',\'" + privilege.ToString() + "\')";

                    cmd = new SQLiteCommand(db_cmd, this.db_con);
                    this.db_con.Open();
                    int num = cmd.ExecuteNonQuery();
                    this.db_con.Close();
                    if(num > 0)
                    {
                        MessageBox.Show("Success!");
                        this.success = true;
                        this.Close();
                    }
                }
                
                
            }
            else
            {
                MessageBox.Show("Please enter your username and password", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
