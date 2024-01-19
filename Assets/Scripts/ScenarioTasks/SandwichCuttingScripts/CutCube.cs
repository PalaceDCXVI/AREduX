using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutCube : MonoBehaviour
{
    private CutSandwichTask cutTask;
    // Start is called before the first frame update
    void Start()
    {
        cutTask = GetComponentInParent<CutSandwichTask>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("KnifeHead") && cutTask.ForkIsInPlace)
        {
            this.gameObject.SetActive(false);

            cutTask.IncrementNumberOfCutCubes();
        }
    }
}
