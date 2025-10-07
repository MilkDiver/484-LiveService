using UnityEngine;
using Unity.Cinemachine;

public class cinamachinePOVExtension : CinemachineExtension
{
    private Vector3 startingRotation;
    [SerializeField] private PlayerCam playerCam;

    protected override void Awake()
    {
        base.Awake();
        // Optional: if you want to be extra safe in Awake
        playerCam = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCam>();
    }

    void Start()
    {
        startingRotation = transform.localRotation.eulerAngles;

        if (playerCam == null)
        {
            playerCam = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCam>();
        }
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam == null || playerCam == null || vcam.Follow == null)
            return;

        if (stage == CinemachineCore.Stage.Aim)
        {
            Vector2 deltaInput = playerCam.MouseVal;

            startingRotation.x += deltaInput.x * playerCam.SenseX * Time.deltaTime;
            startingRotation.y += -deltaInput.y * playerCam.SenseY * Time.deltaTime;

            startingRotation.y = Mathf.Clamp(startingRotation.y, -playerCam.ViewAngle, playerCam.ViewAngle);

            state.RawOrientation = Quaternion.Euler(startingRotation.y, startingRotation.x, 0f);
            
            playerCam.Orientation.rotation = Quaternion.Euler(0, startingRotation.x, 0f);
            //playerCam.WeaponOrientation.rotation = Quaternion.Euler(startingRotation.y, startingRotation.x, 0);
        }
    }
}
