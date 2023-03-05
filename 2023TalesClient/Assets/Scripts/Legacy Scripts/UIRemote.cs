using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIRemote : MonoBehaviour
{
    public int keyIndex = 0;

    public bool shoot = false;

    string[] keys = new string[4];

    public void Start()
    {
        keys[0] = "S";
        keys[1] = "F";
        keys[2] = "R";
        keys[3] = "L";
    }

    public void SetToDefaultKey()
    {
        keyIndex = 0;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = keys[keyIndex];

    }

    public void NextKey()
    {
        keyIndex++;

        if (keyIndex == 4)
        {
            keyIndex = 0;
        }

        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = keys[keyIndex];
    }

    public void CannonShoot()
    {
        if(GetComponent<Image>().color == Color.white)
        {
            GetComponent<Image>().color = Color.red;
            shoot = true;
        }

        else if (GetComponent<Image>().color == Color.red)
        {
            GetComponent<Image>().color = Color.white;
            shoot = false;
        }
    }
}

