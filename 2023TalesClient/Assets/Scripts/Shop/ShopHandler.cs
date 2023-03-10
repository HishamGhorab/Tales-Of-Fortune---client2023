using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHandler : MonoBehaviour
{
    [SerializeField] private List<ItemSO> allItemSOs;
    private void Start()
    {
        foreach (var item in allItemSOs)
        {
            ItemSO.itemSos.Add(item.id, item);
        }
        
    }
}
