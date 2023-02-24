using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class StageDataFile
{
    public static string directory = "/SaveData/";
    public static string myDataFileName = "MyStageData.json";
    public static string defaultDataFileName = "defaultStageData.json";

    public static void SaveMyData(StageDataCollection collection)
    {
        SaveFile(collection, myDataFileName);
    }

    public static void SaveDefaultData(StageDataCollection collection)
    {
        SaveFile(collection, defaultDataFileName);
    }

    public static StageDataCollection LoadMyData()
    {
        return LoadFile(myDataFileName);
    }

    public static StageDataCollection LoadDefaultData()
    {
        return LoadFile(defaultDataFileName);
    }

    public static void SaveFile(StageDataCollection collection, string fileName)
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(collection);
        File.WriteAllText(dir + fileName, json);
        //Debug.Log("Save " + fileName + " to " + dir);
    }

    public static StageDataCollection LoadFile(string fileName)
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        StageDataCollection collection = new StageDataCollection();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            collection = JsonUtility.FromJson<StageDataCollection>(json);
        }
        else
        {
            Debug.Log("No " + fileName + " Found");
        }

        return collection;
    }
    
    /*
        public static void SaveMyData(StageDataCollection collection)
        {
            string dir = Application.persistentDataPath + directory;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string json = JsonUtility.ToJson(collection);
            File.WriteAllText(dir + myDatafileName, json);
            Debug.Log("Save MyStageData to " + dir);
        }

        public static StageDataCollection LoadMyData()
        {
            string fullPath = Application.persistentDataPath + directory + myDatafileName;
            StageDataCollection collection = new StageDataCollection();

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                collection = JsonUtility.FromJson<StageDataCollection>(json);
            }
            else
            {
                Debug.Log("No MyStageData File Found");
            }

            return collection;
        }
    */
}

[Serializable]
public class TeamData//A team with weapons
{
    public List<TeamWeaponData> weapons = new List<TeamWeaponData>();

    public TeamData()
    {
        weapons = new List<TeamWeaponData>();
    }

    public TeamData(List<TeamWeaponData> weapons)
    {
        this.weapons = new List<TeamWeaponData>(weapons);
    }
}

[Serializable]
public class StageData//Mutiple teams with stage as tag
{
    public int stage = 1;
    public List<TeamData> allTeamData = new List<TeamData>();

    public TeamData GetRandomTeamData()
    {
        if (allTeamData.Count == 0)
        {
            Debug.LogError("NO TEAMDATA IN STAGE : " + stage);
            return null;
        }

        int choice = UnityEngine.Random.Range(0, allTeamData.Count);
        return allTeamData[choice];
    }

    public StageData(int stage)
    {
        this.stage = stage;
    }
}

[Serializable]
public class StageDataCollection//All stageData
{
    public List<StageData> stageDataList = new List<StageData>();

    public void AddStageData(StageData newStageData)
    {
        StageData stageData = GetStageDataByStage(newStageData.stage);
        if (stageData == null)
        {
            stageDataList.Add(newStageData);
        }
        else
        {
            foreach (var team in newStageData.allTeamData)
            {
                AddTeamData(team, newStageData.stage);
            }
        }
    }

    public void AddTeamData(TeamData teamData, int stage)
    {
        StageData stageData = GetStageDataByStage(stage);
        if (stageData == null)
        {
            stageData = new StageData(stage);
            stageDataList.Add(stageData);
        }

        stageData.allTeamData.Add(teamData);
    }

    public StageData GetStageDataByStage(int stage)
    {
        foreach (var sd in stageDataList)
        {
            if (sd.stage == stage)
                return sd;
        }

        //Debug.LogError("NO STAGE DATA FOR STAGE : " + stage);
        return null;
    }
}
