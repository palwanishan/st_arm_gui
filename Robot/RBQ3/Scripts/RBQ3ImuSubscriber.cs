using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using ImuMsg = RosMessageTypes.Sensor.ImuMsg;
using QuaternionMsg = RosMessageTypes.Geometry.QuaternionMsg;

public class RBQ3ImuSubscriber : MonoBehaviour
{
    public static readonly string[] LinkNames =
    {
        "up",
        "up/right",
        "up/right/forward",
        "up/right/forward/yaw",
        "up/right/forward/yaw/pitch",
        "up/right/forward/yaw/pitch/roll"
    };

    // Hardcoded variables
    const int k_degreesOfFreedom = 6;

    public GameObject m_ImuSensor;
    private Quaternion m_ImuSensorOrientation;
    public Vector3 m_ImuSensorRPY;

    private ArticulationBody[] m_JointArticulationBodies;

    ROSConnection m_Ros;

    public GameObject m_World;

    // Start is called before the first frame update
    void Start()
    {
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.Subscribe<ImuMsg>("rbq3/imu", UpdateImuState); 

         m_JointArticulationBodies = new ArticulationBody[k_degreesOfFreedom];

        for (int i = 0; i < k_degreesOfFreedom; i++)
        {
            m_JointArticulationBodies[i] = m_World.transform.Find(LinkNames[i]).GetComponent<ArticulationBody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_ImuSensor.transform.localRotation = Quaternion.Slerp(m_ImuSensor.transform.localRotation, m_ImuSensorOrientation, Time.deltaTime * 10.0f);
        m_ImuSensorRPY = m_ImuSensor.transform.localEulerAngles;
        UpdateRPY(m_ImuSensorRPY);
    }

    public void UpdateImuState(ImuMsg msg)
    {
        m_ImuSensorOrientation = ToUnityQuaternion(msg.orientation);
    }

    public void UpdateJointAngle(double angle, int joint)
    {
        //var angle = (float)cmd * Mathf.Rad2Deg;
        var jointXDrive = m_JointArticulationBodies[joint].xDrive;
        jointXDrive.target = (float)angle;
        m_JointArticulationBodies[joint].xDrive = jointXDrive;
    }

    public void UpdateRPY(Vector3 rpy)
    {
        for (int i = 0; i < 3; i++)
        {
            if (rpy[i] > 180) rpy[i] -= 360;
        }

        UpdateJointAngle(rpy[0], 4);
        UpdateJointAngle(rpy[1], 3);
        UpdateJointAngle(rpy[2], 5);
    }

    private Quaternion ToUnityQuaternion(QuaternionMsg q) => new((float)q.y, -(float)q.x, (float)q.z, -(float)q.w);
}
