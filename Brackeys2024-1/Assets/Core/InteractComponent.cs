using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct CombinationKey
{
    public string InteractID;
    
    [Tooltip("READ ONLY. Is set at runtime, default = false.")]
    public bool IsCombined;
}
[RequireComponent(typeof(Rigidbody))]
public class InteractComponent : MonoBehaviour
{
    public string InteractID => this.gameObject.name;
    private Rigidbody _rigidbody;
    [Header("Holding")]
    public bool CanBeHeld;
    public PlayerInteractionComponent IsHeldBy;
    
    [Header("Socketing")]
    public SocketComponent IsSocketedBy;
    
    [Header("Usage")]
    [Tooltip("The InteractIDs that this object can be used on/Combined with.")]
    public List<CombinationKey> ValidCombinationKeys;
    public bool CanBeUsedWithoutCombination;
    public bool DestroyOnUse;

    //Called when an interactable is used (Self or on a target).
    //@Param string = this.InteractID
    public static event Action<string> OnInteractUsed;
    //Called when an interactable is used on this.
    //@Param string = this.InteractID
    //@Param string = other.InteractID
    public static event Action<string, string> OnValidInteractUsed;
    //Called when all objects in ValidCombinations have been met
    //@Param string = this.InteractID
    public static event Action<string> OnInteractKeysComplete;
    
    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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

    public bool CanBePickedUp()
    {
        return CanBeHeld;
    }

    public void ToggleIsHeld(bool toggle, PlayerInteractionComponent isHeldBy=null)
    {
        if (IsSocketedBy)
        {
            RemoveFromSocket();
        }
        IsHeldBy = isHeldBy;
        _rigidbody.useGravity = !toggle;
    }

    public void AddToSocket(SocketComponent socket)
    {
        ToggleIsHeld(false);
        IsSocketedBy = socket;
        _rigidbody.useGravity = false;
    }

    public void RemoveFromSocket()
    {
        IsSocketedBy = null;
    }
    
    public virtual bool Interact(InteractComponent focus)
    {
        if (focus)
        {
            //This item is trying to be used on another item
            if (IsValidCombination(focus))
            { 
                Use(focus);
            }
        }
        else
        {
            //This item is trying to be used on nothing
            
            if (CanBeUsedWithoutCombination)
            {
                Use(null);
                return true;
            }
            else
            {
                //Drop Item
                ToggleIsHeld(false);
            }
            return false;
        }

        return false;
    }

    private bool IsValidCombination(InteractComponent otherComponent)
    {
        for (int i = 0; i < ValidCombinationKeys.Count; i++)
        {
            if (ValidCombinationKeys[i].InteractID == otherComponent.InteractID)
            {
                //Update Key
                CombinationKey tempKey = ValidCombinationKeys[i];
                tempKey.IsCombined = true;
                ValidCombinationKeys[i] = tempKey;
                return true;
            }
        }

        return false;
    }

    public virtual bool Use(InteractComponent focusTarget=null)
    {
        (bool, bool) successAndDestroy = (false, true);
        
        if (focusTarget)
        {
            successAndDestroy = focusTarget.UsedOnThis(this);
        }
        else
        {
            successAndDestroy = (CanBeUsedWithoutCombination, DestroyOnUse);
        }
        
        //Was usage successful?
        if (successAndDestroy.Item1)
        {
            Debug.Log($"{InteractID} was used on {(focusTarget ? focusTarget.InteractID : "self")}{(successAndDestroy.Item2 ? ":Destroying" : "")}");
            OnUse(successAndDestroy.Item2);
        }

        return (successAndDestroy.Item2);
    }

    public (bool, bool) UsedOnThis(InteractComponent componentBeingUsed)
    {
        bool success = false;
        //TODO Implement, sometimes things won't want to be destroyed.
        bool destroyOnSuccess = componentBeingUsed.DestroyOnUse;
        success = IsValidCombination(componentBeingUsed);
        if (success)
        {
            OnValidInteractUsed?.Invoke(InteractID, componentBeingUsed.InteractID);

            if (AreAllKeysCombined())
            {
                Debug.Log($"All Keys are complete on {InteractID}");
                OnInteractKeysComplete?.Invoke(InteractID);
            }
        }
        return (success, destroyOnSuccess);
    }

    private bool AreAllKeysCombined()
    {
        foreach (CombinationKey key in ValidCombinationKeys)
        {
            if (!key.IsCombined)
            {
                return false;
            }
        }

        return true;
    }

    public virtual void OnUse(bool destroyOnUse)
    {
        OnInteractUsed?.Invoke(InteractID);

        //Does it need to be destroyed?
        if (destroyOnUse)
        {
            if (IsHeldBy is not null)
            {
                IsHeldBy.currentlyHoldingObject = null;
            }
            Destroy(gameObject);
        }
    }
}
