using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Item[,] items = new Item[3, 6];
    public static Image[,] itemPanels = new Image[3,6];
    public static Image[] equipmentPanels = new Image[5];
    
    private void Awake()
    {
        for (int i = 0; i < items.GetLength(0); i++)
        {
            for (int j = 0; j < items.GetLength(1); j++)
            {
                items[i, j] = new Item();
                items[i, j].id_ = -1;
                items[i, j].name_ = "0";
            }
        }
        UpdateInventory();
    }
    public static void AddItem(Item it)
    {
        for (int i = 0; i < items.GetLength(0); i++)
        {
            for (int j = 0; j < items.GetLength(1); j++)
            {
                if (items[i, j].id_ == -1)
                {
                    items[i, j] = it;
                    return;
                }
            }
        }
    }
    public static void DeleteItem(Vector2Int itemPos)
    {
        items[itemPos.x, itemPos.y] = new Item();
        items[itemPos.x, itemPos.y].id_ = -1;
        items[itemPos.x, itemPos.y].name_ = "0";
        items[itemPos.x, itemPos.y].icon_ = CamMove.defaultSprite;
    }
    public static void UpdateInventory()
    {
        for (int i = 0; i < Inventory.itemPanels.GetLength(0); i++)
        {
            for (int j = 0; j < Inventory.itemPanels.GetLength(1); j++)
            {
                if(items[i,j].icon_ == null) continue;
                itemPanels[i, j].sprite = items[i, j].icon_;
            }
        }
    }
    public static void SortItems()
    {
        List<Item> a = new List<Item>();
        a = items.OfType<Item>().ToList().Where(x => x.id_ != -1).ToList();
        Debug.Log(a.Count);
        for (int i = 0; i < items.GetLength(0); i++)
        {
            for (int j = 0; j < items.GetLength(1); j++)
            {
                items[i, j] = new Item();
                items[i, j].id_ = -1;
                items[i, j].name_ = "0";
                items[i, j].icon_ = CamMove.defaultSprite;
            }
        }

        int ij = 0;
        for (int i = 0; i < items.GetLength(0); i++)
        {
            for (int j = 0; j < items.GetLength(1); j++)
            {
                try
                {
                    items[i, j] = a[ij];
                    ij++;
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
    }
}
