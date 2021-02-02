using UnityEngine;

public class AirRapController : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public float distance;
    public float damage;
    public LayerMask wtIsSolid;
    public Rigidbody2D rigidbody;
    private Vector2 startPosition;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.simulated = false;
        startPosition = transform.position;
    }

    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, wtIsSolid);
        if (hitInfo.collider != null)
        {
            return;
        }

        if (Vector2.Distance(startPosition, transform.position) > 7)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 180), 10.0f * Time.deltaTime);
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        //else
        //{
        //    rigidbody.simulated = true;
        //}
    }
}
