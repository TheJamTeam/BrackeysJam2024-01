using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    public bool IsHeld;
    private Rigidbody _rigidbody;
    
    
    [Header("Holding Properties")]
    [Tooltip("How strong the gravity should be based on how close the target is. \n " +
             "Helps to combat jitter at close distances.")]
    public AnimationCurve gravityDistanceCurve;
    
    [Tooltip("How snappy the object should be to the cursor. High values can cause strange results.")]
    public float gravityStrength;
    
    [Tooltip("How quick the object should face the correct rotation.")]
    public float rotationSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public bool CanBePickedUp()
    {
        //TODO Implement
        return true;
    }

    public void ToggleIsHeld(bool toggle)
    {
        IsHeld = toggle;
        _rigidbody.useGravity = !toggle;
    }

    public void HoldUpdate(Transform holdOrigin, Rigidbody holdRigidbody)
    {
        Vector3 directionToOrigin = holdOrigin.position - transform.position;
        Vector3 relativeVelocity = holdRigidbody.GetComponent<PlayerMovementComponent>().Velocity - _rigidbody.velocity;

        Vector3 movementStep = directionToOrigin.normalized * ((gravityStrength * gravityDistanceCurve.Evaluate(directionToOrigin.magnitude)) * directionToOrigin.magnitude);
        
        _rigidbody.velocity += movementStep + relativeVelocity;
        

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, holdOrigin.rotation, rotationSpeed * Time.fixedDeltaTime);


    }
}
