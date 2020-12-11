using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public int sensitivity;

    Vector3 rotation = new Vector3();
    bool cameraRotating;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 50))
        {
            cameraRotating = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            cameraRotating = false;
        }
        if (cameraRotating)
        {
            rotation.x += Input.GetAxis("Mouse X") * sensitivity;
            rotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        }

        Quaternion rotationQuat = Quaternion.Euler(rotation.y, rotation.x, 0);
        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, rotationQuat, Time.deltaTime * sensitivity);
    }
}
