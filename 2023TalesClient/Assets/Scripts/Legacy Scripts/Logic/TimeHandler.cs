using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler
{
    private static TimeHandler instance;

    public static TimeHandler Instance
    {
        get
        {
            if (instance == null)
                instance = new TimeHandler();
            return instance;
        }
    }

    public enum RoundState
    {
        PreRound, WaitingForRound, PlayingSegment 
    }

    public RoundState roundState; // this was static

    public int round;
    int segment = 1;
    public int Segment { get {return segment; } set { segment = value; } }
    
    public bool playSegment;

    public float timeBetweenRounds = 10;
    public float timeBetweenSegments;

    [Header("Components")]
    public Text roundText;
    public Text roundTimeText;
    public Text roundStatText;

    public  IEnumerator IncreaseRound(float time)
    {
        yield return new WaitForSeconds(time);
        round++;
        roundState = RoundState.PlayingSegment;
        playSegment = true;
    }

    public void CalculateSegmentLength(string move, bool rightCannon, bool leftCannon)
    {
        int moveTime = 0;
        
        switch(move)
        {
            case "S":
                moveTime = TOFTime.noMoveTime;
                break;
            case "F":
                moveTime = TOFTime.forwardMoveTime;
                break;
            case "R": case "L":
                moveTime = TOFTime.diagonalMoveTime;
                break;
        }

        if (rightCannon || leftCannon)
        {
            moveTime += TOFTime.shootTime;
        }

        if(timeBetweenSegments == 0)
        {
            timeBetweenSegments = moveTime;
        }
        
        if(moveTime > timeBetweenSegments)
        {
            timeBetweenSegments = moveTime;
        }
    }









































    /*public enum RoundState {PreRound, CalculatingSegment, PlayingSegment}
    [SerializeField] public static RoundState roundState;

    [Header("Debug Variables")]
    public int round = 1;
    public float timer = 0;
    public int segment = 0;
    public int Segment { get {return segment; } set { segment = value; } }


    [Header("Variables")]
    public int lengthOfRound = 0;
    public int segmentLength = 0;

    [Header("Components")]
    public Text roundText;
    public Text roundTimeText;
    public Text roundStatText;

    //int storedTime = 0;

    //public int SegmentTime {get { return segmentTime; } set { segmentTime = value; } }

    //int index = 0;

    void Update()
    {
        Debug.Log(roundState);
        roundStatText.text = roundState.ToString();
        if (roundState == RoundState.PreRound)
        {
            PlayTimer(lengthOfRound);
        }
    }

    void PlayTimer(int givenLengthOfRound)
    {
        if(timer <= givenLengthOfRound)
        {
            timer += Time.deltaTime;
        }
        else{
            //segment = 0;
            roundState = RoundState.CalculatingSegment;
        }
    }

    public void CalculateSegmentLength(string move, bool rightCannon, bool leftCannon)
    {
        int moveTime = 0;
        
        if (move == "S"){
            moveTime = TOFTime.noMoveTime;
        }
        else if (move == "F"){
            moveTime = TOFTime.forwardMoveTime;
        }
        else if (move == "R" || move == "L"){
            moveTime = TOFTime.diagonalMoveTime;
        }

        if (rightCannon || leftCannon)
        {
            moveTime += TOFTime.shootTime;
        }

        if(segmentLength == 0)
        {
            segmentLength = moveTime;
        }
        
        if(moveTime > segmentLength)
        {
            segmentLength = moveTime;
            //Debug.Log("Segment set to " + segmentLength);
        }
    }*/
}
