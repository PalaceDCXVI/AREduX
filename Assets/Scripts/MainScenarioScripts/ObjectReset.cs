using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using VoxelEngine;

public class ObjectReset : MonoBehaviour
{
    public Transform ResetParent;
    public Transform ResetTransform;
    public bool UseLocalPosition = false;
    public Rigidbody ResetRigidbody;

    public bool ReAddRigidBody = false;
    public bool ResetColliderData = false;
    private Vector3 ResetColliderSize = new Vector3();
    private Vector3 ResetColliderCenter = new Vector3();
    public bool ColliderIsDefaultEnabled = true;

    public Vector3 startingPosition = new Vector3();
    public Quaternion startingRotation = new Quaternion();

    public Color OriginalColor;
    public bool IsDefualtEnabled;
    public bool IsMovableOnReset = true;

    public bool RendererEnabledByDefault = true;

    // Start is called before the first frame update
    void Start()
    {
        ResetParent = this.transform.parent;
        if (ResetColliderData)
        {
            ResetColliderSize = GetComponent<BoxCollider>().size;
            ResetColliderCenter = GetComponent<BoxCollider>().center;
        }

        if (UseLocalPosition)
        {
            startingPosition = this.transform.localPosition;
        }
        else 
        {
            startingPosition = this.transform.position;
        }

        startingRotation = this.transform.rotation;

        OriginalColor = GetComponentInChildren<Renderer>().material.color;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateResetPositionAndRotation()
    {
        if (UseLocalPosition)
        {
            //Do nothing here actually.
            //startingPosition = this.transform.localPosition;
        }
        else
        {
            startingPosition = this.transform.position;
            startingRotation = this.transform.rotation;
        }
    }
    
    public void ResetMaterialColour()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer)
        {
            renderer.material.color = OriginalColor;
        }
    }

    public void ResetObject(bool overrideIsActive = false)
    {
        if (transform.parent != ResetParent)
        {
            transform.SetParent(ResetParent);
        }

        if (ResetTransform == null)
        {
            if (UseLocalPosition)
            {
                this.transform.localPosition = startingPosition;
                this.transform.rotation = startingRotation;
            }
            else
            {
                this.transform.SetPositionAndRotation(startingPosition, startingRotation);
            }
        }
        else 
        {
            if (UseLocalPosition)
            {
                this.transform.localPosition = ResetTransform.localPosition;
                this.transform.rotation = ResetTransform.rotation;
            }
            else
            {
                this.transform.SetPositionAndRotation(ResetTransform.position, ResetTransform.rotation);
            }
        }

        GetComponent<ObjectManipulator>()?.ForceEndManipulation();

        if (ReAddRigidBody && !GetComponent<Rigidbody>())
        {
            this.gameObject.AddComponent<Rigidbody>();
        }

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
        }

        if (ResetColliderData)
        {
            GetComponent<BoxCollider>().center = ResetColliderCenter;
            GetComponent<BoxCollider>().size = ResetColliderSize;
        }

        if (GetComponent<Collider>())
        {
            GetComponent<Collider>().enabled = ColliderIsDefaultEnabled;
        }

        if (GetComponent<ObjectManipulator>() && GetComponent<ObjectManipulator>().enabled == false && IsMovableOnReset == true)
        {
            GetComponent<ObjectManipulator>().enabled = true;
        }

        if (GetComponent<Renderer>())
        {
            GetComponent<Renderer>().enabled = RendererEnabledByDefault;
        }

        this.gameObject.SetActive(overrideIsActive ? true : IsDefualtEnabled);
    }

    public void ResetRigidBody()
    {
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
