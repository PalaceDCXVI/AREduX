using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartOptionsToggle : MonoBehaviour
{

    public GameObject StartButtonsParent;
    public GameObject OptionsButtonsParent;

    // Start is called before the first frame update
    void Start()
    {
        if (OptionsButtonsParent.gameObject.activeSelf)
        {
            GetComponent<ButtonConfigHelper>().MainLabelText = "Start\r\n\r\n\r\nMenu";
        }
        else
        {
            GetComponent<ButtonConfigHelper>().MainLabelText = "Options\r\n\r\n\r\nMenu";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ToggleButtons()
    {
        StartButtonsParent.SetActive(!StartButtonsParent.gameObject.activeSelf);
        OptionsButtonsParent.SetActive(!OptionsButtonsParent.gameObject.activeSelf);

        if (OptionsButtonsParent.gameObject.activeSelf)
        {
            GetComponent<ButtonConfigHelper>().MainLabelText = "Start\r\n\r\n\r\nMenu";
        }
        else
        {
            GetComponent<ButtonConfigHelper>().MainLabelText = "Options\r\n\r\n\r\nMenu";
        }
    }
}
