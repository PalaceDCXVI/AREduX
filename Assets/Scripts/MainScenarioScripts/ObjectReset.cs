using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using VoxelEngine;

public class ObjectReset : MonoBehaviour
{
    public Transform ResetParent;
    public Transform ResetTransform;
    private Vector3 ResetTransformWorldPosition = new Vector3();
    private Vector3 ResetTransformLocalPosition = new Vector3();
    private Quaternion ResetTransformRotation = Quaternion.identity;
    public bool UseLocalPosition = false;
    public Rigidbody ResetRigidbody;

    public bool ReAddRigidBody = false;
    public bool ResetColliderData = false;
    private Vector3 ResetColliderSize = new Vector3();
    private Vector3 ResetColliderCenter = new Vector3();
    public bool ColliderIsDefaultEnabled = true;

    private Vector3 startingPosition = new Vector3();
    private Quaternion startingRotation = new Quaternion();

    public Color OriginalColor;
    public bool IsDefualtEnabled;
    public bool IsMovableOnReset = true;

    // Start is called before the first frame update
    void Start()
    {
        if (ResetTransform)
        {
            ResetTransformWorldPosition = ResetTransform.position;
            ResetTransformLocalPosition = ResetTransform.localPosition;
            ResetTransformRotation = ResetTransform.rotation;
        }

        ResetParent = this.transform.parent;
        if (ResetColliderData)
        {
            ResetColliderSize = GetComponent<BoxCollider>().size;
            ResetColliderCenter = GetComponent<BoxCollider>().center;
        }

        startingPosition = this.transform.position;
        startingRotation = this.transform.rotation;

        OriginalColor = GetComponentInChildren<Renderer>().material.color;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateResetPositionAndRotation()
    {
        startingPosition = this.transform.position;
        startingRotation = this.transform.rotation;
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
            this.transform.SetPositionAndRotation(startingPosition, startingRotation);
        }
        else 
        {
            if (UseLocalPosition)
            {
                this.transform.localPosition = ResetTransformLocalPosition;
                this.transform.rotation = ResetTransformRotation;
            }
            else
            {
                this.transform.SetPositionAndRotation(ResetTransformWorldPosition, ResetTransformRotation);
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

        this.gameObject.SetActive(overrideIsActive ? true : IsDefualtEnabled);
    }
}
