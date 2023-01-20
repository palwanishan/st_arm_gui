using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Int32Msg = RosMessageTypes.Std.Int32Msg;

public class WeightEstimatePub : MonoBehaviour
{
    [SerializeField] GameObject referencePoseObject;
    public string topicName = "unity/calibrate_obj_weight";

    ROSConnection m_Ros;
    TaskSpaceFollower taskSpaceFollower;

    private Int32Msg calibrateObjWeightMsg;
    private float awaitingResponseUntilTimestamp = -1;
    // Start is called before the first frame update
    void Start()
    {
        taskSpaceFollower = referencePoseObject.GetComponent<TaskSpaceFollower>();
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<Int32Msg>(topicName);
        calibrateObjWeightMsg = new();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > awaitingResponseUntilTimestamp && taskSpaceFollower.isEstimateWeight)
        {
            calibrateObjWeightMsg = new(1);

            m_Ros.Publish(topicName, calibrateObjWeightMsg);
            awaitingResponseUntilTimestamp = Time.time + 5.0f;
        }
    }
}
