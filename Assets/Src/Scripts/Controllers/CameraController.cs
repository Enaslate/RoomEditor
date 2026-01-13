using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private SceneInputController _inputController;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _target;

    [Header("Move")]
    [SerializeField] private Vector3 _currentOffset;
    [SerializeField] private float _moveSpeed = 0.25f;
    [SerializeField] private float _moveSmoothness = 20f;

    [Header("Rotate")]
    [SerializeField] private Vector2 _rotation;
    [SerializeField] private float _sensitivity = 15f;

    [Header("Zoom")]
    [SerializeField] private float _zoom = -5f;
    [SerializeField] private float _zoomSpeed = 25f;
    [SerializeField] private float _zoomSmoothness = 20f;

    private Vector3 _targetOffset;
    private float _targetZoom;

    private void Start()
    {
        _inputController.MoveReceived += OnMoveReceived;
        _inputController.LookReceived += OnLookReceived;
        _inputController.ZoomReceived += OnZoomReceived;

        _targetOffset = _currentOffset;
        _targetZoom = _zoom;
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    public void SetTarget(Transform target)
    {
        _target = target;

        if (_target == null) return;

        _targetOffset = Vector3.zero;
        _currentOffset = Vector3.zero;
    }

    private void UpdateCamera()
    {
        _currentOffset = Vector3.Lerp(_currentOffset, _targetOffset, Time.deltaTime * _moveSmoothness);
        _zoom = Mathf.Lerp(_zoom, _targetZoom, Time.deltaTime * _zoomSmoothness);

        Quaternion rotation = Quaternion.Euler(_rotation.y, _rotation.x, 0);
        
        Vector3 basePosition = _target != null
            ? _target.position 
            : Vector3.zero;
        Vector3 position = basePosition + _currentOffset + rotation * Vector3.forward * _zoom;

        _cameraTransform.position = position;
        _cameraTransform.rotation = rotation;
    }

    private void OnMoveReceived(Vector2 delta)
    {
        Quaternion rotation = Quaternion.Euler(_rotation.y, _rotation.x, 0);
        Vector3 right = rotation * Vector3.right;
        Vector3 up = rotation * Vector3.up;

        Vector3 move = (right * -delta.x + up * -delta.y) * _moveSpeed * Time.deltaTime;
        _targetOffset += move;
    }

    private void OnLookReceived(Vector2 delta)
    {
        _rotation.x += delta.x * _sensitivity * Time.deltaTime;
        _rotation.y -= delta.y * _sensitivity * Time.deltaTime;
    }

    private void OnZoomReceived(int value)
    {
        _targetZoom += value * _zoomSpeed * Time.deltaTime;
    }

    private void OnDestroy()
    {
        _inputController.MoveReceived -= OnMoveReceived;
        _inputController.LookReceived -= OnLookReceived;
        _inputController.ZoomReceived -= OnZoomReceived;
    }
}