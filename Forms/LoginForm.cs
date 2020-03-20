using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SQLite;
using System.Reflection;

namespace ControlBoardTest
{
    public partial class LoginForm : Form
    {
        public USER user;

        string DB_FILE;
        const string ErrorMessage = "Cannot connect to database! Please contact the Ventec Life Systems engineer responsible for this tool";
        SQLiteConnection db_con;

        public LoginForm(string db_filepath)
        {
            this.DB_FILE = db_filepath;
            InitializeComponent();

            //Create a connection to the database


            if (File.Exists(DB_FILE))
            {
                //File.Create(".\\log.txt");
                File.WriteAllText(".\\logfile.txt", "Made it heree");
                this.db_con = new SQLiteConnection("Data Source=.\\Database\\MFG_527.db;Version=3");
            }
            else
            {
                //Display error message, and close application

                MessageBox.Show(ErrorMessage, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();

            }
            
            
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            string user = this.Field_User.Text;
            string pass = this.Field_Pass.Text;

            bool login = false;

            if ((user != null) && (pass != null))
            {
                string db_cmd = "SELECT * FROM USERS WHERE USERNAME LIKE \'" + user + "\';";

                var cmd = new SQLiteCommand(db_cmd, this.db_con);
                this.db_con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string username = Convert.ToString(reader["USERNAME"]);
                    string password = Convert.ToString(reader["PASSWORD"]);
                    bool admin = Convert.ToBoolean(reader["ADMIN"]);

                    if ((user == username) && (pass == password))
                    {
                        login = true;

                        this.user = new USER(username, admin);

                    }
                    else
                    {
                        
                    }
                        
                }
                reader.Close();
                this.db_con.Close();
                if (!login)
                {
                    this.user = null;
                    MessageBox.Show("Username or password did not match", "Wrong credentials", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                   
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Please enter your username and password", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        public USER ShowForm()
        {
            
            var output = this.ShowDialog();

            return this.user;
        }
    }
    public class USER
    {
        public bool admin = false;
        public string name;

        public USER(string name, bool admin)
        {
            this.name = name;
            this.admin = admin;
        }
    }

}
