using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovementComponent : MonoBehaviour
{
    [Header("Components")]
    public Transform playerCamera;

    [Header("Movement")]
    public float movementSpeed;
    private Vector3 _movementInput;
    private Vector3 velocity;
    public Vector3 Velocity => velocity;

    [Header("Rotation")]
    public float rotationSensitivity;
    public float rotationDeadZone;
    public Vector2 verticalRotationLimits;
    private Vector3 _currentRotation = Vector3.zero;
    private float horizontalRotationInput;
    private float verticalRotationInput;
    
    void OnEnable()
    {
        InputManager.OnMoveUpdated += OnMovementInput;
        InputManager.OnLookUpdated += OnLookInput;
    }
    
    void OnDisable()
    {
        InputManager.OnMoveUpdated -= OnMovementInput;
        InputManager.OnLookUpdated -= OnLookInput;
    }

    void FixedUpdate()
    {
		if(Game.IsPaused) return;

        if (_movementInput != Vector3.zero)
        {
            velocity = Time.deltaTime * movementSpeed * _movementInput;
            transform.Translate( velocity);
        }
        
        //Horizontal Rotation
        _currentRotation.x += horizontalRotationInput * rotationSensitivity;
        _currentRotation.x = _currentRotation.x % 360.0f;
        transform.rotation = Quaternion.Euler(0, _currentRotation.x, 0);
        
        //Vertical Rotation
        _currentRotation.y += verticalRotationInput * rotationSensitivity;
        _currentRotation.y = Mathf.Clamp(_currentRotation.y, verticalRotationLimits.x, verticalRotationLimits.y);
        playerCamera.transform.localRotation = Quaternion.Euler(-_currentRotation.y, 0 , 0);
    }

    public bool IsMoving()
    {
        return !Game.IsPaused && _movementInput != Vector3.zero;
    }

    void OnMovementInput(Vector2 input)
    {
        _movementInput = new Vector3(input.x, 0, input.y);
    }

    void OnLookInput(Vector2 inputDelta)
    {
        horizontalRotationInput = Mathf.Abs(inputDelta.x) >= rotationDeadZone ? inputDelta.x : 0;
        verticalRotationInput = Mathf.Abs(inputDelta.y) >= rotationDeadZone ? inputDelta.y : 0;
    }
}
