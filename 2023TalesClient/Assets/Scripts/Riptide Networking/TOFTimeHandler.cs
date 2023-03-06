using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using TMPro;

public class TOFTimeHandler : MonoBehaviour
{
    public static int GameTime;
    public TextMeshProUGUI gametimetext;
    public TextMeshProUGUI roundtimetext;

    [NonSerialized] public int SegmentTime;
    [NonSerialized] public int MoveTime;
    [NonSerialized] public int CannonTime;
    [NonSerialized] public int SinkTime;
    
    private static TOFTimeHandler _singleton;
    public static TOFTimeHandler Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(TOFTimeHandler)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    public void Update()
    {
        //Temporary ui
        gametimetext.text = GameTime.ToString();
        roundtimetext.text = TOFRoundHandler.currentRound.ToString();
        
        string minutes = Mathf.Floor(GameTime / 60).ToString("00");
        string seconds = (GameTime % 60).ToString("00");
     
        gametimetext.text = string.Format("{0}:{1}", minutes, seconds);
    }
    
    [MessageHandler((ushort)ServerToClientId.updateSegmentTimes)]
    static void GetSegmentAndCannonTime(Message message)
    {
        Singleton.SegmentTime = message.GetInt();

        Singleton.MoveTime = message.GetInt();
        Singleton.CannonTime = message.GetInt();
        Singleton.SinkTime = message.GetInt();
    }
}
