using TMPro;

using UnityEngine;

public class UIManager : MonoBehaviour
{
    // References
    
    [SerializeField] private PlayerInteraction playerInteraction;

    // UI References
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private GameObject shopPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerInteraction.OnButtonInteraction += UpdatePlayerBalance;
        PlayerInteraction.OnShopInteraction += ToggleShop;

        UpdatePlayerBalance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdatePlayerBalance()
    {
        currencyText.text = playerInteraction.CurrentBalance.ToString();
    }

    void ToggleShop()
    {
        if (shopPanel.activeSelf)
        {
            // Unlock Cursor and shop Shop
            shopPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // Hide and lock cursor
            shopPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }
}
