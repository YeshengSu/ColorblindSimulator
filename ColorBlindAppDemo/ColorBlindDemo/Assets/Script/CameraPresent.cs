using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPresent : MonoBehaviour {

    private CamDeviceManager CamDeviceManager;
    private Material mat;

	// Use this for initialization
	void Start () {
        // set transform for different divices
#if UNITY_IOS || UNITY_IPHONE
        transform.Rotate (new Vector3 (0, 180, 90));
#elif UNITY_ANDROID
        transform.Rotate (new Vector3 (0, 0, -90));
#elif UNITY_STANDALONE_WIN
        transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
#endif
        // get camera device
        CamDeviceManager = GameObject.Find("CameraDeviceManager").GetComponent<CamDeviceManager>();

        mat = gameObject.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {

        if (CamDeviceManager.camTexture != null && !CamDeviceManager.requestingCamera)
        {
            float worldScreenHeight = Camera.main.orthographicSize * 2;
            float worldScreenWidth = worldScreenHeight / CamDeviceManager.camTexture.height * CamDeviceManager.camTexture.width;

#if UNITY_STANDALONE_WIN
            transform.localScale = new Vector3(worldScreenWidth, -worldScreenHeight, 1);
#else
            if (CamDeviceManager.isBackCamera)
	        {
                transform.localScale = new Vector3(worldScreenWidth, worldScreenHeight, 1);
	        }
            else
            {
                transform.localScale = new Vector3(-worldScreenWidth, worldScreenHeight, 1);
            }
#endif

            mat.SetTexture("_MainTex", CamDeviceManager.camTexture);
        }
    }
}
