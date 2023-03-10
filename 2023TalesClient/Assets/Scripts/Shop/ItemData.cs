using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public ItemData(Item item, int quantity) 
    {
        this.item = item; 
        this.quantity = quantity; 
        this.name = item.name;
    }
    
    public string name; 
    public Item item; 
    public int quantity;
}
