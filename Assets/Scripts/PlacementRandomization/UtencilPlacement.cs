using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class UtencilPlacement : MonoBehaviour
{
    private BoxCollider plane;
    private Vector3 MinPlane = new Vector3();
    private Vector3 MaxPlane = new Vector3();

    private const int Dimensions = 5;

    private List<List<Vector2>> pointGrid = new List<List<Vector2>>();

    public float minimumDistance = 0.1f;

    public List<GameObject> Utensils;

    public Transform SpotToAvoid;
    public float AreaOfSpot = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        plane = GetComponent<BoxCollider>();

        //MinPlane = plane.center - (plane.size / 2.0f);
        //MinPlane += plane.transform.position;
        //MinPlane = MinPlane.RotateAround(plane.bounds.center, transform.rotation);
        //
        //MaxPlane = plane.center + (plane.size / 2.0f);
        //MaxPlane += plane.transform.position;
        //MaxPlane = MaxPlane.RotateAround(plane.bounds.center, transform.rotation);
        //Debug.DrawLine(MinPlane, MaxPlane, Color.magenta, 30.0f);

        CreateNewGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateNewGrid()
    {
        pointGrid.Clear();

        plane = GetComponent<BoxCollider>();

        Vector3 halfSize = plane.size / 2.0f;
        MinPlane = new Vector3(-halfSize.x, halfSize.y, -halfSize.z); //MinPlane = transform.rotation * MinPlane;   MinPlane += plane.transform.position;
        MaxPlane = new Vector3(halfSize.x, halfSize.y, halfSize.z);   //MaxPlane = transform.rotation * MaxPlane;   MaxPlane += plane.transform.position;

        //Debug.DrawLine(boundsMinPlane, boundsMaxPlane, Color.cyan, 30.0f);
        //Debug.DrawLine(boundsMinMaxPlane, boundsMaxMinPlane, Color.cyan, 30.0f);
        //Debug.DrawLine(SpotToAvoid.position, SpotToAvoid.position + Vector3.up, Color.cyan);

        //for (int i = 0; i < 8; i++)
        //{
        //    Vector3 endPosition = SpotToAvoid.position + ((Quaternion.AngleAxis((float)(i / 8.0f) * 360, Vector3.up) * transform.forward).normalized * AreaOfSpot);
        //    Debug.DrawLine(SpotToAvoid.position, endPosition, Color.cyan, 30.0f);
        //}

        for (int i = 0; i < Dimensions; i++)
        {
            pointGrid.Add(new List<Vector2>());
            for (int j = 0; j < Dimensions; j++)
            {
                pointGrid[i].Add( new Vector2(Random.Range(i + minimumDistance, i + 1.0f - minimumDistance) * (1.0f / Dimensions),
                    Random.Range(j + minimumDistance, j + 1.0f - minimumDistance) * (1.0f / Dimensions)) );
                Vector3 debugPosition = new Vector3(Mathf.Lerp(MinPlane.x, MaxPlane.x, pointGrid[i].Last().x),
                        plane.center.y + 0.026f,
                        Mathf.Lerp(MinPlane.z, MaxPlane.z, pointGrid[i].Last().y));
                debugPosition = transform.rotation * debugPosition;
                debugPosition += plane.transform.position;
                //debugPosition -= transform.position;
                //debugPosition = transform.rotation * debugPosition;
                //debugPosition += transform.position;
                //Vector3 debugPosition = new Vector3(Random.Range(i + minimumDistance, i + 1.0f - minimumDistance) * (1.0f / Dimensions), plane.bounds.center.y, Random.Range(j + minimumDistance, j + 1.0f - minimumDistance) * (1.0f / Dimensions));
                //Debug.DrawLine(debugPosition, debugPosition + Vector3.up, Color.magenta, 30.0f);
            }
        }
    }

    public void RandomizeUtensilPlacement()
    {
        //if (pointGrid.Count == 0)
        {
            CreateNewGrid();
        }

        List<Vector2> usedIndices = new List<Vector2>();
        foreach (GameObject item in Utensils)
        {
            List<Vector2> possibleIndices = new List<Vector2>();
            foreach (List<Vector2> vec in pointGrid)
            {
                possibleIndices.AddRange(vec.Except(usedIndices));
            }

            if (possibleIndices.Count == 0)
            {
                Debug.LogError("All possible indices have been used up.");
            }

            //possibleIndices.RemoveAll(x => ((new Vector3(Mathf.Lerp(plane.bounds.min.x, plane.bounds.max.x, x.x),
            //            item.transform.position.y,
            //            Mathf.Lerp(plane.bounds.min.z, plane.bounds.max.z, x.y))) -
            //        SpotToAvoid.position).magnitude < AreaOfSpot);
            possibleIndices.RemoveAll(x => (
            
            ((transform.rotation * new Vector3(Mathf.Lerp(MinPlane.x, MaxPlane.x, x.x),
                        plane.center.y + 0.026f,
                        Mathf.Lerp(MinPlane.z, MaxPlane.z, x.y))) + plane.transform.position) -
                    SpotToAvoid.position).magnitude < AreaOfSpot);

            if (possibleIndices.Count == 0)
            {
                Debug.LogError("All possible indices have been removed by avoided area.");
            }

            Vector2 newIndex = possibleIndices[Random.Range(0, possibleIndices.Count-1)];
            usedIndices.Add(newIndex);
            //item.transform.SetPositionAndRotation(new Vector3(Mathf.Lerp(plane.bounds.min.x, plane.bounds.max.x, newIndex.x), item.transform.position.y, Mathf.Lerp(plane.bounds.min.z, plane.bounds.max.z, newIndex.y)), item.transform.rotation);

            Vector3 itemPosition = new Vector3(Mathf.Lerp(MinPlane.x, MaxPlane.x, newIndex.x),
                    plane.center.y + 0.01f, 
                    //item.transform.position.y,
                    Mathf.Lerp(MinPlane.z, MaxPlane.z, newIndex.y));
            itemPosition = transform.rotation * itemPosition;
            itemPosition += plane.transform.position; 
            item.transform.SetPositionAndRotation(itemPosition, item.transform.rotation);
            foreach (ObjectReset reset in item.GetComponentsInChildren<ObjectReset>(true))
            {
                reset.GetComponent<ObjectReset>()?.UpdateResetPositionAndRotation();
            }
        }
    }
}
