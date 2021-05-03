using System;
using UnityEngine;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    public Camera ordinaryCamera;

    private bool isPixelPerfect;

    void Awake()
    {
        isPixelPerfect = true;
        ValidateCameras(isPixelPerfect);
    }

    public void TogglePixelPerfect(bool value)
    {
        isPixelPerfect = value;
        ValidateCameras(isPixelPerfect);
    }

    public void ValidateCameras(bool value)
    {
        if (value)
        {
            ordinaryCamera.gameObject.SetActive(false);
        }
        else
        {
            ordinaryCamera.gameObject.SetActive(true);
        }
    }
}
