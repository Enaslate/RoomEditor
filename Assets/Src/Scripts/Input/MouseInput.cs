using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInput
{
    public event Action<Vector2> SelectReceived;
    public event Action<Vector2> DragReceived;
    public event Action DragCancelReceived;
    public event Action<Vector2> MoveReceived;
    public event Action<Vector2> LookReceived;
    public event Action<int> ZoomReceived;

    private readonly InputMap _actionMap;

    public MouseInput(InputMap actionMap)
    {
        _actionMap = actionMap;

        _actionMap.Mouse.Select.started += OnSelectStarted;
        _actionMap.Mouse.Select.canceled += OnSelectCanceled;

        _actionMap.Mouse.Move.started += OnMoveStarted;
        _actionMap.Mouse.Move.canceled += OnMoveCanceled;

        _actionMap.Mouse.Look.started += OnLookStarted;
        _actionMap.Mouse.Look.canceled += OnLookCanceled;

        _actionMap.Mouse.Zoom.performed += OnZoom;
    }

    private void OnSelectStarted(InputAction.CallbackContext context)
    {
        var position = Mouse.current.position.ReadValue();

        SelectReceived?.Invoke(position);

        _actionMap.Mouse.Delta.performed += OnDragPerformed;
    }

    private void OnDragPerformed(InputAction.CallbackContext context)
    {
        DragReceived?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnSelectCanceled(InputAction.CallbackContext context)
    {
        _actionMap.Mouse.Delta.performed -= OnDragPerformed;
        DragCancelReceived?.Invoke();
    }

    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        _actionMap.Mouse.Delta.performed += OnMovePerformed;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _actionMap.Mouse.Delta.performed -= OnMovePerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MoveReceived?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnLookStarted(InputAction.CallbackContext context)
    {
        _actionMap.Mouse.Delta.performed += OnLookPerformed;
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        _actionMap.Mouse.Delta.performed -= OnLookPerformed;
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        LookReceived?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnZoom(InputAction.CallbackContext ctx)
    {
        var vector = ctx.ReadValue<Vector2>();

        var value = Math.Sign(vector.y);
        ZoomReceived?.Invoke(value);
    }
}