using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwrapMode : MonoBehaviour {

    //base
    private UIManager UIManager;
    private GameObject colorBlindToggles;
    private GameObject daltoniseToggles;

    //temporary
    private bool startPosFlag;
    private Vector2 startFingerPos;
    private Vector2 nowFingerPos;
    private float xMoveDistance;
    private float sensitivity;
    // Use this for initialization
    void Start () {
        startPosFlag = true;
        sensitivity = 0.1f;

        UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        colorBlindToggles = GameObject.Find("Canvas/SimulatorSetting/ColorBlindSetting");
        daltoniseToggles = GameObject.Find("Canvas/SimulatorSetting/DaltonizationSetting");

    }
	
	// Update is called once per frame
	void Update () {
        int backval = judueFinger();
        if (startPosFlag)
            SetMode(backval);
        
    }

    void SetMode(int backval)
    {
        int childrenNum = 0;
        int currentMode = 0;
        if (UIManager.isDaltonization)
        {
            childrenNum = daltoniseToggles.transform.childCount;
            currentMode = UIManager.currentDaltonization;
            currentMode += backval;

            if (currentMode > childrenNum - 2)
            {
                currentMode = 0;
            }
            if (currentMode < 0)
            {
                currentMode = childrenNum - 2;
            }
            daltoniseToggles.transform.GetChild(currentMode).GetComponent<Toggle>().isOn = true;
        }
        else
        {
            childrenNum = colorBlindToggles.transform.childCount;
            currentMode = UIManager.currentColorBlind;
            currentMode += backval;
            if (currentMode > childrenNum - 2)
            {
                currentMode = 0;
            }
            if (currentMode < 0)
            {
                currentMode = childrenNum - 2;
            }
            colorBlindToggles.transform.GetChild(currentMode).GetComponent<Toggle>().isOn = true;

        }
    }

    int judueFinger()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began && startPosFlag == true)
        {
            startFingerPos = Input.GetTouch(0).position;
            startPosFlag = false;
        }
        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            startPosFlag = true;
        }
        nowFingerPos = Input.GetTouch(0).position;
        xMoveDistance = Mathf.Abs(nowFingerPos.x - startFingerPos.x);

        int backValue = 0;
        if (xMoveDistance > sensitivity*Screen.width)
        {
            if (nowFingerPos.x - startFingerPos.x > 0)
            {
                backValue = -1;
            }
            else
            {
                backValue = 1;
            }
        }
        return backValue;
    }
}
