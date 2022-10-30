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

    public void ToggleVisualEffects()
    {
        VisualsActive = !VisualsActive;

        Camera.main.GetComponent<myInpainter2>().enabled = VisualsActive;
        Camera.main.GetComponent<myFieldLoss>().enabled = VisualsActive;
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
