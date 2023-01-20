using System;
using Unity.Robotics;
using UnityEngine;

// Set the joint values to change how it moves
public class RBQ3InitializeJointAttributes : MonoBehaviour
{
    // Play with setting these variables
    // to find the best spot behavior
    public float stiffness = 1500;
    public float damping = 100;
    public float forceLimit = 1000;
    public int dynamicVal = 10;

    int k_NumRobotJoints = 12; 

    [SerializeField]
    GameObject m_RBQ3;
    public GameObject RBQ3 { get => m_RBQ3; set => m_RBQ3 = value; }

    void Start()
    {
        // Get the articulationbody for each joint
        var linkName = string.Empty;
        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            var articulationBody = m_RBQ3.transform.Find(RBQ3JointSubscriber.LinkNames[i]).GetComponent<ArticulationBody>();

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

