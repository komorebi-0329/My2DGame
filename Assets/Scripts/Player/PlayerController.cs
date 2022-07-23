using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
    private Rigidbody2D rb;
    private Animator anim;
    private FixedJoystick joystick;

    [Header("Player State")]
    public float HP;
    public float speed;    
    public float jumpForce;

    [Header("Ground Check")] //��������
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    
    [Header("States Check")]//���״̬
    public bool isGround;
    public bool isJumping;
    public bool isHurt;
    public bool isDead;
    public bool canJump;

    [Header("VFX")]//��ɫ�ƶ�Ч��
    public GameObject jumpFX;
    public GameObject landFX;

    [Header("Attack Settings")]
    public GameObject bombPrefad;
    public float nextAttack=0;
    public float attackRate;

    // Start Unity��ʼʱִ��һ��
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        anim= GetComponent<Animator>();
        joystick=FindObjectOfType<FixedJoystick>();

        GameManager.instance.IsPlayer(this);

        HP = GameManager.instance.LoadHealth();
        UIManager.instance.UpdateHealth(HP);
    }

    // Update ÿִ֡��һ�� һ�㲶׽��������
    void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
            return;

        if (!isGround)
            rb.gravityScale = 5;

        isHurt = anim.GetCurrentAnimatorStateInfo(1).IsName("Hit");
        CheckInput();   
    }

    //FixedUpdate 1sִ��50   һ��ִ�������йصĺ���
    public void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        PhysicsCheck();
        
        if (!isHurt)
        {
            Movement();
            Jump();
        }
        
    }

    //��ⰴ��
    void CheckInput()
    {
        if (Input.GetButtonDown("Jump")&&isGround)
        {           
            canJump=true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    //��ɫ�ƶ�
    public void Movement()
    {
        /*//PC����
        float horizontalinput = Input.GetAxisRaw("Horizontal");//��Χ-1~1 ��������С��˲���ٶȣ�
        //float horizontalinput = Input.GetAxis("Horizontal");//��Χ-1~1 ������С��������ǰ��*/
        

        //�ֻ����ݸ�
        float horizontalinput = joystick.Horizontal;

        rb.velocity =new Vector2 (horizontalinput*speed,rb.velocity.y);

        /*//PC����
        if (horizontalinput != 0)
            transform.localScale = new Vector3(horizontalinput, 1, 1);*/
       
        if (horizontalinput > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if (horizontalinput < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    //��ɫ��Ծ
    public void Jump()
    {
        if (canJump)
        {           
            isJumping = true;
            jumpFX.SetActive(true);
            jumpFX.transform.position= transform.position+new Vector3(0,-0.45f,0);
            rb.velocity=new Vector2 (rb.velocity.x,jumpForce);                       
            canJump =false;
        }
    }

    //��ɫ��Ծ�����ݸˣ�
    public void IphoneJump()
    {
        if (isGround)
        {
            canJump = true;
            isJumping = true;
            jumpFX.SetActive(true);
            jumpFX.transform.position = transform.position + new Vector3(0, -0.45f, 0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canJump = false;
        }
    }

    public void Attack()
    {
        if (Time.time > nextAttack)
        {
            Instantiate(bombPrefad, transform.position, bombPrefad.transform.rotation);
            nextAttack = Time.time+ attackRate;
        }
    }

    //������
    void PhysicsCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGround)
        {
            isJumping = false;
            rb.gravityScale = 1;
        }
    }

    //Animation Event
    public void LandFX()
    { 
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.75f, 0);
    }
    //�Դ��Ļ��ߺ���
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    public void GetHurt(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Player01_Hit"))
        {
            HP -= damage;
            if (HP < 1)
            {
                HP = 0;
                isDead = true;
            }
            anim.SetTrigger("hit");

            UIManager.instance.UpdateHealth(HP);
        }
    }
}
