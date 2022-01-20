using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
