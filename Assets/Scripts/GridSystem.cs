using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridSystem : MonoBehaviour
{

    [SerializeField]
    private int gridSize = 30;
    private float tileSize = 1f;

    private Tile[,] gridTiles;
    private Tile[,] emptyTiles;

    private float noiseFrequency = .175f;
    private float noiseHeight = 1f;

    private float noiseObstacleThreshold = .63f;

    public GameObject player;

    //the time left of the player powerup
    //when the player is in powerup mode, the enemies run the opposite direction
    public float powerUpTime = 0f;

    List<GameObject> enemies;

    float noiseSeed = 0f;

    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        gridTiles = new Tile[gridSize, gridSize];
        emptyTiles = new Tile[gridSize, gridSize];

        //randomize the noise seed
        noiseSeed = Random.Range(100f, 10000f);

        SetupGrid();


        enemies = new List<GameObject>();

        enemies.Add(AddEnemy());
        enemies.Add(AddEnemy());

        GameObject spawnTile = GetRandomEmptyTile(10).tileObject;
        player = Instantiate((GameObject)Resources.Load("Player"), spawnTile.transform.position, Quaternion.identity);

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<AIDestinationSetter>().target = player.transform;
        }


        //add food to map
        int foodAmount = Random.Range(3, 6);
        for (int i = 0; i < foodAmount; i++) {
            AddFood();
        }

    }

    GameObject AddEnemy() {
        GameObject spawnTile = GetRandomEmptyTile(1).tileObject;
        return Instantiate((GameObject)Resources.Load("Baddie"), spawnTile.transform.position, Quaternion.identity);
    }

    public void EatFood(GameObject food) {
        score += food.GetComponent<FoodManager>().points;

        GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();

        //add powerup time
        powerUpTime += food.GetComponent<FoodManager>().points / 10f;

        Vector2Int tilePos = GetTilePos(food.GetComponent<FoodManager>().tile.tileObject);
        emptyTiles[tilePos.x, tilePos.y] = food.GetComponent<FoodManager>().tile;
        Destroy(food);

        AddFood();

    }
    void AddFood() {
        Tile spawnTile = GetRandomEmptyTile(1);
        Instantiate((GameObject)Resources.Load("Apple"), spawnTile.transform.position, Quaternion.identity).GetComponent<FoodManager>().tile = spawnTile;
        SetEmptyTile(spawnTile.tileObject);
    }

    void SetEmptyTile(GameObject tile) {
        Vector2Int tilePos = GetTilePos(tile);
        emptyTiles[tilePos.x, tilePos.y] = null;
    }

    float GetNoiseObstacle(int x, int y){
        //use math noise to create an obstacle pattern and return whether the obstacle at the current position should be an obstacle or not
        float height = Mathf.PerlinNoise((x+noiseSeed)*noiseFrequency, (y+noiseSeed) *noiseFrequency)*noiseHeight;

        return height;
    }

    Vector2Int GetTilePos(GameObject tile) {
        return new Vector2Int(Mathf.RoundToInt(tile.transform.position.x + (gridSize / 2) - (tileSize / 2)), Mathf.RoundToInt(tile.transform.position.y + (gridSize / 2) - (tileSize / 2)));
    }

    Tile GetRandomEmptyTile(float distanceFromEnemies)
    {
        Tile foundTile = emptyTiles[Random.Range(0, gridSize), Random.Range(0, gridSize)];

        bool isFarEnough = true;
        if (foundTile != null) {
            foreach (GameObject enemy in enemies)
            {
                if (Vector3.Distance(enemy.transform.position, foundTile.tileObject.transform.position) < distanceFromEnemies)
                {
                    isFarEnough = false;
                }
            }
        }

        if ((foundTile != null) && isFarEnough)
        {
            return foundTile;
        }
        else {
            return GetRandomEmptyTile(distanceFromEnemies);
        }

    }

    void SetupGrid(){
        for(int x = 0; x < gridSize; x++){
            for(int y = 0; y < gridSize; y++){
                GameObject tileObject = Instantiate((GameObject)Resources.Load("Tile"));

                tileObject.transform.position = new Vector3(x*tileSize, y*tileSize, 0) - (new Vector3(gridSize, gridSize, 0)/2) + new Vector3(tileSize/2, tileSize/2, 0);
                tileObject.name = x + "," + y;
                tileObject.transform.parent = GameObject.Find("GridContainer").transform;

                Tile tileClass = tileObject.GetComponent<Tile>();
                tileClass.tileObject = tileObject;
                tileClass.obstacle = tileObject.transform.Find("Obstacle").gameObject;

                tileClass.xIndex = x;
                tileClass.yIndex = y;

                float noiseObstacle = GetNoiseObstacle(x, y);

                if ((x+y)%2 == 0) {
                    tileClass.tileObject.GetComponent<SpriteRenderer>().color = new Color(76f/255f, 81f/255f, 87f/255f);
                }

                if (noiseObstacle >= noiseObstacleThreshold){
                    //this tile should be an obstacle
                    tileClass.occupationType = "Obstacle";

                    //modify the obstacle color depending on the noise
                    //this gives it a nicer effect and makes the game more lively

                    float H, S, V;

                    Color.RGBToHSV(tileClass.obstacle.GetComponent<SpriteRenderer>().color, out H, out S, out V);


                    tileClass.obstacle.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(H, S, V - ((noiseObstacle - noiseObstacleThreshold)*1.5f));
                }else{
                    emptyTiles[x, y] = tileClass;
                    tileClass.obstacle.SetActive(false);
                }

                gridTiles[x, y] = tileClass;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        powerUpTime = Mathf.Max(0, powerUpTime - Time.deltaTime);
        AstarPath.active.Scan();
    }
}
