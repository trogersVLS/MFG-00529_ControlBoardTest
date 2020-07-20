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
using System.Windows.Forms;
using System.Configuration;

namespace ControlBoardTest
{

/*********************************************************************************************************************************************************
    * FunctionalTest - This file contains all of the test step code needed for the functional test. The program can run any combination of these test steps
    * 
    *********************************************************************************************************************************************************/
    partial class FunctionalTest
    {

        const int RELAY_DELAY = 10;    //this is for when you release the relay.  

        public bool test_dummy_test(IProgress<string> message, IProgress<string> log, TestData test)
        {
            return true;
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

            //else    //we can never get here, so I have removed it.   DLR
              //  success = false;

         
            return success;
        }

        // simulate successful power up
        public bool test_power_on()
        {
            bool success = true;
            Thread.Sleep(3000);
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
        public bool test_power_on(IProgress<string> message, IProgress<string> log)
        {
            bool success = false;

            //Determine if the devices are properly connected
            if (this.GPIO.Connected && this.SOM.Connected)
            {

                string output;
                success = Power_On(message);
                if (success)
                {
                    this.powered = true;
                    message.Report("Successfully powered up");
                }
                else
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

        bool Power_On(IProgress<string> message)
        {
            bool success = false;
            //Enables the AC power supply
            this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

            //Toggles the power button in case the device does not begin booting right away (device was successfully powered down by the power button prior to testing).
            this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);
            Thread.Sleep(500);
            this.GPIO.ClearBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);

            message.Report("Powering up ... ");

            string output;
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
                    success = true;
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
            bool success = false;

            //Blocking until user input is given --> Possible options are: "yes", or "no" 
            success = this.PromptUser_YesNo("Are LCD colors correct and display free of defects?", test.name);

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
                
                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.MEAS_3V3_HOT_EN.port, GPIO_Defs.MEAS_3V3_HOT_EN.pin);

                message.Report("Measured: " + measured.ToString() + " V");

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

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.MEAS_5V0_HOT_EN.port, GPIO_Defs.MEAS_5V0_HOT_EN.pin);

                message.Report("Measured: " + measured.ToString() + " V");

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
                
                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.MEAS_5V3_EN.port, GPIO_Defs.MEAS_5V3_EN.pin);

                message.Report("Measured: " + measured.ToString() + " V");

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

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.MEAS_12V0_EN.port, GPIO_Defs.MEAS_12V0_EN.pin);

                message.Report("Measured: " + measured.ToString() + " V");

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

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.MEAS_3V3_EN.port, GPIO_Defs.MEAS_3V3_EN.pin);

                message.Report("Measured: " + measured.ToString() + " V");

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

                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.MEAS_1V2_EN.port, GPIO_Defs.MEAS_1V2_EN.pin);

                message.Report("Measured: " + measured.ToString() + " V");

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
               
                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.MEAS_VREF_EN.port, GPIO_Defs.MEAS_VREF_EN.pin);

                message.Report("Measured: " + measured.ToString() + " V");

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
                
                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.MEAS_3V3A_EN.port, GPIO_Defs.MEAS_3V3A_EN.pin);

                message.Report("Measured: " + measured.ToString() + " V");

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
                
                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.MEAS_30V_EN.port, GPIO_Defs.MEAS_30V_EN.pin);

                message.Report("Measured: " + measured.ToString() + " V");

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
                
                //Measure the voltage and disconnect DMM
                float measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.MEAS_36V_EN.port, GPIO_Defs.MEAS_36V_EN.pin);

                message.Report("Measured: " + measured.ToString() + " V");

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

                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_FREQ_BLOWER.port, GPIO_Defs.MEAS_FREQ_BLOWER.pin);
                
                //Measure the frequency with the DMM.  It can do this.  DLR
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

                //Measure value
                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.MEAS_FREQ_PUMP.port, GPIO_Defs.MEAS_FREQ_PUMP.pin);
                
                //Measure the frequency
                Thread.Sleep(2000); //Wait for the motor to get up to speed
                measured = this.DMM.Get_Freq(); //Convert to RPM
                measured = measured * 6;

                if ((measured > upper) || (measured < lower))//something may have gone wrong.  Give it another try and toss out the old measurement 
                {
                    Thread.Sleep(1000); //Wait for the motor to get up to speed
                    measured = this.DMM.Get_Freq(); //Convert to RPM
                    measured = measured * 6;//scale the measurement to what we want  
                }

                this.GPIO.ClearBit(GPIO_Defs.MEAS_FREQ_PUMP.port, GPIO_Defs.MEAS_FREQ_PUMP.pin);
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
                Thread.Sleep(1000); // Waiting for solenoid to do something

                //Measure the voltage                
                measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV1N_EN.port, GPIO_Defs.MEAS_O2_SV1N_EN.pin);

                this.Vent.CMD_Write("set vcm sv 9 0");


                message.Report("Measured: " + measured.ToString() + " V");

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
                Thread.Sleep(1000); // Waiting for solenoid to do something

                //Measure the voltage                
                measured = this.DMM.Get_Volts();

                //this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.pin);

                this.Vent.CMD_Write("set vcm sv 10 0");


                message.Report("Measured: " + measured.ToString() + " V");

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

                //Measure the voltage
                measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV1N_EN.port, GPIO_Defs.MEAS_O2_SV1N_EN.pin);

                this.Vent.CMD_Write("set vcm sv 9 0");

                message.Report("Measured: " + measured.ToString() + " V");

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

                //Measure the voltage
                measured = this.DMM.Get_Volts();

                this.GPIO.ClearBit(GPIO_Defs.EXT_O2_DIS.port, GPIO_Defs.EXT_O2_DIS.pin);
                this.GPIO.ClearBit(GPIO_Defs.MEAS_O2_SV2N_EN.port, GPIO_Defs.MEAS_O2_SV2N_EN.pin);

                this.Vent.CMD_Write("set vcm sv 10 0");

                message.Report("Measured: " + measured.ToString() + " V");

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
           
            //we will test the cough valve in 3 positions. Recall lthat we don't know where the valve left off of last time.  So two possible outcomes after 2 cycles are possible. 
            int pos1 = 0;
            int pos2 = 0;

            int pos1a = 0;
            int pos2a = 0;

            byte flag = 0;

            int value;


            message.Report(test.name + " Test cough valve position 0");

            //examine the at rest leftover positions of the cough valve
            pos1 = this.GPIO.GetBit(GPIO_Defs.COUGH_POS1.port, GPIO_Defs.COUGH_POS1.pin);
            pos2 = this.GPIO.GetBit(GPIO_Defs.COUGH_POS2.port, GPIO_Defs.COUGH_POS2.pin);
            
            //now try to move the valve with a command string 0.  This is arbitrary.  we could start with command string 1, it doesn't matter 

            this.Vent.CMD_Write("set vcm coughv 0");//this may or may not move the valve, depending upon where it left off.   
            Thread.Sleep(150); //Value doesn't really matter, the device doesn't drive the valve any faster
            pos1a = this.GPIO.GetBit(GPIO_Defs.COUGH_POS1.port, GPIO_Defs.COUGH_POS1.pin);
            pos2a= this.GPIO.GetBit(GPIO_Defs.COUGH_POS2.port, GPIO_Defs.COUGH_POS2.pin);

            //now, if they moved they will change state, this is good.  If they don't move then try the other command  and see if they move. 

            if ((pos1 == pos1a) && (pos2 == pos2a))
            {//valve did not move with a  command string 0 so try command string 1
                flag = 1;  //so we know what way to try next time around 
                this.Vent.CMD_Write("set vcm coughv 1");
                Thread.Sleep(150); //Value doesn't really matter, the device doesn't drive the valve any faster
                pos1a = this.GPIO.GetBit(GPIO_Defs.COUGH_POS1.port, GPIO_Defs.COUGH_POS1.pin);
                pos2a = this.GPIO.GetBit(GPIO_Defs.COUGH_POS2.port, GPIO_Defs.COUGH_POS2.pin);

                if ((pos1 != pos1a) && (pos2 != pos2a))  //aha, they changed state so the valve had to move one direction on the second try . 
                    success = true; 
            }

            else if ((pos1 != pos1a) && (pos2 != pos2a))  //the valve moved with the first try, this is good too 
                success = true;



            if (success)//no point in continuing if the valve failed 
            { //now try the other state.  If we made it here then we managed to get the valve to move one way.  Need to check both directions. 
                message.Report(test.name + " Test cough valve position 1");

                pos1 = this.GPIO.GetBit(GPIO_Defs.COUGH_POS1.port, GPIO_Defs.COUGH_POS1.pin);
                pos2 = this.GPIO.GetBit(GPIO_Defs.COUGH_POS2.port, GPIO_Defs.COUGH_POS2.pin);

                if (flag == 1)
                {
                    this.Vent.CMD_Write("set vcm coughv 0");
                    Thread.Sleep(150); //Value doesn't really matter, the device doesn't drive the valve any faster
                }

                if (flag == 0)
                {
                    this.Vent.CMD_Write("set vcm coughv 1");
                    Thread.Sleep(150); //Value doesn't really matter, the device doesn't drive the valve any faster
                }

                pos1a = this.GPIO.GetBit(GPIO_Defs.COUGH_POS1.port, GPIO_Defs.COUGH_POS1.pin);
                pos2a = this.GPIO.GetBit(GPIO_Defs.COUGH_POS2.port, GPIO_Defs.COUGH_POS2.pin);

                if ((pos1 != pos1a) && (pos2 != pos2a))  //the valve moved with the second try, this is good too 
                    success = true;
            }


            value = 10*(pos1 + pos1a) + pos2 + pos2a;    
            
            //Fill in measurement parameter
            if (success)
            {
                message.Report(test.name + ": PASS at " + value.ToString());
                test.parameters["measured"] ="PASS";
            }
            else
            {
                message.Report(test.name + ": FAIL at " + value.ToString());
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
            //string str_value;
            //bool value_available;
            bool success = false;

            // Get parameters from test data object
            float upper = float.Parse(test.parameters["upper"]);
            float lower = float.Parse(test.parameters["lower"]);
                                

            //Connect the desired voltage node to the DMM
            this.GPIO.SetBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);

            //Measure the voltage
            float measured = this.DMM.Get_Volts();

            this.GPIO.ClearBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);

            //string val;

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
            int i = 0;

            if (this.powered && this.Vent.Connected && this.DMM.Connected && this.GPIO.Connected)
            {
                float upper = float.Parse(test.parameters["upper"]);
                float lower = float.Parse(test.parameters["lower"]);

                //this.NotifyUser("Please clear all alarms");
                this.Vent.CMD_Write("restart");
                
                this.SetDutSettings(ConfigurationManager.AppSettings["EXT_O2_SETTINGSPATH"]);
                Thread.Sleep(3000); //Wait 1 second for changes to take effect
                //this.Vent.CMD_Write("set uim screen 5045");  //Nebulizer start screenID = 5039 --> Change to Oxygen screen and start extO2

                //this.NotifyUser("Please press START");

                //Determine if the nebulizer needs to be running.
                //string returnVal = this.Vent.CMD_Write("get vcm monitors");
                //if (returnVal.Contains("nebulizerActive: 0"))
                //{                    
                //    do
                //    {
                //        Thread.Sleep(500);
                //        i++;
                //        returnVal = this.Vent.CMD_Write("get vcm monitors");
                //    }
                //    while (returnVal.Contains("nebulizerActive: 0") && (i < 30));
                //}

                //Connect the desired voltage node to the DMM
                this.GPIO.SetBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);

                //Measure the voltage
                float v_measured = this.DMM.Get_Volts();

                if ((v_measured > lower) && (v_measured < upper))
                {
                    success = true;
                }

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

                //Done with high speed fan mode - time to shut it down.                 
                //We will introduce a fan fault to force the nebulizer to stop and measure the fan voltage to see that it worked
                //this.GPIO.SetBit(GPIO_Defs.FAN_FAULT_EN.port, GPIO_Defs.FAN_FAULT_EN.pin);

                //i = 0;
                //do
                //{
                //    i++;
                //    v_measured = this.DMM.Get_Volts();
                //}
                //while (v_measured > lower && (i < 20));

                //this.GPIO.ClearBit(GPIO_Defs.FAN_FAULT_EN.port, GPIO_Defs.FAN_FAULT_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.VFAN_MEAS_EN.port, GPIO_Defs.VFAN_MEAS_EN.pin);

                //TAR Added a restart to stop high speed fan
                this.SetDutSettings(ConfigurationManager.AppSettings["LOW_VOL_SETTINGSPATH"]);
                this.Vent.CMD_Write("mfgmode");
                //this.Vent.CMD_Write("resart");
                //this.Vent.CMD_Write("mfgmode");
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

            //Measure the voltage
            float measured = this.DMM.Get_Freq();

            this.GPIO.ClearBit(GPIO_Defs.FAN_FREQ_MEAS_EN.port, GPIO_Defs.FAN_FREQ_MEAS_EN.pin);

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

        ******************************************************************************************************************************/

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
                Thread.Sleep(RELAY_DELAY);
                output = this.Vent.CMD_Write("get vcm buttons");
                this.GPIO.ClearBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);
                Thread.Sleep(RELAY_DELAY);

                buttonStateMatches = Regex.Matches(output, @"(?'button'\s+\w+,)(?'state'\s+\d)(?'falling',\s+\w+\s\w+,)(?'fState'\s+\d)(?'rising',\s+\w+\s\w+)(?'rState'\s+\d)");

                //Build binary num representing measured data.
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
                Thread.Sleep(RELAY_DELAY);
                output = this.Vent.CMD_Write("get vcm buttons");
                this.GPIO.ClearBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);
                Thread.Sleep(RELAY_DELAY);


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
                       

            this.GPIO.SetBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);
            Thread.Sleep(RELAY_DELAY);
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
        /******************************************************************************************************************************/
        //
        //
        //
        //
        //
        /******************************************************************************************************************************/
        //
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
         * test_microphone
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
            int meas;
            int i = 0;
            bool success = false;

            if (this.powered && this.Vent.Connected && this.GPIO.Connected)
            {
                //Toggles the power button in case the piezo is doing its constant beep pattern
                this.GPIO.SetBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);
                Thread.Sleep(500);
                this.GPIO.ClearBit(GPIO_Defs.PB_BTN_ON.port, GPIO_Defs.PB_BTN_ON.pin);

                //Enable the fan fault to force a quick error
                this.GPIO.SetBit(GPIO_Defs.FAN_FAULT_EN.port, GPIO_Defs.FAN_FAULT_EN.pin);

                message.Report("Forcing CPLD Alarm");
                this.Vent.CMD_Write("set vcm cpld 0a 5");
                              
                //turn on the speaker and piezo
                this.GPIO.SetBit(GPIO_Defs.PIEZO_EN.port, GPIO_Defs.PIEZO_EN.pin);
                this.GPIO.SetBit(GPIO_Defs.SPKR_EN.port, GPIO_Defs.SPKR_EN.pin);

                // set loop to check the piezo for 10 seconds
                i = 0;
                do
                {
                    meas = this.GPIO.GetBit(GPIO_Defs.MEAS_PIEZO_N.port, GPIO_Defs.MEAS_PIEZO_N.pin);

                    // leave the loop as soon as the Piezo is off
                    if (meas == 1)
                    {
                        Thread.Sleep(5000);
                        meas = this.GPIO.GetBit(GPIO_Defs.MEAS_PIEZO_N.port, GPIO_Defs.MEAS_PIEZO_N.pin);
                        //message.Report("MEAS_PIEZO_N = " + meas.ToString());
                        if (meas == 1)
                        {
                            success = true;
                            break;
                        }
                    }
                    Thread.Sleep(500);
                    i++;
                } while (i < 40);
           
                this.GPIO.ClearBit(GPIO_Defs.SPKR_EN.port, GPIO_Defs.SPKR_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.FAN_FAULT_EN.port, GPIO_Defs.FAN_FAULT_EN.pin);
                this.GPIO.ClearBit(GPIO_Defs.PIEZO_EN.port, GPIO_Defs.PIEZO_EN.pin);
                               
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

            //Disable the speaker
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
        ******************************************************************************************************************************/

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

                int meas;
                int i = 0;

                do
                {
                    meas = this.GPIO.GetBit(GPIO_Defs.MEAS_PIEZO_N.port, GPIO_Defs.MEAS_PIEZO_N.pin);

                    if (meas == 0)
                    {
                        success = true;
                        break;
                    }
                    Thread.Sleep(100);
                    i++;
                } while (i < 50);

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
            //int i2c_error = 0;
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


                //Fill in measurement parameter
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
            int DELAY = 500;

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

                if (!success)
                {
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
        *  test_rotary_valve_2
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
                if (!success)//then try again  
                {
                    if (measured == 0)
                    {
                        //Starting off good, the home flag is not set. Now move 


                        response = this.Vent.CMD_Write("set vcm rotaryv 2 4");
                        Thread.Sleep(DELAY);
                        response = this.Vent.CMD_Write("set vcm rotaryv 2 0");
                        Thread.Sleep(DELAY);
                        measured = this.GPIO.GetBit(GPIO_Defs.MEAS_RV2_HOME.port, GPIO_Defs.MEAS_RV2_HOME.pin);
                        if (measured == 1) success = true;


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
                // Set DUT to Alarm screen and clear errors (needed for the single test mode)
                this.Vent.CMD_Write("set uim screen 5023");
                this.NotifyUser("Please clear all alarms before proceeding");

                //Toggle Alarm Silence Button
                this.GPIO.SetBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);
                Thread.Sleep(100);
                this.GPIO.ClearBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);

                //Alarm Silence LED toggles at 2Hz --> Will sample to get a reasonable capture of the value
                var fs = int.Parse(test.parameters["fs"]);
                var time = int.Parse(test.parameters["time"]);

                int[] ledFlash = new int[fs * time]; // List to hold all of
                for(int i = 0; i < (fs*time); i++)
                {
                    ledFlash[i] = this.GPIO.GetBit(GPIO_Defs.MEAS_AS_LED.port, GPIO_Defs.MEAS_AS_LED.pin);

                    if (!(ledFlash[i] == 1 || ledFlash[i] == 0))
                        message.Report("Something is wrong with GPIO.GetBit()");
                        
                    Thread.Sleep(1000 / fs);
                }

                if (ledFlash.Contains(0) && ledFlash.Contains(1))
                {
                    //Get average
                    var average = ledFlash.Average();
                    message.Report("Average = " + average);
                    if (average >= .3 && average <= .7)                    
                        success = true;                        
                }

                //Toggle Alarm Silence Button
                this.GPIO.SetBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);
                Thread.Sleep(100);
                this.GPIO.ClearBit(GPIO_Defs.AS_BTN_ON.port, GPIO_Defs.AS_BTN_ON.pin);

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

/****************************************************************************************************/
//
//
//
//
/****************************************************************************************************/


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
            //int retry = 0;
            string powerOutput = "";
            //string matchPattern = @"(?'battery'^\s+[a-zA-Z0-9]+:)(?'present'\s+\d+,)(?'charge'\s+\d+,)(?'err'\s+\d+,)(?'RSOC'\s+\d+,)(?'ASOC'\s+\d+,)(?'temp'\s+\d+)";

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
                } while ((powerOutput == "") || (retry < 3)); //TODO: I think that this is a bug. Should be --> ((powerOutput == "") && (retry <3))



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
                Thread.Sleep(250); // wanta little more than just the standard relay delay
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                Thread.Sleep(RELAY_DELAY);

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
                Thread.Sleep(RELAY_DELAY);
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
                Thread.Sleep(RELAY_DELAY);

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
                Thread.Sleep(RELAY_DELAY);
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
                Thread.Sleep(RELAY_DELAY);

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
                Thread.Sleep(RELAY_DELAY);
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
                Thread.Sleep(RELAY_DELAY);

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
                Thread.Sleep(RELAY_DELAY);


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
                this.GPIO.SetBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);  //this connects BATT+ from PPS to DVM_LO in preparation for measuring current 
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.SetBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);//this connectes the real main battery positive terminal to AMM_HI input on DVM
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.SetBit(GPIO_Defs.TEMP_BATT0.port, GPIO_Defs.TEMP_BATT0.pin);//enable the temeperature sense circuit and route it to the correct battery interface.  Charger will not operate if temperature is out of range. 
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.SetBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);//this routes BATT+ from the PPS  and DVM_LO out to the battery connector on the UUT. 

                float meas;
                int time = 0;


                //DLR if we measure current at this stage that is > upper range, then shut down immediately, something went very wrong and we don't want to damage a board. 

                do
                {
                    meas = this.DMM.Get_Amps() * -1;//grab the current and swap the polarity.

                    if (Math.Abs(meas) > upper)//something bad could be happening here.   DLR 
                    {
                        message.Report("Critical Error, charger current.  Halting internal battery test at CN302");
                        success = false;
                        break;

                    }

                    if ((meas <= upper) && (meas >= lower))
                    {
                        success = true;
                        break;
                    }
                    Thread.Sleep(1000);
                    time++;
                } while (time < timeout);

                //Turn off PPS and battery
                this.GPIO.ClearBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);//this will disconnect the battery connector on the UUT.   This should happen first.  The 
                //order of removal of the remaining charger connections is non-critical. 
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.ClearBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.ClearBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.ClearBit(GPIO_Defs.TEMP_BATT0.port, GPIO_Defs.TEMP_BATT0.pin);


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
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.SetBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.SetBit(GPIO_Defs.TEMP_BATT1.port, GPIO_Defs.TEMP_BATT1.pin);
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.SetBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);

                float meas;
                int time = 0;



                do
                {
                    meas = this.DMM.Get_Amps() * -1;

                    if (Math.Abs(meas) > upper)//something bad could be happening here.   DLR 
                    {
                        message.Report("Critical Error, charger current.  Halting External battery test at CN301m");
                        success = false;
                        break;

                    }

                    if ((meas <= upper) && (meas >= lower))
                    {
                        success = true;
                        break;
                    }
                    Thread.Sleep(1000);
                    time++;
                } while (time < timeout);

                //Turn off PPS and battery
                this.GPIO.ClearBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.ClearBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.ClearBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.ClearBit(GPIO_Defs.TEMP_BATT1.port, GPIO_Defs.TEMP_BATT1.pin);


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
         *  
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
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.SetBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.SetBit(GPIO_Defs.TEMP_BATT2.port, GPIO_Defs.TEMP_BATT2.pin);
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.SetBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);

                float meas;
                int time = 0;


                do
                {
                    meas = this.DMM.Get_Amps() * -1;

                    if (Math.Abs(meas) > upper)//something bad could be happening here.   DLR 
                    {
                        message.Report("Critical Error, charger current.  Halting External battery test at CN300m");
                        success = false;
                        break;

                    }

                    if ((meas <= upper) && (meas >= lower))
                    {
                        success = true;
                        break;
                    }
                    Thread.Sleep(1000);
                    time++;
                } while (time < timeout);

                //Turn off PPS and battery

                this.GPIO.ClearBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.ClearBit(GPIO_Defs.AMM_EN.port, GPIO_Defs.AMM_EN.pin);
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.ClearBit(GPIO_Defs.CHG_LOAD_EN.port, GPIO_Defs.CHG_LOAD_EN.pin);
                Thread.Sleep(10); //dlr for relay settling time 
                this.GPIO.ClearBit(GPIO_Defs.TEMP_BATT2.port, GPIO_Defs.TEMP_BATT2.pin);


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
            int MAX_ATTEMPTS = 3;
            bool success = false;

            float upper = float.Parse(test.parameters["upper"]);
            float lower = float.Parse(test.parameters["lower"]);

            if (this.powered && this.Vent.Connected && this.GPIO.Connected && this.DMM.Connected)
            {

                ////////////////////////////////////////////////////////////////////////////////
                // New testing methodology will apply 24V to C at all times.

                
                float NurseCall_NC;
                float NurseCall_NO;
                float NurseCall_NC_Active;
                float NurseCall_NO_Active;
                //Forces the relay into an active state
                this.Vent.CMD_Write("set vcm cpld 0a 5");

                // Relay is in the active state. Expect NC to open and NO to be closed. 
                    // NC = 0V
                    // NO = 24V
                this.GPIO.SetBit(GPIO_Defs.MEAS_NC_NC.port, GPIO_Defs.MEAS_NC_NC.pin);
                NurseCall_NC_Active = this.DMM.Get_Volts();
                //MessageBox.Show(string.Format("Normally closed active = {0}", NurseCall_NC_Active.ToString()));
                this.GPIO.ClearBit(GPIO_Defs.MEAS_NC_NC.port, GPIO_Defs.MEAS_NC_NC.pin);

                this.GPIO.SetBit(GPIO_Defs.MEAS_NC_NO.port, GPIO_Defs.MEAS_NC_NO.pin);
                NurseCall_NO_Active = this.DMM.Get_Volts();
                //MessageBox.Show(string.Format("Normally open active = {0}", NurseCall_NO_Active.ToString()));
                this.GPIO.ClearBit(GPIO_Defs.MEAS_NC_NO.port, GPIO_Defs.MEAS_NC_NO.pin);

                message.Report("Relay ON:");
                message.Report("NC measured: " + NurseCall_NC_Active.ToString());
                message.Report("NO measured: " + NurseCall_NO_Active.ToString());
                

                //Clear the alarms to get the device back into a non-alarming state.
                this.Vent.CMD_Write("set uim screen 5023");
                this.NotifyUser("Please clear all alarms before proceeding");
                int attempt = 0;
                do
                {
                    //Force CPLD to clear the relay.
                    this.Vent.CMD_Write("set vcm cpld 0a 0");

                    // Relay is in the inactive state. Expect NC to closed and NO to be open. 
                    // NC = 24V
                    // NO = 0V
                    this.GPIO.SetBit(GPIO_Defs.MEAS_NC_NC.port, GPIO_Defs.MEAS_NC_NC.pin);
                    NurseCall_NC = this.DMM.Get_Volts();
                    //MessageBox.Show(string.Format("Normally closed OFF = {0}", NurseCall_NC.ToString()));

                    this.GPIO.ClearBit(GPIO_Defs.MEAS_NC_NC.port, GPIO_Defs.MEAS_NC_NC.pin);

                    this.GPIO.SetBit(GPIO_Defs.MEAS_NC_NO.port, GPIO_Defs.MEAS_NC_NO.pin);
                    NurseCall_NO = this.DMM.Get_Volts();
                    //MessageBox.Show(string.Format("Normally open = {0}", NurseCall_NO.ToString()));
                    this.GPIO.ClearBit(GPIO_Defs.MEAS_NC_NO.port, GPIO_Defs.MEAS_NC_NO.pin);

                    message.Report("Relay OFF:");
                    message.Report("NC measured: " + NurseCall_NC.ToString());
                    message.Report("NO measured: " + NurseCall_NO.ToString());

                    if ((NurseCall_NC > upper) && (NurseCall_NC_Active < lower) && (NurseCall_NO < lower) && (NurseCall_NO_Active > upper))
                    {   
                        success = true;
                        break;
                    }
                    attempt++;

                }
                while (attempt < MAX_ATTEMPTS);

                if (success)
                {
                    message.Report(test.name + ": PASS");
                    test.parameters["measured"] = "PASS";
                    //success = true;
                }
                else
                {
                    message.Report(test.name + ": FAIL");
                    test.parameters["measured"] = "FAIL";
                }                 
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
                this.PPS.Set_Output(true, lower, 7);
                //Confirm that device is in mfgmode to prevent overcurrent and accidental shutoff
                this.Vent.CMD_Write("mfgmode");
                //Set telemetry channels
                int channelNum = this.Vent.TLMChannels["Sensor:VppoMonitor_F_mv"];
                this.Vent.CMD_Write("set vcm telemetry " + channelNum + " 0 0 0");

                this.GPIO.SetBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);
                Thread.Sleep(RELAY_DELAY);
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                success = true;
                for (int i = (int)lower; i < upper; i++)
                {
                    float high_val = (float)i * (1 + (tolerance / 100));
                    float low_val = (float)i * (1 - (tolerance / 100));
                    this.PPS.Set_Output(true, i, 7);
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
                this.PPS.Set_Output(true, lower, 7);
                //Confirm that device is in mfgmode to prevent overcurrent and accidental shutoff
                this.Vent.CMD_Write("mfgmode");
                //Set telemetry channels
                int channelNum = this.Vent.TLMChannels["Sensor:VppoMonitor_F_mv"];
                this.Vent.CMD_Write("set vcm telemetry " + channelNum + " 0 0 0");

                this.GPIO.SetBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);
                Thread.Sleep(RELAY_DELAY);
                this.GPIO.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

                success = true;
                for (int i = (int)lower; i < upper; i++)
                {
                    float high_val = (float)i * (1 + (tolerance / 100));
                    float low_val = (float)i * (1 - (tolerance / 100));
                    this.PPS.Set_Output(true, i, 7);
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
                this.PPS.Set_Output(true, lower, 7);
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
                    this.PPS.Set_Output(true, i, 7);
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
                    Thread.Sleep(10);
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
                    test.parameters["measured"] = "FAIL";
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
                Thread.Sleep(RELAY_DELAY);

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
                    test.parameters["measured"] = "FAIL";
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
                    test.parameters["measured"] = "FAIL";
                    test.parameters["result"] = "FAIL";
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
                    test.parameters["measured"] = "FAIL";
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


            //int upper;
           // int lower;
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


        /******************************************************************************************************************/

        private bool test_extdc_diode(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;


           // int upper;
           // int lower;
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


        /******************************************************************************************************************/


        private bool test_cpld_diode(IProgress<string> message, IProgress<string> log, TestData test)
        {
            bool success = false;
            string response;
            //bool ib_ok;
            //bool eb1_ok;
            //bool xdc_ok;
            //bool eb2_ok;


            int xdc_only = 8;
            int ib_only = 1;
            int eb1_only = 4;
            int eb2_only = 2;
            


            if (this.powered && this.Vent.Connected && this.PPS.Connected && this.GPIO.Connected)
            {
                int ok = 0;

                //Confirm external power is on.
                this.GPIO.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);
                response = this.Vent.CMD_Write("get vcm power");
                Thread.Sleep(100);

                // Sometimes we get garbage data from the first read --> TODO: Seriously got to fix this nonsense.
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
                    message.Report("XDC: Not OK (Source = " + src_match.ToString() + ")");
                }
                //Internal Battery
                this.PPS.Set_Output(true, 16, 7);
                this.GPIO.SetBit(GPIO_Defs.BAT0_EN.port, GPIO_Defs.BAT0_EN.pin);
                Thread.Sleep(RELAY_DELAY);
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
                    message.Report("Internal: Not OK (Source = " + src_match.ToString() + ")");

                }
                //External Battery 1
                this.GPIO.SetBit(GPIO_Defs.BAT1_EN.port, GPIO_Defs.BAT1_EN.pin);
                Thread.Sleep(RELAY_DELAY);
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
                    message.Report("External 1: Not OK (Source = " + src_match.ToString() + ")");

                }

                //External Battery 2
                this.GPIO.SetBit(GPIO_Defs.BAT2_EN.port, GPIO_Defs.BAT2_EN.pin);
                Thread.Sleep(RELAY_DELAY);
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
                    message.Report("External 2: Not OK (Source = " + src_match.ToString() + ")");

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
           // string response;
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

                //var power = this.test_power_on(message, log, 15000);
                //if (power)
                //{
                //    Thread.Sleep(20000); //Need to wait for ip address to be collected
                //    this.Vent.Connect(this.Vent._ip_address, "mfgmode", false);
                //}


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
           // float measured;
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
                var qnxpresent = this.Vent.QNX_Write("ls /fs/sd0/");

                if (response.Contains(randNum.ToString()) && qnxpresent.Contains("qnxifs"))
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
            //float measured;
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
                    response = this.Vent.QNX_Write("ls /fs/usb/");
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