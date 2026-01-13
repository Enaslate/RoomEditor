using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public BoxCollider Collider { get; private set; }
    public List<Furniture> Furnitures { get; private set; } = new();

    private void Awake()
    {
        if (Collider == null)
            Collider = GetComponent<BoxCollider>();
    }

    public void SetSize(Vector3 size)
    {
        transform.localScale = size;
    }
}