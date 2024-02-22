using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[PanelInfo(UILayer.Tips, UILife.S0, "Res/Prefabs/UI/Panel/UIToastPanel")]
public class UIToastPanel : BasePanel
{
    public TextMeshProUGUI Tmp_Info;
    public Animation Anim_Info;

    public void ShowToast(string info)
    {
        Tmp_Info.text = info;
        Anim_Info.Play();
    }
}
