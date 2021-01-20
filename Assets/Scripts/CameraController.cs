using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 distatnce;

    private void Start()
    {
       distatnce = transform.position - target.position;
    }

    void Update()
    {
        if (distatnce != transform.position - target.position)
        {
            transform.position = Vector3.Lerp(transform.position , distatnce + target.position, Time.timeScale);
        }
    }
}
