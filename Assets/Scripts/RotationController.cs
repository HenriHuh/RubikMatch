using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    Matrix4x4 rot;
    int rotTimeLeft;
    List<GameObject> filteredObjects = new List<GameObject>();
    private int x;
    private int y;
    private int z;
    private Vector2 mousePos;
    private Vector3 hitPos;
    private bool isChecked = true;

    Matrix4x4 rotX(float angle)
    {
        float ca = Mathf.Cos(angle);
        float sa = Mathf.Sin(angle);

        return new Matrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, ca, -sa, 0),
            new Vector4(0, sa, ca, 0),
            new Vector4(0, 0, 0, 1)
        );
    }
    Matrix4x4 rotY(float angle)
    {
        float ca = Mathf.Cos(angle);
        float sa = Mathf.Sin(angle);

        return new Matrix4x4(
            new Vector4(ca, 0, sa, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(-sa, 0, ca, 0),
            new Vector4(0, 0, 0, 1)
        );
    }

    Matrix4x4 rotZ(float angle)
    {
        float ca = Mathf.Cos(angle);
        float sa = Mathf.Sin(angle);

        return new Matrix4x4(
            new Vector4(ca, -sa, 0, 0),
            new Vector4(sa, ca, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 1)
        );
    }

    Vector3 getGeneralDirection(Vector3 v)
    {
        Vector3[] compass3D = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back, Vector3.up, Vector3.down };
        Vector3 generalDirection = Vector3.zero;
        float maxDot = 0;
        foreach (Vector3 dir in compass3D)
        {
            float dotProd = Vector3.Dot(dir, v);
            if (dotProd > maxDot)
            {
                generalDirection = dir;
                maxDot = dotProd;
            }
        }

        return generalDirection;
    }

    void Rotate(int x, int y, int z, Vector3 dir)
    {
        int animMultiplier = 200;
        float deg90 = 2 * Mathf.PI / 4 / animMultiplier;
        if (dir.x == 1)
        {
            rot = rotX(deg90);
        }
        if (dir.x == -1)
        {
            rot = rotX(-deg90);
        }
        if (dir.y == 1)
        {
            rot = rotY(deg90);
        }
        if (dir.y == -1)
        {
            rot = rotY(-deg90);
        }
        if (dir.z == 1)
        {
            rot = rotZ(deg90);
        }
        if (dir.z == -1)
        {
            rot = rotZ(-deg90);
        }
        rotTimeLeft = animMultiplier;
        isChecked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotTimeLeft > 0)
        {
            foreach(GameObject go in filteredObjects) 
            {
                go.transform.position = rot * go.transform.position;
            }
            rotTimeLeft--;
        }
        else if(rotTimeLeft == 0 && !isChecked)
        {
            CheckMatch.CheckAllNodes();
            isChecked = true;
        }
        else if(CheckMatch.nodesToBeDeleted.Count == 0)
        { 
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
            
                if(Physics.Raycast(ray, out hit, 100))
                {
                    Vector3 pos = hit.transform.position;
                    hitPos = hit.point;
                    x = Mathf.RoundToInt(pos.x);
                    y = Mathf.RoundToInt(pos.y);
                    z = Mathf.RoundToInt(pos.z);

                    mousePos = Input.mousePosition;
                }
                else
                {
                    x = 0;
                    y = 0;
                    z = 0;
                }
            }

            if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                Vector2 newMousePos = Input.mousePosition;
                if(Vector2.Distance(newMousePos, mousePos) > 32) {
                    if (x != 0 || y != 0 || z != 0)
                    {
                        Vector3 dragDir = Camera.main.transform.TransformDirection(newMousePos - mousePos);
                        Vector3 hitDir = getGeneralDirection(hitPos);
                        Vector3 axis = Vector3.Cross(dragDir, hitDir);
                        axis = getGeneralDirection(axis);
                        
                        Rotate(x, y, z, axis);

                        filteredObjects.Clear();
                        float E = 0.1f;
                        foreach (GameObject go in MainController.allObjects)
                        {
                            if(axis.x != 0 && Mathf.Abs(go.transform.position.x - x) < E)
                            {
                                filteredObjects.Add(go);
                            }
                            else if (axis.y != 0 && Mathf.Abs(go.transform.position.y - y) < E)
                            {
                                filteredObjects.Add(go);
                            }
                            else if (axis.z != 0 && Mathf.Abs(go.transform.position.z - z) < E)
                            {
                                filteredObjects.Add(go);
                            }
                        }
                    }
                }
            }
        }
    }
}
