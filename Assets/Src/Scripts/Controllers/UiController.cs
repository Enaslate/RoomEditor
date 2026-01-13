using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    public event Action<Vector3> SaveRoomGeometryRequested;
    public event Action<FurnitureType> AddFurnitureRequested;

    public event Action<Furniture, FurnitureDto> SaveFurnitureRequested;
    public event Action<Furniture> DeleteFurnitureRequested;

    [SerializeField] private UIDocument _document;
    [SerializeField] private VisualTreeAsset _card;

    private VisualElement _geometryPanel;
    private VisualElement _layoutPanel;
    private VisualElement _objectsList;

    private FloatField _roomWidth;
    private FloatField _roomHeight;
    private FloatField _roomLength;

    private void Start()
    {
        var root = _document.rootVisualElement;

        _geometryPanel = root.Q("GeometryPanel");
        _layoutPanel = root.Q("LayoutPanel");

        var geometryTabButton = root.Q<Button>("GeometryTab");
        var layoutTabButton = root.Q<Button>("LayoutTab");

        _roomWidth = root.Q<FloatField>("RoomWidth");
        _roomHeight = root.Q<FloatField>("RoomHeight");
        _roomLength = root.Q<FloatField>("RoomLength");

        _objectsList = root.Q<VisualElement>("ObjectsList");

        var saveRoomButton = root.Q<Button>("SaveRoom");
        var addDoorButton = root.Q<Button>("AddDoor");
        var addWindowButton = root.Q<Button>("AddWindow");

        geometryTabButton.clicked += () => SetMode(UiType.Geometry);
        layoutTabButton.clicked += () => SetMode(UiType.Layout);

        saveRoomButton.clicked += OnSaveRoomGeometryClicked;
        addDoorButton.clicked += OnAddDoorClicked;
        addWindowButton.clicked += OnAddWindowClicked;

        SetMode(UiType.Geometry);
    }

    public void UpdateCards(List<Furniture> furnitures)
    {
        _objectsList.Clear();

        foreach (var furniture in furnitures)
        {
            CreateFurnitureCard(furniture);
        }
    }

    public void CreateFurnitureCard(Furniture furniture)
    {
        var card = _card.CloneTree();

        card.userData = furniture;

        var title = card.Q<Label>("Title");
        var posX = card.Q<FloatField>("PosX");
        var posY = card.Q<FloatField>("PosY");
        var width = card.Q<FloatField>("Width");
        var height = card.Q<FloatField>("Height");
        var saveButton = card.Q<Button>("Save");
        var deleteButton = card.Q<Button>("Delete");

        title.text = furniture.Type.ToString();
        posX.value = furniture.Offset.x;
        posY.value = furniture.Offset.y;
        width.value = furniture.Size.x;
        height.value = furniture.Size.y;

        saveButton.clicked += () => OnSaveFurnitureClicked(furniture, new FurnitureDto
        {
            Offset = new Vector2(posX.value, posY.value),
            Size = new Vector2(width.value, height.value)
        });

        deleteButton.clicked += () => OnDeleteFurnitureClicked(furniture);

        _objectsList.Add(card);
    }

    private void SetMode(UiType type)
    {
        _geometryPanel.style.display =
            type == UiType.Geometry ? DisplayStyle.Flex : DisplayStyle.None;

        _layoutPanel.style.display =
            type == UiType.Layout ? DisplayStyle.Flex : DisplayStyle.None;
    }

    private void OnAddDoorClicked()
    {
        AddFurnitureRequested?.Invoke(FurnitureType.Door);
    }

    private void OnAddWindowClicked()
    {
        AddFurnitureRequested?.Invoke(FurnitureType.Window);
    }

    private void OnSaveRoomGeometryClicked()
    {
        var x = _roomWidth.value;
        var y = _roomHeight.value;
        var z = _roomLength.value;

        var size = new Vector3(x, y, z);
        SaveRoomGeometryRequested?.Invoke(size);
    }

    private void OnSaveFurnitureClicked(Furniture furniture,FurnitureDto dto)
    {
        SaveFurnitureRequested?.Invoke(furniture, dto);
    }

    private void OnDeleteFurnitureClicked(Furniture furniture)
    {
        DeleteFurnitureRequested?.Invoke(furniture);
    }
}
