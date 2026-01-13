using UnityEngine;

public class SelectHandler : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Camera _camera;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    public Furniture Handle(Vector2 position)
    {
        Ray ray = _camera.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
        {
            if (hit.collider.TryGetComponent<Furniture>(out var furniture))
            {
                return furniture;
            }
        }

        return null;
    }
}