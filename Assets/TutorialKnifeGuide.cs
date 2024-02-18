using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKnifeGuide : MonoBehaviour
{
    public Animator knifeAnimator;

    public bool CubesCut = false;

    // Start is called before the first frame update
    void Start()
    {
        knifeAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Angle(transform.parent.transform.up, Vector3.up) > 90)
        {
            knifeAnimator.SetBool("UpsideDown", false);
        }
        else
        {
            knifeAnimator.SetBool("UpsideDown", true);
        }
    }

    public void KnifePickedUp()
    {
        this.gameObject.SetActive(false);
    }

    public void KnifeDropped()
    {
        if (!CubesCut)
        {
            this.gameObject.SetActive(true);
        }
    }
}
