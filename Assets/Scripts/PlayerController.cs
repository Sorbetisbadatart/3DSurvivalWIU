using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField]
    private CharacterController
    _characterController;
    [SerializeField] private PlayerInput _playerInput;
    private InputActionAsset _inputActions;
    // Start is called before the first frame update
    void Start()
    {
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
            Quaternion targetRotation =
            Quaternion.LookRotation(moveDirection);
            transform.rotation =
            Quaternion.Slerp(transform.rotation, targetRotation,
            Time.deltaTime * 10f);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
    }
    private void OnAnimatorMove()
    {
        Vector3 velocity = _animator.deltaPosition;
        _characterController.Move(velocity);
    }
}

