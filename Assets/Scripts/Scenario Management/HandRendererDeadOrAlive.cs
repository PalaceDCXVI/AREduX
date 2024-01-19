using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRendererDeadOrAlive : MonoBehaviour
{
    public GameObject deadPanel;

    private void Start()
    {
        deadPanel = GameObject.FindObjectOfType<HandRendererDeathPanel>(true).gameObject;
    }

    private void OnDisable()
    {
        //deadPanel?.SetActive(true);
    }

    void OnDestroy()
    {
    }
}
