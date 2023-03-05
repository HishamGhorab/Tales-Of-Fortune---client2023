using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour, IBoardListener
{
    //public enum Movements {UNSET, FORWARD, RIGHT, LEFT}
    //public Movements[] movements = new Movements[4];

    //public GameObject[] shipsInGame;
    public GameObject[] controllerUI;
    GameObject[,] moveOrder;
    //public GameObject[] moveOrderPlayer2;

    GameObject[,] shootOrder;
    //public GameObject[] shootOrderPlayer2;

    //int index;
    int playerAmount = 2;

    private IBoardModel boardModel = BoardModel.Instance;
    BoardView boardView;
    TimeHandler timeHandler;

    int shipRot = int.MaxValue;

    bool executeMove = false;
    public bool ExecuteMove { get { return executeMove; } set { executeMove = value; } }

    public Text roundText;
    public Text segmentText;
    public Text roundLength;
    //public Text segmentLength;

    void Start()
    {
        boardModel.AddListener(this);
        boardView = BoardView.Instance;

        moveOrder = new GameObject[playerAmount,4]; //2 and 4
        shootOrder = new GameObject[playerAmount, 8];

        SetControllerToMoveList();

        timeHandler = TimeHandler.Instance;
        timeHandler.roundState = TimeHandler.RoundState.PreRound;

        executeMove = true; //temp
    }

    void Update()
    {
        //timeHandler.roundStatText.text = timeHandler.roundState.ToString();
        roundText.text = "Round:" + timeHandler.round;
        roundLength.text = timeHandler.timeBetweenRounds.ToString();
        segmentText.text = "Segment:" + timeHandler.Segment;
        //segmentLength.text = timeHandler.timeBetweenSegments.ToString();

        if (timeHandler.roundState == TimeHandler.RoundState.PreRound)
        {
            StartCoroutine(timeHandler.IncreaseRound(timeHandler.timeBetweenRounds)); // is there an error here?
            timeHandler.roundState = TimeHandler.RoundState.WaitingForRound;
        }

        //THIS PLAYS SEGMENTS
        if (timeHandler.roundState == TimeHandler.RoundState.PlayingSegment) //hardcoded change in future //move 1
        {
            if(timeHandler.Segment > 4)
            {
                timeHandler.Segment = 1;
                timeHandler.roundState = TimeHandler.RoundState.PreRound;
                return;
            }

            if (executeMove)
            {
                for (int i = 0; i < playerAmount; i++)
                {
                    //Add StartPositions
                    boardModel.PlayersEndPositionsSegment[0].Add(boardView.shipModel[i].transform.position);
                    //Add EndPositions
                    boardModel.PlayersEndPositionsSegment[1].Add(
                        GetPieceEndPosition(moveOrder[i, timeHandler.Segment - 1].GetComponent<Text>().text, boardView.shipModel[i]));
                }

                for (int i = 0; i < playerAmount; i++)
                {
                    string crashKey;
                    crashKey = boardModel.PieceCrashCheck(boardView.shipModel[i].transform.position,  
                        GetPieceEndPosition(moveOrder[i, timeHandler.Segment - 1].GetComponent<Text>().text, boardView.shipModel[i]),  
                            (int)boardView.shipModel[i].transform.eulerAngles.y, boardModel.PlayersEndPositionsSegment, i);
    
                    //Debug.Log(crashKey + moveOrder[i, timeHandler.Segment - 1].GetComponent<Text>().text);

                    PlayTurn(crashKey + moveOrder[i, timeHandler.Segment - 1].GetComponent<Text>().text, boardView.shipModel[i], i,
                        shootOrder[i, timeHandler.Segment - 1].GetComponent<Toggle>().isOn, shootOrder[i, timeHandler.Segment + 3].GetComponent<Toggle>().isOn);
                }
                executeMove = false;
            }
        }
    }

    Vector3 GetPieceEndPosition(string move, GameObject shipObject)
    {
        switch(move)
        {
            case "S":
                return shipObject.transform.position;
            case "F":
                switch(shipObject.transform.localEulerAngles.y)
                {
                    case 0: return new Vector3(shipObject.transform.position.x, shipObject.transform.position.y, shipObject.transform.position.z + 1);
                    case 90: return new Vector3(shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z);
                    case 180: return new Vector3(shipObject.transform.position.x, shipObject.transform.position.y, shipObject.transform.position.z - 1);
                    case 270: return new Vector3(shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z);
                } break; 
            case "R":
                if (shipObject.transform.localEulerAngles.y > 0 && shipObject.transform.localEulerAngles.y < 89)
                {
                    shipObject.transform.rotation = Quaternion.Euler(shipObject.transform.rotation.x, 0, shipObject.transform.rotation.x);
                }
                shipRot = (int)shipObject.transform.eulerAngles.y + 90; //next pos;
                if(shipRot == 360) shipRot = 0; // NOTE: Needed for end rotation to work properly

                switch(shipObject.transform.localEulerAngles.y)
                {
                    case 0: return new Vector3(shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z + 1);
                    case 90: return new Vector3(shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z - 1);
                    case 180: return new Vector3(shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z - 1);
                    case 270: return new Vector3(shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z + 1);
                } break;
            case "L":
                if (shipObject.transform.localEulerAngles.y > 0 && shipObject.transform.localEulerAngles.y < 89)
                {
                    shipObject.transform.rotation = Quaternion.Euler(shipObject.transform.rotation.x, 0, shipObject.transform.rotation.x);
                }
                shipRot = (int)shipObject.transform.eulerAngles.y - 90; //next pos;
                if(shipRot == 360) shipRot = 0; // NOTE: Needed for end rotation to work properly

                switch(shipObject.transform.localEulerAngles.y)
                {
                    case 0: return new Vector3(shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z + 1);
                    case 90: return new Vector3(shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z + 1);
                    case 180: return new Vector3(shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z - 1);
                    case 270: return new Vector3(shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z - 1);
                }break; 
        }
        return new Vector3();
    }
    
    
    //todo : use TOFMath.Approximation to round to correct rotation
    void PlayTurn(string move, GameObject shipObject, int shipIndex, bool rightCannon, bool leftCannon)
    {
        switch(move)
        {
            case "S": case "CFS": case "CDS":
                boardModel.MovePiece(shipObject, shipObject.transform.position, shipObject.transform.position, int.MaxValue, "still", shipIndex,
                rightCannon, leftCannon);
                break;
            case "F":
                switch(shipObject.transform.localEulerAngles.y)
                {
                    case 0:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x, shipObject.transform.position.y, shipObject.transform.position.z + 1), int.MaxValue, "forward", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 90:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z), int.MaxValue, "forward", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 180:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x, shipObject.transform.position.y, shipObject.transform.position.z - 1), int.MaxValue, "forward", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 270:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z), int.MaxValue, "forward", shipIndex,
                    rightCannon, leftCannon);
                    break;
                } break; 
            case "R":
                if (shipObject.transform.localEulerAngles.y > 0 && shipObject.transform.localEulerAngles.y < 89)
                {
                    shipObject.transform.rotation = Quaternion.Euler(shipObject.transform.rotation.x, 0, shipObject.transform.rotation.x);
                }
                shipRot = (int)shipObject.transform.eulerAngles.y + 90; //next pos;
                if(shipRot == 360) shipRot = 0; // NOTE: Needed for end rotation to work properly

                switch(shipObject.transform.localEulerAngles.y)
                {
                    case 0:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z + 1), shipRot, "right", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 90:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z - 1), shipRot, "right", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 180:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z - 1), shipRot, "right", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 270:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z + 1), shipRot, "right", shipIndex,
                    rightCannon, leftCannon);
                    break;
                } break;
            case "L":
                if (shipObject.transform.localEulerAngles.y > 0 && shipObject.transform.localEulerAngles.y < 89)
                {
                    shipObject.transform.rotation = Quaternion.Euler(shipObject.transform.rotation.x, 0, shipObject.transform.rotation.x);
                }
                shipRot = (int)shipObject.transform.eulerAngles.y - 90; //next pos;
                if(shipRot == 360) shipRot = 0; // NOTE: Needed for end rotation to work properly

                switch(shipObject.transform.localEulerAngles.y)
                {
                    case 0:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z + 1), shipRot, "left", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 90:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z + 1), shipRot, "left", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 180:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z - 1), shipRot, "left", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 270:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z - 1), shipRot, "left", shipIndex,
                    rightCannon, leftCannon);
                    break;
                } break;
            case "CFF":
                boardModel.MovePiece(shipObject, shipObject.transform.position, shipObject.transform.position, shipRot, "CFForward", shipIndex,
                rightCannon, leftCannon);
                break;
            case "CFR":
                if (shipObject.transform.localEulerAngles.y > 0 && shipObject.transform.localEulerAngles.y < 89)
                {
                    shipObject.transform.rotation = Quaternion.Euler(shipObject.transform.rotation.x, 0, shipObject.transform.rotation.x);
                }
                shipRot = (int)shipObject.transform.eulerAngles.y + 90; //next pos;
                if(shipRot == 360) shipRot = 0;

                boardModel.MovePiece(shipObject, shipObject.transform.position, shipObject.transform.position, shipRot, "CFRight", shipIndex,
                rightCannon, leftCannon);
            break;
            case "CFL":
                if (shipObject.transform.localEulerAngles.y > 0 && shipObject.transform.localEulerAngles.y < 89)
                {
                    shipObject.transform.rotation = Quaternion.Euler(shipObject.transform.rotation.x, 0, shipObject.transform.rotation.x);
                }
                shipRot = (int)shipObject.transform.eulerAngles.y - 90; //next pos;
                if(shipRot == 360) shipRot = 0;
                boardModel.MovePiece(shipObject, shipObject.transform.position, shipObject.transform.position, shipRot, "CFLeft", shipIndex,
                rightCannon, leftCannon);
            break;
            case "CDR":

                if (shipObject.transform.localEulerAngles.y > 0 && shipObject.transform.localEulerAngles.y < 89)
                {
                    shipObject.transform.rotation = Quaternion.Euler(shipObject.transform.rotation.x, 0, shipObject.transform.rotation.x);
                }
                shipRot = (int)shipObject.transform.eulerAngles.y + 90; //next pos;
                if(shipRot == 360) shipRot = 0; // NOTE: Needed for end rotation to work properly

                switch(shipObject.transform.localEulerAngles.y)
                {
                    case 0:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x, shipObject.transform.position.y, shipObject.transform.position.z + 1), shipRot, "CDRight", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 90:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z), shipRot, "CDRight", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 180:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x, shipObject.transform.position.y, shipObject.transform.position.z - 1), shipRot, "CDRight", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 270:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z), shipRot, "CDRight", shipIndex,
                    rightCannon, leftCannon);
                    break;
                }
            break;
            case "CDL":

                if (shipObject.transform.localEulerAngles.y > 0 && shipObject.transform.localEulerAngles.y < 89)
                {
                    shipObject.transform.rotation = Quaternion.Euler(shipObject.transform.rotation.x, 0, shipObject.transform.rotation.x);
                }
                shipRot = (int)shipObject.transform.eulerAngles.y - 90; //next pos;
                if(shipRot == 360) shipRot = 0; // NOTE: Needed for end rotation to work properly

                switch(shipObject.transform.localEulerAngles.y)
                {
                    case 0:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x, shipObject.transform.position.y, shipObject.transform.position.z + 1), shipRot, "CDLeft", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 90:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z), shipRot, "CDLeft", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 180:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x, shipObject.transform.position.y, shipObject.transform.position.z - 1), shipRot, "CDLeft", shipIndex,
                    rightCannon, leftCannon);
                    break;
                    case 270:
                    boardModel.MovePiece(shipObject, shipObject.transform.position, new Vector3(
                    shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z), shipRot, "CDLeft", shipIndex,
                    rightCannon, leftCannon);
                    break;
                }
            break;
        }
    }

    void SetControllerToMoveList() //this just references to the controller that each player has.
    {
        for (int i = 0; i < playerAmount; i++) //loops through players
        {
            for (int j = 0; j < moveOrder.GetLength(1); j++)
            {
                //Moves
                moveOrder[i, j] = controllerUI[i].transform.Find("MoveInputs/Move" + (j + 1) + "/Button/Text").gameObject;

                //Cannons
                shootOrder[i, j] = controllerUI[i].transform.Find("CannonInputs/RightCannon" + (j + 1)).gameObject;
                shootOrder[i, j + 4] = controllerUI[i].transform.Find("CannonInputs/LeftCannon" + (j + 1)).gameObject;
            }
        }
    }

    public void PlacePiece(Vector3 pos, Piece piece) { }
    public void MovePiece(GameObject shipObject, Vector3 endPos, int rotation, string turn, int shipIndex, bool rightCannon, bool leftCannon) { }
    
}
