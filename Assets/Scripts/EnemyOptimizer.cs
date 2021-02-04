using UnityEngine;

public class EnemyOptimizer : MonoBehaviour
{
    private EnemyController enemy;
    private Animator animation;

    private void Start()
    {
        enemy = GetComponent<EnemyController>();
        animation = GetComponent<Animator>();
    }



    private void OnBecameVisible()
    {
        enemy.enabled = true;
        animation.enabled = true;
    }

    private void OnBecameInvisible()
    {
        if (enemy.isDeath)
        {
            Destroy(gameObject);
        }
        enemy.enabled = false;
        animation.enabled = false;
    }
}
