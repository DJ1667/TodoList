using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[PanelInfo(UILayer.FullWindow, UILife.S0, "Res/Prefabs/UI/Panel/MainPanel")]
public class MainPanel : BasePanel
{
    public Button Btn_Create;
    public RectTransform RectTrasn_Content;
    public RectTransform RectTrans_Create;

    private GameObject prefab_ToDoItem = null;

    private Dictionary<int, ToDoItem> _toDoItemDict = new Dictionary<int, ToDoItem>();

    private void Awake()
    {
        Btn_Create.onClick.AddListener(BtnOnClick_Create);
    }

    protected override void OnShowStart(bool immediate)
    {
        base.OnShowStart(immediate);

        UpdateInfo();
    }

    public void UpdateInfo()
    {
        UpdateToDoItem();
    }

    private void UpdateToDoItem()
    {
        var curCount = _toDoItemDict.Count;
        var trueCount = LocalData.Instance.PlayerData.NowToDoDataList.Count;

        for (int i = curCount; i < trueCount; i++)
        {
            var data = LocalData.Instance.PlayerData.NowToDoDataList[i];

            if (prefab_ToDoItem == null)
                prefab_ToDoItem = Resources.Load<GameObject>("Res/Prefabs/UI/Item/ToDoItem");

            var item = Instantiate(prefab_ToDoItem, RectTrasn_Content, false);
            var todoItem = item.GetComponent<ToDoItem>();
            todoItem.UpdateInfo(data);
            if (!_toDoItemDict.ContainsKey(i))
                _toDoItemDict.Add(i, todoItem);
        }

        UpdateItemInfo();
        SortItem();
    }

    private void UpdateItemInfo()
    {
        foreach (var item in _toDoItemDict.Values)
        {
            item.UpdateInfo(item.Data);
        }
    }

    /// <summary>
    /// ∂‘TodoItem≈≈–Ú
    /// </summary>
    private void SortItem()
    {
        List<ToDoItem> finishList = new List<ToDoItem>();
        List<ToDoItem> unFinishList = new List<ToDoItem>();

        var idList = _toDoItemDict.Keys.ToList();
        for (int i = 0; i < idList.Count; i++)
        {
            var id = idList[i];
            var item = _toDoItemDict[id];

            if (item.IsFinish())
                finishList.Add(item);
            else
                unFinishList.Add(item);
        }

        foreach (var item in unFinishList)
        {
            item.transform.SetAsLastSibling();
        }
        RectTrans_Create.SetAsLastSibling();
        foreach (var item in finishList)
        {
            item.transform.SetAsLastSibling();
        }
    }

    private void BtnOnClick_Create()
    {
        UIManager.Instance.OpenPanel<UICreateTaskPanel>();
        UIManager.Instance.ClosePanel<MainPanel>();
    }
}
