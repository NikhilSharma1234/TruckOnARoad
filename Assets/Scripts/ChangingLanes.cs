using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Text;
using System.IO;
using System;

public class ChangingLanes : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private float movementZ;
    public float speed = 0;
    public GameObject BLTire;
    public GameObject BRTire;
    public GameObject FLTire;
    public GameObject FRTire;
    public GameObject MLTire;
    public GameObject MRTire;
    private float frontTireHalfWidth;
    private float rearAndMiddleTireHalfWidth;
    public GameObject Truck;
    public GameObject TruckBody;
    private bool writingFlag;
    private List<string[]> rowData = new List<string[]>();
    public Camera[] cameraList;
    public int activeCameraNumber;
    private bool rotationFlag;

    // Start is called before the first frame update
    void Start()
    {
        rotationFlag = true;
        // Get rigid body component from truck
        rb = GetComponent<Rigidbody>();

        // Set desired camera to be active
        cameraSwitch(activeCameraNumber);

        // Get half of front tire width
        Renderer FLTireRenderer = FLTire.GetComponent<Renderer>();
        frontTireHalfWidth = (FLTireRenderer.bounds.size.x/2);

        // Get half of middle and back tire width (they are equal in size)
        Renderer MLTireRenderer = MLTire.GetComponent<Renderer>();
        rearAndMiddleTireHalfWidth = (MLTireRenderer.bounds.size.x/2);

        writingFlag = true;
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[12];
        rowDataTemp[0] = "FL_X";
        rowDataTemp[1] = "FL_Y";
        rowDataTemp[2] = "FR_X";
        rowDataTemp[3] = "FR_Y";
        rowDataTemp[4] = "ML_X";
        rowDataTemp[5] = "ML_Y";
        rowDataTemp[6] = "MR_X";
        rowDataTemp[7] = "MR_Y";
        rowDataTemp[8] = "BL_X";
        rowDataTemp[9] = "BL_Y";
        rowDataTemp[10] = "BR_X";
        rowDataTemp[11] = "BR_Y";
        rowData.Add(rowDataTemp);
        InvokeRepeating("RepeatingFunction", 0f, 0.0333333333f);
    }

    // Invoking repeating function to record tire positions every 0.0033333333 seconds
    void RepeatingFunction()
    {
        string[] rowDataTemp = new string[12];
        rowDataTemp[0] = (((FLTire.transform.position.z)/10)+61).ToString("0.00");
        rowDataTemp[1] = (((FLTire.transform.position.x*-1)+frontTireHalfWidth)/10).ToString("0.00");
        rowDataTemp[2] = (((FRTire.transform.position.z)/10)+61).ToString("0.00");
        rowDataTemp[3] = (((FRTire.transform.position.x*-1)-frontTireHalfWidth)/10).ToString("0.00");
        rowDataTemp[4] = (((MLTire.transform.position.z)/10)+61).ToString("0.00");
        rowDataTemp[5] = (((MLTire.transform.position.x*-1)+rearAndMiddleTireHalfWidth)/10).ToString("0.00");
        rowDataTemp[6] = (((MRTire.transform.position.z)/10)+61).ToString("0.00");
        rowDataTemp[7] = (((MRTire.transform.position.x*-1)-rearAndMiddleTireHalfWidth)/10).ToString("0.00");
        rowDataTemp[8] = (((BLTire.transform.position.z)/10)+61).ToString("0.00");
        rowDataTemp[9] = (((BLTire.transform.position.x*-1)+rearAndMiddleTireHalfWidth)/10).ToString("0.00");
        rowDataTemp[10] = (((BRTire.transform.position.z)/10)+61).ToString("0.00");
        rowDataTemp[11] = (((BRTire.transform.position.x*-1)-rearAndMiddleTireHalfWidth)/10).ToString("0.00");
        rowData.Add(rowDataTemp);
        if ((Truck.transform.position.z) >= 640 && writingFlag == true) {
                writingFlag = false;
                string[][] output = new string[rowData.Count][];

            for(int i = 0; i < output.Length; i++){
                output[i] = rowData[i];
            }

            int     length         = output.GetLength(0);
            string     delimiter     = ",";

            StringBuilder sb = new StringBuilder();
            
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            
            
            string filePath = getPath();

            StreamWriter outStream = System.IO.File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_WEBPLAYER
            Application.OpenURL(webplayerQuitURL);
            #else
            Application.Quit();
            #endif
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            cameraSwitch(1);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            cameraSwitch(2);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            cameraSwitch(3);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            cameraSwitch(4);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            cameraSwitch(5);
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            cameraSwitch(6);
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            cameraSwitch(7);
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            cameraSwitch(8);
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            cameraSwitch(9);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            cameraSwitch(10);
        }
        if (Input.GetKey(KeyCode.W))
        {
            cameraSwitch(11);
        }
        if (Input.GetKey(KeyCode.E))
        {
            cameraSwitch(12);
        }
        if (Input.GetKey(KeyCode.R))
        {
            cameraSwitch(13);
        }
        if (Input.GetKey(KeyCode.T))
        {
            cameraSwitch(14);
        }
        if (Input.GetKey(KeyCode.Y))
        {
            cameraSwitch(15);
        }
        if (Input.GetKey(KeyCode.U))
        {
            cameraSwitch(16);
        }
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+"WestToEast"+activeCameraNumber+".csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"WestToEast"+activeCameraNumber+".csv";
        #elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"WestToEast"+activeCameraNumber+".csv";
        #else
        return Application.dataPath +"/"+"WestToEast"+activeCameraNumber+".csv";
        #endif
    }

    //  Fixed update runs every 0.02 seconds (50 times). If speed is 0.4,
    //  that means the object moves 2 meters/second
    void FixedUpdate()
    {
        if(Truck.transform.position.z > -150) {
            if(Truck.transform.position.x > -0.3) {
                if (Truck.transform.localRotation.y >= -1)
                {
                    if (rotationFlag == true) {
                        FLTire.transform.rotation = Quaternion.Euler(0, 0, 0);;
                        FRTire.transform.rotation = Quaternion.Euler(0, 0, 0);;
                        rotationFlag = false;
                    }
                    Truck.transform.Rotate(0,  -0.1f * Time.deltaTime * 100, 0);
                    float distanceTraveled1 = 15 * Time.deltaTime;
                    float rotationInRadians1 = distanceTraveled1 / (frontTireHalfWidth);
                    float rotationInDegrees1 = rotationInRadians1 * Mathf.Rad2Deg;
                    if (FLTire.transform.localRotation.y >-10) {
                        FLTire.transform.Rotate(0,  -0.19f, 0);
                        FRTire.transform.Rotate(0,  -0.19f, 0);
                    }
                }
                Truck.transform.Translate(Vector3.forward * speed);
                float distanceTraveled = 15 * Time.deltaTime;
                float rotationInRadians = distanceTraveled / (frontTireHalfWidth);
                float rotationInDegrees = rotationInRadians * Mathf.Rad2Deg;
                BLTire.transform.Rotate(rotationInDegrees, 0, 0);
                BRTire.transform.Rotate(rotationInDegrees, 0, 0);
                MLTire.transform.Rotate(rotationInDegrees, 0, 0);
                MRTire.transform.Rotate(rotationInDegrees, 0, 0);
            }
            else {
                if (Truck.transform.localRotation.y <= 0)
                {
                    Truck.transform.Rotate(0,  0.1f * Time.deltaTime * 100, 0);
                    float distanceTraveled1 = 15 * Time.deltaTime;
                    float rotationInRadians1 = distanceTraveled1 / (frontTireHalfWidth);
                    float rotationInDegrees1 = rotationInRadians1 * Mathf.Rad2Deg;
                    if (FLTire.transform.localRotation.y != 0) {
                        FLTire.transform.Rotate(0,  0.19f, 0);
                        FRTire.transform.Rotate(0,  0.19f, 0);
                    }
                }
                Truck.transform.Translate(Vector3.forward * speed);
                float distanceTraveled = 15 * Time.deltaTime;
                float rotationInRadians = distanceTraveled / (frontTireHalfWidth);
                float rotationInDegrees = rotationInRadians * Mathf.Rad2Deg;
                BLTire.transform.Rotate(rotationInDegrees, 0, 0);
                BRTire.transform.Rotate(rotationInDegrees, 0, 0);
                MLTire.transform.Rotate(rotationInDegrees, 0, 0);
                MRTire.transform.Rotate(rotationInDegrees, 0, 0);
            }
            
        }
        else {
            Truck.transform.Translate(Vector3.forward * speed);
            float distanceTraveled = 15 * Time.deltaTime;
            float rotationInRadians = distanceTraveled / (frontTireHalfWidth);
            float rotationInDegrees = rotationInRadians * Mathf.Rad2Deg;
            FLTire.transform.Rotate(rotationInDegrees, 0, 0);
            FRTire.transform.Rotate(rotationInDegrees, 0, 0);
            BLTire.transform.Rotate(rotationInDegrees, 0, 0);
            BRTire.transform.Rotate(rotationInDegrees, 0, 0);
            MLTire.transform.Rotate(rotationInDegrees, 0, 0);
            MRTire.transform.Rotate(rotationInDegrees, 0, 0);
        } 
    }

    void cameraSwitch(int cameraNumber)
    {
        int cameraCount = cameraList.Length;
        for (int i = 0; i < cameraCount; i++)
        {
            Camera currentCamera = cameraList[i];
            if (cameraNumber == (i + 1))
            {
                currentCamera.enabled = true;
                continue;
            }
            currentCamera.enabled = false;
        }
    }
}
