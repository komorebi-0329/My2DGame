using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    public PlayerController player;

    private void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        player=FindObjectOfType<PlayerController>();

        GameManager.instance.IsExitDoor(this);

        coll.enabled = false;
    }

    public void OpendDoor() //gameManager µ÷ÓÃ
    {
        anim.Play("opend");
        coll.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&!player.isDead)
        {
            Debug.Log("Player go to the nextroom");
            GameManager.instance.SaveData();          
            GameManager.instance.NextScene();           
        }
    }
}
