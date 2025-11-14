using System.Linq;

using UnityEngine;

public class Door : MonoBehaviour
{
    // References
    [SerializeField] private TMPro.TextMeshProUGUI doorInteractionText;

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
