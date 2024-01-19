using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovementTutorialTask : ScenarioTask
{
    override protected void Start()
    {
        base.Start();
        IsTutorialTask = true;
        ScenarioManager.Instance.InTutorial = true;
    }

    override protected void Update()
    {
        base.Update();
    }
}
