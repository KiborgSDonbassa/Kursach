using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Shop : UsableObjects
{
    public Item Select;
    [SerializeField] private Item[] selectesScrObg;
    private GameObject[] selectes;
    private GameObject purchase;
    [SerializeField] private GameObject shopInterface;
    private GameManager.VoidEvent loadShop;
    private GameManager.VoidEvent unloadShop;

    private void Awake()
    {
        loadShop += LoadSelects;
        unloadShop += UnLoadSelects;
        selectes = new GameObject[shopInterface.transform.childCount - 1];
        for(int i = 0; i < selectes.Length; i++)
        {
            selectes[i] = shopInterface.transform.GetChild(i + 1).gameObject;
        }
        purchase = shopInterface.transform.GetChild(0).gameObject;
        shopInterface.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Enabled = true;
        GameManager.openShopInventory += loadShop;
        GameManager.closeShopInventory += unloadShop;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Enabled = false;
        GameManager.openShopInventory -= loadShop;
        GameManager.closeShopInventory -= unloadShop;
        if (shopInterface.activeSelf == true)
        {
            shopInterface.SetActive(false);
        }
        UnLoadSelects();
    }
    private void LoadSelects()
    {
        shopInterface.SetActive(true);
        for (int i = 0; i < selectesScrObg.Length; i++)
        {
            int g = i;
            selectes[g].GetComponent<Image>().sprite = selectesScrObg[g].icon_;
            selectes[g].GetComponent<Button>().onClick.AddListener(() => SelectItem(selectesScrObg[g]));
        }
        purchase.GetComponent<Button>().onClick.AddListener(Purchase);
    }
    private void UnLoadSelects()
    {
        shopInterface.SetActive(false);
        for (int i = 0; i < selectesScrObg.Length; i++)
        {
            selectes[i].GetComponent<Image>().sprite = null;
            selectes[i].GetComponent<Button>().onClick.RemoveAllListeners();
        }
        purchase.GetComponent<Button>().onClick.RemoveAllListeners();
    }
    public void SelectItem(Item it)
    {
        Select = it;
        return;
    }
    public void Purchase()
    {
        Activate();
        Inventory.UpdateInventory();
    }
    override protected void Activate()
    {
        if (Select == null)
        {
            return;
        }
        else
        {
            Inventory.AddItem(Select);
        }
    }
}
