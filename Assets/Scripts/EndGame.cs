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
    public Text continueText;

    public void End(bool win)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        if(win)
        {
            if (CheckMatch.currentLevel == 4)
            {
                winText.text = "GAME COMPLETE!";
                continueText.text = "Play again";
            }
            else
            {
                winText.text = "LEVEL " + (CheckMatch.currentLevel + 1).ToString() + " COMPLETE!";
            }

            victory.Play();
        }
        else
        {
            winText.text = "YOU LOST...";
            continueText.text = "Retry";
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
