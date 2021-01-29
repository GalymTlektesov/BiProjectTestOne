using UnityEngine;

public class ThrowPlayer : MonoBehaviour
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

    public bool mouseHigherThanPlayer;


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
        mousePos = TargetPointValue(mousePos.x, mousePos.y);
        ray = mouseHigherThanPlayer ? new Ray2D(mousePos, Vector2.down) :
            new Ray2D(new Vector2(mousePos.x, target[2].position.y), Vector2.down);
        hit = Physics2D.Raycast(ray.origin, ray.direction, distance * 2, mask[0]);

        if (hit)
        {
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.black);
            if (Physics2D.Raycast(ray.origin, ray.direction, -0.5f, mask[1]))
            {
                RaycastHit2D hitWall = Physics2D.Raycast(rayHorizontal.origin, rayHorizontal.direction, distance, mask[0]);
                TargetRotate(new Vector2(hitWall.point.x, mousePos.y), 4.0f, 1.0f, 0);
            }
            else
            {
                TargetRotate(hit.point);
            }
            isJerk = true;
            Time.timeScale = 0.1f;
            Trajectory.ShowTrajectory(target[2].position, target[1].position);
        }
    }

    private Vector2 TargetPointValue(float xValue, float yValue)
    {
        var distancePosition = mouseHigherThanPlayer ? distance : -distance;
        xValue = xValue - transform.position.x > distance ?
                                    transform.position.x + distance :
                                    xValue;
        xValue = transform.position.x - xValue > distance ?
                                    transform.position.x - distance :
                                    xValue;
        yValue = yValue - target[2].position.y > distance ?
                                    target[2].position.y + distancePosition :
                                    yValue;
        yValue = target[2].position.y - yValue > distance ?
                                   target[2].position.y - distancePosition :
                                   yValue;
        var targetPoint = new Vector2(xValue, yValue);
        return targetPoint;
    }

    private void TargetRotate(Vector3 targetPosition, float zRotation = 1.0f, float wRotation = -1.0f, int index = 1)
    {
        target[index].rotation = Quaternion.identity;
        Quaternion rotation = Quaternion.LookRotation(transform.position - target[Mathf.Abs(index -1)].position,
            transform.TransformDirection(Vector3.up));
        target[0].rotation = new Quaternion(0, 0, rotation.z * zRotation, rotation.w * wRotation);
        target[0].position = targetPosition;
    }

    private void Flip(Vector3 mousePos)
    {
        var difference = mousePos.x - transform.position.x;
        playerController.Flip(difference);
        float direction = Mathf.Abs(playerController.scaleX / playerController.scaleX);

        rayHorizontal = new Ray2D(target[2].position, new Vector2(direction, 0));
        mouseHigherThanPlayer = mousePos.y > target[2].position.y;
    }
}