using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TOFShip : MonoBehaviour
{
    //what do we do here...
    //[SerializeField] private VisualEffect cannonSmokeRight;
    //[SerializeField] private VisualEffect cannonSmokeLeft;

    //[SerializeField] private ParticleSystem fireExplosionRight;
    //[SerializeField] private ParticleSystem fireExplosionLeft;

    //create different types of cannons in the futures
    [SerializeField] private GameObject cannonPrefab;

    //[SerializeField] private AudioSource cannonShotSound;

    private void Start()
    {
        //fireExplosionRight.Stop();
        //fireExplosionLeft.Stop();

        //cannonSmokeRight.Stop();
        //cannonSmokeLeft.Stop();
    }

    //refactor this to be for all units
    //make this method a bit more usable :D:D:D:D:D::D:D:D:D:D:D::D
    public IEnumerator PlayCannonVFX(bool cannonRight, bool cannonLeft)
    {
        if(cannonLeft || cannonRight)
        {
            //cannonShotSound.Play();
            InstantiateCannon(cannonRight, cannonLeft, 1500);
        }

        if (cannonRight)
        {
            //fireExplosionRight.Play();
            //cannonSmokeRight.Play();
        }

        if(cannonLeft)
        {
            //fireExplosionLeft.Play();
            //cannonSmokeLeft.Play();
        }

        yield return new WaitForSeconds(0.2f);
        //fireExplosionRight.Stop();
        //fireExplosionLeft.Stop();

        //cannonSmokeRight.Stop();
        //cannonSmokeLeft.Stop();

        void InstantiateCannon(bool _cannonRight, bool _cannonLeft, float speed)
        {
            if(_cannonRight)
            {
                GameObject thisProjectile = Instantiate(cannonPrefab, 
                    new Vector3(transform.position.x, 7, transform.position.z), Quaternion.identity);
                
                thisProjectile.GetComponent<Rigidbody>().AddForce(transform.right * speed);
                Destroy(thisProjectile, 1);
            }
    
            if(_cannonLeft)
            {
                GameObject thisProjectile = Instantiate(cannonPrefab, 
                    new Vector3(transform.position.x, 7, transform.position.z), Quaternion.identity);
                
                thisProjectile.GetComponent<Rigidbody>().AddForce(-transform.right * speed);
                Destroy(thisProjectile, 1);
            }
        }
        yield return null;
    }
    
    public void AnimateShipSink(ushort client)
    {
        StartCoroutine(SinkShip(TOFTimeHandler.Singleton.SinkTime + 1));

        IEnumerator SinkShip(float time)
        {
            TOFPlayer.playerShipObjects[client].GetComponentInChildren<Animator>().SetBool("Sinking", true);

            yield return new WaitForSeconds(time);
            TOFPlayer.playerShipObjects[client].SetActive(false);
        }
    }
}
