using UnityEngine;

public class RapierController : MonoBehaviour
{
    public float offset;
    public AirRapController rapierRef;
    public Transform shotPoint;
    private int numberOfButtonPresses = 0;

    public Transform trans;
    public Transform playerRapier;
    public Transform rapierTransform;
    public float distance;
    private GameObject rapier;
    internal static bool dropRapier;

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;


        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        rapierTransform.position = rapier ? rapier.transform.position : trans.position;

        if(Input.GetKeyDown(KeyCode.E) && !Input.GetButton("Fire2"))
        {
            if (numberOfButtonPresses % 2 == 0)
            {
                numberOfButtonPresses++;
                rapier = Instantiate(rapierRef.gameObject, shotPoint.position, transform.rotation);
                playerRapier.gameObject.SetActive(false);
                dropRapier = true;
            }
            else if (numberOfButtonPresses % 2 == 1)
            {
                numberOfButtonPresses++;
                trans.position = rapier.transform.position;
                playerRapier.gameObject.SetActive(true);
                Destroy(rapier);
                dropRapier = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && numberOfButtonPresses > 0 && rapier)
        {
            playerRapier.gameObject.SetActive(true);
            dropRapier = false;
            numberOfButtonPresses++;
            Destroy(rapier);
        } 
    }
}
