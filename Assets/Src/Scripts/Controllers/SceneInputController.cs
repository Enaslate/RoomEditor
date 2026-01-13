using System;
using UnityEngine;

public class SceneInputController : MonoBehaviour
{
    public Action<Vector2> SelectReceived;
    public Action<Vector2> DragReceived;
    public Action DragCancelReceived;
    public Action<Vector2> MoveReceived;
    public event Action<Vector2> LookReceived;
    public event Action<int> ZoomReceived;

    private InputMap _inputMap;
    private MouseInput _input;

    private void Awake()
    {
        _inputMap = new InputMap();

        _inputMap.Enable();

        InitInput(_inputMap);
    }

    private void InitInput(InputMap inputMap)
    {
        _input = new MouseInput(_inputMap);

        _input.SelectReceived += OnSelectReceived;
        _input.DragReceived += OnDragReceived;
        _input.DragCancelReceived += OnDragCancelReceived;
        _input.MoveReceived += OnMoveReceived;
        _input.LookReceived += OnLookReceived;
        _input.ZoomReceived += OnZoomReceived;
    }

    private void OnSelectReceived(Vector2 position)
    {
        SelectReceived?.Invoke(position);
    }

    private void OnDragReceived(Vector2 delta)
    {
        DragReceived?.Invoke(delta);
    }

    private void OnDragCancelReceived()
    {
        DragCancelReceived?.Invoke();
    }

    private void OnMoveReceived(Vector2 delta)
    {
        MoveReceived?.Invoke(delta);
    }

    private void OnLookReceived(Vector2 delta)
    {
        LookReceived?.Invoke(delta);
    }

    private void OnZoomReceived(int value)
    {
        ZoomReceived?.Invoke(value);
    }

    private void OnDestroy()
    {
        _input.SelectReceived -= OnSelectReceived;
        _input.DragReceived -= OnDragReceived;
        _input.DragCancelReceived -= OnDragCancelReceived;
        _input.MoveReceived -= OnMoveReceived;
        _input.LookReceived -= OnLookReceived;
        _input.ZoomReceived -= OnZoomReceived;
    }
}