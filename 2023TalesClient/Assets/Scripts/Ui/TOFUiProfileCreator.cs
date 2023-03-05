using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOFUiProfileCreator : MonoBehaviour
{
    [SerializeField] private GameObject playerProfileUiPrefab;

    private void Start()
    {
        foreach (GameObject playerObject in TOFPlayer.playerShipObjects.Values)
        {
            TOFPlayer player = playerObject.GetComponent<TOFPlayer>();
            CreateProfileUI(player);
        }
    }

    void CreateProfileUI(TOFPlayer player)
    {
        //todo: instantiate new profile and place it as a child of this object.
        //todo: set values from player to the profile ui.

        GameObject playerProfile = Instantiate(playerProfileUiPrefab, transform);
        playerProfile.GetComponent<TOFUiProfile>().SetProfileUsername(TOFClient.clients[player.playerData.Id].username);
        playerProfile.GetComponent<TOFUiProfile>().SetProfileId(player.playerData.Id);
        playerProfile.GetComponent<TOFUiProfile>().SetProfileHealth();

        TOFUiProfile.UiProfiles.Add(player.playerData.Id, playerProfile.GetComponent<TOFUiProfile>());
    }
}
