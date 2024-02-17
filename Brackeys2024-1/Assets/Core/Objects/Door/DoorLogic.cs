using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Search;
using UnityEngine.Serialization;

public class DoorLogic : MonoBehaviour
{
    private Transform _localTransform;
    private AudioComponent _audioComponent;
    
    public bool isOpen = false;
    [SerializeField] private float openDuration = 1.5f;
    [SerializeField] private float closeDurationSlow = 1.2f;
    [SerializeField] private float closeDurationFast = 0.5f;

    [Header("Rotation Configuration")]
    [SerializeField] private float rotationAmount = 45f;
    
    private Vector3 _startRotationVector;
    private Vector3 _forwardVector;

    private Coroutine _doorAnimationCoRoutine; 
    
    [Header("Event Configuration")]
    [SerializeField] [Tooltip("ID of the Game-object that will trigger the door open event. Case Insensitive")]
    private string triggeringInteractID;
    [SerializeField] [Tooltip("ID of this Door, Relative to all other doors in the game. Used for the Closing Event")]
    private int doorID;

    [Header("Audio Configuration")] 
    public string openAudioToPlay = "Open";
    public string closeSoftAudioToPlay = "CloseSoft";
    public string closeSlamAudioToPlay = "CloseSlam";

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
        
        _audioComponent = GetComponent<AudioComponent>();
        
        InteractComponent.OnInteractKeysComplete += OnDoorwayOpen;
        InteractComponent.OnInteractUsed += OnDoorwayOpen;
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
        
        _audioComponent.PlaySound(openAudioToPlay);
            
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
        while (time < openDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime;
        }
    }
    
    

    /// <summary>
    /// Call this method to close the door (Requires Object Reference - Non static)
    /// </summary>
    private void Close(bool isFast=false)
    {
        if (isOpen)
        {
            if (_doorAnimationCoRoutine != null)
            {
                StopCoroutine(_doorAnimationCoRoutine);
            }
            
            _audioComponent.PlaySound(isFast ? closeSlamAudioToPlay : closeSoftAudioToPlay);

            _doorAnimationCoRoutine = StartCoroutine(DoorRotationClose(isFast));
        }
    }

    IEnumerator DoorRotationClose(bool isFast)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(_startRotationVector);

        isOpen = false;

        float time = 0;
        float closeDuration = isFast ? closeDurationFast : closeDurationSlow;
        while (time < closeDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime;
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
