using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInteractionComponent : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [Tooltip("The object the player is currently holding. Can only be holding 1 object at a time.")][ReadOnly]
    public InteractComponent currentlyHoldingObject = null;
    
    [Tooltip("The object the player is currently looking at")][ReadOnly]
    public InteractComponent currentFocus = null;
    
    [Header("Holding Properties")]
    [Tooltip("The Transform held objects will stick to. \nAutomatically position, update offset with 'Hold Offset' variable")]
    public Transform holdTransform;
    
    [Tooltip("The distance (in units) that held items gravitate towards.")]
    public Vector3 holdOffset;
    
    [Tooltip("How strong the gravity should be based on how close the target is. \n " +
             "Helps to combat jitter at close distances.")]
    public AnimationCurve gravityDistanceCurve;
    
    [Tooltip("How snappy the object should be to the cursor. High values can cause strange results.")]
    public float gravityStrength = 10f;
    
    [Tooltip("How quick the object should face the correct rotation.")]
    public float rotationSpeed = 5f;
    
    [Header("Focusing Properties")]
    [Tooltip("The radius around the center of the camera that will be checked for focus targets.")]
    public float focusRadius;
    
    [Tooltip("The distance (in units) focus targets must be within.")]
    public float focusRange;
    
    
    void OnEnable()
    {
        InputManager.OnPrimaryUpdated += OnPrimaryAction;
    }

    void OnDisable()
    {
        InputManager.OnPrimaryUpdated -= OnPrimaryAction;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void OnPrimaryAction()
    {
        Interact();
    }

    void Update()
    {
        CalculateCurrentFocus();
    }
    
    //Physics Calculations go in Fixed Update
    void FixedUpdate()
    {
        if (currentlyHoldingObject && currentlyHoldingObject.IsHeld)
        {
            if (currentlyHoldingObject.IsHeld)
            {
                currentlyHoldingObject.HoldUpdate(holdTransform, _rigidbody, gravityStrength, gravityDistanceCurve, rotationSpeed);
            }
            else
            {
                currentlyHoldingObject = null;
            }
        }
        
    }

    //Raycasts from the origin and checks to see if any objects that can interacted with. Then chooses the primary target. Will always choose closest/First
    void CalculateCurrentFocus()
    {
        Vector3 originPosition = holdTransform.transform.position;
        Vector3 endPosition = originPosition + holdTransform.transform.forward * focusRange;
        RaycastHit[] hits = Physics.CapsuleCastAll(originPosition, endPosition, focusRadius, holdTransform.forward, focusRange);
        InteractComponent bestFocus = null;
        
        foreach (RaycastHit hit in hits)
        {
            InteractComponent interactComponent = null;
            interactComponent = hit.transform.GetComponent<InteractComponent>();
            if (interactComponent is not null)
            {
                if (interactComponent != currentlyHoldingObject)
                {
                    //Just focus the first valid target.
                    //TODO Rework for if the interactable can be focused.
                    bestFocus = interactComponent;
                    break;
                }
            }
        }

        currentFocus = bestFocus;
    }

    //Either picks up the object, drops the object, or uses it on another interact-object.
    void Interact()
    {
        if (currentlyHoldingObject is not null)
        {
            //TODO Use object on valid Focus
            
            //OR drop the item
            currentlyHoldingObject.ToggleIsHeld(false);
            currentlyHoldingObject = null;
        }
        else if(currentFocus)
        {
            //Interact with the object.
            if (currentFocus.CanBePickedUp())
            {
                currentlyHoldingObject = currentFocus;
                currentlyHoldingObject.ToggleIsHeld(true);
            }
        }
    }

    protected void OnValidate()
    {
        holdTransform.localPosition = holdOffset;
    }
}
