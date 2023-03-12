using System;
using System.Collections;
using System.Collections.Generic;
using RiptideNetworking;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ShopInWorld : MonoBehaviour
{
    public string id;
    
    //public delegate void OnEnterStoreRange();
    //public static OnEnterStoreRange onEnterStoreRange;
    
    //public delegate void OnExitStoreRange();
    //public static OnExitStoreRange onExitStoreRange;

    public static Dictionary<string, ShopInWorld> Shops = new Dictionary<string, ShopInWorld>();
    public static Dictionary<string, List<ItemData>> ShopsBuyingItems = new Dictionary<string, List<ItemData>>();

    //public List<ItemData> BuyingItems { get; set; }
    //public List<int> ShopLevels { get; set; }
    public string ShopName { get; set; }

    private static ShopUIController shopUIController;
    
    protected bool canOpenShop = false;
    private void Awake()
    {
        //BuyingItems = new List<ItemData>();
        shopUIController = GameObject.Find("ShopUI").GetComponent<ShopUIController>();
    }

    private void OnMouseDown()
    {
        //todo: is this rly the best way to do it? shouldn't each tile be detectable without object colliders?
        OnInteract();
    }
    
    public bool Buy(List<InventorySlot> itemsToBuy)
    {
        Debug.Log("Buying request to shop: " + id);
        
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.onShopBuy);
        message.AddInt(itemsToBuy.Count);
        
        foreach (var item in itemsToBuy)
        {
            string itemId = item.itemData.item.id;
            message.AddString(itemId);
        }
        
        TOFNetworkManager.Singleton.Client.Send(message);
        return true;
    }

    /*public virtual bool Sell(List<InventorySlot> itemsToSell, PlayerStats sellerStats)
    {
        if (itemsToSell.Count <= 0)
            return false;
        
        foreach (var itemSlot in itemsToSell)
        {
            for (int i = 0; i < itemSlot.GetQuantitySliderValue(); i++)
            {
                if(InventoryManager.Instance.RemoveItemFromInventory(itemSlot.itemData.item))
                {
                    sellerStats.money += itemSlot.itemData.item.baseSellValue;
                    UIHandler.Instance.changeMoneyDirtyFlag = true;
                }
            }
        }

        return true;
    }*/

    public void OnInteract()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.onShopInteract);
        message.AddString(id);
        TOFNetworkManager.Singleton.Client.Send(message);
    }
    
    public void OpenMenu()
    {
        //UpdateShopUI(this);
    }

    [MessageHandler((ushort) ServerToClientId.sendShopState)]
    private static void InitializeShop(Message message)
    {
        string shopId = message.GetString();
        
        /*if(shopId != id) //????
            return;*/

        List<ItemData> items = new List<ItemData>();

        int buyingItemsCount = message.GetInt();

        for (int i = 0; i < buyingItemsCount; i++)
        {
            string id = message.GetString();
            string type = message.GetString();
            string name = message.GetString();
            int itemLevel = message.GetInt();
            int baseBuyValue = message.GetInt();

            Sprite icon = ItemSO.itemSos[id].icon;
            
            Item item = new Item(id, type, name, itemLevel, baseBuyValue, icon);
            
            items.Add(new ItemData(item, 1));
        }
        
        if(!ShopsBuyingItems.ContainsKey(shopId))
            ShopsBuyingItems.Add(shopId, items);
        
        shopUIController.InitializeMenu(Shops[shopId], shopId);
    }
    
    /*private void UpdateShopUI(IShop activeShop)
    {
        ShopUIController.Instance.SetCurrentShop(activeShop);
    }*/
}
