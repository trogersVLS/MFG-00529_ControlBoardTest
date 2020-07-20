namespace ControlBoardTest
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainPanel = new System.Windows.Forms.Panel();
            this.Panel_Status = new System.Windows.Forms.Panel();
            this.Button_PowerUp = new System.Windows.Forms.Button();
            this.Button_Telnet = new System.Windows.Forms.Button();
            this.PassCountIndicator = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.FailCountIndicator = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Panel_Actions = new System.Windows.Forms.Panel();
            this.Button_Run = new System.Windows.Forms.Button();
            this.Output_Window = new System.Windows.Forms.RichTextBox();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.Logo_VLS = new System.Windows.Forms.PictureBox();
            this.Panel_Settings = new System.Windows.Forms.FlowLayoutPanel();
            this.PN_Label = new System.Windows.Forms.Label();
            this.radioButtonVOCSN = new System.Windows.Forms.RadioButton();
            this.radioButtonVPRO = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.Field_SerialNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Check_FCT = new System.Windows.Forms.CheckBox();
            this.Check_SingleTest = new System.Windows.Forms.CheckBox();
            this.Dropdown_Test_List = new System.Windows.Forms.ComboBox();
            this.MenuBar = new System.Windows.Forms.MainMenu(this.components);
            this.Menu_Settings = new System.Windows.Forms.MenuItem();
            this.Settings_LoggingLabel = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.Menu_About = new System.Windows.Forms.MenuItem();
            this.About_VersionLabel = new System.Windows.Forms.MenuItem();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.Status_LocLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Status_LocationTag = new System.Windows.Forms.ToolStripStatusLabel();
            this.Status_ToolLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Status_ToolTag = new System.Windows.Forms.ToolStripStatusLabel();
            this.Status_UserLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Status_UserTag = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainPanel.SuspendLayout();
            this.Panel_Status.SuspendLayout();
            this.Panel_Actions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Logo_VLS)).BeginInit();
            this.Panel_Settings.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.Panel_Status);
            this.MainPanel.Controls.Add(this.Panel_Actions);
            this.MainPanel.Controls.Add(this.Output_Window);
            this.MainPanel.Controls.Add(this.ProgressBar);
            this.MainPanel.Controls.Add(this.Logo_VLS);
            this.MainPanel.Controls.Add(this.Panel_Settings);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Margin = new System.Windows.Forms.Padding(7);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1456, 1208);
            this.MainPanel.TabIndex = 31;
            // 
            // Panel_Status
            // 
            this.Panel_Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_Status.Controls.Add(this.Button_PowerUp);
            this.Panel_Status.Controls.Add(this.Button_Telnet);
            this.Panel_Status.Controls.Add(this.PassCountIndicator);
            this.Panel_Status.Controls.Add(this.label3);
            this.Panel_Status.Controls.Add(this.FailCountIndicator);
            this.Panel_Status.Controls.Add(this.label6);
            this.Panel_Status.Controls.Add(this.label5);
            this.Panel_Status.Controls.Add(this.label4);
            this.Panel_Status.Location = new System.Drawing.Point(936, 203);
            this.Panel_Status.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Panel_Status.Name = "Panel_Status";
            this.Panel_Status.Size = new System.Drawing.Size(495, 431);
            this.Panel_Status.TabIndex = 2;
            // 
            // Button_PowerUp
            // 
            this.Button_PowerUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_PowerUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Button_PowerUp.Enabled = false;
            this.Button_PowerUp.FlatAppearance.BorderSize = 0;
            this.Button_PowerUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_PowerUp.Location = new System.Drawing.Point(226, 13);
            this.Button_PowerUp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Button_PowerUp.Name = "Button_PowerUp";
            this.Button_PowerUp.Size = new System.Drawing.Size(245, 87);
            this.Button_PowerUp.TabIndex = 31;
            this.Button_PowerUp.Text = "Not Powered";
            this.Button_PowerUp.UseVisualStyleBackColor = false;
            this.Button_PowerUp.Click += new System.EventHandler(this.Button_PowerUp_Click);
            // 
            // Button_Telnet
            // 
            this.Button_Telnet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Telnet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Button_Telnet.Enabled = false;
            this.Button_Telnet.FlatAppearance.BorderSize = 0;
            this.Button_Telnet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Telnet.Location = new System.Drawing.Point(226, 120);
            this.Button_Telnet.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Button_Telnet.Name = "Button_Telnet";
            this.Button_Telnet.Size = new System.Drawing.Size(245, 83);
            this.Button_Telnet.TabIndex = 32;
            this.Button_Telnet.Text = "Not Connected";
            this.Button_Telnet.UseVisualStyleBackColor = false;
            // 
            // PassCountIndicator
            // 
            this.PassCountIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PassCountIndicator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.PassCountIndicator.Enabled = false;
            this.PassCountIndicator.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PassCountIndicator.Location = new System.Drawing.Point(226, 228);
            this.PassCountIndicator.Margin = new System.Windows.Forms.Padding(7);
            this.PassCountIndicator.Name = "PassCountIndicator";
            this.PassCountIndicator.Size = new System.Drawing.Size(245, 83);
            this.PassCountIndicator.TabIndex = 2;
            this.PassCountIndicator.Text = "0";
            this.PassCountIndicator.UseVisualStyleBackColor = false;
            this.PassCountIndicator.Click += new System.EventHandler(this.PassCountIndicator_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 40);
            this.label3.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 29);
            this.label3.TabIndex = 33;
            this.label3.Text = "Power Status:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FailCountIndicator
            // 
            this.FailCountIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FailCountIndicator.BackColor = System.Drawing.Color.DarkGray;
            this.FailCountIndicator.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FailCountIndicator.Location = new System.Drawing.Point(226, 335);
            this.FailCountIndicator.Margin = new System.Windows.Forms.Padding(7);
            this.FailCountIndicator.Name = "FailCountIndicator";
            this.FailCountIndicator.Size = new System.Drawing.Size(245, 83);
            this.FailCountIndicator.TabIndex = 3;
            this.FailCountIndicator.Text = "0";
            this.FailCountIndicator.UseVisualStyleBackColor = false;
            this.FailCountIndicator.Click += new System.EventHandler(this.FailCountIndicator_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(30, 361);
            this.label6.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(171, 29);
            this.label6.TabIndex = 36;
            this.label6.Text = "Failed Tests:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 254);
            this.label5.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(187, 29);
            this.label5.TabIndex = 35;
            this.label5.Text = "Passed Tests:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 147);
            this.label4.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(183, 29);
            this.label4.TabIndex = 34;
            this.label4.Text = "Telnet Status:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Panel_Actions
            // 
            this.Panel_Actions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_Actions.Controls.Add(this.Button_Run);
            this.Panel_Actions.Location = new System.Drawing.Point(642, 203);
            this.Panel_Actions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Panel_Actions.Name = "Panel_Actions";
            this.Panel_Actions.Size = new System.Drawing.Size(289, 431);
            this.Panel_Actions.TabIndex = 30;
            // 
            // Button_Run
            // 
            this.Button_Run.BackColor = System.Drawing.Color.SkyBlue;
            this.Button_Run.Dock = System.Windows.Forms.DockStyle.Left;
            this.Button_Run.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Run.Location = new System.Drawing.Point(0, 0);
            this.Button_Run.Margin = new System.Windows.Forms.Padding(7);
            this.Button_Run.Name = "Button_Run";
            this.Button_Run.Size = new System.Drawing.Size(282, 431);
            this.Button_Run.TabIndex = 1;
            this.Button_Run.Text = "Start";
            this.Button_Run.UseVisualStyleBackColor = false;
            this.Button_Run.Click += new System.EventHandler(this.Button_Run_Click);
            // 
            // Output_Window
            // 
            this.Output_Window.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Output_Window.Location = new System.Drawing.Point(28, 716);
            this.Output_Window.Margin = new System.Windows.Forms.Padding(7);
            this.Output_Window.Name = "Output_Window";
            this.Output_Window.ReadOnly = true;
            this.Output_Window.Size = new System.Drawing.Size(1399, 478);
            this.Output_Window.TabIndex = 9;
            this.Output_Window.Text = "";
            this.Output_Window.TextChanged += new System.EventHandler(this.Output_Window_TextChanged);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar.Location = new System.Drawing.Point(28, 645);
            this.ProgressBar.Margin = new System.Windows.Forms.Padding(7);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(1400, 58);
            this.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ProgressBar.TabIndex = 12;
            // 
            // Logo_VLS
            // 
            this.Logo_VLS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Logo_VLS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Logo_VLS.Image = ((System.Drawing.Image)(resources.GetObject("Logo_VLS.Image")));
            this.Logo_VLS.InitialImage = ((System.Drawing.Image)(resources.GetObject("Logo_VLS.InitialImage")));
            this.Logo_VLS.Location = new System.Drawing.Point(28, 20);
            this.Logo_VLS.Margin = new System.Windows.Forms.Padding(0);
            this.Logo_VLS.Name = "Logo_VLS";
            this.Logo_VLS.Size = new System.Drawing.Size(1400, 170);
            this.Logo_VLS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Logo_VLS.TabIndex = 26;
            this.Logo_VLS.TabStop = false;
            // 
            // Panel_Settings
            // 
            this.Panel_Settings.Controls.Add(this.PN_Label);
            this.Panel_Settings.Controls.Add(this.radioButtonVOCSN);
            this.Panel_Settings.Controls.Add(this.radioButtonVPRO);
            this.Panel_Settings.Controls.Add(this.label1);
            this.Panel_Settings.Controls.Add(this.Field_SerialNumber);
            this.Panel_Settings.Controls.Add(this.label2);
            this.Panel_Settings.Controls.Add(this.Check_FCT);
            this.Panel_Settings.Controls.Add(this.Check_SingleTest);
            this.Panel_Settings.Controls.Add(this.Dropdown_Test_List);
            this.Panel_Settings.Location = new System.Drawing.Point(28, 203);
            this.Panel_Settings.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Panel_Settings.Name = "Panel_Settings";
            this.Panel_Settings.Size = new System.Drawing.Size(590, 431);
            this.Panel_Settings.TabIndex = 0;
            // 
            // PN_Label
            // 
            this.PN_Label.AutoSize = true;
            this.Panel_Settings.SetFlowBreak(this.PN_Label, true);
            this.PN_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PN_Label.Location = new System.Drawing.Point(7, 11);
            this.PN_Label.Margin = new System.Windows.Forms.Padding(7, 11, 7, 7);
            this.PN_Label.Name = "PN_Label";
            this.PN_Label.Size = new System.Drawing.Size(95, 29);
            this.PN_Label.TabIndex = 34;
            this.PN_Label.Text = "Model:";
            // 
            // radioButtonVOCSN
            // 
            this.radioButtonVOCSN.Checked = true;
            this.radioButtonVOCSN.Location = new System.Drawing.Point(7, 59);
            this.radioButtonVOCSN.Margin = new System.Windows.Forms.Padding(7);
            this.radioButtonVOCSN.Name = "radioButtonVOCSN";
            this.radioButtonVOCSN.Size = new System.Drawing.Size(189, 38);
            this.radioButtonVOCSN.TabIndex = 35;
            this.radioButtonVOCSN.TabStop = true;
            this.radioButtonVOCSN.Text = "VOCSN Pro";
            this.radioButtonVOCSN.UseVisualStyleBackColor = true;
            this.radioButtonVOCSN.CheckedChanged += new System.EventHandler(this.radioButtonVOCSN_CheckedChanged);
            // 
            // radioButtonVPRO
            // 
            this.radioButtonVPRO.Location = new System.Drawing.Point(210, 59);
            this.radioButtonVPRO.Margin = new System.Windows.Forms.Padding(7);
            this.radioButtonVPRO.Name = "radioButtonVPRO";
            this.radioButtonVPRO.Size = new System.Drawing.Size(271, 38);
            this.radioButtonVPRO.TabIndex = 36;
            this.radioButtonVPRO.TabStop = true;
            this.radioButtonVPRO.Text = "V Pro  (DEPOP)";
            this.radioButtonVPRO.UseVisualStyleBackColor = true;
            this.radioButtonVPRO.CheckedChanged += new System.EventHandler(this.radioButtonVPRO_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.Panel_Settings.SetFlowBreak(this.label1, true);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 115);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 11, 7, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 29);
            this.label1.TabIndex = 33;
            this.label1.Text = "Serial Number:";
            // 
            // Field_SerialNumber
            // 
            this.Field_SerialNumber.BackColor = System.Drawing.SystemColors.Window;
            this.Field_SerialNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Field_SerialNumber.Location = new System.Drawing.Point(7, 158);
            this.Field_SerialNumber.Margin = new System.Windows.Forms.Padding(7);
            this.Field_SerialNumber.Name = "Field_SerialNumber";
            this.Field_SerialNumber.Size = new System.Drawing.Size(569, 35);
            this.Field_SerialNumber.TabIndex = 0;
            this.Field_SerialNumber.TextChanged += new System.EventHandler(this.Field_SerialNumber_TextChanged);
            this.Field_SerialNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Field_SerialNumber_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.Panel_Settings.SetFlowBreak(this.label2, true);
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 211);
            this.label2.Margin = new System.Windows.Forms.Padding(7, 11, 7, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(284, 29);
            this.label2.TabIndex = 33;
            this.label2.Text = "Functional Test Mode:";
            // 
            // Check_FCT
            // 
            this.Check_FCT.AutoSize = true;
            this.Check_FCT.Checked = true;
            this.Check_FCT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Check_FCT.Enabled = false;
            this.Panel_Settings.SetFlowBreak(this.Check_FCT, true);
            this.Check_FCT.Location = new System.Drawing.Point(35, 254);
            this.Check_FCT.Margin = new System.Windows.Forms.Padding(35, 7, 7, 7);
            this.Check_FCT.Name = "Check_FCT";
            this.Check_FCT.Size = new System.Drawing.Size(139, 33);
            this.Check_FCT.TabIndex = 2;
            this.Check_FCT.Text = "Full Test";
            this.Check_FCT.UseVisualStyleBackColor = true;
            this.Check_FCT.CheckedChanged += new System.EventHandler(this.Check_Functional_CheckedChanged);
            // 
            // Check_SingleTest
            // 
            this.Check_SingleTest.AutoSize = true;
            this.Check_SingleTest.Enabled = false;
            this.Panel_Settings.SetFlowBreak(this.Check_SingleTest, true);
            this.Check_SingleTest.Location = new System.Drawing.Point(35, 301);
            this.Check_SingleTest.Margin = new System.Windows.Forms.Padding(35, 7, 7, 7);
            this.Check_SingleTest.Name = "Check_SingleTest";
            this.Check_SingleTest.Size = new System.Drawing.Size(168, 33);
            this.Check_SingleTest.TabIndex = 3;
            this.Check_SingleTest.Text = "Single Test";
            this.Check_SingleTest.UseVisualStyleBackColor = true;
            this.Check_SingleTest.CheckedChanged += new System.EventHandler(this.Check_SingleTest_CheckedChanged);
            // 
            // Dropdown_Test_List
            // 
            this.Dropdown_Test_List.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Dropdown_Test_List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Dropdown_Test_List.Enabled = false;
            this.Dropdown_Test_List.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dropdown_Test_List.FormattingEnabled = true;
            this.Dropdown_Test_List.Location = new System.Drawing.Point(35, 348);
            this.Dropdown_Test_List.Margin = new System.Windows.Forms.Padding(35, 7, 7, 7);
            this.Dropdown_Test_List.Name = "Dropdown_Test_List";
            this.Dropdown_Test_List.Size = new System.Drawing.Size(541, 40);
            this.Dropdown_Test_List.TabIndex = 4;
            this.Dropdown_Test_List.SelectedIndexChanged += new System.EventHandler(this.Dropdown_Test_List_SelectedIndexChanged);
            // 
            // MenuBar
            // 
            this.MenuBar.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.Menu_Settings,
            this.Menu_About});
            // 
            // Menu_Settings
            // 
            this.Menu_Settings.Index = 0;
            this.Menu_Settings.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.Settings_LoggingLabel,
            this.menuItem1});
            this.Menu_Settings.Text = "Settings";
            // 
            // Settings_LoggingLabel
            // 
            this.Settings_LoggingLabel.Checked = true;
            this.Settings_LoggingLabel.Index = 0;
            this.Settings_LoggingLabel.RadioCheck = true;
            this.Settings_LoggingLabel.Text = "Log to database";
            this.Settings_LoggingLabel.Click += new System.EventHandler(this.Logging_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "Configuration";
            this.menuItem1.Click += new System.EventHandler(this.Configuration_Click);
            // 
            // Menu_About
            // 
            this.Menu_About.Index = 1;
            this.Menu_About.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.About_VersionLabel});
            this.Menu_About.Text = "About";
            // 
            // About_VersionLabel
            // 
            this.About_VersionLabel.Index = 0;
            this.About_VersionLabel.Text = "Version";
            this.About_VersionLabel.Click += new System.EventHandler(this.About_Version_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status_LocLabel,
            this.Status_LocationTag,
            this.Status_ToolLabel,
            this.Status_ToolTag,
            this.Status_UserLabel,
            this.Status_UserTag});
            this.StatusBar.Location = new System.Drawing.Point(0, 1208);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.StatusBar.Size = new System.Drawing.Size(1456, 48);
            this.StatusBar.TabIndex = 27;
            this.StatusBar.Text = "StatusStrip";
            // 
            // Status_LocLabel
            // 
            this.Status_LocLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.Status_LocLabel.Name = "Status_LocLabel";
            this.Status_LocLabel.Size = new System.Drawing.Size(135, 37);
            this.Status_LocLabel.Text = "Location:";
            // 
            // Status_LocationTag
            // 
            this.Status_LocationTag.Name = "Status_LocationTag";
            this.Status_LocationTag.Size = new System.Drawing.Size(149, 37);
            this.Status_LocationTag.Text = "<location>";
            // 
            // Status_ToolLabel
            // 
            this.Status_ToolLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.Status_ToolLabel.Name = "Status_ToolLabel";
            this.Status_ToolLabel.Size = new System.Drawing.Size(116, 37);
            this.Status_ToolLabel.Text = "Tool ID:";
            // 
            // Status_ToolTag
            // 
            this.Status_ToolTag.Name = "Status_ToolTag";
            this.Status_ToolTag.Size = new System.Drawing.Size(101, 37);
            this.Status_ToolTag.Text = "<tool>";
            // 
            // Status_UserLabel
            // 
            this.Status_UserLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.Status_UserLabel.Name = "Status_UserLabel";
            this.Status_UserLabel.Size = new System.Drawing.Size(83, 37);
            this.Status_UserLabel.Text = "User:";
            // 
            // Status_UserTag
            // 
            this.Status_UserTag.Name = "Status_UserTag";
            this.Status_UserTag.Size = new System.Drawing.Size(102, 37);
            this.Status_UserTag.Text = "<user>";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1456, 1256);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.StatusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(7);
            this.Menu = this.MenuBar;
            this.MinimumSize = new System.Drawing.Size(1456, 989);
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Control Board Functional Test";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.MainPanel.ResumeLayout(false);
            this.Panel_Status.ResumeLayout(false);
            this.Panel_Status.PerformLayout();
            this.Panel_Actions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Logo_VLS)).EndInit();
            this.Panel_Settings.ResumeLayout(false);
            this.Panel_Settings.PerformLayout();
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Run;
        private System.Windows.Forms.TextBox Field_SerialNumber;
        private System.Windows.Forms.RichTextBox Output_Window;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.ComboBox Dropdown_Test_List;
        private System.Windows.Forms.PictureBox Logo_VLS;
        private System.Windows.Forms.MainMenu MenuBar;
        private System.Windows.Forms.MenuItem Menu_Settings;
        private System.Windows.Forms.MenuItem Menu_About;
        private System.Windows.Forms.MenuItem About_VersionLabel;
        private System.Windows.Forms.StatusStrip StatusBar;
        
        private System.Windows.Forms.MenuItem Settings_LoggingLabel;
        private System.Windows.Forms.FlowLayoutPanel Panel_Settings;

        private System.Windows.Forms.ToolStripStatusLabel Status_LocLabel;
        private System.Windows.Forms.ToolStripStatusLabel Status_LocationTag;
        private System.Windows.Forms.ToolStripStatusLabel Status_UserLabel;
        private System.Windows.Forms.ToolStripStatusLabel Status_UserTag;
        private System.Windows.Forms.ToolStripStatusLabel Status_ToolLabel;
        private System.Windows.Forms.ToolStripStatusLabel Status_ToolTag;
        private System.Windows.Forms.Button Button_Telnet;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Button FailCountIndicator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button PassCountIndicator;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel Panel_Status;
        private System.Windows.Forms.Button Button_PowerUp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel Panel_Actions;
        private System.Windows.Forms.Label PN_Label;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.RadioButton radioButtonVOCSN;
        private System.Windows.Forms.RadioButton radioButtonVPRO;
        private System.Windows.Forms.CheckBox Check_FCT;
        private System.Windows.Forms.CheckBox Check_SingleTest;
    }
}

