using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RiptideNetworking;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class InGameHud : MonoBehaviour
{
    private static InGameHud Singleton;
    private static readonly object padlock = new object();

    public static InGameHud Instance
    {
        get
        {
            lock (padlock)
            {
                return Singleton;
            }
        }
    } 
    
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    private int roundLength = 10;
    private float timeLeftForRound = 0;
    private bool planningPhase = false;
    private bool playingPhase = false;

    private UIDocument doc;
    private VisualElement root;
    private VisualElement roundProgressBar;
    private Label roundLengthText;
    private Label roundSegmentText;

    private void Awake()
    {
        Singleton = this;
        
        doc = GetComponent<UIDocument>();
        root = doc.rootVisualElement;
        roundLengthText = root.Q<Label>("RoundLengthText");
        roundSegmentText = root.Q<Label>("RoundSegmentText");
        roundProgressBar = root.Q<VisualElement>("RoundProgressBar");

        roundProgressBar.style.width = Length.Percent(100);
        
        roundLengthText.text = roundLength.ToString();
        roundSegmentText.text = "1:0";
        
        InitInventory();
    }

    private void OnEnable()
    {
        TOFRoundHandler.onPlanningPhase += PlanningPhaseStarted;
        TOFRoundHandler.onPlayingPhase += PlayingPhaseStarted;
    }
    
    private void OnDisable()
    {
        TOFRoundHandler.onPlanningPhase -= PlanningPhaseStarted;
        TOFRoundHandler.onPlayingPhase -= PlayingPhaseStarted;
    }
    
    private void Update()
    {
        //needs fix
        if (playingPhase)
        {
            Debug.Log("w");
            roundSegmentText.text = string.Format("{0}:{1}", TOFRoundHandler.currentRound, TOFRoundHandler.Singleton.currentSegment);
        }
        
        if (planningPhase)
        {
            timeLeftForRound -= Time.deltaTime;

            timeLeftForRound = Mathf.Clamp(timeLeftForRound, 0, roundLength);
            
            roundLengthText.text = (math.round(timeLeftForRound)).ToString();
            roundProgressBar.style.width = Length.Percent((timeLeftForRound / roundLength) * 100);
        }
    }

    private void PlanningPhaseStarted()
    {
        playingPhase = false;
        
        timeLeftForRound = roundLength;
        planningPhase = true;
    }
    
    private void PlayingPhaseStarted()
    {
        planningPhase = false;
        playingPhase = true;
    }

    private void InitInventory()
    {
        VisualElement inventory = root.Q<VisualElement>("InventoryContainer");

        for (int i = 0; i < 6; i++)
        {
            InventorySlot slot = new InventorySlot();
            inventorySlots.Add(slot);
            
            inventory.Add(slot);
        }
    }
    
    [MessageHandler((ushort) ServerToClientId.sendInventoryState)]
    private static void OnInventoryChanged(Message message)
    {
        List<ItemData> inventoryItemDatas = new List<ItemData>();

        //add items to inventory
        int inventoryItemsCount = message.GetInt();

        for (int i = 0; i < inventoryItemsCount; i++)
        {
            string id = message.GetString();
            
            Sprite icon = ItemSO.itemSos[id].icon;
            Item item = new Item(id, "", "", 0, 0, icon);
            
            inventoryItemDatas.Add(new ItemData(item, 1));
        }

        //sort item locally
        //shopItemDatas = SortItems.Sort(shopItemDatas, true);
        
        //Reset the ui first
        foreach (var item in Singleton.inventorySlots)
        {
            item.DropItem();
        }
        
        foreach (ItemData item in inventoryItemDatas)
        {
            var emptySlot = Singleton.inventorySlots.FirstOrDefault(x => x.itemData == null);
            
            if (emptySlot != null)
            {
                emptySlot.HoldItem(item);
            }        
        }
    }
}
