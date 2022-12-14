using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveBacktrackAlgorithm : MazeAlgorithm
{
    private int currentCol = 0;
    private int currentRow = 0;

    private int visitedCells = 0;

    private int maxCols, maxRows;

    Stack<KeyValuePair<int, int>> mazeStack = new Stack<KeyValuePair<int, int>>();
    List<int> adjacents;

    public RecursiveBacktrackAlgorithm(MazeCell[,] mazeCells) : base(mazeCells)
    {
    }
    
    public override IEnumerator GenerateMaze(int cols, int rows)
    {
        maxCols = cols;
        maxRows = rows;

        //make sure nothing is visited
        for (int col = 0; col < maxCols; col++)
        {
            for (int row = 0; row < maxRows; row++)
            {
                if(mazeCells[col, row].visited == true)
                {
                    mazeCells[col, row].visited = false;
                }
            }
        }

        //initialize the first current cell
        mazeStack.Push(new KeyValuePair<int, int>(currentCol, currentRow));
        mazeCells[currentCol, currentRow].visited = true;
        visitedCells = 1;

        while (visitedCells < maxCols * maxRows)
        {
            DoAlgorithm();
            yield return null;
        }
        Debug.Log("maze generation complete");
    }

    private void DoAlgorithm()
    {
        //first look if there are adjacent cells
        ListAdjacent();
        if (adjacents.Count > 0)
        {
            //choose a random cell from the adjacent cells, make a path to it and make it the current cell
            int nextCelDir = adjacents[Random.Range(0,adjacents.Count)];
            CreatePath(nextCelDir);
        }
        else
        {
            //backtrack
            mazeStack.Pop();
            currentCol = mazeStack.Peek().Key;
            currentRow = mazeStack.Peek().Value;
        }
    }

    private void ListAdjacent()
    {
        adjacents = new List<int>();

        //fill the list of adjacent cells with numbers that corrospond to the direction of the cells
        //foreward
        if (CellIsAvailable(currentCol, currentRow + 1))
        {
            adjacents.Add(0);
            mazeCells[currentCol, currentRow].wallForeward.SetActive(true);
        }
        //right
        if (CellIsAvailable(currentCol + 1, currentRow))
        {
            adjacents.Add(1);
            mazeCells[currentCol, currentRow].wallRight.SetActive(true);
        }
        //back
        if (CellIsAvailable(currentCol, currentRow - 1))
        {
            adjacents.Add(2);
            mazeCells[currentCol, currentRow - 1].wallForeward.SetActive(true);
        }
        //left
        if (CellIsAvailable(currentCol - 1, currentRow))
        {
            adjacents.Add(3);
            mazeCells[currentCol - 1, currentRow].wallRight.SetActive(true);
        }
    }

    private void CreatePath(int direction)
    {
        //delete a wall between the current cell and the next cell and move to the the next cell
        switch(direction)
        {
            case 0:
                DestroyWallIfExist(mazeCells[currentCol, currentRow].wallForeward);
                moveToCell(currentCol, currentRow + 1);
                break;

            case 1:
                DestroyWallIfExist(mazeCells[currentCol, currentRow].wallRight);
                moveToCell(currentCol + 1, currentRow);
                break;

            case 2:
                DestroyWallIfExist(mazeCells[currentCol, currentRow - 1].wallForeward);
                moveToCell(currentCol, currentRow - 1);
                break;

            case 3:
                DestroyWallIfExist(mazeCells[currentCol - 1, currentRow].wallRight);
                moveToCell(currentCol - 1, currentRow);
                break;
        }
    }

    private void moveToCell(int col, int row)
    {
        currentCol = col;
        currentRow = row;

        //add the new coordinates to the stack and make the cell visited
        mazeStack.Push(new KeyValuePair<int, int>(currentCol, currentRow));

        mazeCells[currentCol, currentRow].visited = true;
        visitedCells ++;
    }

    private bool CellIsAvailable(int col, int row)
    {
        if (row >= 0 && col >= 0 && col < maxCols && row < maxRows && mazeCells[col, row].visited == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DestroyWallIfExist(GameObject wall)
    {
        if (wall != null)
        {
            GameObject.Destroy(wall);
        }
    }
}
