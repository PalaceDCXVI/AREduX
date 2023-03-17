using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisSim;

public class BlurSizeSlider : MonoBehaviour
{
    public static string BlurSizeKey = "BlurSize";

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(BlurSizeKey))
        {
            GetComponent<PinchSlider>().SliderValue = PlayerPrefs.GetFloat(BlurSizeKey);
            Camera.main.GetComponent<myInpainter2>().UpdateThreshold(PlayerPrefs.GetFloat(BlurSizeKey));
        }
    }

    public void UpdateValue()
    {
        PlayerPrefs.SetFloat(BlurSizeKey, GetComponent<PinchSlider>().SliderValue);
        PlayerPrefs.Save();
    }
}
