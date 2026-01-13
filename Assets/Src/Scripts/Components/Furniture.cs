using UnityEngine;

public class Furniture : MonoBehaviour
{
    public FurnitureType Type;
    public Wall Wall;
    public Vector2 Offset;
    public Vector2 Size;

    private Room _room;

    public void SetRoom(Room room)
    {
        _room = room;
        UpdateTransform();
    }

    public void UpdateTransform()
    {
        var bounds = _room.Collider.bounds;
        var center = bounds.center;

        Vector3 pos = center;
        Quaternion rot = Quaternion.identity;

        switch (Wall)
        {
            case Wall.Front:
                pos.x += Offset.x;
                pos.y += Offset.y;
                pos.z = bounds.max.z;
                rot = Quaternion.Euler(0, 180, 0);
                break;

            case Wall.Back:
                pos.x += Offset.x;
                pos.y += Offset.y;
                pos.z = bounds.min.z;
                rot = Quaternion.identity;
                break;

            case Wall.Left:
                pos.x = bounds.min.x;
                pos.y += Offset.y;
                pos.z += Offset.x;
                rot = Quaternion.Euler(0, 90, 0);
                break;

            case Wall.Right:
                pos.x = bounds.max.x;
                pos.y += Offset.y;
                pos.z += Offset.x;
                rot = Quaternion.Euler(0, -90, 0);
                break;
        }

        transform.SetPositionAndRotation(pos, rot);
        transform.localScale = new Vector3(Size.x, Size.y, transform.localScale.z);
    }
}
