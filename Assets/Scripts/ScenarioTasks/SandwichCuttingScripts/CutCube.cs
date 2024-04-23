using Microsoft.MixedReality.Toolkit.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutCube : MonoBehaviour
{
    private CutSandwichTask cutTask;
    private FamiliarizationScript familiarizationTask;

    // Start is called before the first frame update
    void Start()
    {
        cutTask = GetComponentInParent<CutSandwichTask>();
        familiarizationTask = GetComponentInParent<FamiliarizationScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("KnifeHead") && cutTask != null)
        {
            if (cutTask.ForkIsInPlace)
            {
                this.gameObject.SetActive(false);

                other.gameObject.GetComponentInParent<ToolMaterialManipulator>()?.SetHandHighlightOff();

                cutTask.IncrementNumberOfCutCubes();
            }
            else
            {
                other.gameObject.GetComponentInParent<ToolMaterialManipulator>()?.SetHandHighlightOn(this.GetComponent<Collider>());
            }
        }

        if (other.gameObject.CompareTag("KnifeHead") && familiarizationTask != null)
        {
            if (familiarizationTask.ForkIsInPlace)
            {
                this.gameObject.SetActive(false);

                other.gameObject.GetComponentInParent<ToolMaterialManipulator>()?.SetHandHighlightOff();

                familiarizationTask.IncrementCutSlots();
            }
            else
            {
                other.gameObject.GetComponentInParent<ToolMaterialManipulator>()?.SetHandHighlightOn(this.GetComponent<Collider>());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("KnifeHead") && cutTask != null)
        {
            if (!cutTask.ForkIsInPlace)
            {
                other.gameObject.GetComponentInParent<ToolMaterialManipulator>()?.IncrementContactCounter();
            }
        }

        if (other.gameObject.CompareTag("KnifeHead") && familiarizationTask != null)
        {
            if (!familiarizationTask.ForkIsInPlace)
            {
                other.gameObject.GetComponentInParent<ToolMaterialManipulator>()?.IncrementContactCounter();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("KnifeHead") && cutTask != null)
        {
            other.gameObject.GetComponentInParent<ToolMaterialManipulator>()?.SetHandHighlightOff();
        }

        if (other.gameObject.CompareTag("KnifeHead") && familiarizationTask != null)
        {
            other.gameObject.GetComponentInParent<ToolMaterialManipulator>()?.SetHandHighlightOff();
        }
    }
}
