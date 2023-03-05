using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour, IBoardListener
{
    #region Singleton
    private static BoardView instance;
    public static BoardView Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = new GameObject("GameHandler");
                go.AddComponent<BoardView>();
            }

            return instance;
        }
    }
    #endregion

    private BoardModel boardModel = BoardModel.Instance;

    public delegate void boardViewDelegate(Transform trans, Vector3 endPos);
    public event boardViewDelegate onMoveForward;
    public event boardViewDelegate onMoveRight;
    public event boardViewDelegate onMoveLeft;

    public GameObject[] shipModel;
    
    bool playAnim = false;
    bool playCannon = false;

    [Header("Projectile Prefabs")]
    [SerializeField] GameObject cannonPrefab;
    [SerializeField] float cannonSpeed = 200;

    TimeHandler timeHandler;
    //GameObject rightIndicator;
    //GameObject leftIndicator;

    [Header("Game State")]
    [SerializeField] BoardController boardController;
    [SerializeField] int playerNumber;
    public Vector2 boardSize = new Vector2(16, 16);
    
    void Awake()
    {
        instance = this;
        //GameData.Load(); //placeholder position shuold be changed to a more global script
        
        shipModel = new GameObject[playerNumber];
        StartGame(playerNumber, boardSize);
    }
    
    void Start()
    {
        boardModel.AddListener(this);
        timeHandler = TimeHandler.Instance;
    }

    void StartGame(int numPlayers, Vector2 _boardSize)
    {
        GameObject[] players = new GameObject[numPlayers];
        
        //Set players to positions and instantiate
        boardModel.StartGame(numPlayers, (int)_boardSize.x, (int)_boardSize.y);

        for (int i = 0; i < numPlayers; i++)
        {
            //players[i] = InstantiatePlayer(PieceInfo.startingPositions[numPlayers][i].x, PieceInfo.startingPositions[numPlayers][i].y, 
                //GameData.Ships["001"].shipPrefab);

            players[i].GetComponent<ShipHandler>().currentPos.x = PieceInfo.startingPositions[numPlayers][i].x;
            players[i].GetComponent<ShipHandler>().currentPos.y = PieceInfo.startingPositions[numPlayers][i].y;
            //players[i].name += $"_{PieceInfo.startingPositions[numPlayers][i].x},{PieceInfo.startingPositions[numPlayers][i].y}";
            shipModel[i] = players[i];
            boardModel.AddShipToStateList(players[i]);
        }
    }

    #region Delegates
    public void OnForward(Transform trans, Vector3 endPos)
    {
        if(onMoveForward != null)
        {
            onMoveForward(trans, endPos);
        }
    }

    public void OnRight(Transform trans, Vector3 endPos)
    {
        if(onMoveRight != null)
        {
            onMoveRight(trans, endPos);
        }
    }

    public void OnLeft(Transform trans, Vector3 endPos)
    {
        if(onMoveLeft != null)
        {
            onMoveLeft(trans, endPos);
        }
    }
    #endregion

    GameObject InstantiatePlayer(float x, float z, GameObject shipPrefab)
    {
        return Instantiate(shipPrefab, new Vector3(x, 0.06f, z), Quaternion.identity);
    }

    public void MovePiece(GameObject shipObject, Vector3 endPos, int endRotation, string turn, int shipIndex, bool rightCannon, bool leftCannon)
    {
        StartCoroutine(PlayAnimation(endPos, endRotation, turn, shipIndex, rightCannon, leftCannon));
    }

    #region Animations
    IEnumerator PlayAnimation(Vector3 endPos, int endRotation, string turn, int shipIndex, bool rightCannon, bool leftCannon)
    {
        MoveAnimation(endPos, endRotation, turn, shipIndex);
        yield return new WaitForSeconds(3);

        switch(turn)
        {
            case "still": case "forward": case "CFForward":
                shipModel[shipIndex].transform.position = endPos;
                break;
            case "right": case "left": case "CFRight": case "CFLeft": case "CDRight": case "CDLeft":
                shipModel[shipIndex].transform.position = endPos;
                shipModel[shipIndex].transform.rotation = Quaternion.Euler(0,endRotation,0);
                break;
            default: break;
        }

        yield return new WaitForSeconds(CannonAnimation(shipModel[shipIndex].transform, rightCannon, leftCannon, cannonSpeed));

        shipModel[shipIndex].GetComponent<ShipHandler>().FinishedSegment = true;
        if(UpdateSegment(shipModel))
        {
            foreach (GameObject ship in shipModel)
            {
                ship.GetComponent<ShipHandler>().FinishedSegment = false;
            }
            
            for (int i = 0; i < boardModel.PlayersEndPositionsSegment.Length; i++)
            {                
                boardModel.PlayersEndPositionsSegment[i].Clear();
            }

            timeHandler.Segment++;
            boardController.ExecuteMove = true;
        }
    }

    void MoveAnimation(Vector3 endPos, int endRotation, string turn, int shipIndex)
    {
        switch(turn)
        {
            case "still":
                //still move
                break;
            case "forward":
                OnForward(shipModel[shipIndex].transform, endPos);
                break;
            case "right":
                OnRight(shipModel[shipIndex].transform, endPos);
                break;
            case "left":
                OnLeft(shipModel[shipIndex].transform, endPos);
                break;
            case "CFForward": break;
            case "CFRight": break;
            case "CFLeft": break;
            case "CDRight": break;
            case "CDLeft": break;
            default: break;
        }
    }

    int CannonAnimation(Transform trans, bool rightCannon, bool leftCannon, float speed)
    {
        if (rightCannon)
        {
            ShootCannon(trans, rightCannon, leftCannon, speed, 1);
            return 2;
        }
        else if (leftCannon)
        {
            ShootCannon(trans, rightCannon, leftCannon, speed, -1);
            return 2;
        }
        return 0;
    }

    void ShootCannon(Transform trans, bool rightCannon, bool leftCannon, float speed, float direction)
    {
        string shootAnimDir = "ShootCannonLeft";
        if(direction == 1) shootAnimDir = "ShootCannonRight";
        
        Vector2 hitPos;
        
        trans.transform.Find("Ship").GetComponent<ResetTransform>().ResetTrans();
        trans.transform.Find("Ship").GetComponent<Animator>().SetTrigger(shootAnimDir);
        trans.transform.Find("Sound").transform.GetChild(0).GetComponent<AudioSource>().Play();

        GameObject thisProjectile = Instantiate(cannonPrefab, trans.position, Quaternion.identity);
        Physics.IgnoreCollision(thisProjectile.GetComponent<Collider>(), trans.gameObject.GetComponent<Collider>());
        thisProjectile.GetComponent<Rigidbody>().AddForce(direction * trans.transform.right * speed);
        Destroy(thisProjectile, 1);

        StartCoroutine(trans.GetComponent<ShipHandler>().PlayShootSmokeVFX(direction));

        if (boardModel.ProjectileHitCheck(out hitPos, trans, rightCannon, leftCannon) == true)
        {
            boardModel.RemoveHealthFromShip(hitPos, 1);
        }
    }
    #endregion

    bool UpdateSegment(GameObject[] allShips)
    {
        foreach (GameObject thisShip in allShips)
        {
            if(!thisShip.GetComponent<ShipHandler>().FinishedSegment)
            {
                return false;
            }
        }
        return true;
    }

    public void PlacePiece(Vector3 pos, Piece piece){}
}