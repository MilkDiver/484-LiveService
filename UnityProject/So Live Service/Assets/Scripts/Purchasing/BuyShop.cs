using TMPro;
using UnityEngine;

public class BuyShop : MonoBehaviour,IPurchase, IInteractable
{
    public bool purchased => isPurchased;

    public float price => purchasePrice;

    public TextMeshProUGUI textObject => interactionText;

    public string InteractMessage => purchaseMessage;

    private bool isPurchased;
    private string purchaseMessage;
    private float purchasePrice;

    [SerializeField] private GameObject spawnItem;

    //The text part of floating UI element
    [SerializeField] private TMPro.TextMeshProUGUI interactionText;

    private void Start()
    {
        interactionText.text = purchaseMessage;
    }

    private void SpawnItem()
    {
        Instantiate(spawnItem);
    }

    public void Interact()
    {
        if (isPurchased != true && CurrencyManagement.Instance.CurrentBalance >= purchasePrice)
        {
            CurrencyManagement.Instance.CurrentBalance -= purchasePrice;

            SpawnItem();

        }
        if (isPurchased == false)
        {
            Debug.Log("Not Enought Money");
        }
    }
}
