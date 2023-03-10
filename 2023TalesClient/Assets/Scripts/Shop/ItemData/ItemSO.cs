using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public static Dictionary<string, ItemSO> itemSos = new Dictionary<string, ItemSO>();

    public string id;
    public Sprite icon;
}
