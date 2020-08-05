using System;
using System.IO;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GPIO;
using VLS;
using System.Xml;
using System.Configuration;
//using System.Data.SQLite;
using System.Windows.Forms;
using System.Management;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

namespace ControlBoardTest
{

    /******************************************************************************************************************************************
     *                                               Test Data Structure
     ******************************************************************************************************************************************/
    public struct TestData
    {
        public int step;                                // Step number in list of tests
        public string name;                             // Human readable test name
        public string method_name;                      // Method name in the Tests.cs file
        public MethodInfo testinfo;                     // Pointer to the method used to run the test
        public Dictionary<string, string> parameters;                    // Dictionary of parameters used in the test

        public string Result 
        {
            get { return parameters["result"]; }
            set { parameters["result"] = value; }
        }

        public TestData(int step, string name, string method_name, MethodInfo function, Dictionary<string,string> parameters)
        {
            this.step = step;
            this.name = name;
            this.method_name = method_name;
            this.testinfo = function;
            this.parameters = parameters;
            this.parameters.Add("measured", "Not tested");
            this.parameters.Add("result", "Not tested");
           
        }
        public void SetResult(bool result)
        {
            if (result)
            {
                this.parameters["result"] = "PASS";
            }
            else
            {
                this.parameters["result"] = "FAIL";
            }
        }


    }
    /******************************************************************************************************************************************
     *                                               Functional Test Class
     ******************************************************************************************************************************************/
    public partial class FunctionalTest
    {
        //Test specific data --> To be stored in results file and in database
        private string serial="27B-6";             //Test serial number
        public string result;
        public bool test;

        Data config;


        public MccDaq_GPIO GPIO;
        public Test_Equip DMM;
        public Test_Equip PPS;
        public VOCSN_Serial SOM;
        public VLS_Tlm Vent;


        public List<TestData> VOCSN_TESTS { get; set; }
        public List<TestData> V_TESTS { get; set; }

        public List<TestData> DUMMY_TEST;

        bool DEBUG;
        

        

        //SQLite Variables
        //SQLiteConnection db_con;
        private long test_id;


        private bool powered = true;



        /************************************************************************************************************
         * Functional Test Class Constructor
         * 
         * Parameters: - ConcurrentQueue<string> _queue--> The message passing data structure used between the calling object (presumably GUI)
         *             - XmlNode tests --> An xml data type containing the tests required and the specs to pass
         *             - String serial--> The serial number of the board in which this test has been initialized
         * 
         * **********************************************************************************************************/
        public FunctionalTest(bool getFunctions = false)
        {
            if (!getFunctions)
            {
                // Get settings for Test Equipments
                string json_config = File.ReadAllText(Environment.ExpandEnvironmentVariables(System.Configuration.ConfigurationManager.AppSettings["CONFIG"]));
                this.config = System.Text.Json.JsonSerializer.Deserialize<Data>(json_config);


                //Generate Test Lists -----> There might need to be a better way to determine how many configurations that there are, but for now they are hardcoded as VOCSN_PRO and V_PRO
                this.VOCSN_TESTS = GetTestList(Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["VOCSN_Tests"]));
                this.V_TESTS = GetTestList(Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["V_Tests"]));

                //Generate Test Equipment Objects and test them
                this.GPIO = new MccDaq_GPIO();
                this.DMM = new Test_Equip(config.settings.dmm_settings.name,
                                          config.settings.dmm_settings.baudrate,
                                          config.settings.dmm_settings.stopbits,
                                          config.settings.dmm_settings.address);

                this.PPS = new Test_Equip(config.settings.pps_settings.name,
                                          config.settings.pps_settings.baudrate,
                                          config.settings.pps_settings.stopbits,
                                          config.settings.pps_settings.address);
                this.SOM = new VOCSN_Serial(config.settings.som_settings.address);
                this.ConnectDevices();

            }

        }

        public void SetDutSettings(string filepath)
        {

            if (this.Vent != null)
            {
                if (this.Vent.Connected)
                {
                    this.Vent.CMD_Write(string.Format("set uim params {0}", filepath));
                }
            }
            return;
        }
        public void GetSettings()
        {
            // Get settings for Test Equipments
            string json_config = File.ReadAllText(Environment.ExpandEnvironmentVariables(System.Configuration.ConfigurationManager.AppSettings["CONFIG"]));
            this.config = System.Text.Json.JsonSerializer.Deserialize<Data>(json_config);


            //Generate Test Lists -----> There might need to be a better way to determine how many configurations that there are, but for now they are hardcoded as VOCSN_PRO and V_PRO
            this.VOCSN_TESTS = GetTestList(Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["VOCSN_Tests"]));
            this.V_TESTS = GetTestList(Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["V_Tests"]));
        }
        /************************************************************************************************************************************************/
        public void ConnectDevices()
        {
            // Connect to GPIO and catch any errors
            try { this.GPIO.ConnectDevice(); }
            catch { System.Windows.Forms.MessageBox.Show("Error! GPIO connect Universal Library ", "Error"); }
            
            //Connect to DMM and catch any errors
            try { this.DMM.Connect(); }
            catch { System.Windows.Forms.MessageBox.Show("Error! RS232 DMM control init", "Error"); }
            
            // Connect to PPS and catch any errors
            try { this.PPS.Connect(); }
            catch { System.Windows.Forms.MessageBox.Show("Error! RS232 PPS control init", "Error"); }
            
            // Connect to SOM and catch any errors.
            try { this.SOM.Connect(); }
            catch { System.Windows.Forms.MessageBox.Show("Error! UUT SOM debug output RS232 connect", "Error"); }

        }
        /************************************************************************************************************************************************/
        public bool ConnectToTelnet(string _ip_address, IProgress<string> message, IProgress<string> logs)
        {   
            if (_ip_address != null)
            {   
                this.Vent = new VLS_Tlm(_ip_address);
                message.Report("Connecting to telemetry ...");
            }

            //There's a long delay between the device booting to the VCM app and the device acquiring an IP address.
            var success = this.Vent.Connect(_ip_address, "mfgmode", true);            
            if (success)
            {
                message.Report("Connected!");
                //Once connected, set to MFG mode so that we can begin testing the various functions
            }
            else
            {
                DisconnectTelnet();
                message.Report("Connection failed!");                
            }

            return success;
        }
        /************************************************************************************************************************************************/

        public bool DisconnectTelnet()
        {
            try
            {
                if (this.Vent != null)
                {
                    this.Vent.Disconnect();
                    this.Vent.Connected = false;

                }
            }
            catch
            {

            }
            return !this.Vent.Connected;
        }
        private List<TestData> GetTestList(string test_filepath)
        {
            List<TestData> tests = new List<TestData>();
            XmlDocument configuration = new XmlDocument();
            configuration.Load(test_filepath);

            foreach (XmlNode xml in configuration.DocumentElement.ChildNodes)
            {
                
                if (xml.Name == "tests")
                {
                    try
                    {
                        XmlNode TestNames = xml;
                        tests = Match_MethodsToTests(TestNames);

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Program has encountered an error in the configuration file\n\rERROR: " + e.Message, "Configuration");
                        throw e;
                    }

                }

            }
            return tests;
        }
        private List<TestData> Match_MethodsToTests(XmlNode TestNodes)
        {
            //Get list of methods in this class
            FunctionalTest test = new FunctionalTest(true);
            MethodInfo[] methods = test.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            int step_num = 0;
            //Iterate through each xml node to find the matching function and create a list.

            List<TestData> Tests = new List<TestData>();
            foreach (XmlNode x in TestNodes.ChildNodes)
            {
                if (x.NodeType != XmlNodeType.Comment)
                {
                    foreach (MethodInfo method in methods)
                    {
                        if (method.Name == x.Attributes["method_name"].Value)
                        {
                            int step = step_num;
                            string name = x.Attributes["name"].Value;
                            string method_name = x.Attributes["method_name"].Value;
                            Dictionary<string, string> parameters = new Dictionary<string, string>();
                            for (int i = 0; i < x.Attributes.Count; i++)
                            {
                                parameters.Add(x.Attributes[i].Name, x.Attributes[i].Value);
                            }

                            TestData test_step = new TestData(step, name, method_name, method, parameters);

                            Tests.Add(test_step);
                            step_num++;
                            break;
                        }
                    }
                }

            }
            return Tests;
        }

        /******************************************************************************************************************************************
         *                  MESSAGE PASSING UTILITIES
         ******************************************************************************************************************************************/



        private string PromptUser(string question, string TestName)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(question, TestName, "", 0, 0);

            return input;
        }


        private bool PromptUser_YesNo(string question, string TestName)
        {
            bool result;
            var output = System.Windows.Forms.MessageBox.Show(question, TestName, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question );

            if(output == System.Windows.Forms.DialogResult.Yes)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        private void NotifyUser(string message)
        {
            var output = System.Windows.Forms.MessageBox.Show(message, "Notification", System.Windows.Forms.MessageBoxButtons.OK);


            return;
        }

        /******************************************************************************************************************************************
        *                                               TEST RUNNING FUNCTIONS
        *                                               
        *
        * Task<TestData> RunTest 
        *
        *
        ******************************************************************************************************************************************/
        public async Task<TestData> RunTest(TestData test, IProgress<string> message, IProgress<string> log)
        {
            bool success = false;

            var param = new object[] { message, log, test };


            try
            {

                message.Report("\nStarting test: " + test.name);
                //log.Report("Starting test: " + test.method_name);
                //await Task.Factory.StartNew(() => (success = this.RunTest(this.TEST_ID, this.SERIAL_NUM, TestToRun, message, log)));
                
                //Invoke the test function --> Each test will fill the parameters table to measured
                Task<bool> testRunning = Task.Factory.StartNew(() => (success = (bool)test.testinfo.Invoke(this, param)), TaskCreationOptions.LongRunning);
                await testRunning;
               
               


            }
            catch (Exception e)
            {
                var error = e.InnerException.Message;

                test.parameters["measured"] = "ERROR";
                string errormessage = "Exception caught: " + error + "\n\rThe following test failed: " + test.name;

                log.Report(error);


                MessageBox.Show(errormessage, "Exception caught", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                this.GPIO.ClearAllButPower();
            }
            
            //message.Report("Clear GPIO ...");
            test.SetResult(success);

            if (success)
            {
                //message.Report(test.name + ": PASS");

            }
            else
            {
                //message.Report(test.name + ": FAIL");
                success = false;
            }
            return test;
        }

        public void EndTest()
        {




           
            this.GPIO.ClearAll();
            this.PPS.Set_Output(false);
            this.DisconnectTelnet();
        }

        /******************************************************************************************************************************************
         *                                               DATA LOGGING FUNCTIONS
         ******************************************************************************************************************************************/

        /******************************************************************************************************************************************
         * SQLite tables
         *  - Control_Board_Test
         *      - An entry in the control board Test Table is posted every time an instance of the functional test class is created. This holds
         *      a unique test_id value that is used to group multiple tests that are run at the same time by one press of the "Run" button in 
         *      the GUI. A control board test table looks like. The unique id will be shared by a secondary table
         *      _______________________________________________________________________________
         *      |                             Control Board Test                               |
         *      --------------------------------------------------------------------------------
         *      | Test_ID   | EQID     | USER_ID  | LOCATION | TIMESTAMP | SERIAL   | RESULT   |
         *      --------------------------------------------------------------------------------
         *      | <string>  | <string> | <string> | <string> | <string>  | <string> | <string> |
         *      |    ...    |    ...   |    ...   |    ...   |    ...    |    ...   |    ...   |
         *      |    ...    |    ...   |    ...   |    ...   |    ...    |    ...   |    ...   |
         *      
         *      
         * - Test_Table
         *      - An entry in the test table indicates an instance that a test was run on the board and the result of that specific test. This
         *      will store the test name, serial number, parameters that the test was run against and the measured result of the test and the 
         *      ultimate result of the test. The test id is unique to a set of tests, but no two entries in the table should be the same.
         *      _________________________________________________________________________________
         *      |                              Tests                                             |
         *      ----------------------------------------------------------------------------------
         *      | Test_ID   | SERIAL   | TEST_NAME| UpperBound | LowerBound | Measured | RESULT  |
         *      ----------------------------------------------------------------------------------
         *      | <string>  | <string> | <string> | <real>     | <real>     | <real>   | <string>|
         *      |    ...    |    ...   |    ...   |    ...     |    ...     |    ...   |    ...  |
         *      |    ...    |    ...   |    ...   |    ...     |    ...     |    ...   |    ...  |
         *         
         */

        /************************************************************************************************************
        * LogTestInstance() - Logs a test instance to the Test_Instance table in the local database. This 
        * 
        * Parameters: - None
        * Returns:    - bool success - Returns true if the database log was successful
        *                            - Returns false if the database log was unsuccessful
        *             
        * **********************************************************************************************************/
        public async Task<(int, int)> LogNewTest(string username, string serial, bool remote)
        {
            int remote_test_id = -1;
            int local_test_id = -1;
            try
            {
                Dictionary<string, string> row = new Dictionary<string, string>();
                row.Add("eqid", this.config.settings.app_settings.eqid);
                row.Add("user-id", username);
                row.Add("location", this.config.settings.app_settings.location);
                row.Add("timestamp", DateTime.UtcNow.ToString());
                row.Add("serial", serial);
                row.Add("result", "In progress");
                
                
                Task<int> LoggingNewTest;
                if (remote)
                {
                    // Order of operations is important, any exception made trying to log a new remote test (because of IP and/or connectivity problems will result in not updating the local test id.
                    Task<int> LoggingLocalNewTest = SQLServer.Local_InsertOneRow(ConfigurationManager.ConnectionStrings["Local"].ToString(),
                                                                 ConfigurationManager.AppSettings["INSTANCE_TABLENAME"].Replace("[dbo].", ""),
                                                                 row);
                    local_test_id = await LoggingLocalNewTest;
                    LoggingNewTest = SQLServer.InsertOneRow(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["Environment"]].ToString(),
                                                                  ConfigurationManager.AppSettings["INSTANCE_TABLENAME"],
                                                                  row);
                    remote_test_id = await LoggingNewTest;
                }
                else
                {
                    LoggingNewTest = SQLServer.Local_InsertOneRow(ConfigurationManager.ConnectionStrings["Local"].ToString(),
                                                                  ConfigurationManager.AppSettings["INSTANCE_TABLENAME"].Replace("[dbo].", ""),
                                                                  row);
                    local_test_id = await LoggingNewTest;
                }
                
                
                
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error logging new test" + e.Message);
                
            }
            return (remote_test_id, local_test_id);
        }
        public async Task<int> LogTestResult(string result, bool remote, long remote_test_id, long local_test_id)
        {
            try
            {
                Dictionary<string, string> updatedData = new Dictionary<string, string>();
                updatedData.Add("result", result);

                Task<int> updating;
                if (remote)
                {   
                    Task<int> LocalUpdating = SQLServer.Local_UpdateResult(ConfigurationManager.ConnectionStrings["Local"].ToString(),
                                                                ConfigurationManager.AppSettings["INSTANCE_TABLENAME"].Replace("[dbo].", ""),
                                                                local_test_id.ToString(),
                                                                updatedData);
                    await LocalUpdating;
                    updating = SQLServer.UpdateResult(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["Environment"]].ToString(),
                                                                ConfigurationManager.AppSettings["INSTANCE_TABLENAME"],
                                                                remote_test_id.ToString(),
                                                                updatedData);
                    
                }
                else
                {
                    updating = SQLServer.Local_UpdateResult(ConfigurationManager.ConnectionStrings["Local"].ToString(),
                                                                ConfigurationManager.AppSettings["INSTANCE_TABLENAME"].Replace("[dbo].",""),
                                                                local_test_id.ToString(),
                                                                updatedData);
                }
                int rowsAffected = await updating;
                return rowsAffected;
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error logging the test result" + e.Message);
                return -1;
            }
        }
        /************************************************************************************************************
        * LogTestData() - Logs the results of a test to the Tests table in the database
        * 
        * Parameters: - Hashtable table - Table contains keys to the data that needs to be logged.
        * Returns:    - bool success - Returns true if the database log was successful
        *                            - Returns false if the database log was unsuccessful
        *             
        * **********************************************************************************************************/
        public async Task<int> LogTestData(TestData test, string serial, bool remote, long remote_test_id, long local_test_id)
        {
            int rowsUpdated = -1;
            try
            {

                Dictionary<string, string> row = new Dictionary<string, string>();
                row.Add("test-id", "");
                row.Add("serial", serial);
                row.Add("test-name", test.name);

                test.parameters.TryGetValue("upper-bound", out var upper);
                if(upper == null) { upper = "N/A"; }
                row.Add("upper-bound", upper);

                test.parameters.TryGetValue("lower-bound", out var lower);
                if(lower == null) { lower = "N/A"; }
                row.Add("lower-bound", lower);

                test.parameters.TryGetValue("measured", out var measured);
                if (measured == null) { lower = "ERROR"; }
                row.Add("measured", measured);

                test.parameters.TryGetValue("result", out var result);
                if (result == null) { result = "ERROR"; }
                row.Add("result", result);

                Task<int> LoggingTestResults;
                if (remote) // Log to remote server
                {
                    Task<int> LoggingLocalTestResults = SQLServer.Local_InsertOneRow(ConfigurationManager.ConnectionStrings["Local"].ToString(),
                                                                          ConfigurationManager.AppSettings["TESTS_TABLENAME"].Replace("[dbo].", ""),
                                                                          row);
                    await LoggingLocalTestResults;
                    row["test-id"] = remote_test_id.ToString();
                    LoggingTestResults = SQLServer.InsertOneRow(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["Environment"]].ToString(),
                                                                          ConfigurationManager.AppSettings["TESTS_TABLENAME"],
                                                                          row);
                    row["test-id"] = local_test_id.ToString();
                    
                }
                else // Couldn't connect to remote server, log to local database
                {
                    row["test-id"] = remote_test_id.ToString();
                    LoggingTestResults = SQLServer.Local_InsertOneRow(ConfigurationManager.ConnectionStrings["Local"].ToString(),
                                                                          ConfigurationManager.AppSettings["TESTS_TABLENAME"].Replace("[dbo].",""),
                                                                          row);
                    
                }
                rowsUpdated = await LoggingTestResults;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error Logging test data" + e.Message);
                return -1;
            }

            return rowsUpdated;
        }

        private Hashtable CreateData(TestData test, bool result)
        {
            Hashtable data = new Hashtable
            {
                { "test_id", this.test_id },
                { "serial_number", this.serial },
                { "test_name", test.name }
            };

            if (test.parameters["qual"] != "true")
            {
                data.Add("upper_bound", test.parameters["upper"]);
                data.Add("lower_bound", test.parameters["lower"]);

                if (!test.parameters.ContainsKey("measured"))
                {
                    data.Add("measured", "err");
                }
                else
                {
                    data.Add("measured", test.parameters["measured"]);
                }
            }
            else
            {
                data.Add("upper_bound", "--");
                data.Add("lower_bound", "--");
                data.Add("measured", "--");
            }
            if (result) data.Add("result", "PASS");
            else data.Add("result", "FAIL");


            return data;
        }

    }
}
