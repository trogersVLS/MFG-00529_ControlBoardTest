/* Tests.cs
 * Partial class FunctionalTest
 * 
 * - To be used with FunctionalTest.cs
 * 
 * Author: Taylor Rogers
 * Date: 10/23/2019
 * 
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using MccDaq;
using ErrorDefs;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System.IO.Ports;
using GPIO;

namespace ControlBoardTest
{

/*********************************************************************************************************************************************************
    * FunctionalTest - This file contains all of the test step code needed for the functional test. The program can run any combination of these test steps
    * 
    *********************************************************************************************************************************************************/
    partial class FunctionalTest
    {

        const int VOLTAGE_DELAY = 500;
        /*******************************************************************************************************************************  
         *  CPLD_Verify
         *  
         *  Function: Performs a verification of the CPLD firmware on the board using a TCL Script run using Flashpro.exe
         *      Assumptions: Flashpro.exe must be installed in the default location on the C: drive.
         *  
         *  Arguments: IProgress<string> message - This argument allows the function to send updates back to the GUI thread
         *  
         *  Returns: bool success - returns true is the verification is successful, returns false if the verification is not successful
         * 
         *******************************************************************************************************************************/
        private bool CPLD_Verify(IProgress<string> message)
        {
            string VerifyScriptPath;
            string ResultFilePath;
            string Verify_CMD;
            string Verify_Success = "Executing action VERIFY PASSED";
            bool success;
            //TODO: Confirm that this is still true when a setup project is used to create the installer.
            //The path to the CPLD_Program script is always two directories up from the executing path.
            VerifyScriptPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            VerifyScriptPath = VerifyScriptPath.Remove(VerifyScriptPath.LastIndexOf("\\")); //Up one directory
            VerifyScriptPath = VerifyScriptPath.Remove(VerifyScriptPath.LastIndexOf("\\")); // Up two directories
            ResultFilePath = VerifyScriptPath + "\\ProgramLoad\\CPLDLoad\\Results\\VerifyResult.txt";
            VerifyScriptPath = VerifyScriptPath + "\\ProgramLoad\\CPLDLoad\\cpld_verify.tcl";

            Verify_CMD = "script:" + VerifyScriptPath + " logfile:" + ResultFilePath;

            System.Diagnostics.Process cpld_cmd = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo cpld_info = new System.Diagnostics.ProcessStartInfo();
            cpld_info.FileName = "C:\\Microsemi\\Program_Debug_v11.9\\bin\\flashpro.exe";
            cpld_info.Arguments = Verify_CMD;
            cpld_info.RedirectStandardOutput = true;
            cpld_info.UseShellExecute = false;
            cpld_cmd.StartInfo = cpld_info;
            cpld_cmd.Start();
            //string output = cpld_cmd.StandardOutput.ReadToEnd();
            message.Report("Starting programmer ...");
            while (!File.Exists(ResultFilePath))
            {
                Thread.Sleep(2000);
                message.Report("...");
            }
            if (CPLD_LogRead(ResultFilePath, Verify_Success))
            {
                message.Report("CPLD Verify Successful");
                success = true;
            }
            else
            {
                message.Report("CPLD Verify unsuccessful");
                success = false;
            }


            return success;
        }

        /******************************************************************************************************************************
         *CPLD_Program
         *  
         *  Function: Performs a verification of the CPLD firmware on the board using a TCL Script run using Flashpro.exe
         *      Assumptions: Flashpro.exe must be installed in the default location on the C: drive.
         *  
         *  Arguments: IProgress<string> message - This argument allows the function to send updates back to the GUI thread
         *  
         *  Returns: bool success - returns true is the verification is successful, returns false if the verification is not successful
         * 
         ******************************************************************************************************************************/
        private bool CPLD_Program(IProgress<string> message)
        {
            string ProgramScriptPath;
            string ResultFilePath;
            string Program_CMD;
            string Program_Success = "Executing action PROGRAM PASSED";
            bool success;

            //TODO: Confirm that this is still true when a setup project is used to create the installer.
            //The path to the CPLD_Program script is always two directories up from the executing path.
            ProgramScriptPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            ProgramScriptPath = ProgramScriptPath.Remove(ProgramScriptPath.LastIndexOf("\\")); //Up one directory
            ProgramScriptPath = ProgramScriptPath.Remove(ProgramScriptPath.LastIndexOf("\\")); // Up two directories
            ResultFilePath = ProgramScriptPath + "\\ProgramLoad\\CPLDLoad\\Results\\ProgramResult.txt";
            ProgramScriptPath = ProgramScriptPath + "\\ProgramLoad\\CPLDLoad\\cpld_program.tcl";

            Program_CMD = "script:" + ProgramScriptPath + " logfile:" + ResultFilePath;

            System.Diagnostics.Process cpld_cmd = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo cpld_info = new System.Diagnostics.ProcessStartInfo();
            cpld_info.FileName = "C:\\Microsemi\\Program_Debug_v11.9\\bin\\flashpro.exe";
            cpld_info.Arguments = Program_CMD;
            cpld_info.RedirectStandardOutput = true;
            cpld_info.UseShellExecute = false;
            cpld_cmd.StartInfo = cpld_info;
            cpld_cmd.Start();
            //string output = cpld_cmd.StandardOutput.ReadToEnd();
            message.Report("Starting programmer ...");
            while (!File.Exists(ResultFilePath))
            {
                Thread.Sleep(2000);
                message.Report("...");
            }
            if (CPLD_LogRead(ResultFilePath, Program_Success))
            {
                message.Report("CPLD Program Successful");
                success = true;
            }
            else
            {
                message.Report("CPLD Program unsuccessful");
                success = false;
            }


            return success;
        }
        /******************************************************************************************************************************
         *  CPLD_LogRead
         *  
         *  Function: Reads the Flashpro log file that is generated after a running the Flashpro .tcl file. Will open the log file and
         *  and determine if the content of the log file contains the pass phrase.
         *  
         *  
         *  Arguments: string path - the path to the logfile.
         *             string pass - the phrase that indicates a passing result
         *  
         *  Returns: bool success - returns true is the log file contains the pass phrase.
         *                          returns false if the log file does not contain the pass phrase.
         * 
         ******************************************************************************************************************************/
        private bool CPLD_LogRead(string path, string pass)
        {
            bool success;
            string file;
            
            file = File.ReadAllText(path);

            if (file.Contains(pass))
            {
                success = true;
            }
            else
            {
                success = false;
            }

            File.Delete(path);
            return success;
        }
        /******************************************************************************************************************************
         *  Hercules_Program
         *  
         *  Function: Programs the herculues microcontroller on the control board using a Uniflash script.
         *  
         *  Arguments: IProgress<string> message - A thread-safe string variable that is used to update the calling thread.(GUI in this case)
         *  
         *  Returns: bool success - returns true if the programming is unsuccessful
         *                          returns false if the programming is unsuccessful
         * 
         ******************************************************************************************************************************/
        private bool Hercules_Program(IProgress<string> message)
        {
            string HerculesScriptPath;
            string Hercules_CMD;
            string cmd_output;

            bool success;
            //TODO: Confirm that this is still true when a setup project is used to create the installer.
            //The path to the Herc_Program script is always two directories up from the executing path.
            HerculesScriptPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            HerculesScriptPath = HerculesScriptPath.Remove(HerculesScriptPath.LastIndexOf("\\")); //Up one directory
            HerculesScriptPath = HerculesScriptPath.Remove(HerculesScriptPath.LastIndexOf("\\")); // Up two directories
            HerculesScriptPath = HerculesScriptPath + "\\ProgramLoad\\HerculesLoad\\dslite.bat";

            Hercules_CMD = "/c " + HerculesScriptPath;

            //Sets up the shell that will be used to execute the hercules programming script
            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo cmd_info = new System.Diagnostics.ProcessStartInfo("cmd");
            cmd_info.FileName = "cmd.exe";
            cmd_info.Arguments = Hercules_CMD;
            cmd_info.CreateNoWindow = true;
            cmd_info.RedirectStandardOutput = true;
            cmd_info.RedirectStandardError = true;
            cmd_info.UseShellExecute = false;
            cmd_info.WorkingDirectory = HerculesScriptPath.Remove(HerculesScriptPath.LastIndexOf("\\")); // TODO: sets the current directory to the directory that the hercules program script is located by removing the file name at the end of the string
            cmd.StartInfo = cmd_info;
            message.Report("Starting Hercules programmer ...");


            cmd.Start(); //Executes the script and pauses until the script has finished executing

            cmd_output = cmd.StandardOutput.ReadToEnd();
            message.Report("Programmer exit");

            //Confirms that the script was successful by comparing the 
            if (cmd_output.Contains("Program verification successful")) // Contains may be somewhat slow for this use case TODO: Determine if there is a better way?
            {
                success = true;
            }
            else
            {
                success = false;
            }
            return success;
        }

        /******************************************************************************************************************************
         *  SOM_Program
         *  
         *  Function: Programs the SOM using the pre-installed bootloader
         *  
         *  Arguments: IProgress<string> message - Path to GUI thread for output messages
         *  
         *  Returns: bool success - returns true is the log file contains the pass phrase.
         *                          returns false if the log file does not contain the pass phrase.
         * 
         ******************************************************************************************************************************/
        private bool SOM_Program(IProgress<string> message)
        {
            bool success = false;
            bool timeout;



            if (this.SOM.Connected)
            {
                //Cycle power
                message.Report("Cycling Power ...\n");
                this.GPIO.setPort(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.val);
                if (!this.SOM.SOM_ReadUntil_UBoot())
                {
                    message.Report("Device did not boot to U-Boot\nCycling power again ...\n");
                    this.GPIO.setPort(GPIO_Defs.AC_EN.port, 0x00);
                    Thread.Sleep(100);
                    this.GPIO.setPort(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.val);
                    if (!this.SOM.SOM_ReadUntil_Boot())
                    {
                        message.Report("Device does not boot properly.\nPowering down ...");
                        return false;
                    }
                }
                else
                {
                    message.Report("Successfully booted to U-Boot\nLoading QNX ...");
                    this.SOM.SOM_UBoot_CMD("mmc dev; fatload mmc 0 0x81000000 qnxifs; go 0x81000000");
                    if (!this.SOM.SOM_ReadUntil_Boot())
                    {
                        message.Report("Something went wrong ...");
                    }
                    else
                    {
                        message.Report("Formatting NAND");
                        string output = this.SOM.SOM_WriteCMD("fs-etfs-jacinto5_micron -D gpmc=0x50000000, cache, ipl=4, ifs=1024 -r131072 -e -m /fs/etfs");
                        //TODO: Add error checking

                    }

                }



            }
            else
            {
                success = false;
            }
            return success;
        }

        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for software update to finish by monitoring the serial port on the UUT. Then turns the power off.
         *            
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_software_install(IProgress<string> message, TestData test)
        {

            bool success = false;
            if(CopySoftware_USB(message, "MFG"))
            {   

                //Swap the USB drive to the UUT.
                message.Report("Turning on the device ...");
                this.GPIO.setPort(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.val);

                if (this.SOM.SOM_ReadUntil("Waiting 3 seconds for /fs/usb", true,  1000)) {
                    message.Report("Starting software update ...");

                    this.GPIO.setPort(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.val);
                    if (this.SOM.SOM_ReadUntil("Update IFS", true, 1000))
                    {
                        message.Report("Updating the NAND flash ...");

                        if (this.SOM.SOM_ReadUntil("display_image", true, 1000))
                        {
                            Thread.Sleep(500);
                            message.Report("Powering down device ...");
                            this.GPIO.setPort(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.val);
                            Thread.Sleep(500);
                            this.GPIO.setPort(GPIO_Defs.PB_BTN_ON.port, 0x00);

                            message.Report("\nSoftware update successful!");


                           if()
                            success = true;
                        }

                    }
 
                }
                
            }
            
            return success;
        }
        /******************************************************************************************************************************
         *  CopySoftware_USB
         *  
         *  Function: Prepares the USB drive with the correct files to perform a USB software update. Will look for the USB drive recursively until the device has
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             string sw_version - Acts as a keyword argument to switch between the two software versions (MFG or PRD)
         *  
         *  Returns: bool success - returns true if the usb drive is correctly prepared.
         *                          returns false if the usb drive is not correctly prepared.
         * 
         ******************************************************************************************************************************/
        private bool CopySoftware_USB(IProgress<string> message, string sw_version = "MFG", int iteration = 0)
        {
            bool success = false;
            string filepath;
            //Prepare USB Drive with the correct software, find the USB drive first
            DriveInfo[] alldrives = DriveInfo.GetDrives();

            if (sw_version == "MFG") filepath = ".\\ProgramLoad\\MFG_SW\\vswupdate";
            else if (sw_version == "PRD") filepath = ".\\ProgramLoad\\PRD_SW\\vswupdate";
            else return success;


            foreach (DriveInfo drive in alldrives)
            {
                //Special volume label for drive used as Software loader
                if (drive.IsReady)
                {
                    if (drive.VolumeLabel == "MFG527")
                    {
                        message.Report("Preparing USB drive ...");
                        //Delete the current software update files
                        string targetPath = drive.Name;
                        if (Directory.Exists(targetPath + "vswupdate"))
                        {
                            message.Report("Deleting directory: " + targetPath + "\\vswupdate");
                            Directory.Delete(targetPath + "vswupdate", true);
                        }
                        message.Report("Copying software update files to " + targetPath);
                        //Copy the update files for the intended software
                        CopyDirectory(filepath, targetPath + "vswupdate");
                        success = true;
                    }
                }
            }
            if(!success && iteration < 4) //Couldn't find the USB drive.
            {
                message.Report("Could not find the drive");
                ushort initialVal = 1;//this.GPIO.getPort(GPIO_Defs.USB_TGL.port);
                this.GPIO.setPort(GPIO_Defs.USB_TGL.port, (ushort)(GPIO_Defs.USB_TGL.val | initialVal));
                Thread.Sleep(100);
                this.GPIO.setPort(GPIO_Defs.USB_TGL.port, initialVal);
                Thread.Sleep(1000);
                this.ClearInput();
                
                CopySoftware_USB(message, sw_version, ++iteration);
            }

            Thread.Sleep(500);
            return success;
        }


        private static void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                //Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }


        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/

        private bool test_touch_cal(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_mfg_install(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_power_on(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_pre_use(IProgress<string> message, TestData test)
        {
            //Assumptions - Unit has been powered on
            //            - Unit is connected to telemetry
            //            - Unit has an electronic solenoid valve and connected to a burn-in lung
            //
            string str;
            bool success = false;

            //Confirm assumptions and correct if wrong
            //Device is powered on
            //this.GPIO.ReadPort()


            //Clear the queue
            this.ClearInput();

            //Blocking until user input is given --> Possible options are: "yes", "no" and "cancel"
            message.Report("Follow the on-screen instructions and click yes when you are done");
            if (!this.cancel_request)
            {   
                //
                str = ReceiveInput();
                if (str == "yes")
                {
                    message.Report("Test Passed!");
                    success = true;

                }
                else if (str == "no")
                {
                    message.Report("Test Failed");
                    success = false;
                }
            }
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_lcd(IProgress<string> message, TestData test)
        {
            //Assumptions - Unit has been powered on
            string str;
            bool success = false;

            //Clear the queue
            this.ClearInput();

            //Blocking until user input is given --> Possible options are: "yes", "no" and "cancel"
            message.Report("Is the LCD screen clear?");
            if (!this.cancel_request)
            {
                str = ReceiveInput();
                if (str == "yes")
                {
                    message.Report("Test Passed!");
                    success = true;

                }
                else if (str == "no")
                {
                    message.Report("Test Failed");
                    success = false;
                }
            }
            return success;
        }
        /******************************************************************************************************************************
         *  test_3V3_HOT
         *  
         *  Function: Tests the 3V3_HOT node.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if 3V3_HOT is within the upper and lower limits.
         *                          returns false if 3V3_HOT is outside the defined limits or the DMM is not connected.
         * 
         ******************************************************************************************************************************/
        private bool test_3V3_HOT(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper=0;
            float lower=0;
            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
            
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM and pause for a moment.
                this.GPIO.setPort(GPIO_Defs.MEAS_3V3_HOT_EN.port, GPIO_Defs.MEAS_3V3_HOT_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Read the DMM
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");


                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }  
                }
                catch
                {
                    success = false;
                    
                }

            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }
            if (!test.parameters.ContainsKey("measured"))
            {
                test.parameters.Add("measured", "err");
            }

            return success;
        }

        /******************************************************************************************************************************
         *  test_5V0_HOT
         *  
         *  Function: Tests the 5V0_HOT node.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if 5V0_HOT is within the upper and lower limits.
         *                          returns false if 5V0_HOT is outside the defined limits or the DMM is not connected.
         ******************************************************************************************************************************/
        private bool test_5V0_HOT(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;
            if (this.DMM.Connected)
            {
                    //Get parameters from test data object
                    value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.MEAS_5V0_HOT_EN.port, GPIO_Defs.MEAS_5V0_HOT_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }

            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }


            return success;
        }
        /******************************************************************************************************************************
         *  test_5V3_SMPS
         *  
         *  Function: Tests the +5V3 SMPS power supply.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if 5V3 SMPS is within the upper and lower limits.
         *                          returns false if 5V3 SMPS is outside the defined limits or the DMM is not connected.
         ******************************************************************************************************************************/
        private bool test_5V3_SMPS(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;
            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.MEAS_5V3_EN.port, GPIO_Defs.MEAS_5V3_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }

            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_12V0_SMPS
         *  
         *  Function: Tests the +12V0 SMPS power supply.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if 12V0 SMPS is within the upper and lower limits.
         *                          returns false if 12V0 SMPS is outside the defined limits or the DMM is not connected.
         ******************************************************************************************************************************/
        private bool test_12V0_SMPS(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;
            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.MEAS_12V0_EN.port, GPIO_Defs.MEAS_12V0_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }

            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_3V3_SMPS
         *  
         *  Function: Tests the +3V3 SMPS power supply.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if 3V3 SMPS is within the upper and lower limits.
         *                          returns false if 3V3 SMPS is outside the defined limits or the DMM is not connected.
         ******************************************************************************************************************************/
        private bool test_3V3_SMPS(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;
            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.MEAS_3V3_EN.port, GPIO_Defs.MEAS_3V3_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }

            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_1V2_SMPS
         *  
         *  Function: Tests the +1V2 SMPS power supply.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if 1V2 SMPS is within the upper and lower limits.
         *                          returns false if 1V2 SMPS is outside the defined limits or the DMM is not connected.
         ******************************************************************************************************************************/
        private bool test_1V2_SMPS(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;
            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.MEAS_1V2_EN.port, GPIO_Defs.MEAS_1V2_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }

            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }
        /******************************************************************************************************************************
        *  test_VREF
        *  
        *  Function: Tests the +2V048 LDO power supply.
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if 2V048 LDO is within the upper and lower limits.
        *                          returns false if 2V048 LDO is outside the defined limits or the DMM is not connected.
        ******************************************************************************************************************************/
        private bool test_VREF(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;
            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.MEAS_VREF_EN.port, GPIO_Defs.MEAS_VREF_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }

            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_3V3_LDO
         *  
         *  Function: Tests the +5V3 SMPS power supply.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if 5V3 SMPS is within the upper and lower limits.
         *                          returns false if 5V3 SMPS is outside the defined limits or the DMM is not connected.
         ******************************************************************************************************************************/
        private bool test_3V3_LDO(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;
            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.MEAS_3V3A_EN.port, GPIO_Defs.MEAS_3V3A_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }

            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }
        /******************************************************************************************************************************
 *  test_5V3_SMPS
 *  
 *  Function: Tests the +5V3 SMPS power supply.
 *  
 *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
 *             TestData test             - Variable that contains all of the necessary test data.
 *  
 *  Returns: bool success - returns true if 5V3 SMPS is within the upper and lower limits.
 *                          returns false if 5V3 SMPS is outside the defined limits or the DMM is not connected.
 ******************************************************************************************************************************/
        private bool test_30V_SMPS(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;
            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.MEAS_30V_EN.port, GPIO_Defs.MEAS_30V_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }

            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_36V_SMPS
         *  
         *  Function: Tests the +5V3 SMPS power supply.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if 36V SMPS is within the upper and lower limits.
         *                          returns false if 36V SMPS is outside the defined limits or the DMM is not connected.
         ******************************************************************************************************************************/
        private bool test_36V_SMPS(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;
            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.MEAS_36V_EN.port, GPIO_Defs.MEAS_36V_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }

            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_spi1_bus(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_i2c_vent(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_blower(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_exhalation(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_dac(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_sov(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_oxygen(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_sv9_off
         *  
         *  Function: Tests the +5V3 SMPS power supply.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if 5V3 SMPS is within the upper and lower limits.
         *                          returns false if 5V3 SMPS is outside the defined limits or the DMM is not connected.
         ******************************************************************************************************************************/
        private bool test_sv9_off(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;

            if (!this.Vent.Connected)
            {
                //Need to connect to device.
                this.Vent.Connect();
                
            }
        
            this.Vent.CMD_Write("mfgmode");
            this.Vent.CMD_Read();
            this.Vent.CMD_Write("set vcm sv 9 1");
            this.Vent.CMD_Read();
          
            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.val);
                this.GPIO.setPort(GPIO_Defs.MEAS_O2_SV1N_EN.port, GPIO_Defs.MEAS_O2_SV1N_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("SV1 Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("SV1 Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }
                

            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_sv10_off
         *  
         *  Function: Tests the +5V3 SMPS power supply.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if 5V3 SMPS is within the upper and lower limits.
         *                          returns false if 5V3 SMPS is outside the defined limits or the DMM is not connected.
         ******************************************************************************************************************************/
        private bool test_sv10_off(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;

            if (!this.Vent.Connected)
            {
                //Need to connect to device.

            }

            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.val);
                this.GPIO.setPort(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }


            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }
        /******************************************************************************************************************************
 *  test_sv9_off
 *  
 *  Function: Tests the +5V3 SMPS power supply.
 *  
 *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
 *             TestData test             - Variable that contains all of the necessary test data.
 *  
 *  Returns: bool success - returns true if 5V3 SMPS is within the upper and lower limits.
 *                          returns false if 5V3 SMPS is outside the defined limits or the DMM is not connected.
 ******************************************************************************************************************************/
        private bool test_sv9_on(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;

            if (!this.Vent.Connected)
            {
                //Need to connect to device.

            }

            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.val);
                this.GPIO.setPort(GPIO_Defs.MEAS_O2_SV1N_EN.port, GPIO_Defs.MEAS_O2_SV1N_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("SV1 Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("SV1 Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }


            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_sv10_off
         *  
         *  Function: Tests the +5V3 SMPS power supply.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if 5V3 SMPS is within the upper and lower limits.
         *                          returns false if 5V3 SMPS is outside the defined limits or the DMM is not connected.
         ******************************************************************************************************************************/
        private bool test_sv10_on(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;

            if (!this.Vent.Connected)
            {
                //Need to connect to device.

            }

            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.val);
                this.GPIO.setPort(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }
            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }

        /******************************************************************************************************************************
         *  test_cough
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_cough(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_neb
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_neb(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_suction
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_suction(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_low_fan_volt
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_low_fan_volt(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;
            float upper = 0;
            float lower = 0;

            if (!this.Vent.Connected)
            {
                //Need to connect to device.

            }

            if (this.DMM.Connected)
            {
                //Get parameters from test data object
                value_available = test.parameters.TryGetValue("upper", out str_value);
                if (value_available)
                {
                    upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }
                value_available = test.parameters.TryGetValue("lower", out str_value);
                if (value_available)
                {
                    lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
                }

                //Connect the desired voltage node to the DMM
                this.GPIO.setPort(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.val);
                Thread.Sleep(VOLTAGE_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Volts();
                test.parameters.Add("measured", measured.ToString());
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V\n");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                        success = true;
                    }
                    else
                    {
                        message.Report("Test FAIL");
                        success = false;
                    }
                }
                catch
                {
                    success = false;
                }
            }
            else
            {
                message.Report("Multimeter is not connected");
                success = false;
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_low_fan_freq(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_high_fan_volt(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_high_fan_freq(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_alarm_silence(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_ambient_pressure(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_ambient_temperature(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_microphone(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_speaker(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_piezo(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_external_ac(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_internal_battery(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_external_battery_1(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_external_battery_2(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_internal_chg_cc(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_internal_chg_cv(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_external1_chg_cc(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_external1_chg_cv(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_external2_chg_cc(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }
        /******************************************************************************************************************************
         *  test_software_install
         *  
         *  Function: Installs software via USB drive to the UUT. Requires some user interaction to finish the software update. 
         *            Loads software onto a USB drive, then switches the USB drive to the UUT. Powers the UUT up, and shorts CN309m.25 and CN309m.26 together
         *            to initiate the software update.
         *            Waits for user input to determine when software has finished updating. //TODO: Update this function to wait for SOM's serial port to tell us that the program updated successfully.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_external2_chg_cv(IProgress<string> message, TestData test)
        {
            bool success = false;
            return success;
        }


    }
}