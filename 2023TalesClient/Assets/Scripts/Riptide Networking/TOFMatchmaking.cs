using System.Collections.Generic;
using UnityEngine.SceneManagement;
using RiptideNetworking;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TOFMatchmaking : MonoBehaviour //TOFMatchmakingUI
{
    private static TOFMatchmaking _singleton;
    public static TOFMatchmaking Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(TOFMatchmaking)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("Connect")] 
    [SerializeField] private GameObject connectUI;
    [SerializeField] private TMP_InputField  usernameField;
    [SerializeField] private static GameObject matchmakingUi;
    [SerializeField] private GameObject startButton;

    [SerializeField] private Color lobbyFullColor;
    [SerializeField] private Color lobbyNotFullColor;

    private static Color lobbyFullColorStatic;
    private static Color lobbyNotFullColorStatic;
    private static GameObject startButtonStatic;

    private void Awake()
    {
        Singleton = this;
        matchmakingUi = GameObject.Find("matchmaking");

    }

    private void Start()
    {
        //TOFNetworkManager.Singleton.Client.Connected += ShowMatchmakingUi;

        //static set
        
        matchmakingUi.SetActive(false);
        startButton.SetActive(false);
        
        lobbyFullColorStatic = lobbyFullColor;
        lobbyNotFullColorStatic = lobbyNotFullColor;
        startButtonStatic = startButton;
    }

    public void ConnectClicked()
    {
        usernameField.interactable = false;
        connectUI.SetActive(false);
        matchmakingUi.SetActive(true);

        TOFNetworkManager.Singleton.Connect();
    }

    public void DisconnectClicked()
    {
        //matchmakingUi.SetActive(false);
        SendOnDisconnectedClientData();
        BackToMenu();

        //TOFNetworkManager.Singleton.Disconnect();
    }
    
    public void StartSession()
    {
        //när man trycker start så skickar man helt enkelt ett meddelandet till servern och ber den att byta scen till alla clienter!
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.onSessionStart);
        TOFNetworkManager.Singleton.Client.Send(message);
    }

    public void BackToMenu()
    {
        usernameField.interactable = true;
        matchmakingUi.SetActive(false);
        connectUI.SetActive(true);
    }

    public void SendOnConnectedClientData()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.onConnectedClientData);
        message.AddString(usernameField.text);
        TOFNetworkManager.Singleton.Client.Send(message);
    }

    public void SendOnDisconnectedClientData()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.onDisconnectedClientData);
        message.AddString(usernameField.text);
        TOFNetworkManager.Singleton.Client.Send(message);
    }

    public static void UpdateMatchMakingText(int currentConnectedClients, ushort maxClients)
    {
        matchmakingUi.GetComponentInChildren<TMP_Text>().text = $"Matchmaking ({currentConnectedClients}/{maxClients})";

        if (currentConnectedClients < maxClients)
        {
            matchmakingUi.GetComponent<Image>().color = lobbyNotFullColorStatic;
        }
        else
        {
            matchmakingUi.GetComponent<Image>().color = lobbyFullColorStatic;
        }
    }

    [MessageHandler((ushort) ServerToClientId.matchmakingClientsChanged)]
    private static void UpdateMatchmaking(Message message)
    {
        int currentClients = message.GetInt();
        ushort maxClientsCount = message.GetUShort();

        UpdateMatchMakingText(currentClients, maxClientsCount);
    }
    
    [MessageHandler((ushort) ServerToClientId.isLobbyLeader)]
    private static void UpdateLobbyLeader(Message message)
    {
        bool canStart = message.GetBool();
        startButtonStatic.SetActive(true);
        
        if (canStart)
        {
            startButtonStatic.GetComponent<Button>().interactable = true;
        }
        else
        {
            startButtonStatic.GetComponent<Button>().interactable = false;
        }
    }
    
    [MessageHandler((ushort)ServerToClientId.onSessionStarted)]
    private static void StartToSessionScene(Message message)
    {
        
        //todo, wtf are these variables
        int segmentsInRound = message.GetInt();
        int cannonShotLength = message.GetInt();
        
        int clientCount = message.GetInt();
        
        //Load session scene
        //SceneManager.LoadScene("TOF1");

        TOFLoadManager.Singleton.LoadSession();
        //SceneManager.LoadSceneAsync("TOF1", LoadSceneMode.Additive);
    
        //todo : connect this to the server to make sure all clients are in game before moving on
        SceneManager.sceneLoaded += SceneSetup;

        void SceneSetup(UnityEngine.SceneManagement.Scene arg0, LoadSceneMode arg1)
        {
            //Set current sessions game mode values
            TOFPlayer.SpawnPlayers();

            Message _message = Message.Create(MessageSendMode.reliable, ClientToServerId.gameSceneLoaded);
            TOFNetworkManager.Singleton.Client.Send(_message);
        }
    }
}
