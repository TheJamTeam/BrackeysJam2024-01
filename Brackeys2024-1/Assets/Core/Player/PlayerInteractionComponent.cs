using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionComponent : MonoBehaviour
{
    public Transform originTransform;
    //The object the player is currently holding. Can only be holding 1 object at a time.
    public InteractComponent currentlyHoldingObject = null;
    //The object the player is currently looking at
    public InteractComponent currentFocus = null;
    
    [Header("Properties")]
    //The radius around the center of the camera that will be checked for focus targets.
    public float focusRadius;
    //The distance (in units) focus targets must be within.
    public float focusRange;
    
    void OnEnable()
    {
        InputManager.OnPrimaryUpdated += OnPrimaryAction;
    }

    void OnDisable()
    {
        InputManager.OnPrimaryUpdated -= OnPrimaryAction;
    }

    void OnPrimaryAction()
    {
        Interact();
    }
    
    void Update()
    {
        CalculateCurrentFocus();
    }

    //Raycasts from the origin and checks to see if any objects that can interacted with. Then chooses the primary target. Will always choose closest/First
    void CalculateCurrentFocus()
    {
        Vector3 originPosition = originTransform.transform.position;
        Vector3 endPosition = originPosition + originTransform.transform.forward * focusRange;
        RaycastHit[] hits = Physics.CapsuleCastAll(originPosition, endPosition, focusRadius, originTransform.forward, focusRange);
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
}
