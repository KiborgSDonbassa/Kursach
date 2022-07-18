using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public float healPoints = 100;
    public float manaPoints = 100;
    public int grivny;
    
    public void ChangeHP(float i, GameObject killer)
    {
        if (gameObject == Hero.Player)
        {
            ChangePlayerHP(i, killer);
            return;
        }
        healPoints += i;
        if(healPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void ChangeHP(float i)
    {
        if (gameObject == Hero.Player)
        {
            ChangePlayerHP(i, null);
            return;
        }
        healPoints += i;
        if(healPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void ChangeMP(float i)
    {
        manaPoints += i;
        if (manaPoints < 0) manaPoints = 0;
        GameObject.Find("MP_Bar").GetComponent<Image>().fillAmount = manaPoints/100;
    }
    public void ChangeGold(int i)
    {
        grivny += i;
    }
    private void ChangePlayerHP(float i, GameObject killer)
    {
        healPoints += i;
        GameObject.Find("HP_Bar").GetComponent<Image>().fillAmount = healPoints/100;
        if (healPoints <= 0)
        {
            CamMove.DieScreenStatic.SetActive(true);
            Image killer_ = CamMove.DieScreenStatic.transform.GetChild(1).GetComponent<Image>();
            killer_.sprite = killer.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
