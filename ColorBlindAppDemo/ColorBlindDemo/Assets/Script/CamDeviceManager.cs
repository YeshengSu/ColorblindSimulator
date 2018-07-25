using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDeviceManager : MonoBehaviour {

    [HideInInspector]
    public WebCamTexture camTexture;

    public bool isBackCamera = false;
    public bool requestingCamera = true;
    void Start()
    {
#if UNITY_STANDALONE_WIN
        isBackCamera = false;
        StartCoroutine(CallCamera());
#else
        isBackCamera = true;
        StartCoroutine(CallCamera());
#endif

    }

    IEnumerator CallCamera()
    {
        requestingCamera = true;
        if (camTexture != null)
            camTexture.Stop();
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] cameraDevices = WebCamTexture.devices;

            for (int i = 0; i < cameraDevices.Length; i++)
            {
                if (cameraDevices[i].isFrontFacing & !isBackCamera) //front camera
                {
                    string deviceName = cameraDevices[i].name;
                    camTexture = new WebCamTexture(deviceName, Screen.width, Screen.height, 60);
                    camTexture.wrapMode = TextureWrapMode.Mirror;
                    camTexture.Play();
                    break;
                }
                else if(!cameraDevices[i].isFrontFacing & isBackCamera) //back camera
                {
                    string deviceName = cameraDevices[i].name;
                    camTexture = new WebCamTexture(deviceName, Screen.width, Screen.height, 60);
                    camTexture.wrapMode = TextureWrapMode.Mirror;
                    camTexture.Play();
                    break;
                }
            }
            requestingCamera = false;
        }
    }

    public void SwitchCamera()
    {
#if UNITY_STANDALONE_WIN
        isBackCamera = false;
        StartCoroutine(CallCamera());
#else
        isBackCamera = !isBackCamera;
        StartCoroutine(CallCamera());
#endif

    }

}
