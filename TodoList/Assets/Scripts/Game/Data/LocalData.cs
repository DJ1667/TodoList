using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LocalData : SingletonBase<LocalData>
{
    private PlayerData playerData = null;
    public PlayerData PlayerData => playerData;

    public void LoadData()
    {
        var json_playerData = PlayerPrefs.GetString("json_playerData", "");

        try
        {
            playerData = JsonConvert.DeserializeObject<PlayerData>(json_playerData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("加载数据时出错!!!   " + e.Message);
            playerData = null;
        }

        if (playerData == null) playerData = new PlayerData();
    }

    public void SaveData()
    {

        var json_playerData = JsonConvert.SerializeObject(playerData);

        PlayerPrefs.SetString("json_playerData", json_playerData);
    }
}

[System.Serializable]
public class PlayerData
{
    public List<ToDoData> NowToDoDataList = new List<ToDoData>();  //当前的待办数据
    public List<ToDoData> ContinuousToDoDataList = new List<ToDoData>(); //持续的待办数据
}
