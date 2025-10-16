using UnityEngine;

public class TestButton : MonoBehaviour, IInteractable
{
    public string InteractMessage => testButtonMessage;

    [SerializeField] public string testButtonMessage;

    public void Interact()
    {
        TestMessage();
    }

    private void TestMessage()
    {
        Debug.Log("The button was pressed");
    }

    
}
