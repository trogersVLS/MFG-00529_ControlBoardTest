using System;
using System.IO;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GPIO;
using VLS;
using System.Data.SQLite;
using System.Windows.Forms;



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
        public Dictionary<string,string> parameters;                    // Dictionary of parameters used in the test

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
        private string location="earth";
        private int eqid=42;
        private string user_id="everett";
        private string timestamp=null;
        public string result;
        public bool test;
       // private Hashtable Parameters;


        private MccDaq_GPIO GPIO;
        private Test_Equip DMM;
        private Test_Equip PPS;
        private VOCSN_Serial SOM;
        private VLS_Tlm Vent;
        

        public List<TestData> Tests = new List<TestData>();

        bool DEBUG;
        

        

        //SQLite Variables
        SQLiteConnection db_con;
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
        public FunctionalTest()
        {
            //This constructor is used for collecting methods from this data type
        }
         
        public FunctionalTest(MccDaq_GPIO GPIO, Test_Equip DMM, Test_Equip PPS, VOCSN_Serial SOM, VLS_Tlm VENT, bool debug = false)
        {
            //This constructor is used for production

            this.DEBUG = debug;
/********************************************************************/

            try
            {
                this.GPIO = GPIO;
            }

            catch
            {
                //Something didn't work. Update user
                System.Windows.Forms.MessageBox.Show("Error! Invalid GPIO object", "Error");

            }

 /********************************************************************/


            try
            {
                this.GPIO.ConnectDevice();
               
            }
            catch
            {

                System.Windows.Forms.MessageBox.Show("Error! GPIO connect Universal Library ", "Error");

            }

/********************************************************************/

            try
            {
               this.DMM = DMM;
                
            }
            catch
            {

                System.Windows.Forms.MessageBox.Show("Error! Test instrument DMM object ", "Error");
           }

            /********************************************************************/

            try
            {
                             
                this.DMM.Connect();
               
            }
            catch
            {

                System.Windows.Forms.MessageBox.Show("Error! RS232 DMM control init", "Error");

            }

            /********************************************************************/

            try
            {
               
                this.PPS = PPS;
              
            }
            catch
            {

                System.Windows.Forms.MessageBox.Show("Error! Test Instrument PPS object", "Error");

            }


            /********************************************************************/

            try
            {
                this.PPS.Connect();
                
            }
            catch
            {

                System.Windows.Forms.MessageBox.Show("Error! RS232 PPS control init", "Error");

            }

            /********************************************************************/

            try
            {
                this.SOM = SOM;

            }
            catch
            {

                System.Windows.Forms.MessageBox.Show("Error! UUT SOM debug output object", "Error");

            }

            /********************************************************************/

            try
            {
               this.SOM.Connect();

            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Error! UUT SOM debug output RS232 connect", "Error");

            }

            /********************************************************************/


        }




        /************************************************************************************************************************************************/
        public bool ConnectToTelnet(string _ip_address)
        {
            if (_ip_address != null)
            {
                this.Vent = new VLS_Tlm(_ip_address);
            }

            //There's a long delay between the device booting to the VCM app and the device acquiring an IP address.
            var success = this.Vent.Connect(_ip_address, "mfgmode", true);
            int count = 15;
            while (!this.Vent.Connected)
            {
                count--;
                Thread.Sleep(1000);
            }
            if (success)
            {
                //Once connected, set to MFG mode so that we can begin testing the various functions
            }
            else
            {
                this.Vent = null;
            }

            return success;
        }



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
                message.Report("Connection failed!");
                this.Vent = null;
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
        ******************************************************************************************************************************************/

        /************************************************************************************************************
         * RunTest() - Runs the list of tests determined by the functional test
         * 
         * Parameters: - progress --> Progress interface variable. Indicates the percentage of the test that is complete
         *             - message  --> Progress interface variable. Used to update the text in the output box.  
         *             - TestList --> List of TestStep. Used to tell RunTest which tests need to be run.
         * **********************************************************************************************************/
        public void RunTest(IProgress<int> progress, IProgress<string> message, IProgress<string> logs, List<TestData> TestList)
        {
            int i = 0;
            bool success = true;

            try
            {
              

                foreach (TestData test in TestList)
                {
                    var param = new object[] { message, test };
                    message.Report("Starting " + test.name);   //Indicate which test is being run
                    progress.Report((i * 100) / (TestList.Count)); // Indicate the progress made


                    //All tests should return a bool
                    var result = (bool)test.testinfo.Invoke(this, param);
                    if (result)
                    {
                        message.Report("\nTest: " + test.name + " - PASS\n");

                    }
                    else
                    {
                        message.Report("\nTest: " + test.name + " - FAIL\n");
                        if (success) success = false;
                    }

                    ////Build hashtable for logging data.
                    //if (this.Log_Data)
                    //{
                    //    Hashtable data = this.CreateData(test, (bool)result);
                    //    this.LogTestData(data);
                    //}

                    i++;
                }
                
                
            }
            catch (Exception e)
            {

                message.Report( "Something went wrong");
                message.Report(e.Message.ToString() + "\n\n" + e.StackTrace.ToString());
               
                Thread.CurrentThread.Abort();
            }

            if (success) this.result = "PASS";
            else this.result = "FAIL";

            
            Thread.Sleep(2000);
            return;
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
        * DatabaseExist() - Looks for the expected Database file. If the database does not exist, this function will
        * create it.
        * 
        * Parameters: - None
        * Returns:    - bool success - Returns true if the database exists or if creation was successful
        *                              Returns false if the database creation was unsuccessful
        *             
        * **********************************************************************************************************/
        private bool DatabaseExist()
        {
            
            SQLiteCommand cmd;
            //SQLiteDataReader dr;
            bool success = false;
            

            try
            {


                if (!File.Exists(".\\Database\\MFG_527.db"))
                {
                    SQLiteConnection.CreateFile(".\\Database\\MFG_527.db");

                    string test_instance_table = @"CREATE TABLE Test_Instance(
                                      TEST_ID INTEGER PRIMARY KEY AUTOINCREMENT ,
                                      EQID INTEGER ,
                                      USER TEXT ,
                                      LOCATION TEXT ,
                                      TIMESTAMP TEXT,
                                      SERIAL_NUMBER TEXT ,
                                      RESULT TEXT
                                      );";

                    string tests_table = @"CREATE TABLE Tests(
                               TEST_ID INTEGER,
                               SERIAL_NUMBER TEXT ,
                               TEST_NAME TEXT,
                               UPPER_BOUND TEXT,
                               LOWER_BOUND TEX,
                               MEASURED TEXT,
                               RESULT TEXT
                               );";

                    this.db_con = new SQLiteConnection("Data Source=.\\Database\\MFG_527.db;Version=3");
                    this.db_con.Open();

                    cmd = new SQLiteCommand(test_instance_table, this.db_con);
                    //cmd.CommandText = test_instance_table;
                    int d = cmd.ExecuteNonQuery();

                    cmd.CommandText = tests_table;
                    d = cmd.ExecuteNonQuery();

                    this.db_con.Close();
                    if (File.Exists(".\\Database\\MFG_527.db"))
                    {
                        //TODO: Check if tables exist in file
                        success = true;
                    }

                }
                else
                {
                    this.db_con = new SQLiteConnection("Data Source=.\\Database\\MFG_527.db;Version=3");
                }

            }
            catch
            {
                success = false;
            }
            return success;
        }
        /************************************************************************************************************
        * LogTestInstance() - Logs a test instance to the Test_Instance table in the local database. This 
        * 
        * Parameters: - None
        * Returns:    - bool success - Returns true if the database log was successful
        *                            - Returns false if the database log was unsuccessful
        *             
        * **********************************************************************************************************/
        private bool LogTestInstance()
        {   
            bool success = false;
            
            try
            {

                SQLiteCommand cmd = new SQLiteCommand(this.db_con);
                cmd.CommandText = @"insert into Test_Instance(EQID,USER,LOCATION,TIMESTAMP,SERIAL_NUMBER,RESULT) VALUES('" + this.eqid + "','" + this.user_id + "','" + this.location + "','" + this.timestamp.ToString() + "','" + this.serial + "','" + this.result + "')";
                this.db_con.Open();
                cmd.ExecuteNonQuery();
                cmd.CommandText = @"select last_insert_rowid()";
                this.test_id = (long)cmd.ExecuteScalar();
                this.db_con.Close();
                success = true;

            }
            catch
            {
                success = false;
            }
            return success;
        }
        /************************************************************************************************************
        * LogTestData() - Logs the results of a test to the Tests table in the database
        * 
        * Parameters: - Hashtable table - Table contains keys to the data that needs to be logged.
        * Returns:    - bool success - Returns true if the database log was successful
        *                            - Returns false if the database log was unsuccessful
        *             
        * **********************************************************************************************************/
        private bool LogTestData(Hashtable table)
        {
            bool success = true;
            
               
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(this.db_con);
                cmd.CommandText = @"insert into Tests(TEST_ID, SERIAL_NUMBER, TEST_NAME, UPPER_BOUND, LOWER_BOUND, MEASURED, RESULT) VALUES('" + table["test_id"] + "','" + table["serial_number"] + "','" + table["test_name"] + "','" + table["upper_bound"] + "','" + table["lower_bound"] + "','" + table["measured"] + "','" + table["result"] + "')";
                this.db_con.Open();
                cmd.ExecuteNonQuery();
                this.db_con.Close();
            }
            catch
            {
                //Throw exception to RUN TEST function so say which item failed.
                this.db_con.Close();

            }

            return success;
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
