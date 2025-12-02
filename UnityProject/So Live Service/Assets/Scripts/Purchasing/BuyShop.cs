using TMPro;
using UnityEngine;

public class BuyShop : MonoBehaviour,IPurchase, IInteractable
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

    [SerializeField] private GameObject spawnItem;

    [SerializeField] private GameObject spawnPositionOBJ;

    //The text part of floating UI element
    [SerializeField] private TMPro.TextMeshProUGUI interactionText;

    private void Start()
    {
        interactionText.text = CreateText();
    }

    private string CreateText()
    {
        string text = string.Empty;

        text = $"${purchasePrice} {displayedPurchaseMessage}";

        return text;
    }

    private void SpawnItem()
    {
        Instantiate(spawnItem,spawnPositionOBJ.transform.position,Quaternion.identity);
    }

    public void Interact()
    {
        if (isPurchased != true && CurrencyManagement.Instance.CurrentBalance >= purchasePrice)
        {
            CurrencyManagement.Instance.CurrentBalance -= purchasePrice;

            SpawnItem();

            UIManager.Instance.UpdatePlayerBalance();
        }
        if (isPurchased == false)
        {
            Debug.Log("Not Enought Money");
        }
    }
}
