using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

abstract public class Controller : MonoBehaviour
{
    public float AttackRadius;
    [SerializeField] protected float Attack_currentReload = 0;
    [SerializeField] protected float Attack_reloadTime;
    [SerializeField] protected bool isMoving = false;
    [SerializeField] protected float MoveSpeed;
    public List<Vector2> PathToTarget = new List<Vector2>();
    protected Vector2 _target;
    protected PathFinder PathFinder;
    protected SpriteRenderer characterSprite;
    protected Animator characterAnimator;
    
    protected GameManager.VoidTargetEvent SetTargetDelegate;
    protected GameManager.VoidEvent MoveDelegate;
    
    abstract protected void Move();
    abstract protected void SetTarget(Vector2 TargetPoss);
    abstract protected void ReCalculatePath(Vector2 target);
    abstract protected bool RadiusCheck(Vector2 target, float range);
}

abstract public class UsableObjects : MonoBehaviour
{
    public bool Enabled = true;
    
    abstract protected void Activate();
}

public interface SpellCaster 
{
    public void Activate();
}

abstract public class Spell : MonoBehaviour
{
    protected string Name;
    protected Sprite icon;
    protected int damage = 10;
    public int manaCost = 10;
    protected float bulletSpeed;
    protected GameObject target;
    protected GameObject caster;
    protected Vector2 targetPos;
    protected GameObject bulletParticle;
    
    protected GameManager.VoidEvent activateDelegate;
    
    public void initialize(GameObject _target)
    {
        target = _target;
    }
    public void initialize(Vector2 _targetPos)
    {
        targetPos = _targetPos;
    }
    public void initialize(GameObject _target, GameObject _caster)
    {
        target = _target;
        caster = _caster;
    }
}
