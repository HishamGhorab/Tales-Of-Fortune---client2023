using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RiptideNetworking;
using UnityEngine;

public class TOFPlayer : MonoBehaviour
{
    public static Dictionary<ushort, TOFPlayerData> players = new Dictionary<ushort, TOFPlayerData>(); 
    public static Dictionary<ushort, GameObject> playerShipObjects = new Dictionary<ushort, GameObject>();
    
    public static List<Vector2Int> StartPositions = new List<Vector2Int>();
    public static List<int> StartRotations = new List<int>();

    //public static List<ushort> hitClientsArray = new List<ushort>();

    public TOFPlayerData playerData;
    
    [Serializable]
    public class TOFPlayerData
    {
        public TOFPlayerData(ushort id, string username, Vector2Int position, int rotation, int currentHealth, int maxHealth, bool isAlive)
        {
            this.id = id;
            this.username = username;
            this.position = position;
            this.rotation = rotation;
            this.currentHealth = currentHealth;
            this.maxHealth = maxHealth;
            this.isAlive = isAlive;
        }
        
        [SerializeField] private ushort id;
        [SerializeField] private string username;
        
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;
        [SerializeField] private bool isAlive;
        private bool sinking = false;
        
        public ushort Id {get => id; set { id = value; } }
        public string Username {get => username; set { username = value; } }
        public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public bool Sinking { get => sinking; set => sinking = value; }

        public Vector2Int position;
        public int rotation;
        
        public void SetPosition(Vector2Int pos)
        {
            position = pos;
        }
        
        public void SetRotation(int rot)
        {
            rotation = rot;
        }
    }
    

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public static void SpawnPlayers()
    {
        //int count = 0;
        foreach (TOFPlayerData playerData in players.Values)
        {
            GameObject playerGameObject = new GameObject();
            TOFPlayer player = playerGameObject.AddComponent<TOFPlayer>();
            
            player.playerData = playerData;
            
            playerGameObject.name = $"Player {playerData.Id} ({TOFPlayer.players[playerData.Id].Username})";

            //player.playerData.position = StartPositions[count];
            //player.playerData.rotation = StartRotations[count];

            //playerGameObject.transform.rotation = new Quaternion(0, player.playerData.rotation, 0, 0);
            
            playerShipObjects.Add(player.playerData.Id, playerGameObject);
            
            //count++;
        }
    }

    [MessageHandler((ushort) ServerToClientId.updateHealthValues)]
    static void UpdateHealthValue(Message message)
    {
        ushort client = message.GetUShort();
        int newHealth = message.GetInt();
        
        players[client].IsAlive = message.GetBool();
        players[client].Sinking = message.GetBool();
        
        players[client].CurrentHealth = newHealth;
    }

    [MessageHandler((ushort) ServerToClientId.AddPlayerToList)]
    static void AddPlayerData(Message message)
    {
        ushort id = message.GetUShort();
        string username = message.GetString();
        Vector2 startPosition = message.GetVector2();
        int startRotation = message.GetInt();
        int currentHealth = message.GetInt();
        int maxHealth = message.GetInt();
        bool isAlive = message.GetBool();
        
        TOFPlayerData playerData = new TOFPlayerData(id, username,Vector2Int.RoundToInt(startPosition) , startRotation, currentHealth, maxHealth, isAlive);
        
        Debug.Log(id);
        
        players.Add(id, playerData);
    }
        
}
