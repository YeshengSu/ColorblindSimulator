using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBlindPresent : MonoBehaviour {
    public enum COLORBLINDMODE
    {
        Normal                  = 0,
        Protanomaly             = 1,
        Deuteranomaly           = 2,
        Tritanomaly             = 3,
        Protanope               = 4,
	    Deuteranope             = 5,
	    Tritanope               = 6, 
	    RedConeMonochromats     = 7,
	    GreenConeMonochromats   = 8,
	    BlueConeMonochromats    = 9,
	    RodMonochromats         = 10,
    }
    public enum DALTONIZATIONMODE
    {
        Normal = 0,
        ProtanopeDaltonization = 1,
        DeuteranopeDaltonization = 2,
        TritanopeDaltonization = 3,
    }

    // Use this for initialization
    void Start () {

    }

    public void SetTexture(Texture tex)
    {
        gameObject.GetComponent<RawImage>().texture = tex;
    }

    public void SetMaterial(Material mat)
    {
        gameObject.GetComponent<RawImage>().material = mat;
    }
}
