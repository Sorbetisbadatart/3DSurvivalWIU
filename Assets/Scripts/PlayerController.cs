using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Camera
    [SerializeField] private CinemachineVirtualCamera _FirstPersonCamera;
    [SerializeField] private CinemachineVirtualCamera _ThirdPersonCamera;
    private int _currentCam = 1;

    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerInput _playerInput;
    private InputActionAsset _inputActions;

    [SerializeField] private float mouseSensitivity;
    private InputAction lookAction;
    private Vector2 mouseDelta;
    private float cameraPitch = 0f;
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        lookAction = _playerInput.actions["Look"]; // Reference the "Look" action
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _inputActions = _playerInput.actions;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input =
        _inputActions["Move"].ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);
        if (moveDirection.magnitude > 0)
        {
            _animator.SetBool("IsWalking", true);
            // Modify move direction to where camera is facing
            moveDirection = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * moveDirection;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 100f);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
        // Run
        if (_inputActions["Run"].IsPressed())
            _animator.SetBool("IsRunning", true);
        else
            _animator.SetBool("IsRunning", false);

    }

    private void LateUpdate()
    {
        HandleCameraPitch();
        Look();
        // Switch Camera
        if (_inputActions["SwitchCamera"].WasPressedThisFrame())
        {
            if (_currentCam == 0)
            {
                _currentCam = 1;
                _FirstPersonCamera.Priority = 10;
                _ThirdPersonCamera.Priority = 20;
            }
            else if (_currentCam == 1)
            {
                _currentCam = 0;
                _FirstPersonCamera.Priority = 20;
                _ThirdPersonCamera.Priority = 10;
            }
        }
    }
    private void OnAnimatorMove()
    {
        Vector3 velocity = _animator.deltaPosition;
        _characterController.Move(velocity);
    }

    private void HandleCameraPitch()
    {
        if (_currentCam == 0)
        {
            mouseDelta = lookAction.ReadValue<Vector2>();

            float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;
            cameraPitch -= mouseY;
            cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90);
            _FirstPersonCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
            Debug.Log(cameraPitch);
        }
    }
    public void Look()
    {
        if (_currentCam == 0)
        {
            mouseDelta = lookAction.ReadValue<Vector2>();
            float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
            // Rotate the camera vertically horizontally
           gameObject.transform.Rotate(Vector3.up * mouseX);
        }
    }
}

