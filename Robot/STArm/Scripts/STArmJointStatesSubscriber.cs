using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using JointStateMsg = RosMessageTypes.Sensor.JointStateMsg;

public class STArmJointStatesSubscriber : MonoBehaviour
{
    // Each link name
    public static readonly string[] LinkNames =
    {   "base_link/shoulder_yaw_link",
        "base_link/shoulder_yaw_link/shoulder_pitch_link",
        "base_link/shoulder_yaw_link/shoulder_pitch_link/elbow_pitch_link",
        "base_link/shoulder_yaw_link/shoulder_pitch_link/elbow_pitch_link/wrist_pitch_link",
        "base_link/shoulder_yaw_link/shoulder_pitch_link/elbow_pitch_link/wrist_pitch_link/wrist_roll_link",
        "base_link/shoulder_yaw_link/shoulder_pitch_link/elbow_pitch_link/wrist_pitch_link/wrist_roll_link/wrist_yaw_link",
        "base_link/shoulder_yaw_link/shoulder_pitch_link/elbow_pitch_link/wrist_pitch_link/wrist_roll_link/wrist_yaw_link/gripper_left_link",
        "base_link/shoulder_yaw_link/shoulder_pitch_link/elbow_pitch_link/wrist_pitch_link/wrist_roll_link/wrist_yaw_link/gripper_right_link"
    };

    // Hardcoded variables
    const int k_NumRobotJoints = 8;

    [SerializeField]
    GameObject m_STArm;
    public GameObject STArm { get => m_STArm; set => m_STArm = value; }

    // Articulation Bodies
    ArticulationBody[] m_JointArticulationBodies;

    ROSConnection m_Ros;

    void Start()
    {
        // Get ROS connection static instance
        m_Ros = ROSConnection.GetOrCreateInstance();

        // Create array for articulation bodies of each joint
        m_JointArticulationBodies = new ArticulationBody[k_NumRobotJoints];

        // Get the articulationbody for each joint
        var linkName = string.Empty;
        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            m_JointArticulationBodies[i] = m_STArm.transform.Find(LinkNames[i]).GetComponent<ArticulationBody>();
        }

        m_Ros.Subscribe<JointStateMsg>("st_arm/joint_states", UpdateJointState);
    }

    // Update the joint angle by setting the
    // xDrive.Target of each Articulationbody
    public void UpdateJointAngle(double cmd, int joint)
    {
        var angle = (float)cmd * Mathf.Rad2Deg;
        var jointXDrive = m_JointArticulationBodies[joint].xDrive;
        jointXDrive.target = angle;
        m_JointArticulationBodies[joint].xDrive = jointXDrive;
    }

    public void UpdateJointAnglePrismatic(double cmd, int joint)
    {
        var angle = (float)cmd;
        var jointXDrive = m_JointArticulationBodies[joint].xDrive;
        jointXDrive.target = angle;
        m_JointArticulationBodies[joint].xDrive = jointXDrive;
    }

    public void UpdateJointState(JointStateMsg msg)
    {
        msg.position[6] = Map((float)msg.position[6], 0, 2, -0.03f, 0);
        for (int i = 0; i < 6; i++)
        {
            UpdateJointAngle(msg.position[i], i);
        }
        UpdateJointAnglePrismatic(msg.position[6], 6);
        UpdateJointAnglePrismatic(msg.position[6], 7);
    }


    float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
