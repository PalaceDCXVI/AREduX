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
    private bool breadSlotInUse = false;

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

    private void OnCollisionEnter(Collision collision)
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

                filledTomatoSlots++;
                totalFilledTomatoSlots++;

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

                filledPickleSlots++;
                totalFilledPickleSlots++;

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
            !breadSlotInUse && collision.gameObject.CompareTag("Breadtop") && collision.gameObject.GetComponent<ManipulationCheck>() && collision.gameObject.GetComponent<ManipulationCheck>().CanBeSlotted())
        {
            soundFXPlayer.PlayOneShot(placementSound);

            Destroy(collision.rigidbody);

            collision.gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().ForceEndManipulation();
            gameObject.GetComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>().ForceEndManipulation();

            collision.transform.SetParent(Breadslot.transform, true);
            collision.transform.localPosition = Vector3.zero;
            collision.transform.localRotation = Quaternion.identity;
            collision.collider.enabled = false;

            this.gameObject.GetComponent<MeshCollider>().enabled = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = true;

            breadSlotInUse = true;

            GetComponent<ScenarioTask>().CompleteTask();
        }
    }
}
