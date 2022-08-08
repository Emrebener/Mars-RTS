using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    private float panSpeed = 10f;
    private float panBorderThickness = 40f;

    void Update()
    {
        Vector2 cameraPosition = transform.position;

        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            cameraPosition.y += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            cameraPosition.y -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            cameraPosition.x += panSpeed * Time.deltaTime;

        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            cameraPosition.x -= panSpeed * Time.deltaTime;
        }
        transform.position = new Vector3(cameraPosition.x, cameraPosition.y, -300);
    }
}