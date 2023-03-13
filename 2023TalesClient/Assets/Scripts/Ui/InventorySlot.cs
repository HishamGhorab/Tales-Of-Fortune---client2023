using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class InventorySlot : VisualElement
{
    public UIStorageController owner;
    
    public Image icon;
    public Label quantityText;
    public VisualElement topHalf;
    public VisualElement botHalf;
    
    public ItemData itemData;
    public bool isShopView = false;

    private bool isSelected = false;
    
    public InventorySlot()
    {
        itemData = null;

        topHalf = new VisualElement();
        botHalf = new VisualElement();
        icon = new Image();
        quantityText = new Label();
        
        Add(icon);
        icon.Add(topHalf);
        icon.Add(botHalf);
        botHalf.Add(quantityText);
        
        AddUSSToElements();
        
        quantityText.text = "";
        
        RegisterCallback<PointerDownEvent>(OnPointerDown);
        RegisterCallback<PointerOverEvent>(OnPointerOver);
        RegisterCallback<PointerOutEvent>(OnPointerOut);
        
        void AddUSSToElements()
        {
            //Add USS style properties to the elements
            icon.AddToClassList("slotIcon");
            topHalf.AddToClassList("slotIconPropertiesContainer");
            botHalf.AddToClassList("slotIconPropertiesContainer");
            quantityText.AddToClassList("slotText");
            quantityText.AddToClassList("slotQuantityText");
            AddToClassList("inventorySlotContainer");
        }
    }
    
    public void HoldItem(ItemData item)
    {
        itemData = item;
        icon.image = item.item.icon.texture;

        if (itemData.quantity > 1)
        {
            quantityText.text = itemData.quantity.ToString();
        }
    }
    
    public void DropItem()
    {
        itemData = null;
        icon.image = null;
        quantityText.text = "";
    }
    
    private void OnPointerOver(PointerOverEvent evt)
    {
        if (itemData != null)
        {
            
        }
    }
    
    private void OnPointerOut(PointerOutEvent evt)
    {
        
    }
    
    private void OnPointerDown(PointerDownEvent evt)
    {
        //Not the left mouse button
        if (evt.button != 0 || itemData == null)
        {
            return;
        }
        
    }
    
    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<InventorySlot, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}
