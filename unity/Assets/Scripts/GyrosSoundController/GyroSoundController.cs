using UnityEngine;
using System.IO.Ports;
using System;
using System.Threading;
using System.Collections;

// Requires a Rigidbody component to be attached to the GameObject
[RequireComponent(typeof(Rigidbody))]
public class GyroSoundController : MonoBehaviour
{

    // An enum with various modes of operation to choose from
    public enum ModusSelection
    {
        OnlyRotationModus,
        MovementWithOutYawRotation,
        MovementWithYawRotation,
        MovementOnlyLeftAndRight,
        MovementOnlyForthAndBack,
        NinetyDegreeTurnWIP,
        SpaceShip
    }

    // An enum with various sound modes to choose from
    public enum SoundSelection
    {
        NoSoundInteraction,
        OneSoundInteraction,
        TwoSoundInteraction,
        ChangeColour,
        Jump,
        Shoot,
        JumpAndShoot,
        Acceleration,
        Honk
    }

    [SerializeField] private bool movingForwardAllowed = false;

    [Header("Gyro Settings")]
    // A field of the ModusSelection enum that can be set in the Inspector using a dropdown menu
    [Tooltip("Selection of the gyro mode.")]
    [SerializeField] private ModusSelection modusSelection;

    // Sensitivity for game object movement in translation mode
    [Range(0.0f, 200.0f)]
    [Tooltip("The sensitivity and speed setting for the controlling object.")]
    [SerializeField] private float translationMovementSensitivity = 6f;
    // multiply by 10000 and divide by 10000 to round off the sensitivity value to two decimal places
    private float translationMovementSensitivityRoundingHelper = 10000;

    // Threshold for detecting a 90 degree rotation in the gyroscope
    private float rotationThreshold = 1.38f;

    // The speed at which the object moves in translation mode
    // Used for example in Spaceship mode
    [Range(0, 1000)]
    [SerializeField] private float movementSpeed = 1.0f;

    [Header("Sound Settings")]
    // A field of the SoundSelection enum that can be set in the Inspector using a dropdown menu
    [Tooltip("Selection of the sound mode.")]
    [SerializeField] private SoundSelection soundSelection;

    // Base-Ground-Value in voltage
    [Tooltip("Value for sound in IDLE when basically no sound is present.")]
    [SerializeField] private float _groundSoundValue = 40.0f;

    // Lower-Sound-Value in voltage
    [Tooltip("The low sound perimeter used for one ( or two sound interaction.")]
    [SerializeField] private float _lowSoundValue = 150.0f;

    // Higher-Sound-Value in voltage
    [Tooltip("The high sound perimeter used only for the two sound interaction (loudest).")]
    [SerializeField] private float _highSoundValue = 240.0f;

    // Material for changing color based on sound value
    private Material m_Material;

    [Header("Debug Settings")]
    // Variables for received message data
    [SerializeField] private string strReceived;
    [SerializeField] private string[] strData = new string[4];
    //[SerializeField] private string[] strData_received = new string[4];
    [SerializeField] private float qx, qy, qz, qw;


    // Serial port for Arduino communication
    SerialPort stream = new SerialPort("COM3", 115200);

    // Flag to stop the thread
    private bool shouldExit = false;

    // Flag for resetting after a 90 degree turn
    private bool DMPCountdownIsDone = false;

    // Flag in case SpaceShip mode is enabled
    private bool spaceShipIsEnabled = false;

    void Start()
    {
        // Get the material component of the game object that this script is attached to
        m_Material = GetComponent<Renderer>().material;

        // Set up Arduino serial port
        // Set DtrEnable and RtsEnable to true
        stream.DtrEnable = true;
        stream.RtsEnable = true;

        // Open the serial port
        if (!stream.IsOpen)
        {
            try
            {
                // Open the serial stream
                stream.Open();
            }
            catch (TimeoutException)
            {
                // Handle any TimeoutException and log the error message
                Debug.LogError("Timeout exception while opening serial port.");
            }
        }
        else
        {
            // Log an error message if the serial port is already open
            Debug.LogError("Port is already open.");           
        }

        // Start a new thread 
        // Create a new thread object and pass the ThreadWorker method as an argument to read from the serial port
        Thread myThread = new Thread(new ThreadStart(ThreadWorker));

        // Start the new thread
        myThread.Start();

        // For loading the DMP module
        // Start the WaitAndLoadCoroutine method as a coroutine
        StartCoroutine(WaitAndLoadCoroutine());
    }

    // Coroutine for waiting and loading the DMP module
    private IEnumerator WaitAndLoadCoroutine()
    {
        // Wait for 5 seconds while logging a countdown message to the console
        for (int i = 5; i > 0; i--)
        {
            Debug.Log("Loading Qubey (DMP) in " + i + " seconds...");

            yield return new WaitForSeconds(1.0f);
        }

        // Set the DMP countdown flag to true when the coroutine completes
        DMPCountdownIsDone = true;

        // Log a message to the console when the DMP module is loaded
        Debug.Log("Qubey (DMP) loading is complete!");
    }

    void Update()
    {
        // Call the MessageReceived method and pass the strReceived variable as an argument
        MessageReceived(strReceived);

        // Only used for spaceship modus
        // Check if spaceship mode is enabled and if the DMP countdown has completed and if moving forward is allowed
        if (spaceShipIsEnabled && DMPCountdownIsDone && movingForwardAllowed)
        {
            // Update the position of the object by adding the direction of movement
            // multiplied by the speed and the elapsed time since the last frame
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
        else if (movingForwardAllowed && DMPCountdownIsDone)
        {
            // Update the position of the object by adding the direction of movement
            // multiplied by the speed and the elapsed time since the last frame
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
    }

    // This method is run in a separate thread to read messages from a serial port
    void ThreadWorker()
    {
        // While the thread is not set to exit
        while (shouldExit == false) 
        {
            // If the serial port is open
            if (stream.IsOpen) 
            {
                try
                {
                    // Read a line from the serial port
                    strReceived = stream.ReadLine();
                }
                catch (System.Exception)
                {
                    // Handle exception, such as closing the port
                    // (TODO: implement proper exception handling)
                }
            }
        }
    }

    void MessageReceived(string message)
    {
        if (message.Length > 0)
        {
            // Get the message type from the first character
            string type = message.Substring(0, 1);

            // Get the message data value after the first character
            var dataValue = message.Substring(1);


            // Process the arduino data based on its type
            switch (type)
            {
                case "S":
                    // Convert the sound value to a float
                    float soundValue = float.Parse(dataValue);

                    // Debug.Log(soundValue);

                    // Check the selected modus modus
                    switch (soundSelection)
                    {
                        case SoundSelection.NoSoundInteraction:

                            break;

                        case SoundSelection.OneSoundInteraction:

                            // Do something according to low value
                            if (soundValue < _groundSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once low sound is detected
                                */

                                DoThingsWhenNoSoundIsDetected();

                            }
                            else if (soundValue > _lowSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once louder sound is detected
                                */

                                DoThingsWhenHighSoundIsDetected();
                            }

                            break;

                        case SoundSelection.TwoSoundInteraction:


                            if (soundValue < 50.0f)
                            {
                                /*
                                // Gamelogic what shall happen once low sound is detected
                                */
                                DoThingsWhenNoSoundIsDetected();

                            }
                            else if (soundValue > _groundSoundValue && soundValue > _lowSoundValue && soundValue < _highSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once louder sound is detected
                                */

                                DoThingsWhenLowSoundIsDetected();
                            }
                            else if (soundValue > _groundSoundValue && soundValue > _lowSoundValue && soundValue > _highSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once louder sound is detected
                                */

                                DoThingsWhenHighSoundIsDetected();
                            }

                            break;

                        case SoundSelection.Acceleration:

                            // Do something according to low value
                            if (soundValue < _groundSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once low sound is detected
                                */

                                DoThingsWhenNoSoundIsDetected();

                            }
                            else if (soundValue > _lowSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once louder sound is detected
                                */

                                Acceleration();
                            }

                            break;

                        case SoundSelection.ChangeColour:

                            // Do something according to low value
                            if (soundValue < _groundSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once low sound is detected
                                */

                                DoThingsWhenNoSoundIsDetected();

                            }
                            else if (soundValue > _lowSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once louder sound is detected
                                */

                                ChangeColour();
                            }

                            break;

                        case SoundSelection.Jump:

                            // Do something according to low value
                            if (soundValue < _groundSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once low sound is detected
                                */

                                DoThingsWhenNoSoundIsDetected();

                            }
                            else if (soundValue > _lowSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once louder sound is detected
                                */

                                Jump();
                            }

                            break;

                        case SoundSelection.Shoot:

                            // Do something according to low value
                            if (soundValue < _groundSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once low sound is detected
                                */

                                DoThingsWhenNoSoundIsDetected();

                            }
                            else if (soundValue > _lowSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once louder sound is detected
                                */

                                Shoot();
                            }

                            break;

                        case SoundSelection.JumpAndShoot:


                            if (soundValue < 50.0f)
                            {
                                /*
                                // Gamelogic what shall happen once low sound is detected
                                */

                                DoThingsWhenNoSoundIsDetected();

                            }
                            else if (soundValue > _groundSoundValue && soundValue > _lowSoundValue && soundValue < _highSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once louder sound is detected
                                */

                                Rigidbody rb = GetComponent<Rigidbody>();
                                rb.useGravity = true;
                                rb.isKinematic = false;

                                Jump();
                            }
                            else if (soundValue > _groundSoundValue && soundValue > _lowSoundValue && soundValue > _highSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once louder sound is detected
                                */
                                Rigidbody rb = GetComponent<Rigidbody>();
                                rb.useGravity = true;
                                rb.isKinematic = false;

                                Shoot();
                            }

                            break;



                        case SoundSelection.Honk:

                            // Do something according to low value
                            if (soundValue < _groundSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once low sound is detected
                                */

                                DoThingsWhenNoSoundIsDetected();

                            }
                            else if (soundValue > _lowSoundValue)
                            {
                                /*
                                // Gamelogic what shall happen once louder sound is detected
                                */

                                Honk();
                            }

                            break;
                    }

                    break;

                case "G":
                    // Split the gyro data into separate values
                    strData = dataValue.Split(',');

                    // If data is not empty
                    if (strData[0] != "" && strData[1] != "" && strData[2] != "" && strData[3] != "")
                    {

                        // Convert the gyro data to floats
                        qw = float.Parse(strData[0]);
                        qx = float.Parse(strData[1]);
                        qy = float.Parse(strData[2]);
                        qz = float.Parse(strData[3]);

                        // Check the selected modus
                        switch (modusSelection)
                        {
                            case ModusSelection.OnlyRotationModus:

                                // Turn off gravity

                                // Get the Rigidbody component for turning off the gravity in rotation-modus
                                Rigidbody rb = GetComponent<Rigidbody>();

                                // Set the gravity scale to 0
                                rb.useGravity = false;
                                //rb.isKinematic = true;

                                // Rotate the game object based on the gyro data
                                transform.rotation = new Quaternion(-qx, -qz, -qy, qw);

                                // Old parameter
                                //transform.rotation = new Quaternion(-qy, -qz, qx, qw);  
                                break;

                            case ModusSelection.MovementWithOutYawRotation:
                                rb = GetComponent<Rigidbody>();
                                rb.useGravity = false;
                                

                                // Translate the game object based on the transmitted gyro data
                                transform.position += new Vector3(qy * (translationMovementSensitivity / translationMovementSensitivityRoundingHelper), qz * 0, -qx * (translationMovementSensitivity / translationMovementSensitivityRoundingHelper));

                                break;

                            case ModusSelection.MovementWithYawRotation:
                                rb = GetComponent<Rigidbody>();
                                rb.useGravity = false;
                                rb.isKinematic = false;


                                transform.position += new Vector3(qy * (translationMovementSensitivity / translationMovementSensitivityRoundingHelper), qz * 0, -qx * (translationMovementSensitivity / translationMovementSensitivityRoundingHelper));
                                transform.rotation = new Quaternion(qy * 0, -qz * 1, qx * 0, qw);

                                break;

                            case ModusSelection.MovementOnlyLeftAndRight:
                                rb = GetComponent<Rigidbody>();
                                rb.useGravity = true;
                                //rb.isKinematic = false;

                                // Rotate and translate the game object based on the gyro data
                                transform.position += new Vector3(qy * (translationMovementSensitivity / translationMovementSensitivityRoundingHelper), 0, 0);

                                break;

                            case ModusSelection.MovementOnlyForthAndBack:
                                rb = GetComponent<Rigidbody>();
                                rb.useGravity = true;
                                rb.isKinematic = false;

                                // Rotate and translate the game object based on the gyro data
                                transform.position += new Vector3(0, 0, -qx * (translationMovementSensitivity / translationMovementSensitivityRoundingHelper));

                                break;

                            case ModusSelection.NinetyDegreeTurnWIP:

                                // TODO

                                break;

                            case ModusSelection.SpaceShip:

                                // Same as rotation but with an additional flag added
                                spaceShipIsEnabled = true;

                                // Get the Rigidbody component for turning off the gravity in rotation-modus
                                rb = GetComponent<Rigidbody>();

                                // Set the gravity scale to 0
                                rb.useGravity = false;
                                rb.isKinematic = true;

                                // Rotate the game object based on the gyro data
                                transform.rotation = new Quaternion(-qx, -qz, -qy, qw);

                                // Old parameter
                                //transform.rotation = new Quaternion(-qy, -qz, qx, qw);  
                                break;
                        }
                    }

                    break;

                default:
                    // print("No values are read!");
                    break;
            }
        }

       
    }
    // This coroutine waits for a set interval of 5 seconds
    IEnumerator WaitInterval()
    {
        for (int i = 1; i > 0; i--)
        {
            yield return new WaitForSeconds(5.0f);
        }
    }

    void OnApplicationQuit()
    {
        // Close the port when the application ends
        if (stream.IsOpen)
        {
            try
            {
                // Close the port
                stream.Close();
                shouldExit = true;
            }
            catch (UnityException e)
            {
                // Exception handling
                Debug.LogError(e.Message);
            }
        }
    }

    /*
     * GAME LOGIC 
     * TODO: Refactor into separate class
    */

    // Do things when no sound is detected
    void DoThingsWhenNoSoundIsDetected()
    {
    }

    // Do things when a low sound is detected
    void DoThingsWhenLowSoundIsDetected()
    {
        // Set the color of the material to blue
        m_Material.color = Color.blue;
    }

    // Do things when a high sound is detected
    void DoThingsWhenHighSoundIsDetected()
    {
        // Set the color of the material to green
        m_Material.color = Color.green;
    }

    // Change the color
    void ChangeColour()
    {
        // Get the ChangeColourScript component and call the ChangeColor method
        ChangeColourScript changeColourScript = gameObject.GetComponent<ChangeColourScript>();
        changeColourScript.ChangeColor();
    }

    // Jump
    void Jump()
    {
        // Get the JumpScript component and call the Jump method
        JumpScript jumpScript = gameObject.GetComponent<JumpScript>();
        jumpScript.Jump();
    }

    // Shoot
    void Shoot()
    {
        // Get the ShootScript component and call the Jump method
        ShootScript shootScript = gameObject.GetComponent<ShootScript>();
        shootScript.Jump();
    }

    // Acceleration
    void Acceleration()
    {
        // Get the AccelerationScript component and call the Accelerate method
        AccelerationScript accelerationScript = gameObject.GetComponent<AccelerationScript>();
        accelerationScript.Accelerate();
    }

    // Honk
    void Honk()
    {
        // Get the HonkScript component and call the Honk method
        HonkScript honkScript = gameObject.GetComponent<HonkScript>();
        honkScript.Honk();
    }
}