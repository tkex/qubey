

/* ============================================
The original code (see IC2 Library) was modified with own adaptations and customizations by me.

It's been throughout commented for a better understanding and clarity in case more features are added along the line.

As mentioned, a  part of that code is from the I2CDev device library while other parts have been written and modified by me for my custom needs for Unity.
in this file. For the original MPU6050_DMP code, please check out the I2Cdev device library yourself.
===============================================
*/


/* ============================================
I2Cdev device library code is placed under the MIT license

Copyright (c) 2012 Jeff Rowberg

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
===============================================
*/

// Include the necessary libraries for the MPU6050 sensor
#include "I2Cdev.h"
#include "MPU6050_6Axis_MotionApps20.h"

#if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
#include "Wire.h"
#endif

// Declare the MPU6050 object
MPU6050 mpu;

// Define constants for readable quaternion output and interrupt pin
#define OUTPUT_READABLE_QUATERNION
#define INTERRUPT_PIN 2 
#define LED_PIN 13

// Initialize the blink state and DMP ready flag
bool blinkState = false;
bool dmpReady = false;

// Variables to store interrupt status, device status, packet size, and FIFO count
uint8_t mpuIntStatus;    
uint8_t devStatus;       
uint16_t packetSize;     
uint16_t fifoCount;      

// FIFO storage buffer
uint8_t fifoBuffer[64];

// Quaternion and vector containers for storing sensor data
Quaternion q;          
VectorInt16 aa;        
VectorInt16 aaReal;    
VectorInt16 aaWorld;   
VectorFloat gravity;   

// Euler angle and yaw/pitch/roll containers
float euler[3];        
float ypr[3];          

// Flag for indicating MPU interrupt
volatile bool mpuInterrupt = false;

// Interrupt service routine to set the MPU interrupt flag
void dmpDataReady() {
  mpuInterrupt = true;
}

// Sample window width in mS for the sensor data
const int sampleWindow = 50;

// Variable to store the sample count
unsigned int sample;



void setup() {

  // Initialize I2C communication

  // Choose the I2C implementation based on the defined macro
  #if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
    Wire.begin();
    // Set the I2C clock frequency to 400kHz
    Wire.setClock(400000); 
    // Uncomment this line if facing compilation difficulties
    // Wire.setClock(400000); 
  #elif I2CDEV_IMPLEMENTATION == I2CDEV_BUILTIN_FASTWIRE
    Fastwire::setup(400, true);
  #endif

    // Start the serial communication at 115200 baud rate
    Serial.begin(115200);
    // Wait for the serial communication to start
    while (!Serial);

    // Inform the user about initializing I2C devices
    Serial.println(F("Initializing I2C devices..."));
    // Initialize the MPU6050 device
    mpu.initialize();
    // Set the interrupt pin as input
    pinMode(INTERRUPT_PIN, INPUT);

    // Inform the user about testing device connections
    Serial.println(F("Testing device connections..."));
    // Check the MPU6050 connection
    Serial.println(mpu.testConnection() ? F("MPU6050 connection successful") : F("MPU6050 connection failed"));

    // Inform the user about initializing the DMP
    Serial.println(F("Initializing DMP..."));
    // Store the result of the DMP initialization
    devStatus = mpu.dmpInitialize();

    // Set the gyro offsets
    mpu.setXGyroOffset(220);
    mpu.setYGyroOffset(76);
    mpu.setZGyroOffset(-85);
    // Set the acceleration offset
    mpu.setZAccelOffset(1788);

    // If the DMP initialization was successful
    if (devStatus == 0) {
      // Calibrate the accelerometer and gyro
      mpu.CalibrateAccel(6);
      mpu.CalibrateGyro(6);

      // Print the active offsets
      mpu.PrintActiveOffsets();

      // Inform the user about enabling the DMP
      Serial.println(F("Enabling DMP..."));
      // Enable the DMP
      mpu.setDMPEnabled(true);

      // Inform the user about enabling the interrupt detection
      Serial.print(F("Enabling interrupt detection (Arduino external interrupt "));
      Serial.print(digitalPinToInterrupt(INTERRUPT_PIN));
      Serial.println(F(")..."));
      // Attach the interrupt service routine
      attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN), dmpDataReady, RISING);
      // Get the interrupt status
      mpuIntStatus = mpu.getIntStatus();

      // Inform the user that the DMP is ready
      Serial.println(F("DMP ready! Waiting for first interrupt..."));
      // Set the flag to indicate that the DMP is ready
      dmpReady = true;

      // Get the size of the FIFO packet
      packetSize = mpu.dmpGetFIFOPacketSize();
    } 
    else {
      // In case the DMP initialization failed, print the error code.
      Serial.print(F("DMP Initialization failed (error code: "));
      Serial.print(devStatus);
      Serial.println(F(")"));
    }

    // Set the LED_PIN as an output pin.
    pinMode(LED_PIN, OUTPUT);
}


void loop() {

  // Check if DMP is ready
  if (!dmpReady) return;

  // Get the latest packet from FIFO buffer
  if (mpu.dmpGetCurrentFIFOPacket(fifoBuffer)) {

    #ifdef OUTPUT_READABLE_QUATERNION
      // Get quaternion data from the packet
      mpu.dmpGetQuaternion(&q, fifoBuffer);
      
      // Print the quaternion data to serial output
      Serial.print("G");
      Serial.print(q.w);
      Serial.print(",");
      Serial.print(q.x);
      Serial.print(",");
      Serial.print(q.y);
      Serial.print(",");
      Serial.println(q.z);
    #endif

    #ifdef OUTPUT_READABLE_YAWPITCHROLL
      // Get quaternion and gravity data from the packet
      mpu.dmpGetQuaternion(&q, fifoBuffer);
      mpu.dmpGetGravity(&gravity, &q);
      
      // Get yaw, pitch and roll data from the quaternion and gravity data
      mpu.dmpGetYawPitchRoll(ypr, &q, &gravity);
      
      // Print yaw, pitch, and roll data to serial output
      Serial.print("ypr\t");
      Serial.print(ypr[0] * 180 / M_PI);
      Serial.print("\t");
      Serial.print(ypr[1] * 180 / M_PI);
      Serial.print("\t");
      Serial.println(ypr[2] * 180 / M_PI);
    #endif

    #ifdef OUTPUT_READABLE_REALACCEL
      // Get quaternion, gravity, and acceleration data from the packet
      mpu.dmpGetQuaternion(&q, fifoBuffer);
      mpu.dmpGetAccel(&aa, fifoBuffer);
      mpu.dmpGetGravity(&gravity, &q);
      
      // Get linear acceleration data from the acceleration, gravity, and quaternion data
      mpu.dmpGetLinearAccel(&aaReal, &aa, &gravity);
      
      // Print linear acceleration data to serial output
      Serial.print("areal\t");
      Serial.print(aaReal.x);
      Serial.print("\t");
      Serial.print(aaReal.y);
      Serial.print("\t");
      Serial.println(aaReal.z);
    #endif

    #ifdef OUTPUT_READABLE_WORLDACCEL
      // Get the quaternion from the MPU
      mpu.dmpGetQuaternion(&q, fifoBuffer);

      // Get the acceleration data from the MPU
      mpu.dmpGetAccel(&aa, fifoBuffer);

      // Get the gravity data from the MPU based on the quaternion
      mpu.dmpGetGravity(&gravity, &q);

      // Get the linear acceleration data based on the acceleration and gravity data
      mpu.dmpGetLinearAccel(&aaReal, &aa, &gravity);

      // Get the linear acceleration in the world coordinate system based on the linear acceleration data and quaternion
      mpu.dmpGetLinearAccelInWorld(&aaWorld, &aaReal, &q);

      // Print the world linear acceleration data
      Serial.print("World Linear Acceleration\t");
      Serial.print(aaWorld.x);
      Serial.print("\t");
      Serial.print(aaWorld.y);
      Serial.print("\t");
      Serial.println(aaWorld.z);
    #endif


    //This part measures the peak-to-peak amplitude of an analog signal

    // Toggle the state of the LED and set it to the value of blinkState
    blinkState = !blinkState;
    digitalWrite(LED_PIN, blinkState);

    // Flush the Serial buffer
    Serial.flush();

    // Wait for 5 milliseconds
    delay(5);

    // Initialize startMillis to the current time
    unsigned long startMillis = millis();

    // Initialize peakToPeak to 0
    unsigned int peakToPeak = 0;

    // Initialize signalMax to 0
    unsigned int signalMax = 0;

    // Initialize signalMin to 1024
    unsigned int signalMin = 1024;

    // Loop until the current time minus startMillis is less than sampleWindow
    while (millis() - startMillis < sampleWindow) {
      // Read the analog input at pin 0 and store it in sample
      sample = analogRead(0);

      // If sample is less than 1024 (not a spurious reading)
      if (sample < 1024) {
        // If sample is greater than signalMax, set signalMax to sample
        if (sample > signalMax) {
          signalMax = sample;
        } 
        // If sample is less than signalMin, set signalMin to sample
        else if (sample < signalMin) {
          signalMin = sample;
        }
      }
    }

    // Calculate the peak-to-peak amplitude as signalMax minus signalMin
    peakToPeak = signalMax - signalMin;

    // Calculate the voltage as peakToPeak times 5.0 divided by 1024
    float volts = (peakToPeak * 5.0) / 1024;

    // Initialize an empty string "myStringe"
    String myStringe = "";

    // Concatenate the voltage value to myStringe
    myStringe.concat(volts);

    // Initialize the prefix string "prefixSound" to "S"
    String prefixSound = "S";

    // Concatenate volts to prefixSound so value with a "S"-prefix is added
    prefixSound.concat(volts);

    // Print/Send prefixSound to the Serial interface
    Serial.println(prefixSound);

    // Flush the Serial buffer
    Serial.flush();

    // Wait for 5 milliseconds
    delay(5);

  }
}