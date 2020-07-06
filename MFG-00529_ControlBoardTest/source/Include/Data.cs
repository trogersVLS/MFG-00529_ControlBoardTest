using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlBoardTest
{
    class Data
    {
        public ConfigObject settings { get; set; }
    }
    class ConfigObject
    {
        public AppConfig app_settings { get; set; }
        public DmmConfig dmm_settings { get; set; }
        public PpsConfig pps_settings { get; set; }
        public SomConfig som_settings { get; set; }
    }
    class AppConfig
    {
        public string location { get; set; }
        public string eqid { get; set; }
        public string mfg_code { get; set; }
        public bool dhcp_enable { get; set; }
        public string dhcp_start { get; set; }
        public string dhcp_end { get; set; }
    }
    class DmmConfig
    {
        public string address { get; set; }
        public int baudrate { get; set; }
        public int stopbits { get; set; }
        public string name { get; set; }
    }
    class PpsConfig
    {
        public string address { get; set; }
        public int baudrate { get; set; }
        public int stopbits { get; set; }
        public string name { get; set; }
    }

    class SomConfig
    {
        public string address { get; set; }
        public int baudrate { get; set; }
    }






}
