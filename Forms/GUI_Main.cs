/* GUI_Main.cs
 * Partial class GUI_Main
 * 
 * - To be used with GUI_Main.Designer.cs (a visual studio generated file)
 * 
 * Author: Taylor Rogers
 * Date: 10/23/2019
 * 
 */
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using GPIO;




namespace ControlBoardTest
{


    public partial class GUI_Main : Form
    {
        //Non designer GUI components
        
        //Users that have admin privelege
        const string ADMIN = "admin";
        const string TEST = "TEST";
        
        //Log variable - Changes if program is run by an admin
        bool LogData = true;
        //Database variables
        string location;
        int eqid;
        string mfg_code;
        string user_id;
        string serial;


        //Equipment connections
        Test_Equip DMM;
        string dmm_address;
        Test_Equip PPS;
        string pps_address;
        MccDaq_GPIO GPIO;
        Programmer HERC;
        Programmer CPLD;
        Programmer SOM;
        string som_address;

        

        //Functional Test Data
        XmlNode TestSpecs;
        List<TestData> Session_Tests= new List<TestData>();
        FunctionalTest test;
        private readonly ConcurrentQueue<string> message_queue = new ConcurrentQueue<string>();

        public GUI_Main(string user)
        {   
            //Initialize the GUI components
            InitializeComponent();


            //Initialize Settings
            this.user_id = user;
            this.GetSettings();
            this.Reset_GUI();
            this.ConnectDevices();

        }
        /*****************************************************************************************************************************************
         * GetSettings
         * 
         * Function: Reads from the settings.xml file, collects the system configurations. Collects the specs This list is the tests that will
         * run during a full functional test.
         * 
         * Arguments: None
         * 
         * Returns: None - Update the class variable Tests
         *
         *********************************************************************************************************************************************/
        private void ConnectDevices()
        {

            //Add user indicator 
            this.HERC = new Programmer(ProgrammerType.HERCULES);
            if (this.HERC.Connected)
            {
                this.Button_Jtag_Herc.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                this.Button_Jtag_Herc.Enabled = false;
            }
            this.CPLD = new Programmer(ProgrammerType.CPLD);
            if (this.CPLD.Connected)
            {
                this.Button_Jtag_CPLD.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                this.Button_Jtag_CPLD.Enabled = false;
            }
            this.SOM = new Programmer(ProgrammerType.SOM, this.som_address);
            if (this.SOM.Connected)
            {
                this.Button_SOM.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                this.Button_SOM.Enabled = false;
            }
            this.DMM = new Test_Equip("DMM", "FLUKE", "RS232", this.dmm_address, 115200, System.IO.Ports.StopBits.One);
            if (this.DMM.Connected)
            {
                this.Button_DMM.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                this.Button_DMM.Enabled = false;
            }
            this.PPS= new Test_Equip("PPS", "HEWLETT", "RS232", this.pps_address, 1200, System.IO.Ports.StopBits.Two);
            if (this.PPS.Connected)
            {
                this.Button_PPS.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                this.Button_PPS.Enabled = false;
            }
            this.GPIO = new MccDaq_GPIO();
            if (this.GPIO.Connected)
            {
                this.Button_GPIO.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                this.Button_GPIO.Enabled = false;
            }
            else
            {
                this.GPIO = null;
            }


        }
        /*****************************************************************************************************************************************
         * ConnectDevices
         * 
         * Function: Connects to all peripheral devices that are used in this test fixture
         * 
         * Arguments: None
         * 
         * Returns: None - Creates new class object instances and updates the displayed connections 
         *
         *********************************************************************************************************************************************/
        private bool CheckDeviceConnections(string device)
        {
            bool connected=false;
            if (device == "hercules")
            {
                this.HERC.ConnectProgrammer();
                if (this.HERC.Connected)
                {
                    this.Button_Jtag_Herc.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                    this.Button_Jtag_Herc.Enabled = false;
                    connected = true;
                }
            }
            else if (device == "cpld")
            {
                this.CPLD.ConnectProgrammer();
                if (this.CPLD.Connected)
                {
                    this.Button_Jtag_CPLD.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                    this.Button_Jtag_CPLD.Enabled = false;
                    connected = true;
                }
            }
            else if (device == "som")
            {
                this.SOM.ConnectSOM(this.som_address);
                if (this.SOM.Connected)
                {
                    this.Button_SOM.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                    this.Button_SOM.Enabled = false;
                    connected = true;
                }
            }
            else if (device == "dmm")
            {
                this.DMM.ConnectDevice();
                if (this.DMM.Connected)
                {
                    this.Button_DMM.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                    this.Button_DMM.Enabled = false;
                    connected = true;
                }
            }
            else if (device == "pps")
            {
                this.PPS.ConnectDevice();
                if (this.PPS.Connected)
                {
                    this.Button_PPS.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                    this.Button_PPS.Enabled = false;
                    connected = true;
                }
            }
            else if(device == "gpio")
            {
                this.GPIO = new MccDaq_GPIO();
                if (this.GPIO.Connected)
                {
                    this.Button_GPIO.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
                    this.Button_GPIO.Enabled = false;
                    connected = true;
                }
                else
                {
                    this.GPIO = null;
                }
            }

            return connected;
        }
        /*****************************************************************************************************************************************
         * GetSettings
         * 
         * Function: Reads from the settings.xml file, collects the system configurations. Collects the specs This list is the tests that will
         * run during a full functional test.
         * 
         * Arguments: Device - Takes a string variable as the device connection type and attempts to reconnect to the device.
         * 
         * Returns: None - Updates class object instances
         *
         *********************************************************************************************************************************************/
        private void GetSettings()
        {
            //open the settings.xml file

            XmlDocument configuration = new XmlDocument();
            XmlNode settings_node;
            

            
            configuration.Load(@"..\..\Configuration\settings.xml");


            foreach(XmlNode xml in configuration.DocumentElement.ChildNodes)
            {
                if(xml.Name == "config")
                {
                    settings_node = xml;
                    this.eqid = Convert.ToInt32(settings_node.Attributes["eqid"].Value);
                    this.location = settings_node.Attributes["location"].Value;
                    this.mfg_code = settings_node.Attributes["mfg_code"].Value;
                    this.som_address = settings_node.Attributes["som_address"].Value;
                    this.dmm_address = settings_node.Attributes["dmm_address"].Value;
                    this.pps_address = settings_node.Attributes["pps_address"].Value;
                }
                else if (xml.Name == "specs")
                {
                    this.TestSpecs = xml;
                    
                    foreach(XmlNode node in xml.ChildNodes)
                    {
                        this.Dropdown_Test_List.Items.Add(node.Attributes["name"].Value);
                    }
                    
                }
            }



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
        private void GetTests(XmlNode nodes)
        {
            //Get list of methods in this class
            FunctionalTest test = new FunctionalTest();
            MethodInfo[] methods = test.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            int step_num = 0;
            //Iterate through each xml node to find the matching function.
            foreach (XmlNode x in nodes.ChildNodes)
            {
                foreach (MethodInfo method in methods)
                {
                    if (method.Name == x.Attributes["method_name"].Value)
                    {
                        int step = step_num;
                        string name = x.Attributes["name"].Value;
                        string method_name = x.Attributes["method_name"].Value;
                        Dictionary<string, string> parameters = new Dictionary<string, string>();
                        for(int i=0;i<x.Attributes.Count;i++){
                            parameters.Add(x.Attributes[i].Name, x.Attributes[i].Value);
                        }

                        TestData test_step = new TestData(step, name, method_name, method, parameters);
                                                          
                        this.Session_Tests.Add(test_step);
                        step_num++;
                        break;
                    }
                }

            }
            return;
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

            //Constant string variables for brevity
            const string run_disp = "Run";
            const string cancel_disp = "Cancel";
            //async progress variables. Update the linked property when changed, hooking the appropriate event handler to update the GUI.
            var progress = new Progress<int>(i => StatusBar.Value = i);
            var messages = new Progress<string>(s => this.console_debugOutput.AppendText("\n" + s));
            Hashtable Test_Params;

            this.StatusBar.Value = 0; //Set the statusbar value to 0 to indicate that the test is about to start.

            //Button is pressed while program is not doing anything
            if (this.Button_Run.Text == "Run")
            {   //Change button text to indicate to user that the button serves a new function now. 
                this.Button_Run.Text = "Cancel";

                //Running a new instance of a functional test
                if (this.LogData)
                {   
                    //Production environment, data is logged to local database.
                    //Create a dictionary to pass to the functional test class with all the information needed to run and log a test
                    Test_Params = Create_Test_Table();
                    this.test = new FunctionalTest(this.message_queue, Test_Params);
                }
                else
                {   
                    //Development environment, data is not logged to a database and is only displayed on the debug window.
                    this.test = new FunctionalTest(this.message_queue);
                }


                //Control board first pass. The board requires programming and functional testing
                if (Check_FullTest.Checked & Check_Program.Checked)
                {
                    console_debugOutput.Text = "Program and Functional Test";
                    this.Button_Yes.Show();
                    this.Button_No.Show();

                    
                    await Task.Factory.StartNew(() => this.test.Program(progress, messages),
                                                TaskCreationOptions.LongRunning);
                    await Task.Factory.StartNew(() => this.test.RunTest(progress, messages, this.test.Tests),
                                                TaskCreationOptions.LongRunning);

                }
                //Control board  not-first pass. The board only requires functional testing
                else if (Check_FullTest.Checked & !Check_Program.Checked)
                {

                    console_debugOutput.Text = "Functional Test only";
                    this.Button_Yes.Show();
                    this.Button_No.Show();

                    //TODO:Get the selected task and pass to function.
                    await Task.Factory.StartNew(() => this.test.RunTest(progress, messages, this.Session_Tests),
                                                                    TaskCreationOptions.LongRunning);




                }
                //Control board only requires programming. Will program the board with current production software
                else if (!Check_FullTest.Checked & Check_Program.Checked)
                {
                    this.Button_Run.Text = "Cancel";
                    this.Button_Yes.Show();
                    this.Button_No.Show();

                    console_debugOutput.Text = "Programming only";

                    await Task.Factory.StartNew(() => this.test.Program(progress, messages),
                                                TaskCreationOptions.LongRunning);


                }
                //Only a single test is requested. Can be used as an inspection tool / investigation tool
                else if (Check_SingleTest.Checked)
                {
                    this.Button_Run.Text = "Cancel";
                    this.Button_Yes.Show();
                    this.Button_No.Show();

                    try // Catch the inevitable null string exception.
                    {
                        string s = this.Dropdown_Test_List.SelectedItem.ToString();
                        List<TestData> singleTest = new List<TestData>();
                        //Grab the selected item from the test list
                        foreach (TestData test in Session_Tests)
                        {
                            if (test.name == s)
                            {
                                singleTest.Add(test);
                                break;
                            }
                        }
                        await Task.Factory.StartNew(() => this.test.RunTest(progress, messages, singleTest),
                                                TaskCreationOptions.LongRunning);

                    }
                    catch
                    {
                        console_debugOutput.Text = ("Please select a test");

                    }
                    
                }
                else
                {
                    console_debugOutput.Text = "Please select an action or exit the program";
                }
                this.StatusBar.Value = 100;
            }
            else
            {   //Clear the queue and send message to cancel functional test thread.
                while (!this.message_queue.IsEmpty)
                {
                    this.message_queue.TryDequeue(out string str);
                }
                this.message_queue.Enqueue("cancel");

            }
            //Reset state to exit method
            this.Button_Yes.Hide();
            this.Button_No.Hide();
            this.Button_Run.Text = "Run";

            // Reset GUI?
            this.EndTest();
        
        }

        private Hashtable Create_Test_Table()
        {   
            //Dictionary<string,string> param = new Dictionary<string,string>();
            Hashtable param = new Hashtable();
            //string test_id;
            string eqid;
            string user;
            string location;
            string timestamp;
            string serial;
            string result;
            //Test_id
            DateTime date = DateTime.UtcNow;
            //test_id = this.serial + "_" + date.Year.ToString() +
            //                                     date.DayOfYear.ToString() +
            //                                     date.Hour.ToString() +
            //                                     date.Minute.ToString() +
            //                                     date.Second.ToString() +
            //                                     date.Millisecond.ToString();
            //param.Add("test_id", test_id);
            //Equipment id --> Stored in settings.xml
            eqid = this.eqid.ToString();
            param.Add("eqid", Int32.Parse(eqid));
            //User id --> These usernames are stored in the settings.xml file. Each user will login prior to opening the program
            user = this.user_id;
            param.Add("user_id", user);
            //Location --> Also stored in the settings.xml file. Will be updated after the device moves to a new location
            location = this.location;
            param.Add("location", location);
            //Grabs the current timestamp in local time and converts to string, this value is human readable
            timestamp = DateTime.Now.ToString();
            param.Add("timestamp", timestamp);
            //Store the serial number, the serial number is unique to the board, but the board can be tested multiple times
            serial = this.serial;
            param.Add("serial", serial);
            //The result is false until the functional test class sets the result to true
            result = false.ToString();
            param.Add("result", result);

            //Pointers to test equipment instances (Power supply, gpio, dmm, SOM)
            param.Add("gpio", this.GPIO);
            param.Add("pps", this.PPS);
            param.Add("dmm", this.DMM);
            param.Add("som", this.SOM);

            return param;
        }
        /******************************************************************************************************
         * EndTest()
         * - Method displays a dialog prompt to let the user save to a new location.
         *   The new save location is then updated in the configuration file.
         *   
         * 
         * ****************************************************************************************************/
        private void EndTest()
        {

            
            var user_confirmation = MessageBox.Show("Test is finished, save debug output?", "Save!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                    
                    File.WriteAllText(sv.FileName, console_debugOutput.Text);


                }
                sv.Dispose();

            }
            else
            {

            }

            this.Reset_GUI();

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
            //This field is then locked until the GUI is reset after a test has run.
            if (e.KeyData == Keys.Enter)
            {   
                //TODO: Check if the serial is valid and/or has changed.
                this.console_debugOutput.Text = "Serial Number =  " + this.Field_SerialNumber.Text;
                this.serial = this.Field_SerialNumber.Text;
                this.Field_SerialNumber.Enabled = false;
                 
                //Uncheck all checkboxes;
                this.Uncheck_All();
                this.Button_Run.Show();
                this.Check_Program.Show();
                this.Check_FullTest.Show();
                this.Check_SingleTest.Show();
                
                this.GetTests(this.TestSpecs);
            }
           

        }
        /******************************************************************************************************
         * Console_DebugOutput_TextChanged   
         * - Method to scroll the rich text box to the end after each text change
         * 
         * 
         * ****************************************************************************************************/
        private void Console_debugOutput_TextChanged(object sender, EventArgs e)
        {
            this.console_debugOutput.SelectionStart = this.console_debugOutput.Text.Length;
            this.console_debugOutput.ScrollToCaret();
        }
        /******************************************************************************************************
         * Button_Yes_Click   
         * - Adds the message "yes" to the event handler for the Functional Test Thread to handle
         * 
         * 
         * ****************************************************************************************************/
        private void Button_Yes_Click(object sender, EventArgs e)
        {
            message_queue.Enqueue("yes");
        }
        /******************************************************************************************************
         * Button_No_Click   
         * - Adds the message "no" to the event handler for the Functional Test Thread to handle
         * 
         * 
         * ****************************************************************************************************/
        private void Button_No_Click(object sender, EventArgs e)
        {
            message_queue.Enqueue("no");
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
                Dropdown_Test_List.Show();
            }
            else
            {
                Dropdown_Test_List.Hide();
            }
        }
        /******************************************************************************************************
         * Check_LogToDatabase_CheckedChanged
         * - Event handler that allows user to change whether they want to log to the database during the session
         * 
         * ****************************************************************************************************/
        private void Check_LogToDatabase_CheckedChanged(object sender, EventArgs e)
        {
            if (Check_LogToDatabase.Checked)
            {
                this.LogData = true;
            }
            else
            {
                this.LogData = false;
            }
        }


        /******************************************************************************************************
         * Uncheck_All
         * - Utility method to uncheck all of the check boxes
         * 
         * ****************************************************************************************************/
        private void Uncheck_All()
        {
            this.Check_FullTest.Checked = false;
            this.Check_SingleTest.Checked = false;
            this.Check_Program.Checked = false;

        }
        /******************************************************************************************************
         * Reset_GUI
         * - Utility method to reset the GUI to a start-up state. Should be called after a test has finished.
         * 
         * ****************************************************************************************************/
        private void Reset_GUI()
        {
            this.Uncheck_All();

            //Hide buttons
            this.Button_No.Hide();
            this.Button_Yes.Hide();
            this.Button_Run.Hide();
            //Hide Checkboxes
            this.Check_FullTest.Hide();
            this.Check_Program.Hide();
            this.Check_SingleTest.Hide();
            //Clear serial number box
            this.Field_SerialNumber.ResetText();
            this.Field_SerialNumber.Enabled = true;
            //Reset status bar
            this.StatusBar.Value = 0;
            //Reset Debug_Output
            this.console_debugOutput.ResetText();
            this.console_debugOutput.AppendText("Please enter a serial number");
        }

        private void Button_Jtag_Herc_Click(object sender, EventArgs e)
        {
            CheckDeviceConnections("hercules");
        }

        private void Button_Jtag_CPLD_Click(object sender, EventArgs e)
        {
            CheckDeviceConnections("cpld");
        }

        private void Button_SOM_Click(object sender, EventArgs e)
        {
            CheckDeviceConnections("som");
            //if (!this.SOM.Connected)
            //{
            //    this.SOM.ConnectProgrammer();
            //    if (this.SOM.Connected)
            //    {
            //        //Disable the button and change the color to green
            //        this.Button_SOM.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);

            //    }

            //}
        }

        private void Button_DMM_Click(object sender, EventArgs e)
        {
            CheckDeviceConnections("dmm");
        }

        private void Button_PPS_Click(object sender, EventArgs e)
        {
            CheckDeviceConnections("pps");
        }

        private void Button_GPIO_Click(object sender, EventArgs e)
        {
            CheckDeviceConnections("gpio");
        }

    }

  
}
