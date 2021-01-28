using UnityEngine;

public class EyesEnemy : MonoBehaviour
{
    public bool isSeeThePlayer;
    public bool isDanger;
    public Vector2 playerPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            isSeeThePlayer = true;
            playerPosition = collision.transform.position;
        }
        else
        {
            isSeeThePlayer = false;
        }


        if (collision.GetComponent<EyesEnemy>())
        {
            var otherEyesEnemy = collision.GetComponent<EyesEnemy>();
            isDanger = otherEyesEnemy.isDanger;
        }
    }
}
