using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[PanelInfo(UILayer.FullWindow, UILife.S0, "Res/Prefabs/UI/Panel/UICreateTaskPanel")]
public class UICreateTaskPanel : BasePanel
{
    public TMP_InputField InputField_TaskName;
    public TMP_InputField InputField_SubTaskName;

    public Button Btn_Close, Btn_Ok, Btn_CreateSubTask;
    public Button Btn_Close_Window, Btn_Ok_Window;

    public RectTransform RectTrans_Content;
    public GameObject Obj_Window;

    private TaskData _createdData = null;
    private ToDoData _toDoData = null;

    private GameObject prefab_SubItem = null;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(BtnOnClick_Close);
        Btn_Ok.onClick.AddListener(BtnOnClick_Ok);
        Btn_CreateSubTask.onClick.AddListener(BtnOnClick_CreateSubTask);
        Btn_Close_Window.onClick.AddListener(BtnOnClick_Close_Window);
        Btn_Ok_Window.onClick.AddListener(BtnOnClick_Ok_Window);

        InputField_TaskName.onEndEdit.AddListener(InputFieldOnEndEdit_Task);
        InputField_SubTaskName.onEndEdit.AddListener(InputFieldOnEndEdit_SubTask);
    }

    protected override void OnShowStart(bool immediate)
    {
        base.OnShowStart(immediate);

        _toDoData = new ToDoData();
    }

    private void BtnOnClick_Close()
    {
        UIManager.Instance.ClosePanel<UICreateTaskPanel>();
        UIManager.Instance.OpenPanel<MainPanel>();
    }

    private void BtnOnClick_Ok()
    {
        if (string.IsNullOrEmpty(_toDoData.Data.Title))
        {
            UIManager.Instance.ShowToast("必须有任务名称!");
            return;
        }

        //保存数据
        LocalData.Instance.PlayerData.NowToDoDataList.Add(_toDoData);
        LocalData.Instance.SaveData();
        UIManager.Instance.ClosePanel<UICreateTaskPanel>();

        UIManager.Instance.OpenPanel<MainPanel>();
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
        item.GetComponent<SubItem>().UpdateInfo(_createdData, true);

        _toDoData.SubTaskDataList.Add(_createdData);
        _createdData = null;
        Obj_Window.SetActiveEx(false);

        InputField_SubTaskName.text = "";
    }

    private void InputFieldOnEndEdit_Task(string info)
    {
        _toDoData.Data.Title = info;
    }

    private void InputFieldOnEndEdit_SubTask(string info)
    {
        _createdData.Title = info;
    }
}
