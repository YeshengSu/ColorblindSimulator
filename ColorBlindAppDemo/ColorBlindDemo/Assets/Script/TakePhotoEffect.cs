using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TakePhotoEffect : MonoBehaviour {

    public float disappearTime = 1.0f;
    private float accumulatedTime = 0.0f;

    private RawImage rawImage;
    void Start()
    {
        rawImage = GetComponentInChildren<RawImage>();
    }

    void Update()
    {
        accumulatedTime += Time.deltaTime;
        Color color = rawImage.color;
        color.a = Mathf.Lerp(1.0f, 0.0f, accumulatedTime / disappearTime);
        rawImage.color = color;
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
