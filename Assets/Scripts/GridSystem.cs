using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{

    [SerializeField]
    private int gridSize = 30;
    private float tileSize = 1f;

    private Tile[,] gridTiles;
    private Tile[,] emptyTiles;

    private float noiseFrequency = .2f;
    private float noiseHeight = 1f;

    private float noiseObstacleThreshold = .55f;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        gridTiles = new Tile[gridSize, gridSize];
        emptyTiles = new Tile[gridSize, gridSize];
        SetupGrid();

        GameObject spawnTile = GetRandomEmptyTile().tileObject;
        player = Instantiate((GameObject)Resources.Load("Player"), spawnTile.transform.position, Quaternion.identity);

    }

    float GetNoiseObstacle(int x, int y){
        //use math noise to create an obstacle pattern and return whether the obstacle at the current position should be an obstacle or not
        float height = Mathf.PerlinNoise(x*noiseFrequency, y*noiseFrequency)*noiseHeight;

        return height;
    }

    Tile GetRandomEmptyTile()
    {
        Tile foundTile = emptyTiles[Random.Range(0, gridSize), Random.Range(0, gridSize)];

        if (foundTile != null)
        {
            return foundTile;
        }
        else {
            return GetRandomEmptyTile();
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

                float noiseObstacle = GetNoiseObstacle(x, y);

                if (noiseObstacle >= noiseObstacleThreshold){
                    //this tile should be an obstacle
                    tileClass.occupationType = "Obstacle";

                    //modify the obstacle color depending on the noise
                    //this gives it a nicer effect and makes the game more lively

                    float H, S, V;

                    Color.RGBToHSV(tileClass.obstacle.GetComponent<SpriteRenderer>().color, out H, out S, out V);


                    tileClass.obstacle.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(H, S, V - (noiseObstacle-noiseObstacleThreshold));
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
        
    }
}
