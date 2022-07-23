using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : Enemy, IDamageable
{
    public float scale;
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

    public void Swalow()//animation Event
    {
        if (transform.lossyScale.y > 1.5)
        {
            isDead = true;
            Debug.Log("≥≈À¿");
        }

        targetPoint.GetComponent<Bomb>().TurnOff();
        targetPoint.gameObject.SetActive(false);
        
        transform.localScale *= scale;
    }
}
