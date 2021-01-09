using UnityEngine;

public class TrajectoryRender : MonoBehaviour
{
    private LineRenderer LineRenderer;

    void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();    
    }

    public void ShowTrajectory(Vector3 origin, Vector3 endPoint)
    {
        LineRenderer.SetPosition(0, origin);
        LineRenderer.SetPosition(1, endPoint);
    }
}
