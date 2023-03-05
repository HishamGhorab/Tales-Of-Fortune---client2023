using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class InGameHud : MonoBehaviour
{
    private int roundLength = 10;
    private float timeLeftForRound = 0;
    private bool planningPhase = false;
    private bool playingPhase = false;
    
    private UIDocument doc;
    private VisualElement root;
    private VisualElement roundProgressBar;
    private Label roundLengthText;
    private Label roundSegmentText;


    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        root = doc.rootVisualElement;
        roundLengthText = root.Q<Label>("RoundLengthText");
        roundSegmentText = root.Q<Label>("RoundSegmentText");
        roundProgressBar = root.Q<VisualElement>("RoundProgressBar");

        roundProgressBar.style.width = Length.Percent(100);
        
        roundLengthText.text = roundLength.ToString();
        roundSegmentText.text = "1:0";
    }

    private void OnEnable()
    {
        TOFRoundHandler.onPlanningPhase += PlanningPhaseStarted;
        TOFRoundHandler.onPlayingPhase += PlayingPhaseStarted;
    }
    
    private void OnDisable()
    {
        TOFRoundHandler.onPlanningPhase -= PlanningPhaseStarted;
        TOFRoundHandler.onPlayingPhase -= PlayingPhaseStarted;
    }
    
    private void Update()
    {
        //needs fix
        if (playingPhase)
        {
            Debug.Log("w");
            roundSegmentText.text = string.Format("{0}:{1}", TOFRoundHandler.currentRound, TOFRoundHandler.Singleton.currentSegment);
        }
        
        if (planningPhase)
        {
            timeLeftForRound -= Time.deltaTime;

            timeLeftForRound = Mathf.Clamp(timeLeftForRound, 0, roundLength);
            
            roundLengthText.text = (math.round(timeLeftForRound)).ToString();
            roundProgressBar.style.width = Length.Percent((timeLeftForRound / roundLength) * 100);
        }
    }

    private void PlanningPhaseStarted()
    {
        playingPhase = false;
        
        timeLeftForRound = roundLength;
        planningPhase = true;
    }
    
    private void PlayingPhaseStarted()
    {
        planningPhase = false;
        playingPhase = true;
    }
}
