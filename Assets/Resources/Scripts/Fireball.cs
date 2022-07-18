using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell, SpellCaster
{
    public void Activate()
    {
        Name = "Fireball";
        damage = 30;
        bulletSpeed = 20;
        bulletParticle = Instantiate(Resources.Load<GameObject>("Prefab/FireParticle"), transform.position, Quaternion.identity);
        activateDelegate += BulletMove;
    }
    void Update()
    {
        activateDelegate?.Invoke();
    }
    void OnDestroy()
    {
        Destroy(bulletParticle);
        if(target != null) target.GetComponent<HP>().ChangeHP(-damage, caster);
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
            Destroy(gameObject);
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, bulletSpeed * Time.deltaTime);
        if(bulletParticle != null)bulletParticle.transform.position = Vector2.MoveTowards(transform.position, target.transform.position, bulletSpeed * Time.deltaTime);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, target.transform.position);
        if(bulletParticle != null)bulletParticle.transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 360);
    }
}
