using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using PoseStampedMsg = RosMessageTypes.Geometry.PoseStampedMsg;
using Unity.Ros;

public class ReferencePosePub : MonoBehaviour
{
    [SerializeField] GameObject referencePoseObject;
    public string topicName = "unity/virtual_box_pose";

    ROSConnection m_Ros;

    private PoseStampedMsg poseMsg;

    void Start()
    {
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<PoseStampedMsg>(topicName);

        poseMsg = new();
    }
    void Update()
    {
        Pose pose = new(referencePoseObject.transform.localPosition.Unity2Ros(),
                                referencePoseObject.transform.localRotation.Unity2Ros());

        poseMsg.pose.position = new(pose.position.x,
                                    pose.position.y,
                                    pose.position.z);

        poseMsg.pose.orientation = new( pose.rotation.x,
                                        pose.rotation.y,
                                        pose.rotation.z,
                                        pose.rotation.w );

        m_Ros.Publish(topicName, poseMsg);
    }
}
