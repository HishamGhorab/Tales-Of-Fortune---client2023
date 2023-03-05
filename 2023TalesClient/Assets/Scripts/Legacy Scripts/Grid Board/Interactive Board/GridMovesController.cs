using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovesController : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;

    private int gridPieceLayer;

    private void Start()
    {
        gridPieceLayer = LayerMask.GetMask("Grid Piece");
    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, gridPieceLayer))
        {
            
        }
        
    }
}


