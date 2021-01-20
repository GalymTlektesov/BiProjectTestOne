using UnityEngine;

public class TrajectoryRender : MonoBehaviour
{
    private LineRenderer LineRenderer;

    void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();    
    }

    public void ShowTrajectory(Vector2 origin, Vector2 endPoint)
    {
        LineRenderer.SetPosition(0, origin);
        LineRenderer.SetPosition(1, endPoint);
    }
}
