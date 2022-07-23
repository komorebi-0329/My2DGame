using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBaseState currentState;
    private GameObject alarmSign;

    public Animator anim;
    public int animState;

    [Header("Enemy State")]
    public float Hp;
    public float speed;
    public bool hasBomb;
    public bool isBoss;
    public bool isDead;

    [Header("Setting")]  
    public Transform pointA;
    public Transform pointB;
    public Transform targetPoint;
    
    [Header("Attack Setting")]
    public float attackCD;
    public float attackRange, skillRange;
    private float nextAttck = 0;


    public List<Transform> attackList = new List<Transform>();

    public PatrolState patrolState =new PatrolState();
    public AttackState attackState = new AttackState();


    public virtual void Init()
    { 
        anim=GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;      
    }

    public void Awake()
    {
        Init();
    }

    void Start()
    {
        GameManager.instance.IsEnemy(this);
        if (isBoss)
            UIManager.instance.SetBossHealth(Hp);         

        TransitionToState(patrolState);
    }

    public virtual void Update()
    {
        anim.SetBool("dead", isDead);

        if(isBoss)
        UIManager.instance.UpdateBossHealth(Hp);

        if (isBoss & isDead)
        {            
            UIManager.instance.bossHealthBar.SetActive(false);
            UIManager.instance.PlayOverMenu.SetActive(true);
        }

        if (isDead)
        {
            GameManager.instance.EnemyIsDead(this);          
            return;
        }

        currentState.OnUpdate(this);
        anim.SetInteger("state", animState);    
                         
    }

    public void TransitionToState(EnemyBaseState state)//转变状态
    {
        currentState=state;
        currentState.EnterState(this);
    }

    public void MoveToTarget()  //移动到目标点
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FilpDirection();
    }

    public void AttackAction()//普通攻击
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time> nextAttck)
            {
                anim.SetTrigger("attack");
                Debug.Log("调用attack动画");
                nextAttck = Time.time+attackCD;
            }
        }
    }

    public virtual void SkillAction()//技能
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAttck)
            {
                anim.SetTrigger("skill");
                Debug.Log("调用skill动画");
                nextAttck = Time.time + attackCD;
            }
        }
    }

    public void FilpDirection()//模型翻转
    {
        if (transform.position.x < targetPoint.position.x)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }
    public void SwitchPoint()//判断巡逻点
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
            targetPoint = pointA;
        else
            targetPoint = pointB;
    }

    public void OnTriggerStay2D(Collider2D collision) //添加攻击列表
    {
        if (!attackList.Contains(collision.transform)&&!hasBomb&&!isDead&&!GameManager.instance.gameOver)
        attackList.Add(collision.transform);
    }

    public void OnTriggerExit2D(Collider2D collision) //移出攻击列表
    {
        attackList.Remove(collision.transform);
    }

    public void OnTriggerEnter2D(Collider2D collision) //发现玩家标识
    {
        if (!isDead && !GameManager.instance.gameOver)
            StartCoroutine(OnAlarm());  
    }

    IEnumerator OnAlarm() //协程 同时播放警示标识
    { 
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);
    }

    public void EnemyDestroy()//Animator Event 
    {
        Destroy(gameObject);
    }
}
