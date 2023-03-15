using System;
using System.Collections;
using RiptideNetworking;
using RiptideNetworking.Utils;
using TMPro;
using UnityEngine;

public enum ServerToClientId : ushort
{
    clientJoinedLobby = 1, clientLeftLobby, matchmakingClientsChanged, isLobbyLeader, onSessionStarted, updateSegmentTimes, 
    updateHealthValues, AddPlayerToList,

    //Session flow
    sendSegmentToClient, onFetchMoves, sendRoundState, onSendMovesBackToClient, onHardSetShipTrans,
    
    //chat
    sendChatText, sendServerChatText, 
    
    //shop
    spawnShop, sendShopState, closeShop,
    
    //inventory
    sendInventoryState, sendMoneyState,
}

public enum ClientToServerId : ushort
{
    onConnectedClientData = 1, onDisconnectedClientData, onSessionStart,

    //Session flow
    sendPlayerMoves, gameSceneLoaded,
    
    //chat
    sendChatText,
    
    //shop
    onShopInteract, onShopBuy,
}


public class TOFNetworkManager : MonoBehaviour
{
    private static TOFNetworkManager _singleton;
    public static TOFNetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(TOFNetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Client Client { get; private set; }
    [SerializeField] private string ip;
    [SerializeField] private ushort port;

    [SerializeField] private GameObject ipTextGO;
    
    private void Awake()
    {
        Singleton = this;
        
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        Client = new Client();

        ipTextGO.GetComponent<TMP_InputField>().text = ip;
        
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailToConnect;
        Client.ClientDisconnected += ClientLeft;
        Client.Disconnected += DidDisconnect;
    }

    private void FixedUpdate()
    {
        Client.Tick();    
    }

    private void OnApplicationQuit()
    {
        //todo: some exception here
        
        Client.Disconnect();
    }

    private void DidConnect(object sender, EventArgs e)
    {
        TOFMatchmaking.Singleton.SendOnConnectedClientData();
    }
    
    private void FailToConnect(object sender, EventArgs e)
    {
        TOFMatchmaking.Singleton.BackToMenu();
    }

    private void ClientLeft(object sender, ClientDisconnectedEventArgs e)
    {
        //TOFMatchmaking.Singleton.SendOnDisconnectedClientData();
        //Destroy(Player.list[e.Id].gameObject);
    }
    
    private void DidDisconnect(object sender, EventArgs e)
    {
        TOFMatchmaking.Singleton.BackToMenu();
    }

    public void Connect()
    {
        ip = ipTextGO.GetComponent<TMP_InputField>().text;

        Client.Connect($"{ip}:{port}");
    }

    public void Disconnect()
    {
        Client.Disconnect();
    }
}
