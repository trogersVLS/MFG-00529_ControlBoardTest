/* GUI_Main.cs
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
        readonly string REVISION = "A";

        readonly string VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                                  Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
        readonly string ABOUT_VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                                  Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + "." +
                                  Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();

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
        USER USER_ID;
        string SERIAL_NUM;
        string RESULT;

        private bool Powered = false;
        private bool Telnet = false;


        //DHCP Vars
        bool DHCP_ENABLED;
        string DHCP_START;
        string DHCP_END;

        List<TestData> Tests= new List<TestData>();
        List<TestData> Programming_Steps = new List<TestData>();
        List<TestData> Startup_Steps = new List<TestData>();
        List<TestData> PowerDown_Steps = new List<TestData>();

      
        //Test Equipment
        private MccDaq_GPIO GPIO;
        private Test_Equip DMM;
        private Test_Equip PPS;
        private VOCSN_Serial SOM;
        private VLS_Tlm VENT;

        //Functional Test Data
        private FunctionalTest FCT;
        private readonly ConcurrentQueue<string> message_queue = new ConcurrentQueue<string>();
        private List<TestData> failedTests;
        private List<TestData> passedTests;

        public MainForm(GUIConsoleWriter logConsole = null)
        {
            this.ConsoleLog = logConsole;
            //Initialize the GUI components
            InitializeComponent();
            this.Text += " v" + this.VERSION;

            //Initialize program settings from configuration file
            this.InitSettings();

            //Initialize GUI settings
            this.InitGUI();

            
        }

        /* *********************************************************************
         * User Methods
         * 
         * 
         ***********************************************************************/
        private void ChangeUser()
        {

            //Show login form
            var login = new LoginForm(this.DB_FILEPATH);

            this.USER_ID = login.ShowForm();
            login.Dispose();

            if (this.USER_ID == null)
            {   
                
                Application.Exit();
                return;
            }
            
            try
            {
                if (this.USER_ID.admin)
                {
                    this.File_AddUser.Enabled = true;
                    this.Settings_LoggingLabel.Enabled = true;
                }
                else
                {
                    this.File_AddUser.Enabled = false;
                    this.Settings_LoggingLabel.Enabled = false;
                }
            }
            catch
            {

            }
  
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

            this.ChangeUser();
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
            //Update status bar values
            this.Status_LocTag.Text = this.TEST_LOCATION;
            this.Status_RevTag.Text = REVISION;
            this.Status_ToolTag.Text = this.EQID;
            if(this.USER_ID == null)
            {
                Application.Exit();
            }
            else if (this.USER_ID.name == "")
            {
                this.Status_UserTag.Text = "TEST";
            }
            else
            {
                this.Status_UserTag.Text = this.USER_ID.name;
            }

            //Disable the action panel
            this.Panel_Actions.Enabled = false;
           
            //Enable Settings Panel
            this.Panel_Settings.Enabled = true;
            this.Field_SerialNumber.Enabled = true;
           
            
            //Disable the check buttons
            this.Check_FCT.Enabled = false;
            this.Check_SingleTest.Enabled = false;



            //
            this.Field_SerialNumber.Focus();
            this.DisplayMessage("Please enter the serial number");
            
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
                
                message.Report("Starting test: " + test.name + "\n");
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
        private async void Button_Run_Click(object sender, EventArgs e)
        {
            this.Panel_Actions.Enabled = false;
            this.Panel_Settings.Enabled = false;

            Progress<int> progress = new Progress<int>(i => this.ProgressBar.Value = i);
            Progress<string> message = new Progress<string>(s => this.DisplayMessage(s));
            Progress<string> log = new Progress<string>(s => this.LogMessage(s));

            //Get current program state
            this.ProgressBar.Value = 0;
            if (Check_FCT.Checked)
            {

                if (this.Powered && this.Telnet)
                {
                    
                    LogTestInstance();
                    int pass = 0;
                    int fail = 0;
                    UpdateStatus("busy");
                    UpdateTestCount(pass, fail);
                    int ctr = 1;
                    int total = this.Tests.Count;
                    bool success = true;

                    this.failedTests = new List<TestData>();
                    this.passedTests = new List<TestData>();
                    
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
                        this.ProgressBar.Value = (ctr * 100) / total;
                        ctr++;
                    }

                    if (fail == 0)
                    {
                        UpdateStatus("pass");
                        this.RESULT = "PASS";

                        
                    }
                    else
                    {
                        this.RESULT = "FAIL";
                        UpdateStatus("fail");
                    }
                    MessageBox.Show(this.RESULT, "RESULTS");
                    LogTestResult();
                }
            }
            else if (Check_SingleTest.Checked)
            {
                if(this.Powered && this.Telnet)
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
                                    TestToRun = t;
                                    LogTestInstance();
                                    int pass = 0;
                                    int fail = 0;
                                    UpdateStatus("busy");
                                    UpdateTestCount(pass, fail);


                                    this.ProgressBar.Value = 30;

                                    bool success = true;
                                    await Task.Factory.StartNew(() => (success = this.RunTest(this.TEST_ID, this.SERIAL_NUM, TestToRun, message, log)));

                                    this.ProgressBar.Value = 100;
                                    LogTestResult();

                                    if (success)
                                    {
                                        this.UpdateStatus("pass");
                                        this.UpdateTestCount(1, 0);
                                    }
                                    else
                                    {
                                        this.UpdateStatus("fail");
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
                       
                    }
                    catch (Exception exc)
                    {

                        string errormessage = "Exception caught: " + exc.Message.ToString();


                        MessageBox.Show(errormessage, "Exception caught", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DisplayMessage(errormessage);
                    }
                    
                }
                

            }

            this.Panel_Actions.Enabled = true ;
            this.Panel_Settings.Enabled = true;

            return;
        }

        private void UpdateStatus(string status)
        {
            if (status == "busy")
            {
                this.StatusIndicator.BackColor = System.Drawing.Color.Yellow;
                this.StatusLabel.Text = "Busy";
            }
            else if (status == "pass")
            {
                this.StatusIndicator.BackColor = System.Drawing.Color.Green;
                this.StatusLabel.Text = "Pass!";
            }
            else if (status == "fail")
            {
                this.StatusIndicator.BackColor = System.Drawing.Color.Red;
                this.StatusLabel.Text = "Fail!";
            }
            else if (status == "idle")
            {
                this.StatusIndicator.BackColor = System.Drawing.Color.DarkGray;
                this.StatusLabel.Text = "Idle";
            }
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
                                + this.USER_ID.name + "','" + this.TEST_LOCATION + "','" + timestamp + "','" + this.SERIAL_NUM + "','" 
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
            
            if (e.KeyData == Keys.Enter)
            {   
                //TODO: Check if the serial is valid and/or has changed.
                this.DisplayMessage("Serial Number =  " + this.Field_SerialNumber.Text);
                this.SERIAL_NUM = this.Field_SerialNumber.Text;

                this.Check_FCT.Enabled = true;
                this.Check_SingleTest.Enabled = true;

                this.Panel_Actions.Enabled = true;
                
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

                this.Panel_Actions.Enabled = false;
                this.Check_SingleTest.Enabled = false;
                this.Check_FCT.Enabled = false;
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
       
        /******************************************************************************************************
         * Check_SingleTest_CheckedChanged  
         * - When Check_SingleTest is checked, the dropdown list appears and on the first check, the dropdown
         * list is populated with all of the test names that are available.
         * - When Check_SingleTest is unchecked, the dropdown lists is hidden, but the test names are preserved.
         * 
         * ****************************************************************************************************/

        private void Check_SingleTest_CheckedChanged(object sender, EventArgs e)
        {
            if (Check_SingleTest.Checked)
            {
                Dropdown_Test_List.Enabled = true;
                this.Check_FCT.Checked = false;
                this.Button_PowerUp.Enabled = true;
                this.Button_Telnet.Enabled = true;

            }
            else
            {
                this.Button_PowerUp.Enabled = false;
                this.Button_Telnet.Enabled = false;
            }
        }

        private async void Button_Telnet_Click(object sender, EventArgs e)
        {
            Progress<string> message = new Progress<string>(s => this.Button_Telnet.Text = s);
            Progress<string> log = new Progress<string>(s => this.LogMessage(s));

            if (this.Powered && !this.Telnet)
            {
                //Device is already powered, can connect to Telnet now

                string ip = null;

                if (!this.DHCP_ENABLED)
                {
                    
                    ip = Microsoft.VisualBasic.Interaction.InputBox("Please enter the IP adress", "IP Address", null);
                    
                }
                else
                {
                    this.DisplayMessage("Searching for available IP addresses ...");
                    ip = this.DHCP_START;
                    
                }

                var success = false;

                //this.DisplayMessage("Trying to connect to " + ip);

               
                this.Button_Telnet.BackColor = System.Drawing.Color.Yellow;
                await Task.Factory.StartNew(() => (success = this.FCT.ConnectToTelnet(ip, message, log)));

                    

                
                

                if (success)
                {
                    this.Telnet = true;
                    this.Button_Telnet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                    this.Button_Telnet.Text = "Connected";
                    this.DisplayMessage("Successfully connected!");
                }
                else
                {
                    this.Telnet = false;
                    this.Button_Telnet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
                    this.Button_Telnet.Text = "Not Connected";
                }
            }
            else if(this.Powered && this.Telnet)
            {
                this.FCT.DisconnectTelnet();
                this.Telnet = false;
                this.Button_Telnet.Text = "Disconnected";
                this.Button_Telnet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            }
            
        }
        private async void Button_PowerUp_Click(object sender, EventArgs e)
        {
            //if (true)
            //{
            //    this.Powered = true;
            //    this.Button_PowerUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            //    return;
            //}
            bool success = false;
            if (!this.Powered)
            {
                Progress<int> progress = new Progress<int>(i => this.ProgressBar.Value = i);
                Progress<string> message = new Progress<string>(s => this.DisplayMessage(s));
                Progress<string> log = new Progress<string>(s => this.LogMessage(s));
                
                await Task.Factory.StartNew(() => (success = this.FCT.test_power_on(message, log, this.Startup_Steps[0])));
                if (success)
                {
                    this.Powered = true;
                    this.Button_PowerUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));

                }
            }
            else
            {
                //Turning off power --> Turn off telnet first
                this.Telnet = false;
                if (this.FCT.DisconnectTelnet())
                {
                    this.Telnet = false;
                }
                this.Button_Telnet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));


                //Power down
                if (this.FCT.test_power_down());
                {
                    this.Powered = false;
                }
                this.Button_PowerUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            }

        }
        
        /******************************************************************************************************
         * Check_Functional_CheckedChanged  
         * - When Check_Functional is checked, FullTest is unchecked
         *
         * 
         * ****************************************************************************************************/

        private void Check_Functional_CheckedChanged(object sender, EventArgs e)
        {
            if (Check_FCT.Checked)
            {
                this.ProgressBar.Value = 0;
                this.Check_SingleTest.Checked = false;
                this.Button_PowerUp.Enabled = true;
                this.Button_Telnet.Enabled = true;
            }
            else
            {

            }
        }

        /******************************************************************************************************
         * Reset_GUI
         * - Utility method to reset the GUI to a start-up state. Should be called after a test has finished.
         * 
         * ****************************************************************************************************/
        private void Reset_GUI()
        {
            
        }

        private void About_Version_Click(object sender, EventArgs e)
        {   

            MessageBox.Show("Version: " + this.ABOUT_VERSION + "\n\rRevision: : " + this.REVISION  , "Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void File_ChangeUser_Click(object sender, EventArgs e)
        {
            ChangeUser();

            //Update all user name references

            if (this.USER_ID.name == "")
            {
                this.Status_UserTag.Text = "TEST";
            }
            else
            {
                this.Status_UserTag.Text = this.USER_ID.name;
            }
        }
        private void File_AddUser_Click(object sender, EventArgs e)
        {
            
            var addUser = new AddUserForm(this.DB_FILEPATH);

            var success = addUser.ShowForm();

            addUser.Dispose();

        }
        private void Logging_Click(object sender, EventArgs e)
        {
            this.Settings_LoggingLabel.Checked ^= true;
        }

        private void File_ChangePassword_Click(object sender, EventArgs e)
        {
            var changePassword = new ChangePassForm(this.DB_FILEPATH);

            var success = changePassword.ShowForm(this.USER_ID);

            changePassword.Dispose();
        }

        private void Dropdown_Test_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ProgressBar.Value = 0;
        }

        private void FailCountIndicator_Click(object sender, EventArgs e)
        {
            //Show a new form listing the failed tests and their measurements

            this.Output_Window.AppendText("\n\r\n\r");

            foreach(TestData test in failedTests)
            {  
                this.Output_Window.AppendText("Test Name: " + test.name + ", ");
                this.Output_Window.AppendText("Measured: " + test.parameters["measured"] + ", ");
                this.Output_Window.AppendText("Result: " + test.parameters["result"] + "\n\r");
            }


        }

        private void PassCountIndicator_Click(object sender, EventArgs e)
        {
            //Show a new form listing the passed tests and their measurements

            this.Output_Window.AppendText("\n\r\n\r");

            foreach (TestData test in passedTests)
            {
                this.Output_Window.AppendText("Test Name: " + test.name + ", ");
                this.Output_Window.AppendText("Measured: " + test.parameters["measured"] + ", ");
                this.Output_Window.AppendText("Result: " + test.parameters["result"] + "\n\r");
            }

        }
    }

  
}
