using System.Collections;

using UnityEngine;

using static UnityEngine.Rendering.DebugUI;

public class CurrencyManagement : MonoBehaviour
{
    public static CurrencyManagement Instance { get; private set; }

    // Data
    [SerializeField] private float currentBalance = 0.000f;
    [SerializeField] private bool clickTimer = false;

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
        currentBalance = Mathf.Round(currentBalance * 1000f) / 1000f;
        UIManager.Instance.UpdatePlayerBalance();
    }

    //This can be added on
    public void ButtonCicked()
    {
        Debug.Log("Yep");

        if (clickTimer == false)
        {
            ChangeBalance(0.001f);
            clickTimer = true;
            StartCoroutine(ClickTimer());
        }
        
    }

    IEnumerator ClickTimer()
    {
        yield return new WaitForSeconds(0.2f);

        clickTimer = false;
    }
}
