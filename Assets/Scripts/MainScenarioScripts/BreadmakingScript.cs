using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadmakingScript : MonoBehaviour
{
    public List<GameObject> TomatoSlots;
    private int filledTomatoSlots = 0;
    private static int totalFilledTomatoSlots = 0;

    public GameObject Breadslot;
    private bool breadSlotInUse = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Foodstuffs"))
        {
            if (collision.gameObject.name.ToLower().Contains("tomato") && filledTomatoSlots < TomatoSlots.Count && collision.gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().IsBeingManipulated())
            {
                Destroy(collision.rigidbody);

                collision.transform.SetParent(TomatoSlots[filledTomatoSlots].transform, true);
                collision.transform.localPosition = Vector3.zero;
                collision.transform.localRotation = Quaternion.identity;

                collision.gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().ForceEndManipulation();
                collision.collider.enabled = false;

                filledTomatoSlots++;
                totalFilledTomatoSlots++;
            }
        }

        if (totalFilledTomatoSlots == TomatoSlots.Count && !breadSlotInUse && collision.gameObject.CompareTag("Breadtop") && collision.gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().IsBeingManipulated())
        {
            Destroy(collision.rigidbody);

            collision.transform.SetParent(Breadslot.transform, true);
            collision.transform.localPosition = Vector3.zero;
            collision.transform.localRotation = Quaternion.identity;

            collision.gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().ForceEndManipulation();

            breadSlotInUse = true;

            GetComponent<ScenarioTask>().CompleteTask();
        }
    }
}
