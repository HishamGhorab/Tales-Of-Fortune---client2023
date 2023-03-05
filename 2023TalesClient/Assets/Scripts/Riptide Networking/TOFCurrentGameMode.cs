using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOFCurrentGameMode : MonoBehaviour
{
    private static TOFCurrentGameMode _singleton;
    public static TOFCurrentGameMode Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(TOFCurrentGameMode)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }
}
