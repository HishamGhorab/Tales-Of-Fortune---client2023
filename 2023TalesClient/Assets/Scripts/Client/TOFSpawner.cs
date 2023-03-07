using System.Collections;
using System.Collections.Generic;
using RiptideNetworking;
using UnityEngine;

public class TOFSpawner : MonoBehaviour
{
    [MessageHandler((ushort) ServerToClientId.spawnShop)]
    private static void SpawnShop(Message message)
    {
        Vector2 spawnPosition = message.GetVector2();
    }
}
