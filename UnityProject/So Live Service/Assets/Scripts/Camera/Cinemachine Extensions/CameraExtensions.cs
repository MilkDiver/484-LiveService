using Unity.Cinemachine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public static class CameraExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="endValue"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static void DOFOV(this CinemachineCamera cam, float fovValue, float duration = 0)
    {
        DOTween.To(() => cam.Lens.FieldOfView, x => cam.Lens.FieldOfView = x, fovValue, duration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="tilt"></param>
    /// <param name="duration"></param>
    public static void DOTilt(this CinemachineCamera cam, float tilt, float duration = 0)
    {
       DOTween.To(() => cam.Lens.Dutch, x => cam.Lens.Dutch = x, tilt, duration);
    }
}
