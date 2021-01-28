using UnityEngine;

public class AreaAtack : MonoBehaviour
{
    public Transform pointAtatck;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyController>())
        {
            pointAtatck.position = collision.transform.position;
        }
    }
}
