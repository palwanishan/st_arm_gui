using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [SerializeField] InputActionReference controllerActionGrip;
    [SerializeField] InputActionReference controllerActionTrigger;

    private Animator _handAnimator;

    private void Awake()
    {
        controllerActionGrip.action.performed += GripPress;
        controllerActionTrigger.action.performed += TriggerPress;

        _handAnimator = GetComponentInChildren<Animator>();  //GetComponentInChild<Animator>();

    }

    private void GripPress(InputAction.CallbackContext obj)
    {
        _handAnimator.SetFloat("Flex", obj.ReadValue<float>());
    }
    private void TriggerPress(InputAction.CallbackContext obj)
    {
        _handAnimator.SetFloat("Pinch", obj.ReadValue<float>());
    }
}
