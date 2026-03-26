using System.Collections.Generic;
using System.Collections;
using NUnit.Framework;

using UnityEngine;
using UnityEngine.UI;

public class LootboxManager : MonoBehaviour
{
    public static LootboxManager Instance { get; private set; }

    // Data
    public List<GameObject> lootboxRewards = new List<GameObject>();
    public Dictionary<GameObject, string> collection = new Dictionary<GameObject, string>();
    public int lootboxIncrement;

    // References
    [SerializeField] private Slider lootboxSlider;
    [SerializeField] private GameObject player;
    [SerializeField] private UIManager uiManager;

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
        InitializeCollection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeCollection()
    {
        foreach (GameObject reward in lootboxRewards)
        {
            collection.Add(reward, "Not Collected");
        }
    }

    public void IncreaseLootboxIncrement()
    {
        Debug.Log("Lootbox Increment!");
        lootboxIncrement++;
        lootboxSlider.value++;

        // Once lootboxIncrement reaches the max
        if (lootboxIncrement >= 20)
        {
            // Reset lootbox increment and start lootbox animation
            lootboxIncrement = 0;
            lootboxSlider.value = 0;
            StartCoroutine(OpenLootbox());
        }
    }

    IEnumerator OpenLootbox()
    {
        // Start Animation

        yield return new WaitForSeconds(0.5f);

        // Spawn random gameobject
        int index = Random.Range(0, lootboxRewards.Count);
        GameObject rewardPrefab = lootboxRewards[index];

        GameObject spawned = Instantiate(rewardPrefab, player.transform.position, player.transform.rotation);
        spawned.name = rewardPrefab.name;

        collection[spawned] = "Collected";
    }

    public void PurchaseLootbox()
    {
        Debug.Log("Purchasing Lootbox!");
        float playerBalance = player.GetComponent<PlayerInteraction>().CurrentBalance;
        Debug.Log(playerBalance);
        if (playerBalance > 10)
        {
            Debug.Log("Purchasing Lootbox!");
            player.GetComponent<PlayerInteraction>().CurrentBalance = playerBalance - 10;
            uiManager.UpdatePlayerBalance();
            StartCoroutine(OpenLootbox());
        }
    }
}
