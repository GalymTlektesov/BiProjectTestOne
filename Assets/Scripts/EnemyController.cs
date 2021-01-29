using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private new Animator animation;
    public float speed;
    public float distance = 7;

    public Transform Eyes;
    public Transform alertEyes;
    public LayerMask[] mask;
    
    private float maxDistance;
    public bool isDanger;
    public float ifSeeThePlayer;

    public float health = 100;
    private Slider slider;

    private Ray2D ray;
    private RaycastHit2D hit;
    private float direction;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        ifSeeThePlayer = Random.Range(30, 50);
        maxDistance = distance * 2;
        alertEyes.position = new Vector2(Eyes.position.x + distance, Eyes.position.y);
        Flip();
        VisibilityOverview(distance, Eyes.position);
    }

    private void Update()
    {
        if (hit)
        {
            ifSeeThePlayer = 0;
            if (PlayerController.Popularity > 25 || isDanger)
            {
                VisibilityOverview(maxDistance, alertEyes.position);
                Flip();
                if (PlayerController.Popularity > 50 || isDanger)
                {
                    if (Physics2D.Raycast(alertEyes.position, ray.direction, maxDistance, mask[1]).collider.GetComponent<EnemyController>() != gameObject.GetComponent<EnemyController>())
                    {
                        Physics2D.Raycast(alertEyes.position, ray.direction, maxDistance, mask[1]).collider.GetComponent<EnemyController>().isDanger = true;
                    }

                    if (Mathf.Abs(hit.point.x - transform.position.x) > 1.5f)
                    {
                        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
                        AnimSet(1);
                    }
                    else if (Mathf.Abs(hit.point.x - transform.position.x) < 0.1f)
                    {
                        rb.velocity = new Vector2(-direction * speed, rb.velocity.y);
                        AnimSet(1);
                    }
                    else
                    {
                        AnimSet(2);
                    }
                }
            }
        }
        if(hit.collider == null)
        {
            VisibilityOverview(distance, Eyes.position);
            Random.Range(30, 50);
            AnimSet(0);
            isDanger = false;
            slider.gameObject.SetActive(false);
        }
    }

    private void AnimSet(int animNumber)
    {
        animation.SetInteger("State", animNumber);
    }

    private void Flip()
    {
        var diference = hit.point.x - transform.position.x;
        direction = diference / Mathf.Abs(diference);
        transform.localScale = new Vector3(direction, transform.localScale.y, 1);
    }

    private void VisibilityOverview(float distanceRay, Vector2 eyes)
    {
        ray = new Ray2D(eyes, new Vector2(direction, 0));
        hit = Physics2D.Raycast(ray.origin, ray.direction, distanceRay, mask[0]);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<AtackPlayer>())
        {
            var atackPlayer = collision.GetComponent<AtackPlayer>();
            slider.gameObject.SetActive(true);
            slider.value = health;
            isDanger = true;
            health += atackPlayer.TakeDamage() - ifSeeThePlayer;
            Debug.Log(ifSeeThePlayer);
            if (health <= 0)
            {
                PlayerController.Popularity += Random.Range(3, 11);
                Destroy(gameObject);
            }
        }
    }


    private void OnBecameVisible()
    {
        animation.enabled = true;
    }

    private void OnBecameInvisible()
    {
        animation.enabled = false;
    }
}
