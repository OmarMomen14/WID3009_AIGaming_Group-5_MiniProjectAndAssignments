/// Small controller to the camera to make it adjust to different screen width
/// as well as resizes.
///
/// source: https://gamedev.stackexchange.com/questions/167317/scale-camera-to-fit-screen-size-unity

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class MatchWidth : MonoBehaviour
{
    public float horizontalFoV = 90.0f;

    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        var halfWidth = Mathf.Tan(0.5f * horizontalFoV * Mathf.Deg2Rad);
        var halfHeight = halfWidth * Screen.height / Screen.width;
        var verticalFoV = 2.0f * Mathf.Atan(halfHeight) * Mathf.Rad2Deg;

        _camera.fieldOfView = verticalFoV;
    }
}