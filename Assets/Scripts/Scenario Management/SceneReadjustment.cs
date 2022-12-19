using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine.InputSystem;

public class SceneReadjustment : MonoBehaviour
{
    public GameObject SceneRoot;
 
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecenterScene()
    {
        Vector3 repositionVector = Camera.main.transform.position - SceneRoot.transform.position;
        repositionVector.x = 0; repositionVector.z = 0;
        SceneRoot.transform.Translate(repositionVector);
        Camera.main.transform.Translate(-repositionVector);
    }
}
