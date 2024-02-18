using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSandwichTask : ScenarioTask
{
    public List<GameObject> CutCubes = new List<GameObject>();

    public int totalNumberOfCutCubes = 4;
    public int cutCubes = 0;

    public bool ForkIsInPlace = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        totalNumberOfCutCubes = CutCubes.Count;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void ResetScenario()
    {
        base.ResetScenario();
        GetComponentInChildren<TutorialKnifeGuide>(true).CubesCut = false;
        GetComponentInChildren<ForkCube>(true).ForkHighlight.SetActive(true);
        cutCubes = 0;
    }

    public void IncrementNumberOfCutCubes()
    {
        cutCubes++;

        if (totalNumberOfCutCubes == cutCubes) 
        {
            GetComponentInChildren<TutorialKnifeGuide>(true).CubesCut = true;
            CompleteTask();
        }
    }
}
