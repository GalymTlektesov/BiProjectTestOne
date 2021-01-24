using DragonBones;
using UnityEngine;
using UnityEngine.UI;

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
    public float Popularity;
    public Slider slider;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        //Cursor.visible = false;
    }

    private void Update()
    {
        slider.value = Popularity;
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
        velocity = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(velocity * speed, rb.velocity.y);

        if (velocity != 0)
        {
            Flip(velocity);
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
