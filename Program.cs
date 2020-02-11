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

namespace ControlBoardTest
{
    static class Program
    {
        [STAThread]
        static void Main()
        {

            GUIConsoleWriter log_console = new GUIConsoleWriter();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //TestTelemetry(log_console);
            //TestGPIO();

            Application.Run(new ControlBoardTest(log_console));
            
           
        }



        /* *******************************************************************
         * Tests
         * 
         * 
         *********************************************************************/
        private static void TestTelemetry(GUIConsoleWriter console)
        {

            VLS_Tlm Vent = new VLS_Tlm("10.10.2.204");

            if (Vent.Connect())
            {
                Vent.CMD_Write("mfgmode");



                Vent.CMD_Write("set vcm testmgr o2speed 1000");
                Vent.CMD_Write("set vcm testmgr speed 20000");

                Vent.CMD_Write("stream vcm 1");

                string tlm = Vent.TLM_Read();

                string str = Vent.CMD_Write("get vcm table telemetry");

                console.WriteLine(str);

            }



            Application.Exit();
        }
        private static void TestGPIO()
        {
            MccDaq_GPIO test = new MccDaq_GPIO();
            int delay = 250;
            for (int j = 0; j < 20; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    test.SetBit(DigitalPortType.FirstPortA, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.FirstPortA, i);
                }
                for (int i = 0; i < 8; i++)
                {
                    test.SetBit(DigitalPortType.FirstPortB, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.FirstPortB, i);
                }
                for (int i = 0; i < 4; i++)
                {
                    test.SetBit(DigitalPortType.FirstPortCL, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.FirstPortCL, i);
                }
                for (int i = 0; i < 4; i++)
                {
                    test.SetBit(DigitalPortType.FirstPortCH, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.FirstPortCH, i);
                }
                for (int i = 0; i < 8; i++)
                {
                    test.SetBit(DigitalPortType.SecondPortA, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.SecondPortA, i);
                }
                for (int i = 0; i < 8; i++)
                {
                    test.SetBit(DigitalPortType.SecondPortB, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.SecondPortB, i);
                }
                for (int i = 0; i < 4; i++)
                {
                    test.SetBit(DigitalPortType.SecondPortCL, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.SecondPortCL, i);
                }
                for (int i = 0; i < 4; i++)
                {
                    test.SetBit(DigitalPortType.SecondPortCH, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.SecondPortCH, i);
                }
                for (int i = 0; i < 8; i++)
                {
                    test.SetBit(DigitalPortType.ThirdPortA, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.ThirdPortA, i);
                }
                for (int i = 0; i < 8; i++)
                {
                    test.SetBit(DigitalPortType.ThirdPortB, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.ThirdPortB, i);
                }
                for (int i = 0; i < 4; i++)
                {
                    test.SetBit(DigitalPortType.ThirdPortCL, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.ThirdPortCL, i);
                }
                for (int i = 0; i < 4; i++)
                {
                    test.SetBit(DigitalPortType.ThirdPortCH, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.ThirdPortCH, i);
                }
                for (int i = 0; i < 8; i++)
                {
                    test.SetBit(DigitalPortType.FourthPortA, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.FourthPortA, i);
                }
                for (int i = 0; i < 8; i++)
                {
                    test.SetBit(DigitalPortType.FourthPortB, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.FourthPortB, i);
                }
                for (int i = 0; i < 4; i++)
                {
                    test.SetBit(DigitalPortType.FourthPortCL, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.FourthPortCL, i);
                }
                for (int i = 0; i < 4; i++)
                {
                    test.SetBit(DigitalPortType.FourthPortCH, i);
                    Thread.Sleep(200);
                    test.ClearBit(DigitalPortType.FourthPortCH, i);

                }
            }



            Application.Exit();
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