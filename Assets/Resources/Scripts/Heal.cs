using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Spell, SpellCaster
{
    public void Activate()
    {
        caster.GetComponent<HP>().ChangeHP(30);
        Destroy(gameObject);
    }
}
