using UnityEngine;

public class RapierController : MonoBehaviour
{
    public float offset;
    public AirRapController rapierRef;
    public Transform shotPoint;
    private int numberOfButtonPresses = 0;

    public PlayerController prince;
    public Transform playerRapier;
    public Transform rapierTransform;
    public float distance;
    public float canDelay;
    private float nextDelay;
    private GameObject rapier;
    internal static bool dropRapier;

    void Update()
    {
        Vector3 difference = Input.GetJoystickNames() != null ? new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) :
            Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        bool canThrow = nextDelay < Time.time;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        rapierTransform.position = rapier ? rapier.transform.position : prince.transform.position;

        if((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button4)) && !Input.GetButton("Fire2") && prince.isLive)
        {
            if (numberOfButtonPresses % 2 == 0 && canThrow)
            {
                nextDelay = Time.time + canDelay;
                numberOfButtonPresses++;
                rapier = Instantiate(rapierRef.gameObject, shotPoint.position, transform.rotation);
                playerRapier.gameObject.SetActive(false);
                dropRapier = true;
            }
            else if (numberOfButtonPresses % 2 == 1)
            {
                numberOfButtonPresses++;
                prince.transform.position = rapier.transform.position;
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
