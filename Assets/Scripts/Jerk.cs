using UnityEngine;

public class Jerk : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public LayerMask mask;
 
    private void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            Time.timeScale = 0.2f;
            var mousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //var mousePos = new Vector2(mousePos3.x, mousePos3.y);

            //Physics2D.queriesStartInColliders = false;
            //RaycastHit2D hitMouse = Physics2D.Raycast(transform.position, mousePos, distance, mask);
            //target.gameObject.SetActive(true);


            //if (hitMouse.collider == null)
            //{
            //    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.down, distance, mask);
            //    target.position = hit.point;
            //}
            //else
            //{
            //    target.position = hitMouse.point;

            Ray2D rayMouse = new Ray2D(transform.position, new Vector2(mousePos3.x, mousePos3.y));
            Debug.DrawRay(rayMouse.origin, rayMouse.direction * distance, Color.black);
            Physics2D.queriesStartInColliders = false;
            RaycastHit2D hitMouse = Physics2D.Raycast(rayMouse.origin, rayMouse.direction, distance, mask);
            target.gameObject.SetActive(true);

            if (Physics2D.Raycast(rayMouse.origin, rayMouse.direction, distance, mask))
            {
                target.position = hitMouse.point;
            }
        }
        if (Input.GetButtonUp("Fire2"))
        {
            transform.position = new Vector2(target.position.x, target.position.y + 1.0f);
            target.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }


}
