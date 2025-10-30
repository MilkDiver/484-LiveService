using TMPro;

using UnityEngine;

public class UIManager : MonoBehaviour
{
    // References
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private PlayerInteraction playerInteraction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerInteraction.OnButtonInteraction += UpdatePlayerBalance;

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
}
