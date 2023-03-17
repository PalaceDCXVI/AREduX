using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisSim;

public class BlurStrengthSlider : MonoBehaviour
{
    public static string BlurStrengthKey = "BlurStrength";

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(BlurStrengthKey))
        {
            GetComponent<PinchSlider>().SliderValue = PlayerPrefs.GetFloat(BlurStrengthKey);
            Camera.main.GetComponent<myInpainter2>().UpdateEffectStrength(PlayerPrefs.GetFloat(BlurStrengthKey));
        }
    }

    public void UpdateValue()
    {
        PlayerPrefs.SetFloat(BlurStrengthKey, GetComponent<PinchSlider>().SliderValue);
        PlayerPrefs.Save();
    }
}
