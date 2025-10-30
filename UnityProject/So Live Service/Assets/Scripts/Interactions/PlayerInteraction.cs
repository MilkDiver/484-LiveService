using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerInteraction : MonoBehaviour
{
    // Delegates
    public delegate void PlayerButtonInteractionDelegate();
    // Events
    public static event PlayerButtonInteractionDelegate OnButtonInteraction;

    // References
    [SerializeField] private InputActionReference interactionAction;

    // Data
    [SerializeField] private float currentBalance = 0.0f;

    // Properties
    public float CurrentBalance { get => currentBalance; set { currentBalance = value; } }

    private void OnEnable()
    {
        if (interactionAction != null && interactionAction.action != null)
            interactionAction.action.performed += OnInteractionPerformed;
    }

    private void OnDisable()
    {
        if (interactionAction != null && interactionAction.action != null)
            interactionAction.action.performed -= OnInteractionPerformed;
    }

    private void OnInteractionPerformed(InputAction.CallbackContext ctx)
    {
        // Ensure this came from the mouse left button (ignore other bindings/devices)
        if (ctx.control is ButtonControl button && button == Mouse.current.leftButton)
        {
            ButtonInteraction();
        }
    }

    private void ButtonInteraction()
    {
        currentBalance += 0.25f;
        OnButtonInteraction.Invoke();
    }
}
