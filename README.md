



# Qubey - The Interactive Cube

This repository contains the code and design files for an Arduino- and Unity based project that incorporates a MPU6050 gyroscope and accelerator, a MAX9814 sound sensor, and a modeled 3D-file made in Blender with accurate measures for printing a case.  

The project, called "Qubey," is an interactive cube that utilizes the MPU6050 and MAX9814 sensors to respond to movement and sound levels in Unity for creating application such as simulations or games that allows for dynamic visual and audio effects based on the environmental sensor data it captures. Qubey is designed for use in interactive demos. For that, various scripts have been written that allow users to set up their applications in a fast manner in a Framework like manner over the Unity inspector. It's kept modular so Qubey can be enhanced in all sorts of directions.

In short, Qubey is an experimental, multi-sensorial gesture and audio medium for the delicate interaction with computer based systems to adapt and enhance learning, motor activity and sensoral feedback in the domain of assistive technology. It enables you to interact with mediums in a multisensory manner where sound recognition and motor activity come together for a truly immersive experience.

![Qubey-1](https://github.com/tkex/qubey/blob/main/images/qubey.jpg)

![Qubey-2](https://github.com/tkex/qubey/blob/main/images/qubey-2.jpg)

# For who is it

Qubey is a tool that was created for fun and out of curiosity in University. After a bit of thinking and tinkering I came to the conclusion it would be very nice to have an input device (human-computer-interaction) that everyone could use despite mental or physical capabilities and eventual limitations. In a way it's in the domain of assistive technology and adaptive technology. 

A bonus is, that it allows individuals with illnesses and disabilities, disregarding the age, to engage in simulations or video gaming despite any limitations they may have. It offers them the chance to participate in activities that may otherwise be challenging, allowing for training (for regaining or even enhance someones senses) and motoral functionalities - or just for fun and social interactions. In any case, it can improve ones mental well-being, support the medical rehabiliation process and connect disabled and non-disabled players in a playful way.

# Notes
It's important to point out (since it's a prototype with cheap material costs in mind) 'only' a MAX9814 has been used and setup for registering the volume of sounds instead word pattern recogniztion (like voice commands). If you wish to do so, you can use Unity APIs Windows Speech KeywordRecognizer (https://docs.unity3d.com/ScriptReference/Windows.Speech.KeywordRecognizer.html) and rewrite the code (or better microphone or an Arduiino speech recognition module).

# Components

* Arduino Rev 3
* MPU6050 (Gyroscope/Accelerometer)
* MAX9814 (Sound Sensor)
* Optimized 3D-file for printing the 3D-case (3D printer needed)

* Superglue
* Breadboard/PCB
* Jumpwires
* USB-Cable (Type A to Typ B)

# Setup

To build and set up this project, you will need to:

* Solder the sensors and solder them to the Jumpwires
* Connect the jumpwires to the Arduino board
* Print the 3D-printed case in a 3D printer
* Put in (and glue) the components
* Upload the Arduino sketch to the board
* Install Unity and import the Qubey project and its assets
* Enjoy!

Please refer to the detailed instructions in the official manuals of the both sensors how to connect the sensors to the Arduino. If you don't have experience in soldering, be careful and  refer to an adequate ressource (Youtube, friend) as well.

# Features

With this combination of the listed sensors, Qubey can detect and respond to the movement and sound. With the use of a gyroscope, it can be detected when the cube is being tilted or rotated. And with the use the sound sensor it can be detected when there is noise near the cube and how loud it is. 

Depending on the input data, specific actions can be triggered in Unity (such as changing the color of the cube, jumping, shooting etc.) For that, some scripts have been written. For the gyroscope data, there are various modi how an object can be moved inside the virtual environment in Unity (XYZ rotation, linear X/Z movement, linear X movement etc.) with Qubey. Depending on the sound level (that can be configured in the inspector), already (in the code) defined or new custom actions can be triggered.

It's been made in a way (modular architecture) so different kind of simulations, games and demos can be realized with a predefined set of functions in an efficient manner. Possible applications include rehabiliation (stroke, dementia, generally loss/restriction of motoral functionality, disabilities), learning and training with sensoral feedback.

# Possible future outlook

- [] Add a bluetooth component so a USB-cable (for data transfer) is not needed anymore
- [] Add a battery pack so a USB-cable (for power connection) is not needed anymore
- [] Remodel the 3D file (case) better haptic and handling
- [] Add a tray in 3D file so weigths can dynamically be put inside (Qubey is a tad light)
