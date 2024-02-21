using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkCube : MonoBehaviour
{
    public CutSandwichTask cutTask;

    public GameObject ForkHighlight;
    public GameObject ForkHighlightOther;
    public GameObject ForkPosition1;
    public GameObject ForkPosition2;

    public bool ForkIsBeingManipulated = false;

    // Start is called before the first frame update
    void Start()
    {
        cutTask = GetComponentInParent<CutSandwichTask>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Angle(transform.parent.transform.up, Vector3.up) > 90)
        {
            ForkHighlight.transform.SetPositionAndRotation(ForkPosition1.transform.position, ForkPosition1.transform.rotation);
        }
        else
        {
            ForkHighlight.transform.SetPositionAndRotation(ForkPosition2.transform.position, ForkPosition2.transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ForkHead"))
        {
            switch (MaterialManager.Instance.highlightType)
            {
                case MaterialManager.HighlightType.ObjectHighlight:
                    this.gameObject.GetComponent<Renderer>().material.color = MaterialManager.Instance.ObjectHighlightColor;
                    break;
                case MaterialManager.HighlightType.HandHighlight:
                case MaterialManager.HighlightType.SphericalCursor:
                case MaterialManager.HighlightType.DotCursor:
                case MaterialManager.HighlightType.None:
                    this.gameObject.GetComponent<Renderer>().enabled = false;
                    break;
                default:
                    break;
            }

            ForkHighlightOther.GetComponent<Renderer>().enabled = false;

            //Color originalColor = this.gameObject.GetComponent<Renderer>().material.color;
            //this.gameObject.GetComponent<Renderer>().material.color = new UnityEngine.Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

            cutTask.ForkIsInPlace = true;
            //ForkHighlight.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ForkHead"))
        {
            switch (MaterialManager.Instance.highlightType)
            {
                case MaterialManager.HighlightType.ObjectHighlight:
                    this.gameObject.GetComponent<ObjectReset>()?.ResetMaterialColour();
                    break;
                case MaterialManager.HighlightType.HandHighlight:
                case MaterialManager.HighlightType.SphericalCursor:
                case MaterialManager.HighlightType.DotCursor:
                case MaterialManager.HighlightType.None:
                    this.gameObject.GetComponent<Renderer>().enabled = true;
                    break;
                default:
                    break;
            }

            ForkHighlightOther.GetComponent<Renderer>().enabled = true;

            //Color originalColor = this.gameObject.GetComponent<Renderer>().material.color;
            //this.gameObject.GetComponent<Renderer>().material.color = new UnityEngine.Color(originalColor.r, originalColor.g, originalColor.b, 1.0f);

            cutTask.ForkIsInPlace = false;
            
            //ForkHighlight.SetActive(true);
        }
    }

    public void ForkPickedUp()
    {
        ForkHighlight.SetActive(false);
        ForkIsBeingManipulated = true;
    }

    public void ForkDropped()
    {
        if (cutTask && !cutTask.ForkIsInPlace)
        {
            ForkHighlight.SetActive(true);
        }

        ForkIsBeingManipulated = false;
    }
}
