namespace ControlBoardTest
{
    partial class ConfigurationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            this.ButtonSave = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.Group_DMM = new System.Windows.Forms.GroupBox();
            this.Label_BaudDMM = new System.Windows.Forms.Label();
            this.Label_NameDMM = new System.Windows.Forms.Label();
            this.Label_StopBitsDMM = new System.Windows.Forms.Label();
            this.Label_AddressDMM = new System.Windows.Forms.Label();
            this.Input_NameDmm = new System.Windows.Forms.TextBox();
            this.Input_BaudDmm = new System.Windows.Forms.TextBox();
            this.Input_StopBitsDmm = new System.Windows.Forms.TextBox();
            this.Label_DMM = new System.Windows.Forms.Label();
            this.Input_AddressDmm = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Input_AddressSom = new System.Windows.Forms.TextBox();
            this.Group_PPS = new System.Windows.Forms.GroupBox();
            this.Label_BaudPPS = new System.Windows.Forms.Label();
            this.Label_PowerSupply = new System.Windows.Forms.Label();
            this.Label_NamePPS = new System.Windows.Forms.Label();
            this.Input_AddressPps = new System.Windows.Forms.TextBox();
            this.Label_StopBitsPPS = new System.Windows.Forms.Label();
            this.Input_StopBitsPps = new System.Windows.Forms.TextBox();
            this.Label_AddressPPS = new System.Windows.Forms.Label();
            this.Input_BaudPps = new System.Windows.Forms.TextBox();
            this.Input_NamePps = new System.Windows.Forms.TextBox();
            this.Group_Settings = new System.Windows.Forms.GroupBox();
            this.Check_EnableDHCP = new System.Windows.Forms.CheckBox();
            this.Label_StopDHCP = new System.Windows.Forms.Label();
            this.Input_DhcpStop = new System.Windows.Forms.TextBox();
            this.Label_MfgCode = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Input_Location = new System.Windows.Forms.TextBox();
            this.Label_StartDHCP = new System.Windows.Forms.Label();
            this.Input_DhcpStart = new System.Windows.Forms.TextBox();
            this.Label_Location = new System.Windows.Forms.Label();
            this.Input_MfgCode = new System.Windows.Forms.TextBox();
            this.Input_Eqid = new System.Windows.Forms.TextBox();
            this.Group_DMM.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.Group_PPS.SuspendLayout();
            this.Group_Settings.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonSave
            // 
            this.ButtonSave.Location = new System.Drawing.Point(224, 502);
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(110, 46);
            this.ButtonSave.TabIndex = 4;
            this.ButtonSave.Text = "Save";
            this.ButtonSave.UseVisualStyleBackColor = true;
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(72, 502);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(110, 46);
            this.ButtonCancel.TabIndex = 5;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // Group_DMM
            // 
            this.Group_DMM.Controls.Add(this.Label_BaudDMM);
            this.Group_DMM.Controls.Add(this.Label_NameDMM);
            this.Group_DMM.Controls.Add(this.Label_StopBitsDMM);
            this.Group_DMM.Controls.Add(this.Label_AddressDMM);
            this.Group_DMM.Controls.Add(this.Input_NameDmm);
            this.Group_DMM.Controls.Add(this.Input_BaudDmm);
            this.Group_DMM.Controls.Add(this.Input_StopBitsDmm);
            this.Group_DMM.Controls.Add(this.Label_DMM);
            this.Group_DMM.Controls.Add(this.Input_AddressDmm);
            this.Group_DMM.Location = new System.Drawing.Point(12, 219);
            this.Group_DMM.Name = "Group_DMM";
            this.Group_DMM.Size = new System.Drawing.Size(184, 150);
            this.Group_DMM.TabIndex = 1;
            this.Group_DMM.TabStop = false;
            // 
            // Label_BaudDMM
            // 
            this.Label_BaudDMM.AutoSize = true;
            this.Label_BaudDMM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_BaudDMM.Location = new System.Drawing.Point(7, 101);
            this.Label_BaudDMM.Name = "Label_BaudDMM";
            this.Label_BaudDMM.Size = new System.Drawing.Size(36, 13);
            this.Label_BaudDMM.TabIndex = 13;
            this.Label_BaudDMM.Text = "Baud";
            // 
            // Label_NameDMM
            // 
            this.Label_NameDMM.AutoSize = true;
            this.Label_NameDMM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_NameDMM.Location = new System.Drawing.Point(7, 75);
            this.Label_NameDMM.Name = "Label_NameDMM";
            this.Label_NameDMM.Size = new System.Drawing.Size(57, 13);
            this.Label_NameDMM.TabIndex = 10;
            this.Label_NameDMM.Text = "Identifier";
            // 
            // Label_StopBitsDMM
            // 
            this.Label_StopBitsDMM.AutoSize = true;
            this.Label_StopBitsDMM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_StopBitsDMM.Location = new System.Drawing.Point(7, 127);
            this.Label_StopBitsDMM.Name = "Label_StopBitsDMM";
            this.Label_StopBitsDMM.Size = new System.Drawing.Size(58, 13);
            this.Label_StopBitsDMM.TabIndex = 9;
            this.Label_StopBitsDMM.Text = "Stop Bits";
            // 
            // Label_AddressDMM
            // 
            this.Label_AddressDMM.AutoSize = true;
            this.Label_AddressDMM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_AddressDMM.Location = new System.Drawing.Point(7, 52);
            this.Label_AddressDMM.Name = "Label_AddressDMM";
            this.Label_AddressDMM.Size = new System.Drawing.Size(52, 13);
            this.Label_AddressDMM.TabIndex = 5;
            this.Label_AddressDMM.Text = "Address";
            // 
            // Input_NameDmm
            // 
            this.Input_NameDmm.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_NameDmm.Location = new System.Drawing.Point(70, 72);
            this.Input_NameDmm.Name = "Input_NameDmm";
            this.Input_NameDmm.Size = new System.Drawing.Size(100, 20);
            this.Input_NameDmm.TabIndex = 1;
            // 
            // Input_BaudDmm
            // 
            this.Input_BaudDmm.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_BaudDmm.Location = new System.Drawing.Point(70, 98);
            this.Input_BaudDmm.Name = "Input_BaudDmm";
            this.Input_BaudDmm.Size = new System.Drawing.Size(100, 20);
            this.Input_BaudDmm.TabIndex = 2;
            // 
            // Input_StopBitsDmm
            // 
            this.Input_StopBitsDmm.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_StopBitsDmm.Location = new System.Drawing.Point(70, 124);
            this.Input_StopBitsDmm.Name = "Input_StopBitsDmm";
            this.Input_StopBitsDmm.Size = new System.Drawing.Size(100, 20);
            this.Input_StopBitsDmm.TabIndex = 3;
            // 
            // Label_DMM
            // 
            this.Label_DMM.AutoSize = true;
            this.Label_DMM.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_DMM.Location = new System.Drawing.Point(6, 16);
            this.Label_DMM.Name = "Label_DMM";
            this.Label_DMM.Size = new System.Drawing.Size(171, 22);
            this.Label_DMM.TabIndex = 6;
            this.Label_DMM.Text = "Digital Multimeter";
            // 
            // Input_AddressDmm
            // 
            this.Input_AddressDmm.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_AddressDmm.Location = new System.Drawing.Point(70, 46);
            this.Input_AddressDmm.Name = "Input_AddressDmm";
            this.Input_AddressDmm.Size = new System.Drawing.Size(100, 20);
            this.Input_AddressDmm.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.Input_AddressSom);
            this.groupBox3.Location = new System.Drawing.Point(12, 375);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(184, 83);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 22);
            this.label2.TabIndex = 9;
            this.label2.Text = "SOM";
            // 
            // Input_AddressSom
            // 
            this.Input_AddressSom.Location = new System.Drawing.Point(70, 46);
            this.Input_AddressSom.Name = "Input_AddressSom";
            this.Input_AddressSom.Size = new System.Drawing.Size(100, 20);
            this.Input_AddressSom.TabIndex = 0;
            // 
            // Group_PPS
            // 
            this.Group_PPS.Controls.Add(this.Label_BaudPPS);
            this.Group_PPS.Controls.Add(this.Label_PowerSupply);
            this.Group_PPS.Controls.Add(this.Label_NamePPS);
            this.Group_PPS.Controls.Add(this.Input_AddressPps);
            this.Group_PPS.Controls.Add(this.Label_StopBitsPPS);
            this.Group_PPS.Controls.Add(this.Input_StopBitsPps);
            this.Group_PPS.Controls.Add(this.Label_AddressPPS);
            this.Group_PPS.Controls.Add(this.Input_BaudPps);
            this.Group_PPS.Controls.Add(this.Input_NamePps);
            this.Group_PPS.Location = new System.Drawing.Point(206, 219);
            this.Group_PPS.Name = "Group_PPS";
            this.Group_PPS.Size = new System.Drawing.Size(184, 150);
            this.Group_PPS.TabIndex = 2;
            this.Group_PPS.TabStop = false;
            // 
            // Label_BaudPPS
            // 
            this.Label_BaudPPS.AutoSize = true;
            this.Label_BaudPPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_BaudPPS.Location = new System.Drawing.Point(7, 101);
            this.Label_BaudPPS.Name = "Label_BaudPPS";
            this.Label_BaudPPS.Size = new System.Drawing.Size(36, 13);
            this.Label_BaudPPS.TabIndex = 22;
            this.Label_BaudPPS.Text = "Baud";
            // 
            // Label_PowerSupply
            // 
            this.Label_PowerSupply.AutoSize = true;
            this.Label_PowerSupply.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_PowerSupply.Location = new System.Drawing.Point(6, 16);
            this.Label_PowerSupply.Name = "Label_PowerSupply";
            this.Label_PowerSupply.Size = new System.Drawing.Size(139, 22);
            this.Label_PowerSupply.TabIndex = 19;
            this.Label_PowerSupply.Text = "Power Supply";
            // 
            // Label_NamePPS
            // 
            this.Label_NamePPS.AutoSize = true;
            this.Label_NamePPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_NamePPS.Location = new System.Drawing.Point(7, 75);
            this.Label_NamePPS.Name = "Label_NamePPS";
            this.Label_NamePPS.Size = new System.Drawing.Size(57, 13);
            this.Label_NamePPS.TabIndex = 21;
            this.Label_NamePPS.Text = "Identifier";
            // 
            // Input_AddressPps
            // 
            this.Input_AddressPps.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_AddressPps.Location = new System.Drawing.Point(70, 46);
            this.Input_AddressPps.Name = "Input_AddressPps";
            this.Input_AddressPps.Size = new System.Drawing.Size(100, 20);
            this.Input_AddressPps.TabIndex = 0;
            // 
            // Label_StopBitsPPS
            // 
            this.Label_StopBitsPPS.AutoSize = true;
            this.Label_StopBitsPPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_StopBitsPPS.Location = new System.Drawing.Point(7, 127);
            this.Label_StopBitsPPS.Name = "Label_StopBitsPPS";
            this.Label_StopBitsPPS.Size = new System.Drawing.Size(58, 13);
            this.Label_StopBitsPPS.TabIndex = 20;
            this.Label_StopBitsPPS.Text = "Stop Bits";
            // 
            // Input_StopBitsPps
            // 
            this.Input_StopBitsPps.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_StopBitsPps.Location = new System.Drawing.Point(70, 124);
            this.Input_StopBitsPps.Name = "Input_StopBitsPps";
            this.Input_StopBitsPps.Size = new System.Drawing.Size(100, 20);
            this.Input_StopBitsPps.TabIndex = 3;
            // 
            // Label_AddressPPS
            // 
            this.Label_AddressPPS.AutoSize = true;
            this.Label_AddressPPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_AddressPPS.Location = new System.Drawing.Point(7, 52);
            this.Label_AddressPPS.Name = "Label_AddressPPS";
            this.Label_AddressPPS.Size = new System.Drawing.Size(52, 13);
            this.Label_AddressPPS.TabIndex = 18;
            this.Label_AddressPPS.Text = "Address";
            // 
            // Input_BaudPps
            // 
            this.Input_BaudPps.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_BaudPps.Location = new System.Drawing.Point(70, 98);
            this.Input_BaudPps.Name = "Input_BaudPps";
            this.Input_BaudPps.Size = new System.Drawing.Size(100, 20);
            this.Input_BaudPps.TabIndex = 2;
            // 
            // Input_NamePps
            // 
            this.Input_NamePps.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_NamePps.Location = new System.Drawing.Point(70, 72);
            this.Input_NamePps.Name = "Input_NamePps";
            this.Input_NamePps.Size = new System.Drawing.Size(100, 20);
            this.Input_NamePps.TabIndex = 1;
            // 
            // Group_Settings
            // 
            this.Group_Settings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Group_Settings.Controls.Add(this.Check_EnableDHCP);
            this.Group_Settings.Controls.Add(this.Label_StopDHCP);
            this.Group_Settings.Controls.Add(this.Input_DhcpStop);
            this.Group_Settings.Controls.Add(this.Label_MfgCode);
            this.Group_Settings.Controls.Add(this.label4);
            this.Group_Settings.Controls.Add(this.label5);
            this.Group_Settings.Controls.Add(this.Input_Location);
            this.Group_Settings.Controls.Add(this.Label_StartDHCP);
            this.Group_Settings.Controls.Add(this.Input_DhcpStart);
            this.Group_Settings.Controls.Add(this.Label_Location);
            this.Group_Settings.Controls.Add(this.Input_MfgCode);
            this.Group_Settings.Controls.Add(this.Input_Eqid);
            this.Group_Settings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Group_Settings.Location = new System.Drawing.Point(12, 12);
            this.Group_Settings.Name = "Group_Settings";
            this.Group_Settings.Size = new System.Drawing.Size(378, 201);
            this.Group_Settings.TabIndex = 0;
            this.Group_Settings.TabStop = false;
            // 
            // Check_EnableDHCP
            // 
            this.Check_EnableDHCP.AutoSize = true;
            this.Check_EnableDHCP.Location = new System.Drawing.Point(10, 152);
            this.Check_EnableDHCP.Name = "Check_EnableDHCP";
            this.Check_EnableDHCP.Size = new System.Drawing.Size(103, 17);
            this.Check_EnableDHCP.TabIndex = 3;
            this.Check_EnableDHCP.Text = "Enable DHCP";
            this.Check_EnableDHCP.UseVisualStyleBackColor = true;
            this.Check_EnableDHCP.CheckedChanged += new System.EventHandler(this.Check_EnableDHCP_CheckedChanged);
            // 
            // Label_StopDHCP
            // 
            this.Label_StopDHCP.AutoSize = true;
            this.Label_StopDHCP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_StopDHCP.Location = new System.Drawing.Point(195, 176);
            this.Label_StopDHCP.Name = "Label_StopDHCP";
            this.Label_StopDHCP.Size = new System.Drawing.Size(71, 13);
            this.Label_StopDHCP.TabIndex = 24;
            this.Label_StopDHCP.Text = "DHCP Stop";
            // 
            // Input_DhcpStop
            // 
            this.Input_DhcpStop.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_DhcpStop.Location = new System.Drawing.Point(272, 172);
            this.Input_DhcpStop.Name = "Input_DhcpStop";
            this.Input_DhcpStop.Size = new System.Drawing.Size(100, 20);
            this.Input_DhcpStop.TabIndex = 5;
            // 
            // Label_MfgCode
            // 
            this.Label_MfgCode.AutoSize = true;
            this.Label_MfgCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_MfgCode.Location = new System.Drawing.Point(7, 101);
            this.Label_MfgCode.Name = "Label_MfgCode";
            this.Label_MfgCode.Size = new System.Drawing.Size(66, 13);
            this.Label_MfgCode.TabIndex = 22;
            this.Label_MfgCode.Text = "MFG Code";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(92, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(195, 22);
            this.label4.TabIndex = 19;
            this.label4.Text = "Application Settings";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "EQID";
            // 
            // Input_Location
            // 
            this.Input_Location.BackColor = System.Drawing.Color.White;
            this.Input_Location.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_Location.Location = new System.Drawing.Point(84, 45);
            this.Input_Location.Name = "Input_Location";
            this.Input_Location.Size = new System.Drawing.Size(100, 20);
            this.Input_Location.TabIndex = 0;
            // 
            // Label_StartDHCP
            // 
            this.Label_StartDHCP.AutoSize = true;
            this.Label_StartDHCP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_StartDHCP.Location = new System.Drawing.Point(7, 179);
            this.Label_StartDHCP.Name = "Label_StartDHCP";
            this.Label_StartDHCP.Size = new System.Drawing.Size(72, 13);
            this.Label_StartDHCP.TabIndex = 20;
            this.Label_StartDHCP.Text = "DHCP Start";
            // 
            // Input_DhcpStart
            // 
            this.Input_DhcpStart.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_DhcpStart.Location = new System.Drawing.Point(84, 175);
            this.Input_DhcpStart.Name = "Input_DhcpStart";
            this.Input_DhcpStart.Size = new System.Drawing.Size(100, 20);
            this.Input_DhcpStart.TabIndex = 4;
            // 
            // Label_Location
            // 
            this.Label_Location.AutoSize = true;
            this.Label_Location.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_Location.Location = new System.Drawing.Point(7, 52);
            this.Label_Location.Name = "Label_Location";
            this.Label_Location.Size = new System.Drawing.Size(56, 13);
            this.Label_Location.TabIndex = 18;
            this.Label_Location.Text = "Location";
            // 
            // Input_MfgCode
            // 
            this.Input_MfgCode.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_MfgCode.Location = new System.Drawing.Point(84, 97);
            this.Input_MfgCode.Name = "Input_MfgCode";
            this.Input_MfgCode.Size = new System.Drawing.Size(100, 20);
            this.Input_MfgCode.TabIndex = 2;
            // 
            // Input_Eqid
            // 
            this.Input_Eqid.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Input_Eqid.Location = new System.Drawing.Point(84, 71);
            this.Input_Eqid.Name = "Input_Eqid";
            this.Input_Eqid.Size = new System.Drawing.Size(100, 20);
            this.Input_Eqid.TabIndex = 1;
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 560);
            this.Controls.Add(this.Group_Settings);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.Group_PPS);
            this.Controls.Add(this.Group_DMM);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigurationForm";
            this.Text = "Configuration";
            this.Group_DMM.ResumeLayout(false);
            this.Group_DMM.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.Group_PPS.ResumeLayout(false);
            this.Group_PPS.PerformLayout();
            this.Group_Settings.ResumeLayout(false);
            this.Group_Settings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonSave;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.GroupBox Group_DMM;
        private System.Windows.Forms.TextBox Input_NameDmm;
        private System.Windows.Forms.TextBox Input_BaudDmm;
        private System.Windows.Forms.TextBox Input_StopBitsDmm;
        private System.Windows.Forms.TextBox Input_AddressDmm;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label Label_BaudDMM;
        private System.Windows.Forms.Label Label_NameDMM;
        private System.Windows.Forms.Label Label_StopBitsDMM;
        private System.Windows.Forms.Label Label_AddressDMM;
        private System.Windows.Forms.Label Label_DMM;
        private System.Windows.Forms.GroupBox Group_PPS;
        private System.Windows.Forms.Label Label_BaudPPS;
        private System.Windows.Forms.Label Label_PowerSupply;
        private System.Windows.Forms.Label Label_NamePPS;
        private System.Windows.Forms.TextBox Input_AddressPps;
        private System.Windows.Forms.Label Label_StopBitsPPS;
        private System.Windows.Forms.TextBox Input_StopBitsPps;
        private System.Windows.Forms.Label Label_AddressPPS;
        private System.Windows.Forms.TextBox Input_BaudPps;
        private System.Windows.Forms.TextBox Input_NamePps;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Input_AddressSom;
        private System.Windows.Forms.GroupBox Group_Settings;
        private System.Windows.Forms.Label Label_MfgCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Input_Location;
        private System.Windows.Forms.Label Label_StartDHCP;
        private System.Windows.Forms.TextBox Input_DhcpStart;
        private System.Windows.Forms.Label Label_Location;
        private System.Windows.Forms.TextBox Input_MfgCode;
        private System.Windows.Forms.TextBox Input_Eqid;
        private System.Windows.Forms.CheckBox Check_EnableDHCP;
        private System.Windows.Forms.Label Label_StopDHCP;
        private System.Windows.Forms.TextBox Input_DhcpStop;
    }
}