using UnityEngine;

public class AreaAtack : MonoBehaviour
{
    public Transform pointAtatck;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyController>())
        {
            pointAtatck.position = collision.transform.position;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyController>())
        {
            pointAtatck.localPosition = startPosition;
        }
    }
}
