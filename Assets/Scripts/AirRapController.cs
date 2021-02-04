using UnityEngine;

public class AirRapController : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public float distance;
    public float damage;
    public LayerMask wtIsSolid;
    private Vector2 startPosition;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        startPosition = transform.position;
    }

    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, wtIsSolid);
        if (hitInfo.collider != null)
        {
            boxCollider.enabled = false;
            return;
        }

        if (Vector2.Distance(startPosition, transform.position) > 7)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 180), 10.0f * Time.deltaTime);
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
