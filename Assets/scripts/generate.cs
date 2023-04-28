using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[DefaultExecutionOrder(-1)] 
public class generate : MonoBehaviour
{
    [SerializeField]
    public int MazeNumber;
    public int width = 10;
    public int height = 10;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject goalPrefab;
    public float cellSize = 1.0f;
    private int[,] maze;
    public enum CellType { Wall = 0, Path = 1 };

    void Start()
    {
        LoadMaze(MazeNumber);
        InstantiateMaze();
    }

    void Update()
    {

    }

    public int[,] GetMaze()
    {
        return maze;
    }

    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    private void LoadMaze(int mazeNumber)
    {
        string filename = "maze" + mazeNumber + ".csv";
        if (File.Exists(filename))
        {
            string[] lines = File.ReadAllLines(filename);
            maze = new int[width, height];
            for (int y = 0; y < height; y++)
            {
                string[] values = lines[y].Split(',');
                for (int x = 0; x < width; x++)
                {
                    maze[x, y] = int.Parse(values[x]);
                }
            }
        }
        else
        {
            Debug.LogError("File not found: " + filename);
        }
    }

    private void InstantiateMaze()
    {
        InstantiateOuterWalls();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject prefab = maze[x, y] == (int)CellType.Path ? floorPrefab : wallPrefab;
                Vector3 position = new Vector3(x * cellSize, 0, y * cellSize);
                Instantiate(prefab, position, Quaternion.identity, transform);
            }
        }
        Vector3 goalPosition = GetGoalPosition();
        Instantiate(goalPrefab, goalPosition + new Vector3(0, 0.3f, 0), Quaternion.Euler(90, 0, 0), transform);
    }

    public Vector3 GetGoalPosition()
    {
        int goalX = width - 1;
        int goalY = height - 1;
        return new Vector3(goalX * cellSize, 0, goalY * cellSize);
    }
private void InstantiateOuterWalls()
{
    for (int x = -1; x < width + 1; x++) 
    {
        Instantiate(wallPrefab, new Vector3(x * cellSize, 0, -cellSize), Quaternion.identity, transform);
        Instantiate(wallPrefab, new Vector3(x * cellSize, 0, height * cellSize), Quaternion.identity, transform);
    }

    for (int y = 0; y < height + 1; y++) 
    {
        Instantiate(wallPrefab, new Vector3(-cellSize, 0, y * cellSize), Quaternion.identity, transform);
        Instantiate(wallPrefab, new Vector3(width * cellSize, 0, y * cellSize), Quaternion.identity, transform);
    }
}

}
