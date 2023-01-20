using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STArmInitializeJointAttributes : MonoBehaviour
{
    // Play with setting these variables
    // to find the best robot behavior
    public float stiffness = 1500;
    public float damping = 100;
    public float forceLimit = 1000;
    public int dynamicVal = 10;

    int k_NumRobotJoints = 8;

    [SerializeField]
    GameObject m_STArm;
    public GameObject STArm { get => m_STArm; set => m_STArm = value; }

    void Start()
    {
        // Get the articulationbody for each joint
        var linkName = string.Empty;
        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            var articulationBody = m_STArm.transform.Find(STArmJointStatesSubscriber.LinkNames[i]).GetComponent<ArticulationBody>();

            // Not sure what to set these values to
            articulationBody.jointFriction = dynamicVal;
            articulationBody.angularDamping = dynamicVal;

            // Update important xDrive values here
            var jointXDrive = articulationBody.xDrive;
            jointXDrive.stiffness = stiffness;
            jointXDrive.damping = damping;
            jointXDrive.forceLimit = forceLimit;
            articulationBody.xDrive = jointXDrive;
        }
    }
}
