using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Search;
using UnityEngine.Serialization;

public class DoorLogic : MonoBehaviour
{
    private Transform _localTransform;


    
    public bool isOpen = false;
    [SerializeField] private float speed = 0.5f;

    [Header("Rotation Configuration")]
    [SerializeField] private float rotationAmount = 45f;
    
    private Vector3 _startRotationVector;
    private Vector3 _forwardVector;

    private Coroutine _doorAnimationCoRoutine; 
    
    [Header("Event Configuration")]
    [SerializeField] [Tooltip("ID of the Game-object that will trigger the door open event. Case Insensitive")]
    private string triggeringInteractID;
    [SerializeField] [Tooltip("ID of this Door, Relative to all other doors in the game")]
    private int doorID;

    [Header("DoorPhysics")] 
    [SerializeField] private Rigidbody doorBody;

    public string TriggeringInteractID
    {
        get => triggeringInteractID;
        set => triggeringInteractID = value;
    }
    /// <summary>
    /// OnEnable is called each time an Object is Enabled
    /// </summary>
    private void OnEnable()
    {
        _localTransform = transform; // Efficiency 
        _startRotationVector = _localTransform.rotation.eulerAngles; 
        _forwardVector = _localTransform.right;
        
        //OpenDoor(); //Testing Purposes
        
        InteractComponent.OnInteractKeysComplete += OnDoorwayOpen;
        DoorEvents.CloseDoor += OnDoorwayClose;
    }

    /// <summary>
    /// Opens the door and plays the sound associated with the door. 
    /// </summary>
    /// <param name="interactID">InteractID parsed from the InteractComponent Event. Equivalent to gameObject Name</param>
    private void OnDoorwayOpen(string interactID)
    {   // Are the two IDs equal, ignoring case (incase of typo)
        if (String.Equals(interactID, triggeringInteractID, StringComparison.OrdinalIgnoreCase))
        {
            OpenDoor();
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="doorToCloseID">The ID of the door to close, Relative to all doors in game</param>
    void OnDoorwayClose(int doorToCloseID)
    {
        if (doorToCloseID == doorID)
        {
            Close();
        }
    }

    /// <summary>
    /// Checks door isnt open before running the Open Coroutine
    /// </summary>
    private void OpenDoor()
    {
        if (isOpen) return;
        if (_doorAnimationCoRoutine != null)
        {
            StopCoroutine(_doorAnimationCoRoutine);
        }
            
        _doorAnimationCoRoutine = StartCoroutine(DoorRotationOpen());
    }

    /// <summary>
    /// Open the Door in the Positive Y Direction, Over Several Frames
    /// </summary>
    /// <returns>null</returns>
    IEnumerator DoorRotationOpen()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;
        
        // Rotate in the positive Y direction
        endRotation = Quaternion.Euler(new Vector3(0, startRotation.y + rotationAmount, 0));
        
        isOpen = true;
        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        doorBody.isKinematic = false;
    }
    
    

    /// <summary>
    /// Call this method to close the door (Requires Object Reference - Non static)
    /// </summary>
    private void Close()
    {
        if (isOpen)
        {
            if (_doorAnimationCoRoutine != null)
            {
                StopCoroutine(_doorAnimationCoRoutine);
            }

            _doorAnimationCoRoutine = StartCoroutine(DoorRotationClose());
        }
    }

    IEnumerator DoorRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(_startRotationVector);

        doorBody.isKinematic = true;
        
        isOpen = false;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
    
    /// <summary>
    /// Called when on Object is Disabled at runtime, or when it is Destroyed
    /// </summary>
    private void OnDisable()
    {
        InteractComponent.OnInteractKeysComplete -= OnDoorwayOpen;
        DoorEvents.CloseDoor -= OnDoorwayClose;
    }
}
