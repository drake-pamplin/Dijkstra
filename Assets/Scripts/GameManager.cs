using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    [Header ("Prefabs")]
    public GameObject tilePrefab;
    
    [Header ("Class Variables")]
    public int numberOfRows = 8;
    public int numberOfColumns = 16;
    public GameObject startPoint;
    public GameObject endPoint;
    public float delay;

    [Header ("Map Name (No Extensions)")]
    public string map = "MazeNoWalls";

    private int numberOfTiles;
    private List<GameObject> tiles = new List<GameObject>();
    private bool pathFound = false;
    private Queue<GameObject> tileQueue = new Queue<GameObject>();
    private bool findingPath = false;
    private string mazePath;
    private List<GameObject> path;
    
    // Start is called before the first frame update
    void Start()
    {
        numberOfTiles = numberOfRows * numberOfColumns;
        mazePath = map;
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !findingPath && endPoint != null) {
            findingPath = true;
            StartCoroutine("Pathfinding");
        }

        if (Input.GetKeyDown(KeyCode.Space) && pathFound) {
            Cleanup();
        }
    }

    // Build map off of .txt file loaded
    void GenerateGrid() {
        string filePath = "Assets/Resources/Maps/" + mazePath + ".txt";
        string[] lines = System.IO.File.ReadAllLines(filePath);
        numberOfRows = int.Parse(lines[0].ToString());
        numberOfColumns = int.Parse(lines[1].ToString());
        numberOfTiles = numberOfRows * numberOfColumns;
        string map = "";
        for (int lineIndex = 2; lineIndex < lines.Length; lineIndex++) {
            map += lines[lineIndex];
        }
        
        Camera.main.transform.position = new Vector3(
            ((float)numberOfColumns / 2) - 0.5f,
            Camera.main.transform.position.y,
            (-(numberOfRows / 2)) + 0.5f
        );
        if (numberOfColumns > numberOfRows) {
            Camera.main.orthographicSize = (numberOfColumns / 2) + 0.75f;
        }
        else {
            Camera.main.orthographicSize = (numberOfRows / 2) + 0.75f;
        }
        
        for (int tileNumber = 0; tileNumber < numberOfTiles; tileNumber++) {
            GameObject tmp = Instantiate(
                tilePrefab,
                new Vector3(
                    tileNumber % numberOfColumns, 
                    0, 
                    -(tileNumber / numberOfColumns)
                ),
                Quaternion.identity
            );
            tmp.name = "Tile(" + tileNumber + ")";
            tmp.GetComponent<Tile>().index = tileNumber;
            if (map[tileNumber].ToString().ToLower() == "x") {
                tmp.GetComponent<Tile>().cost = -1;
            }
            else {
                tmp.GetComponent<Tile>().cost = int.Parse(map[tileNumber].ToString());
            }
            tiles.Add(tmp);
        }

        // Link the tiles up to form a grid
        foreach (GameObject tileObject in tiles) {
            Tile tile = tileObject.GetComponent<Tile>();

            // connect top
            if (tile.index >= numberOfColumns) {
                tile.top = tiles[tile.index - numberOfColumns];
            }
            
            // connect right
            if (tile.index % numberOfColumns != numberOfColumns-1) {
                tile.right = tiles[tile.index+1];
            }

            // connect bottom
            if (tile.index < (numberOfTiles-numberOfColumns)) {
                tile.bottom = tiles[tile.index + numberOfColumns];
            }

            // connect left
            if (tile.index % numberOfColumns != 0) {
                tile.left = tiles[tile.index-1];
            }
        }
    }

    // Cleanup the map and reload
    void Cleanup() {
        foreach (GameObject tile in tiles) {
            Destroy(tile);
        }
        tiles = new List<GameObject>();
        pathFound = false;
        findingPath = false;
        startPoint = null;
        endPoint = null;
        tileQueue = new Queue<GameObject>();

        GenerateGrid();
    }

    // Find the distance between each tile and the starting tile until
    // the end point is found
    private IEnumerator Pathfinding() {
        tileQueue.Enqueue(startPoint);
        
        while (!pathFound && findingPath) {
            GameObject currentTile = tileQueue.Dequeue();
            Tile tile = currentTile.GetComponent<Tile>();

            if (tile.top != null) {
                if (tile.top.GetComponent<Tile>().cost == 0 && tile.top != startPoint) {
                    tile.top.GetComponent<Tile>().cost = tile.cost+1;
                    tile.top.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("{0}", tile.top.GetComponent<Tile>().cost);
                    if (tile.top == endPoint) {
                        pathFound = true;
                        continue;
                    }
                    if (!pathFound) {
                        tileQueue.Enqueue(tile.top);
                    }
                    tile.top.SendMessage("Visit");
                }
            }

            yield return new WaitForSeconds(delay);

            if (tile.right != null) {
                if (tile.right.GetComponent<Tile>().cost == 0 && tile.right != startPoint) {
                    tile.right.GetComponent<Tile>().cost = tile.cost+1;
                    tile.right.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("{0}", tile.right.GetComponent<Tile>().cost);
                    if (tile.right == endPoint) {
                        pathFound = true;
                        continue;
                    }
                    if (!pathFound) {
                        tileQueue.Enqueue(tile.right);
                    }
                    tile.right.SendMessage("Visit");
                }
            }

            yield return new WaitForSeconds(delay);

            if (tile.bottom != null) {
                if (tile.bottom.GetComponent<Tile>().cost == 0 && tile.bottom != startPoint) {
                    tile.bottom.GetComponent<Tile>().cost = tile.cost+1;
                    tile.bottom.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("{0}", tile.bottom.GetComponent<Tile>().cost);
                    if (tile.bottom == endPoint) {
                        pathFound = true;
                        continue;
                    }
                    if (!pathFound) {
                        tileQueue.Enqueue(tile.bottom);
                    }
                    tile.bottom.SendMessage("Visit");
                }
            }

            yield return new WaitForSeconds(delay);

            if (tile.left != null) {
                if (tile.left.GetComponent<Tile>().cost == 0 && tile.left != startPoint) {
                    tile.left.GetComponent<Tile>().cost = tile.cost+1;
                    tile.left.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("{0}", tile.left.GetComponent<Tile>().cost);
                    if (tile.left == endPoint) {
                        pathFound = true;
                        continue;
                    }
                    if (!pathFound) {
                        tileQueue.Enqueue(tile.left);
                    }
                    tile.left.SendMessage("Visit");
                }
            }

            yield return new WaitForSeconds(delay);
        }

        pathFound = false;
        StartCoroutine("FindPath");
    }

    // Find path using the tile distances
    IEnumerator FindPath() {
        path = new List<GameObject>();
        GameObject currentTile = endPoint; 
        path.Add(currentTile);

        while (!pathFound) {
            yield return new WaitForSeconds(delay);
            
            if (currentTile == startPoint) {
                pathFound = true;
                currentTile.SendMessage("Click");
                path.Add(currentTile);
                continue;
            }
            Tile tile = currentTile.GetComponent<Tile>();
            
            if (tile.top != null) {
                if (tile.top.GetComponent<Tile>().cost < tile.cost && (tile.top.GetComponent<Tile>().cost > 0 || tile.top == startPoint)) {
                    currentTile = tile.top;
                    currentTile.SendMessage("Click");
                    path.Add(currentTile);
                    continue;
                }
            }
            if (tile.right != null) {
                if (tile.right.GetComponent<Tile>().cost < tile.cost && (tile.right.GetComponent<Tile>().cost > 0 || tile.right == startPoint)) {
                    currentTile = tile.right;
                    currentTile.SendMessage("Click");
                    path.Add(currentTile);
                    continue;
                }
            }
            if (tile.bottom != null) {
                if (tile.bottom.GetComponent<Tile>().cost < tile.cost && (tile.bottom.GetComponent<Tile>().cost > 0 || tile.bottom == startPoint)) {
                    currentTile = tile.bottom;
                    currentTile.SendMessage("Click");
                    path.Add(currentTile);
                    continue;
                }
            }
            if (tile.left != null) {
                if (tile.left.GetComponent<Tile>().cost < tile.cost && (tile.left.GetComponent<Tile>().cost > 0 || tile.left == startPoint)) {
                    currentTile = tile.left;
                    currentTile.SendMessage("Click");
                    path.Add(currentTile);
                    continue;
                }
            }
        }

        StartCoroutine("DisplayPath");
    }

    // Display the path that was found
    private IEnumerator DisplayPath() {
        for (int index = path.Count-1; index >= 0; index--) {
            yield return new WaitForSeconds(delay);

            path[index].SendMessage("FindPath");
        }
    }
}
