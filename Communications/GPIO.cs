using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MccDaq;
using ErrorDefs;

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
            InitUL();

            //TODO: add error handling for writes to ports when port is disables
            //dsetPort(DigitalPortType.FirstPortB, 10);

            //short x = getPort(DigitalPortType.FirstPortB);
        }

        public bool setBit(DigitalPortType port, int bit, DigitalLogicState val)
        {
            bool success;
            try
            {
                this.gpio_board.DBitOut(port, bit, val);
                success = true;
            }
            catch
            {
                success = false;
            }
            return success;
            
        }

        public bool setPort(DigitalPortType port, ushort val)
        {
            bool success;
            try
            {
                this.gpio_board.DOut(port, val);
                success = true;
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public ushort getPort(DigitalPortType port)
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

        public int getBit(DigitalPortType port, int bit)
        {
            //TODO: Write this function
            return 5;
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
                (ErrorReporting.PrintAll, ErrorHandling.DontStop);

            this.err = this.gpio_board.BoardConfig.GetDiNumDevs(out this.numChannels);



            if (this.numChannels != 0)
            {
                this.Connected = true;
                for (int i = 0; i < (numChannels - 1); i++)
                {
                    err = this.gpio_board.DConfigPort(Ports[i], DigitalPortDirection.DigitalOut);
                    this.setPort(Ports[i], 0);
                }
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
