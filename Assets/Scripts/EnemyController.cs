using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    GridSystem gridSystem;
    Transform runAwayTarg;

    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.GetInt("Difficulty", 1) == 1) {
            //easy mode
            GetComponent<AILerp>().speed = 5f;
        }else if (PlayerPrefs.GetInt("Difficulty", 1) == 2)
        {
            //normal mode
            GetComponent<AILerp>().speed = 7f;
        }else if (PlayerPrefs.GetInt("Difficulty", 1) == 3)
        {
            //hard mode
            GetComponent<AILerp>().speed = 9f;
        }

        runAwayTarg = (new GameObject()).transform;
        runAwayTarg.parent = GameObject.Find("EnemyTargets").transform;
        gridSystem = GameObject.Find("GameManager").GetComponent<GridSystem>();
    }

    IEnumerator HitCooldown() {
        GetComponent<BoxCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(1);
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    void HitPlayer(GameObject plr) {
        StartCoroutine(HitCooldown());
        if (plr.GetComponent<PlayerController>() != null) {
            plr.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            plr.GetComponent<PlayerController>().DamagePlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            HitPlayer(collision.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (gridSystem.powerUpTime > 0f)
        {
            //run away
            if (GetComponent<AIDestinationSetter>().target != runAwayTarg)
            {
                GetComponent<AIDestinationSetter>().target = runAwayTarg;
            }

            Vector3 enemyToPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;

            runAwayTarg.position = transform.position - enemyToPlayer;
            GetComponent<SpriteRenderer>().color = Color.HSVToRGB(((Time.time*255f) % 255)/255f, .25f, 1);
        }
        else {
            //run towards player
            if (GetComponent<AIDestinationSetter>().target == runAwayTarg)
            {
                GetComponent<SpriteRenderer>().color = new Color(.94f, .28f, .32f);
                GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }
}
