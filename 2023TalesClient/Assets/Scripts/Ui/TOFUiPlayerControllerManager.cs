using System;
using System.Collections;
using System.Collections.Generic;
using RiptideNetworking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TOFUiPlayerControllerManager : MonoBehaviour
{
    public Button[] moveButtons;
    public Toggle[] rightCannons;
    public Toggle[] leftCannons;
    
    private static TOFUiPlayerControllerManager _singleton;
    public static TOFUiPlayerControllerManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(TOFUiPlayerControllerManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    private void OnEnable()
    {
        TOFRoundHandler.onPlanningPhase += EnableController;
        TOFRoundHandler.onPlayingPhase += DisableController;
    }
    
    private void OnDisable()
    {
        TOFRoundHandler.onPlanningPhase -= EnableController;
        TOFRoundHandler.onPlayingPhase -= DisableController;
    }

    [MessageHandler((ushort)ServerToClientId.onFetchMoves)]
    static void SendMovesToServer(Message message)
    {
        int[] moves = new int[4];
        
        for (int i = 0; i < 4; i++)
        {
            string text = Singleton.moveButtons[i].GetComponentInChildren<TextMeshProUGUI>().text;
            
            switch (text)
            {
                case "S": moves[i] = 0;
                    break;
                case "F": moves[i] = 1;
                    break;
                case "R": moves[i] = 2;
                    break;
                case "L": moves[i] = 3;
                    break;
            }
        }

        //todo: send these moves back to server
        Message _message = Message.Create(MessageSendMode.reliable, ClientToServerId.sendPlayerMoves);

        bool[] _rightCannons = new bool[4];
        bool[] _leftCannons = new bool[4]; 
 
        for(int i = 0; i < 4; i++)
        {
            _rightCannons[i] = Singleton.rightCannons[i].isOn;
            _leftCannons[i] = Singleton.leftCannons[i].isOn;
        }

        _message.AddInts(moves);
        _message.AddBools(_rightCannons);
        _message.AddBools(_leftCannons);

        TOFNetworkManager.Singleton.Client.Send(_message);
    }

    //TEMPORARY LOCATION
    public void DisableController()
    {
        foreach (Button button in moveButtons)
        {
            button.interactable = false;
        }

        for (int i = 0; i < rightCannons.Length; i++)
        {
            rightCannons[i].interactable = false;
            leftCannons[i].interactable = false;
        }
    }

    public void EnableController()
    {
        foreach (Button button in moveButtons)
        {
            button.interactable = true;

            foreach (Button b in moveButtons)
            {
                b.GetComponentInParent<UIRemote>().SetToDefaultKey();
            }
        }

        for (int i = 0; i < rightCannons.Length; i++)
        {
            rightCannons[i].interactable = true;
            leftCannons[i].interactable = true;

            rightCannons[i].isOn = false;
            leftCannons[i].isOn = false;
        }
    }
}
