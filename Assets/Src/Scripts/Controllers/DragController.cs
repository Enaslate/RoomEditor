using UnityEngine;

public class DragController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _roomMask;

    private Vector2 _position;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    public void BeginDrag(Vector2 startScreenPosition)
    {
        _position = startScreenPosition;
    }

    public void Drag(Vector2 delta, Furniture furniture, Room room)
    {
        _position += delta;

        Ray ray = _camera.ScreenPointToRay(_position);

        Vector3 dir = ray.direction;

        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, _roomMask))
            return;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
        {
            furniture.Wall = dir.x > 0 
                ? Wall.Right 
                : Wall.Left;
        }
        else
        {
            furniture.Wall = dir.z > 0 
                ? Wall.Front 
                : Wall.Back;
        }

        Bounds bounds = room.Collider.bounds;
        Vector3 localPoint = hit.point - bounds.center;

        switch (furniture.Wall)
        {
            case Wall.Front:
            case Wall.Back:
                furniture.Offset = new Vector2(localPoint.x, localPoint.y);
                break;

            case Wall.Left:
            case Wall.Right:
                furniture.Offset = new Vector2(localPoint.z, localPoint.y);
                break;
        }

        furniture.UpdateTransform();
    }
}
