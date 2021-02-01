﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;

    public static PlayerController player;
    public float Popularity;
    public Slider[] slider;
    public float health = 100;
    public Animator panelAnimation;

    internal Rigidbody2D rb;
    internal float scaleX;

    private AreaAtack areaAtack;
    private Animator animation;
    private float velocity;
    internal bool isLive;


    [Header("Jump")]
    public float jumpForce;
    public float jumpTime;
    public static bool isJumping;
    public static bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    private float jumpTimeCounter;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        isLive = true;
        scaleX = transform.localScale.x;
        areaAtack = GetComponentInChildren<AreaAtack>();
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        //Cursor.visible = false;
        slider[0].value = Popularity;
        slider[1].value = health;
        panelAnimation.SetBool("PlayerDeath", false);
    }

    private void Update()
    {
        if (!Input.anyKey && isLive)
        {
            AnimSet(0);
        }

        Jump();
    }

    private void FixedUpdate()
    {
        if (!isLive)
        {
            return;
        }
        if (Input.GetButton("Fire1"))
        {
            Flip(areaAtack.pointAtatck.position.x - transform.position.x);
            AnimSet(3);
        }
        velocity = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(velocity * speed, rb.velocity.y);

        if (velocity != 0)
        {
            AnimSet(1);
            Flip(velocity);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<AtackEnemy>())
        {
            var atackEnemy = collision.GetComponent<AtackEnemy>();
            health += atackEnemy.TakeDamage();
            slider[0].value = Popularity;
            slider[1].value = health;
            if (health <= 0)
            {
                StartCoroutine(Death());
            }
        }
    }

    private void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);


        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }


    public void AnimSet(int animNumber)
    {
        animation.SetInteger("State", animNumber);
    }

    public void Flip(float value)
    {
        transform.localScale = new Vector3(scaleX * (value / Mathf.Abs(value)), transform.localScale.y, 1);
    }



    private IEnumerator Death()
    {
        isLive = false;
        AnimSet(4);
        yield return new WaitForSeconds(2.5f);
        panelAnimation.SetBool("PlayerDeath", true);
    }


    public static void PopularityPlus(float value)
    {
        player.Popularity += value;
        player.slider[0].value = player.Popularity;
    }
}
