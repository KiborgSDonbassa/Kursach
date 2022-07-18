using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static List<GameObject> Actors = new List<GameObject>();
    public delegate void VoidEvent();
    public delegate void VoidTargetEvent(Vector2 target);
    public static event VoidEvent openShopInventory;
    public static void OpenShopInventory() => openShopInventory?.Invoke();
    public static event VoidEvent closeShopInventory;
    public static void CloseShopInventory() => closeShopInventory?.Invoke();
    public static event VoidEvent changeInventory;
    public static void ChangeInventory() => changeInventory?.Invoke();

    public static void ShootArrow(GameObject target, GameObject caster)
    {
        GameObject arrow = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/arrow"), caster.transform.position, Quaternion.identity);
        Arrow arrowComp =  arrow.AddComponent<Arrow>();
        arrowComp.initialize(target, caster);
        arrowComp.Activate();
    }
    public static void CastHeal(GameObject caster, int _manaCost = 0)
    {
        if (_manaCost != 0)
        {
            HP casterMP = caster.GetComponent<HP>();
            if (casterMP.manaPoints >= _manaCost) casterMP.ChangeMP(-_manaCost);
            else return;
        }
        GameObject healSPell = new GameObject();
        Heal a = healSPell.AddComponent<Heal>();
        a.initialize(null, caster);
        a.Activate();
    }
    public static void CastLightBolt(GameObject target, GameObject caster, int _manaCost = 0, GameObject castPoint = null)
    {
        if (_manaCost != 0)
        {
            HP casterMP = caster.GetComponent<HP>();
            Debug.Log(casterMP.manaPoints + " " + _manaCost);
            if (casterMP.manaPoints >= _manaCost)
            {
                Debug.Log("Mana");
                casterMP.ChangeMP(-_manaCost);
            }
            else return;
        }

        if (castPoint == null)
        {
            GameObject explosion = new GameObject();
            explosion.transform.position = target.transform.position;
            LightBolt light = explosion.AddComponent<LightBolt>();
            light.initialize(target, caster);
            light.Activate();
        }
        else
        {
            GameObject explosion = new GameObject();
            explosion.transform.position = target.transform.position;
            LightBolt light = explosion.AddComponent<LightBolt>();
            light.initialize(target, castPoint);
            light.Activate();
        }
        
    }
    public static void CastFireball(GameObject target, GameObject caster, int _manaCost = 0)
    {
        if (_manaCost != 0)
        {
            HP casterMP = caster.GetComponent<HP>();
            if (casterMP.manaPoints >= _manaCost) casterMP.ChangeMP(-_manaCost);
            else return;
        }
        GameObject fire = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Fire"), caster.transform.position, Quaternion.identity);
        Fireball fireball = fire.AddComponent<Fireball>();
        fireball.initialize(target);
        fireball.Activate();
    }
    public static void CastExlosion(Vector2 targetPos, GameObject caster, int _manaCost = 0)
    {
        if (_manaCost != 0)
        {
            HP casterMP = caster.GetComponent<HP>();
            if (casterMP.manaPoints >= _manaCost) casterMP.ChangeMP(-_manaCost);
            else return;
        }
        GameObject explosion = new GameObject();
        explosion.transform.position = targetPos;
        Explosion ex = explosion.AddComponent<Explosion>();
        ex.initialize(explosion.transform.position);
        ex.Activate();
    }
    public static IEnumerator TakeDamageChangeColor(SpriteRenderer a)
    {
        Color col = Color.white;
        SpriteRenderer? b = a;
        b.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if(b != null) b.color = col;
    }
    public static IEnumerator Timer(VoidEvent a, float second)
    {
        yield return new WaitForSeconds(second);
        a?.Invoke();
    }
    public static IEnumerator Timer(float second)
    {
        yield return new WaitForSeconds(second);
    }
}
