using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public List<GameObject> listOfShips;
    public List<Vector3> endPositionsForThisSegment;
    public Piece[,] pieceBoard;

    public State Clone()
    {
        State s = new State(); //create a new state

        s.pieceBoard = new Piece[16, 16]; //intilize a board 16*16 on the new copy

        for (int y = -8; y <= 8; y++)
        {
            for (int x = -8; x <= 8; x++)
            {
                s.pieceBoard[x, y] = pieceBoard[x, y]; //copy everything in the past board
            }
        }
        return s;
    }

    public List<State> Expand(Player player)
    {
        List<State> stateList = new List<State>();


        for(int i = 0; i < 4; i++)
        {

        }

        //find all the ships within a specific range
        //make predictions for ships inside that range
        //create a clone and make a 3 moves based on that prediction

        return stateList;
    }

    public float Score()
    {
        //todo
        return 0;
    }

    Vector3 GetPieceEndPosition(string move, GameObject shipObject, int shipRot)
    {
        switch (move)
        {
            case "S":
                return shipObject.transform.position;
            case "F":
                switch (shipObject.transform.localEulerAngles.y)
                {
                    case 0: return new Vector3(shipObject.transform.position.x, shipObject.transform.position.y, shipObject.transform.position.z + 1);
                    case 90: return new Vector3(shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z);
                    case 180: return new Vector3(shipObject.transform.position.x, shipObject.transform.position.y, shipObject.transform.position.z - 1);
                    case 270: return new Vector3(shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z);
                }
                break;
            case "R":
                if (shipObject.transform.localEulerAngles.y > 0 && shipObject.transform.localEulerAngles.y < 89)
                {
                    shipObject.transform.rotation = Quaternion.Euler(shipObject.transform.rotation.x, 0, shipObject.transform.rotation.x);
                }
                shipRot = (int)shipObject.transform.eulerAngles.y + 90; //next pos;
                if (shipRot == 360) shipRot = 0; // NOTE: Needed for end rotation to work properly

                switch (shipObject.transform.localEulerAngles.y)
                {
                    case 0: return new Vector3(shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z + 1);
                    case 90: return new Vector3(shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z - 1);
                    case 180: return new Vector3(shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z - 1);
                    case 270: return new Vector3(shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z + 1);
                }
                break;
            case "L":
                if (shipObject.transform.localEulerAngles.y > 0 && shipObject.transform.localEulerAngles.y < 89)
                {
                    shipObject.transform.rotation = Quaternion.Euler(shipObject.transform.rotation.x, 0, shipObject.transform.rotation.x);
                }
                shipRot = (int)shipObject.transform.eulerAngles.y - 90; //next pos;
                if (shipRot == 360) shipRot = 0; // NOTE: Needed for end rotation to work properly

                switch (shipObject.transform.localEulerAngles.y)
                {
                    case 0: return new Vector3(shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z + 1);
                    case 90: return new Vector3(shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z + 1);
                    case 180: return new Vector3(shipObject.transform.position.x + 1, shipObject.transform.position.y, shipObject.transform.position.z - 1);
                    case 270: return new Vector3(shipObject.transform.position.x - 1, shipObject.transform.position.y, shipObject.transform.position.z - 1);
                }
                break;
        }
        return new Vector3();
    }

}
