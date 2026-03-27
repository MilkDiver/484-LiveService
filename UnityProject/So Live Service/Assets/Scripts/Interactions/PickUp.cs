using UnityEngine;

public class PickUp : MonoBehaviour, IInteractable
{
    public string InteractMessage => throw new System.NotImplementedException();

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    [SerializeField] private GameObject _pickedUpItem;

    [SerializeField] private GameObject _holdLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PickUpItem()
    {

    }

    private void DropItem()
    {

    }
}
