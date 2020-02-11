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
    public partial class ChangePassForm : Form
    {
        public USER user;

        string DB_FILE;
        const string ErrorMessage = "Cannot connect to database! Please contact the Ventec Life Systems engineer responsible for this tool";
        SQLiteConnection db_con;
        private bool success = false;
        public ChangePassForm(string db_filepath)
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

        public bool ShowForm(USER currUser)
        {
            bool success = false;
            this.user = currUser;
            var output = this.ShowDialog();

            success = this.success;

            return success;
        }

        private void Button_Confirm_Click(object sender, EventArgs e)
        {
            string old_pass = this.Field_OldPass.Text;
            string new_pass = this.Field_NewPass.Text;




            if ((old_pass != null) && (new_pass != null) && (new_pass == this.Field_VerfPass.Text))
            {
                string db_cmd = "UPDATE USERS " +
                                "SET PASSWORD = \'" + new_pass + "\' " +
                                "WHERE USERNAME = \'" + this.user.name + "\' AND PASSWORD = \'" + old_pass + "\';";

                var cmd = new SQLiteCommand(db_cmd, this.db_con);
                this.db_con.Open();
                cmd.ExecuteNonQuery();
                this.db_con.Close();

                MessageBox.Show("Success!");
                this.success = true;
                this.Close();
            }

            else
            {
                MessageBox.Show("Please try again", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
