using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.EventSystems;
using System.Diagnostics;
using JetBrains.Annotations;
using Debug = UnityEngine.Debug;

public class Hero : Controller
{
    public static GameObject Player;
    public static GameObject CastPoint;

    [SerializeField] private GameObject click;
    [SerializeField] private GameObject firee;
    [SerializeField] private Transform clickContainer;
    [SerializeField] private float DashRange;
    [SerializeField] private GameObject shop;
    private HP hp;
    public static float Q_currentReload = 0,
                W_currentReload = 0,
                E_currentReload = 0,
                R_currentReload = 0,
                F_currentReload = 0;
    public static float Q_Reload = 3,
                W_Reload = 6,
                E_Reload = 9,
                R_Reload = 25,
                F_Reload = 3.5f;

    private void Awake()
    {
        Attack_reloadTime = 1.5f;
        Player = gameObject;
        CastPoint = Player.transform.GetChild(1).gameObject;
        MoveDelegate = Move;
        SetTargetDelegate = SetTarget;
        hp = GetComponent<HP>();
        characterAnimator = GetComponent<Animator>();
        characterSprite = GetComponent<SpriteRenderer>();
        PathFinder = GetComponent<PathFinder>();
        GameManager.Actors.Add(gameObject);
    }
    void Update()
    {
        Q_currentReload -= Time.deltaTime;
        W_currentReload -= Time.deltaTime;
        E_currentReload -= Time.deltaTime;
        R_currentReload -= Time.deltaTime;
        F_currentReload -= Time.deltaTime;
        Attack_currentReload -= Time.deltaTime;
        hp.ChangeMP(Time.deltaTime*3);
        if (hp.manaPoints > 100) hp.manaPoints = 100;

        if (Input.GetMouseButton(1)) StartCoroutine(ButtonClickMove());
        if (Input.GetKeyDown(KeyCode.Q)) StartCoroutine(QSpell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        if (Input.GetKeyDown(KeyCode.F)) FSpell();
        if (Input.GetKeyDown(KeyCode.W)) WSpell();
        if (Input.GetKeyDown(KeyCode.E)) StartCoroutine(ESpell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        if (Input.GetKeyDown(KeyCode.H)) OpenShop();
        if (Input.GetKeyDown(KeyCode.S)) StopMove();
        MoveDelegate?.Invoke();
    }

    private void OpenShop()
    {
        if (shop.activeSelf == false)
            GameManager.OpenShopInventory();
        else
            GameManager.CloseShopInventory();
    }
    private IEnumerator ButtonClickMove()
    {
        if (EventSystem.current.IsPointerOverGameObject()) yield break;
        if (Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 2f) != null)
        {
            GameObject aboba = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 2f).gameObject;
            if(aboba.tag == "Enemy")
            {
                if (Attack_currentReload > 0) yield break;
                else
                {
                    Attack_currentReload = Attack_reloadTime;
                    if(!RadiusCheck(aboba.transform.position, AttackRadius)){yield break;}
                    StopMove();
                    if (transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= 0)
                        FlipHero(true);
                    else
                        FlipHero(false);
                    characterAnimator.SetBool("Spell1", true);
                    yield return new WaitForSeconds(0.35f);
                    GameManager.CastFireball(aboba, CastPoint);
                    yield return new WaitForSeconds(0.4f);
                    characterAnimator.SetBool("Spell1", false);
                }
            }
            else
            {
                SetTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                GameObject a = Instantiate(click, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 10),
                    Quaternion.identity);
                Destroy(a, 0.2f);
            }
        }
        else
        {
            SetTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            GameObject a = Instantiate(click, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 10),
                Quaternion.identity, clickContainer);
            Destroy(a, 0.2f);
        }
    }
    private void StopMove()
    {
        PathToTarget.Clear();
        if(isMoving == true) OnEndMove();
    }
    override protected void SetTarget(Vector2 TargetPos)
    {
        if (Vector2.Distance(transform.position, TargetPos) < 1f)
            return;
        isMoving = true;
        characterAnimator.SetBool("IsMoving", true);
        if (!Physics2D.OverlapPoint(TargetPos, PathFinder.LayerToBlock))
        {
            _target = TargetPos;
        }
        else
        {
            Vector2 aa = TargetPos;
            _target = PathFinder.SearchNearFreeGamePanel(aa);
        }
        RaycastHit2D hit = Physics2D.Linecast(transform.position, _target, 1 << 6);
        if (hit.collider == null)
        {
            PathToTarget.Clear();
            PathToTarget.Add(_target);
        }
        else
            ReCalculatePath(_target);
    }
    override protected bool RadiusCheck(Vector2 target, float range)
    {
        if (Vector2.Distance(transform.position, target) <= range)
            return true;
        else
        {
            ReCalculatePath(target);
            RaycastHit2D hit = Physics2D.Linecast(transform.position, target, 1 << 6);  
            if (hit.collider != null)
            {
                _target = MinDist(target);
                SetTarget(_target);
            }
            else
            {
                PathToTarget.Clear();
                _target = Vector2.MoveTowards(transform.position, target,
                    Vector2.Distance(transform.position, target) - range);
                SetTarget(_target);
            }
            return false;
        }
    }
    private IEnumerator QSpell(Vector2 targetPos)
    {
        GameObject target = Physics2D.OverlapCircle(targetPos, 2f, 1 << 8)?.gameObject;
        if (target == null) yield break;
        if(!RadiusCheck(target.transform.position + Hero.Player.transform.position/40, AttackRadius)){yield break;}
        if (Q_currentReload > 0) 
            yield break;
        else 
            Q_currentReload = Q_Reload;
        
        if (transform.position.x - targetPos.x >= 0)
            FlipHero(true);
        else
            FlipHero(false);
        StopMove();
        characterAnimator.SetBool("Spell1", true);
        yield return new WaitForSeconds(0.35f);
        GameManager.CastLightBolt(target, gameObject, 30, CastPoint);
        yield return new WaitForSeconds(0.4f);
        characterAnimator.SetBool("Spell1", false);
    }
    private void WSpell()
    {
        if (W_currentReload > 0) return;
        else W_currentReload = W_Reload;
        GameManager.CastHeal(gameObject, 20);
    }
    private IEnumerator ESpell(Vector2 targetPos)
    {
        if(!RadiusCheck(targetPos, AttackRadius)){yield break;}
        if (E_currentReload > 0) 
            yield break;
        else 
            E_currentReload = E_Reload;

        if (transform.position.x - targetPos.x >= 0)
            FlipHero(true);
        else
            FlipHero(false);
        StopMove();
        characterAnimator.SetBool("Spell2", true);
        yield return new WaitForSeconds(0.7f);
        GameManager.CastExlosion(targetPos, gameObject, 40);
        yield return new WaitForSeconds(0.6f);
        Debug.Log("asfsd");
        characterAnimator.SetBool("Spell2", false);
    }
    private void FSpell()
    {
        if (F_currentReload > 0) return;
        else F_currentReload = F_Reload;
        if (transform.position.x < Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
            FlipHero(false);
        else
            FlipHero(true);
        SetTargetDelegate -= SetTarget;
        if(PathToTarget != null)PathToTarget.Clear();
        double s = Math.Atan2(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y, 
            Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x);
        Vector2 dir = new Vector2((float)(DashRange * Math.Cos(s)), (float)(DashRange * Math.Sin(s)));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, 1<<6);
        Debug.DrawLine(transform.position, hit.point);
        Debug.DrawLine(transform.position, dir + (Vector2)transform.position, Color.red);
        if (hit.collider != null)
        {
            if (Vector2.Distance(transform.position, hit.point) >= DashRange)
                _target = dir + (Vector2)transform.position;
            else
                _target = hit.point - (Vector2)transform.position / 30;
        }
        else
        {
            _target = dir + (Vector2)transform.position;
        }
        firee = Instantiate(Resources.Load<GameObject>("Prefab/FireParticle"));
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, dir);
        firee.transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 360);
        MoveDelegate += DashMove;
    }
    private void DashMove()
    {
        MoveDelegate -= Move;
        if (Vector2.Distance(transform.position, _target) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target, 70 * Time.deltaTime);
            firee.transform.position = Vector2.MoveTowards(transform.position, _target, 70 * Time.deltaTime);
        }
        else
        {
            OnEndMove();
            MoveDelegate += Move;
            MoveDelegate -= DashMove;
            SetTargetDelegate += SetTarget;
        }
    }
    private Vector2 MinDist(Vector2 target)
    {
        ReCalculatePath(target);
        List<Vector2> range = new List<Vector2>();
        range = PathToTarget.Where(pos => Vector2.Distance(pos, target) <= AttackRadius).ToList();
        Vector2 max = range[0];
        for(int i = 0; i < range.Count - 1; i++)
        {
            if(Vector2.Distance(max, _target) > Vector2.Distance(range[i], _target))
                max = range[i];
        }
        return max;
    }
    override protected void Move()
    {
        if (PathToTarget == null || PathToTarget.Count == 0)
        {
            isMoving = false;
            return;
        }
        if (Vector2.Distance(transform.position, PathToTarget[PathToTarget.Count - 1]) > 0.01f)
        {
            if (transform.position.x - PathToTarget[PathToTarget.Count - 1].x >= 0)
                FlipHero(true);
            else
                FlipHero(false);
            transform.position = Vector2.MoveTowards(transform.position, PathToTarget[PathToTarget.Count - 1], MoveSpeed * Time.deltaTime);
        }
        else
        {
            if (PathToTarget.Count == 1)
            {
                OnEndMove();
                isMoving = false;
                return;
            }
            ReCalculatePath(_target);
        }
    }
    private void OnEndMove()
    {
        characterAnimator.SetBool("IsMoving", false);
    }
    override protected void ReCalculatePath(Vector2 target)
    {
        PathToTarget = PathFinder.GetPath(target);
    }
    private void FlipHero(bool flip)
    {
        characterSprite.flipX = flip;
        if (flip)
            CastPoint.transform.localPosition = new Vector2(-1.45f, 2);
        else
            CastPoint.transform.localPosition = new Vector2(1.45f, 2);
    }
}