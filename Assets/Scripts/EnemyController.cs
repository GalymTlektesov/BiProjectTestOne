using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private new Animator animation;
    public float speedEnemy;
    public float distance = 7;
    private float maxDistance;
    private float minDistance;

    public Transform[] Eyes;
    private Vector3 eyes;
    public LayerMask[] mask;


    private float ifSeeThePlayer;
    
    private float direction;
    public bool isDanger;
    public float canDelay;
    internal float nextDelay;
    private float healthEnemy = 100;
    private Slider sliderEnemmy;
    public EnemyController friends;
    private PlayerController player;
    private Vector2 startPosition;


    public float minDelay;
    public float maxDelay;
    private float nextSpawn;
    private float motion;
    internal bool isDeath;


    private void Start()
    {
        isDeath = false;
        player = FindObjectOfType<PlayerController>();
        sliderEnemmy = GetComponentInChildren<Slider>();
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        eyes = Eyes[0].position;
        ifSeeThePlayer = Random.Range(30, 50);
        maxDistance = distance * 1.5f;
        minDistance = distance;
        Eyes[1].localPosition = new Vector2(Eyes[0].localPosition.x - distance / 2, Eyes[0].localPosition.y);
        sliderEnemmy.gameObject.SetActive(false);
        direction = transform.localScale.x > 0 ? 1 : -1;
        startPosition = transform.position;
        motion = Random.Range(0, 10);
        nextSpawn = Random.Range(minDelay, maxDelay);
    }

    private void Update()
    {
        if (isDeath)
        {
            return;
        }
        var ray = new Ray2D(eyes, new Vector2(direction, 0));
        var hit = Physics2D.Raycast(ray.origin, ray.direction, distance, mask[0]);
        isDanger = nextDelay > Time.time;
        if (hit == player || isDanger)
        {
            distance = player.Popularity > 25 || isDanger ? Watch() : Past();
        }
        if (hit != player && !isDanger && (friends == null || !friends.isDanger))
        {
            Past();
        }
    }


    private float Past()
    {
        ifSeeThePlayer = Random.Range(30, 50);
        distance = minDistance;
        eyes = Eyes[0].position;
        ifSeeThePlayer = Random.Range(30, 50);
        sliderEnemmy.gameObject.SetActive(false);
        if (Time.time > nextSpawn)
        {
            motion = Random.Range(0, 100);
            nextSpawn += Random.Range(minDelay, maxDelay);
        }
        return motion < 70 ? Idle() : Walk();
    }

    private float Walk()
    {
        var rayIsGround = new Ray(Eyes[2].position, Vector2.down);
        var hitIsGroundDown = Physics2D.Raycast(rayIsGround.origin, rayIsGround.direction, 2, mask[2]);
        var hitIsGroundUp = Physics2D.Raycast(rayIsGround.origin, -rayIsGround.direction, 2);

        AnimSet(1);
        if (Mathf.Abs(transform.position.x - startPosition.x) > 4.0f || !hitIsGroundDown ||  hitIsGroundUp)
        {
            Flip(startPosition.x - transform.position.x);
        }
        rb.velocity = new Vector2(direction * speedEnemy / 2, rb.velocity.y);

        return minDistance;
    }

    private float Idle()
    {
        AnimSet(0);
        return minDistance;
    }

    private float Watch()
    {
        eyes = Eyes[1].position;
        Flip(player.transform.position.x - transform.position.x);
        return player.Popularity > 50 || isDanger ? Move() : maxDistance;
    }

    private float Move()
    {
        nextDelay = canDelay + Time.time;
        HelpFriend();

        var rayIsGround = new Ray(Eyes[2].position, Vector2.down);
        var hitIsGroundDown = Physics2D.Raycast(rayIsGround.origin, rayIsGround.direction, 2, mask[2]);
        var hitIsGroundUp = Physics2D.Raycast(rayIsGround.origin, -rayIsGround.direction, 1, mask[2]);

        if (Mathf.Abs(player.transform.position.x - transform.position.x) > 1.7f && hitIsGroundDown && !hitIsGroundUp)
        {
            rb.velocity = new Vector2(direction * speedEnemy, rb.velocity.y);
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
        var friendCollider = Physics2D.OverlapCircle(Eyes[0].position, minDistance, mask[1]);

        if (friendCollider)
        {
            friends = friendCollider.GetComponent<EnemyController>();
            friends.nextDelay = nextDelay;
        }
    }

    private void AnimSet(int animNumber)
    {
        animation.SetInteger("State", animNumber);
    }

    private void Flip(float targetX)
    {
        direction = targetX / Mathf.Abs(targetX);
        transform.localScale = new Vector3(direction, transform.localScale.y, 1);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<AtackPlayer>())
        {
            var atackPlayer = collision.GetComponent<AtackPlayer>();
            sliderEnemmy.gameObject.SetActive(true);
            sliderEnemmy.value = healthEnemy;
            nextDelay = canDelay + Time.time;
            healthEnemy += atackPlayer.TakeDamage();
            if (healthEnemy <= 0)
            {
                sliderEnemmy.gameObject.SetActive(false);
                player.Popularity += Random.Range(3, 11);
                AnimSet(3);
                isDeath = true;
            }
        }
    }
}