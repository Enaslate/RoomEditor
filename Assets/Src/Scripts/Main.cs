using System;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private PlanController _planController;
    [SerializeField] private UiController _uiController;
    [SerializeField] private SceneInputController _sceneInputController;
    [SerializeField] private SelectHandler _selectHandler;
    [SerializeField] private DragController _dragController;

    [SerializeField] private Room _currentRoom;
    [SerializeField] private Furniture _selectedFurniture;

    private void Start()
    {
        _uiController.SaveRoomGeometryRequested += OnRoomGeometrySaveRequested;
        _uiController.AddFurnitureRequested += OnAddFurnitureRequested;
        _uiController.SaveFurnitureRequested += OnSaveFurnitureRequested;
        _uiController.DeleteFurnitureRequested += OnDeleteFurnitureRequested;

        _sceneInputController.SelectReceived += OnSelectReceived;
        _sceneInputController.DragReceived += OnDragReceived;
        _sceneInputController.DragCancelReceived += OnDragCancelReceived;
    }

    private void OnSelectReceived(Vector2 position)
    {
        _selectedFurniture = _selectHandler.Handle(position);

        if (_selectedFurniture != null)
            _dragController.BeginDrag(position);
    }

    private void OnDragReceived(Vector2 delta)
    {
        if (_currentRoom == null || _selectedFurniture == null) return;

        _dragController.Drag(delta, _selectedFurniture, _currentRoom);
    }

    private void OnDragCancelReceived()
    {
        if (_selectedFurniture == null) return;

        _uiController.UpdateCards(_currentRoom.Furnitures);

        _selectedFurniture = null;
    }

    private void OnRoomGeometrySaveRequested(Vector3 size)
    {
        if (_currentRoom == null)
        {
            _currentRoom = _planController.CreateRoom(size);
            return;
        }

        _planController.SetRoomSize(_currentRoom, size);
    }

    private void OnAddFurnitureRequested(FurnitureType furnitureType)
    {
        if (_currentRoom == null)
        {
            Debug.LogWarning($"Trying create furniture without room!");
            return;
        }

        var furniture = _planController.CreateFurniture(furnitureType, _currentRoom);

        _uiController.CreateFurnitureCard(furniture);
    }

    private void OnSaveFurnitureRequested(Furniture furniture, FurnitureDto dto)
    {
        _planController.UpdateFurniture(furniture, dto);
    }

    private void OnDeleteFurnitureRequested(Furniture furniture)
    {
        _planController.DeleteFurniture(furniture, _currentRoom);
        _uiController.UpdateCards(_currentRoom.Furnitures);
    }

    private void OnDestroy()
    {
        _uiController.SaveRoomGeometryRequested -= OnRoomGeometrySaveRequested;
        _uiController.AddFurnitureRequested -= OnAddFurnitureRequested;
        _uiController.SaveFurnitureRequested -= OnSaveFurnitureRequested;
        _uiController.DeleteFurnitureRequested -= OnDeleteFurnitureRequested;
    }
}