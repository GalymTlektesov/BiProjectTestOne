using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public Transform target;
    private Vector3 distatnce;

    private void Start()
    {
       distatnce = transform.position - target.position;
    }

    private void Update()
    {
        if (distatnce != transform.position - target.position)
        {
            transform.position = Vector3.Lerp(transform.position , distatnce + target.position, Time.timeScale);
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
