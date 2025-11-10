using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] GameObject[] inventory = new GameObject[3];
    private int selectedItem = 0;
    private float scrollWheel = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Selected Item Slot: " + selectedItem);

        // Updates the mouse's scroll wheel input
        scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        // Updates what item is being held in the player's hand
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                inventory[i].SetActive(false);
                if (i == selectedItem)
                {
                    inventory[selectedItem].SetActive(true);
                }
            }
        }

        // Checks to see if the mouse has scrolled either up or down to switch between whats in the player's hand

        // SCROLLED AT ALL
        if (scrollWheel != 0.0f)
        {
            // UP
            if (scrollWheel > 0.0f)
            {
                // CAN'T GO ABOVE MAX INVENTORY SIZE
                if(selectedItem < inventory.Length)
                {
                    selectedItem++;
                }
            }

            // DOWN
            else if(scrollWheel < 0.0f)
            {
                // CAN'T GO BELOW 0
                if(selectedItem > 0)
                {
                    selectedItem--;
                }
            }
        }
    }
}
