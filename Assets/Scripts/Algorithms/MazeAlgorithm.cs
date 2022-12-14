using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeAlgorithm
{
    protected MazeCell[,] mazeCells;

    protected MazeAlgorithm(MazeCell[,] mazeCells) : base()
    {
        this.mazeCells = mazeCells;
    }

    public abstract IEnumerator GenerateMaze(int cols, int rows);
}
