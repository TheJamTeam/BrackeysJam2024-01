using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct CombinationKey
{
    public string InteractID;
    
    [Tooltip("READ ONLY. Is set at runtime, default = false.")]
    public bool IsCombined;
}

[Serializable]
public enum UseType
{
    CannotUse,
    UseOnPickup,
    UseWithoutFocus,
    UseOnValidFocus,
    TargetOfUsageOnly,
}

[Serializable]
public enum UseConditions
{
    None,
    DestroyOnUse,
    OneTimeUse,
}
[RequireComponent(typeof(Rigidbody))][RequireComponent(typeof(AudioComponent))]
public class InteractComponent : MonoBehaviour
{
    private AudioComponent _audioComponent;
    private Rigidbody _rigidbody;
    
    public string InteractID => gameObject.name;
    [SerializeField][Tooltip("Is always the GameObject name.")]
    private string interactID;

    [Header("Usage")]
    public UseType useType;
    public UseConditions useConditions = UseConditions.None;
    public int useCount;
    public string HoldingInteractAudioToPlay = "HoldingInteract";
    public string InteractAudioToPlay = "Interact";
    public string InteractVerb;
    public string InteractWhileHoldingVerb;
    
    [Header("Holding")]
    public bool CanBeHeld;
    public PlayerInteractionComponent IsHeldBy;
    
    [Header("Socketing")]
    public SocketComponent IsSocketedBy;

    [Header("Usage")] 
    [Tooltip("Whether all keys are required to be considered complete.")]
    public bool RequiresAllCombinationKeys;
    [Tooltip("The InteractIDs that this object can be used on/Combined with.")]
    public List<CombinationKey> ValidCombinationKeys;

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

    private void OnValidate()
    {
        interactID = gameObject.name;
    }

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioComponent = GetComponent<AudioComponent>();
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
    

    public bool PickedUpByPlayer(PlayerInteractionComponent isHeldBy=null)
    {
        
        if (IsSocketedBy)
        {
            RemoveFromSocket();
        }
        else
        {
            if (useType is UseType.UseOnPickup)
            {
                TryUse();
                return false;
            }
        }

        
        
        if (!CanBeHeld)
        {
            return false;
        }

        //Successful Pickup
        IsHeldBy = isHeldBy;
        _rigidbody.useGravity = false;
        
        return true;
    }

    public void DroppedByPlayer()
    {
        if (IsHeldBy)
        {
            _rigidbody.useGravity = true;
            IsHeldBy = null;
        }
    }

    public void AddToSocket(SocketComponent socket)
    {
        IsSocketedBy = socket;
        _rigidbody.useGravity = false;
    }

    public void RemoveFromSocket()
    {
        IsSocketedBy = null;
        _rigidbody.useGravity = true;
    }
    
    private bool IsValidCombination(InteractComponent otherComponent)
    {
        for (int i = 0; i < ValidCombinationKeys.Count; i++)
        {
            
            //Ignore case for typos
            if (String.Equals(ValidCombinationKeys[i].InteractID, otherComponent.InteractID, StringComparison.OrdinalIgnoreCase))
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

    public virtual bool TryUse(InteractComponent focusTarget=null)
    {
        bool success = false;
        
        //Success if Use On Pickup, One-Time Use (First), or is a valid focus.
        if (useType == UseType.UseOnPickup || useType == UseType.UseWithoutFocus || (useConditions is UseConditions.OneTimeUse && useCount is 0))
        {
            success = true;
        }
        else if (useType is UseType.UseOnValidFocus)
        {
            if (focusTarget)
            {
                success = focusTarget.UsedOnThis(this);
            }
        }

        //Was usage successful?
        if (success)
        {
            Debug.Log($"{InteractID} was used on {(focusTarget ? focusTarget.InteractID : "self")}.");
            OnUse();
        }

        return success;
    }

    public bool UsedOnThis(InteractComponent componentBeingUsed)
    {
        bool success = IsValidCombination(componentBeingUsed);
        if (success)
        {
            OnValidInteractUsed?.Invoke(InteractID, componentBeingUsed.InteractID);

            if (AreAllKeysCombined())
            {
                Debug.Log($"All Keys are complete on {InteractID}");
                OnInteractKeysComplete?.Invoke(InteractID);
            }
        }
        return success;
    }

    private bool AreAllKeysCombined()
    {
        foreach (CombinationKey key in ValidCombinationKeys)
        {
            if (!key.IsCombined)
            {
                if (RequiresAllCombinationKeys)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public virtual void OnUse()
    {
        OnInteractUsed?.Invoke(InteractID);
        _audioComponent.PlaySound(IsHeldBy ? HoldingInteractAudioToPlay : InteractAudioToPlay);

        
        //Does it need to be destroyed?
        if (useConditions is UseConditions.DestroyOnUse)
        {
            if (IsHeldBy is not null)
            {
                IsHeldBy.DropHeldObject(true);
                IsHeldBy.currentlyHoldingObject = null;
            }
            Destroy(gameObject);
        }
        else
        {
            useCount++;
        }
    }
}
