using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementCube : MonoBehaviour
{
    public float StartingCountdown = 30.0f;

    public bool StartImmediately = false;

    private float countdown;

    bool CountingDown = false;


    public float minAlpha = 0.0f;
    public float maxAlpha = 0.08f;
    public float timeDistortion = 0.05f;
    public float alphaChangeDirection = 1.0f;
    private float alphaValue = -0.1f;

    // Start is called before the first frame update
    void Start()
    {
        countdown = StartingCountdown;

        Color currentColour = GetComponent<MeshRenderer>().material.color;
        currentColour.a = minAlpha;

        GetComponent<MeshRenderer>().material.color = currentColour;

        if (StartImmediately)
        {
            StartCountdown();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CountingDown)
        {
            countdown -= Time.deltaTime;

            if (countdown < 0.0f)
            {
                if (alphaValue < minAlpha)
                {
                    alphaChangeDirection = 1.0f;
                }
                else if (alphaValue > maxAlpha)
                {
                    alphaChangeDirection = -1.0f;
                }

                float alphaChange = Time.deltaTime * timeDistortion * alphaChangeDirection;

                alphaValue += alphaChange;

                Color currentColour = GetComponent<MeshRenderer>().material.color;
                currentColour.a = Mathf.Clamp01(alphaValue);

                GetComponent<MeshRenderer>().material.color = currentColour;
            }
        }
    }

    public void StartCountdown()
    {
        CountingDown = true;
    }

    public void RestartCountdown()
    {
        if (StartImmediately)
        {
            return;
        }
        CountingDown = false;

        //countdown = StartingCountdown;
        alphaChangeDirection = 1.0f;
        alphaValue = -0.1f;        
        Color currentColour = GetComponent<MeshRenderer>().material.color;
        currentColour.a = minAlpha;

        GetComponent<MeshRenderer>().material.color = currentColour;
    }
}
