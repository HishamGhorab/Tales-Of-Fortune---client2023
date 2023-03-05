using Minimax;
using Position = UnityEngine.Vector2Int;

public enum Piece {Ship, EnemyShip, SeaMonster, Stone, Empty, Invalid};

public struct Player : IPlayer
{
    private readonly Piece piece;
    private readonly bool human;
    private readonly int pieceIndex;

    public Player(Piece piece, int pieceIndex, bool human)
    {
        this.piece = piece;
        this.human = human;
        this.pieceIndex = pieceIndex;
    }

    public Piece Value()
    {
        return (piece);
    }

    public bool Human()
    {
        return (human);
    }
}

static class PieceInfo
{
    public static readonly Position[][] startingPositions = new Position[5][]
    {
            null,null,
            new Position[2]
            {
                new Position(4,3), new Position(5,3)
            },
            null,
            new Position[4]
            {
                new Position(0,0), new Position(5,3), new Position(1,0), new Position(6,3)
            }
    };
}

