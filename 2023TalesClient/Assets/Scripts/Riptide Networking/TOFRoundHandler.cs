using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using TMPro;

public class TOFRoundHandler : MonoBehaviour
{
    public static int currentRound = 1;
    
    public int currentSegment = 1;
    private int segmentSwapTime = 0;

    public enum RoundState {PlanningPhase, PlayingPhase}
    
    public delegate void OnPlanningPhase();
    public delegate void OnPlayingPhase();
    
    public static OnPlanningPhase onPlanningPhase;
    public static OnPlayingPhase onPlayingPhase;

    public static RoundState currentState;

    private static bool changedRound;
    
    //temporary ui
    public TextMeshProUGUI segmentText;
    
    private static TOFRoundHandler _singleton;
    public static TOFRoundHandler Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(TOFRoundHandler)} instance already exists, destroying duplicate!");
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
        segmentText.enabled = false;
    }

    private void Update()
    {
        if (currentState == RoundState.PlanningPhase && changedRound)
        {
            onPlanningPhase.Invoke(); //no we have segment no planning 2 different delegates? yes
            changedRound = false;
            segmentText.enabled = false;
            Debug.Log("Current round state: " + currentState);
        }
        else if (currentState == RoundState.PlayingPhase && changedRound)
        {
            onPlayingPhase.Invoke();
            changedRound = false;
            segmentText.enabled = true;
            Debug.Log("Current round state: " + currentState);
        }
    }

    [MessageHandler((ushort)ServerToClientId.sendRoundState)]
    static void SetRoundState(Message message)
    {
        string roundState = message.GetString();
        switch (roundState)
        {
            case "PlanningPhase":
                currentState = RoundState.PlanningPhase;
                break;
            case "PlayingPhase":
                currentState = RoundState.PlayingPhase;
                break;
        }

        changedRound = true;
    }

    [MessageHandler((ushort) ServerToClientId.sendSegmentToClient)]
    static void UpdateSegment(Message message)
    {
        int segment = message.GetInt();
        Singleton.currentSegment = segment;
    }
}
