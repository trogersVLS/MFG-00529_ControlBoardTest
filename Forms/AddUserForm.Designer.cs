namespace ControlBoardTest
{
    partial class AddUserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddUserForm));
            this.ToolName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Field_Pass = new System.Windows.Forms.TextBox();
            this.Field_User = new System.Windows.Forms.TextBox();
            this.Button_Exit = new System.Windows.Forms.Button();
            this.Button_Confirm = new System.Windows.Forms.Button();
            this.Label_Password = new System.Windows.Forms.Label();
            this.Label_Username = new System.Windows.Forms.Label();
            this.Check_Admin = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ToolName
            // 
            this.ToolName.AutoSize = true;
            this.ToolName.Font = new System.Drawing.Font("Calibri", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToolName.Location = new System.Drawing.Point(249, 57);
            this.ToolName.Name = "ToolName";
            this.ToolName.Size = new System.Drawing.Size(62, 13);
            this.ToolName.TabIndex = 26;
            this.ToolName.Text = "MFG-00527";
            this.ToolName.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(64, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(246, 70);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 27;
            this.pictureBox1.TabStop = false;
            // 
            // Field_Pass
            // 
            this.Field_Pass.Location = new System.Drawing.Point(65, 110);
            this.Field_Pass.Name = "Field_Pass";
            this.Field_Pass.PasswordChar = '*';
            this.Field_Pass.Size = new System.Drawing.Size(247, 20);
            this.Field_Pass.TabIndex = 2;
            // 
            // Field_User
            // 
            this.Field_User.Location = new System.Drawing.Point(64, 88);
            this.Field_User.Name = "Field_User";
            this.Field_User.Size = new System.Drawing.Size(248, 20);
            this.Field_User.TabIndex = 0;
            // 
            // Button_Exit
            // 
            this.Button_Exit.Location = new System.Drawing.Point(237, 170);
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(75, 23);
            this.Button_Exit.TabIndex = 4;
            this.Button_Exit.Text = "Exit";
            this.Button_Exit.UseVisualStyleBackColor = true;
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_Click);
            // 
            // Button_Confirm
            // 
            this.Button_Confirm.Location = new System.Drawing.Point(64, 170);
            this.Button_Confirm.Name = "Button_Confirm";
            this.Button_Confirm.Size = new System.Drawing.Size(75, 23);
            this.Button_Confirm.TabIndex = 3;
            this.Button_Confirm.Text = "Add User";
            this.Button_Confirm.UseVisualStyleBackColor = true;
            this.Button_Confirm.Click += new System.EventHandler(this.Button_Confirm_Click);
            // 
            // Label_Password
            // 
            this.Label_Password.AutoSize = true;
            this.Label_Password.Location = new System.Drawing.Point(6, 113);
            this.Label_Password.Name = "Label_Password";
            this.Label_Password.Size = new System.Drawing.Size(53, 13);
            this.Label_Password.TabIndex = 25;
            this.Label_Password.Text = "Password";
            // 
            // Label_Username
            // 
            this.Label_Username.AutoSize = true;
            this.Label_Username.Location = new System.Drawing.Point(6, 88);
            this.Label_Username.Name = "Label_Username";
            this.Label_Username.Size = new System.Drawing.Size(55, 13);
            this.Label_Username.TabIndex = 24;
            this.Label_Username.Text = "Username";
            // 
            // Check_Admin
            // 
            this.Check_Admin.AutoSize = true;
            this.Check_Admin.Location = new System.Drawing.Point(65, 136);
            this.Check_Admin.Name = "Check_Admin";
            this.Check_Admin.Size = new System.Drawing.Size(83, 17);
            this.Check_Admin.TabIndex = 3;
            this.Check_Admin.Text = "Administator";
            this.Check_Admin.UseVisualStyleBackColor = true;
            this.Check_Admin.CheckedChanged += new System.EventHandler(this.Check_Admin_CheckedChanged);
            // 
            // AddUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 209);
            this.Controls.Add(this.Check_Admin);
            this.Controls.Add(this.ToolName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Field_Pass);
            this.Controls.Add(this.Field_User);
            this.Controls.Add(this.Button_Exit);
            this.Controls.Add(this.Button_Confirm);
            this.Controls.Add(this.Label_Password);
            this.Controls.Add(this.Label_Username);
            this.Name = "AddUserForm";
            this.Text = "AddUserForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label ToolName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox Field_Pass;
        private System.Windows.Forms.TextBox Field_User;
        private System.Windows.Forms.Button Button_Exit;
        private System.Windows.Forms.Button Button_Confirm;
        private System.Windows.Forms.Label Label_Password;
        private System.Windows.Forms.Label Label_Username;
        private System.Windows.Forms.CheckBox Check_Admin;
    }
}