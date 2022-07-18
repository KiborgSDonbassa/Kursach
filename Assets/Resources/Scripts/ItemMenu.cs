using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemMenu : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        for (int i = 0; i < Inventory.itemPanels.GetLength(0); i++)
        {
            for (int k = 0; k < Inventory.itemPanels.GetLength(1); k++)
            {
                if (eventData.pointerClick == Inventory.itemPanels[i, k].gameObject)
                {
                    Debug.Log("aboBUS");
                    ShowItemMenu(new Vector2Int(i,k));
                    break;
                }
            }
        }
    }
    private void ShowItemMenu(Vector2Int selected)
    {
        CamMove.itemMenu.SetActive(true);
        Button funcButtonnew = CamMove.itemMenu.transform.GetChild(0).gameObject.GetComponent<Button>();
        Button funcButtonnew2 = CamMove.itemMenu.transform.GetChild(1).gameObject.GetComponent<Button>();
        funcButtonnew.onClick.RemoveAllListeners();
        funcButtonnew2.onClick.RemoveAllListeners();
        funcButtonnew.onClick.AddListener(() => DeleteButton(selected));
        funcButtonnew2.onClick.AddListener(() => SortButton());
    }
    private void DeleteButton(Vector2Int itemToDelete)
    {
        Inventory.DeleteItem(itemToDelete);
        Inventory.UpdateInventory();
        CamMove.itemMenu.SetActive(false);
        Button funcButtonnew = CamMove.itemMenu.transform.GetChild(0).gameObject.GetComponent<Button>();
        funcButtonnew.onClick.RemoveAllListeners();
    }
    private void SortButton()
    {
        Inventory.SortItems();
        Inventory.UpdateInventory();
        CamMove.itemMenu.SetActive(false);
        Button funcButtonnew = CamMove.itemMenu.transform.GetChild(1).gameObject.GetComponent<Button>();
        funcButtonnew.onClick.RemoveAllListeners();
    }
}