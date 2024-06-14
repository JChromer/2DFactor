using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public int gridWidth = 7;
    public int gridHeight = 10; 
    [SerializeField] Grid grid;

    void Start()
    {
        createGrid();
    }

    void createGrid()
    {
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

        grid.transform.localScale = new Vector3(cellWidth, cellHeight, -10f);

        float gridOffsetX = grid.transform.localScale.x ;    
        float gridOffsetY = grid.transform.localScale.y ;

        for (int y = 0; y < gridHeight; y++)
        {
            for(int x = 0; x< gridWidth; x++)
            {
                Grid newGrid = Instantiate(grid, new Vector3((leftPos - cellWidth / 2) - (cellWidth * x), (linePos + cellHeight / 2) + (cellHeight * y), -10f), Quaternion.identity);
                newGrid.posX = x;
                newGrid.posY = y;
            }
        }
    }
}
