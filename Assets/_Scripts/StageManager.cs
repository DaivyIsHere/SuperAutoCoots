using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : PersistentSingleton<StageManager>
{
    public StageDataCollection myStageData;
    public StageDataCollection defaultStageData;

    public StageDataCollection allStageData = new StageDataCollection();

    void Start()
    {
        myStageData = StageDataFile.LoadMyData();
        defaultStageData = StageDataFile.LoadDefaultData();

        foreach (var sd in defaultStageData.stageDataList)
        {
            allStageData.AddStageData(sd);
        }
        foreach (var sd in myStageData.stageDataList)
        {
            allStageData.AddStageData(sd);
        }
    }

    public void CollectPlayerTeamData()
    {
        TeamData newTeamData = new TeamData(PlayerDataManager.instance.teamData.weapons);
        myStageData.AddTeamData(newTeamData, PlayerDataManager.instance.stage);
        StageDataFile.SaveMyData(myStageData);
    }

    public void AddToDefaultStageData()
    {
        TeamData newTeamData = new TeamData(PlayerDataManager.instance.teamData.weapons);
        defaultStageData.AddTeamData(newTeamData, PlayerDataManager.instance.stage);
        StageDataFile.SaveDefaultData(defaultStageData);
    }

    public TeamData GetRandomTeamByStage(int stage)
    {
        return myStageData.GetStageDataByStage(stage).GetRandomTeamData();
    }
}
