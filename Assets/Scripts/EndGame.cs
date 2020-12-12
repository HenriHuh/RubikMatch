using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public Text winText;
    public void End(bool win)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        if(win)
        {
            winText.text = "YOU WON!";
        }
        else
        {
            winText.text = "YOU LOST...";
        }
    }

    public void Restart()
    {
        MainController.allObjects.Clear();
        CheckMatch.nodesToBeDeleted.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
