using UnityEngine;

public enum SightMode
{
    OtherVision,
    NormalVision,
    StaticVision,
}

public class CameraViewChange : MonoBehaviour
{
    [SerializeField] private GameObject testObject;

    [SerializeField] private Color cameraColor;

    [SerializeField] private SightMode currentSight;

    private void Awake()
    {
        
    }

    private void SwitchView(SightMode mode)
    {
        
    }

}
