using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;

namespace VLS
{
    public class VOCSN_Serial
    {
        SerialPort SerialConnection;
        public bool Connected;
        private bool booted;
        public bool Booted
        {
            get { return booted; }
            set { booted = value; }
        }
        private string COMPort;

        const long Timeout_MS = 500;
        const string End_String = "# ";

        public VOCSN_Serial(string comport = null)
        {
            this.COMPort = comport;

            //if (this.COMPort != null)
            //{
            //    try
            //    {
            //        this.Connect(this.COMPort);
            //    }
            //    catch(Exception e)
            //    {
            //        Console.WriteLine("Connection failed: " + e);
            //    }
            //}
        }

        public int Available()
        {
            return this.SerialConnection.BytesToRead;
        }


        public bool Connect(string comport = null)
        {
            bool success = false;
            if (comport != null)
            {
                this.COMPort = comport;
            }

            this.SerialConnection = new SerialPort(this.COMPort, 115200, Parity.None, 8);
            var portExists = SerialPort.GetPortNames().Any(x => x == this.COMPort);
            if (portExists)
            {
                this.SerialConnection.Open();
            }
            

            if (this.SerialConnection.IsOpen)
            {
                success = true;
                this.Connected = true;
            }

            return success;
        }

        public bool Command(string cmd, out string output, string readuntil = End_String, long timeout = Timeout_MS)
        {
            bool success = false;
            output = "";

            this.SerialConnection.WriteLine(cmd);
            this.ReadUntil(readuntil, out output, timeout);

            return success;
        }

        public bool Status()
        {
            return this.Connected;
        }

        public bool Close()
        {
            bool success = false;
            return success;
        }

        public bool ReadUntil(string str, out string out_str, long timeout = Timeout_MS)
        {
            bool success = false;
            out_str = "";
            Stopwatch timer = Stopwatch.StartNew();
            while (true)
            {
                if (this.SerialConnection.BytesToRead > 0)
                {
                    out_str += (char)SerialConnection.ReadByte();
                }
                if (out_str.Contains(str))
                {
                    success = true;
                    break;
                }
                if (timer.ElapsedMilliseconds > timeout)
                {
                    timer.Stop();
                    out_str = null;
                    success = false;
                    break;
                }
            }
            return success;
        }
        public bool ReadAll(out string out_str, long timeout = Timeout_MS)
        {
            bool success = true;
            out_str = "";
            Stopwatch timer = Stopwatch.StartNew();
            while (this.SerialConnection.BytesToRead > 0)
            {
                    out_str += (char)SerialConnection.ReadByte();
                    if (timer.ElapsedMilliseconds > timeout)
                    {
                        timer.Stop();
                        out_str = null;
                        success = false;
                        break;
                    }
            }

            return success;
        }
        public bool ReadLine(out string out_str, long timeout = Timeout_MS)
        {
            bool success = true;
            out_str = "";
            Stopwatch timer = Stopwatch.StartNew();
            while (this.SerialConnection.BytesToRead > 0)
            {
                out_str += (char)SerialConnection.ReadByte();
                if (out_str.EndsWith("\n"))
                {
                    break;
                }
                if (timer.ElapsedMilliseconds > timeout)
                {
                    timer.Stop();
                    out_str = null;
                    success = false;
                    break;
                }
            }

            return success;
        }

    }
}
