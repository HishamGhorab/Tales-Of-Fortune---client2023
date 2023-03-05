using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent((typeof(GridCreator)))]
public class GridInteraction : MonoBehaviour
{
    private GridCreator gridCreator;
    private Vector2 gridSize;

    private void Start()
    {
        gridCreator = GetComponent<GridCreator>();
        gridSize = gridCreator.gridSize;
        
        //loops through all the grid positions and creates a piece object at every position
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                CreatePieceObject(x, y);
            } 
        }
    }

    void CreatePieceObject(float x, float y)
    {
        GameObject gridPieceObject = new GameObject();
        gridPieceObject.name = $"Piece: {x},{y}";
        gridPieceObject.transform.position = new Vector3(x, 0, y);
        gridPieceObject.transform.parent = gameObject.transform;
        gridPieceObject.layer = 6;

        BoxCollider thisCollider = gridPieceObject.AddComponent<BoxCollider>();
        GridPiece thisGridPiece = gridPieceObject.AddComponent<GridPiece>();

        thisCollider.isTrigger = true;
        thisCollider.size = new Vector3(1, 0.1f, 1);
        
        thisGridPiece.boardPosition = new Vector2(x, y);
    }
}
