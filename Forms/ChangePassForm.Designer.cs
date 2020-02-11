namespace ControlBoardTest
{
    partial class ChangePassForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePassForm));
            this.ToolName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Field_NewPass = new System.Windows.Forms.TextBox();
            this.Field_OldPass = new System.Windows.Forms.TextBox();
            this.Button_Exit = new System.Windows.Forms.Button();
            this.Button_Confirm = new System.Windows.Forms.Button();
            this.Label_Password = new System.Windows.Forms.Label();
            this.Label_Username = new System.Windows.Forms.Label();
            this.Field_VerfPass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ToolName
            // 
            this.ToolName.AutoSize = true;
            this.ToolName.Font = new System.Drawing.Font("Calibri", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToolName.Location = new System.Drawing.Point(250, 57);
            this.ToolName.Name = "ToolName";
            this.ToolName.Size = new System.Drawing.Size(62, 13);
            this.ToolName.TabIndex = 16;
            this.ToolName.Text = "MFG-00527";
            this.ToolName.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(65, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(246, 70);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // Field_NewPass
            // 
            this.Field_NewPass.Location = new System.Drawing.Point(135, 110);
            this.Field_NewPass.Name = "Field_NewPass";
            this.Field_NewPass.PasswordChar = '*';
            this.Field_NewPass.Size = new System.Drawing.Size(178, 20);
            this.Field_NewPass.TabIndex = 1;
            // 
            // Field_OldPass
            // 
            this.Field_OldPass.Location = new System.Drawing.Point(135, 88);
            this.Field_OldPass.Name = "Field_OldPass";
            this.Field_OldPass.PasswordChar = '*';
            this.Field_OldPass.Size = new System.Drawing.Size(178, 20);
            this.Field_OldPass.TabIndex = 0;
            // 
            // Button_Exit
            // 
            this.Button_Exit.Location = new System.Drawing.Point(238, 170);
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(75, 23);
            this.Button_Exit.TabIndex = 4;
            this.Button_Exit.Text = "Exit";
            this.Button_Exit.UseVisualStyleBackColor = true;
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_Click);
            // 
            // Button_Confirm
            // 
            this.Button_Confirm.Location = new System.Drawing.Point(135, 170);
            this.Button_Confirm.Name = "Button_Confirm";
            this.Button_Confirm.Size = new System.Drawing.Size(75, 23);
            this.Button_Confirm.TabIndex = 3;
            this.Button_Confirm.Text = "Confirm";
            this.Button_Confirm.UseVisualStyleBackColor = true;
            this.Button_Confirm.Click += new System.EventHandler(this.Button_Confirm_Click);
            // 
            // Label_Password
            // 
            this.Label_Password.AutoSize = true;
            this.Label_Password.Location = new System.Drawing.Point(7, 113);
            this.Label_Password.Name = "Label_Password";
            this.Label_Password.Size = new System.Drawing.Size(78, 13);
            this.Label_Password.TabIndex = 15;
            this.Label_Password.Text = "New Password";
            // 
            // Label_Username
            // 
            this.Label_Username.AutoSize = true;
            this.Label_Username.Location = new System.Drawing.Point(7, 88);
            this.Label_Username.Name = "Label_Username";
            this.Label_Username.Size = new System.Drawing.Size(72, 13);
            this.Label_Username.TabIndex = 14;
            this.Label_Username.Text = "Old Password";
            // 
            // Field_VerfPass
            // 
            this.Field_VerfPass.Location = new System.Drawing.Point(135, 132);
            this.Field_VerfPass.Name = "Field_VerfPass";
            this.Field_VerfPass.PasswordChar = '*';
            this.Field_VerfPass.Size = new System.Drawing.Size(178, 20);
            this.Field_VerfPass.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Re-Enter New Password";
            // 
            // ChangePassForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 205);
            this.Controls.Add(this.Field_VerfPass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ToolName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Field_NewPass);
            this.Controls.Add(this.Field_OldPass);
            this.Controls.Add(this.Button_Exit);
            this.Controls.Add(this.Button_Confirm);
            this.Controls.Add(this.Label_Password);
            this.Controls.Add(this.Label_Username);
            this.Name = "ChangePassForm";
            this.Text = "Change Password";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ToolName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox Field_NewPass;
        private System.Windows.Forms.TextBox Field_OldPass;
        private System.Windows.Forms.Button Button_Exit;
        private System.Windows.Forms.Button Button_Confirm;
        private System.Windows.Forms.Label Label_Password;
        private System.Windows.Forms.Label Label_Username;
        private System.Windows.Forms.TextBox Field_VerfPass;
        private System.Windows.Forms.Label label1;
    }
}