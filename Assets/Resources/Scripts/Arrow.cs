using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Spell, SpellCaster
{
    public void Activate()
    {
        damage = 20;
        bulletSpeed = 20;
        activateDelegate += BulletMove;
    }
    void Update()
    {
        activateDelegate?.Invoke();
    }
    protected void BulletMove()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        if (Vector2.Distance(transform.position, target.transform.position) <= 0.1f)
        {
            target.GetComponent<HP>().ChangeHP(-damage, caster);
            Destroy(gameObject);
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, bulletSpeed * Time.deltaTime);
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, target.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 360);
    }
}
