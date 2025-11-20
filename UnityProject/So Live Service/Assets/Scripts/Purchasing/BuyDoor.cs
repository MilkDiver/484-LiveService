using TMPro;
using UnityEngine;

public class BuyDoor : MonoBehaviour, IPurchase, IInteractable
{
    public bool purchased => isPurchased;

    public float price => purchasePrice;

    public TextMeshProUGUI textObject => interactionText;

    public string InteractMessage => purchaseMessage;

    [SerializeField] private bool isPurchased;
    [SerializeField] private string purchaseMessage;
    [SerializeField] private float purchasePrice;

    //The text part of floating UI element
    [SerializeField] private TextMeshProUGUI interactionText;

    private void Start()
    {
        interactionText.text = purchaseMessage;
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

    private void OpenDoor()
    {
        this.transform.parent.gameObject.SetActive(false);
    }

    private void CloseDoor()
    {
        this.transform.parent.gameObject.SetActive(true);
    }

    public void Interact()
    {
        if (isPurchased != true && CurrencyManagement.Instance.CurrentBalance >= purchasePrice)
        {
            CurrencyManagement.Instance.CurrentBalance -= purchasePrice;

            isPurchased = true;

            OpenDoor();
        }
        if (isPurchased == false) 
        {
            Debug.Log("Not Enought Money");
        }
    }
}
