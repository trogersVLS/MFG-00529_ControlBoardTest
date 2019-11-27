namespace ControlBoardTest
{
    partial class LoginForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.Label_Username = new System.Windows.Forms.Label();
            this.Label_Password = new System.Windows.Forms.Label();
            this.Button_Login = new System.Windows.Forms.Button();
            this.Button_Exit = new System.Windows.Forms.Button();
            this.Field_User = new System.Windows.Forms.TextBox();
            this.Field_Pass = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ToolName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Label_Username
            // 
            this.Label_Username.AutoSize = true;
            this.Label_Username.Location = new System.Drawing.Point(7, 78);
            this.Label_Username.Name = "Label_Username";
            this.Label_Username.Size = new System.Drawing.Size(55, 13);
            this.Label_Username.TabIndex = 5;
            this.Label_Username.Text = "Username";
            // 
            // Label_Password
            // 
            this.Label_Password.AutoSize = true;
            this.Label_Password.Location = new System.Drawing.Point(7, 103);
            this.Label_Password.Name = "Label_Password";
            this.Label_Password.Size = new System.Drawing.Size(53, 13);
            this.Label_Password.TabIndex = 6;
            this.Label_Password.Text = "Password";
            // 
            // Button_Login
            // 
            this.Button_Login.Location = new System.Drawing.Point(104, 131);
            this.Button_Login.Name = "Button_Login";
            this.Button_Login.Size = new System.Drawing.Size(75, 23);
            this.Button_Login.TabIndex = 2;
            this.Button_Login.Text = "Login";
            this.Button_Login.UseVisualStyleBackColor = true;
            this.Button_Login.Click += new System.EventHandler(this.ButtonLogin_Click);
            // 
            // Button_Exit
            // 
            this.Button_Exit.Location = new System.Drawing.Point(194, 131);
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(75, 23);
            this.Button_Exit.TabIndex = 3;
            this.Button_Exit.Text = "Exit";
            this.Button_Exit.UseVisualStyleBackColor = true;
            this.Button_Exit.Click += new System.EventHandler(this.ButtonExit_Click);
            // 
            // Field_User
            // 
            this.Field_User.Location = new System.Drawing.Point(65, 78);
            this.Field_User.Name = "Field_User";
            this.Field_User.Size = new System.Drawing.Size(248, 20);
            this.Field_User.TabIndex = 0;
            // 
            // Field_Pass
            // 
            this.Field_Pass.Location = new System.Drawing.Point(65, 100);
            this.Field_Pass.Name = "Field_Pass";
            this.Field_Pass.PasswordChar = '*';
            this.Field_Pass.Size = new System.Drawing.Size(248, 20);
            this.Field_Pass.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(65, 3);
            this.pictureBox1.MaximumSize = new System.Drawing.Size(700, 700);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(248, 69);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // ToolName
            // 
            this.ToolName.AutoSize = true;
            this.ToolName.Font = new System.Drawing.Font("Calibri", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToolName.Location = new System.Drawing.Point(250, 47);
            this.ToolName.Name = "ToolName";
            this.ToolName.Size = new System.Drawing.Size(62, 13);
            this.ToolName.TabIndex = 8;
            this.ToolName.Text = "MFG-00527";
            this.ToolName.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 166);
            this.Controls.Add(this.ToolName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Field_Pass);
            this.Controls.Add(this.Field_User);
            this.Controls.Add(this.Button_Exit);
            this.Controls.Add(this.Button_Login);
            this.Controls.Add(this.Label_Password);
            this.Controls.Add(this.Label_Username);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label_Username;
        private System.Windows.Forms.Label Label_Password;
        private System.Windows.Forms.Button Button_Login;
        private System.Windows.Forms.Button Button_Exit;
        private System.Windows.Forms.TextBox Field_User;
        private System.Windows.Forms.TextBox Field_Pass;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label ToolName;
    }
}
