using UnityEngine;

public class CameraVision : MonoBehaviour
{
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }


    private void OnBecameVisible()
    {
        sprite.enabled = true;
    }

    private void OnBecameInvisible()
    {
        sprite.enabled = false;
    }
}
