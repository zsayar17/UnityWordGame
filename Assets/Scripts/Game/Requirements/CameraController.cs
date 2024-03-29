using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 resolution;
    public Vector2 beginResolution;
    float beginOrthographicSize;

    private void Awake()
    {
        beginOrthographicSize = Camera.main.orthographicSize;
        RepositionCamera();
        beginResolution = resolution;
    }

    private void Update()
    {
        if (beginResolution.x != Screen.width || beginResolution.y != Screen.height)
            RepositionCamera();

    }

    private void RepositionCamera()
    {
        Camera camera;

        float ratioX = Screen.width / resolution.x;
        float ratioY = Screen.height / resolution.y;
        camera = Camera.main;
        camera.orthographicSize = beginOrthographicSize * (ratioY / ratioX);
        if (camera.orthographicSize < beginOrthographicSize)
            camera.orthographicSize = beginOrthographicSize;
        beginResolution.x = Screen.width;
        beginResolution.y = Screen.height;
    }
}
