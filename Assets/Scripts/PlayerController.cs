using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    private int countJump;
    private Animator animation;
    internal float animSpeed = 1.0f;
    private float velocity;
    public float jumpUp;
    public float speedJump;
    public static float Popularity;
    public Slider[] slider;
    public float health = 100;
    public bool isAtatck;

    public AtackPlayer atackPlayer;

    private void Start()
    {
        Popularity = 15;
        health = 100;
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        //Cursor.visible = false;
        slider[0].value = Popularity;
        slider[1].value = health;
    }

    private void Update()
    {
        if (!Input.anyKey)
        {
            AnimSet(0);
        }

        Jump();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && countJump < 2)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            countJump++;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            isAtatck = true;
            AnimSet(3);
        }
        else
        {
            isAtatck = false;
        }
        velocity = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(velocity * speed, rb.velocity.y);

        if (velocity != 0)
        {
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
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            countJump = 0;
        }
    }


    public void AnimSet(int animNumber)
    {
        animation.SetInteger("State", animNumber);
    }

    public void Flip(float value)
    {
        AnimSet(1);
        if (value > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (value < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
