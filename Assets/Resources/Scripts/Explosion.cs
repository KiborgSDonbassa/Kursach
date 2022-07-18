using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Explosion : Spell, SpellCaster
{
    public void Activate()
    {
        damage = 30;
        GameObject explosion = Instantiate(Resources.Load<GameObject>("Animations/Boom"), targetPos, Quaternion.identity);
        explosion.transform.localScale = new Vector3(7, 7, 7);
        List<Collider2D> targets = new List<Collider2D>();
        Physics2D.OverlapCircle(targetPos, 7, new ContactFilter2D(), targets);
        for (int i = 0; i < targets.Count; i++)
        {
            if(targets[i].tag == "Enemy" || targets[i].tag == "Player")
            {
                SpriteRenderer spr = targets[i].GetComponent<SpriteRenderer>();
                StartCoroutine(GameManager.TakeDamageChangeColor(spr));
                Debug.Log("12345123");
                targets[i].GetComponent<HP>().ChangeHP(-damage, caster);
            }
        }
        Debug.Log("expl");
        Destroy(explosion, 0.333f);
    }
}
