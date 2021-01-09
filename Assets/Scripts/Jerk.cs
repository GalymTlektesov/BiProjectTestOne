using UnityEngine;

public class Jerk : MonoBehaviour
{
    public Transform[] target;
    public float distance = 10.0f;
    public LayerMask[] mask;
    public TrajectoryRender Trajectory;
    
    private bool isJerk = false;
    private PlayerController playerController;
    
    public float shortDelay = 1.5f;
    private float nextDelay = 0;
    private bool canDealay;

    Ray2D rayHorizontal = new Ray2D();

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
            if (Input.GetButtonDown("Fire2")) //TODO Исправить падения
            {
                playerController.armature.animation.FadeIn("JerkOne");
            }
            if (Input.GetButton("Fire2"))
            {
                InJerk(mousePos);
            }

        }
        if (Input.GetButtonUp("Fire2") && Mathf.Abs(target[0].position.x - transform.position.x) > 0.1f)
        {
            AfterJerk();
        }
        if (Mathf.Abs(nextDelay - Time.time) < shortDelay - 0.15f && canDealay && !Input.GetButton("Horizontal"))
        {
           playerController.armature.animation.FadeIn("Idle", playerController.animSpeed);
        }
        else if (Mathf.Abs(nextDelay - Time.time) < 0.03f && canDealay && Input.GetButton("Horizontal"))
        {
            playerController.armature.animation.FadeIn("run", playerController.animSpeed);
        }
        if (target[0].position != transform.position && !isJerk)
        {
            target[0].position = transform.position;
        }
    }

    private void AfterJerk()
    {
        nextDelay = Time.time + shortDelay;
        isJerk = false;
        Trajectory.gameObject.SetActive(false);
        transform.position = new Vector2(target[0].position.x, target[0].position.y + transform.localScale.y);
        target[0].position = transform.position;
        target[0].gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        playerController.armature.animation.FadeIn("JerkTwo");
    }



    private void InJerk(Vector3 mousePos)
    {
        Ray2D ray = new Ray2D(mousePos, Vector2.down);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, distance, mask[0]);

        if (Physics2D.Raycast(ray.origin, -ray.direction, 0.5f, mask[1]))
        {
            RaycastHit2D hitMouse = Physics2D.Raycast(rayHorizontal.origin, rayHorizontal.direction, distance, mask[1]);
            target[0].rotation = Quaternion.identity;
            Quaternion rotation = Quaternion.LookRotation(transform.position - target[1].position, transform.TransformDirection(Vector3.up));
            target[0].rotation = new Quaternion(0, 0, rotation.z * 4, rotation.w);

            target[0].position = new Vector2(hitMouse.point.x, mousePos.y);
        }
        else if (hit)
        {
            target[1].rotation = Quaternion.identity;
            Quaternion rotation = Quaternion.LookRotation(transform.position - target[0].position, transform.TransformDirection(Vector3.up));
            target[0].rotation = new Quaternion(0, 0, rotation.z, -rotation.w);
            target[0].position = hit.point;
        }
        else
        {
            return;
        }

        isJerk = true;
        Time.timeScale = 0.1f;

        target[0].gameObject.SetActive(true);
        Trajectory.gameObject.SetActive(true);

        //Flip
        if (mousePos.x < transform.position.x)
        {
            rayHorizontal = new Ray2D(transform.position, Vector2.left);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            rayHorizontal = new Ray2D(transform.position, Vector2.right);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }


        Trajectory.ShowTrajectory(target[2].position, target[1].position);
    }
}