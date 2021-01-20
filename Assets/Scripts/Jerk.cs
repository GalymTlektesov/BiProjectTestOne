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

    private bool mouseHigherThanPlayer;


    Ray2D ray;
    RaycastHit2D hit;
    Ray2D rayHorizontal;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        canDealay = nextDelay < Time.time;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        target[0].gameObject.SetActive(isJerk);
        if (canDealay && Input.GetButton("Fire2"))
        {
            playerController.AnimSet(2);
            Throw(mousePos);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            AfterThrow();
        }
        target[0].position = target[0].position != transform.position && !isJerk ? transform.position : target[0].position;
    }

    private void AfterThrow()
    {
        nextDelay = Time.time + shortDelay;
        isJerk = false;
        transform.position = new Vector2(target[0].position.x, target[0].position.y + transform.localScale.y);
        target[0].position = transform.position;
        Time.timeScale = 1.0f;
    }



    private void Throw(Vector3 mousePos)
    {
        Flip(mousePos);
        ray = new Ray2D(mousePos, Vector2.down);
        hit = Physics2D.Raycast(ray.origin, ray.direction, distance, mask[0]);
        isJerk = true;

        if (hit)
        {
            RaycastHit2D hitWall = Physics2D.Raycast(ray.origin, -ray.direction, 0.5f, mask[1]);
            if (hitWall)
            {
                if (!mouseHigherThanPlayer)
                {
                    RaycastHit2D hitDown = Physics2D.Raycast(new Vector2(mousePos.x, transform.position.y), Vector2.down, distance, mask[0]);
                    //hitDown.point = TargetPointValue(hitDown.point.x, hitDown.point.y);
                    //PointBoundaries(hitDown.point);
                    if (hitDown)
                    {
                        if (Mathf.Abs(transform.position.x - hitDown.point.x) > distance || Mathf.Abs(transform.position.y - hitDown.point.y) > distance) { return; }
                        else { TargetRotate(hitDown.point, 1.0f, -1.0f, 1); }
                    }
                }
                else
                {
                    RaycastHit2D hitMouse = Physics2D.Raycast(rayHorizontal.origin, rayHorizontal.direction, distance, mask[0]);
                    //hitMouse.point = TargetPointValue(hitMouse.point.x, mousePos.y);
                    //PointBoundaries(hitMouse.point);
                    if (hitMouse)
                    {
                        if (Mathf.Abs(transform.position.x - hitMouse.point.x) > distance || Mathf.Abs(transform.position.y - hitMouse.point.y) > distance) { return; }
                        else { TargetRotate(new Vector2(hitMouse.point.x, mousePos.y), 4.0f, 1.0f, 0); }
                    }
                }
            }
            else
            {
                //var targetPoint = TargetPointValue(hit.point.x, hit.point.y);
                //PointBoundaries(hit.point);
                if (Mathf.Abs(transform.position.x - hit.point.x) > distance || Mathf.Abs(transform.position.y - hit.point.y) > distance) { return; }
                else { TargetRotate(hit.point, 1.0f, -1.0f, 1); }
            }
        }
        else
        {
            return;
        }

        Time.timeScale = 0.1f;
        Trajectory.ShowTrajectory(target[2].position, target[1].position);
    }

    private Vector2 TargetPointValue(float xValue, float yValue)
    {
        var distancePosition = mouseHigherThanPlayer ? distance : -distance;
        xValue = transform.position.x - xValue > distance ?
                                    transform.position.x - distance :
                                    xValue;
        xValue = transform.position.x + xValue > distance ?
                                    transform.position.x + distance :
                                    xValue;
        yValue = transform.position.y - yValue > distance ?
                                    transform.position.y - distancePosition :
                                    yValue;
        yValue = transform.position.y - yValue > distance ?
                                   transform.position.y - distancePosition :
                                   yValue;
        var targetPoint = new Vector2(xValue, yValue);
        return targetPoint;
    }

    private void PointBoundaries(Vector2 pointValue)
    {
        if (Mathf.Abs(transform.position.x - pointValue.x) > distance || Mathf.Abs(transform.position.y - pointValue.y) > distance)
        {
            return;
        }
    }

    private void TargetRotate(Vector3 targetPosition, float zRotation, float wRotation, int index)
    {
        target[index].rotation = Quaternion.identity;
        Quaternion rotation = Quaternion.LookRotation(transform.position - target[Mathf.Abs(index -1)].position, transform.TransformDirection(Vector3.up));
        target[0].rotation = new Quaternion(0, 0, rotation.z * zRotation, rotation.w * wRotation);
        target[0].position = targetPosition;
    }

    private void Flip(Vector3 mousePos)
    {
        var direction = new Vector2();
        if (mousePos.x < transform.position.x)
        {
            direction = Vector2.left;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            direction = Vector2.right;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        rayHorizontal = new Ray2D(target[2].position, Vector2.left);

        mouseHigherThanPlayer = mousePos.y < transform.position.y && Mathf.Abs(transform.position.y - mousePos.y) > 1.5f ?
            false : true;
    }
}