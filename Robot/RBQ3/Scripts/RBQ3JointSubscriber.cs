using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using JointStateMsg = RosMessageTypes.Sensor.JointStateMsg;

public class RBQ3JointSubscriber : MonoBehaviour
{
    // Each link name
    public static readonly string[] LinkNames =
    {
        "base_link/rear_rail/rear_right_hip",
        "base_link/rear_rail/rear_right_hip/rear_right_upper_leg",
        "base_link/rear_rail/rear_right_hip/rear_right_upper_leg/rear_right_lower_leg",
        "base_link/rear_rail/rear_left_hip",
        "base_link/rear_rail/rear_left_hip/rear_left_upper_leg",
        "base_link/rear_rail/rear_left_hip/rear_left_upper_leg/rear_left_lower_leg",
        "base_link/front_rail/front_right_hip",
        "base_link/front_rail/front_right_hip/front_right_upper_leg",
        "base_link/front_rail/front_right_hip/front_right_upper_leg/front_right_lower_leg",
        "base_link/front_rail/front_left_hip",
        "base_link/front_rail/front_left_hip/front_left_upper_leg",
        "base_link/front_rail/front_left_hip/front_left_upper_leg/front_left_lower_leg",
    };

    // Hardcoded variables
    const int k_NumRobotJoints = 12;

    [SerializeField]
    GameObject m_RBQ3;
    public GameObject RBQ3 { get => m_RBQ3; set => m_RBQ3 = value; }

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
            m_JointArticulationBodies[i] = m_RBQ3.transform.Find(LinkNames[i]).GetComponent<ArticulationBody>();
        }

        m_Ros.Subscribe<JointStateMsg>("rbq3/joint_states", UpdateJointState);
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

    public void UpdateJointState(JointStateMsg msg)
    {
        for (int i = 0; i < 12; i++)
        {
            UpdateJointAngle(msg.position[i], i);
        }
    }
}
