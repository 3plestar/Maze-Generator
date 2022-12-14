using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCamera : MonoBehaviour
{
    public void ChangeLocation(float cols, float rows, float tileSize)
    {
        tileSize /= 2;
        transform.position = new Vector3(
            cols * tileSize - tileSize,
            Mathf.Max(cols, rows) * 2,
            rows * tileSize - tileSize);
    }
}
