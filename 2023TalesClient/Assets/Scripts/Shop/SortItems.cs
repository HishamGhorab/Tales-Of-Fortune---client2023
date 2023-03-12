using System;
using System.Collections.Generic;
using UnityEngine;

public class SortItems
{
    public static List<ItemData> Sort(List<ItemData> list, /*InventoryUIController.SortingState state,*/ bool buyingItems) //todo: idk, static??
    {
        return SortItems();
        
        List<ItemData> SortItems()
        {
            SortPrice(buyingItems);
            
            /*switch (state)
            {
                case InventoryUIController.SortingState.Price:
                    SortPrice(buyingItems);
                    break;
                case InventoryUIController.SortingState.Level:
                    SortLevel();
                    break;
                case InventoryUIController.SortingState.Quantity:
                    SortQuantity();
                    break;
                case InventoryUIController.SortingState.Type:
                    SortType();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }*/

            void SortPrice(bool buyingItems)
            {
                if (buyingItems)
                {
                    list.Sort((a, b) => a.item.baseBuyValue.CompareTo(b.item.baseBuyValue));
                    list.Reverse(); 
                }/*
                else
                {
                    list.Sort((a, b) => a.item.baseSellValue.CompareTo(b.item.baseSellValue));
                    list.Reverse();
                }*/
            }            
            /*
            void SortLevel()
            {
                list.Sort((a, b) => a.item.itemLevel.CompareTo(b.item.itemLevel));
                list.Reverse();
            }            
            
            void SortQuantity()
            {
                list.Sort((a, b) => a.quantity.CompareTo(b.quantity));
                list.Reverse();
            }

            void SortType()
            {
                list.Sort((a, b) => a.item.Type.CompareTo(b.item.Type));
            }*/

            return list;
        }
    }
}
