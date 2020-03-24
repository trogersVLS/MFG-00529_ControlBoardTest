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
using System.Reflection;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using MccDaq;
using GPIO;
using System.Text.RegularExpressions;

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
        //private bool Verify_CPLD(IProgress<string> message, IProgress<string> log, TestData test)
        //{
        //    string VerifyScriptPath;
        //    string ResultFilePath;
        //    string Verify_CMD;
        //    string Verify_Success = "Executing action VERIFY PASSED";
        //    bool success;
            
        //    //TODO: Confirm that this is still true when a setup project is used to create the installer.
        //    //The path to the CPLD_Program script is always two directories up from the executing path.
        //    VerifyScriptPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //    VerifyScriptPath = VerifyScriptPath.Remove(VerifyScriptPath.LastIndexOf("\\")); //Up one directory
        //    VerifyScriptPath = VerifyScriptPath.Remove(VerifyScriptPath.LastIndexOf("\\")); // Up two directories
        //    ResultFilePath = VerifyScriptPath + "\\ProgramLoad\\CPLDLoad\\Results\\VerifyResult.txt";
        //    VerifyScriptPath = VerifyScriptPath + "\\ProgramLoad\\CPLDLoad\\cpld_verify.tcl";

        //    Verify_CMD = "script:" + VerifyScriptPath + " logfile:" + ResultFilePath;

        //    System.Diagnostics.Process cpld_cmd = new System.Diagnostics.Process();
        //    System.Diagnostics.ProcessStartInfo cpld_info = new System.Diagnostics.ProcessStartInfo();
        //    cpld_info.FileName = "C:\\Microsemi\\Program_Debug_v11.9\\bin\\flashpro.exe";
        //    cpld_info.Arguments = Verify_CMD;
        //    cpld_info.RedirectStandardOutput = true;
        //    cpld_info.UseShellExecute = false;
        //    cpld_cmd.StartInfo = cpld_info;
        //    cpld_cmd.Start();
        //    //string output = cpld_cmd.StandardOutput.ReadToEnd();
        //    message.Report("Starting programmer ...");
        //    while (!File.Exists(ResultFilePath))
        //    {
        //        Thread.Sleep(2000);
        //        message.Report("...");
        //    }
        //    if (CPLD_LogRead(ResultFilePath, Verify_Success))
        //    {
        //        message.Report("CPLD Verify Successful");
        //        success = true;
        //    }
        //    else
        //    {
        //        message.Report("CPLD Verify unsuccessful");
        //        success = false;
        //    }


        //    return success;
        //}

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
        //private bool Program_CPLD(IProgress<string> message, IProgress<string> log, TestData test)
        //{
        //    string ProgramScriptPath;
        //    string ResultFilePath;
        //    string Program_CMD;
        //    string Program_Success = "Executing action PROGRAM PASSED";
        //    bool success;
            



        //    //TODO: Confirm that this is still true when a setup project is used to create the installer.
        //    //The path to the CPLD_Program script is always two directories up from the executing path.
        //    ProgramScriptPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //    ProgramScriptPath = ProgramScriptPath.Remove(ProgramScriptPath.LastIndexOf("\\")); //Up one directory
        //    ProgramScriptPath = ProgramScriptPath.Remove(ProgramScriptPath.LastIndexOf("\\")); // Up two directories
        //    ResultFilePath = ProgramScriptPath + "\\ProgramLoad\\CPLDLoad\\Results\\ProgramResult.txt";
        //    ProgramScriptPath = ProgramScriptPath + "\\ProgramLoad\\CPLDLoad\\cpld_program.tcl";

        //    Program_CMD = "script:" + ProgramScriptPath + " logfile:" + ResultFilePath;

        //    System.Diagnostics.Process cpld_cmd = new System.Diagnostics.Process();
        //    System.Diagnostics.ProcessStartInfo cpld_info = new System.Diagnostics.ProcessStartInfo();
        //    cpld_info.FileName = "C:\\Microsemi\\Program_Debug_v11.9\\bin\\flashpro.exe";
        //    cpld_info.Arguments = Program_CMD;
        //    cpld_info.RedirectStandardOutput = true;
        //    cpld_info.UseShellExecute = false;
        //    cpld_cmd.StartInfo = cpld_info;
        //    cpld_cmd.Start();
        //    //string output = cpld_cmd.StandardOutput.ReadToEnd();
        //    message.Report("Starting programmer ...");
        //    while (!File.Exists(ResultFilePath))
        //    {
        //        Thread.Sleep(2000);
        //        message.Report("...");
        //    }
        //    if (CPLD_LogRead(ResultFilePath, Program_Success))
        //    {
        //        message.Report("CPLD Program Successful");
        //        success = true;
        //    }
        //    else
        //    {
        //        message.Report("CPLD Program unsuccessful");
        //        success = false;
        //    }


        //    return success;
        //}
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
        //private bool CPLD_LogRead(string path, string pass)
        //{
        //    bool success;
        //    string file;
            
        //    file = File.ReadAllText(path);

        //    if (file.Contains(pass))
        //    {
        //        success = true;
        //    }
        //    else
        //    {
        //        success = false;
        //    }

        //    File.Delete(path);
        //    return success;
        //}
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
        //private bool Program_Hercules(IProgress<string> message, IProgress<string> log, TestData test)
        //{
        //    string HerculesScriptPath;
        //    string Hercules_CMD;
        //    string cmd_output;

        //    bool success;
            
        //    //TODO: Confirm that this is still true when a setup project is used to create the installer.
        //    //The path to the Herc_Program script is always two directories up from the executing path.
        //    HerculesScriptPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //    HerculesScriptPath = HerculesScriptPath.Remove(HerculesScriptPath.LastIndexOf("\\")); //Up one directory
        //    HerculesScriptPath = HerculesScriptPath.Remove(HerculesScriptPath.LastIndexOf("\\")); // Up two directories
        //    HerculesScriptPath = HerculesScriptPath + "\\ProgramLoad\\HerculesLoad\\dslite.bat";

        //    Hercules_CMD = "/c " + HerculesScriptPath;

        //    //Sets up the shell that will be used to execute the hercules programming script
        //    System.Diagnostics.Process cmd = new System.Diagnostics.Process();
        //    System.Diagnostics.ProcessStartInfo cmd_info = new System.Diagnostics.ProcessStartInfo("cmd");
        //    cmd_info.FileName = "cmd.exe";
        //    cmd_info.Arguments = Hercules_CMD;
        //    cmd_info.CreateNoWindow = true;
        //    cmd_info.RedirectStandardOutput = true;
        //    cmd_info.RedirectStandardError = true;
        //    cmd_info.UseShellExecute = false;
        //    cmd_info.WorkingDirectory = HerculesScriptPath.Remove(HerculesScriptPath.LastIndexOf("\\")); // TODO: sets the current directory to the directory that the hercules program script is located by removing the file name at the end of the string
        //    cmd.StartInfo = cmd_info;
        //    message.Report("Starting Hercules programmer ...");


        //    cmd.Start(); //Executes the script and pauses until the script has finished executing

        //    cmd_output = cmd.StandardOutput.ReadToEnd();
        //    message.Report("Programmer exit");

        //    //Confirms that the script was successful by comparing the 
        //    if (cmd_output.Contains("Program verification successful")) // Contains may be somewhat slow for this use case TODO: Determine if there is a better way?
        //    {
        //        success = true;
        //    }
        //    else
        //    {
        //        success = false;
        //    }
        //    return success;
        //}

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
        //private bool Program_SOM(IProgress<string> message, IProgress<string> log, TestData test)
        //{
        //    bool success = false;
            
        //    bool Booted = false;
        //    bool Formatted = false;
        //    bool IPL_Installed = false;
        //    string output;

        //    if (this.SOM.Connected)
        //    {
        //        //Cycle power
        //        message.Report("Cycling Power ...\n");
        //        this.GPIO.SetBit(GPIO_Defs.AC_EN.port, 0);
        //        Thread.Sleep(100);
        //        this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

        //        Booted = this.SOM.ReadUntil("U-Boot# ", out output, 10000);
        //        //Device needs to boot U-Boot
        //        if (!Booted)
        //        {
        //            message.Report("Device did not boot to U-Boot\nCycling power again ...\n");
        //            this.GPIO.SetBit(GPIO_Defs.AC_EN.port, 0x00);
        //            Thread.Sleep(100);
        //            this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

        //            Booted = this.SOM.ReadUntil("U-Boot# ", out output, 10000);

        //            if (!Booted)
        //            {
        //                message.Report("Device does not boot properly.\nPowering down ...");
        //                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, 0);
        //                success = false;
        //            }
        //        }
        //        else if(Booted)
        //        {
        //            message.Report("Successfully booted to U-Boot\nLoading QNX ...");
                    
        //            this.SOM.Command("mmc dev; fatload mmc 0 0x81000000 qnxifs; go 0x81000000",out output, "U-Boot# ", 5000);

        //            Booted = false;
        //            Booted = this.SOM.ReadUntil("Welcome to QNX on the Ventec 3000 platform", out output, 20000);

        //            if (!Booted)
        //            {
        //                message.Report("Device did not boot correctly");
        //            }
        //            else
        //            {
        //                message.Report("Formatting NAND");
        //                Formatted = this.SOM.Command("fs-etfs-jacinto5_micron -D gpmc=0x50000000, cache, ipl=4, ifs=1024 -r131072 -e -m /fs/etfs", 
        //                                          out output, "# ", 20000);
        //                if (Formatted)
        //                {
        //                    IPL_Installed = this.SOM.Command("update_nand -i -f /fs/sd0/ipl_nand.bin", out output, "# ", 10000);
        //                    if (IPL_Installed)
        //                    {
        //                        success = true;
        //                    }
        //                }


                         

        //            }

        //        }

        //    }
        //    else
        //    {
        //        success = false;
        //    }
        //    return success;
        //}

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
        //private bool test_mfg_install(IProgress<string> message, IProgress<string> log, TestData test)
        //{

        //    bool success = false;
        //    string output;
            


        //    //Check to see if the current version of software is not MFG01.04

        //    output = this.Vent.CMD_Write("get vcm version");
            


        //    success = CopySoftware_USB(message, "MFG");

        //    if (success)
        //    {   

        //        //Swap the USB drive to the UUT.
        //        message.Report("Turning on the device ...");
        //        this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

        //        success = this.SOM.ReadUntil("Waiting 3 seconds for /fs/usb", out output, 5000);

        //        if (success) {
        //            success = false;
        //            message.Report("Starting software update ...");

        //            this.GPIO.SetBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);

        //            success = this.SOM.ReadUntil("Update IFS", out output, 100000);
        //            if (success)
        //            {
        //                success = false;
        //                message.Report("Updating the NAND flash ...");

        //                success = this.SOM.ReadUntil("display_image", out output, 100000);
        //                if (success)
        //                {
        //                    success = false;
        //                    // Turn off device using the power button.
        //                    Thread.Sleep(500);
        //                    message.Report("Powering down device ...");
        //                    this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);
        //                    Thread.Sleep(500);
        //                    this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, 0x00);
        //                    this.SOM.Booted = false;

        //                    message.Report("\nSoftware update successful!");

                            
                           
        //                    success = true;
        //                }

        //            }
 
        //        }
                
        //    }
            
        //    return success;
        //}
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
        //private bool test_prd_install(IProgress<string> message, IProgress<string> log, TestData test)
        //{

        //    bool success = false;
        //    string output;
            

        //    success = CopySoftware_USB(message, "PRD");

        //    if (success)
        //    {

        //        //Swap the USB drive to the UUT.
        //        message.Report("Turning on the device ...");
        //        this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

        //        success = this.SOM.ReadUntil("Waiting 3 seconds for /fs/usb", out output, 5000);

        //        if (success)
        //        {
        //            success = false;
        //            message.Report("Starting software update ...");

        //            this.GPIO.SetBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);

        //            success = this.SOM.ReadUntil("Update IFS", out output, 100000);
        //            if (success)
        //            {
        //                success = false;
        //                message.Report("Updating the NAND flash ...");

        //                success = this.SOM.ReadUntil("display_image", out output, 100000);
        //                if (success)
        //                {
        //                    success = false;
        //                    // Turn off device using the power button.
        //                    Thread.Sleep(500);
        //                    message.Report("Powering down device ...");
        //                    this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);
        //                    Thread.Sleep(500);
        //                    this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, 0x00);
        //                    this.SOM.Booted = false;

        //                    message.Report("\nSoftware update successful!");



        //                    success = true;
        //                }

        //            }

        //        }

        //    }

        //    return success;
        //}
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
        //private bool CopySoftware_USB(IProgress<string> log, string sw_version = "MFG", int iteration = 0)
        //{
        //    bool success = false;
        //    string filepath;
        //    //Prepare USB Drive with the correct software, find the USB drive first
        //    DriveInfo[] alldrives = DriveInfo.GetDrives();

        //    if (sw_version == "MFG") filepath = ".\\ProgramLoad\\MFG_SW\\vswupdate";
        //    else if (sw_version == "PRD") filepath = ".\\ProgramLoad\\PRD_SW\\vswupdate"; //TODO: Update KVSTORE bit
        //    else return success;


        //    foreach (DriveInfo drive in alldrives)
        //    {
        //        //Special volume label for drive used as Software loader
        //        if (drive.IsReady)
        //        {
        //            if (drive.VolumeLabel == "MFG527")
        //            {
        //                message.Report("Preparing USB drive ...");
        //                //Delete the current software update files
        //                string targetPath = drive.Name;
        //                if (Directory.Exists(targetPath + "vswupdate"))
        //                {
        //                    message.Report("Deleting directory: " + targetPath + "\\vswupdate");
        //                    Directory.Delete(targetPath + "vswupdate", true);
        //                }
        //                message.Report("Copying software update files to " + targetPath);
        //                //Copy the update files for the intended software
        //                CopyDirectory(filepath, targetPath + "vswupdate");
        //                success = true;
        //            }
        //        }
        //    }
        //    if(!success && iteration < 4) //Couldn't find the USB drive.
        //    {
        //        message.Report("Could not find the drive");
        //        ushort initialVal = 1;//this.GPIO.getPort(GPIO_Defs.USB_TGL.port);
        //        this.GPIO.SetBit(GPIO_Defs.USB_TGL.port,GPIO_Defs.USB_TGL.pin);
        //        Thread.Sleep(100);
        //        this.GPIO.SetBit(GPIO_Defs.USB_TGL.port, initialVal);
        //        Thread.Sleep(1000);
                
                
        //        success = CopySoftware_USB(log, sw_version, ++iteration);
        //    }

        //    Thread.Sleep(500);
        //    return success;
        //}

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

        private bool test_touch_cal(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            string output;
            

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
        public bool test_power_on(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
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

                if (!success)
                {
                    //this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                }
            }
            else
            {
                if (!this.SOM.Connected)
                {
                    message.Report("SOM serial port is not connected");
                    
                }
                
                if (!this.GPIO.Connected)
                {
                    message.Report("GPIO device is not connected");
                   
                }
            }

            

           
            return success;
        }
        public bool test_power_on(IProgress<string> message, IProgress<string> log, int timeout)
        {
            bool success = false;

            //Determine if the devices are properly connected
            if (this.GPIO.Connected && this.SOM.Connected)
            {
                string output;
                

                //Enables the AC power supply
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                //Toggles the power button in case the device does not begin booting right away (device was successfully powered down by the power button prior to testing).
                this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);
                Thread.Sleep(500);
                this.GPIO.ClearBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);

                message.Report("Powering up ... ");

                if (this.SOM.ReadUntil("screen driver", out output, 20000))
                {
                    //May need to calibrate touchscreen, which requires user interaction
                    if (!this.SOM.ReadUntil("calib-touch", out output, 5000))
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

                if (!success)
                {
                    this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                }
            }
            else
            {
                if (!this.SOM.Connected)
                {
                    message.Report("SOM serial port is not connected");

                }

                if (!this.GPIO.Connected)
                {
                    message.Report("GPIO device is not connected");

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
        public bool test_power_down(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            if (this.GPIO.Connected)
            {
                //Disables the AC power supply
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                

               message.Report("Powering down");

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
        private bool test_lcd(IProgress<string> message, IProgress<string> log, TestData test)
        {
            //Assumptions - Unit has been powered on
            bool result;
            string measured;
            bool success = false;
            

            //Clear the queue


            //Blocking until user input is given --> Possible options are: "yes", or "no" 
            //message.Report("Is the LCD screen clear?");

            result = this.PromptUser_YesNo("Is the LCD screen clear?", test.name);

            if (result)
            {
                //message.Report("Test Passed!");
                measured = "pass";
                success = true;

            }
            else
            {

                measured = this.PromptUser("Describe the failure", test.name);

                //message.Report("Test Failed");
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
        private bool test_3V3_HOT(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            float upper = 0;
            float lower = 0;

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"], System.Globalization.NumberStyles.Float);
                lower = float.Parse(test.parameters["lower"], System.Globalization.NumberStyles.Float);


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_3V3_HOT_EN.port, GPIO_Defs.MEAS_3V3_HOT_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_3V3_HOT_EN.port, GPIO_Defs.MEAS_3V3_HOT_EN.pin);


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    
                    success = true;
                }
                else
                {
                    
                    success = false;
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
        private bool test_5V0_HOT(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            float upper = 0;
            float lower = 0;

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"], System.Globalization.NumberStyles.Float);
                lower = float.Parse(test.parameters["lower"], System.Globalization.NumberStyles.Float);


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_5V0_HOT_EN.port, GPIO_Defs.MEAS_5V0_HOT_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_5V0_HOT_EN.port, GPIO_Defs.MEAS_5V0_HOT_EN.pin);


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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
        private bool test_5V3_SMPS(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            float upper = 0;
            float lower = 0;

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"], System.Globalization.NumberStyles.Float);
                lower = float.Parse(test.parameters["lower"], System.Globalization.NumberStyles.Float);


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_5V3_EN.port, GPIO_Defs.MEAS_5V3_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_5V3_EN.port, GPIO_Defs.MEAS_5V3_EN.pin);


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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
        private bool test_12V0_SMPS(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            float upper = 0;
            float lower = 0;

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"], System.Globalization.NumberStyles.Float);
                lower = float.Parse(test.parameters["lower"], System.Globalization.NumberStyles.Float);


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_12V0_EN.port, GPIO_Defs.MEAS_12V0_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_12V0_EN.port, GPIO_Defs.MEAS_12V0_EN.pin);


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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
        private bool test_3V3_SMPS(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            float upper = 0;
            float lower = 0;

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"], System.Globalization.NumberStyles.Float);
                lower = float.Parse(test.parameters["lower"], System.Globalization.NumberStyles.Float);


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_3V3_EN.port, GPIO_Defs.MEAS_3V3_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_3V3_EN.port, GPIO_Defs.MEAS_3V3_EN.pin);


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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
        private bool test_1V2_SMPS(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            float upper = 0;
            float lower = 0;

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"], System.Globalization.NumberStyles.Float);
                lower = float.Parse(test.parameters["lower"], System.Globalization.NumberStyles.Float);


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_1V2_EN.port, GPIO_Defs.MEAS_1V2_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_1V2_EN.port, GPIO_Defs.MEAS_1V2_EN.pin);


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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
        private bool test_VREF(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            float upper = 0;
            float lower = 0;

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"], System.Globalization.NumberStyles.Float);
                lower = float.Parse(test.parameters["lower"], System.Globalization.NumberStyles.Float);


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_VREF_EN.port, GPIO_Defs.MEAS_VREF_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_VREF_EN.port, GPIO_Defs.MEAS_VREF_EN.pin);


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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
        private bool test_3V3_LDO(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            float upper = 0;
            float lower = 0;

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"], System.Globalization.NumberStyles.Float);
                lower = float.Parse(test.parameters["lower"], System.Globalization.NumberStyles.Float);


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_3V3A_EN.port, GPIO_Defs.MEAS_3V3A_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_3V3A_EN.port, GPIO_Defs.MEAS_3V3A_EN.pin);


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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
        private bool test_30V_SMPS(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            float upper = 0;
            float lower = 0;

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"], System.Globalization.NumberStyles.Float);
                lower = float.Parse(test.parameters["lower"], System.Globalization.NumberStyles.Float);


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_30V_EN.port, GPIO_Defs.MEAS_30V_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_30V_EN.port, GPIO_Defs.MEAS_30V_EN.pin);


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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
        private bool test_36V_SMPS(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            float upper = 0;
            float lower = 0;

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {
                
                upper = float.Parse(test.parameters["upper"], System.Globalization.NumberStyles.Float);
                lower = float.Parse(test.parameters["lower"], System.Globalization.NumberStyles.Float);
                

                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_36V_EN.port, GPIO_Defs.MEAS_36V_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_36V_EN.port, GPIO_Defs.MEAS_36V_EN.pin);


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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
        private bool test_blower(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            string ventOutput;
            float measured;

            //Note: TryParse not used here because in no situation should the user be able to edit the configuration file. Ergo, no need to catch the error unless working in a dev environment, which throwing an exception is more acceptable as it's useful for debugging what happened.
            float speed = int.Parse(test.parameters["speed"]);            
            float tolerance = int.Parse(test.parameters["tolerance"]);

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {
                message.Report("Setting blower speed  to: " + speed.ToString() + " RPM");

                ventOutput = this.Vent.CMD_Write("set vcm testmgr speed " + speed.ToString());

                Thread.Sleep(500);
                


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_FREQ_BLOWER.port, GPIO_Defs.MEAS_FREQ_BLOWER.pin);
                Thread.Sleep(1000);

                //Measure the voltage
                measured = this.DMM.Get_Freq() * 60; //Convert to RPM
                this.GPIO.ClearBit(GPIO_Defs.MEAS_FREQ_BLOWER.port, GPIO_Defs.MEAS_FREQ_BLOWER.pin);

                this.Vent.CMD_Write("set vcm testmgr stop");

                message.Report("Measured: " + measured.ToString());
                if((measured <= (speed * (1 + (tolerance/100)))) && (measured >= (speed * (1 - (tolerance / 100)))))
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
           




            return success;
        }
        /******************************************************************************************************************************
         *  test_pump
         *  
         *  Function: Tests the pump motor driver by commanding a specific speed, then measuring the frequency out.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             
         *             parameters: - speed : speed to test pump at
         *                         - tolerance: tolerance to measure speed to: in percentage
         *      
         *  
         *  Returns: bool success - returns true if the test passes
         ******************************************************************************************************************************/
        private bool test_pump(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;

            string ventOutput;
            float measured;

            //Note: TryParse not used here because in no situation should the user be able to edit the configuration file. Ergo, no need to catch the error unless working in a dev environment, which throwing an exception is more acceptable as it's useful for debugging what happened.
            float speed = int.Parse(test.parameters["speed"]);
            float tolerance = int.Parse(test.parameters["tolerance"]);

            if (this.powered && this.GPIO.Connected && this.Vent.Connected)
            {

                float upper = float.Parse(test.parameters["upper"]);
                float lower = float.Parse(test.parameters["lower"]);

                message.Report("Setting pump speed  to: " + speed.ToString() + " RPM");

                ventOutput = this.Vent.CMD_Write("set vcm testmgr o2speed " + speed.ToString());

                Thread.Sleep(500);
                //Measure value


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_FREQ_PUMP.port, GPIO_Defs.MEAS_FREQ_PUMP.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage
                measured = this.DMM.Get_Freq(); //Convert to RPM
                this.GPIO.ClearBit(GPIO_Defs.MEAS_FREQ_PUMP.port, GPIO_Defs.MEAS_FREQ_PUMP.pin);

                measured = measured * 6;
                this.Vent.CMD_Write("set vcm testmgr o2stop");

                if ((measured <= upper) && (measured >= lower))
                {
                    success = true;
                }

                message.Report("Measured: " + measured.ToString());
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
        private bool test_sov(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
           
            string output;

            int measured;

            test.parameters.TryGetValue("on_state", out output);

            

            this.Vent.CMD_Write("set vcm sv 11 1");

            //Read the input value
            measured = this.GPIO.GetBit(GPIO_Defs.SOV_SV11.port, GPIO_Defs.SOV_SV11.pin);
            if(measured == 0) { 
                success = true;
            }

            this.Vent.CMD_Write("set vcm sv 11 0");

            //Read the input value
            measured = this.GPIO.GetBit(GPIO_Defs.SOV_SV11.port, GPIO_Defs.SOV_SV11.pin);
            if (measured == 1)
            {
                success = true;
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
        private bool test_sv9_off(IProgress<string> message, IProgress<string> log, TestData test)
        {

            bool success = false;
            float measured;
            float upper = 0;
            float lower = 0;

            if (this.powered && this.DMM.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"]);
                lower = float.Parse(test.parameters["lower"]);
                //Get parameters from test data object

                this.Vent.CMD_Write("set vcm sv 9 0");
                //Connect the desired voltage node to the DMM
                //this.GPIO.SetBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.SetBit(GPIO_Defs.MEAS_O2_SV1N_EN.port, GPIO_Defs.MEAS_O2_SV1N_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage
                measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV1N_EN.port, GPIO_Defs.MEAS_O2_SV1N_EN.pin);
                this.Vent.CMD_Write("set vcm sv 9 0");


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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
        private bool test_sv10_off(IProgress<string> message, IProgress<string> log, TestData test)
        { 

            bool success = false;
            float measured;
            float upper = 0;
            float lower = 0;

            if (this.powered && this.DMM.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"]);
                lower = float.Parse(test.parameters["lower"]);
                //Get parameters from test data object

                this.Vent.CMD_Write("set vcm sv 10 0");
                //Connect the desired voltage node to the DMM
                //this.GPIO.SetBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.SetBit(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage
                measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.pin);
                this.Vent.CMD_Write("set vcm sv 10 0");


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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
        private bool test_sv9_on(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            float measured;
            float upper = 0;
            float lower = 0;

            if (this.powered && this.DMM.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"]);
                lower = float.Parse(test.parameters["lower"]);
                //Get parameters from test data object

                this.Vent.CMD_Write("set vcm sv 9 1");
                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.SetBit(GPIO_Defs.MEAS_O2_SV1N_EN.port, GPIO_Defs.MEAS_O2_SV1N_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage
                measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV1N_EN.port, GPIO_Defs.MEAS_O2_SV1N_EN.pin);
                this.Vent.CMD_Write("set vcm sv 9 0");


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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
        private bool test_sv10_on(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            float measured;
            float upper = 0;
            float lower = 0;

            if (this.powered && this.DMM.Connected && this.Vent.Connected)
            {

                upper = float.Parse(test.parameters["upper"]);
                lower = float.Parse(test.parameters["lower"]);
                //Get parameters from test data object
                
                this.Vent.CMD_Write("set vcm sv 10 1");
                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.SetBit(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage
                measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.pin);
                this.Vent.CMD_Write("set vcm sv 10 0");


                message.Report("Measured: " + measured.ToString() + " V\n");

                if ((measured < (upper)) && (measured > (lower)))
                {
                    success = true;
                }
                else
                {
                    success = false;
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

                this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.pin);

            }
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
        private bool test_cough_valve(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            

            int num;

            test.parameters.TryGetValue("toggle", out var toggle);
            num = int.Parse(toggle);
            for (int i = 0; i < num*2; i++)
            {
                this.Vent.CMD_Write("set vcm coughv " + (i%2).ToString() );
                Thread.Sleep(250); //Value doesn't really matter, the device doesn't drive the valve any faster
                
            }

            if (this.PromptUser_YesNo("Does the cough valve actuate?", test.name))
            {   
                success = true;
            }
            else
            {
                success = false;
            }

            //Fill in measurement parameter
            if (success)
            {
                message.Report(test.name + ": PASS");
                test.parameters["measured"] ="PASS";
            }
            else
            {
                message.Report(test.name + ": FAIL");
                test.parameters["measured"] = "FAIL";
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
        private bool test_low_fan_volt(IProgress<string> message, IProgress<string> log, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;

            // Get parameters from test data object
            float upper = float.Parse(test.parameters["upper"]);
            float lower = float.Parse(test.parameters["lower"]);


            
            

            //Connect the desired voltage node to the DMM
            this.GPIO.SetBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();
            this.GPIO.ClearBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);
            string val;

            message.Report("Measured: " + measured.ToString());

            if ((measured > lower) && (measured < upper))
            {
                success = true;
            }


            
            test.parameters["measured"] = measured.ToString();



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
        private bool test_low_fan_freq(IProgress<string> message, IProgress<string> log, TestData test)
        {
            
            bool success = false;
            
            float upper = float.Parse(test.parameters["upper"]);
            float lower = float.Parse(test.parameters["lower"]);

            if (this.powered && this.DMM.Connected && this.GPIO.Connected)
            {


                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.FAN_FREQ_MEAS_EN.port, GPIO_Defs.FAN_FREQ_MEAS_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage
                float measured = this.DMM.Get_Freq();
                this.GPIO.ClearBit(GPIO_Defs.FAN_FREQ_MEAS_EN.port, GPIO_Defs.FAN_FREQ_MEAS_EN.pin);
                

                message.Report("Measured: " + measured.ToString());

                if ((measured > lower) && (measured < upper))
                {
                    success = true;
                }

                if (success)
                {
                    message.Report(test.name + ": PASS");
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                }
                test.parameters["measured"] = measured.ToString();

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
        private bool test_high_fan(IProgress<string> message, IProgress<string> log, TestData test)
        {
            
            bool success = false;


            if (this.powered && this.Vent.Connected && this.DMM.Connected && this.GPIO.Connected)
            {
                float upper = float.Parse(test.parameters["upper"]);
                float lower = float.Parse(test.parameters["lower"]);

                this.NotifyUser("Please clear all alarms");

                this.Vent.CMD_Write("restart");

                this.Vent.CMD_Write("set uim screen 5039");  //Nebulizer start screenID = 5039

                this.NotifyUser("Please start the nebulizer");

                //Determine if the nebulizer needs to be running.
                string returnVal = this.Vent.CMD_Write("get vcm monitors");
                if (returnVal.Contains("nebulizerActive: 0"))
                {
                    
                    int i = 0;
                    do
                    {
                        Thread.Sleep(1000);
                        i++;
                        returnVal = this.Vent.CMD_Write("get vcm monitors");
                    }
                    while (returnVal.Contains("nebulizerActive: 0") && (i < 15));
                }
                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);
                Thread.Sleep(DMM_DELAY);

                //Measure the voltage
                float v_measured = this.DMM.Get_Volts();
                this.GPIO.ClearBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);


                //this.GPIO.SetBit(GPIO_Defs.FAN_FREQ_MEAS_EN.port, GPIO_Defs.FAN_FREQ_MEAS_EN.pin);
                //float f_measured = this.DMM.Get_Freq();
                //this.GPIO.ClearBit(GPIO_Defs.FAN_FREQ_MEAS_EN.port, GPIO_Defs.FAN_FREQ_MEAS_EN.pin);



                if ((v_measured > lower) && (v_measured < upper))
                {
                    success = true;
                }


                this.Vent.CMD_Write("set uim screen 5039");  //Nebulizer start screenID = 5039
                this.NotifyUser("Please turn off nebulizer therapy");
                var response = this.Vent.CMD_Write("mfgmode");



                message.Report("Measured: " + v_measured.ToString());


                if (success)
                {
                    message.Report(test.name + ": PASS");

                }
                else
                {
                    message.Report(test.name + ": FAIL");
                }
                test.parameters["measured"] = v_measured.ToString();
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_high_fan_freq
         *  
         *  Function: Measures the high speed fan frequency.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the software updates successfully
         *                          returns false if the software does not update successfully
         * 
         ******************************************************************************************************************************/
        private bool test_high_fan_freq(IProgress<string> message, IProgress<string> log, TestData test)
        {
            string str_value;
            bool value_available;
            bool success = false;

            if (this.powered && this.Vent.Connected && this.DMM.Connected && this.GPIO.Connected)
            {
                float upper = float.Parse(test.parameters["upper"]);
                float lower = float.Parse(test.parameters["lower"]);

                //Determine if the nebulizer needs to be running.
                string returnVal = this.Vent.CMD_Write("get vcm monitors");
                if (returnVal.Contains("nebulizerActive: 0"))
                {
                    //Prompt user to begin nebulizer therapy.
                    PromptUser_YesNo("Please start nebulizer therapy on screen and hit enter", test.name);
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
                        this.Vent.CMD_Write("mfgmode");
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
            this.GPIO.SetBit(GPIO_Defs.FAN_FREQ_MEAS_EN.port, GPIO_Defs.FAN_FREQ_MEAS_EN.pin);
            Thread.Sleep(DMM_DELAY);

            //Measure the voltage
            float measured = this.DMM.Get_Freq();
            this.GPIO.ClearBit(GPIO_Defs.FAN_FREQ_MEAS_EN.port, GPIO_Defs.FAN_FREQ_MEAS_EN.pin);
            //Connect the desired voltage node to the DMM
            this.GPIO.SetBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);
            Thread.Sleep(DMM_DELAY);


            var response = this.Vent.CMD_Write("restart");
            PromptUser_YesNo("Please turn off nebulizer therapy now.", test.name);
            response = this.Vent.CMD_Write("mfgmode");



            message.Report("Measured: " + measured.ToString());

            if ((measured > lower) && (measured < upper))
            {
                success = true;
            }


            if (success)
            {
                message.Report(test.name + ": PASS");

            }
            else
            {
                message.Report(test.name + ": FAIL");
            }
            test.parameters["measured"] = measured.ToString();
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
        private bool test_buttons(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;


            int measured=0;
            
            

            //Get Initial State
            var output = this.Vent.CMD_Write("get vcm buttons");
            var buttonStateMatches = Regex.Matches(output, @"(?'button'\s+\w+,)(?'state'\s+\d)(?'falling',\s+\w+\s\w+,)(?'fState'\s+\d)(?'rising',\s+\w+\s\w+)(?'rState'\s+\d)");

            var onState = int.Parse(buttonStateMatches[0].Groups["state"].Value);
            var asState = int.Parse(buttonStateMatches[1].Groups["state"].Value);
            var currState = (onState << 1) + asState;
            if (currState == 0)
            {
                this.GPIO.SetBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);
                Thread.Sleep(200);

                output = this.Vent.CMD_Write("get vcm buttons");
                this.GPIO.ClearBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);

                buttonStateMatches = Regex.Matches(output, @"(?'button'\s+\w+,)(?'state'\s+\d)(?'falling',\s+\w+\s\w+,)(?'fState'\s+\d)(?'rising',\s+\w+\s\w+)(?'rState'\s+\d)");

                onState = int.Parse(buttonStateMatches[0].Groups["state"].Value);
                asState = int.Parse(buttonStateMatches[1].Groups["state"].Value);
                currState = (onState << 1) | asState;


                if ((currState & 1) == 1)
                {
                    measured = currState;
                }
                else
                {
                    measured = 0;
                }
                this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);
                Thread.Sleep(200);

                output = this.Vent.CMD_Write("get vcm buttons");
                this.GPIO.ClearBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);

                buttonStateMatches = Regex.Matches(output, @"(?'button'\s+\w+,)(?'state'\s+\d)(?'falling',\s+\w+\s\w+,)(?'fState'\s+\d)(?'rising',\s+\w+\s\w+)(?'rState'\s+\d)");

                onState = int.Parse(buttonStateMatches[0].Groups["state"].Value);
                asState = int.Parse(buttonStateMatches[1].Groups["state"].Value);
                currState = (onState << 1) | asState;

                if ((currState & 2) == 2)
                {
                    success = true;
                    measured |= currState;
                }



            }
            message.Report("0x" + (measured >> 1).ToString() + (measured & 1).ToString());
            if (success)
            {
                message.Report(test.name + ": PASS");
                test.parameters["measured"] = "0x" + (measured >> 1).ToString() + (measured & 1).ToString();
            }
            else
            {
                message.Report(test.name + ": FAIL");
                test.parameters["measured"] = "0x" + (measured >> 1).ToString() + (measured & 1).ToString();
            }





            //Build binary num representing measured data.

            this.GPIO.SetBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);
            Thread.Sleep(100);
            this.GPIO.ClearBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);
            this.GPIO.ClearBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);

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
         *                          returns false if the barometer responds with pressure outside the range
         * 
         ******************************************************************************************************************************/
        private bool test_barometer(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            
            int upper;
            int lower;
            int samples = int.Parse(test.parameters["samples"]);
            double avePress = 0;
            var count = 0;

            if (this.powered && this.Vent.Connected)
            {
                //Get limits
                upper = int.Parse(test.parameters["upper"]);
                lower = int.Parse(test.parameters["lower"]);

                //Set telemetry channels
                int channelNum = this.Vent.TLMChannels["Sensor:PAmbient_cmH2O_100"];

                this.Vent.CMD_Write("set vcm telemetry " + channelNum + " 0 0 0");

                var response = this.Vent.CMD_Write("get vcm telemetry " + samples.ToString());

                var matches = Regex.Matches(response, @"(?'channel'(?<=vcm\:)\s+" + channelNum.ToString() + @",)(?'counts'(\s+\d+|\s+-\d+))");
                
                
                foreach(Match m in matches)
                {
                    avePress += double.Parse(m.Groups[3].Value);
                    count++;
                }
                avePress = (avePress / count) * 0.000980665;

                if((avePress > lower) && (avePress < upper))
                {
                    success = true;
                }
                //string press = output.Substring(53, 8);
                //var pressure = float.Parse(press) * 0.000980665; //Convert to kPa
                message.Report("Measured: " + avePress.ToString() + " kPa");
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = avePress.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = avePress.ToString();
                }

                
            }
            return success;
        }
    
        /******************************************************************************************************************************
         *  test_ambient_temperature
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
        private bool test_ambient_temperature(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;


            int upper;
            int lower;
            int samples = int.Parse(test.parameters["samples"]);
            double aveTemp = 0;
            var count = 0;

            if (this.powered && this.Vent.Connected)
            {
                //Get limits
                upper = int.Parse(test.parameters["upper"]);
                lower = int.Parse(test.parameters["lower"]);

                //Set telemetry channels
                int channelNum = this.Vent.TLMChannels["Sensor:Tambient_C_100"];

                this.Vent.CMD_Write("set vcm telemetry " + channelNum + " 0 0 0");

                var response = this.Vent.CMD_Write("get vcm telemetry " + samples.ToString());

                var matches = Regex.Matches(response, @"(?'channel'(?<=vcm\:)\s+" + channelNum.ToString() + @",)(?'counts'(\s+\d+|\s+-\d+))");


                foreach (Match m in matches)
                {
                    aveTemp += double.Parse(m.Groups[3].Value);
                    count++;
                }
                aveTemp = (aveTemp / count) / 100;

                if ((aveTemp > lower) && (aveTemp < upper))
                {
                    success = true;
                }
                //string press = output.Substring(53, 8);
                //var pressure = float.Parse(press) * 0.000980665; //Convert to kPa

                message.Report("Measured: " + aveTemp.ToString() + " C");
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = aveTemp.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = aveTemp.ToString();
                }
               

               
            }
            return success;
        }

        private bool test_sysfault9(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;

            if(this.powered && this.Vent.Connected && this.GPIO.Connected)
            {
                int timeout = int.Parse(test.parameters["timeout"]);
                string response = "";
                string fanAlarmStatus = "";


                //Determine initial state, if the alarm is already active then this test should fail
                response = this.Vent.CMD_Write("get vcm alarm status");
                fanAlarmStatus = Regex.Match(response, @"(?<=kVentFanFailure\:)(\s+\w+)").Value;
                if (fanAlarmStatus.Contains("off"))
                {


                    this.GPIO.SetBit(GPIO_Defs.FAN_FAULT_EN.port, GPIO_Defs.FAN_FAULT_EN.pin);


                    int cnt = 0;
                    do
                    {
                        response = this.Vent.CMD_Write("get vcm alarm status");
                        fanAlarmStatus = Regex.Match(response, @"(?<=kVentFanFailure\:)(\s+\w+)").Value;
                        cnt += 500;
                        Thread.Sleep(500); //Don't overload the DUT CPU and wait a bit, expected number of cycles is two through this while loop
                    } while (fanAlarmStatus.Contains("off") && (cnt < timeout));


                    if (!fanAlarmStatus.Contains("off"))
                    {
                        success = true;
                    }


                    this.GPIO.ClearBit(GPIO_Defs.FAN_FAULT_EN.port, GPIO_Defs.FAN_FAULT_EN.pin);
                }
                


                if (success)
                {
                    test.parameters["measured"] = "PASS";
                    message.Report("Measured: kVentFanFailure " + "PASS");

                }
                else
                {
                    test.parameters["measured"] = "FAIL";
                    message.Report("Measured: kVentFanFailure " + "FAIL");

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
        private bool test_microphone(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            float measured = 0;

            if (this.powered && this.Vent.Connected && this.GPIO.Connected)
            {
                /*TODO: 
                 * - Turn on speaker. Sound an alarm.
                 * - Move USB drive to UUT, copy OUT1.wav and OUT2.wav to USB drive.
                 * - Move USB drive to PC.
                 * - Open OUT1.wav or OUT2.wav and perform an amplitude check on the file. Confirm
                 */
                this.Vent.CMD_Write("restart");
                this.GPIO.SetBit(GPIO_Defs.SPKR_EN.port, GPIO_Defs.SPKR_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.FAN_FAULT_EN.port, GPIO_Defs.FAN_FAULT_EN.pin);
                

                //The speaker should now alarm, and the piezo should begin to alarm as well IF the speaker is not loud enough and the microphone is not sensitive enough.
                Thread.Sleep(10000);
                this.GPIO.SetBit(GPIO_Defs.PIEZO_EN.port, GPIO_Defs.PIEZO_EN.pin);
                var ok = this.PromptUser_YesNo("Does the piezo sound?", test.name);

                if (!ok)
                {
                    success = true;
                }



                this.Vent.CMD_Write("mfgmode");
                this.GPIO.ClearBit(GPIO_Defs.SPKR_EN.port, GPIO_Defs.SPKR_EN.pin);
                Thread.Sleep(500);
                this.GPIO.ClearBit(GPIO_Defs.FAN_FAULT_EN.port, GPIO_Defs.FAN_FAULT_EN.pin);
                Thread.Sleep(500);
                this.GPIO.ClearBit(GPIO_Defs.PIEZO_EN.port, GPIO_Defs.PIEZO_EN.pin);
              
                message.Report("Measured: " + "PASS");
                
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                    message.Report("Measured: " + "PASS");
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = "FAIL";
                    message.Report("Measured: " + "FAIL");
                }
            }

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
        private bool test_speaker(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            

            //Enable the speaker
            this.GPIO.SetBit(GPIO_Defs.SPKR_EN.port, GPIO_Defs.SPKR_EN.pin);

            //Restart the VCM app.
            this.Vent.CMD_Write("restart");

            //Prompt user for feedback on speaker sound.
            success = this.PromptUser_YesNo("Wait until the device alarms\nDoes the speaker sound?", test.name);

            //Put device back into MFGmode
            this.Vent.CMD_Write("mfgmode");

            this.GPIO.ClearBit(GPIO_Defs.SPKR_EN.port, GPIO_Defs.SPKR_EN.pin);


            if (success)
            {
                message.Report(test.name + ": PASS");
                test.parameters["measured"] = "PASS";
            }
            else
            {
                message.Report(test.name + ": FAIL");
                test.parameters["measured"] = "FAIIL";
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
        private bool test_piezo(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            

            if (this.GPIO.Connected)
            {
                //Connect piezo alarm
                this.GPIO.SetBit(GPIO_Defs.PIEZO_EN.port, GPIO_Defs.PIEZO_EN.pin);
                this.Vent.CMD_Write("restart");
                //Prompt user to hear piezo.
                Thread.Sleep(5000);
                if(this.PromptUser_YesNo("Does the piezo alarm?", test.name))
                {
                    success = true;
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    success = false;
                    test.parameters["measured"] = "FAIL";
                }

                this.Vent.CMD_Write("mfgmode");
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
         *  Returns: bool success - returns true if valve toggles the specified number of times
         *                          returns false if valve does not toggle the specified number of times
         * 
         ******************************************************************************************************************************/
        private bool test_xflow_sv1(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int toggle;

            if (this.powered && this.Vent.Connected && this.GPIO.Connected)
            {
                toggle = int.Parse(test.parameters["toggle"], System.Globalization.NumberStyles.Integer);

                int count = 0;
                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 1 1");
                    Thread.Sleep(100);

                    if (this.GPIO.GetBit(GPIO_Defs.XFLOW_SV1_2.port, GPIO_Defs.XFLOW_SV1_2.pin) == 1)
                    {
                        this.Vent.CMD_Write("set vcm sv 1 0");
                        Thread.Sleep(100);
                        if (this.GPIO.GetBit(GPIO_Defs.XFLOW_SV1_2.port, GPIO_Defs.XFLOW_SV1_2.pin) == 0)
                        {
                            count++;
                        }
                    }
                    else
                    {
                        this.Vent.CMD_Write("set vcm sv 1 0");
                        Thread.Sleep(100);
                    }
                }
                message.Report("SV1&2 count = " + count.ToString());

                if (count == toggle)
                {
                    success = true;
                }
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = count.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = count.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
         *  test_xflow_sv3
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if valve toggles the specified number of times
         *                          returns false if valve does not toggle the specified number of times
         * 
         ******************************************************************************************************************************/
        private bool test_xflow_sv3(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int toggle;

            if (this.powered && this.Vent.Connected && this.GPIO.Connected)
            {
                toggle = int.Parse(test.parameters["toggle"], System.Globalization.NumberStyles.Integer);

                int count = 0;
                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 3 1");
                    Thread.Sleep(100);

                    if (this.GPIO.GetBit(GPIO_Defs.XFLOW_SV3_4.port, GPIO_Defs.XFLOW_SV3_4.pin) == 1)
                    {
                        this.Vent.CMD_Write("set vcm sv 3 0");
                        Thread.Sleep(100);
                        if (this.GPIO.GetBit(GPIO_Defs.XFLOW_SV3_4.port, GPIO_Defs.XFLOW_SV3_4.pin) == 0)
                        {
                            count++;
                        }
                    }
                    else
                    {
                        this.Vent.CMD_Write("set vcm sv 3 0");
                        Thread.Sleep(100);
                    }
                }
                message.Report("SV3&4 count = " + count.ToString());

                if (count == toggle)
                {
                    success = true;
                }
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = count.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = count.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
        *  test_xflow_i2c
        *  
        *  Function: Commands UUT to read from the external flow module i2c chip
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the i2c command returns with 0 i2c errors
        *                          returns false if the i2c command returns with 1 or more i2c errors
        * 
        ******************************************************************************************************************************/
        private bool test_xflow_i2c(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int i2c_error = 0;

            if (this.powered && this.Vent.Connected)
            {

                //Command UUT to read from device Exhalation board
                var response = this.Vent.CMD_Write("get vcm cal extflow");

                i2c_error = int.Parse(Regex.Match(response, @"(?<=i2cStatus:\s)[\d]").Value);


                if (i2c_error == 0)
                {
                    success = true;
                }
                else
                {

                }

                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = i2c_error.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
        *  test_xflow_spi
        *  
        *  Function: Commands UUT to read from the external flow module spi chip
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the spi bus averages to about 0 over 50 samples
        *                          returns false if the spi bus returns a value that is -1
        * 
        ******************************************************************************************************************************/
        private bool test_xflow_spi(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = true;
            
            int samples;

            if (this.powered && this.Vent.Connected)
            {

                samples = int.Parse(test.parameters["samples"]);

                

                //Get the telemetry channel for DSensor:PdiffIntWide_counts
                int channel = this.Vent.TLMChannels["DSensor:PdiffExtWideRaw_counts"];
                var response = this.Vent.CMD_Write("set vcm telemetry " + channel.ToString() + " 0 0 0");

                response = this.Vent.CMD_Write("get vcm telemetry " + samples.ToString()); //100 samples retrieved from DUT

                var matches = Regex.Matches(response, @"(?'channel'(?<=vcm\:\s\s)" + channel.ToString() + @",)(?'counts'(\s+\d+|\s+-\d+))");

                double averageCounts = 0;
                int cnt = 0;
                foreach (Match m in matches)
                {
                    averageCounts += double.Parse(m.Groups[3].Value);
                    cnt++;
                    if (int.Parse(m.Groups[3].Value) == -1)
                    {
                        success = false;
                        break;
                    }
                }
                averageCounts = averageCounts / cnt;

                message.Report("Measured: " + averageCounts.ToString() + " counts");

                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = averageCounts.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = averageCounts.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
         *  test_exhl_sv6
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if valve toggles the specified number of times
         *                          returns false if valve does not toggle the specified number of times
         * 
         ******************************************************************************************************************************/
        private bool test_exhl_sv6(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int toggle;

            if (this.powered && this.Vent.Connected && this.GPIO.Connected)
            {
                toggle = int.Parse(test.parameters["toggle"], System.Globalization.NumberStyles.Integer);

                int count = 0;
                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 6 1");
                    Thread.Sleep(100);

                    if (this.GPIO.GetBit(GPIO_Defs.EXHL_SV6.port, GPIO_Defs.EXHL_SV6.pin) == 1)
                    {
                        this.Vent.CMD_Write("set vcm sv 6 0");
                        Thread.Sleep(100);
                        if (this.GPIO.GetBit(GPIO_Defs.EXHL_SV6.port, GPIO_Defs.EXHL_SV6.pin) == 0)
                        {
                            count++;
                        }
                    }
                    else
                    {
                        this.Vent.CMD_Write("set vcm sv 6 0");
                        Thread.Sleep(100);
                    }
                }
                message.Report("SV6 count = " + count.ToString());
                if (count == toggle)
                {
                    success = true;
                }
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = count.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = count.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
         *  test_exhl_sv7
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if valve toggles the specified number of times
         *                          returns false if valve does not toggle the specified number of times
         * 
         ******************************************************************************************************************************/
        private bool test_exhl_sv7(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int toggle;

            if (this.powered && this.Vent.Connected && this.GPIO.Connected)
            {
                toggle = int.Parse(test.parameters["toggle"], System.Globalization.NumberStyles.Integer);

                int count = 0;
                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 7 1");
                    Thread.Sleep(100);

                    if (this.GPIO.GetBit(GPIO_Defs.EXHL_SV7.port, GPIO_Defs.EXHL_SV7.pin) == 1)
                    {
                        this.Vent.CMD_Write("set vcm sv 7 0");
                        Thread.Sleep(100);
                        if (this.GPIO.GetBit(GPIO_Defs.EXHL_SV7.port, GPIO_Defs.EXHL_SV7.pin) == 0)
                        {
                            count++;
                        }
                    }
                    else
                    {
                        this.Vent.CMD_Write("set vcm sv 7 0");
                        Thread.Sleep(100);
                    }
                }
                message.Report("SV7 count = " + count.ToString());
                if (count == toggle)
                {
                    success = true;
                }
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = count.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = count.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
         *  test_exhl_sv8
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if valve toggles the specified number of times
         *                          returns false if valve does not toggle the specified number of times
         * 
         ******************************************************************************************************************************/
        private bool test_exhl_sv8(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int toggle;

            if (this.powered && this.Vent.Connected && this.GPIO.Connected)
            {
                toggle = int.Parse(test.parameters["toggle"], System.Globalization.NumberStyles.Integer);

                int count = 0;
                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 8 1");
                    Thread.Sleep(100);

                    if (this.GPIO.GetBit(GPIO_Defs.EXHL_SV8.port, GPIO_Defs.EXHL_SV8.pin) == 0)
                    {
                        this.Vent.CMD_Write("set vcm sv 8 0");
                        Thread.Sleep(100);
                        if (this.GPIO.GetBit(GPIO_Defs.EXHL_SV8.port, GPIO_Defs.EXHL_SV8.pin) == 1)
                        {
                            count++;
                        }
                    }
                    else
                    {
                        this.Vent.CMD_Write("set vcm sv 8 0");
                        Thread.Sleep(100);
                    }
                }
                message.Report("SV8 count = " + count.ToString());
                if (count == toggle)
                {
                    success = true;
                }
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = count.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = count.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
        *  test_exhl_i2c
        *  
        *  Function: Commands UUT to read from the exhalation module i2c chip
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the i2c command returns with 0 i2c errors
        *                          returns false if the i2c command returns with 1 or more i2c errors
        * 
        ******************************************************************************************************************************/
        private bool test_exhl_i2c(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int i2c_error = 0;

            if (this.powered && this.Vent.Connected)
            {

                //Command UUT to read from device Exhalation board
                var response = this.Vent.CMD_Write("get vcm cal exhl");

                i2c_error = int.Parse(Regex.Match(response, @"(?<=i2cStatus:\s)[\d]").Value);
                

                if (i2c_error == 0)
                {
                    success = true;
                }
                else
                {

                }

                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = i2c_error.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
         *  test_flow_sv5
         *  
         *  Function: Commands UUT to toggle the Solenoid valve x number of times, reads the signal at the GPIO module
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *  
         *  Returns: bool success - returns true if the solenoid toggles specified number of times
         *                          returns false if the solenoid does not toggle the specified number of times
         * 
         ******************************************************************************************************************************/
        private bool test_flow_sv5(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int toggle;

            if (this.powered && this.Vent.Connected && this.GPIO.Connected)
            {   
                toggle = int.Parse(test.parameters["toggle"], System.Globalization.NumberStyles.Integer);

                int count = 0;
                for (int i = 0; i < toggle; i++)
                {
                    this.Vent.CMD_Write("set vcm sv 5 1");
                    Thread.Sleep(100);

                    if(this.GPIO.GetBit(GPIO_Defs.FLOW_SV5.port, GPIO_Defs.FLOW_SV5.pin) == 1)
                    {
                        this.Vent.CMD_Write("set vcm sv 5 0");
                        Thread.Sleep(100);
                        if(this.GPIO.GetBit(GPIO_Defs.FLOW_SV5.port, GPIO_Defs.FLOW_SV5.pin) == 0)
                        {
                            count++;
                        }
                    }
                    else
                    {
                        this.Vent.CMD_Write("set vcm sv 5 0");
                        Thread.Sleep(100);
                    }
                }
                message.Report("SV5 count = " + count.ToString());
                if(count == toggle)
                {
                    success = true;
                }
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = count.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = count.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
        *  test_flow_i2c
        *  
        *  Function: Commands UUT to read from the internal flow module i2c chip
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the i2c command returns with 0 i2c errors
        *                          returns false if the i2c command returns with 1 or more i2c errors
        * 
        ******************************************************************************************************************************/
        private bool test_flow_i2c(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int i2c_error = 0;

            if (this.powered && this.Vent.Connected)
            {

                //Command UUT to read from device Exhalation board
                var response = this.Vent.CMD_Write("get vcm cal intflow");

                i2c_error = int.Parse(Regex.Match(response, @"(?<=i2cStatus:\s)[\d]").Value);


                if (i2c_error == 0)
                {
                    success = true;
                }
                else
                {

                }

                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = i2c_error.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
        *  test_flow_spi
        *  
        *  Function: Commands UUT to read from the internal flow module i2c chip
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the spi bus averages to about 0 over 50 samples
        *                          returns false if the i2c command returns with 1 or more i2c errors
        * 
        ******************************************************************************************************************************/
        private bool test_flow_spi(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = true;
            int i2c_error = 0;
            int samples;

            if (this.powered && this.Vent.Connected)
            {

                samples = int.Parse(test.parameters["samples"]);

                //Command UUT to read from device Exhalation board

                //Get the telemetry channel for DSensor:PdiffIntWide_counts
                int channel = this.Vent.TLMChannels["DSensor:PdiffIntWideRaw_counts"];
                var response = this.Vent.CMD_Write("set vcm telemetry " + channel.ToString() + " 0 0 0");

                response = this.Vent.CMD_Write("get vcm telemetry " + samples.ToString()); //100 samples retrieved from DUT

                var matches = Regex.Matches(response, @"(?'channel'(?<=vcm\:\s\s)" + channel.ToString() + @",)(?'counts'(\s+\d+|\s+-\d+))");

                double averageCounts = 0;
                int cnt = 0;
                foreach(Match m in matches)
                {
                    averageCounts += double.Parse(m.Groups[3].Value);
                    cnt++;
                    if (int.Parse(m.Groups[3].Value) == -1)
                    {
                        success = false;
                        break; 
                    }
                }
                averageCounts = averageCounts / cnt;

                message.Report("Measured: " + averageCounts.ToString() + " counts");

                if (success)
                {
                    test.parameters["measured"] = averageCounts.ToString();
                }
                else
                {
                    
                    test.parameters["measured"] = averageCounts.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
        *  test_mrotary_valve_1
        *  
        *  Function: Commands UUT to move the RV1 to position 4, measure the home flag, then move the RV1 to home position
        *  measure the home flag again
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the home flag toggles as anticipated
        *                          returns false if the home flag does not.
        * 
        ******************************************************************************************************************************/
        private bool test_rotary_valve_1(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int DELAY = 1000;

            if (this.powered && this.Vent.Connected)
            {
                var response = this.Vent.CMD_Write("set vcm rotaryv 1 0"); //Command UUT to home motor
                Thread.Sleep(DELAY);

                //Command UUT to move motor to pos 4 --> opposite of home position
                response = this.Vent.CMD_Write("set vcm rotaryv 1 4");
                Thread.Sleep(DELAY);

                var measured = this.GPIO.GetBit(GPIO_Defs.MEAS_RV1_HOME.port, GPIO_Defs.MEAS_RV1_HOME.pin);

                if (measured == 0)
                {
                    //Starting off good, the home flag is not set. Now move 


                   
                    response = this.Vent.CMD_Write("set vcm rotaryv 1 2");
                    Thread.Sleep(DELAY);
                    response = this.Vent.CMD_Write("set vcm rotaryv 1 0");
                    Thread.Sleep(DELAY);
                    measured = this.GPIO.GetBit(GPIO_Defs.MEAS_RV1_HOME.port, GPIO_Defs.MEAS_RV1_HOME.pin);
               

                    if (measured == 1)
                    {
                        success = true;
                    }

                }


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
            return success;
        }
        /******************************************************************************************************************************
        *  test_mrotary_valve_2
        *  
        *  Function: Commands UUT to move the RV2 to position 4, measure the home flag, then move the RV2 to home position
        *  measure the home flag again
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the home flag toggles as anticipated
        *                          returns false if the home flag does not.
        * 
        ******************************************************************************************************************************/
        private bool test_rotary_valve_2(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int DELAY = 500;

            if (this.powered && this.Vent.Connected)
            {

                //Command UUT to move motor to pos 4 --> Opposite of the home position
                var response = this.Vent.CMD_Write("set vcm rotaryv 2 0");
                Thread.Sleep(DELAY);
                response = this.Vent.CMD_Write("set vcm rotaryv 2 2");
                Thread.Sleep(DELAY);

                var measured = this.GPIO.GetBit(GPIO_Defs.MEAS_RV2_HOME.port, GPIO_Defs.MEAS_RV2_HOME.pin);

                if (measured == 0)
                {
                    //Starting off good, the home flag is not set. Now move 


                    response = this.Vent.CMD_Write("set vcm rotaryv 2 4");
                    Thread.Sleep(DELAY);
                    response = this.Vent.CMD_Write("set vcm rotaryv 2 0");
                    Thread.Sleep(DELAY);
                    measured = this.GPIO.GetBit(GPIO_Defs.MEAS_RV2_HOME.port, GPIO_Defs.MEAS_RV2_HOME.pin);
                    if (measured == 1)
                    {
                        success = true;
                    }

                }


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
            return success;
        }
        /******************************************************************************************************************************
        *  test_metering_valve
        *  
        *  Function: Commands UUT to move the MV to an open position, measure the home flag, then move the MV to home position
        *  measure the home flag again
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the home flag toggles as anticipated
        *                          returns false if the home flag does not.
        * 
        ******************************************************************************************************************************/
        private bool test_metering_valve(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int DELAY = 500;

            if (this.powered && this.Vent.Connected)
            {

                //Command UUT to read from device Exhalation board
                var response = this.Vent.CMD_Write("set vcm metering pos 1000");
                Thread.Sleep(DELAY);

                var measured = this.GPIO.GetBit(GPIO_Defs.MEAS_MV_HOME.port, GPIO_Defs.MEAS_MV_HOME.pin);

                if (measured == 1)
                {
                    //Starting off good, the home flag is not set. Now move 


                    response = this.Vent.CMD_Write("set vcm metering home");
                    Thread.Sleep(DELAY);
                    measured = this.GPIO.GetBit(GPIO_Defs.MEAS_MV_HOME.port, GPIO_Defs.MEAS_MV_HOME.pin);
                    if(measured == 0)
                    {
                        success = true;
                    }
                    
                }
                

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
            return success;
        }


        /******************************************************************************************************************************
        *  test_metering_i2c
        *  
        *  Function: Commands UUT to read from the internal flow module i2c chip
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the i2c command returns with 0 i2c errors
        *                          returns false if the i2c command returns with 1 or more i2c errors
        * 
        ******************************************************************************************************************************/
        private bool test_mv_i2c(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int i2c_error = 0;

            if (this.powered && this.Vent.Connected)
            {

                //Command UUT to read from device Exhalation board
                var response = this.Vent.CMD_Write("get vcm cal metering");

                i2c_error = int.Parse(Regex.Match(response, @"(?<=i2cStatus:\s)[\d]").Value);


                if (i2c_error == 0)
                {
                    success = true;
                }
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = i2c_error.ToString();
                }
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
        private bool test_as_led(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            


            if(this.powered && this.GPIO.Connected)
            {
                this.NotifyUser("Please clear all alarms before proceeding");

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
                        if (ledFlash[i] == 1) ;
                        else if (ledFlash[i] == 0) ;
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
        private bool test_pb_led(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            if (this.powered && this.GPIO.Connected)
            {


                var measured = this.GPIO.GetBit(GPIO_Defs.MEAS_ON_LED.port, GPIO_Defs.MEAS_ON_LED.pin);
                if(measured == 0)
                {
                    success = true;
                }

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
        private bool test_batt_comms(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
            int measured = 0;
            int retry = 0;
            string powerOutput = "";
            string matchPattern = @"(?'battery'^\s+[a-zA-Z0-9]+:)(?'present'\s+\d+,)(?'charge'\s+\d+,)(?'err'\s+\d+,)(?'RSOC'\s+\d+,)(?'ASOC'\s+\d+,)(?'temp'\s+\d+)";

            if (this.powered && this.Vent.Connected && this.GPIO.Connected)
            {
                //Command UUT with "get vcm power"
                
                    
                powerOutput = this.Vent.CMD_Write("get vcm power");
                
                var matches = Regex.Matches(powerOutput, @"(?'battery'\s+\w+:)(?'present'\s+\d+,)(?'charge'\s+\d+,)(?'err'\s+\d+,)(?'RSOC'\s+\d+,)(?'ASOC'\s+\d+,)(?'temp'\s+\d+)");


                var batt0_RSOC = matches[2].Groups[6].Value.Replace(",","");
                var batt0_ASOC = matches[2].Groups[7].Value.Replace(",", "");

                var batt1_RSOC = matches[0].Groups[6].Value.Replace(",", "");
                var batt1_ASOC = matches[0].Groups[7].Value.Replace(",", "");
                
                var batt2_RSOC = matches[1].Groups[6].Value.Replace(",", "");
                var batt2_ASOC = matches[1].Groups[7].Value.Replace(",", "");

                if((batt0_ASOC == batt1_ASOC) && (batt0_ASOC == batt2_ASOC))
                {
                    if ((batt0_RSOC == batt1_RSOC) && (batt0_RSOC == batt2_RSOC))
                    {
                        success = true;
                    }
                }
                message.Report("BATT0 ASOC:" + batt0_ASOC);
                message.Report("BATT0 RSOC:" + batt0_RSOC);
                message.Report("BATT1 ASOC:" + batt1_ASOC);
                message.Report("BATT1 RSOC:" + batt1_RSOC);
                message.Report("BATT2 ASOC:" + batt2_ASOC);
                message.Report("BATT2 RSOC:" + batt2_RSOC);



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
        private bool test_batt_temp(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            
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


                    var BAT0_Data = powerArray[9].Trim().Substring(7).Split(',');
                    var BAT1_Data = powerArray[7].Trim().Substring(8).Split(',');
                    var BAT2_Data = powerArray[8].Trim().Substring(8).Split(',');

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
         *  test_batt0_source
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
        private bool  test_batt0_source(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;

            if(this.powered && this.Vent.Connected && this.PPS.Connected)
            {


                //Set power supply to 16V
                this.PPS.Set_Output(true, 16, 5);

                this.GPIO.SetBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);
                Thread.Sleep(250);
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                Thread.Sleep(250);

                //Confirm that device is still running.
                var response = this.Vent.CMD_Write("ping uim");

                if (response.Contains("pong uim"))
                {
                    response = this.Vent.CMD_Write("ping vcm");
                    if(response.Contains("pong vcm"))
                    {
                        success = true;
                    }
                   
                }


                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                Thread.Sleep(250);
                this.GPIO.ClearBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);
                this.PPS.Set_Output(false);


                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = "FAIIL";
                }
            }

            return success;
        }
        private bool test_batt1_source(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;

            if (this.powered && this.Vent.Connected && this.PPS.Connected)
            {


                //Set power supply to 16V
                this.PPS.Set_Output(true, 16, 5);

                this.GPIO.SetBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);
                Thread.Sleep(250);
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                Thread.Sleep(250);

                //Confirm that device is still running.
                var response = this.Vent.CMD_Write("ping uim");

                if (response.Contains("pong uim"))
                {
                    response = this.Vent.CMD_Write("ping vcm");
                    if (response.Contains("pong vcm"))
                    {
                        success = true;
                    }

                }


                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                Thread.Sleep(250);
                this.GPIO.ClearBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);
                this.PPS.Set_Output(false);

                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = "FAIIL";
                }
            }

            return success;
        }
        private bool test_batt2_source(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;

            if (this.powered && this.Vent.Connected && this.PPS.Connected)
            {


                //Set power supply to 16V
                this.PPS.Set_Output(true, 16, 5);

                this.GPIO.SetBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);
                Thread.Sleep(250);
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                Thread.Sleep(250);

                //Confirm that device is still running.
                var response = this.Vent.CMD_Write("ping uim");

                if (response.Contains("pong uim"))
                {
                    response = this.Vent.CMD_Write("ping vcm");
                    if (response.Contains("pong vcm"))
                    {
                        success = true;
                    }

                }


                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                Thread.Sleep(250);
                this.GPIO.ClearBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);
                this.PPS.Set_Output(false);

                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = "FAIIL";
                }
            }

            return success;
        }


        /******************************************************************************************************************************
         *  test_charge_led
         *  
         *  Function: Connects a load to the internal battery port and waits for the system to start charging. The Charge LED should light up and be measured by the GPIO module as a low.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             parameters: - 
         *             

         *  Returns: bool success - returns true if charge led cathode pin is low
         *  Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *                  - parameters does not contain Key
         
         ******************************************************************************************************************************/

        private bool test_charge_led(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;

            if(this.powered && this.GPIO.Connected && this.PPS.Connected && this.Vent.Connected)
            {
                int timeout = int.Parse(test.parameters["timeout"]);

                //Initialize a charging scenario
                this.GPIO.SetBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.TEMP_BATT0.port, GPIO_Defs.TEMP_BATT0.pin);
                this.GPIO.SetBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);

                //Wait for battery to begin charging
                message.Report("Waiting for charge led to light up ... ");

                int meas;
                int time = 0;


                do
                {
                    meas = this.GPIO.GetBit(GPIO_Defs.MEAS_CHG_LED.port, GPIO_Defs.MEAS_CHG_LED.pin);
                    if (meas == 0)
                    {
                        success = true;
                        break;
                    }
                    Thread.Sleep(1000);
                    time++;
                } while (time < timeout);

                //Turn off PPS and battery

                this.GPIO.ClearBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.TEMP_BATT0.port, GPIO_Defs.TEMP_BATT0.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);


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

            return success;
        }
        private bool test_chg_monitor(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            string response;
            double ave_mVolts = 0;

            if (this.powered && this.GPIO.Connected && this.PPS.Connected && this.Vent.Connected)
            {
                
                float upper = float.Parse(test.parameters["upper"]);
                float lower = float.Parse(test.parameters["lower"]);
                int samples = int.Parse(test.parameters["samples"]);

                //Initialize a charging scenario
                this.GPIO.SetBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.TEMP_BATT0.port, GPIO_Defs.TEMP_BATT0.pin);
                this.GPIO.SetBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);



                //Set telemetry channels
                int channelNum = this.Vent.TLMChannels["Sensor:VchargeMonitor_F_mv"];

                this.Vent.CMD_Write("set vcm telemetry " + channelNum + " 0 0 0");

                response = this.Vent.CMD_Write("get vcm telemetry " + samples.ToString());

                var matches = Regex.Matches(response, @"(?'channel'(?<=vcm\:)\s+" + channelNum.ToString() + @",)(?'counts'(\s+\d+|\s+-\d+))");

                int count = 0;
                foreach (Match m in matches)
                {
                    ave_mVolts += double.Parse(m.Groups[3].Value);
                    count++;
                }
                ave_mVolts = (ave_mVolts / count) / 1000;

                if ((ave_mVolts > lower) && (ave_mVolts < upper))
                {
                    success = true;
                }



                //Turn off PPS and battery

                this.GPIO.ClearBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.TEMP_BATT0.port, GPIO_Defs.TEMP_BATT0.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);


                //Fill in measurement parameter
                test.parameters["measured"] = ave_mVolts.ToString();
                message.Report("Measured: " + ave_mVolts.ToString() + "V");
                if (success)
                {
                    message.Report(test.name + ": PASS");
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                }
            }

            return success;
        }

        /********************************************************************************************************************************
         *  test_batt0_charge
         *
         * Function: Connects a power supply and a
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         * TestData test             - Variable that contains all of the necessary test data.
         *
         * parameters: - resistance - Denotes the sense resistor used to calucalate current from volts
         *                         - 
         *                         - upper - Upper limit for passing result
         *                         - lower - Lower limit for passing result
         *                         - chg_delay - Delay in milliseconds to wait between connecting load and measuring current through the load
         *             
         *
         *  Returns: bool success - returns true if alarm silence led lights up
         * returns false if alarm silence led does not light up
         * Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *
         *                  - Parameters does not contain Key
         *
         *
         ******************************************************************************************************************************/

        private bool test_batt0_charge(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            

            if (this.powered && this.GPIO.Connected && this.PPS.Connected && this.Vent.Connected)
            {
                int timeout = int.Parse(test.parameters["timeout"]);
                int delay = int.Parse(test.parameters["delay"]);
                float upper = float.Parse(test.parameters["upper"]);
                float lower = float.Parse(test.parameters["lower"]);

                //Initialize a charging scenario
                this.GPIO.SetBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.TEMP_BATT0.port, GPIO_Defs.TEMP_BATT0.pin);
                this.GPIO.SetBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);

                
                

                float meas;
                int time = 0;

                Thread.Sleep(delay);

                do
                {
                    meas = this.DMM.Get_Amps() * -1;
                    if ((meas <= upper) && (meas >= lower))
                    {
                        success = true;
                        break;
                    }
                    Thread.Sleep(1000);
                    time++;
                } while (time < timeout);

                //Turn off PPS and battery

                this.GPIO.ClearBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.TEMP_BATT0.port, GPIO_Defs.TEMP_BATT0.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);


                //Fill in measurement parameter
                test.parameters["measured"] = meas.ToString();
                message.Report("Measured: " + meas.ToString() + "A");
                if (success)
                {
                    message.Report(test.name + ": PASS");   
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                }
            }

            return success;
        }
        /********************************************************************************************************************************
         *  test_batt1_charge
         *
         * Function: Connects a battery to the battery connector and measures the current flow into the battery
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         * TestData test             - Variable that contains all of the necessary test data.
         *
         * parameters: - resistance - Denotes the sense resistor used to calucalate current from volts
         *                         - 
         *                         - upper - Upper limit for passing result
         *                         - lower - Lower limit for passing result
         *                         - chg_delay - Delay in milliseconds to wait between connecting load and measuring current through the load
         *             
         *
         *  Returns: bool success - returns true if alarm silence led lights up
         * returns false if alarm silence led does not light up
         * Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *
         *                  - Parameters does not contain Key
         *
         *
         ******************************************************************************************************************************/

        private bool test_batt1_charge(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;


            if (this.powered && this.GPIO.Connected && this.PPS.Connected && this.Vent.Connected)
            {
                int timeout = int.Parse(test.parameters["timeout"]);
                int delay = int.Parse(test.parameters["delay"]);
                float upper = float.Parse(test.parameters["upper"]);
                float lower = float.Parse(test.parameters["lower"]);

                //Initialize a charging scenario
                this.GPIO.SetBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.TEMP_BATT2.port, GPIO_Defs.TEMP_BATT2.pin);
                this.GPIO.SetBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);




                float meas;
                int time = 0;

                Thread.Sleep(delay);

                do
                {
                    meas = this.DMM.Get_Amps() * -1;
                    if ((meas <= upper) && (meas >= lower))
                    {
                        success = true;
                        break;
                    }
                    Thread.Sleep(1000);
                    time++;
                } while (time < timeout);

                //Turn off PPS and battery

                this.GPIO.ClearBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.TEMP_BATT2.port, GPIO_Defs.TEMP_BATT2.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);


                //Fill in measurement parameter
                test.parameters["measured"] = meas.ToString();
                message.Report("Measured: " + meas.ToString() + "A");
                if (success)
                {
                    message.Report(test.name + ": PASS");
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                }
            }

            return success;
        }
        /********************************************************************************************************************************
         *  test_batt2_charge
         *
         * Function: Connects a battery to the battery connector and measures the current flow into the battery
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         * TestData test             - Variable that contains all of the necessary test data.
         *
         * parameters: - resistance - Denotes the sense resistor used to calucalate current from volts
         *                         - 
         *                         - upper - Upper limit for passing result
         *                         - lower - Lower limit for passing result
         *                         - chg_delay - Delay in milliseconds to wait between connecting load and measuring current through the load
         *             
         *
         *  Returns: bool success - returns true if alarm silence led lights up
         * returns false if alarm silence led does not light up
         * Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *
         *                  - Parameters does not contain Key
         *
         *
         ******************************************************************************************************************************/

        private bool test_batt2_charge(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;


            if (this.powered && this.GPIO.Connected && this.PPS.Connected && this.Vent.Connected)
            {
                int timeout = int.Parse(test.parameters["timeout"]);
                int delay = int.Parse(test.parameters["delay"]);
                float upper = float.Parse(test.parameters["upper"]);
                float lower = float.Parse(test.parameters["lower"]);

                //Initialize a charging scenario
                this.GPIO.SetBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.TEMP_BATT1.port, GPIO_Defs.TEMP_BATT1.pin);
                this.GPIO.SetBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);




                float meas;
                int time = 0;

                Thread.Sleep(delay);

                
                do
                {
                    meas = this.DMM.Get_Amps() * -1;
                   
                    
                    if ((meas <= upper) && (meas >= lower))
                    {
                        success = true;
                        break;
                    }
                    Thread.Sleep(1000);
                    time++;
                } while (time < timeout);

                //Turn off PPS and battery

                this.GPIO.ClearBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.TEMP_BATT1.port, GPIO_Defs.TEMP_BATT1.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);


                //Fill in measurement parameter
                test.parameters["measured"] = meas.ToString();
                message.Report("Measured: " + meas.ToString() + "A");
                if (success)
                {
                    message.Report(test.name + ": PASS");
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                }
            }

            return success;
        }

        private bool test_nurse_call(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;

            float upper = float.Parse(test.parameters["upper"]);
            float lower = float.Parse(test.parameters["lower"]);

            
            if(this.powered && this.Vent.Connected && this.GPIO.Connected && this.DMM.Connected)
            {
                //Measure NC
                this.NotifyUser("Please clear all alarms before continuing. If the alarm cannot be cleared, this test will fail");


                this.GPIO.SetBit(GPIO_Defs.MEAS_NC_NC.port, GPIO_Defs.MEAS_NC_NC.pin);
                Thread.Sleep(1000);
                var nc_measured = this.DMM.Get_Ohms();
                this.GPIO.ClearBit(GPIO_Defs.MEAS_NC_NC.port, GPIO_Defs.MEAS_NC_NC.pin);
                this.GPIO.SetBit(GPIO_Defs.MEAS_NC_NO.port, GPIO_Defs.MEAS_NC_NO.pin);
                Thread.Sleep(1000);
                var no_measured = this.DMM.Get_Ohms();

                message.Report("No alarms:");
                message.Report("NC measured: " + nc_measured.ToString());
                message.Report("NO measured: " + no_measured.ToString());

                this.GPIO.ClearBit(GPIO_Defs.MEAS_NC_NO.port, GPIO_Defs.MEAS_NC_NO.pin);


                //Try again with alarms
                //this.Vent.CMD_Write("restart");
                this.GPIO.SetBit(GPIO_Defs.FAN_FAULT_EN.port, GPIO_Defs.FAN_FAULT_EN.pin);

                string response;
                string fanAlarmStatus;
                int cnt = 0;
                int timeout = 30000;
                do
                {
                    response = this.Vent.CMD_Write("get vcm alarm status");
                    fanAlarmStatus = Regex.Match(response, @"(?<=kVentFanFailure\:)(\s+\w+)").Value;
                    cnt += 500;
                    Thread.Sleep(500); //Don't overload the DUT CPU and wait a bit, expected number of cycles is two through this while loop
                } while (fanAlarmStatus.Contains("off") && (cnt < timeout));

                this.GPIO.SetBit(GPIO_Defs.MEAS_NC_NC.port, GPIO_Defs.MEAS_NC_NC.pin);
                Thread.Sleep(1000);
                var no_measured_alarm = this.DMM.Get_Ohms(); //Should be short
                this.GPIO.ClearBit(GPIO_Defs.MEAS_NC_NC.port, GPIO_Defs.MEAS_NC_NC.pin);
                this.GPIO.SetBit(GPIO_Defs.MEAS_NC_NO.port, GPIO_Defs.MEAS_NC_NO.pin);
                Thread.Sleep(1000);
                var nc_measured_alarm = this.DMM.Get_Ohms(); //Should be open


                
                message.Report("With alarms: ");
                message.Report("NC measured: " + nc_measured_alarm.ToString());
                message.Report("NO measured: " + no_measured_alarm.ToString());

                if ((nc_measured > lower) && (nc_measured < upper) && (no_measured > 1000) && (no_measured_alarm > lower) && (no_measured_alarm < upper) && (nc_measured_alarm > 1000))
                {
                    success = true;                     

                }
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

                this.GPIO.ClearBit(GPIO_Defs.MEAS_NC_NC.port, GPIO_Defs.MEAS_NC_NC.pin);
                this.GPIO.ClearBit(GPIO_Defs.MEAS_NC_NO.port, GPIO_Defs.MEAS_NC_NO.pin);
                this.GPIO.ClearBit(GPIO_Defs.FAN_FAULT_EN.port, GPIO_Defs.FAN_FAULT_EN.pin);
                //this.Vent.CMD_Write("mfgmode");
            }
            return success;

        }
        private bool test_batt0_diode(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;


            float tolerance = float.Parse(test.parameters["tolerance"]);
            float upper = float.Parse(test.parameters["upper"]);
            float lower = float.Parse(test.parameters["lower"]);
            int samples = int.Parse(test.parameters["samples"]);
            string response;

            if (this.powered && this.Vent.Connected && this.PPS.Connected)
            {
                //Set the power supply to the lower voltage and highest current capability.
                this.PPS.Set_Output(true, lower, 9);
                //Confirm that device is in mfgmode to prevent overcurrent and accidental shutoff
                this.Vent.CMD_Write("mfgmode");
                //Set telemetry channels
                int channelNum = this.Vent.TLMChannels["Sensor:VppoMonitor_F_mv"];
                this.Vent.CMD_Write("set vcm telemetry " + channelNum + " 0 0 0");

                this.GPIO.SetBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                success = true;
                for (int i = (int)lower; i < upper; i++)
                {
                    float high_val = (float)i * (1 + (tolerance / 100));
                    float low_val = (float)i * (1 - (tolerance / 100));
                    this.PPS.Set_Output(true, i, 9);
                    Thread.Sleep(1000);
                    response = this.Vent.CMD_Write("get vcm telemetry " + samples.ToString());

                    var matches = Regex.Matches(response, @"(?'channel'(?<=vcm\:)\s+" + channelNum.ToString() + @",)(?'counts'(\s+\d+|\s+-\d+))");

                    float ave_mVolts = 0;
                    int count = 0;
                    foreach (Match m in matches)
                    {
                        ave_mVolts += float.Parse(m.Groups[3].Value);
                        count++;
                    }
                    ave_mVolts = (ave_mVolts / count) / 1000;

                    if (success && ((ave_mVolts > low_val) && (ave_mVolts < high_val)))
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                Thread.Sleep(1000);
                this.GPIO.ClearBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);
                this.PPS.Set_Output(false);
                

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
            return success;
        }
        private bool test_batt1_diode(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;


            float tolerance = float.Parse(test.parameters["tolerance"]);
            float upper = float.Parse(test.parameters["upper"]);
            float lower = float.Parse(test.parameters["lower"]);
            int samples = int.Parse(test.parameters["samples"]);
            string response;

            if (this.powered && this.Vent.Connected && this.PPS.Connected)
            {
                //Set the power supply to the lower voltage and highest current capability.
                this.PPS.Set_Output(true, lower, 9);
                //Confirm that device is in mfgmode to prevent overcurrent and accidental shutoff
                this.Vent.CMD_Write("mfgmode");
                //Set telemetry channels
                int channelNum = this.Vent.TLMChannels["Sensor:VppoMonitor_F_mv"];
                this.Vent.CMD_Write("set vcm telemetry " + channelNum + " 0 0 0");

                this.GPIO.SetBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);
                Thread.Sleep(500);
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                success = true;
                for (int i = (int)lower; i < upper; i++)
                {
                    float high_val = (float)i * (1 + (tolerance / 100));
                    float low_val = (float)i * (1 - (tolerance / 100));
                    this.PPS.Set_Output(true, i, 9);
                    Thread.Sleep(1000);
                    response = this.Vent.CMD_Write("get vcm telemetry " + samples.ToString());

                    var matches = Regex.Matches(response, @"(?'channel'(?<=vcm\:)\s+" + channelNum.ToString() + @",)(?'counts'(\s+\d+|\s+-\d+))");

                    float ave_mVolts = 0;
                    int count = 0;
                    foreach (Match m in matches)
                    {
                        ave_mVolts += float.Parse(m.Groups[3].Value);
                        count++;
                    }
                    ave_mVolts = (ave_mVolts / count) / 1000;

                    if (success && ((ave_mVolts > low_val) && (ave_mVolts < high_val)))
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                Thread.Sleep(1000);
                this.GPIO.ClearBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);
                this.PPS.Set_Output(false);


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
            return success;
        }
        private bool test_batt2_diode(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;


            float tolerance = float.Parse(test.parameters["tolerance"]);
            float upper = float.Parse(test.parameters["upper"]);
            float lower = float.Parse(test.parameters["lower"]);
            int samples = int.Parse(test.parameters["samples"]);
            string response;

            if (this.powered && this.Vent.Connected && this.PPS.Connected)
            {
                //Set the power supply to the lower voltage and highest current capability.
                this.PPS.Set_Output(true, lower, 9);
                //Confirm that device is in mfgmode to prevent overcurrent and accidental shutoff
                this.Vent.CMD_Write("mfgmode");
                //Set telemetry channels
                int channelNum = this.Vent.TLMChannels["Sensor:VppoMonitor_F_mv"];
                this.Vent.CMD_Write("set vcm telemetry " + channelNum + " 0 0 0");

                this.GPIO.SetBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);
                Thread.Sleep(500);
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                success = true;
                for (int i = (int)lower; i < upper; i++)
                {
                    float high_val = (float)i * (1 + (tolerance / 100));
                    float low_val = (float)i * (1 - (tolerance / 100));
                    this.PPS.Set_Output(true, i, 9);
                    Thread.Sleep(1000);
                    response = this.Vent.CMD_Write("get vcm telemetry " + samples.ToString());

                    var matches = Regex.Matches(response, @"(?'channel'(?<=vcm\:)\s+" + channelNum.ToString() + @",)(?'counts'(\s+\d+|\s+-\d+))");

                    float ave_mVolts = 0;
                    int count = 0;
                    foreach (Match m in matches)
                    {
                        ave_mVolts += float.Parse(m.Groups[3].Value);
                        count++;
                    }
                    ave_mVolts = (ave_mVolts / count) / 1000;

                    if (success && ((ave_mVolts > low_val) && (ave_mVolts < high_val)))
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                Thread.Sleep(1000);
                this.GPIO.ClearBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);
                this.PPS.Set_Output(false);


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
            return success;
        }
        /******************************************************************************************************************************
         *  test_inop_led
         *  
         *  Function: Connects a load to the internal battery port and waits for the system to start charging. The Charge LED should light up and be measured by the GPIO module as a low.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             parameters: - 
         *             

         *  Returns: bool success - returns true if charge led cathode pin is low
         *  Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *                  - parameters does not contain Key
         
         ******************************************************************************************************************************/

        private bool test_inop_led(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;

            if (this.powered && this.GPIO.Connected && this.PPS.Connected && this.Vent.Connected)
            {
                int timeout = int.Parse(test.parameters["timeout"]);

                //Confirm external power is on.
                this.GPIO.SetBit(GPIO_Defs.WDOG_DIS.port, GPIO_Defs.WDOG_DIS.pin);
                


                int time = 0;
                int meas;
                do
                {
                    meas = this.GPIO.GetBit(GPIO_Defs.MEAS_INOP_LED.port, GPIO_Defs.MEAS_INOP_LED.pin);
                    if (meas == 0)
                    {
                        success = true;
                        break;
                    }
                    Thread.Sleep(50);
                    time++;
                } while (time < timeout);

                this.GPIO.ClearBit(GPIO_Defs.WDOG_DIS.port, GPIO_Defs.WDOG_DIS.pin);

                //Fill in measurement parameter
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = "PASS";
                }
            }

            return success;
        }

        /******************************************************************************************************************************
         *  test_ext_led
         *  
         *  Function: Connects a load to the internal battery port and waits for the system to start charging. The Charge LED should light up and be measured by the GPIO module as a low.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             parameters: - 
         *             

         *  Returns: bool success - returns true if charge led cathode pin is low
         *  Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *                  - parameters does not contain Key
         
         ******************************************************************************************************************************/

        private bool test_ext_led(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;

            if (this.powered && this.GPIO.Connected && this.PPS.Connected && this.Vent.Connected)
            {
                int timeout = int.Parse(test.parameters["timeout"]);

                //Confirm external power is on.
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                //Confirm that batteries are off to make sure that no charging is occurring
                this.GPIO.ClearBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);


                int time = 0;
                int meas;
                do
                {
                    meas = this.GPIO.GetBit(GPIO_Defs.MEAS_EXT_LED.port, GPIO_Defs.MEAS_EXT_LED.pin);
                    if (meas == 0)
                    {
                        success = true;
                        break;
                    }
                    Thread.Sleep(1000);
                    time++;
                } while (time < timeout);

                
                //Fill in measurement parameter
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = "PASS";
                }
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_ext_gt14_ok
         *  
         *  Function: Connects external power and pings the CPLD to retrieve a EXT_GT14V flag
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             parameters: - 
         *             

         *  Returns: bool success - returns true if the flag is set.
         *  Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *                  - parameters does not contain Key
         
         ******************************************************************************************************************************/

        private bool test_ext_gt14_ok(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;

            if (this.powered && this.GPIO.Connected &&  this.Vent.Connected)
            {
               

                //Confirm external power is on.
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                //Wait for battery to begin charging

                var response = this.Vent.CMD_Write("get vcm cpld 0c");
                int responseMatch = (((int.Parse(Regex.Matches(response, @"=(?> |)(\w+)")[0].Groups[1].Value, System.Globalization.NumberStyles.HexNumber)) & (1<<6)) >>6);
                if(responseMatch == 1)
                {
                    success = true;
                }

                //Fill in measurement parameter
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                    test.parameters["result"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = "PASS";
                    test.parameters["result"] = "PASS";
                }
                
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_vppo_ok
         *  
         *  Function: Connects external power and pings the CPLD to retrieve a EXT_GT14V flag
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             parameters: - 
         *             

         *  Returns: bool success - returns true if the flag is set.
         *  Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *                  - parameters does not contain Key
         
         ******************************************************************************************************************************/

        public bool test_vppo_ok(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;

            if (DEBUG || (this.powered && this.GPIO.Connected && this.Vent.Connected))
            {


                //Confirm external power is on.
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                

                var response = this.Vent.CMD_Write("get vcm cpld 0c");
                int responseMatch = (((int.Parse(Regex.Matches(response, @"=(?> |)(\w+)")[0].Groups[1].Value, System.Globalization.NumberStyles.HexNumber)) & (1 << 6)) >> 6);
                if (responseMatch == 1)
                {
                    success = true;
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
                    test.parameters["measured"] = "PASS";
                }
            }

            return success;
        }
        /******************************************************************************************************************************
         *  test_vppo_monitor
         *  
         *  Function: Connects external power and pings the CPLD to retrieve a vppo_ok flag
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             parameters: - 
         *             

         *  Returns: bool success - returns true if the flag is set.
         *  Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *                  - parameters does not contain Key
         
         ******************************************************************************************************************************/

        private bool test_vppo_monitor(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;


            int upper;
            int lower;
            int samples = int.Parse(test.parameters["samples"]);
            double ave_mVolts = 0;
            var count = 0;

            if (this.powered && this.Vent.Connected)
            {
                //Get limits
                

                //Set telemetry channels
                int channelNum = this.Vent.TLMChannels["Sensor:VppoMonitor_F_mv"];

                this.Vent.CMD_Write("set vcm telemetry " + channelNum + " 0 0 0");

                var response = this.Vent.CMD_Write("get vcm telemetry " + samples.ToString());

                var matches = Regex.Matches(response, @"(?'channel'(?<=vcm\:)\s+" + channelNum.ToString() + @",)(?'counts'(\s+\d+|\s+-\d+))");


                foreach (Match m in matches)
                {
                    ave_mVolts += double.Parse(m.Groups[3].Value);
                    count++;
                }
                ave_mVolts = (ave_mVolts / count) / 1000;

                if (ave_mVolts > 19)
                {
                    success = true;
                }
                //string press = output.Substring(53, 8);
                //var pressure = float.Parse(press) * 0.000980665; //Convert to kPa

                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = ave_mVolts.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = ave_mVolts.ToString();
                }

                message.Report("Measured: " + ave_mVolts.ToString());
            }
            return success;
        }
        private bool test_extdc_diode(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;


            int upper;
            int lower;
            int samples = int.Parse(test.parameters["samples"]);
            double ave_mVolts = 0;
            var count = 0;

            if (this.powered && this.Vent.Connected)
            {
                //Turn on all sources and set power supply to 15V
                this.PPS.Set_Output(true, 15, 5);
                this.GPIO.SetBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);



                //Set telemetry channels
                int channelNum = this.Vent.TLMChannels["Sensor:VppoMonitor_F_mv"];

                this.Vent.CMD_Write("set vcm telemetry " + channelNum + " 0 0 0");

                var response = this.Vent.CMD_Write("get vcm telemetry " + samples.ToString());

                var matches = Regex.Matches(response, @"(?'channel'(?<=vcm\:)\s+" + channelNum.ToString() + @",)(?'counts'(\s+\d+|\s+-\d+))");


                foreach (Match m in matches)
                {
                    ave_mVolts += double.Parse(m.Groups[3].Value);
                    count++;
                }
                ave_mVolts = (ave_mVolts / count) / 1000;

                if (ave_mVolts > 20)
                {
                    success = true;
                }
                //string press = output.Substring(53, 8);
                //var pressure = float.Parse(press) * 0.000980665; //Convert to kPa


                this.PPS.Set_Output(false);
                this.GPIO.ClearBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);

                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = ave_mVolts.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = ave_mVolts.ToString();
                }

                message.Report("Measured: " + ave_mVolts.ToString());
            }
            return success;
        }

        private bool test_cpld_diode(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            string response;
            bool ib_ok;
            bool eb1_ok;
            bool xdc_ok;
            bool eb2_ok;


            int xdc_only = 8;
            int ib_only = 1;
            int eb1_only = 4;
            int eb2_only = 2;
            


            if (this.powered && this.Vent.Connected && this.PPS.Connected && this.GPIO.Connected)
            {
                int ok = 0;

                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                Thread.Sleep(1000);
                response = this.Vent.CMD_Write("get vcm power");
                //Match the Source OK bits and confirm that the xdc bit is the only ok bit.

                var src_match = Convert.ToInt32(Regex.Match(response, @"((?<=source:\s)\d+)").Value);
                

                if(src_match == xdc_only)
                {
                    message.Report("XDC: OK");
                    ok++;
                }
                else
                {
                    message.Report("XDC: Not OK");
                }
                //Internal Battery
                this.PPS.Set_Output(true, 16, 9);
                this.GPIO.SetBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                Thread.Sleep(1000);
                response = this.Vent.CMD_Write("get vcm power");
                //Match the Source OK bits and confirm that the ib bit is the only ok bit.

                src_match = Convert.ToInt32(Regex.Match(response, @"((?<=source:\s)\d+)").Value);


                if (src_match == ib_only)
                {
                    ok++;
                    message.Report("Internal: OK");
                }
                else
                {
                    message.Report("Internal: Not OK");

                }
                //External Battery 1
                this.GPIO.SetBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);

                Thread.Sleep(1000);
                response = this.Vent.CMD_Write("get vcm power");
                //Match the Source OK bits and confirm that the eb1 bit is the only ok bit.

                src_match = Convert.ToInt32(Regex.Match(response, @"((?<=source:\s)\d+)").Value);


                if (src_match == eb1_only)
                {
                    ok++;
                    message.Report("External 1: OK");
                }
                else
                {
                    message.Report("External 1: Not OK");

                }

                //External Battery 2
                this.GPIO.SetBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);

                Thread.Sleep(1000);
                response = this.Vent.CMD_Write("get vcm power");
                //Match the Source OK bits and confirm that the eb2 bit is the only ok bit.

                src_match = Convert.ToInt32(Regex.Match(response, @"((?<=source:\s)\d+)").Value);


                if (src_match == eb2_only)
                {
                    ok++;
                    message.Report("External 2: OK");
                }
                else
                {
                    message.Report("External 2: Not OK");

                }

                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                Thread.Sleep(1000);
                this.GPIO.ClearBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);


                if (ok == 4)
                {
                    success = true;
                }
                message.Report("Measured: " + ok.ToString());
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = ok.ToString();
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = ok.ToString();
                }
            }


            return success;
        }


        /******************************************************************************************************************************
            *  test_cpld_rev
            *  
            *  Function: Queries the CPLD for the current revision.
            *  
            *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
            *             TestData test             - Variable that contains all of the necessary test data.
            *             parameters: - 
            *             

            *  Returns: bool success - returns true if charge led cathode pin is low
            *  Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
            *                  - parameters does not contain Key

            ******************************************************************************************************************************/

        private bool test_cpld_rev(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            string alpha_response;
            string number_response;
            string revision;
            string revision_meas = "";
            
            if (this.powered && this.Vent.Connected)
            {
                revision = test.parameters["rev"];

                alpha_response = this.Vent.CMD_Write("get vcm cpld 9");
                number_response = this.Vent.CMD_Write("get vcm cpld a");

                var alphaMatch = Regex.Matches(alpha_response, @"=(?> |)(\w+)");
                var numberMatch = Regex.Matches(number_response, @"=(?> |)(\w+)");

                revision_meas += (char)int.Parse(alphaMatch[0].Groups[1].Value);
                revision_meas += ((int.Parse(numberMatch[0].Groups[1].Value, System.Globalization.NumberStyles.HexNumber) & 0x78) >>3).ToString();

                

                  

                if ((revision_meas == revision))
                {
                    success = true;
                }


                //TODO: Parse meas for the correct revision


                message.Report("Measured: " + revision_meas);
                //Fill in measurement parameter
                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = revision_meas;
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = revision_meas;
                }
            }
            
            

            return success;
        }
        /******************************************************************************************************************************
         *  test_vcm_rev
         *  
         *  Function: Queries the CPLD for the current revision.
         *  
         *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
         *             TestData test             - Variable that contains all of the necessary test data.
         *             parameters: - 
         *             

         *  Returns: bool success - returns true if charge led cathode pin is low
         *  Exceptions: - The calling function is expected to catch any exceptions thrown in this function.
         *                  - parameters does not contain Key
         
         ******************************************************************************************************************************/

        private bool test_vcm_rev(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            string revision;
            string revision_meas = "";

            if (this.powered && this.Vent.Connected)
            {
                revision = test.parameters["rev"];

                var response = this.Vent.CMD_Write("get vcm version");

               

                revision_meas = Regex.Match(response, @"((?<=vcm version:\s)\d+.\d+.\d+R)").Value;

                if ((revision_meas == revision))
                {
                    //success = true;

                    //Perform the necessary blower.ctrl stuff now!
                    int ok = 0;
                    response = this.Vent.QNX_Write("/opt/vls/kvstore /fs/etfs/config/uim/datastore/deviceconfig update blower.ctrl 1");
                    Thread.Sleep(500);
                    response = this.Vent.QNX_Write("/opt/vls/kvstore /fs/etfs/config/uim/datastore/deviceconfig list");
                    if (response.Contains("blower.ctrl\t1"))
                    {
                        ok++;
                    }
                    response = this.Vent.QNX_Write("/opt/vls/kvstore /fs/sd0/config/uim/datastore/deviceconfig update blower.ctrl 1");
                    Thread.Sleep(500);
                    response = this.Vent.QNX_Write("/opt/vls/kvstore /fs/sd0/config/uim/datastore/deviceconfig list");
                    if (response.Contains("blower.ctrl\t1"))
                    {
                        ok++;
                    }
                    if(ok == 2)
                    {
                        success = true;
                    }



                }


                //TODO: Parse meas for the correct revision

                message.Report("Measured: " + revision_meas);
                test.parameters["measured"] = revision_meas;
                //Fill in measurement parameter
                if (success)
                {
                    message.Report(test.name + ": PASS");            
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                }
            }



            return success;
        }





        private bool test_supercap(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            string response;
            float measured;
            float upper;
            float lower;

            if (this.powered && this.Vent.Connected && this.DMM.Connected)
            {
                upper = float.Parse(test.parameters["upper"]);
                lower = float.Parse(test.parameters["lower"]);

                this.GPIO.SetBit(GPIO_Defs.MEAS_PIEZO.port, GPIO_Defs.MEAS_PIEZO.pin);

                


                //Disconnect from Telnet, power down the device. Then measure the super cap again
                this.Vent.Disconnect();
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                this.powered = false;

                Thread.Sleep(2000); //Wait a couple seconds, gives time for board to discharge a bit

                measured = this.DMM.Get_Volts();

                var power = this.test_power_on(message, log, 15000);
                if (power)
                {
                    Thread.Sleep(20000); //Need to wait for ip address to be collected
                    this.Vent.Connect(this.Vent._ip_address, "mfgmode", false);
                }


                this.GPIO.ClearBit(GPIO_Defs.MEAS_PIEZO.port, GPIO_Defs.MEAS_PIEZO.pin);

                
                if ((measured <= upper) && (measured >= lower))
                {
                    success = true;
                }

                test.parameters["measured"] = measured.ToString();
                message.Report("Measured: " + measured.ToString());
                //Fill in measurement parameter
                if (success)
                {
                    message.Report(test.name + ": PASS");
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                }
            }



            return success;
        }
        private bool test_sd_card(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            string response;
            float measured;
            string filename = test.parameters["filename"];
            

            if (this.powered && this.Vent.Connected)
            {
                Random rnd = new Random();
                int randNum = rnd.Next(1, 100);

                response = this.Vent.QNX_Write("ls /fs/sd0");
                int cnt = 0;
                while(response.Contains(filename) && (cnt < 50))
                {
                    response = this.Vent.QNX_Write("rm /fs/sd0/" + filename);
                    response = this.Vent.QNX_Write("ls /fs/sd0");
                    cnt++;
                }
                response = this.Vent.QNX_Write("echo \"" + randNum.ToString() + "\" >> /fs/sd0/" + filename);
                response = this.Vent.QNX_Write("cat /fs/sd0/" + filename);

                if (response.Contains(randNum.ToString()))
                {
                    success = true;
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



            return success;
        }
        private bool test_usb(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            string response;
            float measured;
            string filename = test.parameters["filename"];


            if (this.powered && this.Vent.Connected)
            {
                Random rnd = new Random();
                int randNum = rnd.Next(1, 100);

                response = this.Vent.QNX_Write("ls /fs/usb");
                int cnt = 0;
                while (response.Contains(filename) && (cnt < 50))
                {
                    response = this.Vent.QNX_Write("rm /fs/usb/" + filename);
                    response = this.Vent.QNX_Write("ls /fs/usb");
                    cnt++;
                }
                response = this.Vent.QNX_Write("echo \"" + randNum.ToString() + "\" >> /fs/usb/" + filename);
                response = this.Vent.QNX_Write("cat /fs/usb/" + filename);

                if (response.Contains(randNum.ToString()))
                {
                    success = true;
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



            return success;
        }
        /******************************************************************************************************************************
        *  test_oa1_i2c
        *  
        *  Function: Commands UUT to read from the exhalation module i2c chip
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the i2c command returns with 0 i2c errors
        *                          returns false if the i2c command returns with 1 or more i2c errors
        * 
        ******************************************************************************************************************************/
        private bool test_oa1_i2c(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int i2c_error = 0;

            if (this.powered && this.Vent.Connected)
            {

                //Command UUT to read from device Exhalation board
                var response = this.Vent.CMD_Write("get vcm cal oa1");


                
                if (!response.Contains("i2C failure"))
                {
                    success = true;
                }
                else
                {

                }

                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = i2c_error.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
        *  test_oa1_i2c
        *  
        *  Function: Commands UUT to read from the exhalation module i2c chip
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the i2c command returns with 0 i2c errors
        *                          returns false if the i2c command returns with 1 or more i2c errors
        * 
        ******************************************************************************************************************************/
        private bool test_oa2_i2c(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            int i2c_error = 0;

            if (this.powered && this.Vent.Connected)
            {

                //Command UUT to read from device Exhalation board
                var response = this.Vent.CMD_Write("get vcm cal oa2");



                if (!response.Contains("i2C failure"))
                {
                    success = true;
                }
                else
                {

                }

                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = i2c_error.ToString();
                }
            }
            return success;
        }
        /******************************************************************************************************************************
        *  test_oa1_off
        *  
        *  Function: Commands UUT to read the OA1_OFF signal and confirm that it is low.
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the test passes
        *                          returns false if the test fails
        * 
        ******************************************************************************************************************************/
        private bool test_oa1_off(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            

            if (this.powered && this.Vent.Connected)
            {

                //Command UUT to read from device Exhalation board
                var meas = this.GPIO.GetBit(GPIO_Defs.OAX1_OFF.port, GPIO_Defs.OAX1_OFF.pin);

                if (meas == 0)
                {
                    success = true;
                }

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
            return success;
        }
        /******************************************************************************************************************************
        *  test_oa1_off
        *  
        *  Function: Commands UUT to read the OA1_OFF signal and confirm that it is low.
        *  
        *  Arguments: IProgress<string> message - Variable to pass string updates back to the GUI and inform the user on what is happening.
        *             TestData test             - Variable that contains all of the necessary test data.
        *  
        *  Returns: bool success - returns true if the test passes
        *                          returns false if the test fails
        * 
        ******************************************************************************************************************************/
        private bool test_oa2_off(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;


            if (this.powered && this.Vent.Connected)
            {

                //Command UUT to read from device Exhalation board
                var meas = this.GPIO.GetBit(GPIO_Defs.OAX2_OFF.port, GPIO_Defs.OAX2_OFF.pin);

                if (meas == 0)
                {
                    success = true;
                }

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
            return success;
        }

    }
}