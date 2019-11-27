using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Management;
using System.Windows.Forms;
using System.Diagnostics;



namespace ControlBoardTest
{   
    enum ProgrammerType
    {
        HERCULES = 1,
        CPLD = 2,
        SOM = 3,
    }

    
    class Programmer
    {
        SerialPort serialport;
        public bool Connected = false;
        ProgrammerType target;
        public Programmer(ProgrammerType target, string comport = null)
        {
            this.target = target;
            if (target != ProgrammerType.SOM)
            {
                ConnectProgrammer();
            }
            else
            {
                ConnectSOM(comport);
            }
        }

        public bool ConnectProgrammer()
        {
            string cpld_search = "Select * from Win32_USBHub";
            string cpld_name = "FlashPro";
            string herc_search = "Select * from Win32_SerialPort";
            string herc_name = "XDS2xx";
            string som_search = "Select * from Win32_SerialPort";
            string som_name = "USB";


            string name = "";
            string search = "";
            if(this.target == ProgrammerType.HERCULES)
            {
                name = herc_name;
                search = herc_search;
            }
            else if(this.target == ProgrammerType.CPLD)
            {
                name = cpld_name;
                search = cpld_search;
            }
            else if(this.target == ProgrammerType.SOM)
            {
                name = som_name;
                search = som_search;
            }

            

            ManagementObjectCollection ManObjReturn;
            ManagementObjectSearcher ManObjSearch;
            ManObjSearch = new ManagementObjectSearcher(search);
            ManObjReturn = ManObjSearch.Get();
            List<string> names = new List<string>();
            

            foreach (ManagementObject ManObj in ManObjReturn)
            {
                names.Add(ManObj["Name"].ToString());

            }
            ManObjReturn.Dispose();
            ManObjSearch.Dispose();
            foreach (string str in names)
            {
                if (str.Contains(name))
                {
                    this.Connected = true;
                }
            }

            return this.Connected;
        }

        public bool ConnectSOM(string comport)
        {
            try
            {
                this.serialport = new SerialPort(comport, 115200, System.IO.Ports.Parity.None, 8);
                this.serialport.Open();
            }
            catch
            {

            }
            if (this.serialport.IsOpen)
            {
                this.Connected = true;
            }
            
            return true;
        }
        public char SOM_ReadChar(){
            return (char)this.serialport.ReadChar();
        }
        public string SOM_WriteCMD(string cmd){
            this.serialport.WriteLine(cmd);
            return this.SOM_ReadUntil("# ");
        }
        public int SOM_Available(){
            return this.serialport.BytesToRead;
        }
        public string SOM_ReadUntil(string str, long timeout = 5000)
        {
            
            string out_str = "";
            Stopwatch timer = Stopwatch.StartNew();
            while (true)
            {
                if(this.serialport.BytesToRead > 0)
                {
                    out_str = out_str + (char)serialport.ReadByte();
                }
                if (out_str.Contains(str))
                {
                    break;
                }
                if (timer.ElapsedMilliseconds > timeout)
                {
                    out_str = "timeout";
                    break;
                }
            }
            return out_str;
        }
        public bool SOM_ReadUntil(string str, bool boolean, long timeout = 5000)
        {

            string out_str = "";
            bool success = false;
            Stopwatch timer = Stopwatch.StartNew();
            while (true)
            {
                if (this.serialport.BytesToRead > 0)
                {
                    out_str = out_str + (char)serialport.ReadByte();
                }
                if (out_str.Contains(str))
                {
                    success = true;
                    break;
                }
                if (timer.ElapsedMilliseconds > timeout)
                {
                    out_str = "timeout";
                    success = false;
                    break;
                }
            }
            return success ;
        }
        public bool SOM_ReadUntil_Boot()
        {
            const long timeout = 50000;
            string out_str = "";
            bool success = false;
            Stopwatch timer = Stopwatch.StartNew();

            while (true)
            {
                if (this.serialport.BytesToRead > 0)
                {
                    out_str = out_str + (char)serialport.ReadByte();
                }
                if (out_str.Contains("Starting Storyboard from /opt"))
                {
                    //Not quite done, rest of the output is not unique. Pause for 1.5s and then read until buffer is empty
                    timer.Stop();
                    System.Threading.Thread.Sleep(1500);
                    while(this.serialport.BytesToRead > 0)
                    {
                        out_str = out_str + (char)this.serialport.ReadByte();
                    }
                    int a = out_str.Length;
                    success = true;

                    break;
                }
                if(timer.ElapsedMilliseconds > timeout)
                {
                    success = false;
                    break;
                }
            }
            return success;
        }
        //U-Boot Specific commands
        public bool SOM_ReadUntil_UBoot()
        {
            const long timeout = 10000; //Device takes ~9.5s to boot to U-Boot
            string out_str = "";
            bool success = false;
            Stopwatch timer = Stopwatch.StartNew();

            while (true)
            {
                if (this.serialport.BytesToRead > 0)
                {
                    out_str = out_str + (char)serialport.ReadByte();
                }
                if (out_str.Contains("U-Boot# "))
                {
                    timer.Stop();
                    success = true;
                    break;
                }
                if (timer.ElapsedMilliseconds > timeout)
                {
                    success = false;
                    break;
                }
            }
           
            return success;
        }
        public string SOM_UBoot_CMD(string cmd)
        {
            this.serialport.WriteLine(cmd);
            return this.SOM_ReadUntil("U-Boot# ");
        }

    }
}
