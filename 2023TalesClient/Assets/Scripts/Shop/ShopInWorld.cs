using System;
using System.Collections;
using System.Collections.Generic;
using RiptideNetworking;
using UnityEngine;

public class ShopInWorld : MonoBehaviour
{
    public string id;
    
    //public delegate void OnEnterStoreRange();
    //public static OnEnterStoreRange onEnterStoreRange;
    
    //public delegate void OnExitStoreRange();
    //public static OnExitStoreRange onExitStoreRange;

    public List<ItemData> BuyingItems { get; set; }
    public List<int> ShopLevels { get; set; }
    public string ShopName { get; set; }

    protected bool canOpenShop = false;
    private void Awake()
    {
        BuyingItems = new List<ItemData>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
            OnInteract();
    }

    public bool Buy(List<InventorySlot> itemsToBuy)
    {
        /*
        foreach (var itemSlot in itemsToBuy)
        {
            //loop through amount to buy
            for (int i = 0; i < itemSlot.GetQuantitySliderValue(); i++)
            {
                if(InventoryManager.Instance.AddItemToInventory(itemSlot.itemData.item))
                {
                    buyerStats.money -= itemSlot.itemData.item.baseBuyValue;
                    UIHandler.Instance.changeMoneyDirtyFlag = true;
                }
            }
        }*/

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
    
    public void InitializeShop()
    {

    }

    public void OpenMenu()
    {
        //UpdateShopUI(this);
    }

    [MessageHandler((ushort) ServerToClientId.sendShopState)]
    private static void AddItemsToShop(Message message)
    {
        /*string shopId = message.GetString();
        if(shopId != id)
            return;
        
        int buyingItemsCount = message.GetInt();

        for (int i = 0; i < buyingItemsCount; i++)
        {
            string id = message.GetString();
            string type = message.GetString();
            string name = message.GetString();
            int itemLevel = message.GetInt();
            int baseBuyValue = message.GetInt();

            Item item = new Item(id, type, name, itemLevel, baseBuyValue);
            
            BuyingItems.Add(new ItemData(item, 1));
        }

        foreach (var VARIABLE in BuyingItems)
        {
            Debug.Log(VARIABLE.item.name);
        }*/
    }
    
    /*private void UpdateShopUI(IShop activeShop)
    {
        ShopUIController.Instance.SetCurrentShop(activeShop);
    }*/
}
