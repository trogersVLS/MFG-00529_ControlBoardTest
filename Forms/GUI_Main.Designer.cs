namespace ControlBoardTest
{
    partial class GUI_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI_Main));
            this.Button_Run = new System.Windows.Forms.Button();
            this.Field_SerialNumber = new System.Windows.Forms.TextBox();
            this.label_serialNumber = new System.Windows.Forms.Label();
            this.console_debugOutput = new System.Windows.Forms.RichTextBox();
            this.Button_Yes = new System.Windows.Forms.Button();
            this.Button_No = new System.Windows.Forms.Button();
            this.StatusBar = new System.Windows.Forms.ProgressBar();
            this.Label_User = new System.Windows.Forms.Label();
            this.Label_Username = new System.Windows.Forms.Label();
            this.Check_LogToDatabase = new System.Windows.Forms.CheckBox();
            this.Dropdown_Test_List = new System.Windows.Forms.ComboBox();
            this.Check_SingleTest = new System.Windows.Forms.CheckBox();
            this.Check_FullTest = new System.Windows.Forms.CheckBox();
            this.Check_Program = new System.Windows.Forms.CheckBox();
            this.Console_Admin = new System.Windows.Forms.TextBox();
            this.Logo_VLS = new System.Windows.Forms.PictureBox();
            this.Button_Jtag_Herc = new System.Windows.Forms.Button();
            this.Button_Jtag_CPLD = new System.Windows.Forms.Button();
            this.Button_SOM = new System.Windows.Forms.Button();
            this.Button_DMM = new System.Windows.Forms.Button();
            this.Button_PPS = new System.Windows.Forms.Button();
            this.Button_GPIO = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Logo_VLS)).BeginInit();
            this.SuspendLayout();
            // 
            // Button_Run
            // 
            this.Button_Run.Location = new System.Drawing.Point(425, 107);
            this.Button_Run.Name = "Button_Run";
            this.Button_Run.Size = new System.Drawing.Size(103, 42);
            this.Button_Run.TabIndex = 1;
            this.Button_Run.Text = "Run";
            this.Button_Run.UseVisualStyleBackColor = true;
            this.Button_Run.Click += new System.EventHandler(this.Button_Run_Click);
            // 
            // Field_SerialNumber
            // 
            this.Field_SerialNumber.BackColor = System.Drawing.SystemColors.Window;
            this.Field_SerialNumber.Location = new System.Drawing.Point(86, 102);
            this.Field_SerialNumber.Name = "Field_SerialNumber";
            this.Field_SerialNumber.Size = new System.Drawing.Size(150, 20);
            this.Field_SerialNumber.TabIndex = 0;
            this.Field_SerialNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Field_SerialNumber_KeyUp);
            // 
            // label_serialNumber
            // 
            this.label_serialNumber.AutoSize = true;
            this.label_serialNumber.Location = new System.Drawing.Point(8, 105);
            this.label_serialNumber.Name = "label_serialNumber";
            this.label_serialNumber.Size = new System.Drawing.Size(73, 13);
            this.label_serialNumber.TabIndex = 4;
            this.label_serialNumber.Text = "Serial Number";
            // 
            // console_debugOutput
            // 
            this.console_debugOutput.Location = new System.Drawing.Point(5, 195);
            this.console_debugOutput.Name = "console_debugOutput";
            this.console_debugOutput.ReadOnly = true;
            this.console_debugOutput.Size = new System.Drawing.Size(521, 394);
            this.console_debugOutput.TabIndex = 9;
            this.console_debugOutput.Text = "";
            this.console_debugOutput.TextChanged += new System.EventHandler(this.Console_debugOutput_TextChanged);
            // 
            // Button_Yes
            // 
            this.Button_Yes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Button_Yes.Location = new System.Drawing.Point(578, 469);
            this.Button_Yes.Name = "Button_Yes";
            this.Button_Yes.Size = new System.Drawing.Size(127, 120);
            this.Button_Yes.TabIndex = 10;
            this.Button_Yes.Text = "Yes";
            this.Button_Yes.UseVisualStyleBackColor = false;
            this.Button_Yes.Visible = false;
            this.Button_Yes.Click += new System.EventHandler(this.Button_Yes_Click);
            // 
            // Button_No
            // 
            this.Button_No.BackColor = System.Drawing.Color.Red;
            this.Button_No.Location = new System.Drawing.Point(755, 469);
            this.Button_No.Name = "Button_No";
            this.Button_No.Size = new System.Drawing.Size(127, 120);
            this.Button_No.TabIndex = 11;
            this.Button_No.Text = "No";
            this.Button_No.UseVisualStyleBackColor = false;
            this.Button_No.Visible = false;
            this.Button_No.Click += new System.EventHandler(this.Button_No_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.Location = new System.Drawing.Point(5, 166);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(521, 23);
            this.StatusBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.StatusBar.TabIndex = 12;
            // 
            // Label_User
            // 
            this.Label_User.AutoSize = true;
            this.Label_User.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_User.Location = new System.Drawing.Point(9, 7);
            this.Label_User.Name = "Label_User";
            this.Label_User.Size = new System.Drawing.Size(37, 13);
            this.Label_User.TabIndex = 16;
            this.Label_User.Text = "User:";
            // 
            // Label_Username
            // 
            this.Label_Username.AutoSize = true;
            this.Label_Username.Location = new System.Drawing.Point(40, 7);
            this.Label_Username.Name = "Label_Username";
            this.Label_Username.Size = new System.Drawing.Size(41, 13);
            this.Label_Username.TabIndex = 17;
            this.Label_Username.Text = "<User>";
            // 
            // Check_LogToDatabase
            // 
            this.Check_LogToDatabase.AutoSize = true;
            this.Check_LogToDatabase.Checked = true;
            this.Check_LogToDatabase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Check_LogToDatabase.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Check_LogToDatabase.Location = new System.Drawing.Point(12, 32);
            this.Check_LogToDatabase.Name = "Check_LogToDatabase";
            this.Check_LogToDatabase.Size = new System.Drawing.Size(103, 17);
            this.Check_LogToDatabase.TabIndex = 18;
            this.Check_LogToDatabase.Text = "Log to database";
            this.Check_LogToDatabase.UseVisualStyleBackColor = true;
            this.Check_LogToDatabase.CheckedChanged += new System.EventHandler(this.Check_LogToDatabase_CheckedChanged);
            // 
            // Dropdown_Test_List
            // 
            this.Dropdown_Test_List.FormattingEnabled = true;
            this.Dropdown_Test_List.Location = new System.Drawing.Point(281, 128);
            this.Dropdown_Test_List.Name = "Dropdown_Test_List";
            this.Dropdown_Test_List.Size = new System.Drawing.Size(121, 21);
            this.Dropdown_Test_List.TabIndex = 15;
            this.Dropdown_Test_List.Visible = false;
            // 
            // Check_SingleTest
            // 
            this.Check_SingleTest.AutoSize = true;
            this.Check_SingleTest.Location = new System.Drawing.Point(192, 132);
            this.Check_SingleTest.Name = "Check_SingleTest";
            this.Check_SingleTest.Size = new System.Drawing.Size(79, 17);
            this.Check_SingleTest.TabIndex = 13;
            this.Check_SingleTest.Text = "Single Test";
            this.Check_SingleTest.UseVisualStyleBackColor = true;
            this.Check_SingleTest.Visible = false;
            this.Check_SingleTest.CheckedChanged += new System.EventHandler(this.Check_SingleTest_CheckedChanged);
            // 
            // Check_FullTest
            // 
            this.Check_FullTest.AutoSize = true;
            this.Check_FullTest.Location = new System.Drawing.Point(87, 132);
            this.Check_FullTest.Name = "Check_FullTest";
            this.Check_FullTest.Size = new System.Drawing.Size(99, 17);
            this.Check_FullTest.TabIndex = 8;
            this.Check_FullTest.Text = "Functional Test";
            this.Check_FullTest.UseVisualStyleBackColor = true;
            // 
            // Check_Program
            // 
            this.Check_Program.AutoSize = true;
            this.Check_Program.Location = new System.Drawing.Point(16, 132);
            this.Check_Program.Name = "Check_Program";
            this.Check_Program.Size = new System.Drawing.Size(65, 17);
            this.Check_Program.TabIndex = 7;
            this.Check_Program.Text = "Program";
            this.Check_Program.UseVisualStyleBackColor = true;
            // 
            // Console_Admin
            // 
            this.Console_Admin.BackColor = System.Drawing.SystemColors.Window;
            this.Console_Admin.Location = new System.Drawing.Point(578, 195);
            this.Console_Admin.Name = "Console_Admin";
            this.Console_Admin.Size = new System.Drawing.Size(283, 20);
            this.Console_Admin.TabIndex = 19;
            this.Console_Admin.Visible = false;
            this.Console_Admin.WordWrap = false;
            // 
            // Logo_VLS
            // 
            this.Logo_VLS.Image = ((System.Drawing.Image)(resources.GetObject("Logo_VLS.Image")));
            this.Logo_VLS.InitialImage = ((System.Drawing.Image)(resources.GetObject("Logo_VLS.InitialImage")));
            this.Logo_VLS.Location = new System.Drawing.Point(541, 23);
            this.Logo_VLS.Margin = new System.Windows.Forms.Padding(10);
            this.Logo_VLS.Name = "Logo_VLS";
            this.Logo_VLS.Size = new System.Drawing.Size(350, 126);
            this.Logo_VLS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Logo_VLS.TabIndex = 26;
            this.Logo_VLS.TabStop = false;
            // 
            // Button_Jtag_Herc
            // 
            this.Button_Jtag_Herc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Button_Jtag_Herc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Jtag_Herc.Location = new System.Drawing.Point(263, 59);
            this.Button_Jtag_Herc.Name = "Button_Jtag_Herc";
            this.Button_Jtag_Herc.Size = new System.Drawing.Size(75, 23);
            this.Button_Jtag_Herc.TabIndex = 27;
            this.Button_Jtag_Herc.Text = "XDS220";
            this.Button_Jtag_Herc.UseVisualStyleBackColor = false;
            this.Button_Jtag_Herc.Click += new System.EventHandler(this.Button_Jtag_Herc_Click);
            // 
            // Button_Jtag_CPLD
            // 
            this.Button_Jtag_CPLD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Button_Jtag_CPLD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Jtag_CPLD.Location = new System.Drawing.Point(344, 58);
            this.Button_Jtag_CPLD.Name = "Button_Jtag_CPLD";
            this.Button_Jtag_CPLD.Size = new System.Drawing.Size(75, 23);
            this.Button_Jtag_CPLD.TabIndex = 28;
            this.Button_Jtag_CPLD.Text = "FlashPro";
            this.Button_Jtag_CPLD.UseVisualStyleBackColor = false;
            this.Button_Jtag_CPLD.Click += new System.EventHandler(this.Button_Jtag_CPLD_Click);
            // 
            // Button_SOM
            // 
            this.Button_SOM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Button_SOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_SOM.Location = new System.Drawing.Point(425, 59);
            this.Button_SOM.Name = "Button_SOM";
            this.Button_SOM.Size = new System.Drawing.Size(75, 23);
            this.Button_SOM.TabIndex = 29;
            this.Button_SOM.Text = "SOM";
            this.Button_SOM.UseVisualStyleBackColor = false;
            this.Button_SOM.Click += new System.EventHandler(this.Button_SOM_Click);
            // 
            // Button_DMM
            // 
            this.Button_DMM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Button_DMM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_DMM.Location = new System.Drawing.Point(20, 59);
            this.Button_DMM.Name = "Button_DMM";
            this.Button_DMM.Size = new System.Drawing.Size(75, 23);
            this.Button_DMM.TabIndex = 30;
            this.Button_DMM.Text = "DMM";
            this.Button_DMM.UseVisualStyleBackColor = false;
            this.Button_DMM.Click += new System.EventHandler(this.Button_DMM_Click);
            // 
            // Button_PPS
            // 
            this.Button_PPS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Button_PPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_PPS.Location = new System.Drawing.Point(101, 59);
            this.Button_PPS.Name = "Button_PPS";
            this.Button_PPS.Size = new System.Drawing.Size(75, 23);
            this.Button_PPS.TabIndex = 31;
            this.Button_PPS.Text = "PPS";
            this.Button_PPS.UseVisualStyleBackColor = false;
            this.Button_PPS.Click += new System.EventHandler(this.Button_PPS_Click);
            // 
            // Button_GPIO
            // 
            this.Button_GPIO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Button_GPIO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_GPIO.Location = new System.Drawing.Point(182, 59);
            this.Button_GPIO.Name = "Button_GPIO";
            this.Button_GPIO.Size = new System.Drawing.Size(75, 23);
            this.Button_GPIO.TabIndex = 32;
            this.Button_GPIO.Text = "GPIO";
            this.Button_GPIO.UseVisualStyleBackColor = false;
            this.Button_GPIO.Click += new System.EventHandler(this.Button_GPIO_Click);
            // 
            // GUI_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(220)))), ((int)(((byte)(222)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(913, 622);
            this.Controls.Add(this.Button_GPIO);
            this.Controls.Add(this.Button_PPS);
            this.Controls.Add(this.Button_DMM);
            this.Controls.Add(this.Button_SOM);
            this.Controls.Add(this.Button_Jtag_CPLD);
            this.Controls.Add(this.Button_Jtag_Herc);
            this.Controls.Add(this.Logo_VLS);
            this.Controls.Add(this.Console_Admin);
            this.Controls.Add(this.Check_LogToDatabase);
            this.Controls.Add(this.Label_Username);
            this.Controls.Add(this.Label_User);
            this.Controls.Add(this.Dropdown_Test_List);
            this.Controls.Add(this.Check_SingleTest);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.Button_No);
            this.Controls.Add(this.Button_Yes);
            this.Controls.Add(this.label_serialNumber);
            this.Controls.Add(this.console_debugOutput);
            this.Controls.Add(this.Button_Run);
            this.Controls.Add(this.Field_SerialNumber);
            this.Controls.Add(this.Check_FullTest);
            this.Controls.Add(this.Check_Program);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GUI_Main";
            this.Text = "Control Board Test";
            ((System.ComponentModel.ISupportInitialize)(this.Logo_VLS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Run;
        private System.Windows.Forms.TextBox Field_SerialNumber;
        private System.Windows.Forms.Label label_serialNumber;
        private System.Windows.Forms.RichTextBox console_debugOutput;
        private System.Windows.Forms.Button Button_Yes;
        private System.Windows.Forms.Button Button_No;
        private System.Windows.Forms.ProgressBar StatusBar;
        private System.Windows.Forms.Label Label_User;
        private System.Windows.Forms.Label Label_Username;
        private System.Windows.Forms.CheckBox Check_LogToDatabase;
        private System.Windows.Forms.ComboBox Dropdown_Test_List;
        private System.Windows.Forms.CheckBox Check_SingleTest;
        private System.Windows.Forms.CheckBox Check_FullTest;
        private System.Windows.Forms.CheckBox Check_Program;
        private System.Windows.Forms.TextBox Console_Admin;
        private System.Windows.Forms.PictureBox Logo_VLS;
        private System.Windows.Forms.Button Button_Jtag_Herc;
        private System.Windows.Forms.Button Button_Jtag_CPLD;
        private System.Windows.Forms.Button Button_SOM;
        private System.Windows.Forms.Button Button_DMM;
        private System.Windows.Forms.Button Button_PPS;
        private System.Windows.Forms.Button Button_GPIO;
    }
}

