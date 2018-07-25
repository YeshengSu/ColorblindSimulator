using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    
    public Material[] colorBlindMatGroup;
    public Material[] daltonizationMatGroup;
    public Texture[] SampleTexGroup;

    public GameObject[] hidenObject;
    private float hidenTime = 5.0f;
    private float hidenAccumulatedTime = 0.0f;
    private bool isHiden = false;

    public GameObject allColorBlindSmapleCompare;
    public GameObject allDaltonizationCompare;
    public GameObject fullScreenSample;

    public GameObject colorBlindnessSelection;
    public GameObject colorDaltonizationSelection;

    public bool isDaltonization = false;
    public int currentColorBlind = 0;
    public int currentDaltonization = 0;

    // Use this for initialization
    void Start ()
    {
        allDaltonizationCompare.SetActive(false);
        allColorBlindSmapleCompare.SetActive(false);
        fullScreenSample.SetActive(true);
        colorDaltonizationSelection.SetActive(false);
        SelectColorBlindMode(0);
        SelectTexture(0);
    }

    void Update()
    {
        hidenAccumulatedTime += Time.deltaTime;
        if (hidenAccumulatedTime >= hidenTime && !isHiden)
        {
            isHiden = true;
            foreach (GameObject item in hidenObject)
            {
                item.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isHiden)
            {
                foreach (GameObject item in hidenObject)
                {
                    item.SetActive(true);
                }
            }
            isHiden = false;
            hidenAccumulatedTime = 0.0f;
        }

    }
    public void ShowAllISmapleCompare()
    {
        fullScreenSample.SetActive(false);
        colorBlindnessSelection.SetActive(false);
        colorDaltonizationSelection.SetActive(false);
        if (isDaltonization)
        {
            allDaltonizationCompare.SetActive(true);
            allColorBlindSmapleCompare.SetActive(false);
        }
        else
        {
            allDaltonizationCompare.SetActive(false);
            allColorBlindSmapleCompare.SetActive(true);
        }
    }

    public void ShowFullScreenSample()
    {
        if (isDaltonization)
        {
            colorDaltonizationSelection.SetActive(true);
        }
        else
        {
            colorBlindnessSelection.SetActive(true);
        }
        allDaltonizationCompare.SetActive(false);
        allColorBlindSmapleCompare.SetActive(false);
        fullScreenSample.SetActive(true);
    }

    public void SelectColorBlindMode(int mode)
    {
        currentColorBlind = mode;
        ColorBlindPresent.COLORBLINDMODE cOLORBLINDMODE = (ColorBlindPresent.COLORBLINDMODE)mode;
        fullScreenSample.GetComponent<ColorBlindPresent>().SetMaterial(colorBlindMatGroup[mode]);
        fullScreenSample.GetComponentInChildren<Text>().text = "Color Blind : " + cOLORBLINDMODE.ToString();
    }

    public void SelectDaltonizationMode(int mode)
    {
        currentDaltonization = mode;
        ColorBlindPresent.DALTONIZATIONMODE dALTONIZATIONMODE = (ColorBlindPresent.DALTONIZATIONMODE)mode;
        fullScreenSample.GetComponent<ColorBlindPresent>().SetMaterial(daltonizationMatGroup[mode]);
        fullScreenSample.GetComponentInChildren<Text>().text = "Daltonization : " + dALTONIZATIONMODE.ToString();
    }

    public void SelectTexture(int texNum)
    {
        for (int i = 0; i < allColorBlindSmapleCompare.transform.childCount; i++)
        {
            allColorBlindSmapleCompare.transform.GetChild(i).GetComponent<ColorBlindPresent>().SetTexture(SampleTexGroup[texNum]);
        }
        for (int i = 0; i < allDaltonizationCompare.transform.childCount; i++)
        {
            allDaltonizationCompare.transform.GetChild(i).GetComponent<ColorBlindPresent>().SetTexture(SampleTexGroup[texNum]);
        }
        fullScreenSample.GetComponent<ColorBlindPresent>().SetTexture(SampleTexGroup[texNum]);
    }

    public void SelectTexture(Texture texture)
    {
        for (int i = 0; i < allColorBlindSmapleCompare.transform.childCount; i++)
        {
            allColorBlindSmapleCompare.transform.GetChild(i).GetComponent<ColorBlindPresent>().SetTexture(texture);
        }
        for (int i = 0; i < allDaltonizationCompare.transform.childCount; i++)
        {
            allDaltonizationCompare.transform.GetChild(i).GetComponent<ColorBlindPresent>().SetTexture(texture);
        }
        fullScreenSample.GetComponent<ColorBlindPresent>().SetTexture(texture);
    }

    public void SetDaltonization(int isdaltonization)
    {
        switch (isdaltonization)
        {
            case 0:
                isDaltonization = false;
                break;
            case 1:
                isDaltonization = true;
                break;
        }

        if (isDaltonization)
        {
            SelectDaltonizationMode(currentDaltonization);
            colorBlindnessSelection.SetActive(false);
            colorDaltonizationSelection.SetActive(true);
            
        }
        else
        {
            SelectColorBlindMode(currentColorBlind);
            colorDaltonizationSelection.SetActive(false);
            colorBlindnessSelection.SetActive(true);
        }

        ShowFullScreenSample();

    }
}
