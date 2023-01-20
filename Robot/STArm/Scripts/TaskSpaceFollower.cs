using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSpaceFollower : MonoBehaviour
{
    [SerializeField] GameObject followingObject;
    [SerializeField] GameObject TaskSpaceDefiner;

    public bool manipulationMode = false;
    public float gripperValue = 0;
    [Space]
    public bool isEstimateWeight = false;

    TaskSpaceTriggerActivate detector;

    private void Start()
    {
        detector = followingObject.GetComponent<TaskSpaceTriggerActivate>();
    }

    void Update()
    {
        if (manipulationMode)
        {            
            if (detector.inTaskSpace)
            {
                this.transform.position = new Vector3(followingObject.transform.position.x, followingObject.transform.position.y, followingObject.transform.position.z);
                this.transform.rotation = new Quaternion(followingObject.transform.rotation.x, followingObject.transform.rotation.y, followingObject.transform.rotation.z, followingObject.transform.rotation.w);

                if(detector.isActivated)
                {
                    gripperValue = detector.triggerValue;
                }

                if (detector.buttonValueX == 1)
                {
                    isEstimateWeight = true;
                }
                else
                { 
                    isEstimateWeight = false;
                } 

            }
        }
    }
}
