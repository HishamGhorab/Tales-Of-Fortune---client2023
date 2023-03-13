using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TOFUiProfile : MonoBehaviour
{
    public static Dictionary<ushort, TOFUiProfile> UiProfiles = new Dictionary<ushort, TOFUiProfile>();

    [Header("editor")]
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI profileUsername;
    [SerializeField] private TextMeshProUGUI profileHealth;

    [SerializeField] Image healthBar;

    private ushort clientId;

    private void Start()
    {
        //profileHealth.fontSize = 0;
        //profileHealth.fontSize = 36;

        if (TOFClient.clients[clientId].islocal)
            healthBar.color = Color.green;
    }
    
    public void SetProfileId(ushort clientId)
    {
        this.clientId = clientId;
    }
    
    public void SetProfileImage(Image image)
    {
        //todo: implement logic     
    }

    public void SetProfileUsername(string username)
    {
        profileUsername.text = username; 
    }

    public void SetProfileHealth()
    {
        TOFPlayer.TOFPlayerData player = TOFPlayer.players[clientId];
        profileHealth.text = player.CurrentHealth.ToString();
        AnimateHealthBar(player.CurrentHealth, player.MaxHealth);
    }

    public void AnimateHealthBar(int currentHealth, int maxHealth)
    {
        healthBar.fillAmount = LeanTween.easeInOutSine(healthBar.fillAmount, (float)currentHealth / (float)maxHealth, 0.2f);
        //healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
    }

    public void OnClicked()
    {
        if (!Camera.main)
            return;
        
        //Camera.main.GetComponent<CameraMovement>().MoveToPlayer(id);
    }

    public void OnHealthHoverEnter()
    {
        //profileHealth.fontSize = 36;
    }

    public void OnHealthHoverExit()
    {
        //profileHealth.fontSize = 0;
    }
}
