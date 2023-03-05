using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class TOFGameView : MonoBehaviour
{
    public GameObject shipPrefab;
    
    private static TOFGameView _singleton;

    //public List<MoveValues>[] moveValuesList;
    public MoveValues currentSegmentMoveData;
    
    public delegate void AnimateMove(Transform trans, Vector3 endPos);
    public event AnimateMove onMoveForward;
    public event AnimateMove onMoveRight;
    public event AnimateMove onMoveLeft;

    public Dictionary<ushort, MoveValues> previousMoves = new Dictionary<ushort, MoveValues>();

    public static TOFGameView Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(TOFGameView)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    
    private void Awake()
    {
        Singleton = this;
    }

    public void Start()
    {
        List<GameObject> ships = new List<GameObject>();
        foreach (GameObject go in TOFPlayer.playerShipObjects.Values)
        {
            ships.Add(go);
        }

        foreach (TOFPlayer.TOFPlayerData player in TOFPlayer.players.Values)
        {
            Vector2Int startPos = player.position;
            Vector3 v = PieceToWorldPos(new Vector3(startPos.x, 0, startPos.y));
            TOFPlayer.playerShipObjects[player.Id].transform.position = v;
            
            SpawnShip(TOFPlayer.playerShipObjects[player.Id].transform);
        }
    }

    public void SpawnShip(Transform spawnPosition)
    {
        Instantiate(shipPrefab, spawnPosition);
    }

    //move to view?
    [MessageHandler((ushort)ServerToClientId.onSendMovesBackToClient)]
    static void StoreMove(Message message)
    {
        //$"{client}_{allmoves[i][move]}{startPos.x}_{startPos.y}_{endPos.x}_{endPos.y}_{TOFPlayer.players[client].rotation}"
        //client0_move0_s1_s2_e1_e2_rotation, rightCannon, leftCannon
        
        string[] playerPositions = message.GetStrings();
        ushort[] hitClients = message.GetUShorts();
        
        //todo: refactor length? this playPositions is just the amount of players?
        for (int i = 0; i < TOFPlayer.players.Count; i++)
        {
            string[] enteries = playerPositions[i].Split('_');
            ushort client = ushort.Parse(enteries[0]);

            int move = int.Parse(enteries[1]);
            
            Vector2Int startPos = new Vector2Int(int.Parse(enteries[2]), int.Parse(enteries[3]));
            Vector2Int endPos = new Vector2Int(int.Parse(enteries[4]), int.Parse(enteries[5]));

            int rotation = int.Parse(enteries[6]);
            bool rightCannon = bool.Parse(enteries[7]);
            bool leftCannon = bool.Parse(enteries[8]);
            
            Singleton.currentSegmentMoveData = new MoveValues(client, move, startPos, endPos, rotation, rightCannon, leftCannon);
            
            AnimateSegment(i, hitClients[i]);
            Singleton.previousMoves[client] = Singleton.currentSegmentMoveData;
        }
    }
    
    static void AnimateSegment(int i, ushort clientHit)
    {
        MoveValues m = Singleton.currentSegmentMoveData;
            
        //Takes the previous move hard sets the end position and rotation in case its been offset by the editor
        if (TOFRoundHandler.Singleton.currentSegment != 1)
        {
            //check how currentSegment is getting updated
            
            ushort client = m.client;
            
            Singleton.SetShipPos(client, Singleton.previousMoves[client].endPos);
            Singleton.SetShipRot(client, Singleton.previousMoves[client].rotation);
        }
        
        ushort hitClient = clientHit;

        Singleton.StartCoroutine(AnimateInOrder());
        
        IEnumerator AnimateInOrder()
        {
            //Debug.Log($"Segment Time {TOFTimeHandler.Singleton.SegmentTime}, Move Time {TOFTimeHandler.Singleton.MoveTime}, CannonTime {TOFTimeHandler.Singleton.CannonTime}");
            
            Singleton.AnimateShip(m.client, m.move, m.endPos, m.rotation);
            yield return new WaitForSeconds(TOFTimeHandler.Singleton.MoveTime);
            
            bool didHitClientHit;
            didHitClientHit = hitClient != ushort.MaxValue ? true : false;
            if(didHitClientHit)
            {
                Singleton.AnimateDecreaseHealth(hitClient);
            }
            
            Singleton.AnimateCannons(m.client, m.rightCannon, m.leftCannon);
            yield return new WaitForSeconds(TOFTimeHandler.Singleton.CannonTime);

            if (TOFPlayer.players[m.client].Sinking)
            {
                Debug.Log($"Client: {m.client} : Sinking = {TOFPlayer.players[m.client].Sinking}");
                
                TOFShip ship = TOFPlayer.playerShipObjects[m.client].GetComponentInChildren<TOFShip>();

                ship.AnimateShipSink(m.client);
                yield return new WaitForSeconds(TOFTimeHandler.Singleton.SinkTime);
            }
        }
    }

    public void AnimateShip(ushort client, int move, Vector2Int endPos, int rotation)
    {
        Transform shipTrans = TOFPlayer.playerShipObjects[client].transform;
        Vector3 shipEndPos = new Vector3(endPos.x, shipTrans.position.y, endPos.y);
        //shipTrans.eulerAngles = new Vector3(0,rotation,0);
        
        MoveAnimation(move, shipTrans, shipEndPos);
        //todo animate
    }

    public void AnimateCannons(ushort client, bool rightCannon, bool leftCannon)
    {        
        TOFShip ship = TOFPlayer.playerShipObjects[client].GetComponentInChildren<TOFShip>();
        StartCoroutine(ship.PlayCannonVFX(rightCannon, leftCannon));
    }

    public void AnimateDecreaseHealth(ushort client)
    {
        //Debug.Log($"C: {client} : HP: {TOFPlayer.players[client].CurrentHealth}");
        TOFUiProfile.UiProfiles[client].SetProfileHealth();
    }

    public void SetShipPos(ushort client, Vector2Int pos)
    {
        GameObject p = TOFPlayer.playerShipObjects[client];
        Vector3 v = PieceToWorldPos(new Vector3(pos.x, p.transform.position.y, pos.y));
        TOFPlayer.playerShipObjects[client].transform.position = v;
    }

    public void SetShipRot(ushort client, int rot)
    {
        TOFPlayer.playerShipObjects[client].transform.eulerAngles = new Vector3(0, rot, 0);
    }
    
    void MoveAnimation(int move, Transform shipTrans, Vector3 endPos)
    {
        switch(move)
        {
            case 0:
                //still move
                break;
            case 1:
                onMoveForward.Invoke(shipTrans, endPos);
                break;
            case 2:
                onMoveRight.Invoke(shipTrans, endPos);
                break;
            case 3:
                onMoveLeft.Invoke(shipTrans, endPos);
                break;
            //case "CFForward": break;
            //case "CFRight": break;
            //case "CFLeft": break;
            //case "CDRight": break;
            //case "CDLeft": break;
            default: break;
        }
    }

    public static Vector3 PieceToWorldPos(Vector3 position)
    {
        return position * 10;
    }

    public class MoveValues
    {
        public MoveValues(ushort client, int move, Vector2Int startPos, Vector2Int endPos, int rotation, bool rightCannon, bool leftCannon)
        {
            this.client = client;
            this.move = move;
            this.startPos = startPos;
            this.endPos = endPos;
            this.rotation = rotation;
            this.rightCannon = rightCannon;
            this.leftCannon = leftCannon;
        }

        public ushort client;
        public int move;
        public Vector2Int startPos;
        public Vector2Int endPos;
        public int rotation;
        public bool rightCannon;
        public bool leftCannon;
    }
}
