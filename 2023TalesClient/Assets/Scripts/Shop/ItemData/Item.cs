using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public Item(string id, string type, string name, int itemLevel, int baseBuyValue, Sprite icon)
    {
        this.id = id;
        this.type = type;
        this.name = name;
        this.itemLevel = itemLevel;
        this.baseBuyValue = baseBuyValue;
        this.icon = icon;
    }

    public string id;
    public string type;
    public string name;
    public int itemLevel;

    [Header("Vendor")]
    public int baseBuyValue;
    
    public Sprite icon;
}
