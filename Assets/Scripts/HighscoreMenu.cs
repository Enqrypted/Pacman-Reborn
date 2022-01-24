using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoreMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighestScore", 0);
        transform.Find("highestScore").GetComponent<TextMeshProUGUI>().text = highScore.ToString();
        transform.Find("currentScore").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("CurrentScore", 0).ToString();

        transform.Find("scalar").localScale = new Vector3(Mathf.Min(highScore / 2500f, 1f), 1.33f, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMenu() {
        SceneManager.LoadScene("Main");
    }
}
