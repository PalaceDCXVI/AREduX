using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateGrabbableManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveNearInteractionGrabbable()
    {
        Destroy(GetComponent<NearInteractionGrabbable>());
    }

    public void AddNearInteractionGrabbable()
    {
        gameObject.AddComponent<NearInteractionGrabbable>();
    }
}
