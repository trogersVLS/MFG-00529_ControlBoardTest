using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MccDaq;

namespace GPIO
{

    public struct GPIO_PIN
    {
        public DigitalPortType port;
        public ushort pin;

        public GPIO_PIN(DigitalPortType port, ushort pin)
        {   //Defines the port name and port value of the associated GPIO signal
            //In order to do "atomic" writes to the GPIO module using multiple signals, 
            //they will need to share the same port and be OR'd together
            this.port = port;
            this.pin = pin;
        }

    }

    static class GPIO_Defs
    {
        public static readonly GPIO_PIN BAT0_EN;
        public static readonly GPIO_PIN BAT1_EN;
        public static readonly GPIO_PIN BAT2_EN;
        public static readonly GPIO_PIN CHG_LOAD_EN;
        public static readonly GPIO_PIN AMM_EN;

        public static readonly GPIO_PIN MEAS_3V3_HOT_EN;
        public static readonly GPIO_PIN MEAS_5V0_HOT_EN;
        public static readonly GPIO_PIN MEAS_5V3_EN;
        public static readonly GPIO_PIN MEAS_12V0_EN;
        public static readonly GPIO_PIN MEAS_3V3_EN;
        public static readonly GPIO_PIN MEAS_1V2_EN;
        public static readonly GPIO_PIN MEAS_3V3A_EN;
        public static readonly GPIO_PIN MEAS_VREF_EN;
        public static readonly GPIO_PIN MEAS_30V_EN;
        public static readonly GPIO_PIN MEAS_36V_EN;

        public static readonly GPIO_PIN MEAS_O2_SV1N_EN;
        public static readonly GPIO_PIN MEAS_O2_SV2N_EN;

        public static readonly GPIO_PIN FAN_FREQ_MEAS_EN;
        public static readonly GPIO_PIN VFAN_MEAS_EN;
        public static readonly GPIO_PIN FAN_FAULT_EN;

        public static readonly GPIO_PIN EXT_O2_DIS;

        public static readonly GPIO_PIN WDOG_DIS;
        public static readonly GPIO_PIN USB_TGL;

        public static readonly GPIO_PIN MEAS_BATT_CHG_EN;

        public static readonly GPIO_PIN AC_EN;

        public static readonly GPIO_PIN AS_BTN_ON;
        public static readonly GPIO_PIN PB_BTN_ON;
        public static readonly GPIO_PIN SPKR_EN;

        public static readonly GPIO_PIN TEMP_BATT0;
        public static readonly GPIO_PIN TEMP_BATT1;
        public static readonly GPIO_PIN TEMP_BATT2;

        //Solenoid valve signals
        public static readonly GPIO_PIN XFLOW_SV1_2;
        public static readonly GPIO_PIN XFLOW_SV3_4;
        public static readonly GPIO_PIN FLOW_SV5;
        public static readonly GPIO_PIN EXHL_SV6;
        public static readonly GPIO_PIN EXHL_SV7;
        public static readonly GPIO_PIN EXHL_SV8;
        public static readonly GPIO_PIN SOV_SV11;


        //Measurement Signals
        public static readonly GPIO_PIN MEAS_FREQ_BLOWER;
        public static readonly GPIO_PIN MEAS_FREQ_PUMP;
        public static readonly GPIO_PIN MEAS_NC_NO;
        public static readonly GPIO_PIN MEAS_NC_NC;
        public static readonly GPIO_PIN MEAS_CHG_LED;
        public static readonly GPIO_PIN MEAS_ON_LED;
        public static readonly GPIO_PIN MEAS_AS_LED;
        public static readonly GPIO_PIN MEAS_INOP_LED;
        public static readonly GPIO_PIN MEAS_EXT_LED;
        public static readonly GPIO_PIN MEAS_PIEZO;
        public static readonly GPIO_PIN OAX1_OFF;
        public static readonly GPIO_PIN OAX2_OFF;


        public static readonly GPIO_PIN MEAS_MV_HOME;
        public static readonly GPIO_PIN MEAS_RV1_HOME;
        public static readonly GPIO_PIN MEAS_RV2_HOME;

        public static readonly GPIO_PIN MV_HOME_EN;
        public static readonly GPIO_PIN RV1_HOME_EN;
        public static readonly GPIO_PIN RV2_HOME_EN;

        public static readonly GPIO_PIN PIEZO_EN;
        public static readonly GPIO_PIN MEAS_PIEZO_N;
        
        static GPIO_Defs()
        {   
        //FirstPortA -> 8 bits wide --> OUTPUTS
            BAT0_EN =           new GPIO_PIN(DigitalPortType.FirstPortA, 0); 
            //
            BAT1_EN =           new GPIO_PIN(DigitalPortType.FirstPortA, 2); 
            //
            BAT2_EN =           new GPIO_PIN(DigitalPortType.FirstPortA, 4); 
            AC_EN   =           new GPIO_PIN(DigitalPortType.FirstPortA, 5);
            CHG_LOAD_EN =       new GPIO_PIN(DigitalPortType.FirstPortA, 6); 
            AMM_EN =            new GPIO_PIN(DigitalPortType.FirstPortA, 7); 
        //FirstPortB -> 8 bits wide --> OUTPUTS
            //MEAS_3V3_HOT_EN =   new GPIO_PIN(DigitalPortType.FirstPortB, 0); 
            MEAS_5V0_HOT_EN =   new GPIO_PIN(DigitalPortType.FirstPortB, 1); 
            MEAS_5V3_EN =       new GPIO_PIN(DigitalPortType.FirstPortB, 2); 
            MEAS_12V0_EN =      new GPIO_PIN(DigitalPortType.FirstPortB, 3); 
            MEAS_3V3_EN =       new GPIO_PIN(DigitalPortType.FirstPortB, 4);
            MEAS_1V2_EN =       new GPIO_PIN(DigitalPortType.FirstPortB, 5); 
            MEAS_3V3A_EN =      new GPIO_PIN(DigitalPortType.FirstPortB, 6); 
            MEAS_VREF_EN =      new GPIO_PIN(DigitalPortType.FirstPortB, 7); 
        //FirstPortCL -> 4 bits wide --> OUTPUTS
            MEAS_30V_EN =       new GPIO_PIN(DigitalPortType.FirstPortCL, 0); 
            MEAS_36V_EN =       new GPIO_PIN(DigitalPortType.FirstPortCL, 1); 
            MEAS_O2_SV1N_EN =   new GPIO_PIN(DigitalPortType.FirstPortCL, 2); 
            MEAS_O2_SV2N_EN =   new GPIO_PIN(DigitalPortType.FirstPortCL, 3); 
        //FirstPortCH -> 4 bits wide --> OUTPUTS
            FAN_FREQ_MEAS_EN =  new GPIO_PIN(DigitalPortType.FirstPortCH, 0); 
            VFAN_MEAS_EN =      new GPIO_PIN(DigitalPortType.FirstPortCH, 1);
            MEAS_3V3_HOT_EN =   new GPIO_PIN(DigitalPortType.FirstPortCH, 2);
            MEAS_PIEZO =        new GPIO_PIN(DigitalPortType.FirstPortCH, 3); 
        //SecondPortA -> 8 bits wide --> OUTPUTS
            FAN_FAULT_EN =      new GPIO_PIN(DigitalPortType.SecondPortA, 0);
            EXT_O2_DIS =        new GPIO_PIN(DigitalPortType.SecondPortA, 1); 
            PIEZO_EN =          new GPIO_PIN(DigitalPortType.SecondPortA, 2);
            WDOG_DIS =          new GPIO_PIN(DigitalPortType.SecondPortA, 3); 
            //
            AS_BTN_ON =         new GPIO_PIN(DigitalPortType.SecondPortA, 5); 
            PB_BTN_ON =         new GPIO_PIN(DigitalPortType.SecondPortA, 6); 
            SPKR_EN =           new GPIO_PIN(DigitalPortType.SecondPortA, 7); 
        //SecondPortB -> 8 bits wide --> Unused
            //
            //
            //
            //
            //
            //
            //
            //
        //SecondPortCL -> 4 bits wide --> Unused
            TEMP_BATT2 = new GPIO_PIN(DigitalPortType.SecondPortCL, 1);
            TEMP_BATT1 = new GPIO_PIN(DigitalPortType.SecondPortCL, 2);
            TEMP_BATT0 = new GPIO_PIN(DigitalPortType.SecondPortCL, 3);
            //
        //SecondPortCH -> 4 bits wide --> OUTPUTS
            MEAS_NC_NC = new GPIO_PIN(DigitalPortType.SecondPortCH, 0);
            MEAS_NC_NO = new GPIO_PIN(DigitalPortType.SecondPortCH, 1);
            MEAS_FREQ_PUMP = new GPIO_PIN(DigitalPortType.SecondPortCH, 2);
            MEAS_FREQ_BLOWER = new GPIO_PIN(DigitalPortType.SecondPortCH, 3);
        //ThirdPortA
            MEAS_MV_HOME =      new GPIO_PIN(DigitalPortType.ThirdPortA, 0); //Measure with GPIO
            MEAS_RV1_HOME =     new GPIO_PIN(DigitalPortType.ThirdPortA, 1); //Measure with GPIO
            //
            MEAS_CHG_LED =      new GPIO_PIN(DigitalPortType.ThirdPortA, 3); //Measure with GPIO
            MEAS_ON_LED =       new GPIO_PIN(DigitalPortType.ThirdPortA, 4); //Measure with GPIO
            MEAS_AS_LED =       new GPIO_PIN(DigitalPortType.ThirdPortA, 5); //MEasure with GPIO
            MEAS_INOP_LED =     new GPIO_PIN(DigitalPortType.ThirdPortA, 6); //MEasure with GPIO
            MEAS_EXT_LED =      new GPIO_PIN(DigitalPortType.ThirdPortA, 7); //Measure with GPIO
        //ThirdPortB
            //
            //
            //
            //
            //
            //
            //
            //
        //ThirdPortCL
            //
            //
            //
            //
        //ThirdPortCH
            MEAS_PIEZO_N =      new GPIO_PIN(DigitalPortType.ThirdPortCH, 0); //Unused currently
            MEAS_RV2_HOME =     new GPIO_PIN(DigitalPortType.ThirdPortCH, 1); //Measure with GPIO
            OAX2_OFF =          new GPIO_PIN(DigitalPortType.ThirdPortCH, 2); //Measure with GPIO
            //
        //FourthPortA
            XFLOW_SV1_2 =       new GPIO_PIN(DigitalPortType.FourthPortA, 0); //Measure with GPIO 
            XFLOW_SV3_4 =       new GPIO_PIN(DigitalPortType.FourthPortA, 1); //Measure with GPIO
            FLOW_SV5 =          new GPIO_PIN(DigitalPortType.FourthPortA, 2); //Measure with GPIO
            EXHL_SV6 =          new GPIO_PIN(DigitalPortType.FourthPortA, 3); //Measure with GPIO
            EXHL_SV7 =          new GPIO_PIN(DigitalPortType.FourthPortA, 4); //Measure with GPIO
            EXHL_SV8 =          new GPIO_PIN(DigitalPortType.FourthPortA, 5); //Measure with GPIO
            SOV_SV11 =          new GPIO_PIN(DigitalPortType.FourthPortA, 6); //Measure with GPIO
            OAX1_OFF =          new GPIO_PIN(DigitalPortType.FourthPortA, 7); //Measure with GPIO
        //FourthPortB
            //
            //
            //
            //
            //
            //
            //
            //
        //FourthPortCL
            //
            //
            //
            //
        //FourthPortCH
            //
            //
            //
            //    

        }


    }

}
