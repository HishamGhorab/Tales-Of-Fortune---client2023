using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardModel : IBoardModel
{
    private static BoardModel instance;

    State s;
    List<IBoardListener> listeners = new List<IBoardListener>();
    //List<List<Vector3>> playersEndPositionsSegment = new List<List<Vector3>>();
    //public List<List<Vector3>> PlayersEndPositionsSegment { get { return playersEndPositionsSegment; } set { playersEndPositionsSegment = value; } }

    List<Vector3>[] playersEndPositionsSegment = new List<Vector3>[2];
    public List<Vector3>[] PlayersEndPositionsSegment { get { return playersEndPositionsSegment; } set { playersEndPositionsSegment = value; } }

    public static BoardModel Instance
    {
        get
        {
            if (instance == null)
                instance = new BoardModel();
            return instance;
        }
    }

    public void StartGame(int numPlayers, int _boardSizeX, int _boardSizeY)
    {

        playersEndPositionsSegment[0] = new List<Vector3>();
        playersEndPositionsSegment[1] = new List<Vector3>();

        s = new State();
        s.pieceBoard = new Piece[_boardSizeX, _boardSizeY]; //this is 16*16, 0-15
        s.listOfShips = new List<GameObject>();

        //Board starts by being empty
        for (int x = 0; x < s.pieceBoard.GetLength(0); x++)
        {
            for (int z = 0; z < s.pieceBoard.GetLength(1); z++)
            {
                s.pieceBoard[x, z] = Piece.Empty;
            }

            //create board
        }

        //sets player ships to positions on board.
        for (int i = 0; i < numPlayers; i++)
        {
            s.pieceBoard[PieceInfo.startingPositions[numPlayers][i].x, PieceInfo.startingPositions[numPlayers][i].y] = Piece.Ship;
        }
    }

    public void AddListener(IBoardListener listener)
    {
        listeners.Add(listener);
    }

    public Piece GetPiece(Vector3 pos)
    {
        return 0;
    }

    public bool MovePiece(GameObject shipObject, Vector3 startPos, Vector3 endPos, int rotation, string turn, int shipIndex, bool rightCannon, bool leftCannon)
    {

        //Debug.Log(rotation);

        //check valid moves in the future so ships dont move as they wish
        shipObject.GetComponent<ShipHandler>().currentPos = new Vector2(endPos.x, endPos.z);

        s.pieceBoard[(int)startPos.x, (int)startPos.z] = Piece.Empty;
        s.pieceBoard[(int)endPos.x, (int)endPos.z] = Piece.Ship;

        listeners[1].MovePiece(shipObject, endPos, rotation, turn, shipIndex, rightCannon, leftCannon);

        return true;
    }

    public void AddShipToStateList(GameObject shipObject)
    {
        s.listOfShips.Add(shipObject);
    }

    public void RemoveHealthFromShip(Vector2 hitPos, int damage)
    {
        for (int i = 0; i < s.listOfShips.Count; i++)
        {
            if(hitPos == s.listOfShips[i].GetComponent<ShipHandler>().currentPos)
            {
                s.listOfShips[i].GetComponent<ShipHandler>().shipHealth -= damage;
                if(s.listOfShips[i].GetComponent<ShipHandler>().CheckDeath())
                {
                    s.pieceBoard[(int)hitPos.x, (int)hitPos.y] = Piece.Empty; // find better solution for this (SetPiece?)
                }
            }
        }
    }

    public string PieceCrashCheck(Vector3 pieceCurrentPos, Vector3 pieceEndPos, int rotation, List<Vector3>[] listOfPositions, int index)
    {//when checking props crashed, use a sepeate list and check after the ship check.
        Vector3 forwardPos = new Vector3(); 
        switch(rotation) //our current rotation. fetch the correct forward position
        {
            case 0: forwardPos = pieceCurrentPos + new Vector3(0,0,1); break;
            case 90: forwardPos = pieceCurrentPos + new Vector3(1,0,0); break;
            case 180: forwardPos = pieceCurrentPos + new Vector3(0,0,-1); break;
            case 270: forwardPos = pieceCurrentPos + new Vector3(-1,0,0); break;
        }

        //if there is a start position infront of our current ship
        for (int i = 0; i < listOfPositions[0].Count; i++)
        {
            if(listOfPositions[0][i] == forwardPos)
            {
                if(i != index)
                {
                    return "CF";
                }
            }
        }

        //if other end position is infront of current ships forward position
        for (int i = 0; i < listOfPositions[1].Count; i++)
        {
            if(listOfPositions[1][i] == forwardPos)
            {
                if(i != index)
                {
                    return "CF";
                }
            }
        }

        //if other end position is in current ships end position
        for (int i = 0; i < listOfPositions[1].Count; i++)
        {
            if(listOfPositions[1][i] == pieceEndPos)
            {
                if(i != index)
                {
                    return "CD";
                }
            }
        }
        return ""; //return no crash
    }
    
    //Checks if a ship (creature) has been hit by a cannon
    public bool ProjectileHitCheck(out Vector2 hitPos, Transform shipTrans, bool rightShot, bool leftShot) //This part misses for double shot, todo.
    {
        Vector3 pos = shipTrans.position;

        if (shipTrans.eulerAngles.y == 0)
        {
            if (rightShot && !leftShot)
            {
                for (int i = 1; i <= 3; i++)
                {

                    if (s.pieceBoard[(int)pos.x + i, (int)pos.z] == Piece.Ship)
                    {
                        hitPos = new Vector2((int)pos.x + i, (int)pos.z);
                        return true;
                    }
                }
            }
            else if (leftShot && !rightShot)
            {
                for (int i = 1; i <= 3; i++)
                {
                    if (s.pieceBoard[(int)pos.x - i, (int)pos.z] == Piece.Ship)
                    {
                        hitPos = new Vector2((int)pos.x - i, (int)pos.z);
                        return true;
                    }
                }
            }
        }
        else if (shipTrans.eulerAngles.y == 90)
        {
            if (rightShot && !leftShot)
            {
                for (int i = 1; i <= 3; i++)
                {
                    if (s.pieceBoard[(int)pos.x, (int)pos.z - i] == Piece.Ship)
                    {
                        hitPos = new Vector2((int)pos.x, (int)pos.z - i);
                        return true;
                    }
                }
            }
            else if (leftShot && !rightShot)
            {
                for (int i = 1; i <= 3; i++)
                {
                    if (s.pieceBoard[(int)pos.x, (int)pos.z + i] == Piece.Ship)
                    {
                        hitPos = new Vector2((int)pos.x, (int)pos.z + i);
                        return true;
                    }
                }
            }
        }
        if (shipTrans.eulerAngles.y == 180)
        {
            if (rightShot && !leftShot)
            {
                for (int i = 1; i <= 3; i++)
                {
                    if (s.pieceBoard[(int)pos.x - i, (int)pos.z] == Piece.Ship)
                    {
                        hitPos = new Vector2((int)pos.x - i, (int)pos.z);
                        return true;
                    }
                }
            }
            else if (leftShot && !rightShot)
            {
                for (int i = 1; i <= 3; i++)
                {
                    if (s.pieceBoard[(int)pos.x + i, (int)pos.z] == Piece.Ship)
                    {
                        hitPos = new Vector2((int)pos.x + i, (int)pos.z);
                        return true;
                    }
                }
            }
        }
        else if (shipTrans.eulerAngles.y == 270)
        {
            if (rightShot && !leftShot)
            {
                for (int i = 1; i <= 3; i++)
                {
                    if (s.pieceBoard[(int)pos.x, (int)pos.z + i] == Piece.Ship)
                    {
                        hitPos = new Vector2((int)pos.x, (int)pos.z + i);
                        return true;
                    }
                }
            }
            else if (leftShot && !rightShot)
            {
                for (int i = 1; i <= 3; i++)
                {
                    if (s.pieceBoard[(int)pos.x, (int)pos.z - i] == Piece.Ship)
                    {
                        hitPos = new Vector2((int)pos.x, (int)pos.z - i);
                        return true;
                    }
                }
            }
        }

        hitPos = new Vector2(int.MaxValue, int.MaxValue);
        return false;
    }
}
