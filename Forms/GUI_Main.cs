﻿/* GUI_Main.cs
 * Partial class GUI_Main
 * 
 * - To be used with GUI_Main.Designer.cs (a visual studio generated file)
 * 
 * Author: Taylor Rogers
 * Date: 10/23/2019
 * 
 */
using GPIO;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using VLS;
using System.Data.SQLite;
using System.Linq;
using System.Diagnostics;





namespace ControlBoardTest
{


    public partial class MainForm : Form
    {
        readonly string VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                                  Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
        readonly string ABOUT_VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                                  Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
                                    //+ "." + Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();

        //Non designer GUI components
        GUIConsoleWriter ConsoleLog;

        //Database variables
        SQLiteConnection DB_CON;
        string DB_FILEPATH;

        //Test Instance Variables
        long TEST_ID;
        string TEST_LOCATION;
        string EQID;
        string MFG_CODE;
        string SERIAL_NUM;
        string RESULT;
        string USER_NAME = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

        private bool Powered = false;
        private bool Telnet = false;
        SynchronizationContext synchronizationContext;
        //DHCP Vars
        //bool DHCP_ENABLED = false;
        //string DHCP_START=null;
        //string DHCP_END;

        List<TestData> Tests= new List<TestData>();
        List<TestData> Programming_Steps = new List<TestData>();
        List<TestData> Startup_Steps = new List<TestData>();
        List<TestData> PowerDown_Steps = new List<TestData>();

      
        //Test Equipment
        private MccDaq_GPIO GPIO;
        private Test_Equip DMM;
        private Test_Equip PPS;
        private VOCSN_Serial SOM;
        private VLS_Tlm VENT=null;

        //Functional Test Data
        private FunctionalTest FCT;
        private readonly ConcurrentQueue<string> message_queue = new ConcurrentQueue<string>();
        private List<TestData> failedTests = new List<TestData>();
        private List<TestData> passedTests = new List<TestData>();

        Stopwatch stopwatch = new Stopwatch();

        public MainForm(GUIConsoleWriter logConsole = null)
        {
            this.ConsoleLog = logConsole;
            //Initialize the GUI components
            InitializeComponent();
            this.Text += " v" + this.VERSION;
            synchronizationContext = SynchronizationContext.Current;
            //Initialize program settings from configuration file
            this.InitSettings();

            //Initialize GUI settings
            this.InitGUI();

            
        }

 



        /* *********************************************************************
         * Message Functions
         * 
         ***********************************************************************/

        //Displays a message to the Output window in Visual Studio.
        private void LogMessage(string text)
        {
            Debug.WriteLine(text);
        }
        //Displays a message on the UI screen
        private void DisplayMessage(string text)
        {
            LogMessage(text);
            this.Output_Window.AppendText(text + "\n");

            return;
        }

        /*****************************************************************************************************************************************
         * InitSettings
         * 
         * Function: Reads from the settings.xml file, collects the system configurations. Collects the specs This list is the tests that will
         * run during a full functional test.
         * 
         * Arguments: Device - Takes a string variable as the device connection type and attempts to reconnect to the device.
         * 
         * Returns: None - Updates class object instances
         *
         *********************************************************************************************************************************************/
        private void InitSettings()
        {
            //Don't need config file for GPIO module
            this.GPIO = new MccDaq_GPIO();

            //All installation specific settings stored in settings.xml
            XmlDocument configuration = new XmlDocument();
            configuration.Load(@".\Configuration\settings.xml");

            foreach(XmlNode xml in configuration.DocumentElement.ChildNodes)
            {
                if(xml.Name == "Installation")
                {
                    try
                    {
                        this.EQID = xml.Attributes["EQID"].Value;
                        this.TEST_LOCATION = xml.Attributes["Location"].Value;
                        this.MFG_CODE = xml.Attributes["MFG_CODE"].Value;
                        this.DB_FILEPATH = xml.Attributes["DatabasePath"].Value;
                        this.DB_CON = new SQLiteConnection("Data Source=" + this.DB_FILEPATH + ";Version=3");
                        //this.DHCP_ENABLED = bool.Parse(xml.Attributes["DHCP_Enable"].Value);
                        //this.DHCP_START = xml.Attributes["DHCP_Start"].Value; 
                        //this.DHCP_END = xml.Attributes["DHCP_End"].Value;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Program has encountered an error in the configuration file\n\rERROR: " + e.Message, "Configuration");
                        throw e;
                    }
                }

                else if (xml.Name == "EquipAddr")
                {   
                    foreach(XmlNode x in xml)
                    {
                        if(x.Name == "DMM")
                        {
                            try
                            {
                                string protocol = x.Attributes["protocol"].Value;
                                string name = x.Attributes["name"].Value;
                                string AddrDMM = x.Attributes["address"].Value;
                                int BaudRate = int.Parse(x.Attributes["baudrate"].Value);
                                int StopBits = int.Parse(x.Attributes["stopbits"].Value);

                                this.DMM = new Test_Equip(name, protocol, BaudRate, StopBits, AddrDMM);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Program has encountered an error in the configuration file\n\rERROR: " + e.Message, "Configuration");
                                throw e;
                            }
                        }
                        else if (x.Name == "PPS")
                        {
                            try
                            {
                                string protocol = x.Attributes["protocol"].Value;
                                string name = x.Attributes["name"].Value;
                                string AddrPPS = x.Attributes["address"].Value;
                                int BaudRate = int.Parse(x.Attributes["baudrate"].Value);
                                int StopBits = int.Parse(x.Attributes["stopbits"].Value);

                                this.PPS = new Test_Equip(name, protocol, BaudRate, StopBits, AddrPPS);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Program has encountered an error in the configuration file\n\rERROR: " + e.Message, "Configuration");
                                throw e;
                            }
                        }
                        else if (x.Name == "SOM")
                        {
                            try
                            {
                                string protocol = x.Attributes["protocol"].Value;
                                string name = x.Attributes["name"].Value;
                                string AddrSOM = x.Attributes["address"].Value;
                                //int BaudRate = int.Parse(x.Attributes["baudrate"].Value);
                                //int StopBits = int.Parse(x.Attributes["stopbits"].Value);

                                this.SOM = new VOCSN_Serial(AddrSOM);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Program has encountered an error in the configuration file\n\rERROR: " + e.Message, "Configuration");
                                throw e;
                            }
                        }
                    }
                }
                
                else if(xml.Name == "programming")
                {
                    try
                    {
                        XmlNode ProgramTestNames = xml;
                        this.Programming_Steps = this.Match_MethodsToTests(ProgramTestNames);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Program has encountered an error in the configuration file\n\rERROR: " + e.Message, "Configuration");
                        throw e;
                    }

                }
                else if(xml.Name == "startup")
                {
                    try
                    {
                        XmlNode StartupTestNames = xml;
                        this.Startup_Steps = this.Match_MethodsToTests(StartupTestNames);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Program has encountered an error in the configuration file\n\rERROR: " + e.Message, "Configuration");
                        throw e;
                    }
                }
                else if (xml.Name == "powerdown")
                {
                    try
                    {
                        XmlNode PowerDownTestNames = xml;
                        this.PowerDown_Steps = this.Match_MethodsToTests(PowerDownTestNames);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Program has encountered an error in the configuration file\n\rERROR: " + e.Message, "Configuration");
                        throw e;
                    }
                }
                else if (xml.Name == "tests")
                {
                    try
                    {
                        XmlNode TestNames = xml;
                        this.Tests = this.Match_MethodsToTests(TestNames);
                        foreach (TestData test in this.Tests)
                        {
                            this.Dropdown_Test_List.Items.Add(test.name);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Program has encountered an error in the configuration file\n\rERROR: " + e.Message, "Configuration");
                        throw e;
                    }

                }

            }

            this.DatabaseExist();

            //Initialize the functional test class object
            this.FCT = new FunctionalTest(this.GPIO, this.DMM, this.PPS, this.SOM, this.VENT);

            return;
        }


        /*****************************************************************************************************************************************
         * GetTests
         * 
         * Function: Reads from the specs.txt file to generate a list of tests that are available to the user. This list is the tests that will
         * run during a full functional test.
         * 
         * Arguments: None
         * 
         * Returns: None - Update the class variable Tests
         *
         *********************************************************************************************************************************************/
        private List<TestData> Match_MethodsToTests(XmlNode TestNodes)
        {
            //Get list of methods in this class
            FunctionalTest test = new FunctionalTest();
            MethodInfo[] methods = test.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            int step_num = 0;
            //Iterate through each xml node to find the matching function and create a list.

            List<TestData> Tests = new List<TestData>();
            foreach (XmlNode x in TestNodes.ChildNodes)
            {   if (x.NodeType != XmlNodeType.Comment)
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


        /*****************************************************************************************************************************************
         * InitSettings
         * 
         * Function: Initialize all GUI components to their initial settings
         *
         *********************************************************************************************************************************************/
         private void InitGUI()
        {
            // Update status bar values
            this.Status_LocTag.Text = this.TEST_LOCATION;
            this.Status_ToolTag.Text = this.EQID;
                        
            this.Status_UserTag.Text = USER_NAME;

            // Disable the Action and Status panel
            this.Panel_Actions.Enabled = false;
            this.Panel_Status.Enabled = false;

            // Enable Settings Panel
            this.Panel_Settings.Enabled = true;
            this.Field_SerialNumber.Enabled = true;
            
            // Disable the check buttons
            this.Check_FCT.Enabled = false;
            this.Check_SingleTest.Enabled = false;

            this.DisplayMessage("Please enter the serial number");
            this.ActiveControl = Field_SerialNumber;
        }

        /******************************************************************************************************
         * RunTest(): event handler for run test button click. 
         * 
         * Based on the state of the checkboxes, this event handler will initiate a long running async task
         * 
         * Program && FunctionalTest --> Runs the FunctionalTest.RunTest in an asynchronous task
         * Program --> Runs the FunctionalTest.Program() method in an asynchronous task
         * FunctionalTest -- > Runs the FunctionalTest.RunTest() method in an asynchronous task, won't program the board
         * None checked --> Does nothing. Displays a message on the debug console.
         * 
         * 
         * This function will wait until the task finishes before exiting. // Not sure if that is a good thing or not yet
         * 
         * ****************************************************************************************************/
        private bool RunTest(long TEST_ID, string SERIAL, TestData test, IProgress<string> message, IProgress<string> log)
        {
            bool success = false;
            
            var param = new object[] { message, log, test};
           

            try
            {
                
                message.Report("\nStarting test: " + test.name);
                //log.Report("Starting test: " + test.method_name);

                //Invoke the test function --> Each test will fill the parameters table to measured
                success = (bool)test.testinfo.Invoke(this.FCT, param);

                
            }
            catch(Exception e)
            {
                var error = e.InnerException.Message;
                
                test.parameters["measured"] = "ERROR";
                string errormessage = "Exception caught: " + error + "\n\rThe following test failed: " + test.name;

                log.Report(error);


                MessageBox.Show(errormessage, "Exception caught", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            this.GPIO.ClearAllButPower();
            message.Report("Clear GPIO ...");
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

            LogResults(test, SERIAL, TEST_ID);
            


            return success;
        }
        /******************************************************************************************************
         * Button_Run_Click(): event handler for run test button click. 
         * 
         * Based on the state of the checkboxes, this event handler will initiate a long running async task
         * 
         * Program && FunctionalTest --> Runs the FunctionalTest.RunTest in an asynchronous task
         * Program --> Runs the FunctionalTest.Program() method in an asynchronous task
         * FunctionalTest -- > Runs the FunctionalTest.RunTest() method in an asynchronous task, won't program the board
         * None checked --> Does nothing. Displays a message on the debug console.
         * 
         * 
         * This function will wait until the task finishes before exiting. // Not sure if that is a good thing or not yet
         * 
         * ****************************************************************************************************/
        private void Button_Run_Click(object sender, EventArgs e)
        {
            // Update panel settings
            this.Panel_Settings.Enabled = false;
            this.Panel_Actions.Enabled = false;
            this.Panel_Status.Enabled = true;

            // Update Start button text and color
            this.Button_Run.BackColor = System.Drawing.Color.DarkGray;
            this.Button_Run.Text = "Running";

            // Disable power button during testing
            this.Button_PowerUp.Enabled = false;

            // Update status for Pass/Fail
            this.UpdateTestCount(0, 0);

            // Clear the previous lists
            this.failedTests.Clear();
            this.passedTests.Clear();

            //Get current program state
            this.ProgressBar.Value = 0;

            // Start the clock
            stopwatch.Reset();
            stopwatch.Start();

            ButtonClickHandlerAsync();
        }

        private async void ExecuteTests()
        {
            Progress<int> progress = new Progress<int>(i => this.ProgressBar.Value = i);
            Progress<string> message = new Progress<string>(s => this.DisplayMessage(s));
            Progress<string> log = new Progress<string>(s => this.LogMessage(s));
            
            // We didn't successfuly connect
            if(!this.Telnet || !this.Powered)
            {
                // Turn off the power after the test is complete
                Button_PowerUp_Click(null, null);

                // reset GUI
                this.Button_Run.BackColor = System.Drawing.Color.SkyBlue;
                this.Button_Run.Text = "Start";

                // Update panel settings
                this.Panel_Settings.Enabled = true;
                this.Panel_Actions.Enabled = true;
                this.Panel_Status.Enabled = true;

                return;
            }


            if (Check_FCT.Checked)
            {               
                LogTestInstance();
                int pass = 0;
                int fail = 0;
                int total = this.Tests.Count;
                bool success = true;
                                        
                foreach (TestData test in this.Tests)
                {
                    await Task.Factory.StartNew(() => (success = this.RunTest(this.TEST_ID, this.SERIAL_NUM, test, message, log)));

                    if (!success)
                    {
                        fail++;
                        failedTests.Add(test);
                    }
                    else
                    {
                        pass++;
                        passedTests.Add(test);
                    }
                    test.SetResult(success);

                    UpdateTestCount(pass, fail);
                    this.ProgressBar.Value = ((pass + fail) * 100) / total;
                }

                // Turn off the power after the test is complete
                Button_PowerUp_Click(null, null);

                // Show PASS/FAIL message
                DisplayResult(fail);    

                // Reset the serial number if we passed 
                if (fail == 0)
                {                    
                    // Update panel settings
                    this.Panel_Settings.Enabled = true;
                    this.Field_SerialNumber.Text = "";
                    this.ActiveControl = Field_SerialNumber;
                }

                LogTestResult();          
            }
            else if (Check_SingleTest.Checked)
            {
                try
                {
                    //Get selected test
                    TestData TestToRun;
                    string selectedTest = this.Dropdown_Test_List.SelectedItem.ToString();
                    if(selectedTest != null)
                    {
                        foreach(TestData t in this.Tests)
                        {
                            if(t.name == selectedTest)
                            {
                                LogTestInstance();
                                TestToRun = t;
                                this.ProgressBar.Value = 30;
                                bool success = true;

                                await Task.Factory.StartNew(() => (success = this.RunTest(this.TEST_ID, this.SERIAL_NUM, TestToRun, message, log)));
                                this.ProgressBar.Value = 100;
                                LogTestResult();

                                if (success)
                                {
                                    this.UpdateTestCount(1, 0);
                                }
                                else
                                {
                                    this.UpdateTestCount(0, 1);
                                }

                                break;
                            }
                            else
                            {
                                //Nothing
                            }
                        }
                    }

                    // Enable the power button so that the user can shut off power at the end of the single tests
                    this.Button_PowerUp.Enabled = true;
                }
                catch (Exception exc)
                {
                    string errormessage = "Exception caught: " + exc.Message.ToString();
                    MessageBox.Show(errormessage, "Exception caught", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DisplayMessage(errormessage);
                }            
            }

            // reset GUI
            this.Button_Run.BackColor = System.Drawing.Color.SkyBlue;
            this.Button_Run.Text = "Start";

            // Update panel settings
            this.Panel_Settings.Enabled = true;
            this.Panel_Actions.Enabled = true;
            this.Panel_Status.Enabled = true;

            return;
        }

        void DisplayResult(int fail)
        {
            final_result holder = new final_result();

            if (fail == 0)
            {
                this.RESULT = "PASS";
                holder.set_path("pass.png");
                holder.set_userText("Mark unit as passed and move DUT to the next station");
            }
            else
            {
                this.RESULT = "FAIL";
                holder.set_path("fail.png");                
                holder.set_userText("Remove unit from fixture and note failures in Factory Logix");
            }

            stopwatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopwatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            // Set the total test time
            holder.set_testTime(elapsedTime);

            holder.ShowDialog();   // dlr experiment.  this will show a nice large pass fail graphic.  see if the line likes it or not. 
        }


        private void UpdateTestCount(int pass, int fail)
        {
            this.PassCountIndicator.Text = pass.ToString();
            this.FailCountIndicator.Text = fail.ToString();
        }
        private bool DatabaseExist()
        {

            SQLiteCommand cmd;
            
            bool success = false;


            try
            {
                if (!File.Exists(this.DB_FILEPATH))
                {
                    SQLiteConnection.CreateFile(this.DB_FILEPATH);

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

                    this.DB_CON = new SQLiteConnection("Data Source=" + this.DB_FILEPATH + ";Version=3");
                    this.DB_CON.Open();

                    cmd = new SQLiteCommand(test_instance_table, this.DB_CON);
                    //cmd.CommandText = test_instance_table;
                    int d = cmd.ExecuteNonQuery();

                    cmd.CommandText = tests_table;
                    d = cmd.ExecuteNonQuery();

                    this.DB_CON.Close();
                    if (File.Exists(this.DB_FILEPATH))
                    {
                        //TODO: Check if tables exist in file
                        success = true;
                    }

                }
                else
                {
                    this.DB_CON = new SQLiteConnection("Data Source=" + this.DB_FILEPATH + ";Version=3");
                }

            }
            catch
            {
                success = false;
            }
            return success;
        }
        private bool LogTestInstance()
        {
            bool success = false;
            var timestamp = DateTime.UtcNow.ToString();
            this.RESULT = "In Progress";

            try
            {

                SQLiteCommand cmd = new SQLiteCommand(this.DB_CON);
                cmd.CommandText = @"insert into Test_Instance(EQID,USER,LOCATION,TIMESTAMP,SERIAL_NUMBER,RESULT) VALUES('" + this.EQID + "','"
                                + this.USER_NAME + "','" + this.TEST_LOCATION + "','" + timestamp + "','" + this.SERIAL_NUM + "','" 
                                + this.RESULT + "')";
                this.DB_CON.Open();
                cmd.ExecuteNonQuery();
                cmd.CommandText = @"select last_insert_rowid()";
                this.TEST_ID = (long)cmd.ExecuteScalar();
                this.DB_CON.Close();
                success = true;

                

            }
            catch
            {
                success = false;
            }
            return success;
        }
        private bool LogTestResult()
        {
            bool success = false;
            
            if(this.RESULT == "In Progress")
            {
                this.RESULT = "PASS";
            }

            try
            {

                SQLiteCommand cmd = new SQLiteCommand(this.DB_CON);
                cmd.CommandText = "UPDATE Test_Instance " + 
                                   "SET RESULT = \'" + this.RESULT +
                                   "\' WHERE TEST_ID = \'" + this.TEST_ID.ToString() + "\';";
                this.DB_CON.Open();
                cmd.ExecuteNonQuery();
                this.DB_CON.Close();
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
        private bool LogResults(TestData test, string serial, long testid)
        {
            bool success = true;

            
            string testID = testid.ToString();
            string serialNumber = serial;
            string testName = test.name;
            string result = test.parameters["result"];
            string upperBound;
            string lowerBound;
            string measured;


            if (test.parameters["qual"] != "true")
            {
                upperBound = test.parameters["upper"];
                lowerBound = test.parameters["lower"];
                measured = test.parameters["measured"];
            }
            else
            {
                upperBound = "--";
                lowerBound = "--";
            }

            if(!test.parameters.TryGetValue("measured", out measured))
            {
                measured = "--";
            }
            


            try
            {
                SQLiteCommand cmd = new SQLiteCommand(this.DB_CON);
                cmd.CommandText = @"insert into Tests(TEST_ID, SERIAL_NUMBER, TEST_NAME, UPPER_BOUND, LOWER_BOUND, MEASURED, RESULT) 
                                    VALUES('" + testID + "','" + serial + "','" + testName + "','" + upperBound + "','" + lowerBound + "','" +
                                    measured + "','" + result + "')";
                this.DB_CON.Open();
                cmd.ExecuteNonQuery();
                this.DB_CON.Close();
            }
            catch
            {
                //Throw exception to RUN TEST function so say which item failed.
                this.DB_CON.Close();

            }

            return success;
        }

        

    

    /******************************************************************************************************
     * EndTest()
     * - Method displays a dialog prompt to let the user save to a new location.
     *   The new save location is then updated in the configuration file.
     *   
     * 
     * ****************************************************************************************************/
        private void EndTest(string result)
        {

            
            var user_confirmation = MessageBox.Show("Test is finished, save text output?", result, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (user_confirmation == DialogResult.Yes)
            {
                SaveFileDialog sv = new SaveFileDialog();
                sv.Filter = "All Fields | *.txt";
                sv.Title = "Save test output";
                DialogResult okay = sv.ShowDialog();
                if (okay == DialogResult.OK)
                {
                    //Update the save file location in the config file
                    //Save the result file to the location specified.
                    
                    File.WriteAllText(sv.FileName, Output_Window.Text);

                }
                sv.Dispose();

            }
            else
            {

            }

            

        }
       
        /******************************************************************************************************
         * Field_SerialNumber_KeyUp
         * - Method checks to see if the user has finished inputting a serial number by pressing <Enter>.
         *   This allows the user to use a scanner to enter the serial number of the boards.
         *   
         *   TODO: Create file for saved configuration settings. This will allow for each installation of this 
         *   program to be configured specifically for the manufacturer
         *   
         *   Serial number format: @ A # # $ # # #
         *   @ - Manufacturer code
         *   # - A number
         *   $ - Letter to indicate the two week period in which the board was made
         *   
         * 
         * ****************************************************************************************************/
        private void Field_SerialNumber_KeyUp(object sender, KeyEventArgs e)
        {   
            //Only update the serial number if the user hits enter.            
            if (e.KeyData == Keys.Enter && !String.IsNullOrWhiteSpace(this.Field_SerialNumber.Text))
            {   
                //TODO: Check if the serial is valid and/or has changed.
                this.DisplayMessage("Serial Number =  " + this.Field_SerialNumber.Text);
                this.SERIAL_NUM = this.Field_SerialNumber.Text;

                // Enable option to click test mode
                this.Check_FCT.Enabled = true;
                this.Check_SingleTest.Enabled = true;

                // Default select the full test
                this.Check_FCT.Checked = true;

                // Allow the user to click the start button or click the status buttons
                this.Panel_Actions.Enabled = true;
                this.Panel_Status.Enabled = true;                
            }
           

        }
        private void Field_SerialNumber_TextChanged(object sender, EventArgs e)
        {
            if (this.SERIAL_NUM != null)
            {
                this.SERIAL_NUM = null;
                //Disable all components and edit serial number

                this.Field_SerialNumber.Enabled = true;
                this.Field_SerialNumber.Text = "";

                // Disable the action and status panels
                this.Panel_Actions.Enabled = false;
                this.Panel_Status.Enabled = false;

                // Disable test mode selections
                this.Check_SingleTest.Enabled = false;
                this.Check_FCT.Enabled = false;

                // Reset the progress bar
                this.ProgressBar.Value = 0;
            }
        }
        /******************************************************************************************************
         * Console_DebugOutput_TextChanged   
         * - Method to scroll the rich text box to the end after each text change
         * 
         * 
         * ****************************************************************************************************/
        private void Output_Window_TextChanged(object sender, EventArgs e)
        {
            this.Output_Window.SelectionStart = this.Output_Window.Text.Length;
            this.Output_Window.ScrollToCaret();
        }
       

        async void ButtonClickHandlerAsync()
        {
            Progress<string> message = new Progress<string>(s => this.DisplayMessage(s));
            Progress<string> log = new Progress<string>(s => this.LogMessage(s));
            bool success = false;
            
            if (!this.Powered)
            {
                await Task.Run(() =>
                {
                    success = this.FCT.test_power_on(message, log, this.Startup_Steps[0]);
                });

                if (success)
                {
                    this.Powered = true;
                    this.Button_PowerUp.Text = "Powered";
                    this.Button_PowerUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                }
            }
            // connecting Telnet
            if (this.Powered && !this.Telnet)
            {
                ConnectTelnet();
            }

            // already powered and connected
            if (this.Powered && this.Telnet)
            {
                ExecuteTests();
            }
        }

        async void ConnectTelnet()
        {
            Progress<string> message = new Progress<string>(s => this.DisplayMessage(s));
            Progress<string> log = new Progress<string>(s => this.LogMessage(s));
            bool success = false;
            string ip = "10.10.0.5";
            await Task.Run(() =>
            {
                WaitForTelnetConnection(18);    // wait for HDCP to assign IP address
                UpdateDisplayMessage("Trying to connect to " + ip);
                success = this.FCT.ConnectToTelnet(ip, message, log);

                //Let user know that the unit must be in Clinician mode or it wont connect.
                if(!success)
                    MessageBox.Show("Ensure that the unit is in Clinician mode (PW: 1234)", "Telnet Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                //Try connecting again
                int timeout = 10;
                while (timeout > 0 && !success)
                {
                    UpdateDisplayMessage($"Attempting to connect to Telnet: {timeout} attempts");
                    timeout--;
                    success = this.FCT.ConnectToTelnet(ip, message, log);
                }
            });

            if (success)
            {
                this.Telnet = true;
                this.Button_Telnet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                UpdateButtonText("Connected");
                UpdateDisplayMessage("Successfully connected!");                
            }
            else
            {
                this.Telnet = false;
                this.Button_Telnet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
                UpdateButtonText("Not Connected");
            }
            ExecuteTests();
        }

        void WaitForTelnetConnection(int timeout)
        {
            // wait for connection
            while (timeout > 0)
            {
                UpdateDisplayMessage($"Wait for DHCP assigning IP: {timeout} seconds");
                timeout--;
                Thread.Sleep(1000);
            }
        }

        // call to the SynchronizationContext to update the UI DisplayMessage
        void UpdateDisplayMessage(string msg)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                this.DisplayMessage((string)o);
            }), msg);
        }

        void UpdateButtonText(string msg)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                this.Button_Telnet.Text = (string)o;
            }), msg);
        }

        //****************************************************************************************************************//
        private void Button_PowerUp_Click(object sender, EventArgs e)
        {
            // This button is only used to turn off power in Single Test Mode
            if (this.Powered)
            {
                //Turn off telnet first
                if (this.FCT.DisconnectTelnet())
                {
                    this.Telnet = false;
                    this.Button_Telnet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
                    UpdateButtonText("Not Connected");
                }

                //Power down
                if (this.FCT.test_power_down())
                {
                    this.Powered = false;
                    this.Button_PowerUp.Text = "Not Powered";
                    this.Button_PowerUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
                }                
            }
        }
        
        /******************************************************************************************************
         * Check_Functional_CheckedChanged  
         * - When Check_Functional is checked, SingleTest is unchecked
         *
         * 
         * ****************************************************************************************************/

        private void Check_Functional_CheckedChanged(object sender, EventArgs e)
        {
            this.ProgressBar.Value = 0;
            this.Check_SingleTest.Checked = !Check_FCT.Checked;
            Dropdown_Test_List.Enabled = false;
            Dropdown_Test_List.SelectedIndex = -1;
        }

        /******************************************************************************************************
         * Check_SingleTest_CheckedChanged  
         * - When Check_SingleTest is checked, the dropdown list appears and on the first check, the dropdown
         * list is populated with all of the test names that are available.
         * - When Check_SingleTest is unchecked, the dropdown lists is hidden, but the test names are preserved.
         * 
         * ****************************************************************************************************/

        private void Check_SingleTest_CheckedChanged(object sender, EventArgs e)
        {
            this.ProgressBar.Value = 0;
            this.Check_FCT.Checked = !Check_SingleTest.Checked;
            Dropdown_Test_List.Enabled = true;
            Dropdown_Test_List.SelectedIndex = 0;
        }
        

        /******************************************************************************************************/
        //              
        /*****************************************************************************************************/
        public bool Prompt_Modal_User_YesNo(string question, string TestName)
        {
            bool result;
            var output = System.Windows.Forms.MessageBox.Show(question, TestName, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

            if (output == System.Windows.Forms.DialogResult.Yes)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }


        /******************************************************************************************************
         * Reset_GUI
         * - Utility method to reset the GUI to a start-up state. Should be called after a test has finished.
         * 
         * ****************************************************************************************************/
        private void Reset_GUI()
        {
            
        }
        /******************************************************************************************************/
        private void About_Version_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version: " + this.ABOUT_VERSION, "Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /******************************************************************************************************/
        private void Logging_Click(object sender, EventArgs e)
        {
            this.Settings_LoggingLabel.Checked ^= true;
        }

        /******************************************************************************************************/
        private void Dropdown_Test_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ProgressBar.Value = 0;
        }
       
        /******************************************************************************************************/
        private void FailCountIndicator_Click(object sender, EventArgs e)
        {
            // nothing in the failed list
            if (!failedTests.Any())
                return;
            
            //Show a new form listing the failed tests and their measurements
            this.Output_Window.AppendText("\n\r\n\r");

            foreach(TestData test in failedTests)
            {  
                this.Output_Window.AppendText("Test Name: " + test.name + ", ");
                this.Output_Window.AppendText("Measured: " + test.parameters["measured"] + ", ");
                this.Output_Window.AppendText("Result: " + test.parameters["result"] + "\n\r");
            }
        }
        
        /******************************************************************************************************/
        private void PassCountIndicator_Click(object sender, EventArgs e)
        {
            // nothing in the failed list
            if (!passedTests.Any())
                return;

            //Show a new form listing the passed tests and their measurements

            this.Output_Window.AppendText("\n\r\n\r");

            foreach (TestData test in passedTests)
            {
                this.Output_Window.AppendText("Test Name: " + test.name + ", ");
                this.Output_Window.AppendText("Measured: " + test.parameters["measured"] + ", ");
                this.Output_Window.AppendText("Result: " + test.parameters["result"] + "\n\r");
            }
        }
        /******************************************************************************************************/
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
            //this.GPIO.ClearAll();
            this.GPIO = null;
        }
    }  
}
