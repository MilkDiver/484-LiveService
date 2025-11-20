using UnityEngine;

public class CurrencyManagement : MonoBehaviour
{
    public static CurrencyManagement Instance { get; private set; }

    // Data
    [SerializeField] private float currentBalance = 0.0f;

    public float CurrentBalance
    {
        get { return currentBalance; }
        set { currentBalance = value; }
    }

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

    public void ChangeBalance(float change)
    {
        currentBalance += change;
        UIManager.Instance.UpdatePlayerBalance();
    }

    //This can be added on
    public void ButtonCicked()
    {
        Debug.Log("Yep");

        ChangeBalance(0.25f);
    }
}
