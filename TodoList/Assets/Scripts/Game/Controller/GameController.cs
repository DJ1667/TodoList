using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Awake()
    {
        LocalData.Instance.LoadData();

        Application.targetFrameRate = 120;
    }

    private void Start()
    {
        UIManager.Instance.OpenPanel<MainPanel>();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            LocalData.Instance.SaveData();
        }
    }
}
