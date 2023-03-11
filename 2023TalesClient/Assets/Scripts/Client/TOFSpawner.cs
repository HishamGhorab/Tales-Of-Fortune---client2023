using System.Collections;
using System.Collections.Generic;
using RiptideNetworking;
using Unity.Mathematics;
using UnityEngine;

public class TOFSpawner : MonoBehaviour
{
    private static TOFSpawner _singleton;
    public static TOFSpawner Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(TOFSpawner)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private GameObject shopPrefab;
 
    private void Awake()
    {
        Singleton = this;
    }

    [MessageHandler((ushort) ServerToClientId.spawnShop)]
    private static void SpawnShop(Message message)
    {
        string id = message.GetString();
        string shopName = message.GetString();
        Vector2 spawnPosition = TOFGameView.PieceToWorldPos(message.GetVector2());
        
        GameObject shopPrefab = Singleton.shopPrefab;
        GameObject shopGO = Instantiate(shopPrefab, new Vector3(spawnPosition.x, 0, spawnPosition.y), Quaternion.identity);
        ShopInWorld shopInWorld = shopGO.AddComponent<ShopInWorld>();
        shopInWorld.ShopName = shopName;
        shopInWorld.id = id;
        ShopInWorld.Shops.Add(id, shopInWorld);
    }
}
