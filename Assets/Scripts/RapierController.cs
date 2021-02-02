using UnityEngine;

public class RapierController : MonoBehaviour
{
    public float offset;
    public GameObject rapierRef;
    public Transform shotPoint;
    private int numberOfButtonPresses = 0;

    public Transform trans;
    public Transform cursor;
    public float distance;
    private GameObject rapier;


    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;


        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (numberOfButtonPresses % 2 == 0)
            {
                numberOfButtonPresses++;
                rapier = Instantiate(rapierRef, shotPoint.position, transform.rotation);
            }
            else if (numberOfButtonPresses % 2 == 1)
            {
                numberOfButtonPresses++;
                trans.position = rapier.transform.position;
                Destroy(rapier);
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && numberOfButtonPresses > 0 && rapier)
        {
            numberOfButtonPresses++;
            Destroy(rapier);
        } 
    }
}
