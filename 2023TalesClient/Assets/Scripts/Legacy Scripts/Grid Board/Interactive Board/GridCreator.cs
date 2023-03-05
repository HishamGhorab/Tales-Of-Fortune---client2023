using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshRenderer))]
public class GridCreator : MonoBehaviour
{
    [FormerlySerializedAs("boardSize")]
    [Header("Grid Attributes")]
    public Vector2 gridSize;
    [Range(0,1)] [SerializeField] private float gridLineThickness;
    [SerializeField] private Color gridColor;

    [Header("Editor Debugging")] 
    [SerializeField] private bool updateLive = false;
    
    private Renderer renderer;
    private GameObject plane;
    
    private List<GameObject> pieceObjects;

    
    void Start()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
        
        SetGridValues(gridSize, gridColor, gridLineThickness);
        CreatePlane(gridSize);
        
        if(!updateLive)
            this.enabled = false;
    }

    private void Update()
    {
        //This update loop is currently only used for debugging!
        
        if (!updateLive)
            return;
        
        SetGridValues(gridSize, gridColor, gridLineThickness);
        plane.transform.localScale = new Vector3(gridSize.x / 10, 1, gridSize.y / 10);
        plane.transform.position = new Vector3((gridSize.x/2) - 0.5f, 0.01f, (gridSize.y/2) - 0.5f);
    }

    void SetGridValues(Vector4 _gridSize, Color _gridColor, float _gridLineThickness)
    {
        renderer.material.SetVector("gridShaderTilingValue", _gridSize);
        renderer.material.SetColor("gridShaderLineColor", _gridColor);
        renderer.material.SetFloat("gridShaderLineThickness", _gridLineThickness);
    }
    
    GameObject CreatePlane(Vector2 _gridCreator)
    {
        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        
        plane.transform.parent = gameObject.transform;
        plane.transform.localScale = new Vector3(gridSize.x / 10, 1, gridSize.y / 10);
        plane.transform.position = new Vector3((gridSize.x/2) - 0.5f, 0.01f, (gridSize.y/2) - 0.5f);
        plane.GetComponent<MeshRenderer>().material = renderer.material;

        return plane;
    }
}
