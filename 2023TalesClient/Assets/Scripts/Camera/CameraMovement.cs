using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    private Camera cam;
    private CameraActions cameraActions;

    private Vector3 camStartPos;

    [Header("Limitations")]
    [SerializeField] bool canKeyMove;

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private float moveTime = 4;
    [SerializeField] private float xPanThreshold = 3;
    [SerializeField] private float yBotPanThreshold = 0.5f;
    [SerializeField] private float yTopPanThreshold = 10;
    private Vector2 maxMoveValue;
    private Vector3 newMovePos;
    private bool lerpMovement = false;

    [Header("Zoom Settings")] 
    [SerializeField] private float zoomMinMax = 4;
    [SerializeField] private float xRotMinMax = 3;
    [SerializeField] private AnimationCurve zoomRotationCurve;
    private float scrollInputValue;
    private bool lerpZoomMovement = false;
    private Vector3 newScrollPos;

    [Header("Move To Settings")] 
    //[SerializeField] private List<GameObject> players;
    [SerializeField] private float zOffset = 3;
    [SerializeField] private float moveTowardSpeed = 4;
    private Vector2 xyInputValue;
    private bool allowSwap = true;
    private bool doMoveTo = false;
    
    [Header("Drag Settings")]
    [SerializeField] private float dragSpeed = 2;
    private Vector3 dragOrigin;
    private Vector3 dragDifference;
    private bool drag = false;

    private static CameraMovement _singleton;
    public static CameraMovement Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(CameraMovement)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    void Start()
    {
        maxMoveValue = new Vector2(16, 16);
        
        camStartPos = transform.position;
        newMovePos = transform.position;
        newScrollPos = transform.position;
        
        zoomRotationCurve = ZoomRotCurve(zoomMinMax, xRotMinMax);

        cam = Camera.main;
        cameraActions = new CameraActions();
        cameraActions.CameraControls.Enable();
        cameraActions.CameraControls.MouseScroll.performed += ScrollZoom;
        //cameraActions.CameraControls.PlayersNumbers.performed += MoveToPlayer;
    }

    void Update()
    {
        xyInputValue = cameraActions.CameraControls.xyMovement.ReadValue<Vector2>();
        
        MinMaxPanning();
    }

    void LateUpdate()
    {
        CameraKeyMovement(xyInputValue);
        Zoom(scrollInputValue);
        
        //DragMove();
        
        if (Input.GetMouseButtonDown(1))
        {
            ResetCamera(camStartPos);
        }
        
        RotateXOnZoom();
    }
    
    public void MoveToPlayer(int id)
    {
        int value = id;
        
        if (value > 0 && value <= TOFPlayer.playerShipObjects.Count)
        {
            GameObject go = TOFPlayer.playerShipObjects[Convert.ToUInt16(value)].gameObject;
            StartCoroutine(MoveToPlayerSmooth(go));
        }
    }
    void MoveToPlayer(InputAction.CallbackContext context)
    {
        int value = (int)context.ReadValue<float>();
        
        if (value > 0 && value <= TOFPlayer.playerShipObjects.Count)
        {
            GameObject go = TOFPlayer.playerShipObjects[Convert.ToUInt16(value)].gameObject;
            StartCoroutine(MoveToPlayerSmooth(go));
        }
    }
    
    public void SetCameraKeyMovement(bool state)
    {
        canKeyMove = state;
    }

    void CameraKeyMovement(Vector2 _xyInputValue)
    {
        if (!canKeyMove)
            return;

        if (transform.position.x == 0 + xPanThreshold && _xyInputValue.x == -1
            || transform.position.x == maxMoveValue.x - xPanThreshold && _xyInputValue.x == 1
            || transform.position.z == 0 - yBotPanThreshold && _xyInputValue.y == -1
            || transform.position.z == maxMoveValue.y - yTopPanThreshold && _xyInputValue.y == 1)
                return;
        
        if(xyInputValue != new Vector2(0, 0) && !doMoveTo) 
        {
            newMovePos = new Vector3(transform.position.x + _xyInputValue.x * moveSpeed, transform.position.y,
            transform.position.z + _xyInputValue.y * moveSpeed);
            
            lerpZoomMovement = false;
            lerpMovement = true;
        }

        if(lerpMovement)
        {
            transform.position = Vector3.Lerp(transform.position, newMovePos, Time.deltaTime * moveTime);
        }
    }

    void MinMaxPanning()
    {
        float xPos = Mathf.Clamp(transform.position.x, 0 + xPanThreshold, maxMoveValue.x - xPanThreshold);
        float zPos = Mathf.Clamp(transform.position.z, 0 - yBotPanThreshold, maxMoveValue.y - yTopPanThreshold);
        
        float xNewPos = Mathf.Clamp(newMovePos.x, 0 + xPanThreshold, maxMoveValue.x - xPanThreshold);
        float zNewPos = Mathf.Clamp(newMovePos.z, 0 - yBotPanThreshold, maxMoveValue.y - yTopPanThreshold);

        float zoom = Mathf.Clamp(transform.position.y, camStartPos.y - zoomMinMax, camStartPos.y + zoomMinMax);
        
        transform.position = new Vector3(xPos, zoom, zPos);
        newMovePos = new Vector3(xNewPos, transform.position.y, zNewPos);
    }
    
    void ScrollZoom(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            scrollInputValue = context.ReadValue<float>();
        }
    }

    void DragMove()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.transform.position.y;

        if (Input.GetMouseButton(0))
        {
            dragDifference = cam.ScreenToWorldPoint(mousePos) - cam.transform.position;
            if (!drag)
            {
                drag = true;
                dragOrigin = cam.ScreenToWorldPoint(mousePos);
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            drag = false;
        }

        if (drag)
        {
            cam.transform.position = new Vector3(dragOrigin.x - dragDifference.x, cam.transform.position.y,
                dragOrigin.z - dragDifference.z);
        }
    }

    void ResetCamera(Vector3 startPos)
    {
        transform.position = startPos;
    }

    void Zoom(float _value)
    {
        if (transform.position.y == camStartPos.y - zoomMinMax && _value > 0
            || transform.position.y == camStartPos.y + zoomMinMax && _value < 0)
        {
            lerpZoomMovement = false;
            return;
        }
        
        if (_value != 0)
        {
            newScrollPos = transform.position;

            if (_value < 0)
            {
                newScrollPos += new Vector3(0, 1 * moveSpeed, 0);
            }
            else if(_value > 0)
            {
                newScrollPos -= new Vector3(0, 1 * moveSpeed, 0);
            }

            lerpZoomMovement = true;
            lerpMovement = false;
        }

        if (lerpZoomMovement)
        {
            transform.position = Vector3.Lerp(transform.position, newScrollPos, Time.deltaTime * moveTime);
        }
    }

    AnimationCurve ZoomRotCurve(float _zoomMinMax, float rotationMinMax)
    {
        AnimationCurve curve = new AnimationCurve(new Keyframe(transform.position.y - zoomMinMax, transform.rotation.eulerAngles.x - rotationMinMax),
            new Keyframe(transform.position.y + zoomMinMax, transform.rotation.eulerAngles.x + rotationMinMax));
        
        return curve;
    }

    void RotateXOnZoom()
    {
        transform.rotation = Quaternion.Euler(zoomRotationCurve.Evaluate(transform.position.y), transform.rotation.y, transform.rotation.z);
    }

    IEnumerator MoveToPlayerSmooth(GameObject _moveToObj)
    {
        if(allowSwap)
        {
            Vector3 moveTowardThis = _moveToObj.transform.position;
            
            lerpMovement = false;
            lerpZoomMovement = false;
            allowSwap = false;
            
            if(moveTowardThis.x < 0 + xPanThreshold)
            {
                moveTowardThis = new Vector3(0 + xPanThreshold, moveTowardThis.y, moveTowardThis.z);
            }
            else if(moveTowardThis.x > maxMoveValue.x - xPanThreshold)
            {
                moveTowardThis = new Vector3(maxMoveValue.x - xPanThreshold, moveTowardThis.y,
                    moveTowardThis.z);
            }
                
            if(moveTowardThis.z - zOffset < 0 - yBotPanThreshold)
            {
                moveTowardThis = new Vector3(moveTowardThis.x, moveTowardThis.y, 0 - yBotPanThreshold);
            }
            else if(moveTowardThis.z - zOffset > maxMoveValue.y - yTopPanThreshold)
            {
                moveTowardThis = new Vector3(moveTowardThis.x, moveTowardThis.y,
                    maxMoveValue.y - yTopPanThreshold);
            }
            else
            {
                moveTowardThis.z = moveTowardThis.z - zOffset;
            }

            doMoveTo = true;
            while(doMoveTo)
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, moveTowardThis.x, 
                    Time.deltaTime * moveTowardSpeed), transform.position.y, Mathf.Lerp(transform.position.z,
                    moveTowardThis.z, Time.deltaTime * moveTowardSpeed));

                if(CostumMath.Approximation(transform.position.z, moveTowardThis.z, 0.01f)
                   && CostumMath.Approximation(transform.position.x, moveTowardThis.x, 0.01f))
                {
                    transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y,
                        Mathf.RoundToInt(transform.position.z));

                    allowSwap = true;
                    doMoveTo = false;
                }

                yield return new WaitForEndOfFrame();
            }
        }
        yield return null;
    }
}
