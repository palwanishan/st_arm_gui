using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Float32Msg = RosMessageTypes.Std.Float32Msg;

public class GripperStatesPub : MonoBehaviour
{
    [SerializeField] GameObject ReferenceEndEffector;
    public string topicName = "unity/gripper_state";

    ROSConnection m_Ros;
    TaskSpaceFollower taskSpaceFollower;

    private Float32Msg gripperStateMsg;
    private float awaitingResponseUntilTimestamp = -1;
    // Start is called before the first frame update
    void Start()
    {
        taskSpaceFollower = ReferenceEndEffector.GetComponent<TaskSpaceFollower>();
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<Float32Msg>(topicName);
        gripperStateMsg = new();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > awaitingResponseUntilTimestamp)
        {
            gripperStateMsg = new(taskSpaceFollower.gripperValue * 2.3f);

            m_Ros.Publish(topicName, gripperStateMsg);
            awaitingResponseUntilTimestamp = Time.time + 0.01f;
        }
    }
}
