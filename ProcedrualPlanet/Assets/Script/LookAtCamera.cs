using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToTarget = 10;

    private Vector3 previousPosition;
    private void Awake() {
        cam = Camera.main;
    }
    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // forward
        {
            distanceToTarget = Mathf.Clamp(distanceToTarget - 0.5f, 2f, 30f);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // backwards
        {
            distanceToTarget = Mathf.Clamp(distanceToTarget + 0.5f, 2f, 30f);
        }
        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) || Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = previousPosition - newPosition;

            float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
            float rotationAroundXAxis = direction.y * 180; // camera moves vertically

            cam.transform.position = target.position;
            
            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);
            
            cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

            previousPosition = newPosition;
        }
    }
}
