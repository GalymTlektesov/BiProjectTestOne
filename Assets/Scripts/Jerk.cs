using UnityEngine;

public class Jerk : MonoBehaviour
{
    public Transform target;
    public Transform targetUp;
    public Transform swordPosition;
    public float distance = 10.0f;
    public LayerMask maskFloor;
    public LayerMask maskWall;
    public TrajectoryRender Trajectory;
    private bool isJerk = false;
    private PlayerController playerController;
    public float shortDelay = 1.5f;
    private float nextDelay = 0;
    private bool canDealay;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        canDealay = nextDelay < Time.time;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(transform.position, mousePos) <= distance && canDealay)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                playerController.armature.animation.FadeIn("JerkOne");
            }
            if (Input.GetButton("Fire2"))
            {
                isJerk = true;
                Time.timeScale = 0.1f;

                //Physics2D.queriesStartInColliders = false;

                Ray2D ray = new Ray2D(mousePos, Vector2.down);

                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, distance, maskFloor);
                target.gameObject.SetActive(true);
                Trajectory.gameObject.SetActive(true);
                Ray2D rayHorizontal = new Ray2D();
                RaycastHit2D hitMouse;
                if (hit.point.x < transform.position.x)
                {
                    rayHorizontal = new Ray2D(transform.position, Vector2.left);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    rayHorizontal = new Ray2D(transform.position, Vector2.right);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                hitMouse = Physics2D.Raycast(rayHorizontal.origin, rayHorizontal.direction, distance, maskWall);

                if (hitMouse && Physics2D.Raycast(ray.origin, -ray.direction, 0.5f, maskWall))
                {
                    target.rotation = Quaternion.identity;
                    Quaternion rotation = Quaternion.LookRotation(transform.position - targetUp.position, transform.TransformDirection(Vector3.up));
                    target.rotation = new Quaternion(0, 0, rotation.z * 4, rotation.w);

                    target.position = new Vector2(hitMouse.point.x, mousePos.y);
                }
                else if (hit)
                {
                    targetUp.rotation = Quaternion.identity;
                    Quaternion rotation = Quaternion.LookRotation(transform.position - target.position, transform.TransformDirection(Vector3.up));
                    target.rotation = new Quaternion(0, 0, rotation.z, -rotation.w);
                    target.position = hit.point;
                }

                Trajectory.ShowTrajectory(swordPosition.position, targetUp.position, new Vector3(0.1f, 0.1f, 0.1f));
            }

        }
        if (Input.GetButtonUp("Fire2") && Mathf.Abs(target.position.x - transform.position.x) > 0.1f)
        {
            nextDelay = Time.time + shortDelay;
            isJerk = false;
            Trajectory.gameObject.SetActive(false);
            transform.position = new Vector2(target.position.x, target.position.y + transform.localScale.y);
            target.position = transform.position;
            target.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
            playerController.armature.animation.FadeIn("JerkTwo");
        }
        if (Mathf.Abs(nextDelay - Time.time) < shortDelay - 0.15f && canDealay && !Input.GetButton("Horizontal"))
        {
           playerController.armature.animation.FadeIn("Idle", playerController.animSpeed);
        }
        else if (Mathf.Abs(nextDelay - Time.time) < 0.03f && canDealay && Input.GetButton("Horizontal"))
        {
            playerController.armature.animation.FadeIn("run", playerController.animSpeed);
        }
        if (target.position != transform.position && !isJerk)
        {
            target.position = transform.position;
        }
    }


}