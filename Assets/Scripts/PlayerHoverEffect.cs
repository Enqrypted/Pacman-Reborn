using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoverEffect : MonoBehaviour
{

    float originalY = 0;

    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, originalY + Mathf.Sin(Time.time*5f)/3f, 0);

        Vector3 targPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        transform.Find("Visuals").localPosition = Vector3.Lerp(transform.Find("Visuals").localPosition, targPos.normalized/5f, Time.deltaTime*5f);
    }
}
