/**********************************************************************************************
 * Program.cs
 * - Main Entry for Control Board Functional Test Program
 * 
 * Author: Taylor Rogers
 * Date: 10/23/2019
 * 
 **********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using GPIO;
using System.IO.Ports;
using VLS;
using System.IO;
using MccDaq;
using System.Diagnostics;

namespace ControlBoardTest
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            int ms = 500;
            GUIConsoleWriter log_console = new GUIConsoleWriter();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //TestPowerRelayA(ms);
            //TestSignalRelayA(ms);
            //TestSignalRelayB(ms);
            //TestSignalRelayC(ms);
            //TestSignalRelayD(ms);
            //TestUSBSwitch(ms, 3);
            //TestTelemetry(log_console);
            //TestGPIO();


            //TestPPS();

            //TestFCT();



            Application.Run(new MainForm(log_console));


        }



        /* *******************************************************************
         * Tests
         * 
         * 
         *********************************************************************/
         
        private static void TestGPIO()
        {
            MccDaq_GPIO gpio = new MccDaq_GPIO();


            gpio.SetBit(GPIO_Defs.SPKR_EN.port, GPIO_Defs.SPKR_EN.pin);
            gpio.SetBit(GPIO_Defs.FAN_FAULT_EN.port, GPIO_Defs.FAN_FAULT_EN.pin);
            gpio.SetBit(GPIO_Defs.PIEZO_EN.port, GPIO_Defs.PIEZO_EN.pin);

            gpio.ClearBit(GPIO_Defs.SPKR_EN.port, GPIO_Defs.SPKR_EN.pin);
            gpio.ClearBit(GPIO_Defs.FAN_FAULT_EN.port, GPIO_Defs.FAN_FAULT_EN.pin);
            gpio.ClearBit(GPIO_Defs.PIEZO_EN.port, GPIO_Defs.PIEZO_EN.pin);


        }
        private static void TestFCT()
        {
            VLS_Tlm Vent = new VLS_Tlm("10.10.2.101");
            MccDaq_GPIO Gpio = new MccDaq_GPIO();
            Test_Equip DMM = new Test_Equip();
            Test_Equip PPS = new Test_Equip();
            VOCSN_Serial SOM = new VOCSN_Serial();
            //IProgress(string) message = new I


            //if (Vent.Connect())
            //{
            //    FunctionalTest FCT = new FunctionalTest(Gpio, DMM, PPS, SOM, Vent, true);

            //    FCT.test_vppo_ok()

            //}


        }
        private static void TestPPS()
        {
            Test_Equip pps = new Test_Equip("HEWLETT", "RS232", 1200, 2, "COM3");
            pps.Connect();

            pps.PPS_Init();


            for(int i = 1; i <= 20; i++)
            {
                for(int j = 1; j <= 10; j++)
                {
                    pps.Set_Output(true, i, j);
                    Thread.Sleep(200);
                }
            }
            pps.Set_Output(false);
          



            Application.Exit();
        }
        private static void TestTelemetry(GUIConsoleWriter console)
        {

            VLS_Tlm Vent = new VLS_Tlm("");
            string _ip_address = Microsoft.VisualBasic.Interaction.InputBox("Enter IP Address");
            Vent.Connect(_ip_address, "mfgmode", false);

            if (Vent.Connected)
            {
                //Vent.CMD_Write("mfgmode");
                test_cpld_rev(Vent);
            
            }



            Application.Exit();
        }

        private static void test_cpld_rev(VLS_Tlm vent)
        {
            bool success = false;
            string response;
            string revision;
            string revision_meas;

        
           

            response = vent.CMD_Write("get vcm cpld 9");

            Debug.WriteLine(response[20]);



            
        }

        private static void TestPower()
        {
            MccDaq_GPIO test = new MccDaq_GPIO();

            test.SetBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

            int x = 9;
            test.ClearBit(GPIO_Defs.AC_EN.port, GPIO_Defs.AC_EN.pin);

            return;

        }

        private static void TestPowerRelayA(int ms)
        {
            MccDaq_GPIO test = new MccDaq_GPIO();

            test.SetPort_Output(DigitalPortType.FirstPortA);

            for (int i = 0; i < 8; i++)
            {
                if (i != 5)
                {
                    test.SetBit(DigitalPortType.FirstPortA, i);
                    Thread.Sleep(ms);
                    //test.ClearBit(DigitalPortType.FirstPortA, i);
                }
            }
        }
        private static void TestSignalRelayA(int ms)
        {
            MccDaq_GPIO test = new MccDaq_GPIO();

            test.SetPort_Output(DigitalPortType.FirstPortB);

            for (int i = 0; i < 8; i++)
            {
                test.SetBit(DigitalPortType.FirstPortB, i);
                Thread.Sleep(ms);
                //test.ClearBit(DigitalPortType.FirstPortB, i);
            }
        }
        private static void TestSignalRelayB(int ms)
        {
            MccDaq_GPIO test = new MccDaq_GPIO();

            test.SetPort_Output(DigitalPortType.FirstPortCL);
            test.SetPort_Output(DigitalPortType.FirstPortCH);

            for (int i = 0; i < 4; i++)
            {
                test.SetBit(DigitalPortType.FirstPortCL, i);
                Thread.Sleep(ms);
                test.ClearBit(DigitalPortType.FirstPortCL, i);
            }
            for (int i = 0; i < 4; i++)
            {
                test.SetBit(DigitalPortType.FirstPortCH, i);
                Thread.Sleep(ms);
                test.ClearBit(DigitalPortType.FirstPortCH, i);
            }
        }
        private static void TestSignalRelayC(int ms)
        {
            MccDaq_GPIO test = new MccDaq_GPIO();

            test.SetPort_Output(DigitalPortType.SecondPortA);

            for (int i = 0; i < 8; i++)
            {
                test.SetBit(DigitalPortType.SecondPortA, i);
                Thread.Sleep(ms);
                test.ClearBit(DigitalPortType.SecondPortA, i);
            }
        }
        private static void TestSignalRelayD(int ms)
        {
            MccDaq_GPIO test = new MccDaq_GPIO();

            test.SetPort_Output(DigitalPortType.SecondPortCL);
            test.SetPort_Output(DigitalPortType.SecondPortCH);

            for (int i = 0; i < 4; i++)
            {
                test.SetBit(DigitalPortType.SecondPortCL, i);
                Thread.Sleep(ms);
                test.ClearBit(DigitalPortType.SecondPortCL, i);
            }
            for (int i = 0; i < 4; i++)
            {
                test.SetBit(DigitalPortType.SecondPortCH, i);
                Thread.Sleep(ms);
                test.ClearBit(DigitalPortType.SecondPortCH, i);
            }
        }
        private static void TestUSBSwitch(int ms, int toggles)
        {
            MccDaq_GPIO test = new MccDaq_GPIO();

            test.SetPort_Output(GPIO_Defs.USB_TGL.port);
            

            for (int i = 0; i < toggles; i++)
            {
                test.SetBit(GPIO_Defs.USB_TGL.port, GPIO_Defs.USB_TGL.pin);
                Thread.Sleep(ms);
                test.ClearBit(GPIO_Defs.USB_TGL.port, GPIO_Defs.USB_TGL.pin);
                Thread.Sleep(ms);
            }
           
        }
    }


       

    public class GUIConsoleWriter 
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        private const int ATTACH_PARENT_PROCESS = -1;

        StreamWriter _stdOutWriter;

        // this must be called early in the program
        public GUIConsoleWriter()
        {
            // this needs to happen before attachconsole.
            // If the output is not redirected we still get a valid stream but it doesn't appear to write anywhere
            // I guess it probably does write somewhere, but nowhere I can find out about
            var stdout = Console.OpenStandardOutput();
            _stdOutWriter = new StreamWriter(stdout);
            _stdOutWriter.AutoFlush = true;

            AttachConsole(ATTACH_PARENT_PROCESS);
        }

        public void WriteLine(string line)
        {
            _stdOutWriter.WriteLine(line);
            Console.WriteLine(line);
        }
    }
}