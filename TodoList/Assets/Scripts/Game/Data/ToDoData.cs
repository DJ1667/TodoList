using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToDoData
{
    public TaskData Data = new TaskData();
    public List<TaskData> SubTaskDataList = new List<TaskData>();
}

[System.Serializable]
public class TaskData
{
    public string Title = "";
    public bool IsFinish = false;
}
