using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public Text winText;
    public AudioSource victory;
    public AudioSource defeat;

    public void End(bool win)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        if(win)
        {
            winText.text = "YOU WON!";
            victory.Play();
        }
        else
        {
            winText.text = "YOU LOST...";
            defeat.Play();
        }
    }

    public void Restart()
    {
        MainController.allObjects.Clear();
        CheckMatch.nodesToBeDeleted.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
