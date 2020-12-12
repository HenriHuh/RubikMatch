using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMatch : MonoBehaviour
{
    static Vector3[] sides = { Vector3.right, Vector3.up, Vector3.forward };
    static List<GameObject> nodesToCheck = new List<GameObject>();
    public static List<GameObject> nodesToBeDeleted = new List<GameObject>();
    const float maxNodeDeletionTime = 1;
    static float nodeDeletionTime = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if (nodeDeletionTime > 0)
        {
            nodeDeletionTime -= Time.deltaTime;
            foreach (GameObject node in nodesToBeDeleted)
            {
                node.GetComponentInChildren<Renderer>().material.SetFloat("_Metallic", maxNodeDeletionTime - nodeDeletionTime);
            }
        }
        else
        {
            for (int i = nodesToBeDeleted.Count - 1; i >= 0; i--)
            {
                GameObject node = nodesToBeDeleted[i];
                Vector3 pos = node.transform.position;
                Destroy(node);
                MainController.instance.NewNode(pos);
            }
            nodesToBeDeleted.Clear();
        }
    }

    public static void CheckAllNodes()
    {
        foreach (GameObject g in MainController.allObjects)
        {
            nodesToCheck.Add(g);
        }
        while (nodesToCheck.Count > 0)
        {
            CheckAdjacentNodes(nodesToCheck[0]);
            nodesToCheck.RemoveAt(0);
        }
    }

    public static void CheckAdjacentNodes(GameObject node)
    {
        if (node == null) return;

        List<GameObject> currentMatches = new List<GameObject>();
        List<GameObject> totalMatches   = new List<GameObject>();

        Vector3 currentPosition = node.transform.position;

        foreach (Vector3 vec in sides) //Check diagonal of X, Y, Z
        {
            //Positive vector
            while (nodesToCheck.Find(o => ComparePos(o.transform.position, currentPosition + vec) && o.name == node.name))
            {
                currentMatches.Add(nodesToCheck.Find(o => ComparePos(o.transform.position, currentPosition + vec) && o.name == node.name));

                currentPosition += vec;
            }

            //Negative vector
            currentPosition = node.transform.position;

            while (nodesToCheck.Find(o => ComparePos(o.transform.position, currentPosition - vec) && o.name == node.name))
            {
                currentMatches.Add(nodesToCheck.Find(o => ComparePos(o.transform.position, currentPosition - vec) && o.name == node.name));
                currentPosition -= vec;
            }

            if (currentMatches.Count >= 2)
            {
                totalMatches.AddRange(currentMatches);
            }
            currentMatches.Clear();
            currentPosition = node.transform.position;
        }

        if (totalMatches.Count > 0)
        {
            MainController.allObjects.Remove(node);
            nodesToBeDeleted.Add(node);
        }
        foreach (GameObject match in totalMatches)
        {
            MainController.allObjects.Remove(match);
            nodesToBeDeleted.Add(match);
            nodeDeletionTime = maxNodeDeletionTime;

            if (nodesToCheck.Contains(match)) nodesToCheck.Remove(match);
        }
    }

    public static bool ComparePos(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.1f;
    }
}
