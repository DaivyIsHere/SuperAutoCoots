using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateStageHelper : MonoBehaviour
{
    public GameObject helperPanel;
    public Button saveAndGoNextButton;
    
    void Start()
    {
        saveAndGoNextButton.onClick.AddListener(SaveAndGoNext);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
            helperPanel.SetActive(!helperPanel.activeSelf);
    }

    public void SaveAndGoNext()
    {
        //save
        TeamBuildManager.instance.SavePlayerTeam();
        StageManager.instance.AddToDefaultStageData();

        //next stage
        PlayerDataManager.instance.stage += 1;
        TeamBuildManager.instance.RerollShopItems();
        TeamBuildManager.instance.playerGold = 10;
        //TeamBuildManager.instance.playerGold = PlayerDataManager.instance.stage * 10 - PlayerDataManager.instance.teamData.weapons.Count;
    }
}
