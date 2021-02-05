using UnityEngine;

public class CloakController : MonoBehaviour
{

    private Rigidbody2D rigidbody;
    private bool bb = false;
    public float jumpForce;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        rigidbody = GetComponentInParent<Rigidbody2D>();
        if (PlayerController.isGrounded == true)
        {
            bb = false;
        }

        if (rigidbody.velocity.y < 0 && Input.GetButtonDown("Jump") && Input.GetAxis("Vertical") < 0)
        {
            bb = true;
            rigidbody.drag = 0;
            rigidbody.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);
        }
        else if (!PlayerController.isGrounded && rigidbody.velocity.y < 0.5f && !bb)
        {
            rigidbody.drag = 15;
        }
    

        if (rigidbody.velocity.y > 0)
        {
            rigidbody.drag = 0;
        }
        
    }
}
