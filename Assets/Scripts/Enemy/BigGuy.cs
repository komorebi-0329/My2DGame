using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuy : Enemy, IDamageable
{
    public Transform pickupPoint;
    public float power;

    public void GetHurt(float damage)
    {
        Hp -= damage;
        if (Hp < 1)
        {
            Hp = 0;
            isDead = true;
        }
        anim.SetTrigger("hit");
    }

    public void PickupBomb() //animator Event
    {
        if (targetPoint.CompareTag("Bomb")&&!hasBomb)
        {
            targetPoint.gameObject.transform.position = pickupPoint.position;
            targetPoint.SetParent(pickupPoint);
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            hasBomb= true;
        }
    }

    public void ThrowAway() //animator Event
    {
        if (hasBomb&&!isDead)
        {
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            targetPoint.SetParent(pickupPoint.parent.parent);

            if (FindObjectOfType<PlayerController>().gameObject.transform.position.x - transform.position.x < 0)
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 0.7f) * power, ForceMode2D.Impulse);
            else
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 0.7f) * power, ForceMode2D.Impulse);
        }
        hasBomb = false;
    }

}
