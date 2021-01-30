using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private new Animator animation;
    public float speed;
    public float distance = 7;
    private float maxDistance;

    public Transform[] Eyes;
    public LayerMask[] mask;
   
    public bool isDanger;

    public float health = 100;
    private Slider slider;

    private float ifSeeThePlayer;
    private Ray2D ray;
    internal RaycastHit2D hitPlayer;
    private RaycastHit2D hitEnemy;
    private bool noMe;
    private float direction;

    public EnemyController friend;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        ifSeeThePlayer = Random.Range(30, 50);
        maxDistance = distance * 2;
        Eyes[1].position = new Vector2(Eyes[0].position.x + distance, Eyes[0].position.y);
        Flip();
        VisibilityOverview(distance, Eyes[0].position);
        slider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (hitPlayer)
        {
            ifSeeThePlayer = 0;
            if (hitPlayer.collider.GetComponent<PlayerController>().Popularity > 25 || isDanger)
            {
                VisibilityOverview(maxDistance, Eyes[1].position);
                Flip();
                if (hitPlayer.collider.GetComponent<PlayerController>().Popularity > 50 || isDanger)
                {
                    hitEnemy = Physics2D.Raycast(Eyes[1].position, ray.direction, maxDistance, mask[1]);
                    noMe = hitEnemy.collider.GetComponent<EnemyController>() != gameObject.GetComponent<EnemyController>();
                    friend = noMe ? hitEnemy.collider.GetComponent<EnemyController>() : null;
                    if (friend != null) friend.isDanger = isDanger;

                    var rayIsGround = new Ray(Eyes[2].position, Vector2.down);
                    var hitIsGroundDown = Physics2D.Raycast(rayIsGround.origin, rayIsGround.direction, distance, mask[2]);
                    var hitIsGroundUp = Physics2D.Raycast(rayIsGround.origin, -rayIsGround.direction, distance, mask[3]);
                    
                    if (Mathf.Abs(hitPlayer.point.x - transform.position.x) > 1.0f && hitIsGroundDown && !hitIsGroundUp)
                    {
                        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
                        AnimSet(1);
                    }
                    else
                    {
                        AnimSet(2);
                    }
                }
            }
        }
        bool isFriendNoNull = friend != null;
        if (hitPlayer.collider == null && (!isFriendNoNull || !isDanger))
        {
            VisibilityOverview(distance, Eyes[0].position);
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
        var diference = hitPlayer.point.x - transform.position.x;
        direction = diference / Mathf.Abs(diference);
        transform.localScale = new Vector3(direction, transform.localScale.y, 1);
    }

    private void VisibilityOverview(float distanceRay, Vector2 eyes)
    {
        ray = new Ray2D(eyes, new Vector2(direction, 0));
        hitPlayer = Physics2D.Raycast(ray.origin, ray.direction, distanceRay, mask[0]);
        Debug.DrawRay(ray.origin, ray.direction * distanceRay, Color.red);
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
            if (health <= 0)
            {
                hitPlayer.collider.GetComponent<PlayerController>().Popularity += Random.Range(3, 11);
                hitPlayer.collider.GetComponent<PlayerController>().slider[0].value =
                    hitPlayer.collider.GetComponent<PlayerController>().Popularity;
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
