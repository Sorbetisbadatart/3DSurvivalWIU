using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour
{

    private int health = 100;
    // Camera
    [SerializeField] private CinemachineVirtualCamera _FirstPersonCamera;
    [SerializeField] private CinemachineFreeLook _FreeLookCamera;
    private int _currentCam = 1;

    // Animator and input
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerInput _playerInput;
    private InputActionAsset _inputActions;
    // Mouse delta for camea 
    [SerializeField] private float mouseSensitivity;
    private InputAction lookAction;
    private Vector2 mouseDelta;
    private float cameraPitch = 0f;

    // Gravity
    [SerializeField] private float Gravity;
    private Vector3 GravityVelocity = Vector3.zero;

    // Jump
    private Vector3 JumpVelocity;
    private InputAction JumpAction;
    private float JumpHeight = 3.0f;

    private Vector3 move = Vector3.zero;
    private  float Speed = 3;

    //interact
    private readonly int InteractRange = 5;

    public AudioSource footstepsSfx, sprintSfx;

    public bool _isGrounded;

    ThirstNHunger ThirstHunger;

    // Water Layer
    public LayerMask waterLayer;

    private List<IEnumerator> _attackQueue = new List<IEnumerator>();
    private bool _isAttack = false;
    [SerializeField] private string[] _attackNames;
    private int _attackStep;

    [SerializeField] SkinnedMeshRenderer skinmesh;
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _isGrounded = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ThirstHunger = GetComponent<ThirstNHunger>();
        _inputActions = _playerInput.actions;
        lookAction = _playerInput.actions["Look"];
        JumpAction = _playerInput.actions["Jump"];

    }

    // Update is called once per frame
    void Update()
    {

        CheckGrounded();
        Fall();
        Jump();
        Vector2 input =
        _inputActions["Move"].ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);


        if (moveDirection.magnitude > 0)
        {
            moveDirection = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * moveDirection;
            // Rotate the character facing towards the move direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 1000f);
        }


        #region Animation Trigger

        if (_inputActions["Move"].IsPressed() && _isGrounded)
        {
            _animator.SetBool("IsWalking", true);
            footstepsSfx.enabled = true;
        }
        else
        {
            footstepsSfx.enabled = false;
            _animator.SetBool("IsWalking", false);
        }
        // Run
        if (_inputActions["Run"].IsPressed() && _isGrounded)
        {
            _animator.SetBool("IsRunning", true);
            if (_currentCam == 0)
                _FirstPersonCamera.transform.localPosition = new Vector3(-0.200000003f, 0.920000017f, 0.889999986f);
            sprintSfx.enabled = true;
            Speed = 9;
        }
        else
        {
            Speed = 3;
            _animator.SetBool("IsRunning", false);
            if (_currentCam == 0)
                _FirstPersonCamera.transform.localPosition = new Vector3(0, 1.36600006f, 0.5f);
            sprintSfx.enabled = false;
        }

        if (_inputActions["Attack"].IsPressed() && _isGrounded)
        {
            if (_attackQueue.Count < 3)
            {

                _attackQueue.Add(PerformAttack());
                AudioManager.Instance.PlaySFX("Punch");
            }

            if (_attackQueue.Count == 1)
            {
                StartCombo();
            }

        }
      

       

        // Jump
        if (_inputActions["Jump"].IsPressed() && _isGrounded)
        {
            _animator.SetBool("IsJump", true);
            _characterController.Move(moveDirection * Speed * Time.deltaTime);
        }
        else
        {
            _animator.SetBool("IsJump", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            AudioManager.Instance.PlaySFX("Jump");
        }

        // Fall
        if (!_isGrounded)
        {

            _animator.SetBool("IsFalling", true);
            //move = transform.right * input.x + transform.forward * input.y;
            _characterController.Move(Speed * Time.deltaTime * moveDirection);
        }
        else
        {
            _animator.SetBool("IsFalling", false);
            move = Vector3.zero;
        }

        // Land
        if (_isGrounded)
            _animator.SetBool("HasLanded", true);
        else
            _animator.SetBool("HasLanded", false);

        #endregion

        _characterController.Move((JumpVelocity + moveDirection * Speed) * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }
    private IEnumerator PerformAttack()
    {
        _attackStep++;
        _animator.SetInteger("AttackStep", _attackStep);
        while (!
        IsCurrentAnimationReadyForNextStep(_attackNames[_attackStep - 1]))
        {
            yield return null;
        }
        if (_attackStep >= _attackQueue.Count)
        {
            AudioManager.Instance.PlaySFX("Punch");
            ResetCombo();
        }
        else
        {
            StartCoroutine(_attackQueue[_attackStep]);
        }
    }

    private void StartCombo()
    {
        _isAttack = true;
        _animator.SetBool("IsAttack", _isAttack);
        StartCoroutine(_attackQueue[0]);
    }

    private bool IsCurrentAnimationReadyForNextStep(string name)
    {
        // Check if the current animation has played enough to transition
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 0.7f &&
        stateInfo.IsName(name); // Adjust based on when you want to allow

    }

    private void ResetCombo()
    {
        _isAttack = false;
        _attackStep = 0;
        _animator.SetInteger("AttackStep", _attackStep);
        _animator.SetBool("IsAttack", false);
        _attackQueue.Clear();
    }

    private void Interact()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange, 8))
        {
            Debug.Log("Hit object: " + hitInfo.collider.gameObject.name);
            Door item = hitInfo.collider.GetComponent<Door>();

            if (item.isOpen)
                StartCoroutine(item.CloseDoor());
            else
                StartCoroutine(item.OpenDoor());
        }

        if (Physics.Raycast(ray, out RaycastHit hitInfo1, InteractRange, waterLayer))
        {
            Debug.Log(hitInfo1.collider.gameObject.name);
            ThirstHunger.GainThirst(1);
            AudioManager.Instance.PlaySFX("Drink");
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.layer == 4)
            _animator.SetBool("IsSwimming", true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 4)
            _animator.SetBool("IsSwimming", false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), GetComponent<CapsuleCollider>().bounds.size.x / 2);
    }
    private void CheckGrounded()
    {
        //Boxcast to detect whether player touching ground
        Physics.SphereCast(
            origin: new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z),
            radius: GetComponent<CapsuleCollider>().bounds.size.x / 2,
            direction: Vector2.down,
            hitInfo: out RaycastHit hitResult,
            maxDistance: 1.0f
            );
        _isGrounded = hitResult.collider != null;
    }

    private void LateUpdate()
    {
        Look();

        // Switch Camera
        if (_inputActions["SwitchCamera"].WasPressedThisFrame())
        {
            if (_currentCam == 0)
            {
                // CurrentCam => FirstPerson
                _currentCam = 1;
                _FirstPersonCamera.Priority = 10;
                _FreeLookCamera.Priority = 20;
                AddSkin();
            }
            else
             if (_currentCam == 1)
            {
                
                // CurrentCam => Freelook
                _currentCam = 0;
                _FirstPersonCamera.Priority = 20;

                _FreeLookCamera.Priority = 10;
                Invoke(nameof(RemoveSkin), 1);
                RemoveSkin();
            }
        }
    }
    private void OnAnimatorMove()
    {
        Vector3 velocity = _animator.deltaPosition;
        _characterController.Move(velocity);
    }

    public void Look()
    {
        mouseDelta = lookAction.ReadValue<Vector2>();
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        // Rotate the camera vertically horizontally
        gameObject.transform.Rotate(Vector3.up * mouseX);
    }
   
    public void Fall()
    {
        //falling
        GravityVelocity.y += Gravity * Time.deltaTime;
        if (_characterController.isGrounded && JumpVelocity.y < 0)
        {
            JumpVelocity.y = -2f;
        }
        JumpVelocity.y += Gravity * Time.deltaTime;
    }

    void Jump()
    {
        if (JumpAction.IsPressed() && _characterController.isGrounded)
        {
            JumpVelocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }
    }
    private void RemoveSkin()
    {
        skinmesh.enabled = false;
    }
    private void AddSkin()
    {
        skinmesh.enabled = true;
    }
}

