using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Text;
using System.IO;
using System;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private float movementZ;
    public float speed = 0;

    public Camera Camera1;
    public Camera Camera2;
    public Camera Camera3;
    public Camera Camera4;
    public Camera Camera5;
    public Camera Camera6;
    public Camera Camera7;
    public Camera Camera8;
    public Camera Camera9;
    public Camera Camera10;
    public Camera Camera11;
    public Camera Camera12;
    public GameObject BLTire;
    public GameObject BRTire;
    public GameObject FLTire;
    public GameObject FRTire;
    public GameObject MLTire;
    public GameObject MRTire;
    private float tireRadius;
    public GameObject Truck;
    public GameObject TruckBody;
    private bool writingFlag;
    private List<string[]> rowData = new List<string[]>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Camera1.enabled = false;
        Camera2.enabled = true;
        Camera3.enabled = false;
        Camera4.enabled = false;
        Camera5.enabled = false;
        Camera6.enabled = false;
        Camera7.enabled = false;
        Camera8.enabled = false;
        Camera9.enabled = false;
        Camera10.enabled = false;
        Camera11.enabled = false;
        Camera12.enabled = false;

        Renderer renderer = FLTire.GetComponent<Renderer>();
        tireRadius = (renderer.bounds.size.y/2);
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
        InvokeRepeating("TrackWheels", 0f, 1f);
    }

    void TrackWheels()
    {
        // Calculate truck speed by taking the differnce of two points from below output
        Debug.Log(BLTire.transform.position);
        
        // Truck Dimensions
        //Debug.Log(TruckBody.GetComponent<MeshRenderer>().bounds.size);

    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+"Saved_data2.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data2.csv";
        #elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data2.csv";
        #else
        return Application.dataPath +"/"+"Saved_data2.csv";
        #endif
    }

    void FixedUpdate()
    {
        // Debug.Log((((FLTire.transform.position.z)+450)/10)+61);
        // Debug.Log(FLTire.GetComponent<MeshRenderer>().bounds.size);
        //Debug.Log(Time.deltaTime);
        Truck.transform.Translate(Vector3.forward * speed);
        float distanceTraveled = 15 * Time.deltaTime;
        float rotationInRadians = distanceTraveled / (tireRadius);
        float rotationInDegrees = rotationInRadians * Mathf.Rad2Deg;
        FLTire.transform.Rotate(rotationInDegrees, 0, 0);
        FRTire.transform.Rotate(rotationInDegrees, 0, 0);
        BLTire.transform.Rotate(rotationInDegrees, 0, 0);
        BRTire.transform.Rotate(rotationInDegrees, 0, 0);
        MLTire.transform.Rotate(rotationInDegrees, 0, 0);
        MRTire.transform.Rotate(rotationInDegrees, 0, 0);
        string[] rowDataTemp = new string[12];
        rowDataTemp[0] = ((((FLTire.transform.position.z)+450)/10)+61).ToString("0.00");
        rowDataTemp[1] = (((FLTire.transform.position.x*-1)/10)+0.138988).ToString("0.00");
        rowDataTemp[2] = ((((FRTire.transform.position.z)+450)/10)+61).ToString("0.00");
        rowDataTemp[3] = (((FRTire.transform.position.x*-1)/10)+0.138988).ToString("0.00");
        rowDataTemp[4] = ((((MLTire.transform.position.z)+450)/10)+61).ToString("0.00");
        rowDataTemp[5] = (((MLTire.transform.position.x*-1)/10)+0.138988).ToString("0.00");
        rowDataTemp[6] = ((((MRTire.transform.position.z)+450)/10)+61).ToString("0.00");
        rowDataTemp[7] = (((MRTire.transform.position.x*-1)/10)+0.138988).ToString("0.00");
        rowDataTemp[8] = ((((BLTire.transform.position.z)+450)/10)+61).ToString("0.00");
        rowDataTemp[9] = (((BLTire.transform.position.x*-1)/10)+0.138988).ToString("0.00");
        rowDataTemp[10] = ((((BRTire.transform.position.z)+450)/10)+61).ToString("0.00");
        rowDataTemp[11] = (((BRTire.transform.position.x*-1)/10)+0.138988).ToString("0.00");
        rowData.Add(rowDataTemp);
        if ((Truck.transform.position.z+450) >= 640 && writingFlag == true) {
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
        
        // if (Input.GetKey(KeyCode.UpArrow)) {
        //     rb.AddForce(transform.forward * speed);
        // float distanceTraveled = 20 * Time.deltaTime;
        // float rotationInRadians = distanceTraveled / (tireRadius);
        // float rotationInDegrees = rotationInRadians * Mathf.Rad2Deg;
        // FLTire.transform.Rotate(rotationInDegrees, 0, 0);
        // FRTire.transform.Rotate(rotationInDegrees, 0, 0);
        // BLTire.transform.Rotate(rotationInDegrees, 0, 0);
        // BRTire.transform.Rotate(rotationInDegrees, 0, 0);
        // MLTire.transform.Rotate(rotationInDegrees, 0, 0);
        // MRTire.transform.Rotate(rotationInDegrees, 0, 0);
        // }
        // if (Input.GetKey(KeyCode.DownArrow)) {
        //     rb.AddForce(-transform.forward * speed);
        //     float distanceTraveled = speed * Time.deltaTime;
        //     float rotationInRadians = distanceTraveled / (tireRadius);
        //     float rotationInDegrees = rotationInRadians * Mathf.Rad2Deg;
        //     FLTire.transform.Rotate(-rotationInDegrees, 0, 0);
        //     FRTire.transform.Rotate(-rotationInDegrees, 0, 0);
        //     BLTire.transform.Rotate(-rotationInDegrees, 0, 0);
        //     BRTire.transform.Rotate(-rotationInDegrees, 0, 0);
        //     MLTire.transform.Rotate(-rotationInDegrees, 0, 0);
        //     MRTire.transform.Rotate(-rotationInDegrees, 0, 0);
        // }
        if (Input.GetKey(KeyCode.Alpha1)) {
            CameraOne();
        }
        if (Input.GetKey(KeyCode.Alpha2)) {
            CameraTwo();
        }
        if (Input.GetKey(KeyCode.Alpha3)) {
            CameraThree();
        }
        if (Input.GetKey(KeyCode.Alpha4)) {
            CameraFour();
        }
        if (Input.GetKey(KeyCode.Alpha5)) {
            CameraFive();
        }
        if (Input.GetKey(KeyCode.Alpha6)) {
            CameraSix();
        }
        if (Input.GetKey(KeyCode.Alpha7)) {
            CameraSeven();
        }
        if (Input.GetKey(KeyCode.Alpha8)) {
            CameraEight();
        }
        if (Input.GetKey(KeyCode.Alpha9)) {
            CameraNine();
        }
        if (Input.GetKey(KeyCode.Q)) {
            CameraTen();
        }
        if (Input.GetKey(KeyCode.W)) {
            CameraEleven();
        }
        if (Input.GetKey(KeyCode.E)) {
            CameraTwelve();
        }
    }
    void CameraOne() {
        Camera1.enabled = true;
        Camera2.enabled = false;
        Camera3.enabled = false;
        Camera4.enabled = false;
        Camera5.enabled = false;
        Camera6.enabled = false;
        Camera7.enabled = false;
        Camera8.enabled = false;
        Camera9.enabled = false;
        Camera10.enabled = false;
        Camera11.enabled = false;
        Camera12.enabled = false;
    }

    void CameraTwo() {
        Camera1.enabled = false;
        Camera2.enabled = true;
        Camera3.enabled = false;
        Camera4.enabled = false;
        Camera5.enabled = false;
        Camera6.enabled = false;
        Camera7.enabled = false;
        Camera8.enabled = false;
        Camera9.enabled = false;
        Camera10.enabled = false;
        Camera11.enabled = false;
        Camera12.enabled = false;
    }

    void CameraThree() {
        Camera1.enabled = false;
        Camera2.enabled = false;
        Camera3.enabled = true;
        Camera4.enabled = false;
        Camera5.enabled = false;
        Camera6.enabled = false;
        Camera7.enabled = false;
        Camera8.enabled = false;
        Camera9.enabled = false;
        Camera10.enabled = false;
        Camera11.enabled = false;
        Camera12.enabled = false;
    }

    void CameraFour() {
        Camera1.enabled = false;
        Camera2.enabled = false;
        Camera3.enabled = false;
        Camera4.enabled = true;
        Camera5.enabled = false;
        Camera6.enabled = false;
        Camera7.enabled = false;
        Camera8.enabled = false;
        Camera9.enabled = false;
        Camera10.enabled = false;
        Camera11.enabled = false;
        Camera12.enabled = false;
    }
    void CameraFive() {
        Camera1.enabled = false;
        Camera2.enabled = false;
        Camera3.enabled = false;
        Camera4.enabled = false;
        Camera5.enabled = true;
        Camera6.enabled = false;
        Camera7.enabled = false;
        Camera8.enabled = false;
        Camera9.enabled = false;
        Camera10.enabled = false;
        Camera11.enabled = false;
        Camera12.enabled = false;
    }
    void CameraSix() {
        Camera1.enabled = false;
        Camera2.enabled = false;
        Camera3.enabled = false;
        Camera4.enabled = false;
        Camera5.enabled = false;
        Camera6.enabled = true;
        Camera7.enabled = false;
        Camera8.enabled = false;
        Camera9.enabled = false;
        Camera10.enabled = false;
        Camera11.enabled = false;
        Camera12.enabled = false;
    }
    void CameraSeven() {
        Camera1.enabled = false;
        Camera2.enabled = false;
        Camera3.enabled = false;
        Camera4.enabled = false;
        Camera5.enabled = false;
        Camera6.enabled = false;
        Camera7.enabled = true;
        Camera8.enabled = false;
        Camera9.enabled = false;
        Camera10.enabled = false;
        Camera11.enabled = false;
        Camera12.enabled = false;
    }
    void CameraEight() {
        Camera1.enabled = false;
        Camera2.enabled = false;
        Camera3.enabled = false;
        Camera4.enabled = false;
        Camera5.enabled = false;
        Camera6.enabled = false;
        Camera7.enabled = false;
        Camera8.enabled = true;
        Camera9.enabled = false;
        Camera10.enabled = false;
        Camera11.enabled = false;
        Camera12.enabled = false;
    }
    void CameraNine() {
        Camera1.enabled = false;
        Camera2.enabled = false;
        Camera3.enabled = false;
        Camera4.enabled = false;
        Camera5.enabled = false;
        Camera6.enabled = false;
        Camera7.enabled = false;
        Camera8.enabled = false;
        Camera9.enabled = true;
        Camera10.enabled = false;
        Camera11.enabled = false;
        Camera12.enabled = false;
    }
    void CameraTen() {
        Camera1.enabled = false;
        Camera2.enabled = false;
        Camera3.enabled = false;
        Camera4.enabled = false;
        Camera5.enabled = false;
        Camera6.enabled = false;
        Camera7.enabled = false;
        Camera8.enabled = false;
        Camera9.enabled = false;
        Camera10.enabled = true;
        Camera11.enabled = false;
        Camera12.enabled = false;
    }
    void CameraEleven() {
        Camera1.enabled = false;
        Camera2.enabled = false;
        Camera3.enabled = false;
        Camera4.enabled = false;
        Camera5.enabled = false;
        Camera6.enabled = false;
        Camera7.enabled = false;
        Camera8.enabled = false;
        Camera9.enabled = false;
        Camera10.enabled = false;
        Camera11.enabled = true;
        Camera12.enabled = false;
    }
    void CameraTwelve() {
        Camera1.enabled = false;
        Camera2.enabled = false;
        Camera3.enabled = false;
        Camera4.enabled = false;
        Camera5.enabled = false;
        Camera6.enabled = false;
        Camera7.enabled = false;
        Camera8.enabled = false;
        Camera9.enabled = false;
        Camera10.enabled = false;
        Camera11.enabled = false;
        Camera12.enabled = true;
    }
}
