using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using System.Media;
using UnityEngine;

public class BreadmakingScript : MonoBehaviour
{
    public List<GameObject> TomatoSlots;
    private int filledTomatoSlots = 0;
    private static int totalFilledTomatoSlots = 0;

    public List<GameObject> TurkeySlots;
    private int filledTurkeySlots = 0;
    private static int totalFilledTurkeySlots = 0;

    public List<GameObject> PickleSlots;
    private int filledPickleSlots = 0;
    private static int totalFilledPickleSlots = 0;

    public GameObject Breadslot;
    public bool breadSlotInUse = false;

    public AudioSource soundFXPlayer;
    public AudioClip placementSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        
    }

    private void HandleFoodStuffsPlacement(Collision collision)
    {
        if (collision.gameObject.CompareTag("Foodstuffs"))
        {
            if (collision.gameObject.name.ToLower().Contains("tomato") && filledTomatoSlots < TomatoSlots.Count && collision.gameObject.GetComponent<ManipulationCheck>() && collision.gameObject.GetComponent<ManipulationCheck>().CanBeSlotted())
            {
                soundFXPlayer.PlayOneShot(placementSound);

                Destroy(collision.rigidbody);

                collision.gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().ForceEndManipulation();
                collision.gameObject.GetComponent<ObjectReset>().enabled = false;

                collision.transform.SetParent(TomatoSlots[filledTomatoSlots].transform, true);
                collision.transform.localPosition = Vector3.zero;
                collision.transform.localRotation = Quaternion.identity;
                collision.collider.enabled = false;

                foreach (var objectRenderer in collision.gameObject.GetComponentsInChildren<Renderer>())
                {
                    objectRenderer.GetComponent<ObjectReset>()?.ResetMaterialColour();
                }

                filledTomatoSlots++;
                totalFilledTomatoSlots++;

                collision.gameObject.GetComponent<ManipulationCheck>().canBeSlotted = false;

                if (totalFilledTomatoSlots > TomatoSlots.Count)
                {
                    totalFilledTomatoSlots = TomatoSlots.Count;
                }
            }

            if (collision.gameObject.name.ToLower().Contains("turkey") && filledTurkeySlots < TurkeySlots.Count && collision.gameObject.GetComponent<ManipulationCheck>() && collision.gameObject.GetComponent<ManipulationCheck>().CanBeSlotted())
            {
                soundFXPlayer.PlayOneShot(placementSound);

                Destroy(collision.rigidbody);

                collision.gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().ForceEndManipulation();
                collision.gameObject.GetComponent<ObjectReset>().enabled = false;

                collision.transform.SetParent(TurkeySlots[filledTurkeySlots].transform, true);
                collision.transform.localPosition = Vector3.zero;
                collision.transform.localRotation = Quaternion.identity;
                collision.collider.enabled = false;

                foreach (var objectRenderer in collision.gameObject.GetComponentsInChildren<Renderer>())
                {
                    objectRenderer.GetComponent<ObjectReset>()?.ResetMaterialColour();
                }

                collision.gameObject.GetComponent<ManipulationCheck>().canBeSlotted = false;

                filledTurkeySlots++;
                totalFilledTurkeySlots++;

                if (totalFilledTurkeySlots > TurkeySlots.Count)
                {
                    totalFilledTurkeySlots = TurkeySlots.Count;
                }
            }


            if (collision.gameObject.name.ToLower().Contains("pickle") && filledPickleSlots < PickleSlots.Count && collision.gameObject.GetComponent<ManipulationCheck>() && collision.gameObject.GetComponent<ManipulationCheck>().CanBeSlotted())
            {
                soundFXPlayer.PlayOneShot(placementSound);

                Destroy(collision.rigidbody);

                collision.gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().ForceEndManipulation();
                collision.gameObject.GetComponent<ObjectReset>().enabled = false;

                collision.transform.SetParent(PickleSlots[filledPickleSlots].transform, true);
                collision.transform.localPosition = Vector3.zero;
                collision.transform.localRotation = Quaternion.identity;
                collision.collider.enabled = false;

                foreach (var objectRenderer in collision.gameObject.GetComponentsInChildren<Renderer>())
                {
                    objectRenderer.GetComponent<ObjectReset>()?.ResetMaterialColour();
                }

                filledPickleSlots++;
                totalFilledPickleSlots++;

                collision.gameObject.GetComponent<ManipulationCheck>().canBeSlotted = false;

                if (totalFilledPickleSlots > PickleSlots.Count)
                {
                    totalFilledPickleSlots = PickleSlots.Count;
                }
            }
        }

        Debug.Log("Filled Tomato Slots: " + totalFilledTomatoSlots);
        Debug.Log("Filled Turkey Slots: " + totalFilledTurkeySlots);
        Debug.Log("Filled Pickle Slots: " + totalFilledPickleSlots);
        if (totalFilledTomatoSlots == TomatoSlots.Count && totalFilledTurkeySlots == TurkeySlots.Count && totalFilledPickleSlots == PickleSlots.Count &&
            !breadSlotInUse)
        {
            if (collision.gameObject.CompareTag("Breadtop") && gameObject.CompareTag("Breadslice")
                &&
                ((collision.gameObject.GetComponent<ManipulationCheck>() && collision.gameObject.GetComponent<ManipulationCheck>().CanBeSlotted()) || (gameObject.GetComponent<ManipulationCheck>() && gameObject.GetComponent<ManipulationCheck>().CanBeSlotted()))
                )
            {
                soundFXPlayer.PlayOneShot(placementSound);

                Destroy(collision.rigidbody);

                collision.gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().ForceEndManipulation();
                gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().ForceEndManipulation();

                if (gameObject.GetComponent<ManipulationCheck>().CanBeSlotted())
                {
                    Transform targetTransform = collision.transform.GetComponent<BreadmakingScript>().Breadslot.transform;
                    transform.position = targetTransform.position;
                }

                collision.transform.SetParent(Breadslot.transform, true);
                if (!gameObject.GetComponent<ManipulationCheck>().CanBeSlotted())
                {
                    collision.transform.localPosition = Vector3.zero;
                    collision.transform.localRotation = Quaternion.identity;
                }


                Vector3 fullCenter = new Vector3(0.00150859414f,-0.00650000013f,0.000175608337f);
                Vector3 fullSize = new Vector3(0.0910478532f,0.0215000007f,0.0930676311f);
                this.GetComponent<BoxCollider>().center = fullCenter;
                this.GetComponent<BoxCollider>().size = fullSize;
                collision.collider.enabled = false;

                foreach (var objectRenderer in collision.gameObject.GetComponentsInChildren<Renderer>())
                {
                    objectRenderer.GetComponent<ObjectReset>()?.ResetMaterialColour();
                }

                breadSlotInUse = true;
                collision.gameObject.GetComponent<BreadmakingScript>().breadSlotInUse = true;

                GetComponent<ScenarioTask>()?.CompleteTask();
                collision.gameObject.GetComponent<ScenarioTask>()?.CompleteTask();

                Debug.Log("Sandwich Made");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleFoodStuffsPlacement(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        HandleFoodStuffsPlacement(collision);
    }
}
