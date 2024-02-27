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
        if (other.gameObject.CompareTag("KnifeHead") && cutTask != null && cutTask.ForkIsInPlace)
        {
            this.gameObject.SetActive(false);

            cutTask.IncrementNumberOfCutCubes();
        }

        if (other.gameObject.CompareTag("KnifeHead") && familiarizationTask != null && familiarizationTask.ForkIsInPlace)
        {
            this.gameObject.SetActive(false);

            familiarizationTask.IncrementCutSlots();
        }
    }
}
