using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //player color lerp
        GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, new Color(.4f, .72f, 1), Time.deltaTime*5f);

        //player movement
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 velocity = new Vector3(horizontal*speed, vertical* speed, 0);

        GetComponent<Rigidbody2D>().velocity = velocity;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        transform.rotation = Quaternion.identity;

        //face movement
        //move the face of the character to give a sense of direction to the user
        //the face will move based on the horizontal and vertical values

        Vector3 newFacePos = new Vector3(horizontal/4, vertical/4, 0);
        transform.Find("Visuals").localPosition = Vector3.Lerp(transform.Find("Visuals").localPosition, newFacePos, Time.deltaTime * 4);

    }

    public void DamagePlayer() {
        GameObject livesContainer = GameObject.FindGameObjectWithTag("Lives");
        if (livesContainer != null && livesContainer.transform.childCount < 2)
        {

            //set scores
            //get current score first
            int currentScore = GameObject.Find("GameManager").GetComponent<GridSystem>().score;

            //set the current score playerPref
            PlayerPrefs.SetInt("CurrentScore", currentScore);

            //if the current score is higher than the highest high score
            if (PlayerPrefs.GetInt("HighestScore", 0) < currentScore) {
                //update the highest store
                PlayerPrefs.SetInt("HighestScore", currentScore);
            }

            SceneManager.LoadScene("Highscores");
        }
        else
        {
            Destroy(livesContainer.transform.GetChild(0).gameObject);
        }
        
       
    }
}
