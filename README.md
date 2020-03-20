
![Ventec Life Systems](./FunctionalTest/Assets/vls_logo.png)
# MFG-00529 Rev A: 
## Software Specification - Control Board Test Station

This document outlines the functional and design specifications that compose MFG-00529: Control Board Test Station Software

## Build
1. Open mfg_527.sln using Visual Studio.
2. Change the Active build configuration to Debug/Release. The Debug configuration should build only the application. The Release configuration should build the Application and the Installer Project.

## Install
1. Open the MFG-00529_Setup.msi file after a release build.
2. Select all default options in the installer.
3. Install Instacal, located in <em>C:\Program Files (x86)\Ventec Life Systems\Control Board Test\Dependencies\ </em>
4. Restart PC.
5. When opening the application, confirm that the settings.xml file has been updated to reflect the physical COM port addresses that the DMM, PPS, and SOM Serial port use. These can be found in the Device Manager application or via Command Prompt.
6. Be sure that applications have read/write privelages to the <em>C:\Program Files (x86)\Ventec Life Systems\Control Board Test\ </em> folder

## Table Of Contents
1. [Installation Instruction](#Installation-Instructions)
1. [Functional Description](#Functional-Description)
1. [Database Tables](#Database-Tables)
2. [Tests](#Tests)
    1. [Signal Tests](#Signal-Tests)
        1. [Signal: LCD Test](#Signal:-LCD-Test)
        2. [Signal: Exhl SV 6](#Signal:-Exhl-SV-6)
        3. [Signal: Exhl SV 7](#Signal:-Exhl-SV-7)
        4. [Signal: Exhl SV 8](#Signal:-Exhl-SV-8)
        5. [Signal: XFlow SV 1 & 2](#Signal:-XFlow-SV-1-&-2)
        6. [Signal: XFlow SV 3 & 4](#Signal:-XFlow-SV-3-&-4)
        7. [Signal: Flow SV 5](#Signal:-Flow-SV-5)
        8. [Signal: OA1 Off](#Signal:-OA1-Off)
        9. [Signal: OA2 Off](#Signal:-OA2-Off)
    2. [Communications Tests](#Communication-Tests)
        1. [Comms: VCM](#Comms:-VCM)
        2. [Comms: CPLD](#Comms:-CPLD)
        3. [Comms: SD Card](#Comms:-SD-Card)
        4. [Comms: USB](#Comms:-USB)
        5. [Comms: Exhl I2C](#Comms:-Exhl-I2C)
        6. [Comms: XFlow I2C](#Comms:-XFlow-I2C)
        7. [Comms: XFlow SPI](#Comms:-XFlow-SPI)
        8. [Comms: Flow I2C](#Comms:-Flow-I2C)
        9. [Comms: Flow SPI](#Comms:-Flow-SPI)
        10. [Comms: OA1 I2C](#Comms:-OA1-I2C)
        11. [Comms: OA2 I2C](#Comms:-OA2-I2C)
        12. [Comms: Metering Valve I2C](#Comms:-Metering-Valve-I2C)
    3. [Volage Tests](#Voltage-Tests)
        1. [Voltage: 3V3_HOT](#Voltage:-3V3_HOT)
        2. [Voltage: 5V0_HOT](#Voltage:-5V0_HOT)
        3. [Voltage: 5V3 SMPS](#Voltage:-5V3-SMPS)
        4. [Voltage: 12V0 SMPS](#Voltage:-12V0-SMPS)
        5. [Voltage: 3V3 SMPS](#Voltage:-3V3-SMPS)
        6. [Voltage: 1V2 SMPS](#Voltage:-1V2-SMPS)
        7. [Voltage: 2V048 LDO](#Voltage:-2V048-LDO)
        8. [Voltage: 3V3 LDO](#Voltage:-3V3-LDO)
        9. [Voltage: 30V SMPS](#Voltage:-30V-SMPS)
        10. [Voltage: 36V SMPS](#Voltage:-36V-SMPS)
        11. [Voltage: Low Speed Fan](#Voltage:-Low-Speed-Fan)
    4. [Frequency Tests](#Frequency-Tests)
        1. [Frequency: Low Speed Fan](#Frequency:-Low-Speed-Fan)
    5. [LED Tests](#LED-Tests)
        1. [LED: Power Button](#LED:-Power-Button)
        2. [LED: Charge](#LED:-Charge)
        3. [LED: XDC](#LED:-XDC)
        4. [LED: INOP](#LED:-INOP)
    6. [Sensor Tests](#Sensor-Tests)
        1. [Sensor: Barometer](#Sensor:Barometer)
        2. [Sensor: Thermometer](#Sensor:-Thermometer)
        3. [Sensor: Buttons](#Sensor:-Buttons)
    7. [Solenoid Tests](#Solenoid-Tests)
        1. [Solenoid: SV9 Off](#Solenoid:-SV9-Off)
        2. [Solenoid: SV10 Off](#Solenoid:-SV10-Off)
        3. [Solenoid: SV9 On](#Solenoid:-SV9-On)
        4. [Solenoid: SV10 On](#Solenoid:-SV10-On)
    8. [Motor Tests](#Motor-Tests)
        1. [Motor: Blower](#Motor:-Blower)
        2. [Motor: Rotary Valve 1](#Motor:-Rotary-Valve-1)
        3. [Motor: Rotary Valve 2](#Motor:-Rotary-Valve-2)
        4. [Motor: Metering Valve](#Motor:-Metering-Valve)
        5. [Motor: Pump](#Motor:-Pump)
    9. [Charger Tests](#Charger-Tests)
        1. [Charger: Charge Monitor Test](#Charger:-Charge-Monitor-Test)
    10. [Power Tests](#Power-Tests)
        1. [Power: Ext GT14 OK](#Power:-Ext-GT14-OK)
        2. [Power: VPPO OK](#Power:-VPPO-OK)
        3. [Power: XDC OK](#Power:-XDC-OK)
        4. [Power: CPLD Supply](#Power:-CPLD-Supply)
        5. [Power:Supercap](#Power:-Supercap)
    11. [Battery Tests](#Battery-Tests)
        1. [Battery: Temp Test](#Battery-Temp)
        2. [Battery: Comms Test](#Battery:-Comms-Test)
        3. [Battery: IB Diode](#Battery:-IB-Diode)
        4. [Battery: IB Source](#Battery:-IB-Source)
        5. [Battery: IB Charge](#Battery:-IB-Charge)
        6. [Battery: XB1 Diode](#Battery:-XB1-Diode)
        7. [Battery: XB1 Source](#Battery:-XB1-Source)
        8. [Battery: XB1 Charge](#Battery:-XB1-Charge)
        9. [Battery: XB2 Diode](#Battery:-XB2-Diode)
        10. [Battery: XB2 Source](#Battery:-XB2-Source)
        11. [Battery: XB2 Charge](#Battery:-XB2-Charge)
    12. [Alarm Tests](#Alarm-Tests)
        1. [Alarms: Nurse Call](#Alarms:-Nurse-Call)
        2. [Alarms: Speaker](#Alarms:-Speakers)
        3. [Alarms: Piezo](#Alarms:-Piezo)
        4. [Alarms: System Fault 9](#Alarms:-System-Fault-9)


## Functional Description
## Database Tables
The control board test station utilizes a SQLite database with three tables.

* <b>Test_Instance:</b> Table to store the test entry. A test is defined as a group of tests performed on a single control board after one press of "Run" button. This provide a group of tests to be looked at using the TestID of the single test. Contains info for a <em>whole</em> test instance, does not contain information

| TEST_ID | EQID | USER | LOCATION | TIMESTAMP | SERIAL_NUMBER | RESULT |
| ------- | ---- | ---- | -------- | --------- | ------------- | ------ |
| Unique auto-incrementing key | EQID of the test station, specified in the settings.xml file | User performing test, only users who have been added to the database can run tests | Location of test station, specified in the settings.xml file | Timestamp when the test was initiated | Serial number of the device entered by user | Result of the test |

* <b>Tests:</b> Table to store all tests run with the measurements taken and the results of the test. This table should be used in conjunction with Test_Instance by linking the Test_IDs together.

| TEST_ID | SERIAL_NUMBER | TEST_NAME | UPPER_BOUND | LOWER_BOUND | MEASURED | RESULT |
| ------- | ------------- | --------- | ----------- | ----------- | -------- | ------ |
| Matched with the test instance entry | Serial entered before test | Name of test | Upper limit, if specified | Lower limit, if specified | measured value | Pass/Fail result

* <b>USERS:</b> This table stores username, password and privelage data. Admin users are able to add users to this table. Any user can change their password. The default username and password is
    * Username: admin
    * Password: password

| USERNAME | PASSWORD | ADMIN |
| -------- | -------- | ----- |
| Username of the user's choice | Password chosen by user. No limits | Privelages, the options are: true or false |

## Tests

### Signal Tests
There are 9 tests that are focused digital signals on the control board
#### Signal: LCD Test
XML Entry
```xml
<test name="Signal: LCD Test" method_name="test_lcd" qual="true"/>
```
* <b>Circuit Tested:</b>  3B
* <b>Description</b>: This test prompts the user to check to the screen and confirm that it is clear. The user is presented with two choices; Yes or No. If the user responds with a "Yes" the test continues with a passing result. If the user responds with a "No" the test prompts the user to describe the screen failure. The measurement for this test is a "PASS" or the failure described by the user.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>Pass</b> or <b>User Defined Failure</b>


### Motor: High Speed Fan
```xml
<test name="Motor: High Speed Fan" method_name="test_high_fan" qual="false" upper="12.5" lower="10.5" frequency="175" tolerance="15"/>
```
* <b>Circuit Tested:</b>  4XF
* <b>Description</b>: This test enables the high speed fan by turning on nebulizer therapy (running the pump) and measuring the fan voltage an CN400X pin 2. This test performs the following operations in order:
   * Prompt user to clear alarms.
   * Restart the ventilator.
   * Navigate to the nebulizer therapy screen
   * Prompt user to begin nebulizer therapy.
   * Measure the voltage of the high speed fan using the DMM. This is retrieved over an RS232 serial connection.
   * Determine pass fail criteria based on the specified values 
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts


### Motor: Cough Valve
```xml 
<test name="Motor: Cough Valve"  method_name="test_cough_valve" qual="true" toggle="10"/>
```
* <b>Circuit Tested:</b>  12D
* <b>Description</b>:  
This test toggles the cough valve using the port 5000 telnet command "set vcm coughv 0" and "set vcm coughv 1". The command will toggle the cough valve a set number of times, then prompt the user for feedback on whether the cough valve actuated.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>Pass</b> or <b>Fail</b>

### Alarms: Nurse Call
```xml 
    <test name="Alarms: Nurse Call" method_name="test_nurse_call" qual="true" upper="5" lower="0"/>
```
* <b>Circuit Tested:</b> 2XG
* <b>Description:</b>  
    This test measures the resistance between:

    * Normally Closed (4) and Common (2 & 3) on CN200X
        * Expected to be a short connection
    * Normally Open (1) and Common (2 & 3) on CN200X
        * Expected to be an open connection<br>
        
    Then an alarm condition is created by disconnecting the fan frequency pin and waiting for the device to alarm. Then software will measure the resistance between:

    * Normally Closed (4) and Common (2 & 3) on CN200X
        * Expected to be an open connection
    * Normally Open (1) and Common (2 & 3) on CN200X
        * Expected to be a short connection<br>
* <b>Unique Database Entry Info:</b>
    * Upper Bound: upper limit specified in the xml entry
    * Lower Bound: lower limit specified in the xml entry
    * Measured: <b>Pass</b> or <b>Fail</b>

### Alarm: Speaker
```xml
    <test name="Alarm: Speaker" method_name="test_speaker" qual="true"/>
```
* <b>Circuit Tested:</b> 3E
* <b>Description:</b>  
    This test enables a connection between the DUT and the front panel speaker. Then test then creates an alarm condition by restarting the ventilator. The ventilator should begin to alarm, and sound the speaker.
    The test will then prompt the user and ask if the speaker sounded. The user may select 'Yes' or 'No'

    Yes = PASS  
    No = FAIL

* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>Pass</b> or <b>Fail</b>

### Alarm: Piezo
```xml 
    <test name="Alarms: Piezo" method_name="test_piezo" qual="true"/>
```
* <b>Circuit Tested:</b> 2XH
* <b>Description:</b>  
    This test enables a connection between the DUT and the piezo buzzer. The test then creates an alarm condition by restarting the ventilator. The ventilator should begin to alarm, and sound the speaker. After the speaker does NOT sound, the piezo buzzer should begin to alarm. 
    The test will then prompt the user and ask if the piezo sounded. The user may select 'Yes' or 'No'.

    Yes = PASS  
    No = FAIL

* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>Pass</b> or <b>Fail</b>

### Sensor: Microphone
```xml 
    <test name="Sensor: Microphone" method_name="test_microphone" qual="true"/>
```
* <b>Circuit Tested:</b> 3F
* <b>Description:</b>  
    This test enables a connection between the DUT and the front panel speaker. Then creates an alarm condition by restarting the ventilator. The ventilator should begin to alarm. After 10s the test will enable the piezo buzzer, then prompt the user and ask if the piezo buzzer sounded. The user may select 'Yes' or 'No'.

    Yes = FAIL  
    No = PASS

* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>Pass</b> or <b>Fail</b>

### LED: Alarm Silence
```xml 
    <test name="LED: Alarm Silence"  method_name="test_as_led" qual="true" timeout="60" fs="100" time="10"/>
```
* <b>Circuit Tested:</b> 3L
* <b>Description:</b>  
    This test triggers the Audio Pause button by connecting the two pins together. It then reads the audio pause LED cathode using the GPIO input for the number of seconds specified by 'time' in the xml entry at a rate of 'fs' also specified in the xml entry. The measured values are stored and averaged together to get a representative duty cycle of the LED. The duty cycle is stored in the measurement entry. If the LED goes low at any time, this test passes.

* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: Duty cycle obtained from sampling the LED cathode.

### Comms: VCM
```xml 
    <test name="Comms: VCM" method_name="test_vcm_rev" qual="true" rev="4.06.05R"/>
```
* <b>Circuit Tested:</b> 6A
* <b>Description:</b>  
    This test enables reads the VCM software version from the DUY with port 5000 command "get vcm version" over telnet. It then compares the response to the specified 'rev' in the xml entry. If the software version returned matches the revision specified, this test passes.

* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: VCM Software version

### Comms: CPLD
```xml 
    <test name="Comms: CPLD" method_name="test_cpld_rev" qual="true" rev="V12"/>
```
* <b>Circuit Tested:</b> 2XM
* <b>Description:</b>  
    This test enables reads the CPLD firmware version from the DUY with port 5000 command "get vcm cpld 9" and "get vcm cpld a" over telnet. It combines the two responses to get the version number and then compares the response to the specified 'rev' in the xml entry. If the firmware version returned matches the revision specified, this test passes.

* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: CPLD firmware version

### Comms: SD Card
```xml 
    <test name="Comms: SD Card" method_name="test_sd_card" qual="true" filename="fct_test.txt"/>
```
* <b>Circuit Tested:</b> 3H
* <b>Description:</b>  
    This test generates a random integer and commands the DUT to write that integer to a new file on the SD card. Then the test commands the DUT to read that file back using the 'cat' command. The test then compares the value read back to the value originally generated. If the values match, the test passes, otherwise, the test fails

* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>



### Comms: USB
```xml 
    <test name="Comms: USB" method_name="test_usb" qual="true" filename="fct_test.txt"/>
```
* <b>Circuit Tested:</b> 3D
* <b>Description:</b>  
    This test generates a random integer and commands the DUT to write that integer to a new file on the USB drive. Then the test commands the DUT to read that file back using the 'cat' command. The test then compares the value read back to the value originally generated. If the values match, the test passes, otherwise, the test fails

* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Comms: Exhal I2C
```xml 
    <test name="Comms: Exhl I2C"  method_name="test_exhl_i2c" qual="true"/>
```
* <b>Circuit Tested:</b> 12E
* <b>Description:</b>  
    This test commands the DUT to read the calibration data from the EEPROM on the Exhalation target board. The response is parsed to determine if the there are any i2c errors during this request. If there are none, this test passes.

* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Comms: XFlow I2C
```xml 
    <test name="Comms: XFlow I2C"  method_name="test_xflow_i2c" qual="true" i2c_addr="0x50"/>
```
* <b>Circuit Tested:</b> 5C
* <b>Description:</b>  
    This test commands the DUT to read the calibration data from the EEPROM on the External Flow target board. The response is parsed to determine if the there are any i2c errors during this request. If there are none, this test passes.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>
    
### Comms: XFlow SPI
```xml 
    <test name="Comms: XFlow SPI"  method_name="test_xflow_spi" qual="true" samples="100"/>
```
* <b>Circuit Tested:</b> 5C
* <b>Description:</b>  
    This test commands the DUT to read telemetry data from the DSensor:PdiffExtWideRaw_counts channel. The test gets a specified number samples and averages them. The test compares each value read for a -1 value. If the value is -1, the test fails.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Comms: Flow I2C
```xml 
   <test name="Comms: Flow I2C"  method_name="test_flow_i2c" qual="true" i2c_addr="0x55"/>
```
* <b>Circuit Tested:</b> 5D
* <b>Description:</b>  
    This test commands the DUT to read the calibration data from the EEPROM on the Flow target board. The response is parsed to determine if the there are any i2c errors during this request. If there are none, this test passes.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Comms: Flow SPI
```xml 
    <test name="Comms: Flow SPI"  method_name="test_flow_spi" qual="true" samples="100"/>
```
* <b>Circuit Tested:</b> 5D
* <b>Description:</b>  
    This test commands the DUT to read telemetry data from the DSensor:PdiffIntWideRaw_counts channel. The test gets a specified number of samples and averages them. The test compares each value read for a -1 value. If the value is -1, the test fails.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Comms: OA1 I2C
```xml 
    <test name="Comms: OA1 I2C"  method_name="test_oa1_i2c" qual="true"/>
```
* <b>Circuit Tested:</b> 5A
* <b>Description:</b>  
    This test commands the DUT to read the calibration data from the EEPROM on the OAX target board. The response is parsed to determine if the there are any i2c errors during this request. If there are none, this test passes.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Comms: OA2 I2C
```xml 
    <test name="Comms: OA2 I2C"  method_name="test_oa2_i2c" qual="true"/>
```
* <b>Circuit Tested:</b> 5B
* <b>Description:</b>  
    This test commands the DUT to read the calibration data from the EEPROM on the OAX target board. The response is parsed to determine if the there are any i2c errors during this request. If there are none, this test passes.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Comms: Metering Valve I2C
```xml 
    <test name="Comms: Metering Valve I2C"  method_name="test_mv_i2c" qual="true"/>  
```
* <b>Circuit Tested:</b> 10B
* <b>Description:</b>  
    This test commands the DUT to read the calibration data from the EEPROM on the Metering Valve target board. The response is parsed to determine if the there are any i2c errors during this request. If there are none, this test passes.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Voltage: 3V3_HOT
```xml 
    <test TPID ="2XB" name="Voltage: 3V3_HOT" method_name="test_3V3_HOT" qual="false" upper="3.382" lower="3.217"/>
```
* <b>Circuit Tested:</b> 2XB
* <b>Description:</b>  
    This test measures the voltage at TP202X.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Voltage: 5V0_HOT
```xml 
    <test TPID ="2XD" name="Voltage: 5V0_HOT" method_name="test_5V0_HOT" qual="false" upper="5.125" lower="4.875"/>
```
* <b>Circuit Tested:</b> 2XD
* <b>Description:</b>  
    This test measures the voltage at TP203X.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Voltage: 5V3 SMPS
```xml 
    <test TPID ="2A"  name="Voltage: 5V3 SMPS" method_name="test_5V3_SMPS" qual="false" upper="5.432" lower="5.167"/>
```
* <b>Circuit Tested:</b> 2A
* <b>Description:</b>  
    This test measures the voltage at TP217.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Voltage: 12V0 SMPS
```xml 
    <test TPID ="2D"  name="Voltage: 12V0 SMPS" method_name="test_12V0_SMPS" qual="false" upper="12.5" lower="11.7"/>
```
* <b>Circuit Tested:</b> 2D
* <b>Description:</b>  
    This test measures the voltage at TP201.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Voltage: 3V3 SMPS
```xml 
    <test TPID ="2B"  name="Voltage: 3V3 SMPS" method_name="test_3V3_SMPS" qual="false" upper="3.382" lower="3.217"/>
```
* <b>Circuit Tested:</b> 2B
* <b>Description:</b>  
    This test measures the voltage at TP210.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Voltage: 1V2 SMPS
```xml 
    <test TPID ="2C"  name="Voltage: 1V2 SMPS" method_name="test_1V2_SMPS" qual="false" upper="1.4" lower="1.0"/>
```
* <b>Circuit Tested:</b> 2C
* <b>Description:</b>  
    This test measures the voltage at TP214.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Voltage: 2V048 LDO
```xml 
    <test TPID ="2G"  name="Voltage: 2V048 LDO" method_name="test_VREF" qual="false" upper="2.064" lower="2.030"/>
```
* <b>Circuit Tested:</b> 2G
* <b>Description:</b>  
    This test measures the voltage at TP200.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Voltage: 3V3 LDO
```xml 
    <test TPID ="2XB" name="Voltage: 3V3 LDO" method_name="test_3V3_LDO" qual="false" upper="3.382" lower="3.217"/>
```
* <b>Circuit Tested:</b> 2F
* <b>Description:</b>  
    This test measures the voltage at TP226.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Voltage: 30V SMPS
```xml 
    <test TPID ="2XB" name="Voltage: 30V SMPS" method_name="test_30V_SMPS" qual="false" upper="33.0" lower="27.0"/>
```
* <b>Circuit Tested:</b> 4XG
* <b>Description:</b>  
    This test measures the voltage at TP411X.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Voltage: 36V SMPS
```xml 
    <test TPID ="2XB" name="Voltage: 36V SMPS" method_name="test_36V_SMPS" qual="false" upper="39.0" lower="33.0"/>
```
* <b>Circuit Tested:</b> 5XA
* <b>Description:</b>  
    This test measures the voltage at TP511X.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### LED: Power Button
```xml 
    <test name="LED: Power Button" method_name="test_pb_led" qual="true"/>
```
* <b>Circuit Tested:</b> 3K
* <b>Description:</b>  
    This test measures the voltage at CN309m pin 5. This signal represents the cathode of the Power On LED. If the voltage is LOW, then the LED is assumed to be on and the test will pass.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### LED: Charge
```xml 
    <test name="LED: Charge"  method_name="test_charge_led" qual="true" timeout="60"/>
```
* <b>Circuit Tested:</b> 3J
* <b>Description:</b>  
    This test measures the voltage at CN309m pin 18. The test connects a external battery to the DUT and waits for the pin to go low. This signal represents the cathode of the Charge LED. If the voltage is LOW, then the LED is assumed to be on and the test will pass. The test ends by disconnecting the battery.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### LED: XDC
```xml 
    <test name="LED: XDC"  method_name="test_ext_led" qual="true" timeout="30"/>
```
* <b>Circuit Tested:</b> 3M
* <b>Description:</b>  
    This test measures the voltage at CN309m pin 19. This signal represents the cathode of the External Power LED. If the voltage is LOW, then the LED is assumed to be on and the test will pass. The test connects external power to the DUT and waits for the LED to turn on.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>
### LED: INOP
```xml 
    <test name="LED: INOP"  method_name="test_inop_led" qual="true" timeout="60" />
```
* <b>Circuit Tested:</b> 2XJ
* <b>Description:</b>  
    This test measures the voltage at CN309m pin 5. This signal represents the cathode of the Power On LED. If the voltage is LOW, then the LED is assumed to be on and the test will pass.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>
### Voltage: Low Speed Fan
```xml 
    <test name="Voltage: Low Speed Fan" method_name="test_low_fan_volt" qual="false" upper="7.5" lower="7.1"/>
```
* <b>Circuit Tested:</b> 4XF
* <b>Description:</b>  
    This test measures the voltage at CN400X pin 2 while in low speed operation.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts
### Frequency: Low Speed Fan
```xml 
    <test name="Frequency: Low Speed Fan" method_name="test_low_fan_freq" qual="false" upper="125" lower="100"/>
```
* <b>Circuit Tested:</b> 4XF
* <b>Description:</b>  
    This test measures the frequency of the fan at CN400X.1 while in low speed operation
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Hz
    * Lower Bound: Hz
    * Measured: Hz

### Sensor: Barometer
```xml 
    <test name="Sensor: Barometer" method_name="test_barometer" qual="false" samples ="100" upper="105" lower="99"/>
```
* <b>Circuit Tested:</b> 5E
* <b>Description:</b>  
    This test sets the telemetry channel to the "Sensor:PAmbient_cmH2O_100" channel, then requests the specified number of samples from the device. The test averages the samples together and provides the averaged value as the measurement.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: kPa
    * Lower Bound: kPa
    * Measured: kPa

### Sensor: Thermometer
```xml 
    <test name="Sensor: Thermometer" method_name="test_ambient_temperature" qual="false" samples="100" upper="45" lower="15"/>
```
* <b>Circuit Tested:</b> 5E
* <b>Description:</b>  
    This test sets the telemetry channel to the "Sensor:Tambient_C_100" channel, then requests the specified number of samples from the device. The test averages the samples together and provides the averaged value as the measurement.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: degrees C
    * Lower Bound: degrees C
    * Measured: degrees C

### Sensor: Buttons
```xml 
    <test name="Sensor: Buttons" method_name="test_buttons" qual="true" port="FirstPortCH" pin="5"/>   
```
* <b>Circuit Tested:</b> 2XK
* <b>Description:</b>  
    This test tests each button seperately by activating the button using a relay. Each button is confirmed to work by requesting info from the DUT over port 500 using "get vcm buttons". The response is parsed to determine which button is active. If the the button that is currently being tested is active, the corresponding bit is set on the measurement.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: Hex value representing the buttons that function (0x10 = Power Button | 0x01 = Audio Pause Button)

### Solenoid: SV9 Off
```xml 
    <test name="Solenoid: SV9 Off" method_name="test_sv9_off" qual="false" upper="12.5" lower="10.5"/>
```
* <b>Circuit Tested:</b> 12A
* <b>Description:</b>  
    This test sets SV9 to off with the port 5000 command "set vcm sv 9 0". The test then measures the voltage at CN1200.2. 
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Solenoid: SV10 Off
```xml 
    <test name="Solenoid: SV10 Off" method_name="test_sv10_off" qual="false" upper="12.5" lower="10.5"/>
```
* <b>Circuit Tested:</b> 12B
* <b>Description:</b>  
    This test sets SV10 to off with the port 5000 command "set vcm sv 10 0". The test then measures the voltage at CN1200.3. 
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Solenoid: SV9 On
```xml 
    <test name="Solenoid: SV9 ON" method_name="test_sv9_on" qual="false" upper="0.3" lower="-0.3"/>
```
* <b>Circuit Tested:</b> 12A
* <b>Description:</b>  
    This test sets SV9 to on with the port 5000 command "set vcm sv 9 1". The test then measures the voltage at CN1200.2.  After measuring voltage, the test turns the solenoid off.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Solenoid: SV10 On
```xml 
    <test name="Solenoid: SV10 ON" method_name="test_sv9_on" qual="false" upper="0.3" lower="-0.3"/>
```
* <b>Circuit Tested:</b> 12B
* <b>Description:</b>  
    This test sets SV9 to on with the port 5000 command "set vcm sv 10 1". The test then measures the voltage at CN1200.3.  After measuring voltage, the test turns the solenoid off.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Solenoid: Shut-Off Valve
```xml 
    <test name="Solenoid: Shut-Off Valve" method_name="test_sov" qual="true" on_state="low" off_state ="high"/>
```
* <b>Circuit Tested:</b> 12C
* <b>Description:</b>  
    This test measures the SOV- node at CN1203.3. The test then activates the Shut-Off Valve with port 5000 command "set vcm sv 11 1", then measures the SOV-node at CN1203.3. This node is active low.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Signal: Exhl SV 6
```xml 
    <test name="Signal: Exhl SV 6" method_name="test_exhl_sv6" qual="true" toggle="10" upper="10" lower="10"/>
```
* <b>Circuit Tested:</b> 12E
* <b>Description:</b>  
    This test commands the DUT with port 5000 command "set vcm sv 6 1", then measures the level of the signal at CN1202.3 to confirm that it is HIGH. Then commands the DUT with "set vcm sv 6 0" and measures the signal at CN1202.3 to confirm that it is LOW. The test repeats this for a specified number of times.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: Toggle Count

### Signal: Exhl SV 7
```xml 
    <test name="Signal: Exhl SV 7" method_name="test_exhl_sv7" qual="true" toggle="10" upper="10" lower="10"/>
```
* <b>Circuit Tested:</b> 12E
* <b>Description:</b>  
    This test commands the DUT with port 5000 command "set vcm sv 7 1", then measures the level of the signal at CN1202.1 to confirm that it is HIGH. Then commands the DUT with "set vcm sv 7 0" and measures the signal at CN1202.1 to confirm that it is LOW. The test repeats this for a specified number of times.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: Toggle Count

### Signal: Exhl SV 8
```xml 
    <test name="Signal: Exhl SV 8" method_name="test_exhl_sv8" qual="true" toggle="10" upper="10" lower="10" />
```
* <b>Circuit Tested:</b> 12E
* <b>Description:</b>  
    This test commands the DUT with port 5000 command "set vcm sv 8 1", then measures the level of the signal at CN1202.5 to confirm that it is LOW. Then commands the DUT with "set vcm sv 8 0" and measures the signal at CN1202.5 to confirm that it is LOW. The test repeats this for a specified number of times.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: Toggle Count

### Signal: XFlow SV 1 & 2
```xml 
    <test name="Signal: XFlow SV 1&amp;2" method_name="test_xflow_sv1" qual="false" toggle="10" upper="10" lower="10"/>
```
* <b>Circuit Tested:</b> 5C
* <b>Description:</b>  
    This test commands the DUT with port 5000 command "set vcm sv 1 1", then measures the level of the signal at CN501.11 to confirm that it is HIGH. Then commands the DUT with "set vcm sv 1 0" and measures the signal at CN501.11 to confirm that it is LOW. The test repeats this for a specified number of times. Note, SV 1 and SV2 are hardware tied together.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: Toggle Count

### Signal: XFlow SV 3 & 4
```xml 
    <test name="Signal: XFlow SV 3&amp;4" method_name="test_xflow_sv3" qual="false" toggle="10" upper="10" lower="10"/>
```
* <b>Circuit Tested:</b> 5C
* <b>Description:</b>  
    This test commands the DUT with port 5000 command "set vcm sv 3 1", then measures the level of the signal at CN501.17 to confirm that it is HIGH. Then commands the DUT with "set vcm sv 3 0" and measures the signal at CN501.17 to confirm that it is LOW. The test repeats this for a specified number of times. Note, SV 3 and SV 4 are hardware tied together.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: Toggle Count

### Signal: Flow SV 5
```xml 
    <test name="Signal: Flow SV 5" method_name="test_flow_sv5" qual="false" toggle="10" upper="10" lower="10"/>
```
* <b>Circuit Tested:</b> 5D
* <b>Description:</b>  
    This test commands the DUT with port 5000 command "set vcm sv 5 1", then measures the level of the signal at CN500.11 to confirm that it is HIGH. Then commands the DUT with "set vcm sv 5 0" and measures the signal at CN500.11 to confirm that it is LOW. The test repeats this for a specified number of times. Note, SV 3 and SV 4 are hardware tied together.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: Toggle Count

### Signal: OA1 Off
```xml 
    <test name="Signal: OA1 Off"  method_name="test_oa1_off" qual="true"/>
```
* <b>Circuit Tested:</b> 5A
* <b>Description:</b>  
    This test measures the signal at CN503.3 and confirm that the value is LOW.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Signal: OA2 Off
```xml 
    <test name="Signal: OA2 Off"  method_name="test_oa2_off" qual="true"/>
```
* <b>Circuit Tested:</b> 5B
* <b>Description:</b>  
    This test measures the signal at CN502.3 and confirm that the value is LOW.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Motor: Blower
```xml 
    <test name="Motor: Blower" method_name="test_blower" qual="false" speed="20000" upper="22000" lower="18000" tolerance="10"/>
```
* <b>Circuit Tested:</b> 7A
* <b>Description:</b>  
    This test commands the DUT to spin the blower motor at the specified speed with the port 5000 command <em>"set vcm testmgr speed \<speed\>"</em>. The test then measures the frequency of the hall effect signal connected to Phase A of the motor at CN700.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: RPM
    * Lower Bound: RPM
    * Measured: RPM

### Motor: Rotary Valve 1
    ```xml 
    <test name="Motor: Rotary Valve 1"  method_name="test_rotary_valve_1" qual="true"/>
```
* <b>Circuit Tested:</b> 9A, 9B
* <b>Description:</b>  
    This test commands the RV1 to the home position with command <em>"set vcm rotaryv 1 0"</em>. Then the test commands the RV1 to position 4 with <em>"set vcm rotaryv 1 4"</em> and measures the signal at CN901.3 and confirms that it is LOW.
    Next the test commands the RV1 to position 2 (home position) with command <em>"set vcm rotaryv 1 2"</em>. Then comand the RV1 to home position again with <em>"set vcm rotaryv 1 0"</em>. Then measure the signal at CN901.3 and confirm that the value is HIGH.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Motor: Rotary Valve 2
```xml 
    <test name="Motor: Rotary Valve 2"  method_name="test_rotary_valve_2" qual="true"/>
```
* <b>Circuit Tested:</b> 11A, 11B
* <b>Description:</b>  
    This test commands the RV2 to the home position with command <em>"set vcm rotaryv 2 0"</em>. Then the test commands the RV2 to position 4 with <em>"set vcm rotaryv 2 4"</em> and measures the signal at CN1101.3 and confirms that it is LOW.
    Next the test commands the RV2 to position 2 (home position) with command <em>"set vcm rotaryv 2 2"</em>. Then comand the RV2 to home position again with <em>"set vcm rotaryv 2 0"</em>. Then measure the signal at CN901.3 and confirm that the value is HIGH.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Motor: Metering Valve
```xml 
    <test name="Motor: Metering Valve"  method_name="test_metering_valve" qual="true"/>
```
* <b>Circuit Tested:</b> 9A, 9B
* <b>Description:</b>  
    This test commands the MV to position 1000 with command <em>"set vcm metering pos 1000"</em> and measures the signal at CN1001.7 and confirms that it is HIGH.
    Next the test commands the MV to the home position with command <em>"set vcm metering home"</em> and measure the signal at CN1001.7 and confirm that the value is LOW.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Motor: Pump
```xml 
    <test name="Motor: Pump" method_name="test_pump" qual="false" speed="950" upper="1200" lower="800" tolerance="20"/>   
```
* <b>Circuit Tested:</b> 8A
* <b>Description:</b>  
    This test commands the DUT to spin the pump motor at the specified speed with the port 5000 command <em>"set vcm testmgr o2speed \<speed\>"</em>. The test then measures the frequency of the hall effect signal connected to Phase A of the motor at J00.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: RPM
    * Lower Bound: RPM
    * Measured: RPM

### Charger: Charge Monitor Test
```xml 
    <test name="Charger: Charge Monitor Test" method_name="test_chg_monitor" qual="true" samples="100" upper="18" lower="12"/>
```
* <b>Circuit Tested:</b> 3XH
* <b>Description:</b>  
    This test enables a charging scenario with a battery, then queries the DUT for telemetry samples using channel <em>Sensor:VchargeMonitor_F_mv</em>. The specified number of samples is gathered and averaged together then compared to the upper and lower limits.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Power: Ext GT14 OK
```xml 
    <test name="Power: Ext GT14 OK" method_name="test_ext_gt14_ok" qual="true"/>
```
* <b>Circuit Tested:</b> 2XF
* <b>Description:</b>  
    This test queries the DUT while running on external DC power with <em>"get vcm cpld 0c"</em>. The response is parsed for the GT14 OK flag. If the flag is set then the test passes.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: N/A

### Power: VPPO OK
```xml 
    <Test name="Power: VPPO OK" method_name="test_vppo_ok" qual="true" samples="100"/>
```
* <b>Circuit Tested:</b> 2XF
* <b>Description:</b>  
    This test queries the DUT while running on external DC power with <em>"get vcm cpld 0c"</em>. The response is parsed for the VPPO flag. If the flag is set then the test passes.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: N/A

### Power: VPPO Monitor
```xml 
    <test name="Power: VPPO Monitor" method_name="test_vppo_monitor" qual="true" samples="100"/>
```
* <b>Circuit Tested:</b> 3XC
* <b>Description:</b>  
    This test enables the external ac power then queries the DUT for telemetry samples using channel <em>Sensor:VppoMonitor_F_mv</em>. The specified number of samples is gathered and averaged together then compared to the upper and lower limits.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Power: XDC Diode
```xml 
    <test name="Power: XDC Diode" method_name="test_extdc_diode" qual="true" tolerance="50" upper="26" lower="22" samples="100"/>
```
* <b>Circuit Tested:</b> 4XD
* <b>Description:</b>  
    This test enables the external power source and then monitors the VPPO monitor for the specified number of samples. The test averages the mV value and compares to the specified upper and lower limts.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Power: CPLD Supply
```xml 
<test name="Power: CPLD Supply" method_name="test_cpld_diode" qual="true"/>
```
* <b>Circuit Tested:</b> 2XA
* <b>Description:</b>  
    This test enables each source individually and queries the DUT with port 5000 command <em>"get vcm power"</em>. The response is the  parsed to get the OK flags for each source and the each source is confirmed to be the only OK flag that is present.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Power: Supercap
```xml 
    <test name="Power: Supercap" method_name="test_supercap" qual="false" upper="5.3" lower="4.5"/>
```
* <b>Circuit Tested:</b> 2XC
* <b>Description:</b>  
    This test powers down the device by removing external AC power and measures the voltage at PZ200X.1 while the device is off. The test compares the measured value to the upper and lower limits, then powers the device back on and re-conencts to the device over telnet.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: Volts
    * Lower Bound: Volts
    * Measured: Volts

### Battery: Temp Test
```xml 
    <test name="Battery: Temp Test" method_name="test_batt_temp" qual="true" index="6" upper="3500" lower="2832"/>
```
* <b>Circuit Tested:</b> 2XL
* <b>Description:</b>  
    This test queries the DUT for the battery temperature values using <em>"get vcm power"</em>. The response is parsed to get the temperature values reported. The battery queried should be connected to all three battery connectors. The values read are compared to each other to confirm that they are all the same.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Battery: Comms Test
```xml 
    <test name="Battery: Comms Test" method_name="test_batt_comms" qual="true" index="3"/>
```
* <b>Circuit Tested:</b> 2XL
* <b>Description:</b>  
    This test queries the DUT for the battery state-of-charge values using <em>"get vcm power"</em>. The response is parsed to get the ASOC and RSOC values reported. The battery queried should be connected to all three battery connectors. The values read are compared to each other to confirm that they are all the same.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Battery IB Diode
```xml 
    <test name="Battery: IB Diode" method_name="test_batt0_diode" qual="true" tolerance="50" upper="18" lower="15" samples="100"/>
```
* <b>Circuit Tested:</b> 4XC
* <b>Description:</b>  
    This test enables the battery and then monitors the VPPO monitor for the specified number of samples. The test averages the mV value and compares to the specified upper and lower limts.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Battery: IB Source
```xml 
    <test name="Battery: IB Source" method_name="test_batt0_source" qual="true"/>
```
* <b>Circuit Tested:</b> 3XG
* <b>Description:</b>  
    This test enables the battery and pings the UIM and VCM to confirm that the DUT is still operable.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>
    
### Battery: IB Charge
```xml 
    <test name="Battery: IB Charge" method_name="test_batt0_charge" qual="false" delay="1000" timeout="30" upper="1.8" lower="1.2"/>
```
* <b>Circuit Tested:</b> 3XG
* <b>Description:</b>  
    This test enables the battery and external power. The test will measure the current flow into the device until the ammeter reads a current or the specified timeout.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Battery: XB1 Diode
```xml 
    <test name="Battery: XB1 Diode" method_name="test_batt1_diode" qual="true" tolerance="50" upper="18" lower="15" samples="100"/>
```
* <b>Circuit Tested:</b> 4XA
* <b>Description:</b>  
    This test enables the battery and then monitors the VPPO monitor for the specified number of samples. The test averages the mV value and compares to the specified upper and lower limts.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Battery: XB1 Source
```xml 
    <test name="Battery: XB1 Source" method_name="test_batt1_source" qual="true"/>
```
* <b>Circuit Tested:</b> 3XF
* <b>Description:</b>  
    This test enables the battery and pings the UIM and VCM to confirm that the DUT is still operable.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Battery: XB1 Charge
```xml 
    <test name="Battery: XB1 Charge" method_name="test_batt1_charge" qual="false" delay="1000" timeout="30" upper="1.8" lower="1.2"/>
```
* <b>Circuit Tested:</b> 3XE
* <b>Description:</b>  
    This test enables the battery and external power. The test will measure the current flow into the device until the ammeter reads a current or the specified timeout.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Battery: XB2 Diode
```xml 
    <test name="Battery: XB2 Diode" method_name="test_batt2_diode" qual="true" tolerance="50" upper="18" lower="15" samples="100"/>
```
* <b>Circuit Tested:</b> 4XB
* <b>Description:</b>  
    This test enables the battery and then monitors the VPPO monitor for the specified number of samples. The test averages the mV value and compares to the specified upper and lower limts.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Battery: XB2 Source
```xml 
    <test name="Battery: XB2 Source" method_name="test_batt2_source" qual="true"/>
```
* <b>Circuit Tested:</b> 3XF
* <b>Description:</b>  
    This test enables the battery and pings the UIM and VCM to confirm that the DUT is still operable.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>
### Battery: XB2 Charge
```xml 
    <test name="Battery: XB2 Charge" method_name="test_batt2_charge" qual="false" delay="1000" timeout="30" upper="1.8" lower="1.2"/> 
```
* <b>Circuit Tested:</b> 3XF
* <b>Description:</b>  
    This test enables the battery and external power. The test will measure the current flow into the device until the ammeter reads a current or the specified timeout.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>

### Alarm: System Fault 9
```xml 
    <test name="Alarm: System Fault 9" method_name="test_sysfault9" qual="true" timeout="15000"/>
```
* <b>Circuit Tested:</b> 4XF
* <b>Description:</b>  
    This test queries the DUT for the current alarm status with <em>"get vcm alarm status"</em> and parses the response for the kVentFanFailure alarm. If the alarm is confirmed to be OFF, the test then disconnects the Fan Tach signal to the DUT at CN400X.1 and queries the DUT at a rate of 1Hz for the current alarm status until the kVentFanFailure alarm is active of the test timesout.
* <b>Unique Database Entry Info:</b>
    * Upper Bound: N/A
    * Lower Bound: N/A
    * Measured: <b>PASS</b> or <b>FAIL</b>
