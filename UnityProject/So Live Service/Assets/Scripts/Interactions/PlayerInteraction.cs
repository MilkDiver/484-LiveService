using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerInteraction : MonoBehaviour
{
    // Delegates
    public delegate void PlayerInteractionDelegate();

    // Events
    public static event PlayerInteractionDelegate OnButtonInteraction;
    public static event PlayerInteractionDelegate OnShopInteraction;

    // References
    [SerializeField] private InputActionReference interactionAction;
    [SerializeField] private InputActionReference shopInputAction;

    // Data
    [SerializeField] private float currentBalance = 0.0f;
    [SerializeField] private GameObject currentInteractingObject = null;

    // Properties
    public float CurrentBalance { get => currentBalance; set { currentBalance = value; } }
    public GameObject CurrentInteractingObject { get => currentInteractingObject; set {  currentInteractingObject = value; } }

    private void OnEnable()
    {
        if (interactionAction != null && interactionAction.action != null)
            interactionAction.action.performed += OnInteractionPerformed;

        if (shopInputAction != null && shopInputAction.action != null)
            shopInputAction.action.performed += OnShopInteractionPerformed;
    }

    private void OnDisable()
    {
        if (interactionAction != null && interactionAction.action != null)
            interactionAction.action.performed -= OnInteractionPerformed;

        if (shopInputAction != null && shopInputAction.action != null)
            shopInputAction.action.performed -= OnShopInteractionPerformed;
    }

    private void OnInteractionPerformed(InputAction.CallbackContext ctx)
    {
        ButtonControl button = (ButtonControl) ctx.control;
        // Ensure this came from the mouse left button (ignore other bindings/devices)
        if (button == Mouse.current.leftButton)
        {
            ButtonInteraction();
        }

        if (button == Keyboard.current.eKey)
        {
            if (currentInteractingObject != null)
            {
                switch (currentInteractingObject.name)
                {
                    case "DoorTriggerCollider":
                        if (currentBalance >= 10)
                        {
                            currentInteractingObject.GetComponent<Door>().OpenDoor();
                            currentBalance -= 10;
                        }
                        else
                        {
                            Debug.Log("Not Enough Money to Open Door!");
                        }
          
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void OnShopInteractionPerformed(InputAction.CallbackContext ctx)
    {
        OnShopInteraction.Invoke();
    }

    private void ButtonInteraction()
    {
        currentBalance += 0.25f;
        OnButtonInteraction.Invoke();
    }
}
