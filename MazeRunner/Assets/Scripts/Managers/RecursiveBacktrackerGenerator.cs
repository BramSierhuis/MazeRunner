using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Script to generate the maze
public class RecursiveBacktrackerGenerator : MonoBehaviour
{
    #region Variables
    [Header("Variables")]
    [SerializeField]
    private Vector2 entrance = new Vector2(1.5f, 0.5f); //Spawnpoint of the player and where the algorithm begins
    [SerializeField]
    private Vector3 offset = new Vector3(0.5f, 0, 0.5f); //Half the cell size
    [SerializeField]
    private float wallSpawnHeight = 0.25f; //Height to spawn the walls at

    [Header("References")]
    [SerializeField]
    private GameObject groundPrefab;
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject coinPrefab;

    private List<Vector2> cells = new List<Vector2>(); //All cells
    private List<Vector2> stack = new List<Vector2>(); //Stack
    private List<Vector2> visited = new List<Vector2>(); //Visited cells
    private List<GameObject> walls = new List<GameObject>(); //All walls

    private int width; //Width of the maze
    private int height; //Height of the maze
    private int surface; //Surface area of the maze
    #endregion

    #region Singleton
    [HideInInspector]
    public static RecursiveBacktrackerGenerator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion

    //Gets called when game ends, clears all lists
    public void Reset()
    {
        stack.Clear();
        visited.Clear();
        cells.Clear();
        walls.Clear();
    }

    //Get called when game starts, generates the maze
    public void Generate(int width, int height)
    {
        this.width = width;
        this.height = height;
        surface = width * height;

        //Spawn the ground plane, center top down camera above it
        SpawnGround();
        Camera.main.GetComponent<CameraHover>().Center();

        MakeGrid(); //Make a grid for this width and height
        DrawBorders(); //Add borders around the grid
        DrawWalls(); //Add walls to the grid
        Backtrack(); //Remove walls to create the maze
        SpawnCoins(); //Add coins to the maze
    }

    #region Generation Functions
    private void SpawnGround()
    {
        //Spawn the ground plane
        Transform ground = Instantiate(groundPrefab).transform;
        ground.localScale = new Vector3(width / 10f, 1, height / 10f);
        ground.position = new Vector3(width / 2f, 0, height / 2f);
    }

    private void MakeGrid()
    {
        //Make the grid based on width and height
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells.Add(new Vector2(x + offset.x, y + offset.z));
            }
        }
    }

    private void DrawBorders()
    {
        //Add borders around the maze
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //The most left and right borders
                if(x == 0 || x == width)
                {
                    //Add a wall for each y value
                    Instantiate(wallPrefab, new Vector3(x, wallSpawnHeight, y + 0.5f), Quaternion.Euler(0, 90, 0));
                    Instantiate(wallPrefab, new Vector3(width, wallSpawnHeight, y + 0.5f), Quaternion.Euler(0, 90, 0));
                }

                //The most top and down borders
                if (y == 0 || y == height)
                {
                    //Add a wall for each x value
                    Instantiate(wallPrefab, new Vector3(x + 0.5f, wallSpawnHeight, y), Quaternion.identity);
                    Instantiate(wallPrefab, new Vector3(x + 0.5f, wallSpawnHeight, height), Quaternion.identity);
                }

            }
        }
    }

    private void DrawWalls()
    {
        //Add walls to each cell
        for (int i = 0; i < cells.Count; i++)
        {
            //The most right cells just need a wall on the bottom
            if(cells[i].x == width - offset.x)
            {
                if(cells[i].y != offset.z)
                    //Generate bottom wall
                    walls.Add(Instantiate(wallPrefab, new Vector3(cells[i].x, wallSpawnHeight, cells[i].y - 0.5f), Quaternion.identity));
            }
            //The lowest walls just need a wall on the right
            else if(cells[i].y == offset.z)
            {
                //Generate right wall
                walls.Add(Instantiate(wallPrefab, new Vector3(cells[i].x + 0.5f, wallSpawnHeight, cells[i].y), Quaternion.Euler(0, 90, 0)));
            }
            //All other cells need walls on the bottom and the right
            else
            {
                //Generate bottom wall
                walls.Add(Instantiate(wallPrefab, new Vector3(cells[i].x, wallSpawnHeight, cells[i].y - 0.5f), Quaternion.identity));
                //Generate right wall
                walls.Add(Instantiate(wallPrefab, new Vector3(cells[i].x + 0.5f, wallSpawnHeight, cells[i].y), Quaternion.Euler(0, 90, 0)));
            }
        }
    }

    private void Backtrack()
    {
        //Get a randomized list of possible neighbours
        Vector2[] RandomOffsets()
        {
            //Array of possible neighbours (Top, Down, Left, Right)
            Vector2[] neighbourOffsets = { new Vector2(0, 1), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(1, 0) };
           
            //Randomize the array
            for (int i = 0; i < neighbourOffsets.Length; i++)
            {
                int rand = Random.Range(0, neighbourOffsets.Length);
                Vector2 temp = neighbourOffsets[rand];
                neighbourOffsets[rand] = neighbourOffsets[i];
                neighbourOffsets[i] = temp;
            }

            //Return the randomized array
            return neighbourOffsets;
        }

        //Add the starting cell to the stack
        Vector2 currentCell = entrance;
        stack.Add(currentCell);
        visited.Add(currentCell);

        //Keep running till the stack is empty
        while (stack.Count > 0)
        {
            //Try to move to the next pos
            Vector2[] offsets = RandomOffsets();
            bool neighbourFound = false; //Used to track if a neighbour is found

            //Try to find an unvisited neighbour
            foreach(Vector2 offset in offsets)
            {
                //If a neighbour is found and it is unvisited
                if(cells.Contains(currentCell + offset) && !visited.Contains(currentCell + offset)){
                    //Get the position of the wall to break
                    float xTobreak = currentCell.x + offset.x * 0.5f;
                    float yToBreak = wallSpawnHeight;
                    float zToBreak = currentCell.y + offset.y * 0.5f;
                    GameObject wallToBreak = getWallFromVector3(new Vector3(xTobreak, yToBreak, zToBreak));

                    if (wallToBreak != null)
                    {
                        //Remove the wall
                        Destroy(wallToBreak);
                        walls.Remove(wallToBreak);

                        //Update the lists
                        //set the current cell as the neighbour
                        currentCell += offset;
                        //Add this cell to the stack
                        stack.Add(currentCell);
                        //Add the current(previous) cell to the visited list
                        visited.Add(currentCell);
                   
                        neighbourFound = true;
                    }

                    break;
                }

                neighbourFound = false;
            }

            //If we get here we didnt get to the next pos and we couldnt find an unvisited neighbour
            //So move back in the stack (backtrack)
            if (stack.Any() && !neighbourFound)
            {
                stack.RemoveAt(stack.Count - 1);

                if(stack.Count > 0)
                    currentCell = stack[stack.Count - 1];
            }

        }
    }

    private void SpawnCoins()
    {
        int coins = 0;

        //Calculate the number of cpins that should be generated
        int expectedCoins = (int)(surface * GameManager.Instance.GetDifficulty().coinSpawnRate);

        //Make sure at least 1 coin will be generated
        if (expectedCoins < 1) expectedCoins = 1;

        //Loop through all cells and add a coin to a random % of them
        //Only stop if the expected amount of coins is generated
        while (coins < expectedCoins)
        {
            foreach (Vector2 cell in cells)
            {
                if (Random.value <= 1 / surface)
                {
                    Instantiate(coinPrefab, new Vector3(cell.x, 0.5f, cell.y), Quaternion.identity);
                    coins++;
                }
            }
        }

        //Tell the gamemanager how many coins have been added
        GameManager.Instance.SetCoins(coins);
    }

    //Get a wall gameobject based on its position
    private GameObject getWallFromVector3(Vector3 pos)
    {
        //Loop through all walls and compare positions
        foreach(GameObject wall in walls)
        {
            if (wall.transform.position == pos)
                return wall;
        }

        return null;
    }
    #endregion

    #region Getters
    public int GetSurface()
    {
        return surface;
    }

    public Vector2 GetEntrance()
    {
        return entrance;
    }
    #endregion
}
