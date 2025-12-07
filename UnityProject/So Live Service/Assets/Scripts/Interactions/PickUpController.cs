using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public static PickUpController Instance { get; private set; }

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
}
