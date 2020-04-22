using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ControlBoardTest
{
    public class Test_Equip
    {
        private readonly int QUERY_DELAY = 3010; //millisecond delay for SCPI queries
        string name;
        string comm;
        SerialPort Device;
        string address;
        string ID;


        StopBits stopbits;
        

        public bool Connected = false;
        /* Test_Equip Constructor: 
         * Initializes a Test Equipment SCPI compatible device over RS232, GPIB, LAN, or USB
         */
        public Test_Equip()
        {

        }
        public Test_Equip(string ID, string comm,  int baud, int stopbits, string address = null)
        {

            this.ID = ID;
            this.address = address;
            this.comm = comm;
            if(stopbits == 1)
            {
                this.stopbits = StopBits.One;
            }
            else if(stopbits == 2)
            {
                this.stopbits = StopBits.Two;
            }
            else if(stopbits == 0)
            {
                this.stopbits = StopBits.None;
            }

            if (comm == "RS232")
            {
                this.Device = new SerialPort(address, baud, Parity.None, 8, this.stopbits);
                this.Device.RtsEnable = true;
                this.Device.DtrEnable = true;
                this.Device.ReadTimeout = 1500;
            }
            this.Connected = false;
            
        }
        ~Test_Equip()
        {
            try
            {
                this.Device.Close();          
            }
            catch
            {

            }
        }
        
        public bool Connect()
        {
            if (this.comm == "RS232")
            { 
                var portExists = SerialPort.GetPortNames().Any(x => x == this.address);
                if (portExists) { 
                    try
                    {

                        this.Device.Open();
                        string identity = this.Query("*IDN?");
                        if (identity.Contains(this.ID)) this.Connected = true;
                        else this.Connected = false;
                        if (this.Connected) this.Device.Write("SYST:REM\n\r");
                    }
                    catch
                    {
                        this.Connected = false;
                        this.Device.Close();
                    }
                }
                

                
            }
            else if (this.comm == "GPIB")
            {
                //TODO: GPIB constructor
            }
            else if (this.comm == "USB")
            {
                //TODO: USB constructor
            }
            else if (this.comm == "LAN")
            {
                //TODO: LAN constructor
            }

            return this.Connected;
        }

        public float Get_Volts()
        {
            string volt_str = "";
            float volts;

            volt_str = this.Query(":MEAS:VOLT:DC?");
            try
            {
                volts = float.Parse(volt_str, System.Globalization.NumberStyles.Float);
            }
            catch
            {
                volts = 0;
            }

            return volts;
        }
        public float Get_Amps()
        {
            string amp_str = "";
            float amps;

            amp_str = this.Query(":MEAS:CURR:DC? 3");
            try
            {
                amps = float.Parse(amp_str, System.Globalization.NumberStyles.Float);
            }
            catch
            {
                amps = 0;
            }

            return amps;
        }

        public float Get_Freq()
        {
            string freq_str = "";
            float freq;

            this.Device.ReadTimeout = 2000;
            //freq_str = this.Query(":MEAS:FREQ?", 5000);
            int cnt = 0;
            bool ok;
            do
            {
                freq_str = this.Query(":MEAS:FREQ?", 5000);
                ok = float.TryParse(freq_str, out freq);
                cnt++;

            } while (!ok && (cnt < 10));

            try
            {
                freq = float.Parse(freq_str, System.Globalization.NumberStyles.Float);
            }
            catch
            {
                freq = 0;
            }

            return freq;
        }

        public float Get_Ohms()
        {
            string ohms_str = "";
            float ohms;
            bool ok = false;
            int cnt = 0;
            do
            {
                ohms_str = this.Query(":MEAS:RES?", 5000);
                ok = float.TryParse(ohms_str, out ohms);
                cnt++;

            } while (!ok && (cnt < 10));

            try
            {
                ohms = float.Parse(ohms_str, System.Globalization.NumberStyles.Float);
            }
            catch
            {
                ohms = -1;
            }

            return ohms;
        }

        private string Query(string cmd, int query_delay = 0)
        {
            string response = "";
            byte[] byte_response = new byte[256]; // TROGERS,  upped response buffer to accomodate errors.
            if(this.comm == "RS232")
            {   if(query_delay == 0)
                {
                    query_delay = this.QUERY_DELAY;
                }
                try
                {
                    this.Device.ReadTimeout = 5000;

                    //this.Device.Write("SYST:REM\n\r");
                    //this.Device.Write("*RST\n\r");
                    Thread.Sleep(200); // TROGERS - Brought down to 200 from 1000
                    this.Device.Write(cmd +"\n");
                    Thread.Sleep(this.QUERY_DELAY);
                    int num = this.Device.Read(byte_response, 0, byte_response.Length);
                    response = Encoding.ASCII.GetString(byte_response, 0, byte_response.Length);
                }
                catch(Exception e)
                {
                    throw e;
                }
                

            }
            

            return response;
        }

        public void PPS_Init()
        {
            this.Device.Write("SYST:REM\n\r");
            Thread.Sleep(200);
            this.Device.Write("*RST\n\r");
            Thread.Sleep(200);
            
        }
        public bool Set_Output(bool state, float voltage = 0, float current = 0)
        {
            bool success = false;
            float amps;
            string status;
            if (state)
            {
                status = "ON";
            }
            else
            {
                status = "off";
            }
            if (current != 0)
            {
                this.Set_Current(current);
                Thread.Sleep(200);
            }
            if (voltage != 0)
            {
                this.Set_Voltage(voltage);
                Thread.Sleep(200);
            }

            
            this.Device.Write("OUTP " + status + "\r\n");
            Thread.Sleep(200);

            return success;
        }

        public float Set_Current(float current)
        {
            string amp_str = "";
            float amps = 0;

            this.Device.Write("Current " + current.ToString() + "\r\n");
            

            return amps;
        }
        public float Set_Voltage(float volts)
        {
            string volt_str = "";
            float voltage = 0;

            this.Device.Write("Voltage " + volts.ToString() + "\r\n");
            

            return voltage;
        }


        public bool SetPPSOutput(double volts, double current)
        {
            bool success = false;
            string response = "";
            byte[] byte_response = new byte[128];

            double v = Math.Truncate(10 * volts) / 10;
            double i = Math.Truncate(10 * current) / 10;
            string cmd = "Current 2";

            try
            {
                this.Device.ReadTimeout = 5000;
                
                this.Device.Write("Current " + current.ToString() + "\r\n");
                this.Device.Write("Volt " + volts.ToString() + "\r\n");
                this.Device.Write("Output on");


                //Verify that the output was set
                cmd = "APPL?";
                this.Device.Write(cmd + "\r\n");
                Thread.Sleep(this.QUERY_DELAY);
                int num = this.Device.Read(byte_response, 0, byte_response.Length);
                response = Encoding.ASCII.GetString(byte_response, 0, byte_response.Length).Replace("\"", "");
                var responsearray = response.Split(',');

                var retVolts = double.Parse(responsearray[0]);
                var retAmps = double.Parse(responsearray[1]);
                if((retVolts == v) && (retAmps == i)){
                    cmd = "OUTP ON";
                    this.Device.Write(cmd + "\r\n");
                    Thread.Sleep(this.QUERY_DELAY);
                    num = this.Device.Read(byte_response, 0, byte_response.Length);

                    success = true;
                }
            }
            catch(Exception e)
            {
                throw (e);
            }


            return success;
        }
        public bool SetPPSOff()
        {
            bool success = false;
            string response = "";
            byte[] byte_response = new byte[128];

           
            string cmd = "OUTP OFF";

            try
            {
                
                this.Device.ReadTimeout = 5000;
                this.Device.Write(cmd + "\r\n");
                int num = this.Device.Read(byte_response, 0, byte_response.Length);
                response = Encoding.ASCII.GetString(byte_response, 0, byte_response.Length);
                success = true;
                
            }
            catch (Exception e)
            {
                throw (e);
            }


            return success;
        }
    }
}
