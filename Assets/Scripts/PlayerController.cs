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
            armature.animation.FadeIn("Idle", animSpeed);
        }
        if (Input.GetButtonDown("Horizontal"))
        {
            armature.animation.FadeIn("run", animSpeed);
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


        if (velocity < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (velocity > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            countJump = 0;
        }
    }
}
