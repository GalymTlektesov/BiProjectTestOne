using DragonBones;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    private int countJump;
    internal UnityArmatureComponent armature;
    public float animSpeed;
    private float velocity;
    public float jumpUp;
    public float speedJump;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        armature = GetComponent<UnityArmatureComponent>();
        //Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetButtonUp("Horizontal") && !Input.GetButton("Horizontal"))
        {
            AnimSet("Idle", animSpeed);
        }
        if (Input.GetButtonDown("Horizontal"))
        {
            AnimSet("run", animSpeed);
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


        Flip(velocity);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            countJump = 0;
        }
    }


    public void AnimSet(string animName, float speed)
    {
        armature.animation.FadeIn(animName, speed);
    }

    public void Flip(float value)
    {
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
