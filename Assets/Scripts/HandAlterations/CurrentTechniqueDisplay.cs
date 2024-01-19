using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentTechniqueDisplay : MonoBehaviour
{
    Text displayText = null;
    // Start is called before the first frame update
    void Start()
    {
        displayText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        displayText.text = "Current Technique:\n" + MaterialManager.Instance.highlightType.ToString();
    }
}
