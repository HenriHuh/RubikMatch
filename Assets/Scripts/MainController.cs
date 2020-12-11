using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public int gridSize;
    public GameObject nodePrefab;
    public Transform nodeParent;

    Vector3[] sideDirection = { Vector3.right, Vector3.left, Vector3.up, Vector3.down, Vector3.forward, Vector3.back };

    void Start()
    {
        InitCube();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") , -Input.GetAxis("Mouse X") * Time.deltaTime * 50));
        }
    }

    void InitCube()
    {
        List<Vector3> occupiedPositions = new List<Vector3>();


        foreach (Vector3 side in sideDirection)
        {
            for (int x = -gridSize; x <= gridSize; x++)
            {
                for (int y = -gridSize; y <= gridSize; y++)
                {
                    Vector3 pos = side * gridSize + new Vector3(0, x, y);
                    if (side == Vector3.up || side == Vector3.down)
                    {
                        pos = side * gridSize + new Vector3(x, 0, y);
                    }
                    else if (side == Vector3.forward || side == Vector3.back)
                    {
                        pos = side * gridSize + new Vector3(x, y, 0);
                    }
                    if (!occupiedPositions.Contains(pos))
                    {
                        GameObject g = Instantiate(nodePrefab, nodeParent);
                        g.transform.position = pos;
                        occupiedPositions.Add(pos);
                    }
                }
            }
        }
    }
}
