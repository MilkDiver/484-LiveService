using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float senseX;
    [SerializeField] private float senseY;

    [SerializeField] private Transform orientation;

    //[SerializeField] private Transform weaponOrientation;

    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;

    [SerializeField] private Vector2 mouseVal;

    [SerializeField] private float viewAngle;

    [SerializeField] private CinemachineCamera virtualCam;

    public Transform Orientation
    {
        get
        {
            return orientation;
        }
        set
        {
            orientation = value;
        }
    }
    /*
    public Transform WeaponOrientation
    {
        get 
        {
            return weaponOrientation;
        }
        set 
        {
            weaponOrientation = value;
        }
    }
    */
    public float ViewAngle
    {
        get
        {
            return viewAngle;
        }
        set
        {
            viewAngle = value;
        }
    }

    public float XRotation
    {
        get
        {
            return xRotation;
        }
        set
        {
            xRotation = value;
        }
    }

    public float YRotation
    {
        get
        {
            return yRotation;
        }
        set
        {
            yRotation = value;
        }
    }

    public float SenseX
    {
        get
        {
            return senseX;
        }
        set
        {
            senseX = value;
        }
    }

    public float SenseY
    {
        get
        {
            return senseY;
        }
        set
        {
            senseY = value;
        }
    }

    public Vector2 MouseVal
    {
        get
        {
            return mouseVal;
        }
        set
        {
            mouseVal = value;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        virtualCam = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineCamera>();
    }

    public void DoFOV(float endValue)
    {
        CameraExtensions.DOFOV(virtualCam, endValue, 0.25f);
    }
    
    public void DoTilt(float zTilt)
    {
        CameraExtensions.DOTilt(virtualCam, zTilt, 0.5f);
    }
    
}
