using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D coll;

    public float startTime;
    public float waitTime;
    public Collider2D[] aroundObjects;

    [Header("Bomb State")]
    public float bombHit;
    public float radius;
    public float bombForce;
    public LayerMask targetLayer;

    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        coll=GetComponent<Collider2D>();
        rb=GetComponent<Rigidbody2D>(); 
        startTime= Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
        {
            if (Time.time > startTime + waitTime)
            {
                anim.Play("bomb_explotion");
            }
        }
    }

    public void Explotion()//animation event
    {

        coll.enabled = false;
        Collider2D[] aroundObjects =Physics2D.OverlapCircleAll(transform.position,radius,targetLayer);
        rb.gravityScale = 0;
        foreach (var item in aroundObjects)
        {
            Vector3 pos = transform.position - item.transform.position;
            item.GetComponent<Rigidbody2D>().AddForce((-pos+Vector3.up)*bombForce,ForceMode2D.Impulse);
            if (item.CompareTag("Bomb")&&item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
            {
                item.GetComponent<Bomb>().TurnOn();
            }
            if (item.CompareTag("Player")|| item.CompareTag("NPC"))
            {
                item.GetComponent<IDamageable>().GetHurt(bombHit);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void TurnOff()
    {
        anim.Play("bomb_off");
        gameObject.layer = LayerMask.NameToLayer("NPC");
    }

    public void TurnOn()
    {
        anim.Play("bomb_on");
        gameObject.layer = LayerMask.NameToLayer("Bomb");
        startTime = Time.time;
    }
}
