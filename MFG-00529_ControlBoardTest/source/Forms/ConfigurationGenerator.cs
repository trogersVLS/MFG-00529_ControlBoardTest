using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System.Xml;

namespace ControlBoardTest
{   
    


    public partial class ConfigurationForm : Form
    {   
        string CONFIG_PATH;
        public ConfigurationForm()
        {
            InitializeComponent();
            string config_path = Environment.ExpandEnvironmentVariables(System.Configuration.ConfigurationManager.AppSettings["CONFIG"]);
            this.FillForm(config_path);

            this.CONFIG_PATH = config_path;
        }

        private void FillForm(string config_path)
        {
            //Get the config file, parse and fill in the fields according to the new file.
            if (File.Exists(config_path))
            {
                string json_config = File.ReadAllText(config_path);

                Data config = JsonSerializer.Deserialize<Data>(json_config);

                //Fill in App Settings Group
                this.Input_Location.Text = config.settings.app_settings.location;
                this.Input_Eqid.Text = config.settings.app_settings.eqid;
                this.Input_MfgCode.Text = config.settings.app_settings.mfg_code;
                this.Check_EnableDHCP.Checked = config.settings.app_settings.dhcp_enable;
                if (!this.Check_EnableDHCP.Checked) //Disable the IP Address fields if DHCP is not enabled
                {
                    this.Input_DhcpStart.Enabled = false;
                    this.Input_DhcpStop.Enabled = false;
                }
                this.Input_DhcpStart.Text = config.settings.app_settings.dhcp_start;
                this.Input_DhcpStop.Text = config.settings.app_settings.dhcp_end;

                //Fill in Dmm Settings Group
                this.Input_AddressDmm.Text = config.settings.dmm_settings.address;
                this.Input_NameDmm.Text = config.settings.dmm_settings.name;
                this.Input_BaudDmm.Text = config.settings.dmm_settings.baudrate.ToString();
                this.Input_StopBitsDmm.Text = config.settings.dmm_settings.stopbits.ToString();

                //Fill in Pps Settings Group
                this.Input_AddressPps.Text = config.settings.pps_settings.address;
                this.Input_NamePps.Text = config.settings.pps_settings.name;
                this.Input_BaudPps.Text = config.settings.pps_settings.baudrate.ToString();
                this.Input_StopBitsPps.Text = config.settings.pps_settings.stopbits.ToString();

                //Fill in Som Settings Group
                this.Input_AddressSom.Text = config.settings.som_settings.address;

            }
            else
            {
                this.Check_EnableDHCP.Checked = false;
                this.Input_DhcpStart.Enabled = false;
                this.Input_DhcpStop.Enabled = false;
            }

        }

        private void Check_EnableDHCP_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Check_EnableDHCP.Checked) //Disable the IP Address fields if DHCP is not enabled
            {
                this.Input_DhcpStart.Enabled = true;
                this.Input_DhcpStop.Enabled = true;
            }
            else
            {
                this.Input_DhcpStart.Enabled = false;
                this.Input_DhcpStop.Enabled = false;
            }
        }
        private void ResetFieldColors()
        {
            this.Input_Location.BackColor = Color.White;
            this.Input_Eqid.BackColor = Color.White;
            this.Input_MfgCode.BackColor = Color.White;
            this.Input_DhcpStart.BackColor = Color.White;
            this.Input_DhcpStop.BackColor = Color.White;

            this.Input_AddressDmm.BackColor = Color.White;
            this.Input_NameDmm.BackColor = Color.White;
            this.Input_BaudDmm.BackColor = Color.White;
            this.Input_StopBitsDmm.BackColor = Color.White;

            this.Input_AddressPps.BackColor = Color.White;
            this.Input_NamePps.BackColor = Color.White;
            this.Input_BaudPps.BackColor = Color.White;
            this.Input_StopBitsPps.BackColor = Color.White;

            this.Input_AddressSom.BackColor = Color.White;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            Data config;
            //Validate data in fields
            ResetFieldColors();
            if (ValidateFields(out config))
            {
                //Save data
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize<Data>(config, options);

                Directory.CreateDirectory(this.CONFIG_PATH.Substring(0, this.CONFIG_PATH.LastIndexOf("\\")));
                File.WriteAllText(this.CONFIG_PATH, json);
                if (File.Exists(this.CONFIG_PATH))
                {
                    this.Close();
                }
            }
        }

        private bool ValidateFields(out Data data)
        {
            data = new Data();
            data.settings = new ConfigObject();

            bool validApp, validDmm, validPps, validSom;
            data.settings.app_settings = ValidateAppSettings(out validApp);
            data.settings.dmm_settings = ValidateDmmSettings(out validDmm);
            data.settings.pps_settings = ValidatePpsSettings(out validPps);
            data.settings.som_settings = ValidateSomSettings(out validSom);

            return (validApp && validDmm && validPps && validSom);
        }
        private AppConfig ValidateAppSettings(out bool valid)
        {
            valid = true;
            AppConfig config = new AppConfig();
            if (this.Input_Location.Text != null)
            {
                config.location = this.Input_Location.Text;
            }
            else
            {
                valid = false;
                this.Input_Location.BackColor = Color.Red;
            }

            if (this.Input_Eqid != null)
            {
                config.eqid = this.Input_Eqid.Text;
            }
            else
            {
                valid = false;
                this.Input_Eqid.BackColor = Color.Red;
            }

            if (this.Input_MfgCode != null)
            {
                config.mfg_code = this.Input_MfgCode.Text;
            }
            else
            {
                valid = false;
                this.Input_MfgCode.BackColor = Color.Red;
            }
            if (this.Check_EnableDHCP.Checked) {
                if (IPAddress.TryParse(this.Input_DhcpStart.Text, out IPAddress ip))
                {
                    config.dhcp_start = this.Input_DhcpStart.Text;
                }
                else
                {
                    valid = false;
                    this.Input_DhcpStart.BackColor = Color.Red;
                }
                if (IPAddress.TryParse(this.Input_DhcpStop.Text, out ip))
                {
                    config.dhcp_end = this.Input_DhcpStop.Text;
                }
                else
                {
                    valid = false;
                    this.Input_DhcpStop.BackColor = Color.Red;
                }
            }
            else
            {
                if (IPAddress.TryParse(this.Input_DhcpStart.Text, out IPAddress ip))
                {
                    config.dhcp_start = this.Input_DhcpStart.Text;
                }
                else config.dhcp_start = null;

                if (IPAddress.TryParse(this.Input_DhcpStop.Text, out ip))
                {
                    config.dhcp_end = this.Input_DhcpStop.Text;
                }
                else config.dhcp_end = null;
            }
            return config;
        }
        private DmmConfig ValidateDmmSettings(out bool valid)
        {
            DmmConfig config = new DmmConfig();
            valid = true;

            if (this.Input_AddressDmm.Text.StartsWith("COM"))
            {
                string comNum = this.Input_AddressDmm.Text.Substring(3);
                if(int.TryParse(comNum, out int i))
                {
                    config.address = this.Input_AddressDmm.Text;
                }
                else
                {
                    this.Input_AddressDmm.BackColor = Color.Red;
                    valid = false;
                }
               
            }
            else
            {
                this.Input_AddressDmm.BackColor = Color.Red;
                valid = false;
            }

            int baud;
            if (int.TryParse(this.Input_BaudDmm.Text, out baud))
            {
                config.baudrate = baud;
            }
            else
            {
                valid = false;
                this.Input_BaudDmm.BackColor = Color.Red;
            }

            int stopbits;
            if (int.TryParse(this.Input_StopBitsDmm.Text, out stopbits))
            {
                config.stopbits = stopbits;
            }
            else
            {
                valid = false;
                this.Input_StopBitsDmm.BackColor = Color.Red;
            }

            if (this.Input_NameDmm.Text != null)
            {
                config.name = this.Input_NameDmm.Text;
            }
            else
            {
                valid = false;
                this.Input_NameDmm.BackColor = Color.Red;
            }

            return config;
        }
        private PpsConfig ValidatePpsSettings(out bool valid)
        {
            PpsConfig config = new PpsConfig();
            valid = true;

            if (this.Input_AddressPps.Text.StartsWith("COM"))
            {
                string comNum = this.Input_AddressPps.Text.Substring(3);
                if (int.TryParse(comNum, out int i))
                {
                    config.address = this.Input_AddressPps.Text;
                }
                else
                {
                    this.Input_AddressPps.BackColor = Color.Red;
                    valid = false;
                }

            }
            else
            {
                this.Input_AddressPps.BackColor = Color.Red;
                valid = false;
            }

            int baud;
            if (int.TryParse(this.Input_BaudPps.Text, out baud))
            {
                config.baudrate = baud;
            }
            else
            {
                valid = false;
                this.Input_BaudPps.BackColor = Color.Red;
            }

            int stopbits;
            if (int.TryParse(this.Input_StopBitsPps.Text, out stopbits))
            {
                config.stopbits = stopbits;
            }
            else
            {
                valid = false;
                this.Input_StopBitsPps.BackColor = Color.Red;
            }
            if (this.Input_NamePps.Text != null)
            {
                config.name = this.Input_NamePps.Text;
            }

            return config;

        }
        private SomConfig ValidateSomSettings(out bool valid)
        {
            SomConfig config = new SomConfig();
            valid = true;
            config.baudrate = 115200; // Magic number SOM always communicates at baud of 115200

            if (this.Input_AddressSom.Text.StartsWith("COM"))
            {
                string comNum = this.Input_AddressSom.Text.Substring(3);
                if (int.TryParse(comNum, out int i))
                {
                    config.address = this.Input_AddressSom.Text;
                }
                else
                {
                    this.Input_AddressSom.BackColor = Color.Red;
                    valid = false;
                }

            }
            else
            {
                this.Input_AddressSom.BackColor = Color.Red;
                valid = false;
            }

            return config;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
