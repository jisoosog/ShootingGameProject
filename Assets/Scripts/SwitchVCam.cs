
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    //Byter camera mode när spelaren vill sikta/aim in o skjuta
    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private int priorityBoostAmount = 10;

    [SerializeField]
    private Canvas thirdPersonCanvas;

    [SerializeField]
    private Canvas aimCanvas;

    private InputAction aimAction;
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();   //When the button is pressed 
        aimAction.canceled += _ => CancelAim();   //When we stop pressing the button
    }
    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityBoostAmount; //Boost the priority of the virtual camera
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
    }
    private void CancelAim()
    {
        virtualCamera.Priority -= priorityBoostAmount;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;

    }

}
