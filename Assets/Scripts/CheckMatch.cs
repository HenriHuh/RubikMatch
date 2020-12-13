using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckMatch : MonoBehaviour
{
    static Vector3[] sides = { Vector3.right, Vector3.up, Vector3.forward };
    static List<GameObject> nodesToCheck = new List<GameObject>();
    public static List<GameObject> nodesToBeDeleted = new List<GameObject>();
    public List<Text> texts = new List<Text>();
    const float maxNodeDeletionTime = 1;
    static float nodeDeletionTime = 0;
    Dictionary<string, int> Planets = new Dictionary<string, int>();
    int[] planetsLeft = { 8, 8, 8, 8, 8 };
    public static int currentLevel;
    public EndGame endGame;
    public AudioSource plopSound;
    bool ended = false;

    void Start()
    {
        for (int i = 0; i < planetsLeft.Length; i++)
        {
            planetsLeft[i] += currentLevel * 2;
            texts[i].text = "x" + planetsLeft[i];
        }

        Planets.Add("earth", 0);
        Planets.Add("moon", 1);
        Planets.Add("saturn", 2);
        Planets.Add("mars", 3);
        Planets.Add("star", 4);
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

                string nodeName = node.name.Replace("(Clone)", "").Trim().ToLower();
                int index = Planets[nodeName];
                if(RotationController.movesLeft != 20) {
                    planetsLeft[index] = Mathf.Max(0, planetsLeft[index]-1);
                    texts[index].text = "x" + planetsLeft[index];
                }
                Destroy(node);
                plopSound.Play();
                MainController.instance.NewNode(pos);
            }

            bool gameWon = true;
            foreach(int left in planetsLeft)
            {
                if(left > 0)
                {
                    gameWon = false;
                }
            }
            if(gameWon)
            {
                if (!ended)
                {
                    ended = true;
                    endGame.End(true);
                    currentLevel++;
                }
            }
            if (nodesToBeDeleted.Count > 0) StartCoroutine(LateCheck());
            nodesToBeDeleted.Clear();
        }
    }

    IEnumerator LateCheck()
    {
        yield return new WaitForEndOfFrame();
        CheckAllNodes();
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
