using UnityEngine.InputSystem;

public enum Purchasable
{
    Door,
    Object
}

public interface IPurchase
{
    public string interactMessage { get; }

    public float price { get; }

    public bool purchased { get; }

    public TMPro.TextMeshProUGUI textObject { get; }
}
