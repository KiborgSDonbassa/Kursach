using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Archer : Controller
{
    [SerializeField] private bool MoveActivate;
    [SerializeField] private float ViewRange;
    private GameManager.VoidEvent _cast;
    void Start()
    {
        _cast += () => GameManager.ShootArrow(Hero.Player, gameObject);
        GameManager.Actors.Add(gameObject);
        MoveDelegate += Move;
        PathFinder = GetComponent<PathFinder>();
    }
    void Update()
    {
        Attack_currentReload -= Time.deltaTime;
        
        if (Vector2.Distance(transform.position, Hero.Player.transform.position) <= ViewRange)
            MoveActivate = true;
        else
            MoveActivate = false;
        if (Vector2.Distance(transform.position, Hero.Player.transform.position) <= AttackRadius)
            MoveActivate = false;

        if (Vector2.Distance(transform.position, Hero.Player.transform.position) <= AttackRadius)
        {
            if(Attack_currentReload <= 0)
            {
                Debug.Log(123);
                StartCoroutine(GameManager.Timer(_cast, 1));
                Attack_currentReload = Attack_reloadTime;
            }
        }
        
        if (MoveActivate)
        {
            RadiusCheck(Hero.Player.transform.position, AttackRadius);
            MoveDelegate?.Invoke();
        }
    }
    override protected void Move()
    {
        if (PathToTarget == null || PathToTarget.Count == 0) return;
        
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
    override protected void SetTarget(Vector2 target) { }
    override protected bool RadiusCheck(Vector2 target, float range)
    {
        if (Vector2.Distance(transform.position, target) <= range)
        {
            return true;
        }
        else
        {
            ReCalculatePath(target);
            RaycastHit2D hit = Physics2D.Linecast(transform.position, target, 1 << 6);
            _target = MinDist(target, range);
            
            return false;
        }
    }
    private Vector2 MinDist(Vector2 target, float range_)
    {
        ReCalculatePath(target);
        List<Vector2> range = new List<Vector2>();
        range = PathToTarget.Where(pos => Vector2.Distance(pos, target) <= range_).ToList();
        Vector2 min = range[0];
        for(int i = 0; i < range.Count - 1; i++)
        {
            if(Vector2.Distance(min, _target) > Vector2.Distance(range[i], _target))
            {
                min = range[i];
            }
        }
        return min;
    }
    override protected void ReCalculatePath(Vector2 target)
    {
        PathToTarget = PathFinder.GetPath(target);
    }
    
}
