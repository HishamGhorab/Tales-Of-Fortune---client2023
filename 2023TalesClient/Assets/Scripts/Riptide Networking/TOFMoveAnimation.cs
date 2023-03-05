using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOFMoveAnimation : MonoBehaviour
{
    float accelerationRight = 50f;
    float maxSpeedRight = 70f;
    float stopSpeedRight = 60f;

    void Start()
    {
        //BoardView.Instance.onMoveForward += AnimateForward;
        //BoardView.Instance.onMoveRight += AnimateRight;
        //BoardView.Instance.onMoveLeft += AnimateLeft;
        
        TOFGameView.Singleton.onMoveForward += AnimateForward;
        TOFGameView.Singleton.onMoveRight += AnimateRight;
        TOFGameView.Singleton.onMoveLeft += AnimateLeft;
    }

    public void AnimateForward(Transform animTrans, Vector3 endPos) // remove endpos
    {
        StartCoroutine(ForwardMove(animTrans, endPos));
    }

    public void AnimateRight(Transform animTrans, Vector3 endPos)
    {
        StartCoroutine(RightRot(animTrans, endPos));
    }

    public void AnimateLeft(Transform animTrans, Vector3 endPos)
    {
        StartCoroutine(LeftRot(animTrans, endPos));
    }

    IEnumerator ForwardMove(Transform animTrans, Vector3 endPos) //move unchanged variables out
    {
        float accelerationForward = 10.0f;
        float stopSpeedForward = 10.0f;
        float curSpeedForward = 0.0f;
        float moveForwardTime = 0.0f;
        float maxSpeed = 10.0f;

        bool stopShipForward = false;
        bool accelerateShipForward = true;

        while (animTrans.position != endPos)
        {
            moveForwardTime += Time.deltaTime;

            if (accelerateShipForward)
            {
                curSpeedForward += accelerationForward * Time.deltaTime;
            }

            if (curSpeedForward >= maxSpeed)
            {
                curSpeedForward = maxSpeed;
                accelerateShipForward = false;
                stopShipForward = true;
            }

            if (stopShipForward)
            {
                curSpeedForward -= (stopSpeedForward * Time.deltaTime);

                if (curSpeedForward <= 0)
                {
                    curSpeedForward = 0;
                }
            }
            animTrans.Translate(Vector3.forward * curSpeedForward * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator RightRot(Transform animTrans, Vector3 endPos)
    {
        float curSpeedRight = 0.0f;
        float moveRightTime = 0.0f;

        bool stopShipRight = false;
        bool accelerateShipRight = true;

        bool playRightMove = true;

        while(playRightMove)
        {
            moveRightTime += Time.deltaTime;

            if (accelerateShipRight)
            {
                curSpeedRight += accelerationRight * Time.deltaTime;
            }

            animTrans.RotateAround(animTrans.gameObject.transform.GetChild(0).transform.Find("Right").transform.position, 
                new Vector3(0, 1, 0), curSpeedRight * Time.deltaTime);

            if (curSpeedRight >= maxSpeedRight)
            {
                curSpeedRight = maxSpeedRight;
                accelerateShipRight = false;
                stopShipRight = true;
            }

            if (stopShipRight)
            {
                curSpeedRight -= (stopSpeedRight * Time.deltaTime);

                if (curSpeedRight <= 0)
                {
                    curSpeedRight = 0;
                }
            }

            if (moveRightTime >= 2.5f)
            {
                playRightMove = false;
            }
            yield return null;
        }
    }

    IEnumerator LeftRot(Transform animTrans, Vector3 endPos)
    {
        float curSpeedLeft = 0.0f;
        float moveLeftTime = 0.0f;

        bool stopShipLeft = false;
        bool accelerateShipLeft = true;

        bool playLeftMove = true;

        while(playLeftMove)
        {
            moveLeftTime += Time.deltaTime;

            if (accelerateShipLeft)
            {
                curSpeedLeft += accelerationRight * Time.deltaTime;
            }

            animTrans.RotateAround(animTrans.gameObject.transform.GetChild(0).transform.Find("Left").transform.position, 
                new Vector3(0, -1, 0), curSpeedLeft * Time.deltaTime);

            if (curSpeedLeft >= maxSpeedRight)
            {
                curSpeedLeft = maxSpeedRight;
                accelerateShipLeft = false;
                stopShipLeft = true;
            }

            if (stopShipLeft)
            {
                curSpeedLeft -= (stopSpeedRight * Time.deltaTime);

                if (curSpeedLeft <= 0)
                {
                    curSpeedLeft = 0;
                }
            }

            if (moveLeftTime >= 2.5f)
            {
                playLeftMove = false;
            }
            yield return null;
        }
    }
}
