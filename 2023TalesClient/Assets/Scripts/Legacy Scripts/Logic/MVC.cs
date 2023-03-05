using System.Collections.Generic;
using UnityEngine;

//using Position = UnityEngine.Vector2Int;

/* Interfaces that describe the communication between the Model and the Views/Controllers */
// Certain enhancements may require extending the interfaces, do so carefully

public interface IBoardModel
{
  void AddListener(IBoardListener listener);  // Add the listener to the list
  void StartGame(int numPlayers, int boardSizeX, int boardSizeY);       // A new game with numPlayers players will begin
  Piece GetPiece(Vector3 pos);         // Get the piece at pos
  bool MovePiece(GameObject shipObject, Vector3 startPos, Vector3 endPos, int rotation, string turn, int shipIndex, bool rightCannon, bool leftCannon);      // Ask to move the piece at position (startX, startY) to (endX, endY).  Returns true if successful
  string PieceCrashCheck(Vector3 pieceCurrentPos, Vector3 pieceEndPos, int rotation, List<Vector3>[] listOfPositions, int index);
  List<Vector3>[] PlayersEndPositionsSegment {get; set;}
  //void MakeMove(Player player, int index);
  //bool SetPiece(Position pos, Piece piece);  // Place piece at pos
  //void SaveGame();  // Save the current game
  //void LoadGame();  // Load a previous game
}

public interface IBoardListener
{
  //void NewGame(List<Player> players);  // Restart the game
  //void SetNewWinner(Player player);     // player has finished the game
  void PlacePiece(Vector3 pos, Piece piece);  // Put piece at position (pos.x, pos.y) 
  void MovePiece(GameObject shipObject, Vector3 endPos, int rotation, string turn, int shipIndex, bool rightCannon, bool leftCannon);     // Move the piece at position (startPos.x, startPos.y) to (endPos.x, endPos.y)
}

