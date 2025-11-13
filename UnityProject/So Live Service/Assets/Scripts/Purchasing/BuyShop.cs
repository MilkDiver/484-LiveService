using TMPro;
using UnityEngine;

public class BuyShop : MonoBehaviour, IPurchase
{
    public string interactMessage => purchaseMessage;

    public bool purchased => isPurchased;

    public float price => purchasePrice;

    public TextMeshProUGUI textObject => interactionText;

    private bool isPurchased;
    private string purchaseMessage;
    private float purchasePrice;

    //The text part of floating UI element
    [SerializeField] private TMPro.TextMeshProUGUI interactionText;

    private void Start()
    {
        interactionText.text = purchaseMessage;
    }

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
}
