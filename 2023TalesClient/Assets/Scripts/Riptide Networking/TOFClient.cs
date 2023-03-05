using System;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;


public class TOFClient
{
    public static Dictionary<ushort, TOFClient> clients = new Dictionary<ushort, TOFClient>();
    public ushort id;
    public string username;

    public bool islocal;
    
    public TOFClient(ushort id, string username)
    {
        this.id = id;
        this.username = username;
    }
    public static void AddMatchedClient(ushort id, string username)
    {
        TOFClient client = new TOFClient(id, username);
        
        if (id == TOFNetworkManager.Singleton.Client.Id) 
        {
            client.islocal = true;
        }
        else 
        {
            client.islocal = false;
        }

        TOFClient.clients.Add(id, client);
    }

    public static void RemoveMatchedClient(ushort id)
    {
        TOFPlayer.players.Remove(id);

        if(TOFNetworkManager.Singleton.Client.Id == id) 
            TOFNetworkManager.Singleton.Disconnect();
    }
    
    #region MessageHandlers
    [MessageHandler((ushort) ServerToClientId.clientMatched)]
    private static void MatchClient(Message message)
    {
        ushort id = message.GetUShort();
        string username = message.GetString();

        AddMatchedClient(id, username);
    }
    
    [MessageHandler((ushort) ServerToClientId.clientLeft)]
    private static void UnmatchClient(Message message)
    {
        ushort id = message.GetUShort();
        string username = message.GetString();

        RemoveMatchedClient(id);
    }
    #endregion
}
