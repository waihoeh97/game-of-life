using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOfLifeManager : MonoBehaviour {

    public int cellCountX;
    public int cellCountZ;

    Transform cell;
    public Transform cellPrefab;
    private Transform[,] cells;

    public float updateTime = 0;
    private float elapsedTime = 0;

    private bool isPlaying = false;
    private bool isStep = false;

    public Slider speed;

    void Start()
    {
        Grid();
    }

    void Grid()
    {
        cells = new Transform[cellCountZ, cellCountX];

        for (int i = 0; i < cellCountZ; i++)
        {
            for (int j = 0; j < cellCountX; j++)
            {
                cell = GameObject.Instantiate(cellPrefab) as Transform;

                cell.parent = transform;
                cell.transform.position = new Vector3(j * cell.localScale.x, 0, i * cell.localScale.z) - new Vector3(cellCountX * cell.localScale.x / 2, 0, cellCountZ * cell.localScale.z / 2);
                cell.transform.position += new Vector3(cell.localScale.x / 2, 0, cell.localScale.z / 2);

                cells[i, j] = cell;
            }
        }
    }

    void ClearGrid()
    {
        GameObject[] cell = GameObject.FindGameObjectsWithTag("Cell");

        foreach (GameObject go in cell)
        {
            Destroy(go);
        }
    }

    /// <summary>
    /// Time Change Interval
    /// </summary>
    public void speedSlider ()
    {
        updateTime = speed.value;
    }

    void Update()
    {
        if (!isPlaying && !isStep) return;

        if (isPlaying)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= updateTime)
            {
                elapsedTime = 0;
            }
            else
            {
                return;
            }
        }

        for (int i = 0; i < cellCountZ; i++)
        {
            for (int j = 0; j < cellCountX; j++)
            {
                Cell cell = cells[i, j].GetComponent<Cell>();
                int neighbours = 0;

                for (int z = i - 1; z <= i + 1; z++)
                {
                    if (z >= 0 && z < cellCountZ)
                    {
                        for (int x = j - 1; x <= j + 1; x++)
                        {
                            if (x >= 0 && x < cellCountX)
                            {
                                if (z != i || x != j)
                                {
                                    // Ensure it doesn't check itself

                                    Cell neighbourCell = cells[z, x].GetComponent<Cell>();

                                    if (neighbourCell.IsAlive)
                                    {
                                        neighbours++;
                                    }
                                }
                            }
                        }
                    }
                }

                // Game of Life Rules

                if (cell.IsAlive)
                {
                    if (neighbours < 2)
                    {
                        cell.SetIsAlive(false);
                    }
                    else if (neighbours == 2 || neighbours == 3)
                    {
                        cell.SetIsAlive(true);
                    }
                    else if (neighbours > 3)
                    {
                        cell.SetIsAlive(false);
                    }
                }
                else
                {
                    if (neighbours == 3)
                    {
                        cell.SetIsAlive(true);
                    }
                }
            }
        }

        for (int i = 0; i < cellCountZ; i++)
        {
            for (int j = 0; j < cellCountX; j++)
            {
                Cell cell = cells[i, j].GetComponent<Cell>();
                cell.ApplyIsAlive();
            }
        }
    }

    void OnGUI()
    {
        // Background of GUI
        GUI.Box(new Rect(0, 0, 150, 150), "");

        // Time interval text
        GUI.Label (new Rect(50, 90, 50, 30), "Time");

        // Step
        isStep = GUI.Button(new Rect(10, 5, 50, 30), "Step");

        // Play/Pause
        bool pressed = GUI.Button(new Rect(70, 5, 50, 30), isPlaying ? "Stop" : "Play");
        if (pressed) isPlaying = !isPlaying;

        // Clear
        if (GUI.Button(new Rect(10, 45, 50, 30), "Clear"))
        {
            Clear();
        }

        // Change Map size
        if (GUI.Button(new Rect(70, 45, 25, 30), "+"))
        {
            IncreaseSize();
        }

        if (GUI.Button(new Rect(100, 45, 25, 30), "-"))
        {
            DecreaseSize();
        }
    }

    void Clear()
    {
        for (int i = 0; i < cellCountZ; i++)
        {
            for (int j = 0; j < cellCountX; j++)
            {
                Cell cell = cells[i, j].GetComponent<Cell>();
                cell.SetIsAlive(false);
                cell.ApplyIsAlive();
            }
        }

        isPlaying = false;
    }

    void IncreaseSize()
    {
        cellCountX += 25;
        cellCountZ += 25;
        ClearGrid();
        Grid();
    }

    void DecreaseSize()
    {
        cellCountX -= 25;
        cellCountZ -= 25;
        ClearGrid();
        Grid();
    }
}
