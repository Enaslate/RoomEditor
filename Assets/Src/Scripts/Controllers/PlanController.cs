using UnityEngine;

public class PlanController : MonoBehaviour
{
    [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private GameObject _doorPrefab;
    [SerializeField] private GameObject _windowPrefab;

    public Room CreateRoom(Vector3 size)
    {
        var instance = Instantiate(_roomPrefab, Vector3.zero, Quaternion.identity);
        var room = instance.GetComponent<Room>();

        if (room == null)
        {
            Debug.LogError($"Отсутствует компонент {nameof(Room)}");
        }

        room.SetSize(size);

        return room;
    }

    public void SetRoomSize(Room room, Vector3 size)
    {
        room.SetSize(size);
    }

    public Furniture CreateFurniture(FurnitureType furnitureType, Room room)
    {
        var prefab = furnitureType switch
        {
            FurnitureType.Door => _doorPrefab,
            FurnitureType.Window => _windowPrefab,
            _ => null
        };

        if (prefab == null)
        {
            Debug.LogError($"Unknown type {nameof(FurnitureType)}: {furnitureType.ToString()}");
            return null;
        }

        var instance = Instantiate(prefab);
        var furniture = instance.GetComponent<Furniture>();

        room.Furnitures.Add(furniture);

        furniture.Wall = Wall.Front;
        furniture.Offset = Vector2.zero;
        furniture.Size = furniture.transform.localScale;
        furniture.SetRoom(room);

        return furniture;
    }

    public void UpdateFurniture(Furniture furniture, FurnitureDto dto)
    {
        furniture.Offset = dto.Offset;
        furniture.Size = dto.Size;

        furniture.UpdateTransform();
    }

    public void DeleteFurniture(Furniture furniture, Room room)
    {
        if (room == null || furniture == null)
        {
            Debug.LogError("Error furniture deleting");
            return;
        }

        room.Furnitures.Remove(furniture);
        Destroy(furniture.gameObject);
    }
}