using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawGrid : MonoBehaviour
{
    public int gridWidth = 3;
    public int gridHeight = 3; 
    public Color gridColor = Color.green;
    [SerializeField] GridScript script;
    

    private void OnDrawGizmos()
    {
        Gizmos.color = gridColor;

        GameObject wallRight = GameObject.FindWithTag("Wall_right");
        GameObject wallLeft = GameObject.FindWithTag("Wall_left");
        GameObject wallTop = GameObject.FindWithTag("Wall_top");
        GameObject line = GameObject.FindWithTag("Line");

        float topPos = wallTop.transform.position.y - (wallTop.GetComponent<Renderer>().bounds.size / 2).y;
        float linePos = line.transform.position.y + (line.GetComponent<Renderer>().bounds.size / 2).y;

        float leftPos = wallLeft.transform.position.x + (wallLeft.GetComponent<Renderer>().bounds.size / 2).x;
        float rightPos = wallRight.transform.position.x - (wallRight.GetComponent<Renderer>().bounds.size / 2).x;

        float cellHeight = (topPos - linePos) / gridHeight;
        float cellWidth = (leftPos - rightPos) / gridWidth;

        for (int y = 0; y <= gridHeight; y++)
        {
            Gizmos.DrawLine(
                new Vector3(leftPos, 
                linePos + cellHeight * y, -10f), 
                new Vector3(rightPos , linePos + cellHeight * y, -10f)
            );
        }

        for (int x = 0; x <= gridWidth; x++)
        {
            Gizmos.DrawLine(
                new Vector3(leftPos - cellWidth * x,
                linePos, -10f),

                new Vector3(leftPos - cellWidth * x, topPos, -10f)
            );
        }

    }
}
