using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlBoardTest
{
    public partial class final_result : Form
    {

        public string image_path;


        public final_result()
        {
            InitializeComponent();
            this.Text = "DISPLAY RESULTS";

        }

        private void final_results_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            pictureBox1.ImageLocation = image_path; //path to image
        }

        public void set_path(string path)
        {
            image_path = path;

        }

        public void set_testTime(string time)
        {
            TestTime.Text = time;
        }

        public void set_userText(string text)
        {
            userText.Text = text;
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
