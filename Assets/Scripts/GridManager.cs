using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{
    public float cols;
    public float rows;

    public GameObject mazeHolder;
    public GameObject mazePart;
    private GameObject[] outerWalls;

    public float tileSize = 2f;

    private MazeCell[,] mazeCells;
    //private Dictionary<Vector2Int,CellGroup> cellGroupDictionary = new Dictionary<Vector2Int, CellGroup>();

    [SerializeField]
    private ResizeCamera resizeCamera;

    [SerializeField]
    private MazeCombine mazeCombine;

    private bool mazeExists = false;

    // Start is called before the first frame update
    void Start()
    {
        //initialize maximum possible size of the maze
        mazeCells = new MazeCell[250, 250];

        outerWalls = new GameObject[4];
        InitializeOuterWalls();

        GenerateGrid(cols, rows);
        //mazeCombine.CombineMesh();
    }

    

    public void UpdateCols(float newCols)
    {
        if (mazeExists == true)
        {
            StopAllCoroutines();
            GenerateGrid(cols, rows);
            mazeExists = false;
        }

        if (newCols < cols)
        {
            ScaleDownGrid(newCols, rows);
        }
        else
        {
            GenerateGrid(newCols, rows);
        }
    }

    public void UpdateRows(float newRows)
    {
        if (mazeExists == true)
        {
            StopAllCoroutines();
            GenerateGrid(cols, rows);
            mazeExists = false;
        }

        if (newRows < rows)
        {
            ScaleDownGrid(cols, newRows);
        }
        else
        {
            GenerateGrid(cols, newRows);
        }
    }

    private void GenerateGrid(float newCols, float newRows)
    {
        
        //create or repair a grid with dimensions of newCols by newRows
        for (int row = 0; row < newRows; row++)
        {
            for (int col = 0; col < newCols; col++)
            {
                AddGridCell(col, row); 
            }
        }

        //update the current rows and collums
        rows = newRows;
        cols = newCols;

        //update camera location
        resizeCamera.ChangeLocation(cols, rows, tileSize);
        ResizeOuterWalls();
    }

    private void ScaleDownGrid(float newCols, float newRows)
    {
        if (newCols < cols)
        {
            //delete grid columns
            for (int row = 0; row < rows; row++)
            {
                for (int col = (int)newCols; col < cols; col++)
                {
                    DestroyGridCell(col, row);
                }
            }
            cols = newCols;
        }
        if(newRows < rows)
        {
            //delete grid rows
            for (int row = (int)newRows; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    DestroyGridCell(col, row);
                }
            }
            rows = newRows;
        }

        //update camera location
        resizeCamera.ChangeLocation(cols, rows, tileSize);
        ResizeOuterWalls();
    }

    private void AddGridCell(int col, int row)
    {
        //create a new maze cell
        if (mazeCells[col, row] == null)
        {
            mazeCells[col, row] = new MazeCell();
        }

        //the position is the row or column multiplied by the respective width of the cell part
        float posX = col * tileSize;
        float posY = row * tileSize;

        float halfTile = tileSize / 2;

        //floor
        //mazeCells[col, row].floor = Instantiate(mazePart, new Vector3(posX, 0, posY), Quaternion.identity);
        //mazeCells[col, row].floor.name = "Maze Cell " + row + ", " + col;
        //mazeCells[col, row].floor.transform.Rotate(Vector3.right, 90f);
        //mazeCells[col, row].floor.transform.parent = mazeHolder.transform;

        //wall foreward
        if (row != rows - 1)
        {
            if (mazeCells[col, row].wallForeward == null)
            { 
                mazeCells[col, row].wallForeward = Instantiate(mazePart, new Vector3(posX, halfTile, posY + halfTile), Quaternion.identity);
                mazeCells[col, row].wallForeward.name = "Wall Foreward " + row + ", " + col;
                mazeCells[col, row].wallForeward.transform.parent = mazeHolder.transform;
            }
            mazeCells[col, row].wallForeward.SetActive(false);
        }

        //wall right
        if (col != cols - 1)
        {
            if (mazeCells[col, row].wallRight == null)
            {
                mazeCells[col, row].wallRight = Instantiate(mazePart, new Vector3(posX + halfTile, halfTile, posY), Quaternion.identity);
                mazeCells[col, row].wallRight.name = "Wall Right " + row + ", " + col;
                mazeCells[col, row].wallRight.transform.Rotate(Vector3.up, 90f);
                mazeCells[col, row].wallRight.transform.parent = mazeHolder.transform;
            }
            mazeCells[col, row].wallRight.SetActive(false);
        }
    }

    private void DestroyGridCell(int col, int row)
    {
        if (mazeCells[col, row].wallForeward)
            Destroy(mazeCells[col, row].wallForeward);

        if (mazeCells[col, row].wallRight)
            Destroy(mazeCells[col, row].wallRight);

        mazeCells[col, row] = null;
    }

    private void InitializeOuterWalls()
    {
        float halfTile = tileSize / 2;
        //back wall
        if (outerWalls[0] == null)
        {
            outerWalls[0] = Instantiate(mazePart, new Vector3(0, halfTile, 0 - halfTile), Quaternion.identity);
            outerWalls[0].name = "Wall Back";
            outerWalls[0].transform.parent = mazeHolder.transform;
        }

        //front wall
        if (outerWalls[1] == null)
        {
            outerWalls[1] = Instantiate(mazePart, new Vector3(0, halfTile, 0 - halfTile), Quaternion.identity);
            outerWalls[1].name = "Wall Front";
            outerWalls[1].transform.parent = mazeHolder.transform;
        }

        //left wall
        if (outerWalls[2] == null)
        {
            outerWalls[2] = Instantiate(mazePart, new Vector3(0 - halfTile, halfTile, 0), Quaternion.identity);
            outerWalls[2].name = "Wall Left";
            outerWalls[2].transform.Rotate(Vector3.up, 90f);
            outerWalls[2].transform.parent = mazeHolder.transform;
        }

        //right wall
        if (outerWalls[3] == null)
        {
            outerWalls[3] = Instantiate(mazePart, new Vector3(0, halfTile, 0 - halfTile), Quaternion.identity);
            outerWalls[3].name = "Wall Right";
            outerWalls[3].transform.Rotate(Vector3.up, 90f);
            outerWalls[3].transform.parent = mazeHolder.transform;
        }
    }

    private void ResizeOuterWalls()
    {
        float halfTile = tileSize / 2;

        //back wall
        //set location
        Vector3 tempLocation = outerWalls[0].transform.position;
        tempLocation.x = cols * tileSize / 2 - halfTile;
        outerWalls[0].transform.position = tempLocation;

        //set scale
        Vector3 tempScale = outerWalls[0].transform.localScale;
        tempScale.x = cols * tileSize;
        outerWalls[0].transform.localScale = tempScale;


        //front wall
        //set location
        tempLocation.z = rows * tileSize - halfTile;
        outerWalls[1].transform.position = tempLocation;

        //set scale
        outerWalls[1].transform.localScale = tempScale;


        //left wall
        //set location
        tempLocation = outerWalls[2].transform.position;
        tempLocation.z = rows * tileSize / 2 - halfTile;
        outerWalls[2].transform.position = tempLocation;

        //set scale
        tempScale.x = rows * tileSize;
        outerWalls[2].transform.localScale = tempScale;


        //right wall
        //set location
        tempLocation.x = cols * tileSize - halfTile;
        outerWalls[3].transform.position = tempLocation;

        //set scale
        outerWalls[3].transform.localScale = tempScale;
    }

    public void GenerateMaze()
    {
        if(mazeExists == true)
        {
            StopAllCoroutines();
            GenerateGrid(cols, rows);
            mazeExists = false;
        }

        //PrimAlgorithm primAlgorithm = new PrimAlgorithm(mazeCells);
        //StartCoroutine(primAlgorithm.GenerateMaze((int)cols, (int)rows));

        RecursiveBacktrackAlgorithm rbAlgorithm = new RecursiveBacktrackAlgorithm(mazeCells);
        StartCoroutine(rbAlgorithm.GenerateMaze((int)cols, (int)rows));

        mazeExists = true;
    }
}
