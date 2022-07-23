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

    [Header("Ground Check")] //监测点设置
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    
    [Header("States Check")]//监测状态
    public bool isGround;
    public bool isJumping;
    public bool isHurt;
    public bool isDead;
    public bool canJump;

    [Header("VFX")]//角色移动效果
    public GameObject jumpFX;
    public GameObject landFX;

    [Header("Attack Settings")]
    public GameObject bombPrefad;
    public float nextAttack=0;
    public float attackRate;

    // Start Unity开始时执行一次
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        anim= GetComponent<Animator>();
        joystick=FindObjectOfType<FixedJoystick>();

        GameManager.instance.IsPlayer(this);

        HP = GameManager.instance.LoadHealth();
        UIManager.instance.UpdateHealth(HP);
    }

    // Update 每帧执行一次 一般捕捉按键输入
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

    //FixedUpdate 1s执行50   一般执行物理有关的函数
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

    //监测按键
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

    //角色移动
    public void Movement()
    {
        /*//PC按键
        float horizontalinput = Input.GetAxisRaw("Horizontal");//范围-1~1 （不包括小数瞬间速度）
        //float horizontalinput = Input.GetAxis("Horizontal");//范围-1~1 （包括小数减缓向前）*/
        

        //手机操纵杆
        float horizontalinput = joystick.Horizontal;

        rb.velocity =new Vector2 (horizontalinput*speed,rb.velocity.y);

        /*//PC按键
        if (horizontalinput != 0)
            transform.localScale = new Vector3(horizontalinput, 1, 1);*/
       
        if (horizontalinput > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if (horizontalinput < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    //角色跳跃
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

    //角色跳跃（操纵杆）
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

    //物理监测
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
    //自带的画线函数
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
