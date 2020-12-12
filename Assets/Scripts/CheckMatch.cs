using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMatch : MonoBehaviour
{
    static Vector3[] sides = { Vector3.right, Vector3.up, Vector3.forward };
    static List<GameObject> nodesToCheck = new List<GameObject>();

    void Start()
    {
        
    }

    public static void CheckAllNodes()
    {
        foreach (GameObject g in MainController.allObjects)
        {
            nodesToCheck.Add(g);
        }
    }

    private void Update()
    {
        if (nodesToCheck.Count > 0)
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
            while (MainController.allObjects.Find(o => o.transform.position == currentPosition + vec && o.name == node.name))
            {
                currentMatches.Add(MainController.allObjects.Find(o => o.transform.position == currentPosition + vec && o.name == node.name));
                currentPosition += vec;
            }

            //Negative vector
            currentPosition = node.transform.position;
            while (MainController.allObjects.Find(o => o.transform.position == currentPosition - vec && o.name == node.name))
            {
                currentMatches.Add(MainController.allObjects.Find(o => o.transform.position == currentPosition - vec && o.name == node.name));
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
            MainController.instance.NewNode(node.transform.position);
            Destroy(node);
        }
        foreach (GameObject match in totalMatches)
        {
            MainController.allObjects.Remove(match);
            MainController.instance.NewNode(match.transform.position);
            Destroy(match);

            if (nodesToCheck.Contains(match)) nodesToCheck.Remove(match);
        }
    }
}
