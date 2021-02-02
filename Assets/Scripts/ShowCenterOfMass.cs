using UnityEngine;

public class ShowCenterOfMass : MonoBehaviour
{
    public Vector2 centerOfMass;
    private Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(new Vector2(1, 0) * 40, ForceMode2D.Force);
    }

    void Update()
    {
        rigidbody.centerOfMass = centerOfMass;
    }
}
