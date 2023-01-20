using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TaskSpaceTriggerActivate : MonoBehaviour
{
    [SerializeField] InputActionReference controllerActionTriggerLeft;
    [SerializeField] InputActionReference controllerActionTriggerRight;
    [Space]
    [SerializeField] InputActionReference controllerActionXLeft;  // X
    [SerializeField] InputActionReference controllerActionYLeft;  // Y    
    [Space]
    [SerializeField] InputActionReference controllerActionARight; // A
    [SerializeField] InputActionReference controllerActionBRight; // B

    public float triggerValue;
    public float buttonValueX;
    public float buttonValueY;

    public bool isActivated = false;
    public bool inTaskSpace = false;

    private void Awake()
    {
        controllerActionTriggerLeft.action.performed += TriggerPress;
        controllerActionTriggerRight.action.performed += TriggerPress;

        controllerActionXLeft.action.performed += X_A_Press;
        controllerActionXLeft.action.canceled += X_A_Press;
        controllerActionARight.action.performed += X_A_Press;
        controllerActionARight.action.canceled += X_A_Press;

        controllerActionYLeft.action.performed += Y_B_Press;
        controllerActionYLeft.action.canceled += Y_B_Press;
        controllerActionBRight.action.performed += Y_B_Press;
        controllerActionBRight.action.canceled += Y_B_Press;
    }

    private void Y_B_Press(InputAction.CallbackContext obj)
    {
        SetButtonValueY(obj.ReadValue<float>());
    }

    private void X_A_Press(InputAction.CallbackContext obj)
    {
        SetButtonValueX(obj.ReadValue<float>());
    }

    private void SetButtonValueY(float v1)
    {
        if (isActivated)
        {
            buttonValueY = v1;
        }
    }
    private void SetButtonValueX(float v1)
    {
        if (isActivated)
        {
            buttonValueX = v1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "TaskSpaceDefiner")
        {
            inTaskSpace = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "TaskSpaceDefiner")
        {
            inTaskSpace = false;
        }
    }

    public void TriggerPress(InputAction.CallbackContext obj)
    {
        SetTriggerValue("Pinch", obj.ReadValue<float>());
    }

    private void SetTriggerValue(string v1, float v2)
    {
        if (isActivated)
        {
            triggerValue = v2;
        }
    }

    public void SetTriggerTrue()          
    {                                       
        isActivated = true;
    }

    public void SetTriggerFalse()
    {
        isActivated = false;
    }
}
