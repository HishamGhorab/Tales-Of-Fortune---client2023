using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOFCameraIntro : MonoBehaviour
{
    [SerializeField] float startCamPos;
    [SerializeField] float offsettedCameraPos;

    public LeanTweenType ease;

    private void Start()
    {
        gameObject.transform.position = new Vector3(transform.position.x, offsettedCameraPos, transform.position.z);
        LeanTween.moveLocalY(gameObject, startCamPos, 3).setEase(ease);
    }
}
