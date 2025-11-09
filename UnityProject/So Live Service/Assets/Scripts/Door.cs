using System.Linq;

using UnityEngine;

public class Door : MonoBehaviour
{
    // References
    [SerializeField] TMPro.TextMeshProUGUI doorInteractionText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerGeneralCollider")
        {
            doorInteractionText.gameObject.SetActive(true);
            other.transform.parent.gameObject.GetComponent<PlayerInteraction>().CurrentInteractingObject = this.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "PlayerGeneralCollider")
        {
            doorInteractionText.gameObject.SetActive(false);
        }
    }

    public void OpenDoor()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
