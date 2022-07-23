using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain : Enemy, IDamageable
{
    SpriteRenderer sprite;
    public float escapeSpeed;

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

    public override void Init()
    {
        base.Init();
        sprite = GetComponent<SpriteRenderer>();    
    }

    public override void Update()
    {
        base.Update();
        if (animState==0)
            sprite.flipX = false;
    }
    public override void SkillAction()
    {
        base.SkillAction();
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("skill"))
        {
            sprite.flipX = true;
            if (transform.position.x > targetPoint.position.x)
            {      
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.right, speed * escapeSpeed * Time.deltaTime);
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left, speed * escapeSpeed * Time.deltaTime);
        }
        else
            sprite.flipX = false;
    }
}
