using UnityEngine.InputSystem;

public enum Purchasable
{
    Door,
    Object
}

public interface IPurchase
{
    public float price { get; }

    public bool purchased { get; }

    public string displayedMessage { get; }

    public TMPro.TextMeshProUGUI textObject { get; }

    public void Interact();


}


