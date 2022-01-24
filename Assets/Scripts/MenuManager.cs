using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    int currentDifficulty = 1;
    private void Start()
    {
        PlayerPrefs.SetInt("Difficulty", currentDifficulty);

        if (PlayerPrefs.GetInt("TutorialDone", 0) == 0) {
            //show the tutorial
            SceneManager.LoadScene("Tutorial");
        }

    }

    public void PlayGame() {
        SceneManager.LoadScene("Game");
    }

    public void ChangeDiff() {
        currentDifficulty++;
        if (currentDifficulty > 3) {
            currentDifficulty = 1;
        }

        PlayerPrefs.SetInt("Difficulty", currentDifficulty);

        if (currentDifficulty == 1) {
            transform.Find("diffBtn").GetChild(0).GetComponent<TextMeshProUGUI>().text = "DIFFICULTY: E";
        }else if (currentDifficulty == 2)
        {
            transform.Find("diffBtn").GetChild(0).GetComponent<TextMeshProUGUI>().text = "DIFFICULTY: M";
        }else if (currentDifficulty == 3)
        {
            transform.Find("diffBtn").GetChild(0).GetComponent<TextMeshProUGUI>().text = "DIFFICULTY: H";
        }
    }

    public void Score() {
        SceneManager.LoadScene("Highscores");
    }
}
