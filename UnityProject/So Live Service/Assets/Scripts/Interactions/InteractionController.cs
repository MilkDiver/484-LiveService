using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private float interact;

    [SerializeField] private Camera playerCam;

    [SerializeField] private LayerMask interactableLayers;

    [SerializeField] private Transform targetOBJ;

    [SerializeField] TextMeshProUGUI interactionText;

    [SerializeField] private float minScale, maxScale;

    [SerializeField] private float minDistance, maxDistance;

    [SerializeField] private float interactionDistance = 5f;

    [SerializeField] private float interactableUIRadius;

    [SerializeField] private Vector3 offset;

    [SerializeField] private List<GameObject> centerList;

    IInteractable currentTargetInteractable;

    IPurchase currentTargetPurchasable;

    [SerializeField] private float testVar1, testVar2, testVar3;

    public float Interact
    {
        get { return interact; }
        set { interact = value; }
    }

    public void Update()
    {
        UpdateCurrentInteractable();

        UpdateInteractionText();

        UpdateInteraction();
    }

    private void UpdateInteraction()
    {
        if (currentTargetInteractable != null && interact != 0)
        {
            currentTargetInteractable.Interact();
        }
    }

    private void UpdateInteractionText()
    {

        if (targetOBJ == null)
        {
            interactionText.text = string.Empty;
            return;
        }

        currentTargetInteractable = targetOBJ.GetComponent<IInteractable>();

        if (currentTargetInteractable == null)
        {
            interactionText.text = string.Empty;
            return;
        }

        //ScaleInteractionText();

        MoveInteractionText();

        interactionText.text = currentTargetInteractable.InteractMessage;

    }

    /*
    private void ScaleInteractionText()
    {
        Vector3 screenPos = playerCam.WorldToScreenPoint(targetOBJ.position);

        if (screenPos.z < 0)
            return;

        Debug.Log("Test");

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        float distance = Vector3.Distance(playerCam.transform.position, targetOBJ.position);
        float t = Mathf.InverseLerp(maxDistance, minDistance, distance);
        float scale = Mathf.Lerp(minScale, maxScale, t);

        testVar1 = distance;
        testVar2 = t;
        testVar3 = scale;

        interactionText.rectTransform.localScale = Vector3.one * scale;

    }
    */
    private void MoveInteractionText()
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayers);

        Vector3 worldPos = GetTopRightOfObject(hit.collider);

        // Convert to screen position
        Vector3 screenPos = playerCam.WorldToScreenPoint(worldPos);

        interactionText.rectTransform.position = screenPos;
    }

    private Vector3 GetTopRightOfObject(Collider col)
    {
        // Get the bounding box of the object
        Bounds b = col.bounds;

        // Define top-right corner (relative to object bounds)
        // X = max, Y = max (top), Z = center for depth
        Vector3 topRightWorld = new Vector3(b.center.x, b.center.y, b.center.z);

        // Add a small offset if you want to move the label away slightly
        topRightWorld += offset;

        return topRightWorld;
    }

    private void UpdateCurrentInteractable()
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayers);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactObj))
            {
                currentTargetInteractable = interactObj;
            }

            else if (hit.collider.TryGetComponent<IPurchase>(out IPurchase purchaseObj))
            {
                currentTargetPurchasable = purchaseObj;
            }
        }

        

        
            

       

        targetOBJ = hit.collider?.GetComponent<Transform>();
    }

}
