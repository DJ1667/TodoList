using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[PanelInfo(UILayer.FullWindow, UILife.S0, "Res/Prefabs/UI/Panel/UITaskPanel")]
public class UITaskPanel : BasePanel
{
    public TextMeshProUGUI Tmp_Title;
    public TMP_InputField InputField_SubTaskName;

    public Button Btn_Finish, Btn_UnFinish;
    public Button Btn_Close, Btn_CreateSubTask;
    public Button Btn_Close_Window, Btn_Ok_Window;

    public RectTransform RectTrans_Content;
    public GameObject Obj_Window;

    private TaskData _createdData = null;
    private ToDoData _toDoData = null;

    private GameObject prefab_SubItem = null;

    private List<SubItem> _subItemList = new List<SubItem>();

    private void Awake()
    {
        Btn_Finish.onClick.AddListener(BtnOnClick_Finish);
        Btn_UnFinish.onClick.AddListener(BtnOnClick_UnFinish);
        Btn_Close.onClick.AddListener(BtnOnClick_Close);
        Btn_CreateSubTask.onClick.AddListener(BtnOnClick_CreateSubTask);
        Btn_Close_Window.onClick.AddListener(BtnOnClick_Close_Window);
        Btn_Ok_Window.onClick.AddListener(BtnOnClick_Ok_Window);

        InputField_SubTaskName.onEndEdit.AddListener(InputFieldOnEndEdit_SubTask);
    }

    protected override void OnShowFinish()
    {
        base.OnShowFinish();

        CreateItem();
        UpdateMainTask();
    }

    public void InitData(ToDoData data)
    {
        _toDoData = data;
    }

    private void CreateItem()
    {
        Tmp_Title.text = _toDoData.Data.Title;

        var curCount = _subItemList.Count;
        var trueCount = _toDoData.SubTaskDataList.Count;

        for (int i = curCount; i < trueCount; i++)
        {
            var data = _toDoData.SubTaskDataList[i];

            if (prefab_SubItem == null)
                prefab_SubItem = Resources.Load<GameObject>("Res/Prefabs/UI/Item/SubItem");

            var item = Instantiate(prefab_SubItem, RectTrans_Content, false);
            var subItem = item.GetComponent<SubItem>();
            subItem.UpdateInfo(data, false);
            _subItemList.Add(subItem);
        }

        SortItem();
    }

    public void UpdateInfo()
    {
        UpdateMainTask();

        foreach (var subItem in _subItemList)
        {
            subItem.UpdateInfo(subItem.Data, false);

            if (subItem.Data.IsFinish)
                subItem.transform.SetAsLastSibling();
        }
    }

    private void UpdateMainTask()
    {
        if (_subItemList.Count > 0)
        {
            Btn_Finish.gameObject.SetActiveEx(false);
            Btn_UnFinish.gameObject.SetActiveEx(false);
        }
        else
        {
            Btn_Finish.gameObject.SetActiveEx(!_toDoData.Data.IsFinish);
            Btn_UnFinish.gameObject.SetActiveEx(_toDoData.Data.IsFinish);
        }
    }

    private void SortItem()
    {
        foreach (var subItem in _subItemList)
        {
            if (subItem.Data.IsFinish)
                subItem.transform.SetAsLastSibling();
        }
    }

    public void DeleteSubItem(TaskData data)
    {
        _toDoData.SubTaskDataList.Remove(data);
    }

    private void BtnOnClick_Close()
    {
        UIManager.Instance.OpenPanel<MainPanel>();
        UIManager.Instance.ClosePanel<UITaskPanel>();
    }

    private void BtnOnClick_CreateSubTask()
    {
        if (_createdData == null)
            _createdData = new TaskData();

        Obj_Window.SetActiveEx(true);
    }

    private void BtnOnClick_Close_Window()
    {
        Obj_Window.SetActiveEx(false);
    }

    private void BtnOnClick_Ok_Window()
    {
        //应用一个SubItem
        if (prefab_SubItem == null)
            prefab_SubItem = Resources.Load<GameObject>("Res/Prefabs/UI/Item/SubItem");

        var item = Instantiate(prefab_SubItem, RectTrans_Content, false);
        var subItem = item.GetComponent<SubItem>();
        subItem.UpdateInfo(_createdData, false);
        _subItemList.Add(subItem);
        _toDoData.SubTaskDataList.Add(_createdData);

        SortItem();

        _createdData = null;
        Obj_Window.SetActiveEx(false);

        InputField_SubTaskName.text = "";
    }


    private void InputFieldOnEndEdit_SubTask(string info)
    {
        _createdData.Title = info;
    }

    private void BtnOnClick_Finish()
    {
        _toDoData.Data.IsFinish = true;
        UpdateInfo();
    }

    private void BtnOnClick_UnFinish()
    {
        _toDoData.Data.IsFinish = false;
        UpdateInfo();
    }
}
