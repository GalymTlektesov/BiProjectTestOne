using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private Light light;

    private void Start()
    {
        light = GetComponentInChildren<Light>();
        light.enabled = false;
    }

    private void OnBecameVisible()
    {
        light.enabled = true;
    }

    private void OnBecameInvisible()
    {
        light.enabled = false;
    }
}
