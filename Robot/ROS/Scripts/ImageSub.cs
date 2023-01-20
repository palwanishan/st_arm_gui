using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using ImageMsg = RosMessageTypes.Sensor.CompressedImageMsg;

public class ImageSub : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public string topicNameReal = "camera_port1/image_raw/compressed";
    public string topicNameGazebo = "Front/image_raw/compressed";
    public bool isGazeboRobot = false;

    private Texture2D texture2D;
    private byte[] imageData;
    private bool isMessageReceived;


    void Start()
    {
        texture2D = new Texture2D(1280, 720, TextureFormat.RGB24, false);

        meshRenderer.material = new Material(Shader.Find("Standard"));
        if(isGazeboRobot) ROSConnection.GetOrCreateInstance().Subscribe<ImageMsg>(topicNameGazebo, ReceiveMsg);
        else ROSConnection.GetOrCreateInstance().Subscribe<ImageMsg>(topicNameReal, ReceiveMsg);
    }


    void ReceiveMsg(ImageMsg imageMsg)
    {
        imageData = imageMsg.data;
        isMessageReceived = true;
    }

    private void ProcessMessage()
    {
        texture2D.LoadImage(imageData);
        texture2D.Apply();
        meshRenderer.material.SetTexture("_MainTex", texture2D);
        isMessageReceived = false;
    }


    void Update()
    {
        if (isMessageReceived)
        {
            ProcessMessage();
        }
    }
}
