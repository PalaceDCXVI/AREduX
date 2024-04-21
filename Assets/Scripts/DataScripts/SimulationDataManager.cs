using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using static MaterialManager;

public class SimulationDataManager : MonoBehaviour
{

    public static SimulationDataManager Instance;

    [Serializable]
    public class SimulationData
    {
        public MaterialManager.HighlightType highlightType;
        public string HighlightTypeName = "";
        public double ScenarioCompletionTime = 0.0f;
        public double ObjectPlacementCompletionTime = 0.0f;
        public double ToolUsageCompletionTime = 0.0f;
        public double AccuracyRate = 0.0f;
        public double GraspAccuracyRate = 0.0f;
        public int TotalGraspAttempts = 0;
        public int TotalCompletedGrasps = 0;
        public double TotalGraspDistance = 0.0;
        public int IncorrectPlacements = 0;
    }

    public List<SimulationData> Simulations;


    public bool CurrentlyInSimulation = false;
    public bool InObjectsPlacementSection = false;

    public double CurrentSimulationTime = 0.0f;
    public double CurrentObjectPlacementTime = 0.0f;
    public double CurrentToolUsageTime = 0.0f;
    public int CurrentGraspAttempts = 0;
    public int CurrentCompletedGrasps = 0;
    public double CurrentTotalGraspDistance = 0.0;
    public double CurrentAccuracyRate = 0.0;
    public double CurrentGraspAccuracyRate = 0.0;
    public int CurrentIncorrectPlacements = 0;

    public static string LatinTableIndexKey = "LatinTableIndex";
    private int LatinTableIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            if (PlayerPrefs.HasKey(LatinTableIndexKey))
            {
                LatinTableIndex = PlayerPrefs.GetInt(LatinTableIndexKey);
            }
        }
        else
        {
            Debug.LogError("SimulationDataManager already exists.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentlyInSimulation)
        {
            CurrentSimulationTime += Time.deltaTime;
            if (InObjectsPlacementSection)
            {
                CurrentObjectPlacementTime += Time.deltaTime;
            }
            else
            {
                CurrentToolUsageTime += Time.deltaTime;
            }
        }
    }

    public void SaveData()
    {
        string json = "";
        foreach (SimulationData simData in Simulations)
        {
            json += JsonUtility.ToJson(simData) + "\n";
        }

        string path = string.Format("{0}/{1}.json", Application.persistentDataPath, System.DateTime.Now.DayOfWeek.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString());
        
        byte[] data = System.Text.Encoding.ASCII.GetBytes(json);
        
        UnityEngine.Windows.File.WriteAllBytes(path, data);
    }

    public void CheckScenarioCompletion()
    {
        if (Simulations.Count >= MaterialManager.Instance.OrderOfHighlightTypes.Count) //(int)MaterialManager.HighlightType.None)
        {
            ScenarioManager.Instance.ExternalScenarioCompletionCheck = true;
        }
    }

    public void SetScenarioData()
    {
        SimulationData simulation = new SimulationData();

        Simulations.Add(simulation);
    }

    public void FinalizeScenarioData()
    {
        if (!ScenarioManager.Instance.InTutorial)
        {
            SimulationData simulation = new SimulationData();
            simulation.highlightType = MaterialManager.Instance.highlightType;
            simulation.HighlightTypeName = Enum.GetName(typeof(MaterialManager.HighlightType), simulation.highlightType);
            SetScenarioTime(simulation);
            SetAccuracyRate(simulation);
            SetGraspAccuracyRate(simulation);
            simulation.TotalGraspAttempts = CurrentGraspAttempts;
            simulation.TotalCompletedGrasps = CurrentCompletedGrasps;
            simulation.TotalGraspDistance = CurrentTotalGraspDistance;
            simulation.IncorrectPlacements = CurrentIncorrectPlacements;

            Simulations.Add(simulation);

            PlayerPrefs.SetInt(LatinTableIndexKey, LatinTableIndex+1);
            PlayerPrefs.Save();
        }

        if (!CurrentlyInSimulation)
        {
            CurrentSimulationTime = 0;
            CurrentObjectPlacementTime = 0;
            CurrentToolUsageTime = 0;
            CurrentGraspAttempts = 0;
            CurrentCompletedGrasps = 0;
            CurrentTotalGraspDistance = 0.0;
            CurrentAccuracyRate = 0.0;
            CurrentGraspAccuracyRate = 0.0;
            CurrentIncorrectPlacements = 0;
        }
    }

    public void SetScenarioTime(SimulationData simulation)
    {
        simulation.ScenarioCompletionTime = CurrentSimulationTime;
        simulation.ObjectPlacementCompletionTime = CurrentObjectPlacementTime;
        simulation.ToolUsageCompletionTime = CurrentToolUsageTime;
    }

    public void SetAccuracyRate(SimulationData simulation)
    {
        //CurrentAccuracyRate = ((sphereDistance * CurrentCompletedGrasps) - CurrentTotalGraspDistance) / (sphereDistance * CurrentCompletedGrasps);
        simulation.AccuracyRate = CurrentAccuracyRate;
    }

    public void SetGraspAccuracyRate(SimulationData simulation)
    {
        //CurrentErrorRate = (CurrentGraspAttempts / CurrentCompletedGrasps);
        simulation.GraspAccuracyRate = CurrentGraspAccuracyRate;
    }

    public void SetCurrentlyInSimulation(bool isCurrentlyInSimulation)
    {
        CurrentlyInSimulation = isCurrentlyInSimulation;
    }

    public void SetInObjectsPlacementSection(bool currentlyInObjectsPlacementSection)
    {
        InObjectsPlacementSection = currentlyInObjectsPlacementSection;
    }

    public void AddGraspAttempt()
    {
        if (CurrentlyInSimulation)
        {
            CurrentGraspAttempts += 1;
        }
    }

    public void AddGrasp(double distance, double sphereDistance)
    {
        if (CurrentlyInSimulation)
        {
            CurrentCompletedGrasps += 1;
            CurrentTotalGraspDistance += distance;
            //CurrentAccuracyRate // a = (R−d)/R, 
            //CurrentAccuracyRate = (sphereDistance - distance)/sphereDistance
            CurrentAccuracyRate = ((sphereDistance * (double)CurrentCompletedGrasps) - CurrentTotalGraspDistance) / (sphereDistance * (double)CurrentCompletedGrasps);
            CurrentGraspAccuracyRate = ((double)CurrentCompletedGrasps / (double)CurrentGraspAttempts);
            //CurrentErrorRate = (CurrentErrorRate == 0 ? 1 : CurrentErrorRate);
            //where R is the radius of the collision sphere and d is the Euclidean distance between the sphere and the nearest object that can be grabbed.
        }
    }

    public void AddIncorrectPlacement()
    {
        if (CurrentlyInSimulation)
        {
            CurrentIncorrectPlacements += 1;
        }
    }

}
