using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class UtencilPlacement : MonoBehaviour
{
    private BoxCollider plane;

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

        CreateNewGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateNewGrid()
    {
        if (plane == null)
        {
            plane = GetComponent<BoxCollider>();
        }

        for (int i = 0; i < Dimensions; i++)
        {
            pointGrid.Add(new List<Vector2>());
            for (int j = 0; j < Dimensions; j++)
            {
                pointGrid[i].Add( new Vector2(Random.Range(i + minimumDistance, i + 1.0f - minimumDistance) * (1.0f / Dimensions),
                    Random.Range(j + minimumDistance, j + 1.0f - minimumDistance) * (1.0f / Dimensions)) );
            }
        }
    }

    public void RandomizeUtensilPlacement()
    {
        if (pointGrid.Count == 0)
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

            possibleIndices.RemoveAll(x => ((new Vector3(Mathf.Lerp(plane.bounds.min.x, plane.bounds.max.x, x.x),
                        item.transform.position.y,
                        Mathf.Lerp(plane.bounds.min.z, plane.bounds.max.z, x.y))) -
                    SpotToAvoid.position).magnitude < AreaOfSpot);

            if (possibleIndices.Count == 0)
            {
                Debug.LogError("All possible indices have been removed by avoided area.");
            }

            Vector2 newIndex = possibleIndices[Random.Range(0, possibleIndices.Count-1)];
            usedIndices.Add(newIndex);
            item.transform.SetPositionAndRotation(new Vector3(Mathf.Lerp(plane.bounds.min.x, plane.bounds.max.x, newIndex.x), item.transform.position.y, Mathf.Lerp(plane.bounds.min.z, plane.bounds.max.z, newIndex.y)), item.transform.rotation);
            foreach (ObjectReset reset in item.GetComponentsInChildren<ObjectReset>(true))
            {
                reset.GetComponent<ObjectReset>()?.UpdateResetPositionAndRotation();
            }
        }
    }
}
