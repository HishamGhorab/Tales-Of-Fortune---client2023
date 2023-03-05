using System;
using System.Collections;
using System.Collections.Generic;
using RiptideNetworking;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TOFChatManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField chatBox;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private GameObject chatPanel;

    [SerializeField] private GameObject SlidingArea;
    [SerializeField] private Image ScrollWheelImage;
    [SerializeField] private GameObject ChatView;

    private bool isChatInputVisible = false;

    private float timeToHide;
    private bool shouldHide;
    
    private static TOFChatManager _singleton;
    public static TOFChatManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(TOFChatManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        chatBox.characterLimit = 100;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (isChatInputVisible)
            {
                CloseInputField();
            }
            else
            {
                EnableInputField();
            }
        }
        
        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendChatTextToServer(chatBox.text);
                chatBox.text = "";
            }
        }

        if (shouldHide && !isChatInputVisible && Time.time >= timeToHide)
        {
            ChatView.SetActive(false);
        }
    }

    public void SendChatTextToServer(string text)
    {
        Message _message = Message.Create(MessageSendMode.reliable, ClientToServerId.sendChatText);
        _message.AddString(text);
        TOFNetworkManager.Singleton.Client.Send(_message);
    }

    public void CloseInputField()
    {
        SlidingArea.SetActive(false);
        ScrollWheelImage.enabled = false;
        chatBox.enabled = false;
        chatBox.gameObject.SetActive(false);
        isChatInputVisible = false;

        shouldHide = true;
        timeToHide = Time.time + 5;

        //CameraMovement.Singleton.SetCameraKeyMovement(true);
    }

    public void EnableInputField()
    {
        //view
        SlidingArea.SetActive(true);
        ScrollWheelImage.enabled = true;
        chatBox.enabled = true;
            
        ChatView.SetActive(true);
        chatBox.gameObject.SetActive(true);
            
        chatBox.Select();
        chatBox.ActivateInputField();

        isChatInputVisible = true;
    
        shouldHide = false;

        //CameraMovement.Singleton.SetCameraKeyMovement(false);
    }

    public void ShowInputField()
    {
        ChatView.SetActive(true);
        shouldHide = true;
        timeToHide = Time.time + 5;
    }

    [MessageHandler((ushort)ServerToClientId.sendChatText)]
    private static void PrintTextInChat(Message message)
    {
        ushort client = message.GetUShort();
        string time = message.GetString();
        string username = message.GetString();
        string chatText = message.GetString();
     
        Singleton.ShowInputField();
        Singleton.CreateChatMessage(time, chatText, username, false, client);
    }
    
    [MessageHandler((ushort)ServerToClientId.sendServerChatText)]
    private static void PrintServerTextInChat(Message message)
    {
        string time = message.GetString();
        string chatText = message.GetString();
        
        Singleton.ShowInputField();
        Singleton.CreateChatMessage(time, chatText);
    }

    private void CreateChatMessage(string time, string chatText, string username = "Server", bool fromServer = true, ushort client = UInt16.MaxValue)
    {
        if (client == UInt16.MaxValue && !fromServer)
        {
            Debug.LogError("You must specify a client, you likely forgot to add the last defaulted parameter.");
            return;
        }
        
        GameObject textObj = Instantiate(Singleton.textPrefab, Singleton.chatPanel.transform);
        
        //We are <color=green>green</color> with envy
        
        string newText = "";

        if (!fromServer)
        {
            if (TOFClient.clients[client].islocal)
            {
                newText = time + " " + "<color=green>" +username+ "</color>" + ": " + chatText;
            }
            else
            {
                newText = time + " " + "<color=red>" +username+ "</color>" + ": " + chatText;
            }
        }
        else
        {
            newText = time + " " + "<color=yellow>" + username + ": " + chatText + "</color>";
        }

        textObj.GetComponent<TextMeshProUGUI>().text = newText;
    }
}
