using TMPro;

using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // References    
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private CurrencyManagement currencyManagement;

    // UI References
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private GameObject shopPanel;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerInteraction.OnButtonInteraction += UpdatePlayerBalance;
        PlayerInteraction.OnShopInteraction += ToggleShop;

        UpdatePlayerBalance();
    }

    void UpdatePlayerBalance()
    {
        currencyText.text = playerInteraction.CurrentBalance.ToString();
    }

    void ToggleShop()
    {
        if (shopPanel.activeSelf)
        {
            shopPanel.SetActive(false);
        }
        else
        {
            shopPanel.SetActive(true);
        }
        
    }
}
