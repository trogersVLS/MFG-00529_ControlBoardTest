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
            string LOCALDB = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["LOCALDB"]);
            string ENVR = ConfigurationManager.AppSettings["Environment"];
            string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["Environment"]].ToString();

            //Does the VOCSN Test file exist?
            if (!File.Exists(VOCSN_TESTS)) return false;
            //Does the V Pro Test File exist?
            if (!File.Exists(V_TESTS)) return false;
            if (!File.Exists(LOCALDB))
            {
                //Copy local db template from Program Files Directory.
                Directory.CreateDirectory(LOCALDB.Substring(0, LOCALDB.LastIndexOf("\\")));

                string command = @"PRAGMA foreign_keys=OFF;
                                    BEGIN TRANSACTION;
                                    CREATE TABLE [test-results](
                                    [test-id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                    eqid CHAR(50),
                                    [user-id] CHAR(50),
                                    location CHAR(50),
                                    timestamp CHAR(50),
                                    serial CHAR(50),
                                    result CHAR(50)
                                    );
                                    CREATE TABLE [test-data](
                                    [test-id] INTEGER NOT NULL,
                                    serial CHAR(50),
                                    [test-name] CHAR(50),
                                    [upper-bound] CHAR(50),
                                    [lower-bound] CHAR(50),
                                    measured CHAR(50),
                                    result CHAR(50)
                                    );
                                    DELETE FROM sqlite_sequence;
                                    INSERT INTO sqlite_sequence VALUES('test-results',7);
                                    COMMIT;";

                                    
                SQLServer.Local_Generate(LOCALDB, command);
                //File.Copy(LOCALDB.Substring(LOCALDB.LastIndexOf("\\") + 1), LOCALDB);
            }
            //Does the config file exist?
            if (!File.Exists(CONFIG))
            {
                //Generate configuration form

                ConfigurationForm config_form = new ConfigurationForm();

                config_form.ShowDialog();

                
            }
            //Can we ping the SQL server?
            if (!SQLServer.PingServer(CONNECTIONSTRING))
            {
                //System.Windows.Forms.MessageBox.Show("Unable to reach remote database. Using local database");
                return false;
            }

            return true;
        }

    }
}
