using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInteraction;

public class InputController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCam cam;

    //[SerializeField] private TestPlayerMovement movement;

    [SerializeField] private PlayerMovement movement;

    // Delegates
    public delegate void PlayerInteractionDelegate();

    //[SerializeField] private WeaponVals weaponVals;

    [Header("Input Values")]
    public Vector2 inputDirection;

    public Vector2 mouseDirection;

    public float interact = 0;

    private float jumping = 0;

    private float sprinting = 0;

    private float crouching = 0;

    private bool isReleased;

    public static event PlayerInteractionDelegate OnInteractInteraction;
    public static event PlayerInteractionDelegate OnShopInteraction;

    [Header("Input Action References")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private InputActionReference _crouchAction;
    [SerializeField] private InputActionReference _sprintAction;
    
    [SerializeField] private InputActionReference _pauseAction;
    [SerializeField] private InputActionReference _shopAction;

    [SerializeField] private InputActionReference _interactAction;
    [SerializeField] private InputActionReference _mouseAction;

    private void OnEnable()
    {
        _moveAction.action.Enable();
        _jumpAction.action.Enable();
        _crouchAction.action.Enable();
        _sprintAction.action.Enable();

        //UI
        _shopAction.action.Enable();

        //Mouse
        _interactAction.action.Enable();
        _mouseAction.action.Enable();

        //Player Movement

        _moveAction.action.performed += OnMovePerformed;
        _moveAction.action.canceled += OnMoveCancelled;

        _jumpAction.action.performed += OnJumpPerformed;
        _jumpAction.action.canceled += OnJumpCancelled;

        _crouchAction.action.performed += OnCrouchPerformed;
        _crouchAction.action.canceled += OnCrouchCancelled;

        _sprintAction.action.performed += OnSprintPerformed;
        _sprintAction.action.canceled += OnSprintCancelled;

        //UI
        _shopAction.action.performed += OnShopPerformed;

        //Mouse Movement
        _interactAction.action.performed += OnInteractPerformed;
        _interactAction.action.canceled += OnInteractCancelled;

        _mouseAction.action.performed += OnMousePerformed;
        _mouseAction.action.canceled += OnMouseCancelled;
    }

    private void OnDisable()
    {
        _moveAction.action.Disable();
        _jumpAction.action.Disable();
        _crouchAction.action.Disable();
        _sprintAction.action.Disable();

        _interactAction.action.Disable();
        _mouseAction.action.Disable();
        //Player Movement

        _moveAction.action.performed -= OnMovePerformed;
        _moveAction.action.canceled -= OnMoveCancelled;

        _jumpAction.action.performed -= OnJumpPerformed;
        _jumpAction.action.canceled -= OnJumpCancelled;

        _crouchAction.action.performed -= OnCrouchPerformed;
        _crouchAction.action.canceled -= OnCrouchCancelled;

        _sprintAction.action.performed -= OnSprintPerformed;
        _sprintAction.action.canceled -= OnSprintCancelled;

        //Mouse Movement

        _interactAction.action.performed -= OnInteractPerformed;
        _interactAction.action.canceled -= OnInteractCancelled;

        _mouseAction.action.performed -= OnMousePerformed;
        _mouseAction.action.canceled -= OnMouseCancelled;
    }
    #region InputMethods
    //
    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();

        movement.MoveInput = inputDirection.normalized;
    }
    public void OnMoveCancelled(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();

        movement.MoveInput = inputDirection.normalized;
    }

    //
    public void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumping = context.ReadValue<float>();

        movement.JumpInput = jumping;
    }
    public void OnJumpCancelled(InputAction.CallbackContext context)
    {
        jumping = context.ReadValue<float>();

        movement.JumpInput = jumping;
    }

    //
    public void OnSprintPerformed(InputAction.CallbackContext context)
    {
        sprinting = context.ReadValue<float>();

        movement.SprintInput = sprinting;
    }
    public void OnSprintCancelled(InputAction.CallbackContext context)
    {
        sprinting = context.ReadValue<float>();

        movement.SprintInput = sprinting;
    }

    //
    public void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        crouching = context.ReadValue<float>();

        movement.CrouchInput = crouching;
    }
    public void OnCrouchCancelled(InputAction.CallbackContext context)
    {
        movement.CrouchInput = 0;
    }

    //
    public void OnMousePerformed(InputAction.CallbackContext context)
    {
        mouseDirection = context.ReadValue<Vector2>();

        cam.MouseVal = mouseDirection;
    }
    public void OnMouseCancelled(InputAction.CallbackContext context)
    {
        mouseDirection = context.ReadValue<Vector2>();

        cam.MouseVal = mouseDirection;
    }

    //
    public void OnInteractPerformed(InputAction.CallbackContext context)
    {
        interact = context.ReadValue<float>();

        OnInteractInteraction.Invoke();
    }

    public void OnInteractCancelled(InputAction.CallbackContext context)
    {
        interact = context.ReadValue<float>();

    }

    public void OnShopPerformed(InputAction.CallbackContext context)
    {

    }

    #endregion
}
