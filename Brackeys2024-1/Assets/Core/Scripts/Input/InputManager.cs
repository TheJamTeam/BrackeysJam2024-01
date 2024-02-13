using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(EventSystem))]
[RequireComponent(typeof(InputSystemUIInputModule))]
[RequireComponent(typeof(PlayerInput))]

public class InputManager : Singleton<InputManager>
{
    private PlayerInput playerInput;
    [SerializeField] private Vector2 pointerPositionScreenSpace;
    [SerializeField] private Vector3 lookDelta;
    [SerializeField] private Vector3 moveDelta;

    public static event Action OnPrimaryUpdated;
    public static event Action<Vector2> OnLookUpdated;
    public static event Action<Vector2> OnMoveUpdated;

    protected override void Awake()
    {
        base.Awake();
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }
        
        if (!playerInput.camera)
        {
            playerInput.camera = Camera.main;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        moveDelta = pos;
        OnMoveUpdated?.Invoke(moveDelta);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        lookDelta = pos;
        OnLookUpdated?.Invoke(lookDelta);

    }

    public void OnPrimary(InputAction.CallbackContext context)
    {
        OnPrimaryUpdated?.Invoke();
    }

    //Returns the pointer position in screen space as 0..1f
    public void OnPointerPosition(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        pos = new Vector2(Mathf.Clamp(pos.x / Screen.width, 0f, 1f), Mathf.Clamp(pos.y / Screen.height, 0f, 1f));
        pointerPositionScreenSpace = pos;
    }


}
