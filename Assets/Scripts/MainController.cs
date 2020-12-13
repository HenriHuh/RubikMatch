using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public int gridSize;
    public GameObject[] nodePrefabs;
    public Transform nodeParent;
    public Transform cameraTarget;

    //Static objects to be used globally
    public static Vector3[] sideDirection = { Vector3.right, Vector3.left, Vector3.up, Vector3.down, Vector3.forward, Vector3.back };
    public static List<GameObject> allObjects = new List<GameObject>();


    public static MainController instance;

    void Awake()
    {
        InitCube();
        instance = this;
        CheckMatch.CheckAllNodes();
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
                    if (occupiedPositions.Find(occupiedPos => HelpFunctions.V3Equal(pos, occupiedPos)) == Vector3.zero)
                    {
                        GameObject g = Instantiate(nodePrefabs[Random.Range(0, 5)], nodeParent);
                        g.transform.position = pos;
                        occupiedPositions.Add(pos);
                        allObjects.Add(g);
                    }
                }
            }
        }
    }

    public void NewNode(Vector3 pos)
    {
        GameObject g = Instantiate(nodePrefabs[Random.Range(0, 5)], nodeParent);
        pos.x = (int)pos.x;
        pos.y = (int)pos.y;
        pos.z = (int)pos.z;
        g.transform.position = pos;
        allObjects.Add(g);
        CheckMatch.CheckAdjacentNodes(g);
    }
}
