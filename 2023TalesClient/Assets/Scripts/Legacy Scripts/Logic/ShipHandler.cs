using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShipHandler : MonoBehaviour
{
    public string id;
    //public Ship.ShipType shipType;

    [Header("Ship Components")]
    public int shipHealth;
    public int baseDamage;
    public float smokeVFXDelay = 1;

    [Header("State")]
    public Vector2 currentPos;
    //public Vector2 CurrentPos {get {return currentPos;} set {currentPos = value;}}

    [SerializeField] int currentRot;
    public int CurrentRot {get {return currentRot;} set {currentRot = value;}}
    
    private bool finishedSegment;
    public bool FinishedSegment { get {return finishedSegment; } set { finishedSegment = value; } }

    void Start()
    {
        //shipHealth = GameData.Ships[id].shipHealth;
        //baseDamage = GameData.Ships[id].baseDamage;
        
        //temporary to avoid errors
        shipHealth = 0;
        baseDamage = 0;
        
        transform.Find("CannonSmokeRight").GetComponent<VisualEffect>().Stop();
        transform.Find("CannonSmokeLeft").GetComponent<VisualEffect>().Stop();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Cannon")
        {
            Vector3 direction = transform.position - other.gameObject.transform.position;
            CheckHitDirection(direction);
            Destroy(other.gameObject);
            Debug.Log("Pretzel wrocks!");

            if(shipHealth <= 0)
            {
                DestroyShip();
            }  
        }
    }

    private void CheckHitDirection(Vector3 givenDirection)
    {
        if (Mathf.Abs(givenDirection.x) > Mathf.Abs(givenDirection.z))  // see if the object is futher left or right or up down
        {
            if(transform.eulerAngles.y == 0)
            {
                if(givenDirection.x > 0)
                {
                    HitFromLeftAnim();
                    PlayExplosionVFX(-1);
                }
                else
                {
                    HitFromRightAnim();
                    PlayExplosionVFX(1);
                } 
            }
            else if(transform.eulerAngles.y == 180)
            {
                if(givenDirection.x > 0)
                {
                    HitFromRightAnim();
                    PlayExplosionVFX(1);
                }
                else
                {
                    HitFromLeftAnim();
                    PlayExplosionVFX(-1);
                } 
            }
        }
        else
        {
            if(transform.eulerAngles.y == 90)
            {
                if(givenDirection.z > 0)
                {
                    HitFromRightAnim();
                    PlayExplosionVFX(1);
                }
                else
                {
                    HitFromLeftAnim();
                    PlayExplosionVFX(-1);
                } 
            }

            if(transform.eulerAngles.y == 270)
            {
                if(givenDirection.z > 0)
                {
                    HitFromLeftAnim();
                    PlayExplosionVFX(-1);
                }
                else
                {
                    HitFromRightAnim();
                    PlayExplosionVFX(1);
                } 
            }
        }
    }

    private void HitFromLeftAnim()
    {
        transform.Find("Ship").GetComponent<ResetTransform>().ResetTrans();
        transform.Find("Ship").GetComponent<Animator>().SetTrigger("HitFromLeft");
    }

    private void HitFromRightAnim()
    {
        transform.Find("Ship").GetComponent<ResetTransform>().ResetTrans();
        transform.Find("Ship").GetComponent<Animator>().SetTrigger("HitFromRight");
    }

    public bool CheckDeath()
    {
        if(shipHealth <= 0)
        {
            return true;
        }
        else return false;
    }

    public void DestroyShip()
    {
        Destroy(this.gameObject);
    }

    public IEnumerator PlayShootSmokeVFX(float direction)
    {
        float waitTime = smokeVFXDelay; 

        if(direction == 1)
        {
            transform.Find("CannonSmokeRight").GetComponent<VisualEffect>().Play();
        }
        else
        {
            transform.Find("CannonSmokeLeft").GetComponent<VisualEffect>().Play();
        }
        yield return new WaitForSeconds(waitTime);

        transform.Find("CannonSmokeRight").GetComponent<VisualEffect>().Stop();
        transform.Find("CannonSmokeLeft").GetComponent<VisualEffect>().Stop();
    }

    private void PlayExplosionVFX(float direction)
    {
        if(direction == 1)
        {
            transform.Find("FireExplosionRight").GetComponent<ParticleSystem>().Play();
        }
        else
        {
            transform.Find("FireExplosionLeft").GetComponent<ParticleSystem>().Play();
        }
    }
}
