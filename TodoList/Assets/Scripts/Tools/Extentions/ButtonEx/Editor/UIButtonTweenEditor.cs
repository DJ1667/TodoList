#if UNITY_EDITOR


using System.Collections;
using System.Collections.Generic;
using UnityEditor;using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(UIButtonTween))]
public class UIButtonTweenEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("=========动画设置========");
        UIButtonTween script = (UIButtonTween)target;
        script.needGetOriginalScale = EditorGUILayout.Toggle("是否需要记录原缩放", script.needGetOriginalScale);
        
    }
}

#endif
