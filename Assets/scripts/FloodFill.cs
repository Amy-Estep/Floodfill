using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodFill : MonoBehaviour
{
    [SerializeField]
    private generate mazeGenerator;
    [SerializeField]
    private GameObject cubePrefab;
    public float speed = 2.0f;
    private int[,] maze;
    private Queue<Vector3Int> queue;
    private HashSet<Vector3Int> visited;
    private float timeElapsed = 0.0f;

    void Start()
    {
        timeElapsed += Time.deltaTime;
        maze = mazeGenerator.GetMaze();
        if (maze != null)
        {
            queue = new Queue<Vector3Int>();
            visited = new HashSet<Vector3Int>();
            Vector3Int startPos = new Vector3Int(0, 0, 0);
            queue.Enqueue(startPos);
            visited.Add(startPos);
            StartCoroutine(FloodFillAlgorithm());
        }
    }

    private IEnumerator FloodFillAlgorithm()
    {
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };
        while (queue.Count > 0)
        {
        timeElapsed += Time.deltaTime;
        Vector3Int current = queue.Dequeue();
        Vector3 cubePos = new Vector3(current.x * mazeGenerator.cellSize, 0.5f, current.z * mazeGenerator.cellSize);
        GameObject cube = Instantiate(cubePrefab, cubePos, Quaternion.identity, transform);
        if (current.x == Mathf.RoundToInt(mazeGenerator.GetGoalPosition().x) && current.z == Mathf.RoundToInt(mazeGenerator.GetGoalPosition().z))
        {
            Debug.Log("Time to reach the goal: " + timeElapsed + " seconds");
            break;
        }
        float waitTime = 1 / speed;
        timeElapsed += waitTime;
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < 4; i++)
        {
            int newX = current.x + dx[i];
            int newY = current.z + dy[i];
            Vector3Int newPos = new Vector3Int(newX, 0, newY);
            if (mazeGenerator.IsInBounds(newX, newY) && !visited.Contains(newPos) && maze[newX, newY] == (int)generate.CellType.Path)
            {
                queue.Enqueue(newPos);
                visited.Add(newPos);
            }
        }
        }   
    }
}
