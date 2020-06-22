using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MccDaq;
using ErrorDefs;
using System.Windows.Forms;


namespace GPIO
{


    public class MccDaq_GPIO
    {
        MccDaq.MccBoard gpio_board;
        int numChannels;
        MccDaq.ErrorInfo err;
        MccDaq.DigitalPortType[] Ports = {DigitalPortType.FirstPortA, DigitalPortType.FirstPortB, DigitalPortType.FirstPortCH, DigitalPortType.FirstPortCL,
                                          DigitalPortType.SecondPortA, DigitalPortType.SecondPortB, DigitalPortType.SecondPortCH, DigitalPortType.SecondPortCL,
                                          DigitalPortType.ThirdPortA, DigitalPortType.ThirdPortB, DigitalPortType.ThirdPortCH, DigitalPortType.ThirdPortCL,
                                          DigitalPortType.FourthPortA, DigitalPortType.FourthPortB, DigitalPortType.FourthPortCH, DigitalPortType.FourthPortCL,};
        DigitalIO.clsDigitalIO dig_props = new DigitalIO.clsDigitalIO();

        public bool Connected = false;
        public MccDaq_GPIO()
        {
            ConnectDevice();

            try
            {
                this.SetPort_Output(DigitalPortType.FirstPortA);
                this.SetPort(DigitalPortType.FirstPortA, 0);
                this.SetPort_Output(DigitalPortType.FirstPortB);
                this.SetPort(DigitalPortType.FirstPortB, 0);
                this.SetPort_Output(DigitalPortType.FirstPortCL);
                this.SetPort(DigitalPortType.FirstPortCL, 0);
                this.SetPort_Output(DigitalPortType.FirstPortCH);
                this.SetPort(DigitalPortType.FirstPortCH, 0);


                this.SetPort_Output(DigitalPortType.SecondPortA);
                this.SetPort(DigitalPortType.SecondPortA, 0);
                this.SetPort_Output(DigitalPortType.SecondPortB);
                this.SetPort(DigitalPortType.SecondPortB, 0);
                this.SetPort_Output(DigitalPortType.SecondPortCL);
                this.SetPort(DigitalPortType.SecondPortCL, 0);
                this.SetPort_Output(DigitalPortType.SecondPortCH);
                this.SetPort(DigitalPortType.SecondPortCH, 0);

                this.SetPort_Input(DigitalPortType.ThirdPortA);
                //this.SetPort(DigitalPortType.ThirdPortA, 0);
                this.SetPort_Input(DigitalPortType.ThirdPortB);
                //this.SetPort(DigitalPortType.ThirdPortB, 0);
                this.SetPort_Input(DigitalPortType.ThirdPortCL);
                //this.SetPort(DigitalPortType.ThirdPortCL, 0);
                this.SetPort_Input(DigitalPortType.ThirdPortCH);
                //this.SetPort(DigitalPortType.ThirdPortCH, 0);


                this.SetPort_Input(DigitalPortType.FourthPortA);
                this.SetPort_Input(DigitalPortType.FourthPortB);
                //this.SetPort_Output(DigitalPortType.FourthPortCL);
                //this.SetPort_Output(DigitalPortType.FourthPortCH);
            }
            catch (Exception )
            {
                MessageBox.Show("GPIO is not connected\nPlease connect the GPIO module and restart the application", "GPIO ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        ~MccDaq_GPIO()
        {
            //ClearAll();
            this.gpio_board = null;
        }

        public void ClearAllButPower()
        {
            this.SetPort(DigitalPortType.FirstPortA, (ushort)(1<<(GPIO_Defs.AC_EN.pin)));
            this.SetPort(DigitalPortType.FirstPortB, 0);
            this.SetPort(DigitalPortType.FirstPortCL, 0);
            this.SetPort(DigitalPortType.FirstPortCH, 0);

            this.SetPort(DigitalPortType.SecondPortA, 0);
            this.SetPort(DigitalPortType.SecondPortB, 0);
            this.SetPort(DigitalPortType.SecondPortCL, 0);
            this.SetPort(DigitalPortType.SecondPortCH, 0);
        }

        public void ClearAll()
        {
            this.SetPort(DigitalPortType.FirstPortA, 0);
            this.SetPort(DigitalPortType.FirstPortB, 0);
            this.SetPort(DigitalPortType.FirstPortCL, 0);
            this.SetPort(DigitalPortType.FirstPortCH, 0);

            this.SetPort(DigitalPortType.SecondPortA, 0);
            this.SetPort(DigitalPortType.SecondPortB, 0);
            this.SetPort(DigitalPortType.SecondPortCL, 0);
            this.SetPort(DigitalPortType.SecondPortCH, 0);

            this.SetPort(DigitalPortType.ThirdPortA, 0);
            this.SetPort(DigitalPortType.ThirdPortB, 0);
            this.SetPort(DigitalPortType.ThirdPortCL, 0);
            this.SetPort(DigitalPortType.ThirdPortCH, 0);

            this.SetPort(DigitalPortType.FourthPortA, 0);
            this.SetPort(DigitalPortType.FourthPortB, 0);
            this.SetPort(DigitalPortType.FourthPortCL, 0);
            this.SetPort(DigitalPortType.FourthPortCH, 0);
        }

        public bool SetBit(DigitalPortType port, int bit)
        {
            bool success = false;
            try
            {

                var currVal = (int)this.GetPort(port);
                var newVal = (short)(currVal | (1 << bit));
                this.gpio_board.DOut(port, newVal);
                success = true;
            }
            catch(Exception e)
            {
                throw e;
                
            }
            return success;
            
        }
        public bool ClearBit(DigitalPortType port, int bit)
        {
            bool success=false;
            try
            {
                var currVal = (int)this.GetPort(port);
                var currBit = (int)this.GetBit(port, bit);
                var newVal = (short)(currVal ^ ((1 & currBit) << bit));
                this.gpio_board.DOut(port,  newVal);
                success = true;
            }
            catch (Exception e)
            {
                throw e;
                
            }
            return success;

        }

        public bool SetPort(DigitalPortType port, ushort val)
        {
            bool success=false;
            try
            {
                this.gpio_board.DOut(port, val);
                success = true;
            }
            catch(Exception e)
            {
                throw e;
            }
            return success;
        }

        public ushort GetPort(DigitalPortType port)
        {
            short val = 0;
            try
            {
                this.gpio_board.DIn(port, out val);
            }
            catch
            {

            }

            return (ushort)val;
        }

        public int GetBit(DigitalPortType port, int bit)
        {
            short ret_val;
            int val = 0;
            this.gpio_board.DIn(port, out ret_val);

            val = ((int)ret_val & (1 << bit)) >> bit;

            return val;
        }

        public void SetPort_Input(DigitalPortType port)
        {
            try
            {
                this.gpio_board.DConfigPort(port, DigitalPortDirection.DigitalIn);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public void SetPort_Output(DigitalPortType port)
        {
            try
            {
                this.gpio_board.DConfigPort(port, DigitalPortDirection.DigitalOut);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /********************************************************************
         * GPIO_PinParse()
         * 
         * Convert string representation of port and pin to digital port type
         * 
         * *****************************************************************/

        private bool GPIO_PinParse(string port_str, string pin_str, out DigitalPortType port, out ushort pin)
        {
            bool success = false;
            port = DigitalPortType.AuxPort;
            pin = 0;
            switch(port_str){
                case ("FirstPortA"):
                    port = DigitalPortType.FirstPortA;
                    break;
                case ("SecondPortA"):
                    port = DigitalPortType.SecondPortA;
                    break;
                case ("ThirdPortA"):
                    port = DigitalPortType.ThirdPortA;
                    break;
                case ("FourthPortA"):
                    port = DigitalPortType.FourthPortA;
                    break;
                case ("FifthPortA"):
                    port = DigitalPortType.FifthPortA;
                    break;
                case ("SixthPortA"):
                    port = DigitalPortType.SixthPortA;
                    break;
                case ("SeventhPortA"):
                    port = DigitalPortType.SeventhPortA;
                    break;
                case ("EighthPortA"):
                    port = DigitalPortType.EighthPortA;
                    break;
                case ("FirstPortB"):
                    port = DigitalPortType.FirstPortB;
                    break;
                case ("SecondPortB"):
                    port = DigitalPortType.SecondPortB;
                    break;
                case ("ThirdPortB"):
                    port = DigitalPortType.ThirdPortB;
                    break;
                case ("FourthPortB"):
                    port = DigitalPortType.FourthPortB;
                    break;
                case ("FifthPortB"):
                    port = DigitalPortType.FifthPortB;
                    break;
                case ("SixthPortB"):
                    port = DigitalPortType.SixthPortB;
                    break;
                case ("SeventhPortB"):
                    port = DigitalPortType.SeventhPortB;
                    break;
                case ("EighthPortB"):
                    port = DigitalPortType.EighthPortB;
                    break;
                case ("FirstPortCL"):
                    port = DigitalPortType.FirstPortCL;
                    break;
                case ("SecondPortCL"):
                    port = DigitalPortType.SecondPortCL;
                    break;
                case ("ThirdPortCL"):
                    port = DigitalPortType.ThirdPortCL;
                    break;
                case ("FourthPortCL"):
                    port = DigitalPortType.FourthPortCL;
                    break;
                case ("FifthPortCL"):
                    port = DigitalPortType.FifthPortCL;
                    break;
                case ("SixthPortCL"):
                    port = DigitalPortType.SixthPortCL;
                    break;
                case ("SeventhPortCL"):
                    port = DigitalPortType.SeventhPortCL;
                    break;
                case ("EighthPortCL"):
                    port = DigitalPortType.EighthPortCL;
                    break;
                case ("FirstPortCH"):
                    port = DigitalPortType.FirstPortCH;
                    break;
                case ("SecondPortCH"):
                    port = DigitalPortType.SecondPortCH;
                    break;
                case ("ThirdPortCH"):
                    port = DigitalPortType.ThirdPortCH;
                    break;
                case ("FourthPortCH"):
                    port = DigitalPortType.FourthPortCH;
                    break;
                case ("FifthPortCH"):
                    port = DigitalPortType.FifthPortCH;
                    break;
                case ("SixthPortCH"):
                    port = DigitalPortType.SixthPortCH;
                    break;
                case ("SeventhPortCH"):
                    port = DigitalPortType.SeventhPortCH;
                    break;
                case ("EighthPortCH"):
                    port = DigitalPortType.EighthPortCH;
                    break;
                default:
                    break;
            }
            int val;
            if (int.TryParse(pin_str, out val))
            {
                pin = (ushort)val;
                success = true;
            }
            else
            {
                success = false;
            }

            return success;
        }

        private void InitUL()
        {
            //  Initiate error handling
            //   activating error handling will trap errors like
            //   bad channel numbers and non-configured conditions.
            //   Parameters:
            //     MccDaq.ErrorReporting.PrintAll :all warnings and errors encountered will be printed
            //     MccDaq.ErrorHandling.StopAll   :if an error is encountered, the program will stop
            


            clsErrorDefs.ReportError = MccDaq.ErrorReporting.PrintAll;
            clsErrorDefs.HandleError = MccDaq.ErrorHandling.DontStop;
            this.gpio_board = new MccDaq.MccBoard();
            this.err = MccDaq.MccService.ErrHandling
                (ErrorReporting.DontPrint, ErrorHandling.DontStop);

            this.err = this.gpio_board.BoardConfig.GetDiNumDevs(out this.numChannels);



            if (this.numChannels != 0)
            {
                this.Connected = true;
            }
            else
            {
                //GPIO is not configured
                this.Connected = false;
                this.gpio_board = null;
            }


            
        }
        public bool ConnectDevice()
        {
            InitUL();
            return this.Connected;
        }

    }


}
