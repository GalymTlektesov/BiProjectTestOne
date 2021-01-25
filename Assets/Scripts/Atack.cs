using UnityEngine;

public class Atack : MonoBehaviour
{
    [SerializeField]
    private float atack;

    public float TakeDamage()
    {
        return -atack;
    }
}
