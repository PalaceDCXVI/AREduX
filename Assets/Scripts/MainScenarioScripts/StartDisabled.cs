using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDisabled : MonoBehaviour
{
    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
