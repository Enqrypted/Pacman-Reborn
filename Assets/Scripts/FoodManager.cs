using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public int points = 0;
    float H, S, V;
    Vector3 pos;
    float randTimeOffset;
    public Tile tile;
    // Start is called before the first frame update
    void Start()
    {

        points = Random.Range(1,4)*5;

        transform.Find("Canvas").Find("txt").GetComponent<TextMeshProUGUI>().text = points.ToString();

        randTimeOffset = Random.Range(1f, 200f);
        pos = transform.position;
        Color.RGBToHSV(GetComponent<SpriteRenderer>().color, out H, out S, out V);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pos + new Vector3(0, Mathf.Sin((Time.time + randTimeOffset) *10f)/20f, 0);
        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(H, S, V + (Mathf.Sin(Time.time*5f)/7f));
        transform.Find("Apple").gameObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(H, S, V + (Mathf.Sin(Time.time * 5f) / 7f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            GameObject.Find("GameManager").GetComponent<GridSystem>().EatFood(gameObject);
        }
    }
}
