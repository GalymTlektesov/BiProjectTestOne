using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private new Animator animation;
    public float speed;
    public float distance = 7;
    private float maxDistance;
    private float minDistance;

    public Transform[] Eyes;
    private Vector3 eyes;
    public LayerMask[] mask;


    private float ifSeeThePlayer;
    
    private bool noMe;
    private float direction;
    private bool isDanger;
    private float health = 100;
    private Slider slider;
    private EnemyController friend;
    private PlayerController player;


    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        slider = GetComponentInChildren<Slider>();
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        eyes = Eyes[0].position;
        ifSeeThePlayer = Random.Range(30, 50);
        maxDistance = distance * 2;
        minDistance = distance;
        Eyes[1].position = new Vector2(Eyes[0].position.x + distance, Eyes[0].position.y);
        slider.gameObject.SetActive(false);
        Flip();
    }

    private void Update()
    {
        var ray = new Ray2D(eyes, new Vector2(direction, 0));
        var hit = Physics2D.Raycast(ray.origin, ray.direction, distance, mask[0]);

        if (hit == player)
        {
            distance = player.Popularity > 25 || isDanger ? Watch(hit) : Past();
        }
        if (hit != player && (friend == null || !friend.isDanger))
        {
            distance = minDistance;
            eyes = Eyes[0].position;
            ifSeeThePlayer = Random.Range(30, 50);
            AnimSet(0);
            isDanger = false;
            slider.gameObject.SetActive(false);
        }
    }


    private float Past()
    {
        ifSeeThePlayer = 0;
        return minDistance;
    }

    private float Watch(RaycastHit2D hit)
    {
        eyes = Eyes[1].position;
        Flip();
        return player.Popularity > 50 || isDanger ? Move(hit) : maxDistance;
    }

    private float Move(RaycastHit2D hit)
    {
        HelpFriend();

        var rayIsGround = new Ray(Eyes[2].position, Vector2.down);
        var hitIsGroundDown = Physics2D.Raycast(rayIsGround.origin, rayIsGround.direction, distance, mask[2]);
        var hitIsGroundUp = Physics2D.Raycast(rayIsGround.origin, -rayIsGround.direction, distance, mask[3]);

        if (Mathf.Abs(hit.point.x - transform.position.x) > 1.0f && hitIsGroundDown && !hitIsGroundUp)
        {
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            AnimSet(1);
        }
        else
        {
            AnimSet(2);
        }

        return maxDistance;
    }

    private void HelpFriend()
    {
        var ray = new Ray2D(Eyes[1].position, new Vector2(direction, 0));
        var hitEnemy = Physics2D.Raycast(Eyes[1].position, ray.direction, maxDistance, mask[1]);
        noMe = hitEnemy.collider.GetComponent<EnemyController>() != gameObject.GetComponent<EnemyController>();
        friend = noMe ? hitEnemy.collider.GetComponent<EnemyController>() : null;
        if (friend != null) friend.isDanger = true;
    }

    private void AnimSet(int animNumber)
    {
        animation.SetInteger("State", animNumber);
    }

    private void Flip()
    {
        var diference = player.transform.position.x - transform.position.x;
        direction = diference / Mathf.Abs(diference);
        transform.localScale = new Vector3(direction, transform.localScale.y, 1);
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
                player.Popularity += Random.Range(3, 11);
                Destroy(gameObject);
            }
        }
    }
}