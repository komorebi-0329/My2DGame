using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldPirate : Enemy,IDamageable
{
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
}
