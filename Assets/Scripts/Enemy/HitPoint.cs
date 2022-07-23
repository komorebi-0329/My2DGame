using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    public bool bombChoose;
    public float hit=1;
    public float force;

    private int dir;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.position.x>other.transform.position.x)       
            dir = -1;
        else
            dir = 1;
        
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().GetHurt(hit);
            Debug.Log("attack成功");
        }
        if (other.CompareTag("Bomb")&& bombChoose)
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 0.8f)*force, ForceMode2D.Impulse);
            Debug.Log("skill成功");
        }
    }
}
