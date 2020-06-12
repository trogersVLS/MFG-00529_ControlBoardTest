using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ControlBoardTest
{
    class FileManager
    {


        public static bool ProgramInit()
        {
            //Power on self test
            // - Verify that test settings files are in program files folder
            // - Verify that computer settings are in localappdata folder
            // - Verify that connection string works correctly
            string VOCSN_TESTS = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["VOCSN_TESTS"]);
            string V_TESTS = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["V_TESTS"]);
            string CONFIG = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["CONFIG"]);
            string ENVR = ConfigurationManager.AppSettings["Environment"];
            string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["Environment"]].ToString();

            //Does the VOCSN Test file exist?
            if (!File.Exists(VOCSN_TESTS)) return false;
            //Does the V Pro Test File exist?
            if (!File.Exists(V_TESTS)) return false;
            //Does the config file exist?
            if (!File.Exists(CONFIG))
            {
                //Generate configuration form

                ConfigurationForm config_form = new ConfigurationForm();

                config_form.ShowDialog();

                
            }
            //Can we ping the SQL server?
            if (!SQLServer.PingServer(CONNECTIONSTRING)) return false;


            return true;
        }

    }
}
