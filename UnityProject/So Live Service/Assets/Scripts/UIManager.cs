using TMPro;

using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // References    
    //[SerializeField] private PlayerInteraction playerInteraction;
    //[SerializeField] private CurrencyManagement currencyManagement;

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
        //PlayerInteraction.OnButtonInteraction += UpdatePlayerBalance;
        //PlayerInteraction.OnShopInteraction += ToggleShop;

        UpdatePlayerBalance();
    }

    public void UpdatePlayerBalance()
    {
        currencyText.text = $"{CurrencyManagement.Instance.CurrentBalance}";
    }

    public void ToggleShop()
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
