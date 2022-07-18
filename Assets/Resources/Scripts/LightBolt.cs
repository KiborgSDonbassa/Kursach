using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class LightBolt : Spell, SpellCaster
{
    public void Activate()
    {
        damage = 40;
        StartCoroutine(CastBolt());
    }
    private IEnumerator CastBolt()
    {
        GameObject LightBolt = Instantiate(Resources.Load<GameObject>("Prefab/LightBolt"), transform.position,
                    Quaternion.identity);
        LightBolt.transform.position = caster.transform.position;
        LightningBoltScript a = LightBolt.GetComponent<LightningBoltScript>();
        a.StartObject = caster;
        a.EndObject = target;
        List<Collider2D> blackList = new List<Collider2D>();
        blackList.Add(target.GetComponent<Collider2D>());
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            List<Collider2D> targets = new List<Collider2D>();
            Physics2D.OverlapCircle(target.transform.position, 10, new ContactFilter2D(), targets);
            targets.Remove(target.GetComponent<Collider2D>());
            if (blackList.Count != 0)
            {
                for (int i = 0; i < blackList.Count; i++)
                {
                    targets.Remove(blackList[i]);
                }
            }

            if (targets.Count == 0)
            {
                target.GetComponent<HP>().ChangeHP(-damage, caster);
                StartCoroutine(GameManager.TakeDamageChangeColor(target.GetComponent<SpriteRenderer>()));
                break;
            }
            List<Collider2D> Enemies = new List<Collider2D>();
            Enemies.AddRange(targets.Where(x => x.tag == "Enemy"));
            if (Enemies.Count == 0)
            {
                target.GetComponent<HP>().ChangeHP(-damage, caster);
                StartCoroutine(GameManager.TakeDamageChangeColor(target.GetComponent<SpriteRenderer>()));
                break;
            }
            
            GameObject nearestTarget = Enemies.Where(x => Vector2.Distance(x.transform.position,
                target.transform.position) == Enemies.Min(y => Vector2.Distance(y.transform.position,
                target.transform.position))).FirstOrDefault().gameObject;
            
            blackList.Add(nearestTarget.GetComponent<Collider2D>());
            
            target.GetComponent<HP>().ChangeHP(-damage, caster);
            StartCoroutine(GameManager.TakeDamageChangeColor(target.GetComponent<SpriteRenderer>()));
            
            a.EndObject = nearestTarget;
            a.StartObject = target;

            target = nearestTarget;
        }
        
        Destroy(gameObject, 1);
        Destroy(LightBolt);
    }
}
