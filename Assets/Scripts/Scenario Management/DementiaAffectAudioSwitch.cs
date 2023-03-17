using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DementiaAffectAudioSwitch : MonoBehaviour
{
    public static string DementiaAffectedAudioEnabledKey = "DementiaAffectedAudioEnabled";

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(DementiaAffectedAudioEnabledKey))
        {
            ScenarioManager.AphasiaAudio = PlayerPrefs.GetInt(DementiaAffectedAudioEnabledKey) > 0 ? true : false;
        }

        GetComponent<Interactable>().IsToggled = ScenarioManager.AphasiaAudio;

        TextMesh displayText = GetComponentInChildren<TextMesh>();
        if (displayText)
        {
            displayText.text = "Dementia Affected Audio: " + (ScenarioManager.AphasiaAudio ? "On" : "Off");
        }
    }

    public void UpdateValue()
    {
        PlayerPrefs.SetInt(DementiaAffectedAudioEnabledKey, ScenarioManager.AphasiaAudio ? 1 : 0);
        PlayerPrefs.Save();
    }
}
