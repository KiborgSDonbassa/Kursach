using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class івап : Controller
{
    public bool MoveActivate;
    public float ViewRange;
    
    private void Start()
    {
        GameManager.Actors.Add(gameObject);
        MoveDelegate += Move;
        PathFinder = GetComponent<PathFinder>();
        Attack_reloadTime = 2;
        ReCalculatePath(_target);
    }
    private void Update()
    {
        Attack_currentReload -= Time.deltaTime;
        if (Vector2.Distance(transform.position, Hero.Player.transform.position) <= ViewRange)
            MoveActivate = true;
        else
            MoveActivate = false;
        if (Vector2.Distance(transform.position, Hero.Player.transform.position) <= 2f)
            MoveActivate = false;

        if (Vector2.Distance(transform.position, Hero.Player.transform.position) <= AttackRadius)
        {
            if(Attack_currentReload <= 0)
            { 
                Hero.Player.GetComponent<HP>().ChangeHP(-10, gameObject);
                StartCoroutine(GameManager.TakeDamageChangeColor(Hero.Player.GetComponent<SpriteRenderer>()));
                Attack_currentReload = Attack_reloadTime;
            }
        }
        if(MoveActivate)MoveDelegate?.Invoke();
    }

    private void OnDestroy()
    {
        Achives.OnKill();
    }

    override protected void Move()
    {
        if (PathToTarget == null || PathToTarget.Count == 0)
        {
            ReCalculatePath(_target);
        }
        
        if (Vector2.Distance(transform.position, PathToTarget[PathToTarget.Count - 1]) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, PathToTarget[PathToTarget.Count - 1],
                MoveSpeed * Time.deltaTime);
        }
        else
        {
            ReCalculatePath(_target);
        }
    }
    override protected void SetTarget(Vector2 TargetPoss){}
    override protected bool RadiusCheck(Vector2 target, float range){return true;}
    override protected void ReCalculatePath(Vector2 target)
    {
        PathToTarget = PathFinder.GetPath(Hero.Player.transform.position);
    }
}
