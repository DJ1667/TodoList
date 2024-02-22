using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToDoItem : MonoBehaviour
{
    public Button Btn_Task;
    public GameObject Obj_Finish, Obj_Doing;
    public TextMeshProUGUI Tmp_Content, Tmp_Tips;

    private ToDoData _data;
    public ToDoData Data => _data;

    private void Awake()
    {
        Btn_Task.onClick.AddListener(BtnOnClick_Task);
    }

    public void UpdateInfo(ToDoData data)
    {
        _data = data;
        var isFinish = true;
        var unFinishSubTaskNum = 0;
        if (data.SubTaskDataList.Count > 0)
        {
            foreach (var subTask in data.SubTaskDataList)
            {
                if (!subTask.IsFinish)
                {
                    isFinish = false;
                    unFinishSubTaskNum++;
                }
            }
        }
        else
        {
            isFinish = data.Data.IsFinish;
        }

        Obj_Finish.SetActiveEx(isFinish);
        Obj_Doing.SetActiveEx(!isFinish);
        Tmp_Tips.gameObject.SetActiveEx(unFinishSubTaskNum > 0);
        Tmp_Content.text = data.Data.Title;
        Tmp_Tips.text = unFinishSubTaskNum + "Task";
    }

    public bool IsFinish()
    {
        var isFinish = true;
        if (_data.SubTaskDataList.Count > 0)
        {
            foreach (var subTask in _data.SubTaskDataList)
            {
                if (!subTask.IsFinish)
                {
                    isFinish = false;
                    break;
                }
            }
        }
        else
        {
            isFinish = _data.Data.IsFinish;
        }

        return isFinish;
    }

    private void BtnOnClick_Task()
    {
        UIManager.Instance.OpenPanel<UITaskPanel>((panel) =>
        {
            var p = panel as UITaskPanel;
            p.InitData(_data);
        });

        UIManager.Instance.ClosePanel<MainPanel>();
    }
}
