using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAlphaBlink : MonoBehaviour
{
    public float StartingCountdown = 30.0f;

    public bool StartImmediately = false;

    private float countdown;


    public float minAlpha = 0.0f;
    public float maxAlpha = 0.08f;
    public float timeDistortion = 0.05f;
    public float alphaChangeDirection = 1.0f;
    private float alphaValue = -0.1f;

    // Start is called before the first frame update
    void Start()
    {
        if (StartImmediately)
        {
            countdown = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown <= 0.0f)
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
