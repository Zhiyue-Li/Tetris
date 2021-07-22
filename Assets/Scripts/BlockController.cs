using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BlockController : MonoBehaviour
{
    private float fallTime;
    private float previousTime;
    public static int leftBound = 0;
    public static int rightBound = 9;
    public static int lowerBound = 0;
    public static int upperBound = 18;
    public Vector3 rotationPoint;
    private static Transform[,] grid = new Transform[10, 20];
    // Start is called before the first frame update
    void Start()
    {
        fallTime = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - previousTime > fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position += new Vector3(0, 1, 0);
                GetComponent<PlayerInput>().enabled = false;
                GridOccupied();
                enabled = false;
                DetectLines();
                if (!GameOver())
                {
                    GameObject.Find("SpawnManager").GetComponent<SpawnManager>().SpawnBlocks();
                }
            }
            previousTime = Time.time;
        }
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            fallTime = 0.1f;
        }
    }

    public void OnTransform(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
            }
        }
    }

    void GridOccupied()
    {
        foreach (Transform child in transform)
        {
            int roundX = Mathf.RoundToInt(child.position.x);
            int roundY = Mathf.RoundToInt(child.position.y);
            grid[roundX, roundY] = child;
        }
    }

    void DetectLines()
    {
        for (int row = 0; row < 20; row++)
        {
            if (HasLine(row))
            {
                DeleteLine(row);
                RowDown(row);
            }
        }
    }

    bool HasLine(int row)
    {
        for (int col = 0; col < 10; col ++)
        {
            if (grid[col, row] == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int row)
    {
        for (int col = 0; col < 10; col++)
        {
            Destroy(grid[col, row].gameObject);
            grid[col, row] = null;
        }
    }

    void RowDown(int row)
    {
        for (int i = row; i < 19; i++)
        {
            for (int col = 0; col < 10; col++)
            {
                if (grid[col, i + 1] != null)
                {
                    grid[col, i] = grid[col, i + 1];
                    grid[col, i + 1].position += new Vector3(0, -1, 0);
                    grid[col, i + 1] = null;
                }
            }
        }
    }

    bool ValidMove()
    {
        foreach (Transform child in transform)
        {
            int roundX = Mathf.RoundToInt(child.position.x);
            int roundY = Mathf.RoundToInt(child.position.y);
            if (roundX < leftBound || roundX > rightBound || roundY < lowerBound || grid[roundX, roundY])
            {
                return false;
            }
        }
        return true;
    }

    bool GameOver()
    {
        foreach (Transform child in transform)
        {
            int roundY = Mathf.RoundToInt(child.position.y);
            if (roundY >= upperBound)
            {
                return true;
            }
        }
        return false;
    }

}
