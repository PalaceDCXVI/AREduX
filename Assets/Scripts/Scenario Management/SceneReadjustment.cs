using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class SceneReadjustment : MonoBehaviour
{
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
        if (XRGeneralSettings.Instance.Manager.activeLoaders.Count > 0)
        {
            UnityEngine.XR.XRInputSubsystem inputSubsystem = XRGeneralSettings.Instance.Manager.activeLoaders[0].GetLoadedSubsystem<UnityEngine.XR.XRInputSubsystem>();
            if (inputSubsystem != null)
            {
                inputSubsystem.TryRecenter();
            }
        }

        //List<ISubsystem> list = new List<ISubsystem>();
        //SubsystemManager.GetInstances(list);
        //List<UnityEngine.XR.XRInputSubsystem> xRInputSubsystems = new List<XRInputSubsystem>();
        //SubsystemManager.GetInstances<UnityEngine.XR.XRInputSubsystem>(xRInputSubsystems);
        //if (xRInputSubsystems.Count > 0)
        //{
        //    xRInputSubsystems[0].TryRecenter();
        //}
    }
}
