using TMPro;
using UnityEngine;

public class BuyDoor : MonoBehaviour, IPurchase, IInteractable
{
    public bool purchased => isPurchased;

    public float price => purchasePrice;

    public TextMeshProUGUI textObject => interactionText;

    public string InteractMessage => interactionMessage;

    public string displayedMessage => displayedPurchaseMessage;

    [SerializeField] private bool isPurchased;
    [SerializeField] private string interactionMessage;
    [SerializeField] private string displayedPurchaseMessage;
    [SerializeField] private float purchasePrice;

    //The text part of floating UI element
    [SerializeField] private TextMeshProUGUI interactionText;

    private void Start()
    {
        interactionText.text = CreateText();
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerGeneralCollider")
        {
            interactionText.gameObject.SetActive(true);
            other.transform.parent.gameObject.GetComponent<PlayerInteraction>().CurrentInteractingObject = this.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "PlayerGeneralCollider")
        {
            interactionText.gameObject.SetActive(false);
        }
    }
    */

    private string CreateText()
    {
        string text = string.Empty;

        text = $"${purchasePrice} {displayedPurchaseMessage}";

        return text;
    }

    private void OpenDoor()
    {
        gameObject.SetActive(false);
    }

    private void CloseDoor()
    {
        gameObject.SetActive(true);
    }

    public void Interact()
    {
        if (isPurchased != true && CurrencyManagement.Instance.CurrentBalance >= purchasePrice)
        {
            CurrencyManagement.Instance.CurrentBalance -= purchasePrice;

            isPurchased = true;

            OpenDoor();

            UIManager.Instance.UpdatePlayerBalance();
        }
        if (isPurchased == false) 
        {
            Debug.Log("Not Enought Money");
        }
    }
}
