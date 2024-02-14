using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    //The 
    public string InteractID;
    [ReadOnly]
    public bool IsHeld = false;
    public SocketComponent IsSocketedBy;
    private Rigidbody _rigidbody;
    
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

    public void AddToSocket(SocketComponent socket)
    {
        ToggleIsHeld(false);
        IsSocketedBy = socket;
        _rigidbody.useGravity = false;
    }

    public void HoldUpdate(Transform holdOrigin, Rigidbody holdRigidbody, float gravityStrength, AnimationCurve gravityDistanceCurve, float rotationSpeed)
    {
        Vector3 directionToOrigin = holdOrigin.position - transform.position;
        Vector3 relativeVelocity = holdRigidbody.GetComponent<PlayerMovementComponent>().Velocity - _rigidbody.velocity;

        Vector3 movementStep = directionToOrigin.normalized * ((gravityStrength * gravityDistanceCurve.Evaluate(directionToOrigin.magnitude)) * directionToOrigin.magnitude);
        
        _rigidbody.velocity += movementStep + relativeVelocity;
        

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, holdOrigin.rotation, rotationSpeed * Time.fixedDeltaTime);
    }
}
