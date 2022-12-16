using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine.InputSystem;

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
            XRInputSubsystem xRInputSubsystem = XRSubsystemHelpers.InputSubsystem;
            bool? success = xRInputSubsystem?.TryRecenter();

            XRLoader loader = XRGeneralSettings.Instance.Manager.activeLoaders[0];
            XRGeneralSettings.Instance.Manager.activeLoaders[0].Initialize();
            XRGeneralSettings.Instance.Manager.activeLoaders[0].Start();
            UnityEngine.XR.XRInputSubsystem inputSubsystem = XRGeneralSettings.Instance.Manager.activeLoaders[0].GetLoadedSubsystem<UnityEngine.XR.XRInputSubsystem>();
            Debug.Log("Input feteched");
            if (inputSubsystem != null)
            {
                inputSubsystem.TryRecenter();
                Debug.Log("Recenter attempted");
            }
        }


        List<ISubsystem> list = new List<ISubsystem>();
        SubsystemManager.GetInstances(list);
        List<UnityEngine.XR.XRInputSubsystem> xRInputSubsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances<UnityEngine.XR.XRInputSubsystem>(xRInputSubsystems);
        if (xRInputSubsystems.Count > 0)
        {
            xRInputSubsystems[0].TryRecenter();
        }
    }
}
