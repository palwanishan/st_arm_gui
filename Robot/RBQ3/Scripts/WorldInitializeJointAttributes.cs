using System;
using Unity.Robotics;
using UnityEngine;

// Set the joint values to change how it moves
public class WorldInitializeJointAttributes : MonoBehaviour
{
    // Play with setting these variables
    // to find the best spot behavior
    public float stiffness = 1500;
    public float damping = 100;
    public float forceLimit = 1000;
    public int dynamicVal = 10;

    public int m_degreesOfFreedom = 6; 

    [SerializeField]
    GameObject m_World;
    public GameObject World { get => m_World; set => m_World = value; }

    void Start()
    {
        // Get the articulationbody for each joint
        var linkName = string.Empty;
        for (var i = 0; i < m_degreesOfFreedom; i++)
        {
            var articulationBody = m_World.transform.Find(RBQ3ImuSubscriber.LinkNames[i]).GetComponent<ArticulationBody>();

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

