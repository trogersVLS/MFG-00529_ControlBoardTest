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

        const int DMM_DELAY = 600;
        const int FREQ_DELAY = 1000;

        private bool dummy_test(IProgress<string> message = null, IProgress<string> log = null, object test = null)
        {
            

            return true;
        }
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
        private bool Verify_CPLD(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string VerifyScriptPath;
            string ResultFilePath;
            string Verify_CMD;
            string Verify_Success = "Executing action VERIFY PASSED";
            bool success;
            errorCode = -1;
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
        private bool Program_CPLD(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string ProgramScriptPath;
            string ResultFilePath;
            string Program_CMD;
            string Program_Success = "Executing action PROGRAM PASSED";
            bool success;
            errorCode = -1;



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
        private bool Program_Hercules(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string HerculesScriptPath;
            string Hercules_CMD;
            string cmd_output;

            bool success;
            errorCode = -1;
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
        private bool Program_SOM(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            bool Booted = false;
            bool Formatted = false;
            bool IPL_Installed = false;
            string output;

            if (this.SOM.Connected)
            {
                //Cycle power
                message.Report("Cycling Power ...\n");
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, 0);
                Thread.Sleep(100);
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                Booted = this.SOM.ReadUntil("U-Boot# ", out output, 10000);
                //Device needs to boot U-Boot
                if (!Booted)
                {
                    message.Report("Device did not boot to U-Boot\nCycling power again ...\n");
                    this.GPIO.SetBit(GPIO_Defs.AC_EN.port, 0x00);
                    Thread.Sleep(100);
                    this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                    Booted = this.SOM.ReadUntil("U-Boot# ", out output, 10000);

                    if (!Booted)
                    {
                        message.Report("Device does not boot properly.\nPowering down ...");
                        this.GPIO.SetBit(GPIO_Defs.AC_EN.port, 0);
                        success = false;
                    }
                }
                else if(Booted)
                {
                    message.Report("Successfully booted to U-Boot\nLoading QNX ...");
                    
                    this.SOM.Command("mmc dev; fatload mmc 0 0x81000000 qnxifs; go 0x81000000",out output, "U-Boot# ", 5000);

                    Booted = false;
                    Booted = this.SOM.ReadUntil("Welcome to QNX on the Ventec 3000 platform", out output, 20000);

                    if (!Booted)
                    {
                        message.Report("Device did not boot correctly");
                    }
                    else
                    {
                        message.Report("Formatting NAND");
                        Formatted = this.SOM.Command("fs-etfs-jacinto5_micron -D gpmc=0x50000000, cache, ipl=4, ifs=1024 -r131072 -e -m /fs/etfs", 
                                                  out output, "# ", 20000);
                        if (Formatted)
                        {
                            IPL_Installed = this.SOM.Command("update_nand -i -f /fs/sd0/ipl_nand.bin", out output, "# ", 10000);
                            if (IPL_Installed)
                            {
                                success = true;
                            }
                        }


                         

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
         *  test_mfg_install
         *  
         *  Function: Installs software via USB drive to the UUT.
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
        private bool test_mfg_install(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {

            bool success = false;
            string output;
            errorCode = -1;


            //Check to see if the current version of software is not MFG01.04

            output = this.Vent.CMD_Write("get vcm version");
            


            success = CopySoftware_USB(message, "MFG");

            if (success)
            {   

                //Swap the USB drive to the UUT.
                message.Report("Turning on the device ...");
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                success = this.SOM.ReadUntil("Waiting 3 seconds for /fs/usb", out output, 5000);

                if (success) {
                    success = false;
                    message.Report("Starting software update ...");

                    this.GPIO.SetBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);

                    success = this.SOM.ReadUntil("Update IFS", out output, 100000);
                    if (success)
                    {
                        success = false;
                        message.Report("Updating the NAND flash ...");

                        success = this.SOM.ReadUntil("display_image", out output, 100000);
                        if (success)
                        {
                            success = false;
                            // Turn off device using the power button.
                            Thread.Sleep(500);
                            message.Report("Powering down device ...");
                            this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);
                            Thread.Sleep(500);
                            this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, 0x00);
                            this.SOM.Booted = false;

                            message.Report("\nSoftware update successful!");

                            
                           
                            success = true;
                        }

                    }
 
                }
                
            }
            
            return success;
        }
        /******************************************************************************************************************************
         *  test_prd_install
         *  
         *  Function: Installs software via USB drive to the UUT.
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
        private bool test_prd_install(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {

            bool success = false;
            string output;
            errorCode = -1;

            success = CopySoftware_USB(message, "PRD");

            if (success)
            {

                //Swap the USB drive to the UUT.
                message.Report("Turning on the device ...");
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                success = this.SOM.ReadUntil("Waiting 3 seconds for /fs/usb", out output, 5000);

                if (success)
                {
                    success = false;
                    message.Report("Starting software update ...");

                    this.GPIO.SetBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);

                    success = this.SOM.ReadUntil("Update IFS", out output, 100000);
                    if (success)
                    {
                        success = false;
                        message.Report("Updating the NAND flash ...");

                        success = this.SOM.ReadUntil("display_image", out output, 100000);
                        if (success)
                        {
                            success = false;
                            // Turn off device using the power button.
                            Thread.Sleep(500);
                            message.Report("Powering down device ...");
                            this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);
                            Thread.Sleep(500);
                            this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, 0x00);
                            this.SOM.Booted = false;

                            message.Report("\nSoftware update successful!");



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
            else if (sw_version == "PRD") filepath = ".\\ProgramLoad\\PRD_SW\\vswupdate"; //TODO: Update KVSTORE bit
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
                this.GPIO.SetBit(GPIO_Defs.USB_TGL.port,GPIO_Defs.USB_TGL.pin);
                Thread.Sleep(100);
                this.GPIO.SetBit(GPIO_Defs.USB_TGL.port, initialVal);
                Thread.Sleep(1000);
                
                
                success = CopySoftware_USB(message, sw_version, ++iteration);
            }

            Thread.Sleep(500);
            return success;
        }

        /******************************************************************************************************************************
         *  CopyDirectory
         *  
         *  Function: Copies a directory to the target path.
         * 
         * 
         ******************************************************************************************************************************/
        private static void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }
        /******************************************************************************************************************************
         *  CopyAll
         *  
         *  Function: Copies all files and directories in the source directory to the target directory using recursion.
         * 
         * 
         ******************************************************************************************************************************/
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
         *  test_touch_cal
         *  
         *  Function: Powers on the device, disables the Watchdog, and prompts user to perform the touchscreen calibration.
         *  
         *  User selects NO if the device does not power up to a touchscreen calibration screen or if the touchscreen calibration is not
         *  successful.
         *  User selects YES after successfully performing a touchscreen calibration.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the user selects yes
         *                          returns false if the user selects no
         ******************************************************************************************************************************/

        private bool test_touch_cal(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            string output;
            errorCode = -1;

            this.GPIO.SetBit(GPIO_Defs.AC_EN.port, 0x00);
            

            this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

            message.Report("Does the device boot to the touchscreen calibration screen?");
            
            if(true)
            {
                message.Report("Perform the touchscreen calibration, select \"yes\" when done");

                success = this.SOM.ReadUntil("calib-touch", out output);

                if (!success)
                {
                    message.Report("Device did not power up correctly");
                }
            }
            else
            {
                success = false;
            }

         
            return success;
        }

        /******************************************************************************************************************************
         *  test_power_on
         *  
         *  Function: Powers on the device using the power button and/or applying power to the device. Prompts the user to perform the 
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the device powers up correctly and connects to a telnet connection.
         *                          returns false if the device does not power up correctly or connect to Telnet.
         * 
         ******************************************************************************************************************************/
        public bool test_power_on(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            //Determine if the devices are properly connected
            if (this.GPIO.Connected && this.SOM.Connected)
            {
                string output;
                int timeout;

                test.parameters.TryGetValue("timeout", out output);
                timeout = int.Parse(output, System.Globalization.NumberStyles.Integer);

                //Enables the AC power supply
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                //Toggles the power button in case the device does not begin booting right away (device was successfully powered down by the power button prior to testing).
                this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);
                Thread.Sleep(500);
                this.GPIO.ClearBit(GPIO_Defs.PB_BTN_ON.port,GPIO_Defs.PB_BTN_ON.pin);

                message.Report("Powering up ... ");

                if (this.SOM.ReadUntil("screen driver", out output, 20000))
                {   
                    //May need to calibrate touchscreen, which requires user interaction
                    if(!this.SOM.ReadUntil("calib-touch", out output, 5000))
                    {
                        //Device needs touchscreen calibration
                        do
                        {
                            this.PromptUser_YesNo("Please perform the touch screen calibration\n\nThen press \"OK\"", "Touchscreen Calibration");
                        } while (!this.SOM.ReadUntil("calib-touch", out output, 5000));
                    }
                    if (this.SOM.ReadUntil("Storyboard", out output, 50000))
                    {

                        this.powered = true;
                        message.Report("Successfully powered up");
                        success = true;


                    }
                }
            }
            else
            {
                if (!this.SOM.Connected)
                {
                    log.Report("SOM serial port is not connected");
                    errorCode |= 1;
                }
                
                if (!this.GPIO.Connected)
                {
                    log.Report("GPIO device is not connected");
                   
                }
            }

           
            return success;
        }
        /******************************************************************************************************************************
         *  test_power_down
         *  
         *  Function: Powers down the device using the power button and/or applying power to the device.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the device powers up correctly and connects to a telnet connection.
         *                          returns false if the device does not power up correctly or connect to Telnet.
         * 
         ******************************************************************************************************************************/
        public bool test_power_down(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            if (this.GPIO.Connected)
            {
                //Disables the AC power supply
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, 0);

                

               //message.Report("Powering down");

                this.Vent.Disconnect();
                
                success = true;

            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_power_down
         *  
         *  Function: Powers down the device using the power button and/or applying power to the device.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the device powers up correctly and connects to a telnet connection.
         *                          returns false if the device does not power up correctly or connect to Telnet.
         * 
         ******************************************************************************************************************************/
        public bool test_power_down()
        {
            bool success = false;
            
            if (this.GPIO.Connected)
            {
                //Disables the AC power supply and all battery supplies
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);
                if (this.Vent.Connected)
                {
                    this.Vent.Disconnect();
                }


                //TODO: Add feedback checking
                success = true;

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
        private bool test_pre_use(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            //Assumptions - Unit has been powered on
            //            - Unit is connected to telemetry
            //            - Unit has an electronic solenoid valve and connected to a burn-in lung
            //
            string str;
            bool success = false;
            errorCode = -1;

            //Confirm assumptions and correct if wrong
            //Device is powered on
            //this.GPIO.ReadPort()


            //Clear the queue


            //Blocking until user input is given --> Possible options are: "yes" or "no"
            message.Report("Follow the on-screen instructions and click yes when you are done");
            
            if (true)
            {
                message.Report("Test Passed!");
                success = true;

            }
            else if (str == "no")
            {
                message.Report("Test Failed");
                success = false;
            }
            
            return success;
        }
        /******************************************************************************************************************************
         *  test_lcd
         *  
         *  Function: Prompts the user to inspect the LCD display. 
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_lcd(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            //Assumptions - Unit has been powered on
            bool result;
            string measured;
            bool success = false;
            errorCode = -1;

            //Clear the queue


            //Blocking until user input is given --> Possible options are: "yes", or "no" 
            message.Report("Is the LCD screen clear?");

            result = this.PromptUser_YesNo("Is the LCD screen clear?", test.name);

            if (result)
            {
                message.Report("Test Passed!");
                measured = "pass";
                success = true;

            }
            else
            {

                measured = this.PromptUser("Describe the failure", test.name);

                message.Report("Test Failed");
                success = false;
            }

            test.parameters.Add("measured", measured);

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
        private bool test_3V3_HOT(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper=0;
            float lower=0;
            
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
            this.GPIO.SetBit(GPIO_Defs.MEAS_3V3_HOT_EN.port, GPIO_Defs.MEAS_3V3_HOT_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Read the DMM
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
            this.GPIO.ClearBit(GPIO_Defs.MEAS_3V3_HOT_EN.port, GPIO_Defs.MEAS_3V3_HOT_EN.pin);
            
            
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
        private bool test_5V0_HOT(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;
            
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
            this.GPIO.SetBit(GPIO_Defs.MEAS_5V0_HOT_EN.port, GPIO_Defs.MEAS_5V0_HOT_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
            this.GPIO.ClearBit(GPIO_Defs.MEAS_5V0_HOT_EN.port, GPIO_Defs.MEAS_5V0_HOT_EN.pin);




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
        private bool test_5V3_SMPS(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;
            
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
            this.GPIO.SetBit(GPIO_Defs.MEAS_5V3_EN.port, GPIO_Defs.MEAS_5V3_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
            this.GPIO.ClearBit(GPIO_Defs.MEAS_5V3_EN.port, GPIO_Defs.MEAS_5V3_EN.pin);



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
        private bool test_12V0_SMPS(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;
            
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
            this.GPIO.SetBit(GPIO_Defs.MEAS_12V0_EN.port, GPIO_Defs.MEAS_12V0_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
            this.GPIO.ClearBit(GPIO_Defs.MEAS_12V0_EN.port, GPIO_Defs.MEAS_12V0_EN.pin);




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
        private bool test_3V3_SMPS(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;
            
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
            this.GPIO.SetBit(GPIO_Defs.MEAS_3V3_EN.port, GPIO_Defs.MEAS_3V3_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
            this.GPIO.ClearBit(GPIO_Defs.MEAS_3V3_EN.port, GPIO_Defs.MEAS_3V3_EN.pin);



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
        private bool test_1V2_SMPS(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;
        
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
            this.GPIO.SetBit(GPIO_Defs.MEAS_1V2_EN.port, GPIO_Defs.MEAS_1V2_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
            this.GPIO.ClearBit(GPIO_Defs.MEAS_1V2_EN.port, GPIO_Defs.MEAS_1V2_EN.pin);



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
        private bool test_VREF(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;
            
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
            this.GPIO.SetBit(GPIO_Defs.MEAS_VREF_EN.port, GPIO_Defs.MEAS_VREF_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
            this.GPIO.ClearBit(GPIO_Defs.MEAS_VREF_EN.port, GPIO_Defs.MEAS_VREF_EN.pin);


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
        private bool test_3V3_LDO(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;
            
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
            this.GPIO.SetBit(GPIO_Defs.MEAS_3V3A_EN.port, GPIO_Defs.MEAS_3V3A_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
            this.GPIO.ClearBit(GPIO_Defs.MEAS_3V3A_EN.port, GPIO_Defs.MEAS_3V3A_EN.pin);



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
        private bool test_30V_SMPS(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;
            
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
            this.GPIO.SetBit(GPIO_Defs.MEAS_30V_EN.port, GPIO_Defs.MEAS_30V_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
            this.GPIO.ClearBit(GPIO_Defs.MEAS_30V_EN.port, GPIO_Defs.MEAS_30V_EN.pin);


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
        private bool test_36V_SMPS(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;
            
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
            this.GPIO.SetBit(GPIO_Defs.MEAS_36V_EN.port, GPIO_Defs.MEAS_36V_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
            this.GPIO.ClearBit(GPIO_Defs.MEAS_36V_EN.port, GPIO_Defs.MEAS_36V_EN.pin);



            return success;
        }


        /******************************************************************************************************************************
         *  test_blower
         *  
         *  Function: Tests the blower motor driver by commanding a specific speed, then measuring the frequency out.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             
         *             parameters: - speed : speed to test blower at
         *                         - tolerance: tolerance to measure speed to: in percentage
         *      
         *  
         *  Returns: bool success - returns true if the test passes
         ******************************************************************************************************************************/
        private bool test_blower(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            string ventOutput;
            float measured;

            //Note: TryParse not used here because in no situation should the user be able to edit the configuration file. Ergo, no need to catch the error unless working in a dev environment, which throwing an exception is more acceptable as it's useful for debugging what the hell happened.
            int speed = int.Parse(test.parameters["speed"]);            
            int tolerance = int.Parse(test.parameters["tolerance"]);

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {
                message.Report("Setting blower speed  to: " + speed.ToString() + " RPM");

                ventOutput = this.Vent.CMD_Write("set vcm testmgr speed " + speed.ToString());

                Thread.Sleep(500);
                //Measure value


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_FREQ_BLOWER.port, GPIO_Defs.MEAS_FREQ_BLOWER.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage
                measured = this.DMM.Get_Freq();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_FREQ_BLOWER.port, GPIO_Defs.MEAS_FREQ_BLOWER.pin);

                this.Vent.CMD_Write("set vcm testmgr stop");

                if((measured < speed *(1 + (tolerance/10))) && (measured > speed * (1 - (tolerance / 10))))
                {
                    success = true;
                }


                //Fill in measurement parameter
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = measured.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = measured.ToString();
                }
            }
            else
            {
                if (!this.powered)
                {
                    message.Report("UUT is not powered");
                }
                if (!this.Vent.Connected)
                {
                    message.Report("UUT is not connected to Telnet");
                }
                if (!this.GPIO.Connected)
                {
                    message.Report("GPIO Module is not connected");
                }
            }




            return success;
        }


        
        /******************************************************************************************************************************
         *  test_sov
         *  
         *  Function: Toggles the Solenoid on the shut off valve and measures the digital input from the device. 
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success
         * 
         ******************************************************************************************************************************/
        private bool test_sov(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            int on_state;
            string output;

            int measured;

            test.parameters.TryGetValue("on_state", out output);

            if(output == "high")
            {
                on_state = 1;
            }
            else if(output == "low")
            {
                on_state = 0;
            }

            this.Vent.CMD_Write("set vcm sv 11 0");

            //Read the input value
            measured = this.GPIO.GetBit(GPIO_Defs.SOV_SV11.port, GPIO_Defs.SOV_SV11.pin);
            if(measured == 1)
            {
                success = true;
            }

            this.Vent.CMD_Write("set vcm sv 11 1");

            //Read the input value
            measured = this.GPIO.GetBit(GPIO_Defs.SOV_SV11.port, GPIO_Defs.SOV_SV11.pin);
            if (measured == 0)
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
        private bool test_sv9_off(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;

            if (!this.Vent.Connected)
            {
                //Need to connect to device.
                this.Vent.Connect();
                message.Report("Connecting to device ...");
                
            }
            try
            {
                this.Vent.CMD_Write("mfgmode");
                
                this.Vent.CMD_Write("set vcm sv 9 0");
                
            }
            catch
            {

            }
          
            
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
            this.GPIO.SetBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
            this.GPIO.SetBit(GPIO_Defs.MEAS_O2_SV1N_EN.port, GPIO_Defs.MEAS_O2_SV1N_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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




            this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
            this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV1N_EN.port, GPIO_Defs.MEAS_O2_SV1N_EN.pin);


            return success;
        }
        /******************************************************************************************************************************
         *  test_sv10_off
         *  
         *  Function: Tests the sv10 solenoid driver circuit.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the voltage measurement is within the defined limits.
         *                          returns false if voltage measurement is outside the defined limits or the DMM is not connected.
         ******************************************************************************************************************************/
        private bool test_sv10_off(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;

           
            try
            {
                this.Vent.CMD_Write("mfgmode");
                
                this.Vent.CMD_Write("set vcm sv 10 0");
                
            }
            catch
            {

            }

            
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
            this.GPIO.SetBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
            this.GPIO.SetBit(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
            this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
            this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.pin);




            return success;
        }
        /******************************************************************************************************************************
         *  test_sv9_on
         *  
         *  Function: Tests the +5V3 SMPS power supply.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if 5V3 SMPS is within the upper and lower limits.
         *                          returns false if 5V3 SMPS is outside the defined limits or the DMM is not connected.
         ******************************************************************************************************************************/
        private bool test_sv9_on(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;

            try
            {
                this.Vent.CMD_Write("mfgmode");
                
                this.Vent.CMD_Write("set vcm sv 9 1");
                
            }
            catch
            {

            }

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
            this.GPIO.SetBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
            this.GPIO.SetBit(GPIO_Defs.MEAS_O2_SV1N_EN.port, GPIO_Defs.MEAS_O2_SV1N_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
            try
            {
                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    message.Report("SV9 Test PASS");
                    success = true;
                }
                else
                {
                    message.Report("SV9 Test FAIL");
                    success = false;
                }
            }
            catch
            {
                success = false;
            }
                
            try
            {
                this.Vent.CMD_Write("set vcm sv 9 0");
                
            }
            catch
            {

            }
            this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
            this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV1N_EN.port, GPIO_Defs.MEAS_O2_SV1N_EN.pin);

            
            

            return success;
        }
        /******************************************************************************************************************************
         *  test_sv10_on
         *  
         *  Function: Tests the +5V3 SMPS power supply.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success
         ******************************************************************************************************************************/
        private bool test_sv10_on(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;

            try
            {
                this.Vent.CMD_Write("mfgmode");
                
                this.Vent.CMD_Write("set vcm sv 10 1");
                
            }
            catch
            {

            }

            
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
            this.GPIO.SetBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
            this.GPIO.SetBit(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
                
            try
            {
                    
                this.Vent.CMD_Write("set vcm sv 10 0");
                
            }
            catch
            {

            }
            this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
            this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.pin);


            return success;
        }

        /******************************************************************************************************************************
         *  test_cough
         *  
         *  Function: Actuates the cough valve a set number of times and prompts the user to respond 
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success
         * 
         ******************************************************************************************************************************/
        private bool test_cough_valve(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;

            int num;

            test.parameters.TryGetValue("toggle", out var toggle);
            num = int.Parse(toggle);
            for (int i = 0; i < num*2; i++)
            {
                this.Vent.CMD_Write("set vcm coughv " + (i%2).ToString() );
                Thread.Sleep(50); //Value doesn't really matter, the device doesn't drive the valve any faster
                
            }

            if (this.PromptUser_YesNo("Does the cough valve actuate?", test.name))
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
         *  test_low_fan_volt
         *  
         *  Function: Measures the fan driver voltage for low speed and reports the value.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success
         * 
         ******************************************************************************************************************************/
        private bool test_low_fan_volt(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;

            
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
            this.GPIO.SetBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            this.GPIO.ClearBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
           

            return success;
        }
        /******************************************************************************************************************************
         *  test_low_fan_freq
         *  
         *  Function: Measures the fan rpm frequency at low speed.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the fan frequency is within the specified range
         *                          returns false if the frequency is outside the specified range
         * 
         ******************************************************************************************************************************/
        private bool test_low_fan_freq(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;

            
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
            this.GPIO.SetBit(GPIO_Defs.FAN_FREQ_MEAS_EN.port, GPIO_Defs.FAN_FREQ_MEAS_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Freq();
            this.GPIO.ClearBit(GPIO_Defs.FAN_FREQ_MEAS_EN.port, GPIO_Defs.FAN_FREQ_MEAS_EN.pin);
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
            try
            {
                message.Report("Measured: " + measured.ToString() + " Hz\n");

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
            catch(Exception e)
            {
                message.Report("Error");
                message.Report(e.Message);
                message.Report(e.StackTrace);
                success = false;
            }
            

            return success;
        }
        /******************************************************************************************************************************
         *  test_high_fan_volt
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
        private bool test_high_fan_volt(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;

            //Determine if the nebulizer needs to be running.
            string returnVal = this.Vent.CMD_Write("get vcm monitors");
            if(returnVal.Contains("nebulizerActive: 0"))
            {
               //Prompt user to begin nebulizer therapy.
                message.Report("Please start Nebulizer therapy by pressing \"Start\"");
                this.Vent.CMD_Write("set uim screen 5039");  //Nebulizer start screenID = 5039
                this.Vent.CMD_Write("restart");

                int i = 0;
                do
                {
                    Thread.Sleep(1000);
                    i++;
                    returnVal = this.Vent.CMD_Write("get vcm monitors");
                }
                while (returnVal.Contains("nebulizerActive: 0") && (i < 15));
                if (i >= 15)
                {
                    message.Report("Test timed out");
                    return false;
                }
            }

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
            this.GPIO.SetBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            this.GPIO.ClearBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
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
            catch (Exception e)
            {
                message.Report("Error");
                message.Report(e.Message);
                message.Report(e.StackTrace);
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
        private bool test_high_fan_freq(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            string str_value;
            bool value_available;
            bool success = false;
            errorCode = -1;
            float upper = 0;
            float lower = 0;

            //Determine if the nebulizer needs to be running.
            string returnVal = this.Vent.CMD_Write("get vcm monitors");
            if (returnVal.Contains("nebulizerActive: 0"))
            {
                //Prompt user to begin nebulizer therapy.
                message.Report("Please start Nebulizer therapy by pressing \"Start\"");
                this.Vent.CMD_Write("set uim screen 5039");  //Nebulizer start screenID = 5039
                this.Vent.CMD_Write("restart");

                int i = 0;
                do
                {
                    Thread.Sleep(1000);
                    i++;
                    returnVal = this.Vent.CMD_Write("get vcm monitors");
                    this.Vent.CMD_Write("set uim screen 5039");  //Nebulizer start screenID = 5039
                }
                while (returnVal.Contains("nebulizerActive: 0") && (i < 15));
                if (i >= 15)
                {
                    message.Report("Test timed out");
                    return false;
                }

                this.Vent.CMD_Write("mfgmode");
            }

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
            this.GPIO.SetBit(GPIO_Defs.FAN_FREQ_MEAS_EN.port, GPIO_Defs.FAN_FREQ_MEAS_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Freq();
            this.GPIO.ClearBit(GPIO_Defs.FAN_FREQ_MEAS_EN.port, GPIO_Defs.FAN_FREQ_MEAS_EN.pin);
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }
            try
            {
                message.Report("Measured: " + measured.ToString() + " Hz\n");

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
            catch (Exception e)
            {
                message.Report("Error");
                message.Report(e.Message);
                message.Report(e.StackTrace);
                success = false;
            }
            
            

            return success;
        }
        /******************************************************************************************************************************
         *  test_front_panel_buttons
         *  
         *  Function: Grab button data from the UUT, 
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *                 - 
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         *                          
         *                          
         *                          |MeasData | AS  | PB  | 
         *                          ----------------------
         *                          |   11    | ON  | ON  |
         *                          |   10    | ON  | OFF |
         *                          |   01    | OFF | ON  |
         *                          |   00    | OFF | OF  |
         *                                
         * 
         ******************************************************************************************************************************/
        private bool test_buttons(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            string[] output;
            int initialState=0;
            string port_str;
            DigitalPortType port;
            string pin_str;
            DigitalPortType pin;

            var available = test.parameters.TryGetValue("port", out port_str); 
            available = test.parameters.TryGetValue("pin", out pin_str);

            //Get Initial State
            output = this.Vent.CMD_Write("get vcm buttons").Split('\n');

            for(int i = 1; i < 3; i++)
            {
                string[] arr;
                arr = output[i].Split(',');
                initialState |= int.Parse(arr[3]) << (i-1); //Only care about whether the button is pressed, index 3
            }

            if (initialState != 0)
            {
                success = false;
                message.Report("Buttons are not in the correct position\n\rTest failed.");
            }
            else
            {
                
            }

            

            

            //Build binary num representing measured data.


            return success;
        }
        /******************************************************************************************************************************
         *  test_ambient_pressure
         *  
         *  Function: Queries the UUT for the ambient pressure sensor data
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true barometer responds with pressure within the range 
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_barometer(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            string val;
            int upper;
            int lower;

            if (this.Vent.Connected)
            {
                //Get limits
                test.parameters.TryGetValue("upper", out val);
                upper = int.Parse(val);
                test.parameters.TryGetValue("lower", out val);
                lower = int.Parse(val);


                //Set telemetry channels
                int channelNum = 85; //TODO: Add ability to search for channel number using channels names from Vent object

                this.Vent.CMD_Write("set vcm telemetry " + channelNum + " 0 0 0");

                var output = this.Vent.CMD_Write("get vcm telemetry");

                string press = output.Substring(53, 8);
                var pressure = float.Parse(press) * 0.000980665;
                
                if (!test.parameters.TryGetValue("measured", out val))
                {
                    test.parameters["measured"] = pressure.ToString();
                }
                else
                {
                    test.parameters.Add("measured", pressure.ToString());
                }

                if ((pressure < upper) && (pressure > lower))
                {
                    success = true;

                }
                else
                {
                    success = false;
                }

                message.Report("Barometer test: " + success.ToString());
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
        private bool test_ambient_temperature(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1; //Unused, should remove
            string val;
            int upper;
            int lower;
            

            if (this.Vent.Connected)
            {
                //Get limits
                test.parameters.TryGetValue("upper", out val);
                upper = int.Parse(val);
                test.parameters.TryGetValue("lower", out val);
                lower = int.Parse(val);


                //Set telemetry channels
                int channelNum = 86; //TODO: Add ability to search for channel number using channels names from Vent object

                this.Vent.CMD_Write("set vcm telemetry " + channelNum + " 0 0 0");

                var output = this.Vent.CMD_Write("get vcm telemetry");
                var temp = int.Parse(output.Substring(53, 8)) / 100;
                
                if (!test.parameters.TryGetValue("measured", out val))
                {
                    test.parameters["measured"] = temp.ToString();
                }
                else
                {
                    test.parameters.Add("measured", temp.ToString());
                }

                if ((temp < upper) && (temp > lower))
                {
                    success = true;
                }
                else
                {
                    success = false;
                }

                message.Report("Barometer test: " + success.ToString());
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
        private bool test_microphone(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;


            /*TODO: 
             * - Turn on speaker. Sound an alarm.
             * - Move USB drive to UUT, copy OUT1.wav and OUT2.wav to USB drive.
             * - Move USB drive to PC.
             * - Open OUT1.wav or OUT2.wav and perform an amplitude check on the file. Confirm
             */

            return success;
        }
        /******************************************************************************************************************************
         *  test_speaker
         *  
         *  Function: Connects the speaker to the UUT.  
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_speaker(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;

            //Enable the speaker
            this.GPIO.SetBit(GPIO_Defs.SPKR_EN.port, GPIO_Defs.SPKR_EN.pin);

            //Restart the VCM app.
            this.Vent.CMD_Write("restart");

            //Prompt user for feedback on speaker sound.
            success = this.PromptUser_YesNo("Wait until the device alarms\nDoes the speaker sound?", test.name);

            //Put device back into MFGmode
            this.Vent.CMD_Write("mfgmode");

            this.GPIO.ClearBit(GPIO_Defs.SPKR_EN.port, GPIO_Defs.SPKR_EN.pin);

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
        private bool test_piezo(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;

            if (this.GPIO.Connected)
            {
                //Connect piezo alarm
                this.GPIO.SetBit(GPIO_Defs.PIEZO_EN.port, GPIO_Defs.PIEZO_EN.pin);

                //Prompt user to hear piezo.

                if(this.PromptUser_YesNo("Does the piezo alarm?", test.name))
                {
                    success = true;
                    test.result = "PASS";
                }
                else
                {
                    success = false;
                    test.result = "FAIL";
                }


                this.GPIO.ClearBit(GPIO_Defs.PIEZO_EN.port, GPIO_Defs.PIEZO_EN.pin);
            }

            return success;
        }

        
        
        /******************************************************************************************************************************
         *  test_xflow_sv1
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_xflow_sv1 (IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            bool available;
            int toggle;
            string str_toggle;
            int measured = 0;

            available = test.parameters.TryGetValue("toggle", out str_toggle);
            toggle = int.Parse(str_toggle, System.Globalization.NumberStyles.Integer);

            int bit;
            bit = this.GPIO.GetBit(GPIO_Defs.XFLOW_SV1_2.port, GPIO_Defs.XFLOW_SV1_2.pin);
            if (bit == 0)
            {
                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 1 1");
                    Thread.Sleep(100);
                    //Query GPIO to read digital input
                    bit = this.GPIO.GetBit(GPIO_Defs.XFLOW_SV1_2.port, GPIO_Defs.XFLOW_SV1_2.pin);
                    if (bit == 1)
                    {
                        this.Vent.CMD_Write("set vcm sv 1 0");
                        Thread.Sleep(100);
                        bit = this.GPIO.GetBit(GPIO_Defs.XFLOW_SV1_2.port, GPIO_Defs.XFLOW_SV1_2.pin);
                        if (bit == 0)
                        {
                            measured++;
                        }
                    }
                    else
                    {

                        this.Vent.CMD_Write("set vcm sv 1 0");
                        break;
                    }

                }
            }
            if (measured == toggle) success = true;
            string val;
            if (!test.parameters.TryGetValue("measured", out val))
            {
                test.parameters["measured"] = measured.ToString();
            }
            else
            {
                test.parameters.Add("measured", measured.ToString());
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_xflow_sv2
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_xflow_sv2(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            bool available;
            int toggle;
            string str_toggle;
            try
            {
                available = test.parameters.TryGetValue("toggle", out str_toggle);
                toggle = int.Parse(str_toggle, System.Globalization.NumberStyles.Integer);


                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 2 1");
                    Thread.Sleep(100);
                    this.Vent.CMD_Write("set vcm sv 2 0");
                    Thread.Sleep(100);
                }
                int toggled = toggle;
                string val;
                if (!test.parameters.TryGetValue("measured", out val))
                {
                    test.parameters["measured"] = toggled.ToString();
                }
                else
                {
                    test.parameters.Add("measured", toggled.ToString());
                }

            }
            catch
            {

            }


            return success;
        }
        /******************************************************************************************************************************
         *  test_xflow_sv2
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_xflow_sv3(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            bool available;
            int toggle;
            string str_toggle;
            try
            {
                available = test.parameters.TryGetValue("toggle", out str_toggle);
                toggle = int.Parse(str_toggle, System.Globalization.NumberStyles.Integer);


                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 3 1");
                    Thread.Sleep(100);
                    this.Vent.CMD_Write("set vcm sv 3 0");
                    Thread.Sleep(100);
                }
                int toggled = toggle;
                test.parameters.Add("measured", toggled.ToString());

            }
            catch
            {

            }


            return success;
        }
        /******************************************************************************************************************************
         *  test_xflow_sv2
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_xflow_sv4(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            bool available;
            int toggle;
            string str_toggle;
            try
            {
                available = test.parameters.TryGetValue("toggle", out str_toggle);
                toggle = int.Parse(str_toggle, System.Globalization.NumberStyles.Integer);


                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 4 1");
                    Thread.Sleep(100);
                    this.Vent.CMD_Write("set vcm sv 4 0");
                    Thread.Sleep(100);
                }
                int toggled = toggle;
                test.parameters.Add("measured", toggled.ToString());

            }
            catch
            {

            }


            return success;
        }
        /******************************************************************************************************************************
         *  test_xflow_sv2
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_exhl_sv6(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            bool available;
            int toggle;
            string str_toggle;
            try
            {
                available = test.parameters.TryGetValue("toggle", out str_toggle);
                toggle = int.Parse(str_toggle, System.Globalization.NumberStyles.Integer);


                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 6 1");
                    Thread.Sleep(100);
                    this.Vent.CMD_Write("set vcm sv 6 0");
                    Thread.Sleep(100);
                }
                int toggled = toggle;
                test.parameters.Add("measured", toggled.ToString());

            }
            catch
            {

            }


            return success;
        }
        /******************************************************************************************************************************
         *  test_xflow_sv2
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_exhl_sv7(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            bool available;
            int toggle;
            string str_toggle;
            try
            {
                available = test.parameters.TryGetValue("toggle", out str_toggle);
                toggle = int.Parse(str_toggle, System.Globalization.NumberStyles.Integer);


                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 7 1");
                    Thread.Sleep(100);
                    this.Vent.CMD_Write("set vcm sv 7 0");
                    Thread.Sleep(100);
                }
                int toggled = toggle;
                test.parameters.Add("measured", toggled.ToString());

            }
            catch
            {

            }


            return success;
        }
        /******************************************************************************************************************************
         *  test_xflow_sv2
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_exhl_sv8(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            bool available;
            int toggle;
            string str_toggle;
            try
            {
                available = test.parameters.TryGetValue("toggle", out str_toggle);
                toggle = int.Parse(str_toggle, System.Globalization.NumberStyles.Integer);


                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 8 1");
                    Thread.Sleep(100);
                    this.Vent.CMD_Write("set vcm sv 8 0");
                    Thread.Sleep(100);
                }
                int toggled = toggle;
                test.parameters.Add("measured", toggled.ToString());

            }
            catch
            {

            }

            //TODO: Add command to read from board that is measuring the toggles.


            return success;
        }
        /******************************************************************************************************************************
         *  test_xflow_sv2
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times, reads the signal at the GPIO module
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_flow_sv9(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            bool available;
            int toggle;
            string str_toggle;
            try
            {
                available = test.parameters.TryGetValue("toggle", out str_toggle);
                toggle = int.Parse(str_toggle, System.Globalization.NumberStyles.Integer);


                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 9 1");
                    Thread.Sleep(100);
                    this.Vent.CMD_Write("set vcm sv 9 0");
                    Thread.Sleep(100);
                }
                int toggled = toggle;
                test.parameters.Add("measured", toggled.ToString());

            }
            catch
            {

            }


            return success;
        }
        /******************************************************************************************************************************
         *  test_as_led
         *  
         *  Function: Turns on the alarm silence
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             
         *             parameters: fs - sample rate in hz
         *                         time - length of time needed to sample in seconds

         *  Returns: bool success - returns true if alarm silence led lights up
         *                          returns false if alarm silence led does not light up
         *  Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *                  - Parameters does not contain Key
         *                  
         * 
         ******************************************************************************************************************************/
        private bool test_as_led(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;


            if(this.powered && this.GPIO.Connected)
            {

                //Measure CN309m.21 --> Alarm Silence LED Cathode --> Should be HIGH
                var startVal = this.GPIO.GetBit(GPIO_Defs.MEAS_AS_LED.port, GPIO_Defs.MEAS_AS_LED.pin);

                if (startVal == 1)
                {   
                    //LED is currently off --> We may continue with test

                    //Toggle Alarm Silence Button
                    this.GPIO.SetBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);
                    Thread.Sleep(500);
                    this.GPIO.ClearBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);

                    //Alarm Silence LED toggles at 2Hz --> Will sample to get a reasonable capture of the value
                    var fs = int.Parse(test.parameters["fs"]);
                    var time = int.Parse(test.parameters["time"]);


                    int[] ledFlash = new int[fs * time]; // List to hold all of
                    for(int i = 0; i < (fs*time); i++)
                    {
                        ledFlash[i] = this.GPIO.GetBit(GPIO_Defs.MEAS_AS_LED.port, GPIO_Defs.MEAS_AS_LED.pin);
                        if (ledFlash[i] == 1) message.Report("LED is OFF");
                        else if (ledFlash[i] == 0) message.Report("LED is ON");
                        else message.Report("Something is wrong with GPIO.GetBit()");
                        
                        Thread.Sleep(1000 / fs);
                    }

                    if (ledFlash.Contains(0))
                    {
                        //Get average
                        var average = ledFlash.Average();
                        //if (average >= lower && average <= upper) ;
                        message.Report("Average = " + average);
                        success = true;
                        
                    }
                    else
                    {
                        success = false;
                    }
                }
                else
                {
                    success = false;
                }
                //Fill in measurement parameter
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = "FAIL";
                }
            }
            else
            {
                if (!this.powered)
                {
                    message.Report("UUT is not powered");
                    test.parameters["measured"] = "Device not powered";
                }
                
                if (!this.GPIO.Connected)
                {
                    message.Report("GPIO Module is not connected");
                    test.parameters["measured"] = "GPIO module disconnected";
                }
            }

            
            return success;
        }

        /******************************************************************************************************************************
         *  test_batt_comms
         *  
         *  Function: Queries the device for the current battery communications lines
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             
         *             

         *  Returns: bool success - returns true if alarm silence led lights up
         *                          returns false if alarm silence led does not light up
         *  Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *                  - Parameters does not contain Key
         *                  
         * 
         ******************************************************************************************************************************/
        private bool test_batt_comms(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            int measured = 0;
            int retry = 0;
            string powerOutput = "";

            if (this.powered && this.Vent.Connected && this.GPIO.Connected)
            {
                //Command UUT with "get vcm power"
                do
                {
                    
                    powerOutput = this.Vent.CMD_Write("get vcm power").Replace("\r\n", "\r");
                    retry++;
                } while ((powerOutput == "") || (retry > 3));



                if (!(retry > 3)) // Tried too many times
                {
                    var powerArray = powerOutput.Split('\r');


                    var BAT0_Data = powerArray[8].Trim().Substring(7).Split(',');
                    var BAT1_Data = powerArray[6].Trim().Substring(8).Split(',');
                    var BAT2_Data = powerArray[7].Trim().Substring(8).Split(',');

                    //Get the ASOC and RSOC Values from the power array
                    if (int.Parse(BAT0_Data[3]) != 0)
                    {
                        measured = measured | (1 << 0);
                        if (int.Parse(BAT1_Data[3]) != 0)
                        {
                            measured = measured | (1 << 1);
                            if (int.Parse(BAT2_Data[3]) != 0)
                            {
                                measured = measured | (1 << 2);
                                success = true;
                            }

                        }
                    }

                }
                else
                {
                    measured = -1; //Timeout / tried to talk over telnet too many times
                }

                //Fill in measurement parameter
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = measured.ToString();
                }
            }
            else
            {
                if (!this.powered)
                {
                    message.Report("UUT is not powered");
                }
                if (!this.Vent.Connected)
                {
                    message.Report("UUT is not connected to Telnet");
                }
                if (!this.GPIO.Connected)
                {
                    message.Report("GPIO Module is not connected");
                }
            }


            
            return success;
        }

        /******************************************************************************************************************************
         *  test_batt_comms
         *  
         *  Function: Queries the device for the current battery communications lines
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             
         *             

         *  Returns: bool success - returns true if alarm silence led lights up
         *                          returns false if alarm silence led does not light up
         *  Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *                  - Parameters does not contain Key
         *                  
         * 
         ******************************************************************************************************************************/
        private bool test_batt_temp(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            int measured = 0;
            int retry = 0;
            string powerOutput = "";


            int kIndex = int.Parse(test.parameters["index"]);
            int upper = int.Parse(test.parameters["upper"]);
            int lower = int.Parse(test.parameters["lower"]);

            if (this.powered && this.Vent.Connected && this.GPIO.Connected)
            {
                //Command UUT with "get vcm power"
                do
                {

                    powerOutput = this.Vent.CMD_Write("get vcm power").Replace("\r\n", "\r");
                    retry++;
                } while ((powerOutput == "") || (retry > 3));



                if (!(retry > 3)) // Tried too many times
                {
                    var powerArray = powerOutput.Split('\r');


                    var BAT0_Data = powerArray[8].Trim().Substring(7).Split(',');
                    var BAT1_Data = powerArray[6].Trim().Substring(8).Split(',');
                    var BAT2_Data = powerArray[7].Trim().Substring(8).Split(',');

                    //Get the ASOC or RSOC Values from the power array
                    if ((int.Parse(BAT0_Data[kIndex]) > lower) && (int.Parse(BAT0_Data[kIndex]) < upper))
                    {
                        measured = measured | (1 << 0);
                        if ((int.Parse(BAT1_Data[kIndex]) > lower) && (int.Parse(BAT0_Data[kIndex]) < upper))
                        {
                            measured = measured | (1 << 1);
                            if ((int.Parse(BAT2_Data[kIndex]) > lower) && (int.Parse(BAT0_Data[kIndex]) < upper))
                            {
                                measured = measured | (1 << 2);
                                success = true;
                            }

                        }
                    }

                }
                else
                {
                    measured = -1; //Timeout we tried to talk over telnet too many times
                }

                //Fill in measurement parameter
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = measured.ToString();
                }
            }
            else
            {
                if (!this.powered)
                {
                    message.Report("UUT is not powered");
                }
                if (!this.Vent.Connected)
                {
                    message.Report("UUT is not connected to Telnet");
                }
                if (!this.GPIO.Connected)
                {
                    message.Report("GPIO Module is not connected");
                }
            }



            return success;
        }

        /******************************************************************************************************************************
         *  test_batt0_chg
         *  
         *  Function: Connects a power supply and a
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             parameters: - resistance - Denotes the sense resistor used to calucalate current from volts
         *                         - voltage - Denotes the voltag to set the power supply at
         *                         - current  Denotes the current limit to set the power supply at
         *                         - upper - Upper limit for passing result
         *                         - lower - Lower limit for passing result
         *                         - chg_delay - Delay in milliseconds to wait between connecting load and measuring current through the load
         *             

         *  Returns: bool success - returns true if alarm silence led lights up
         *                          returns false if alarm silence led does not light up
         *  Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *                  - Parameters does not contain Key
         *                  
         * 
         ******************************************************************************************************************************/

            //TODO: Test this before copying to test_batt1_charge and test_batt2_charge
        private bool test_batt0_charge(IProgress<string> message, IProgress<string> log, TestData test, out int errorCode)
        {
            bool success = false;
            errorCode = -1;
            float measured = 0;



            float resistance = float.Parse(test.parameters["resistance"]);
            double voltage = double.Parse(test.parameters["voltage"]);
            double current = double.Parse(test.parameters["current"]);
            int upper = int.Parse(test.parameters["upper"]);
            int lower = int.Parse(test.parameters["lower"]);
            int chgDelay = int.Parse(test.parameters["chg_delay"]);

            if (this.powered && this.Vent.Connected && this.GPIO.Connected && this.DMM.Connected && this.PPS.Connected)
            {
                //Command GPIO to connect PPS to CN302
                this.GPIO.SetBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);

                //Command GPIO to connect DMM to measure voltage across sense resistor
                this.GPIO.SetBit(GPIO_Defs.MEAS_BATT_CHG_EN.port, GPIO_Defs.MEAS_BATT_CHG_EN.pin);

                //Command PPS to supply target voltage
                this.PPS.SetPPSOutput(voltage, current);

                //Connect Load
                this.GPIO.SetBit(GPIO_Defs.PPS_LOAD_EN.port, GPIO_Defs.PPS_LOAD_EN.pin);



                //Wait some time until device begins to charge
                Thread.Sleep(chgDelay);

                measured = this.DMM.Get_Volts() / resistance;
                

                //Disconenct the power supply, load and DMM
                
                this.GPIO.ClearBit(GPIO_Defs.PPS_LOAD_EN.port, GPIO_Defs.PPS_LOAD_EN.pin);
                this.PPS.SetPPSOff();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_BATT_CHG_EN.port, GPIO_Defs.MEAS_BATT_CHG_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);

                if ((measured > lower) && (measured < upper))
                {
                    success = true;
                }
                    

                //Fill in measurement parameter
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = measured.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = measured.ToString();
                }
            }
            else
            {
                if (!this.powered)
                {
                    message.Report("UUT is not powered");
                }
                if (!this.Vent.Connected)
                {
                    message.Report("UUT is not connected to Telnet");
                }
                if (!this.GPIO.Connected)
                {
                    message.Report("GPIO Module is not connected");
                }
                if (!this.PPS.Connected)
                {
                    message.Report("Power supply is not connected");
                }
                if (!this.DMM.Connected)
                {
                    message.Report("Multimeter is not connected");
                }
            }



            return success;
        }

    }
}