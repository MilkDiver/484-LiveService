using System;
using TMPro;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera playerCam;

    [SerializeField] TextMeshProUGUI interactionText;

    [SerializeField] float interactionDistance = 5f;

    IInteractable currentTargetInteractable;

    public void Update()
    {
        UpdateCurrentInteractable();

        UpdateInteractionText();
    }

    private void UpdateInteractionText()
    {
        if (currentTargetInteractable == null)
        {
            interactionText.text = string.Empty;
            return;
        }

        interactionText.text = currentTargetInteractable.InteractMessage;
    }

    private void UpdateCurrentInteractable()
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        Physics.Raycast(ray, out RaycastHit hit, interactionDistance);

        currentTargetInteractable = hit.collider?.GetComponent<IInteractable>();
    }

}
