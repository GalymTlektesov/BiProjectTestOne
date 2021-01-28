using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animation;
    public float speed;
    public float distance = 7;

    public Transform Eyes;
    public Transform alertEyes;   
    public LayerMask[] mask;

    private bool isAlert;
    private float maxDistance;
    public bool isDanger;
    public bool isSeeThePlayer;

    public float health = 100;
    private Slider slider;
    private Canvas canvas;

    Ray2D ray;
    RaycastHit2D hit;
    Vector2 direction;

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        slider = GetComponentInChildren<Slider>();
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        maxDistance = distance * 2;
        canvas.gameObject.SetActive(false);
        alertEyes.position = new Vector2(Eyes.position.x + distance, Eyes.position.y);
        Flip();
    }

    
    void FixedUpdate()
    {
        //animation.enabled = CameraViewEnemy();
        if (isAlert)
        {
            VisibilityOverview(maxDistance, alertEyes.position);
        }
        else
        {
            VisibilityOverview(distance, Eyes.position);
        }

        if (hit)
        {
            isSeeThePlayer = true;
            if (PlayerController.Popularity > 25 || isDanger)
            {
                isAlert = true;
                Flip();
                if (PlayerController.Popularity > 50 || isDanger)
                {
                    if (Physics2D.Raycast(alertEyes.position, ray.direction, maxDistance, mask[1]).collider.GetComponent<EnemyController>() != gameObject.GetComponent<EnemyController>())
                    {
                        StartCoroutine(Physics2D.Raycast(alertEyes.position, ray.direction, maxDistance, mask[1]).collider.GetComponent<EnemyController>().DangerForm(2));
                    }

                    if (Mathf.Abs(hit.point.x - transform.position.x) > 1.5f)
                    {
                        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
                        AnimSet(1);
                    }
                    else
                    {
                        AnimSet(2);
                    }
                }
            }
        }
    }

    private void AnimSet(int animNumber)
    {
        animation.SetInteger("State", animNumber);
    }

    private void Flip()
    {
        if (hit.point.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, 1);
            direction = Vector2.right;
        }
        else
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, 1);
            direction = Vector2.left;
        }
    }

    private void VisibilityOverview(float distanceRay, Vector2 eyes)
    {
        ray = new Ray2D(eyes, direction);
        hit = Physics2D.Raycast(ray.origin, ray.direction, distanceRay, mask[0]);
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<AtackPlayer>())
        {
            var atackPlayer = collision.GetComponent<AtackPlayer>();
            slider.value = health;
            StartCoroutine(DangerForm(3));
            health += !isSeeThePlayer ? atackPlayer.TakeDamage() - Random.Range(30, 50) : atackPlayer.TakeDamage();
            if (health <= 0)
            {
                PlayerController.Popularity += Random.Range(3, 11);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Camera>())
        {
            animation.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Camera>())
        {
            animation.enabled = false;
        }
    }


    private bool CameraViewEnemy()
    {
        var origin = new Vector2(alertEyes.position.x + distance, alertEyes.position.y);
        var rayViewEnemy = new Ray2D(origin, direction);
        var hitViewEnemy = Physics2D.Raycast(rayViewEnemy.origin, rayViewEnemy.direction, maxDistance * 2, mask[0]);
        Debug.DrawRay(rayViewEnemy.origin, rayViewEnemy.direction * (maxDistance * 2));

        if (hitViewEnemy.collider.GetComponent<PlayerController>())
        {
            return false;
        }
        else
        {
            return true;
        }
    }



    public IEnumerator DangerForm(float time)
    {
        isDanger = true;
        canvas.gameObject.SetActive(true);

        yield return new WaitForSeconds(time);

        AnimSet(0);
        isDanger = false;
        canvas.gameObject.SetActive(false);
        isSeeThePlayer = false;
        isAlert = false;
        animation.enabled = false;
        VisibilityOverview(distance, Eyes.position);
    }
}
