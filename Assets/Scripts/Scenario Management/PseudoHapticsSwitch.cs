using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PseudoHapticsSwitch : MonoBehaviour
{
    public static string PseudoHapticsEnabledKey = "PseudoHapticsEnabled";

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(PseudoHapticsEnabledKey))
        {
            HandMaterialManager.PseudoHapticsActive = PlayerPrefs.GetInt(PseudoHapticsEnabledKey) > 0 ? true : false;
        }

        GetComponent<Interactable>().IsToggled = HandMaterialManager.PseudoHapticsActive;

        TextMesh displayText = GetComponentInChildren<TextMesh>();
        if (displayText)
        {
            displayText.text = "Pseudo Haptics: " + (HandMaterialManager.PseudoHapticsActive ? "On" : "Off");
        }
    }

    public void UpdateValue()
    {
        PlayerPrefs.SetInt(PseudoHapticsEnabledKey, HandMaterialManager.PseudoHapticsActive ? 1 : 0);
        PlayerPrefs.Save();
    }
}
