using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisSim;

public class ToggleVisuals : MonoBehaviour
{
    public bool VisualsActive = false;

    // Start is called before the first frame update
    void Start()
    {
        //We reverse this here and again at the start of the function to preserve the boolean's readability in editor.
        VisualsActive = !VisualsActive;

        ToggleVisualEffects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateInPainterEffectVariables()
    {
        if (PlayerPrefs.HasKey(BlurSizeSlider.BlurSizeKey))
        {
            Camera.main.GetComponent<myInpainter2>().UpdateThreshold(PlayerPrefs.GetFloat(BlurSizeSlider.BlurSizeKey));
        }

        if (PlayerPrefs.HasKey(BlurStrengthSlider.BlurStrengthKey))
        {
            Camera.main.GetComponent<myInpainter2>().UpdateEffectStrength(PlayerPrefs.GetFloat(BlurStrengthSlider.BlurStrengthKey));
        }
    }

    public void SetVisualEffects(bool active)
    {
        VisualsActive = active;
        UpdateInPainterEffectVariables();
        Camera.main.GetComponent<myInpainter2>().enabled = VisualsActive;
        Camera.main.GetComponent<myFloaters>().enabled = VisualsActive;
        Camera.main.GetComponent<myWiggle>().enabled = VisualsActive;
    }

    public void ToggleVisualEffects()
    {
        VisualsActive = !VisualsActive;

        UpdateInPainterEffectVariables();
        Camera.main.GetComponent<myInpainter2>().enabled = VisualsActive;
        //Camera.main.GetComponent<myFieldLoss>().enabled = VisualsActive;
        //Camera.main.GetComponent<myBrightnessContrastGamma>().enabled = VisualsActive;
        //Camera.main.GetComponent<myDistortionMap>().enabled = VisualsActive;
        //Camera.main.GetComponent<myRecolour>().enabled = VisualsActive;
        //Camera.main.GetComponent<myNystagmus>().enabled = VisualsActive;
        //Camera.main.GetComponent<myDoubleVision>().enabled = VisualsActive;
        //Camera.main.GetComponent<myNoise>().enabled = VisualsActive;
        Camera.main.GetComponent<myFloaters>().enabled = VisualsActive;
        //Camera.main.GetComponent<myTeichopsia>().enabled = VisualsActive;
        Camera.main.GetComponent<myWiggle>().enabled = VisualsActive;
        //Camera.main.GetComponent<myCataract>().enabled = VisualsActive;
    }
}
