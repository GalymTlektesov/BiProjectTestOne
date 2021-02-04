using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public Transform rapier;
    public float speed;
    internal Vector3 offset;
    private Vector3 pointTarget;
    private float sizeCamera;
    internal bool isCameraVision;
    private float distancePlayerOfRapier;
    private Vector3 vel;
    private float velocity;

    private Camera camera;

    private void Start()
    {
        isCameraVision = true;
        camera = Camera.main;
        sizeCamera = camera.orthographicSize;
        offset = transform.position - player.transform.position;
    }

    private void Update()
    {
        float width = camera.pixelWidth;
        float height = camera.pixelHeight;
        Vector3 midPoint = (player.transform.position + rapier.position) / 2;

        Vector2 bottomLeft = camera.ScreenToWorldPoint(new Vector2(0, 0));
        //Vector2 bottomRight = camera.ScreenToWorldPoint(new Vector2(width, 0));
        //Vector2 topLeft = camera.ScreenToWorldPoint(new Vector2(0, height));
        Vector2 topRight = camera.ScreenToWorldPoint(new Vector2(width, height));

        pointTarget = Vector3.Lerp(pointTarget, midPoint, speed);
        isCameraVision = Mathf.Abs(player.transform.position.x - rapier.position.x) < Mathf.Abs(bottomLeft.x - topRight.x) - 5.5f
            && Mathf.Abs(player.transform.position.y - rapier.position.y) < Mathf.Abs(bottomLeft.y - topRight.y) - 3.5f;


        if (!isCameraVision)
        {
            //pointTarget = Vector3.Lerp(pointTarget, player.transform.position, speed);

            camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, camera.orthographicSize + 0.1f, ref velocity, speed);
            distancePlayerOfRapier = Mathf.Abs(player.transform.position.x - rapier.position.x);
        }
        else if (Mathf.Abs(player.transform.position.x - rapier.position.x) < distancePlayerOfRapier && camera.orthographicSize >= sizeCamera)
        {
            camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, camera.orthographicSize - 0.1f, ref velocity, speed);
        }
        //else
        //{
        //    pointTarget = Vector3.Lerp(pointTarget, midPoint, speed);
        //}
        if (offset != transform.position - pointTarget)
        {
            transform.position = Vector3.SmoothDamp(transform.position, pointTarget + offset, ref vel, speed);
        }
    }


    public void Replay()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
