using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimAlgorithm : MazeAlgorithm
{
    private int currentCol = 0;
    private int currentRow = 0;

    private int maxCols, maxRows;

    List<KeyValuePair<int, int>> adjacents;

    public PrimAlgorithm(MazeCell[,] mazeCells) : base(mazeCells)
    { 
    }

    public override IEnumerator GenerateMaze(int cols, int rows)
    {
        adjacents = new List<KeyValuePair<int, int>>();

        maxCols = cols;
        maxRows = rows;

        mazeCells[currentCol, currentRow].visited = true;

        ListAdjacent();

        while (adjacents.Count > 0)
        {
            SelectRandomAdjacent();

            DestroyWalls();
            ListAdjacent();
            yield return null;
        }

    }

    private void SelectRandomAdjacent()
    { 
            int random = Random.Range(0, adjacents.Count);

            currentCol = adjacents[random].Key;
            currentRow = adjacents[random].Value;

            mazeCells[currentCol, currentRow].visited = true;

            adjacents.RemoveAt(random);
    }

    private void DestroyWalls()
    {
        bool searchingForWall = true;
        while (searchingForWall)
        {
            int random = Random.Range(1, 5);
            if(!CellIsAvailable(currentCol, currentRow + 1, true) && !CellIsAvailable(currentCol + 1, currentRow, true)&&
               !CellIsAvailable(currentCol, currentRow - 1, true) && !CellIsAvailable(currentCol - 1, currentRow, true))
            {
                searchingForWall = false;
            }
            //foreward
            else if (random==1 && CellIsAvailable(currentCol, currentRow + 1,true))
            {
                DestroyWallIfExist(mazeCells[currentCol, currentRow].wallForeward);
                searchingForWall = false;
            }
            //right
            else if (random == 2 && CellIsAvailable(currentCol + 1, currentRow,true))
            {
                DestroyWallIfExist(mazeCells[currentCol, currentRow].wallRight);
                searchingForWall = false;
            }
            //back
            else if (random == 3 && CellIsAvailable(currentCol, currentRow - 1,true))
            {
                DestroyWallIfExist(mazeCells[currentCol, currentRow - 1].wallForeward);
                searchingForWall = false;
            }
            //left
            else if (random == 4 && CellIsAvailable(currentCol - 1, currentRow,true))
            {
                DestroyWallIfExist(mazeCells[currentCol - 1, currentRow].wallRight);
                searchingForWall = false;
            }
        }
        
    }

    

    private void ListAdjacent()
    {
        //foreward
        if (CellIsAvailable(currentCol, currentRow + 1, false))
        {
            if (newAdjacent(currentCol, currentRow + 1))
            {
                adjacents.Add(new KeyValuePair<int, int>(currentCol, currentRow +1));
            }
        }
        //right
        if (CellIsAvailable(currentCol + 1, currentRow,false ))
        {
            if (newAdjacent(currentCol + 1, currentRow))
            {
                adjacents.Add(new KeyValuePair<int, int>(currentCol +1, currentRow));
            }
        }
        //back
        if (CellIsAvailable(currentCol, currentRow - 1,false))
        {
            if (newAdjacent(currentCol, currentRow - 1))
            {
                adjacents.Add(new KeyValuePair<int, int>(currentCol, currentRow - 1));
            }
        }
        //left
        if (CellIsAvailable(currentCol - 1, currentRow, false))
        {
            if (newAdjacent(currentCol - 1, currentRow))
            {
                adjacents.Add(new KeyValuePair<int, int>(currentCol-1, currentRow));
            }
                
        }
    }
    private bool CellIsAvailable(int col, int row, bool visited)
    {
        if (row >= 0 && col >= 0 && col < maxCols && row < maxRows && mazeCells[col, row].visited == visited)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool newAdjacent(int col, int row)
    {
        for (int i = 0; i < adjacents.Count; i++)
        {
            if(adjacents[i].Key == col && adjacents[i].Value == row)
            {
                return false;
            }
        }
        return true;
    }

    private void DestroyWallIfExist(GameObject wall)
    {
        if(wall != null)
        {
            GameObject.Destroy(wall);
            //wall.SetActive(false);
        }
    }
}
